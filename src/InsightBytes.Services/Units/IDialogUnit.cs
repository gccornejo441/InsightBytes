using Avalonia;

using InsightBytes.Services.Units.UnitViewModels;
using InsightBytes.Services.UnitViewModels;

namespace InsightBytes.Services.Units;
public interface IDialogUnit : IDataContextProvider
{
    void ShowDialog();
    void CloseDialog(BaseDialogViewModel baseDialogViewModel);

    void SetMessage(string message);
}
