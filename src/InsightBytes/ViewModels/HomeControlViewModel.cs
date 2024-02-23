using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InsightBytes.Services.Factory;
using InsightBytes.Services.Models;
using InsightBytes.Services.ServiceUnits;
using InsightBytes.Services.Units;
using InsightBytes.Services.UnitViewModels;

using InsightBytes.Services.Utils;

using ReactiveUI;

namespace InsightBytes.ViewModels;
public class HomeControlViewModel : MainViewModelBase
{
    readonly ParserHelpers _parserHelpers;
    readonly CosmeticHelpers _cosmeticHelpers;

    private bool _showMenuAndStatusBar = true;


    public bool ShowMenuAndStatusBar
    {
        get => _showMenuAndStatusBar;
        set => this.RaiseAndSetIfChanged(ref _showMenuAndStatusBar,value);
    }

    private bool _showLoadProgress;

    public bool ShowLoadProgress
    {
        get => _showLoadProgress;
        set => this.RaiseAndSetIfChanged(ref _showLoadProgress,value);
    }

    private bool _statusBarVisible = false;

    public bool StatusBarVisible
    {
        get => _statusBarVisible;
        set => this.RaiseAndSetIfChanged(ref _statusBarVisible,value);
    }

    private int _statusBarProgressMaximum = 100;

    public int StatusBarProgressMaximum
    {
        get => _statusBarProgressMaximum;
        set => this.RaiseAndSetIfChanged(ref _statusBarProgressMaximum,value);
    }

    private int _statusBarProgressValue = 0;

    public int StatusBarProgressValue
    {
        get => _statusBarProgressValue;
        set => this.RaiseAndSetIfChanged(ref _statusBarProgressValue,value);
    }

    private StringBuilder _logBuilder = new StringBuilder();

    private string _selectedFileName;

    public string SelectedFileName
    {
        get => _selectedFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedFileName,value);
    }

    private string _selectedDirectory;

    public string SelectedDirectory
    {
        get => _selectedDirectory;
        set => this.RaiseAndSetIfChanged(ref _selectedDirectory,value);
    }

    public string LogMessages
    {
        get => _logBuilder.ToString();
        private set
        {
            _logBuilder.Append(value);
            this.RaisePropertyChanged(nameof(LogMessages));
        }
    }

    private string _downloadMessage;

    public string DownloadMessage
    {
        get => _downloadMessage;
        set => this.RaiseAndSetIfChanged(ref _downloadMessage,value);
    }

    public ICommand RunScriptCommand { get; }
    public ICommand GetFileCommand { get; }
    public ICommand ClearLogWindowCommand { get; }
    public ICommand DownloadCommand { get; }
    public ICommand SwitchContextCommand { get; }
    public ICommand SelectAllCommand { get; }
    public Interaction<Unit,Unit> SelectAllTextInteraction { get; } = new Interaction<Unit,Unit>();
    public Interaction<DownloadDialogViewModel,bool> ShowDownloadDialog { get; }

    public readonly DialogWorker dialogWorker = new DialogWorker();
    public Interaction<IDialogUnit,bool> ShowNotificationDialog { get; }
    private string _notificationMessage;
    public string NotificationMessage
    {
        get => _notificationMessage;
        set => this.RaiseAndSetIfChanged(ref _notificationMessage,value);
    }

    public HomeControlViewModel()
    {
        _cosmeticHelpers = new CosmeticHelpers();
        _parserHelpers = new ParserHelpers();

        ShowNotificationDialog = new Interaction<IDialogUnit,bool>();

        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(Run);
        ClearLogWindowCommand = ReactiveCommand.Create(Clear);
        DownloadCommand = ReactiveCommand.CreateFromTask(DownloadData);
        SelectAllCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SelectAllTextInteraction.Handle(Unit.Default);
        });


    }

    async void Clear()
    {
        uiTitles = new UiTitlesModel("Warning","Warning","Warning!","No data in log to download.");
        var result = await ShowDialog(uiTitles);

        if (!string.IsNullOrEmpty(LogMessages))
        {
            if (!result)
            {
                _logBuilder.Clear();
                this.RaisePropertyChanged(nameof(LogMessages));

                ShowLoadProgress = false;
                StatusBarVisible = false;
                StatusBarProgressValue = 0;
                StatusBarProgressMaximum = 0;
                SelectedFileName = string.Empty;
                SelectedDirectory = string.Empty;

                LogMessages = "Cleared...";
            }
            else
            {
                return;
            }

        }
        else
        {
            LogMessages = "Nothing to clear.";
        }
    }

    public UiTitlesModel uiTitles { get; set; }

    private async Task DownloadData()
    {
        try
        {
            if (_logBuilder.Length == 0)
            {
                uiTitles = new UiTitlesModel("Warning","Warning","Warning!","No data in log to download.");
                await ShowDialog(uiTitles);
                return;
            }

            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "LogData.txt");

            await File.WriteAllTextAsync(filePath,_logBuilder.ToString());

            NotificationMessage = $"Log data successfully saved to: {filePath}";
            
            uiTitles = new UiTitlesModel("Download","Download","Downloading!",NotificationMessage);
            await ShowDialog(uiTitles);
        }
        catch (Exception ex)
        {
            NotificationMessage = $"Exception occurred while saving log data: {ex.Message}";
            uiTitles = new UiTitlesModel("Warning","Warning","Warning!",NotificationMessage);
            await ShowDialog(uiTitles);


        }
    }

    private async Task<bool> ShowDialog(UiTitlesModel titles)
    {
        DialogCalled = titles.DialogType;
        IDialogUnit? dialog = dialogWorker.CreateDialog(titles);
        return await ShowNotificationDialog.Handle(dialog);
    }


    // TODO: Refactor this method to use a StringBuilder
    // TODO: Make generic to accept any type of data
    /// <summary>
    /// Used to log data to the log window supported by the <see cref="LogMessages"/> string property
    /// </summary>
    /// <param name="methodSignature"></param>
    private void LogData(MethodSignature methodSignature)
    {
        string formattedMessage = methodSignature.ToString();
        LogMessages = formattedMessage + "\n";
    }

    async Task Run()
    {
        if (string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));
            LogData(new MethodSignature(0,DateTime.Now.ToString("HH:mm:ss"),"Please select a directory to analyze methods."));
            return;
        }

        try
        {
            _logBuilder.Clear();

            this.RaisePropertyChanged(nameof(LogMessages));

            StatusBarVisible = true;

            var _analyzer = new AnalyzerService(_parserHelpers);

            var methodSignatures = await _analyzer.GetMethodSignatures(_selectedDirectory);

            StatusBarProgressMaximum = methodSignatures.Count;

            if (methodSignatures.Count == 0)
            {
                LogData(new MethodSignature(0,DateTime.Now.ToString("HH:mm:ss"),$"No methods found in the selected directory: {_selectedDirectory}"));
            }
            else
            {
                foreach (var methodSignature in methodSignatures)
                {
                    await _cosmeticHelpers.SimulateLongRunningOperationAsync();
                    LogData(methodSignature);
                    StatusBarProgressValue++;
                }
            }
        }
        catch (Exception ex)
        {
            
            uiTitles = new UiTitlesModel("Warning","Warning","Warning!","Something went wrong: " + ex.Message);

            await ShowDialog(uiTitles);
            return;

        }
    }

    private async Task SelectFileAsync()
    {
        try
        {
            ShowLoadProgress = false;
            StatusBarVisible = false;
            StatusBarProgressValue = 0;
            StatusBarProgressMaximum = 0;
            SelectedFileName = string.Empty;
            SelectedDirectory = string.Empty;

            var window = new Avalonia.Controls.Window();
            var filePickerService = new Services.ServiceUnits.FilePickerService(window.StorageProvider);

            var fileMetaData = await filePickerService.OpenFilePickerAsync();

            if (fileMetaData != null)
            {
                foreach (var folderData in fileMetaData)
                {
                    SelectedDirectory = folderData.Path.AbsolutePath;
                    SelectedFileName = folderData.Name;
                }
            }
        }
        catch (Exception)
        {
        }
    }
}