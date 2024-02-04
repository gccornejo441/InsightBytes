using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

namespace GetnMethods.Services;
public class FilePickerService : IFilePickerService
{
    private readonly IStorageProvider _storageProvider;

    public FilePickerService(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync()
    {
        try
        {
            var folderPickerOptions = new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Folder",
                SuggestedStartLocation = await _storageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Pictures),
            };

            return await _storageProvider.OpenFolderPickerAsync(folderPickerOptions);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error selecting folder: {ex.Message}");
            // Optionally, handle the exception, e.g., return null or throw a custom exception
            return null;
        }
    }

    public async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync()
    {
        try
        {
            var filePickerOptions = new FilePickerOpenOptions
            {
                AllowMultiple = true,
                FileTypeFilter = new List<FilePickerFileType>
            {
                new FilePickerFileType("C# Source Files") { Patterns = new List<string> { "*.cs" } }
            },
                Title = "Select C# Files"
            };

            return await _storageProvider.OpenFilePickerAsync(filePickerOptions);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error selecting files: {ex.Message}");
            return null;
        }
    }

}