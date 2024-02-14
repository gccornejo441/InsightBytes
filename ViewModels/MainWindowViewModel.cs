
using Avalonia.Controls;
using Avalonia.Threading;

using DialogHostAvalonia;

using GetnMethods.Models;
using GetnMethods.Products;
using GetnMethods.Services;
using GetnMethods.Utils;
using GetnMethods.Yard;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GetnMethods.ViewModels;

public class MainWindowViewModel : ViewModelBase
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

    public ICommand RunScriptCommand { get; }
    public ICommand GetFileCommand { get; }
    public ICommand ClearLogWindowCommand { get; }
    public ICommand DownloadCommand { get; }
    public ICommand SwitchContextCommand { get; }
    public ICommand SelectAllCommand { get; }
    public Interaction<Unit,Unit> SelectAllTextInteraction { get; } = new Interaction<Unit,Unit>();
    public Interaction<DownloadDialogViewModel,bool> ShowDownloadDialog { get; }

    public readonly DialogWorker dialogWorker = new DialogWorker();
    public Interaction<IDialogProduct, bool> ShowWarningDialog { get; }

    public MainWindowViewModel()
    {
        _cosmeticHelpers = new CosmeticHelpers();
        _parserHelpers = new ParserHelpers();

        ShowWarningDialog = new Interaction<IDialogProduct, bool>();
        ShowDownloadDialog = new Interaction<DownloadDialogViewModel, bool>();

        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(Run);
        ClearLogWindowCommand = ReactiveCommand.Create(Clear);
        DownloadCommand = ReactiveCommand.CreateFromTask(DownloadData);
        SwitchContextCommand = ReactiveCommand.Create(SwitchContext);
        SelectAllCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SelectAllTextInteraction.Handle(Unit.Default);
        });


    }

    async void Clear()
    {
        IDialogProduct? warningDialog = dialogWorker.CreateDialog("Warning");
        var result = await ShowWarningDialog.Handle(warningDialog);
  

        if (!string.IsNullOrEmpty(LogMessages))
        {
            if (result)
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

    private async Task DownloadData()
    {
        var downloadVMDialog = new DownloadDialogViewModel();
        var result = await ShowDownloadDialog.Handle(downloadVMDialog);

        try
        {
            if (_logBuilder.Length == 0)
            {
                DownloadMessage = "No data in log to download.";

                await DialogHost.Show(DownloadMessage);
                return;
            }

            // Specify the path where the log file will be saved
            // Consider asking the user for a location or using a common location
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "LogData.txt");

            await File.WriteAllTextAsync(filePath,_logBuilder.ToString());

            DownloadMessage = $"Log data successfully saved to: {filePath}";
            await DialogHost.Show(DownloadMessage);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception occurred while saving log data: {ex.Message}");
        }
    }

    void SwitchContext()
    {

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
