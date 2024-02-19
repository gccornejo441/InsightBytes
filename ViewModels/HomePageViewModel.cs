using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using GetnMethods.Products.ProductViewModels;

using GetnMethods.Products;

using GetnMethods.Services;

using GetnMethods.Utils;

using GetnMethods.Yard;

using ReactiveUI;
using System.Reactive.Linq;
using System.IO;
using GetnMethods.Models;

namespace GetnMethods.ViewModels;

public class HomePageViewModel : ViewModelBase
{
    ParserHelpers _parserHelpers;
    CosmeticHelpers _cosmeticHelpers;

    private bool _showMenuAndStatusBar = true;


public bool ShowMenuAndStatusBar
{
    get => _showMenuAndStatusBar;
    set => this.RaiseAndSetIfChanged(ref _showMenuAndStatusBar, value);
}

    private bool _showLoadProgress;

    public bool ShowLoadProgress
    {
        get => _showLoadProgress;
        set => this.RaiseAndSetIfChanged(ref _showLoadProgress, value);
    }

    private bool _statusBarVisible = false;

    public bool StatusBarVisible
    {
        get => _statusBarVisible;
        set => this.RaiseAndSetIfChanged(ref _statusBarVisible, value);
    }

    private int _statusBarProgressMaximum = 100;

    public int StatusBarProgressMaximum
    {
        get => _statusBarProgressMaximum;
        set => this.RaiseAndSetIfChanged(ref _statusBarProgressMaximum, value);
    }

    private int _statusBarProgressValue = 0;

    public int StatusBarProgressValue
    {
        get => _statusBarProgressValue;
        set => this.RaiseAndSetIfChanged(ref _statusBarProgressValue, value);
    }

    private StringBuilder _logBuilder = new StringBuilder();

    private string _selectedFileName;

    public string SelectedFileName
    {
        get => _selectedFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedFileName, value);
    }

    private string _selectedDirectory;

    public string SelectedDirectory
    {
        get => _selectedDirectory;
        set => this.RaiseAndSetIfChanged(ref _selectedDirectory, value);
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
        set => this.RaiseAndSetIfChanged(ref _downloadMessage, value);
    }

    private string _dialogCalled;

    public string DialogCalled { get => _dialogCalled; set => this.RaiseAndSetIfChanged(ref _dialogCalled, value); }

    public ICommand RunScriptCommand { get; }

    public ICommand GetFileCommand { get; }

    public ICommand ClearLogWindowCommand { get; }

    public ICommand DownloadCommand { get; }

    public ICommand SwitchContextCommand { get; }

    public ICommand SelectAllCommand { get; }

    public Interaction<Unit, Unit> SelectAllTextInteraction { get; } = new Interaction<Unit, Unit>();

    public Interaction<DownloadDialogViewModel, bool> ShowDownloadDialog { get; }

    public readonly DialogWorker dialogWorker = new DialogWorker();

    public Interaction<IDialogProduct, bool> ShowNotificationDialog { get; }

    private string _notificationMessage;

    public string NotificationMessage
    {
        get => _notificationMessage;
        set => this.RaiseAndSetIfChanged(ref _notificationMessage, value);
    }

    public HomePageViewModel()
    {
        _cosmeticHelpers = new CosmeticHelpers();
        _parserHelpers = new ParserHelpers();

        ShowNotificationDialog = new Interaction<IDialogProduct, bool>();

        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(Run);
        ClearLogWindowCommand = ReactiveCommand.Create(Clear);
        DownloadCommand = ReactiveCommand.CreateFromTask(DownloadData);
        SelectAllCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await SelectAllTextInteraction.Handle(Unit.Default);
            });
    }

    async void Clear()
    {
        DialogCalled = "Warning";
        IDialogProduct? warningDialog = dialogWorker.CreateDialog("Warning");
        var result = await ShowNotificationDialog.Handle(warningDialog);


        if(!string.IsNullOrEmpty(LogMessages))
        {
            if(!result)
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
            } else
            {
                return;
            }
        } else
        {
            LogMessages = "Nothing to clear.";
        }
    }

    public class UiTitlesModel
    {
        public string DialogType { get; set; }

        public string WindowTitle { get; set; }

        public string DialogTitle { get; set; }

        public string DialogSubTitle { get; set; }
    }


private async Task DownloadData()
{
    try
    {
        if(_logBuilder.Length == 0)
        {
            UiTitlesModel uiTitlesModel = new UiTitlesModel
            {
                DialogType = "Warning",
                WindowTitle = "Warning",
                DialogTitle = "Warning!",
                DialogSubTitle = "No data in log to download."
            };

            await ShowDialog(uiTitlesModel);
            return;
        }
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogData.txt");

        await File.WriteAllTextAsync(filePath, _logBuilder.ToString());

        NotificationMessage = $"Log data successfully saved to: {filePath}";
        IDialogProduct? downloadDialog = dialogWorker.CreateDialog("Download");
        await ShowNotificationDialog.Handle(downloadDialog);
    } catch(Exception ex)
    {
        NotificationMessage = $"Exception occurred while saving log data: {ex.Message}";
        IDialogProduct? warningDialog = dialogWorker.CreateDialog("Warning");
        await ShowNotificationDialog.Handle(warningDialog);
    }
}

    private async Task ShowDialog(UiTitlesModel titles)
    {
        DialogCalled = titles.DialogType;
        IDialogProduct? dialog = dialogWorker.CreateDialog(titles.DialogType);
        if(dialog != null)
        {
            await ShowNotificationDialog.Handle(dialog);
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

    async Task Run()
    {
        if(string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));
            LogData(
                new MethodSignature(
                    0,
                    DateTime.Now.ToString("HH:mm:ss"),
                    "Please select a directory to analyze methods."));
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

            if(methodSignatures.Count == 0)
            {
                LogData(
                    new MethodSignature(
                        0,
                        DateTime.Now.ToString("HH:mm:ss"),
                        $"No methods found in the selected directory: {_selectedDirectory}"));
            } else
            {
                foreach(var methodSignature in methodSignatures)
                {
                    await _cosmeticHelpers.SimulateLongRunningOperationAsync();
                    LogData(methodSignature);
                    StatusBarProgressValue++;
                }
            }
        } catch(Exception ex)
        {
            Debug.WriteLine($"Exception occurred while analyzing methods: {ex.Message}");
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
            var filePickerService = new FilePickerService(window.StorageProvider);

            var fileMetaData = await filePickerService.OpenFilePickerAsync();

            if(fileMetaData != null)
            {
                foreach(var folderData in fileMetaData)
                {
                    SelectedDirectory = folderData.Path.AbsolutePath;
                    SelectedFileName = folderData.Name;
                }
            }
        } catch(Exception ex)
        {
        }
    }
}
