using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Controls;

using DialogHostAvalonia;

using GetnMethods.Services;

using ReactiveUI;

namespace GetnMethods.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private StringBuilder _logBuilder = new StringBuilder();

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
    public MainWindowViewModel()
    {
        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(Run);
        ClearLogWindowCommand = ReactiveCommand.Create(Clear);
        DownloadCommand = ReactiveCommand.CreateFromTask(DownloadData);
    }

    void Clear()
    {
        if (!string.IsNullOrEmpty(LogMessages))
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));

            LogMessages = "Cleared...";
        } 
        else
        {
            LogMessages = "Nothing to clear.";
        }
    }

    async Task Run()
    {
        if (string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            _logBuilder.Clear();
            this.RaisePropertyChanged(nameof(LogMessages));
            LogData("Please select a directory to analyze methods.");
            return; 
        }

        _logBuilder.Clear();
        this.RaisePropertyChanged(nameof(LogMessages));

        var _analyzer = new AnalyzerService();
        List<string> methodNames = await _analyzer.GetAllMethodByNamesAsync(_selectedDirectory);

        if (methodNames.Count == 0)
        {
            LogData($"No methods found in the selected directory: {_selectedDirectory}");
        }
        else
        {
            foreach (var signatureWithLineNumber in methodNames)
            {
                LogData(signatureWithLineNumber);
            }
        }
    }

    private async Task DownloadData()
    {
        try
        {
            if (_logBuilder.Length == 0)
            {
                DownloadMessage = "No data in log to download.";

                DialogHost.Show(DownloadMessage);
                return;
            }

            // Specify the path where the log file will be saved
            // Consider asking the user for a location or using a common location
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"LogData.txt");

            await File.WriteAllTextAsync(filePath,_logBuilder.ToString());

            DownloadMessage = $"Log data successfully saved to: {filePath}";
            DialogHost.Show(DownloadMessage);

        }
        catch (Exception ex)
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
            var window = new Window();
            var filePickerService = new FilePickerService(window.StorageProvider);

            var fileMetaData = await filePickerService.OpenFilePickerAsync();

            if (fileMetaData != null)
            {
                foreach (var folderData in fileMetaData)
                {
                    SelectedDirectory = folderData.Path.AbsolutePath;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
