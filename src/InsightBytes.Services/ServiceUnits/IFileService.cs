﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

namespace InsightBytes.Services.ServiceUnits;
public interface IFilePickerService
{
    Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync();
    Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync();

}