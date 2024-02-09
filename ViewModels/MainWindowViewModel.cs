
using Avalonia.Controls;

using DialogHostAvalonia;

using GetnMethods.Services;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.IO;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Input;

namespace GetnMethods.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
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

    public MainWindowViewModel()
    {
        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(Run);
        ClearLogWindowCommand = ReactiveCommand.Create(Clear);
        DownloadCommand = ReactiveCommand.CreateFromTask(DownloadData);
        SwitchContextCommand = ReactiveCommand.Create(SwitchContext);
    }

    private void SwitchContext()
    {

    }

    void Clear()
    {
        if(!string.IsNullOrEmpty(LogMessages))
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
            LogMessages = "Nothing to clear.";
        }
    }

    async Task Run()
    {
        if(string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));
            LogData("Please select a directory to analyze methods.");
            return;
        }

        try
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));

            StatusBarVisible = true;

            var _analyzer = new AnalyzerService();

            List<string> methodNames = await _analyzer.GetAllMethodByNamesAsync(_selectedDirectory);

            StatusBarProgressMaximum = methodNames.Count;

            if(methodNames.Count == 0)
            {
                LogData($"No methods found in the selected directory: {_selectedDirectory}");
            } else
            {
                if(methodNames.Count >= 150)
                {
                    foreach(var signatureWithLineNumber in methodNames)
                    {
                        await SimulateLongRunningOperationAsync();
                        LogData(signatureWithLineNumber);
                        StatusBarProgressValue++;
                    }
                } else
                {
                    foreach(var signatureWithLineNumber in methodNames)
                    {
                        await SimulateLongRunningOperationAsync();
                        LogData(signatureWithLineNumber);
                        StatusBarProgressValue++;
                    }
                }
            }
        } catch(Exception ex)
        {
            Debug.WriteLine($"Exception occurred while analyzing methods: {ex.Message}");
        }
    }

    public async Task SimulateLongRunningOperationAsync() { await Task.Delay(16); }

    private async Task DownloadData()
    {
        try
        {
            if(_logBuilder.Length == 0)
            {
                DownloadMessage = "No data in log to download.";

                DialogHost.Show(DownloadMessage);
                return;
            }

            // Specify the path where the log file will be saved
            // Consider asking the user for a location or using a common location
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "LogData.txt");

            await File.WriteAllTextAsync(filePath, _logBuilder.ToString());

            DownloadMessage = $"Log data successfully saved to: {filePath}";
            DialogHost.Show(DownloadMessage);
        } catch(Exception ex)
        {
            Debug.WriteLine($"Exception occurred while saving log data: {ex.Message}");
        }
    }



    private void LogData(string message)
    {
        string formattedMessage = $"{message}";

        LogMessages = formattedMessage + "\n";
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

            var window = new Window();
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
