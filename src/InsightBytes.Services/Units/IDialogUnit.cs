using Avalonia;

using InsightBytes.Services.Units.UnitViewModels;
using InsightBytes.Services.UnitViewModels;

namespace InsightBytes.Services.Units;
public interface IDialogUnit : IDataContextProvider
{
    void CloseDialog(BaseDialogViewModel baseDialogViewModel);

}
