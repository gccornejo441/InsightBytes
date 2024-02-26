
using FluentAvalonia.UI.Controls;

using InsightBytes.Services.Factory;
using InsightBytes.Services.Models;
using InsightBytes.Services.ServiceUnits;
using InsightBytes.Services.Units;
using InsightBytes.Services.UnitViewModels;
using InsightBytes.Services.Utils;

using ReactiveUI;

using System;

using System.IO;

using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

using System.Threading.Tasks;

using System.Windows.Input;

namespace InsightBytes.ViewModels;

public class HomeControlViewModel : MainViewModelBase
{

    readonly CosmeticHelpers _cosmeticHelpers;
    CancellationTokenSource _cts;

    private string _downloadMessage;

    bool _isClearVisible = false;

    bool _isDownloadVisible = false;

    bool _isPauseVisible = false;

    bool _isPlayVisible = true;

    bool _isRunning = false;
    bool _isStopVisible = false;

    private StringBuilder _logBuilder = new StringBuilder();
    private string _notificationMessage;
    readonly ParserHelpers _parserHelpers;

    private string _selectedDirectory;

    private string _selectedFileName;

    private bool _showLoadProgress;

    private bool _showMenuAndStatusBar = true;

    private int _statusBarProgressMaximum = 100;

    private int _statusBarProgressValue = 0;

    private bool _statusBarVisible = false;


    int _symbolPlayPause = (int)Symbol.PauseFilled;

    ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);

    public readonly DialogWorker dialogWorker = new DialogWorker();

    public HomeControlViewModel()
    {
        _cts = new CancellationTokenSource();
        _cosmeticHelpers = new CosmeticHelpers();
        _parserHelpers = new ParserHelpers();
        ShowNotificationDialog = new Interaction<IDialogUnit,bool>();

        StopScriptCommand = ReactiveCommand.Create(_cts.Cancel);
        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(RunAsync);
        TogglePauseCommand = ReactiveCommand.Create(TogglePause);
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
        IsClearVisible = false;
    }

    async Task DownloadData()
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

    private void ResetUIState()
    {
        IsRunning = false;
        IsPlayVisible = true;
        IsPauseVisible = false;
        IsClearVisible = true;
        IsStopVisible = false;
        StatusBarVisible = false;
        StatusBarProgressValue = 0;
        IsDownloadVisible = true;
    }

    async Task RunAsync()
    {
        SetUIStateRunning();

        if (string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));
            LogData(
                new MethodSignature(
                    0,
                    DateTime.Now.ToString("HH:mm:ss"),
                    "Please select a directory to analyze methods."));
            ResetUIState();
        }

        try
        {
            _logBuilder.Clear();
            StatusBarVisible = true;

            var _analyzer = new AnalysisService(_parserHelpers);

            var methodSignatures = await _analyzer.GetMethodSignaturesAsync(_selectedDirectory);

            StatusBarProgressMaximum = methodSignatures.Count;

            if (methodSignatures.Count == 0)
            {
                LogData(
                    new MethodSignature(
                        0,
                        DateTime.Now.ToString("HH:mm:ss"),
                        $"No methods found in the selected directory: {_selectedDirectory}"));
            }
            else
            {
                foreach (var methodSignature in methodSignatures)
                {
                    try
                    {
                        await Task.Run(() => pauseEvent.Wait());

                        if (_cts.Token.IsCancellationRequested)
                            break;

                        await _cosmeticHelpers.SimulateLongRunningOperationAsync();
                        LogData(methodSignature);
                        StatusBarProgressValue++;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            uiTitles = new UiTitlesModel("Warning","Warning","Warning!","Something went wrong: " + ex.Message);
            await ShowDialog(uiTitles);
        }
        finally
        {
            ResetUIState();

            _cts.Dispose();
            _cts = new CancellationTokenSource();
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

    private void SetUIStateRunning()
    {
        IsRunning = true;
        IsPlayVisible = false;
        IsPauseVisible = true;
        IsClearVisible = false;
        IsStopVisible = true;
        StatusBarVisible = false;
        IsDownloadVisible = false;
    }

    private async Task<bool> ShowDialog(UiTitlesModel titles)
    {
        DialogCalled = titles.DialogType;
        IDialogUnit? dialog = dialogWorker.CreateDialog(titles);
        return await ShowNotificationDialog.Handle(dialog);
    }

    private void TogglePause()
    {
        if (IsRunning)
        {
            IsPauseVisible = true;
            IsStopVisible = false;
            this.RaisePropertyChanged(nameof(IsStopVisible));
            pauseEvent.Reset();
            ChangeContext();
        }
        else
        {
            IsStopVisible = true;
            pauseEvent.Set();
            ChangeContext();
        }
        IsRunning = !IsRunning;
    }

    public void ChangeContext()
    {
        if (IsRunning)
        {
            SymbolPlayPause = (int)Symbol.Play;
        }
        else
        {
            SymbolPlayPause = (int)Symbol.PauseFilled;
        }
    }


    public ICommand ClearLogWindowCommand { get; }

    public ICommand DownloadCommand { get; }

    public string DownloadMessage
    {
        get => _downloadMessage;
        set => this.RaiseAndSetIfChanged(ref _downloadMessage,value);
    }

    public ICommand GetFileCommand { get; }

    public bool IsClearVisible { get => _isClearVisible; set => this.RaiseAndSetIfChanged(ref _isClearVisible,value); }

    public bool IsDownloadVisible
    {
        get => _isDownloadVisible;
        set => this.RaiseAndSetIfChanged(ref _isDownloadVisible,value);
    }

    public bool IsPauseVisible { get => _isPauseVisible; set => this.RaiseAndSetIfChanged(ref _isPauseVisible,value); }

    public bool IsPlayVisible { get => _isPlayVisible; set => this.RaiseAndSetIfChanged(ref _isPlayVisible,value); }

    public bool IsRunning { get => _isRunning; set => this.RaiseAndSetIfChanged(ref _isRunning,value); }

    public bool IsStopVisible { get => _isStopVisible; set => this.RaiseAndSetIfChanged(ref _isStopVisible,value); }

    public string LogMessages
    {
        get => _logBuilder.ToString();
        private set
        {
            _logBuilder.Append(value);
            this.RaisePropertyChanged(nameof(LogMessages));
        }
    }

    public string NotificationMessage
    {
        get => _notificationMessage;
        set => this.RaiseAndSetIfChanged(ref _notificationMessage,value);
    }

    public ICommand RunScriptCommand { get; }

    public ICommand SelectAllCommand { get; }

    public Interaction<Unit,Unit> SelectAllTextInteraction { get; } = new Interaction<Unit,Unit>();

    public string SelectedDirectory
    {
        get => _selectedDirectory;
        set => this.RaiseAndSetIfChanged(ref _selectedDirectory,value);
    }

    public string SelectedFileName
    {
        get => _selectedFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedFileName,value);
    }

    public Interaction<DownloadDialogViewModel,bool> ShowDownloadDialog { get; }

    public bool ShowLoadProgress
    {
        get => _showLoadProgress;
        set => this.RaiseAndSetIfChanged(ref _showLoadProgress,value);
    }


    public bool ShowMenuAndStatusBar
    {
        get => _showMenuAndStatusBar;
        set => this.RaiseAndSetIfChanged(ref _showMenuAndStatusBar,value);
    }

    public Interaction<IDialogUnit,bool> ShowNotificationDialog { get; }

    public int StatusBarProgressMaximum
    {
        get => _statusBarProgressMaximum;
        set => this.RaiseAndSetIfChanged(ref _statusBarProgressMaximum,value);
    }

    public int StatusBarProgressValue
    {
        get => _statusBarProgressValue;
        set => this.RaiseAndSetIfChanged(ref _statusBarProgressValue,value);
    }

    public bool StatusBarVisible
    {
        get => _statusBarVisible;
        set => this.RaiseAndSetIfChanged(ref _statusBarVisible,value);
    }

    public ICommand StopScriptCommand { get; }

    public ICommand SwitchContextCommand { get; }

    public int SymbolPlayPause
    {
        get => _symbolPlayPause;
        set => this.RaiseAndSetIfChanged(ref _symbolPlayPause,value);
    }

    public ICommand TogglePauseCommand { get; }

    public UiTitlesModel uiTitles { get; set; }
}