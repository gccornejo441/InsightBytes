using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Controls;

using GetAllMethods.Services;

using ReactiveUI;

namespace GetAllMethods.ViewModels;

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

    public ICommand RunScriptCommand { get; }
    public ICommand GetFileCommand { get; }

    public MainWindowViewModel()
    {
        GetFileCommand = ReactiveCommand.CreateFromTask(SelectFileAsync);
        RunScriptCommand = ReactiveCommand.CreateFromTask(Run);
    }

    async Task Run()
    {
        _logBuilder.Clear();
        this.RaisePropertyChanged(nameof(LogMessages));

        var _analyzer = new AnalyzerService();
        List<string> methodNames = await _analyzer.GetAllMethodByNamesAsync(_selectedDirectory);

        foreach (var method in methodNames)
        {
            LogData(method);
        }
    }

    private void LogData(string message)
    {
        string formattedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";

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
