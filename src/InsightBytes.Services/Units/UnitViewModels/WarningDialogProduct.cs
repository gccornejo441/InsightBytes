using Avalonia.Controls;
using Avalonia.Threading;

using InsightBytes.Services.Units;
using InsightBytes.Services.Units.UnitViewModels;

namespace InsightBytes.Services.UnitViewModels;

public class WarningDialogProduct : BaseDialog, IDialogUnit
{
   
    /// <summary>
    /// Constructor for the WarningDialogProduct
    /// </summary>
    /// <param name="dialogTitle"></param>
    /// <param name="dialogSubTitle"></param>
    public WarningDialogProduct(string windowTitle, string dialogTitle, string dialogSubTitle)
    {
        var baseDialogViewModel = new BaseDialogViewModel();

        baseDialogViewModel.WindowTitle = windowTitle;
        baseDialogViewModel.DialogTitle = dialogTitle;
        baseDialogViewModel.DialogSubTitle = dialogSubTitle;


        DataContext = baseDialogViewModel;

        CloseDialog(baseDialogViewModel);
    }

    /// <summary>
    /// Closes the dialog by invoking the RequestClose event from the BaseDialogProductViewModel
    /// </summary>
    /// <param name="baseDialogProductViewModel"></param>
    public void CloseDialog(BaseDialogViewModel baseDialogProductViewModel)
    {
        baseDialogProductViewModel.RequestClose += (dialogResult) =>
        {
            Close(dialogResult);
        };
    }
}
