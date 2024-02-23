using InsightBytes.Services.Models;
using InsightBytes.Services.Units;
using InsightBytes.Services.UnitViewModels;

namespace InsightBytes.Services.Factory;
public class DialogWorker
{
    public IDialogUnit CreateDialog(UiTitlesModel uiTitles)
    {
        switch (uiTitles.DialogType)
        {
            case "Download":
                return new DownloadDialogViewModel(uiTitles.WindowTitle,uiTitles.DialogTitle,uiTitles.DialogSubTitle);
            case "Warning":
                return new WarningDialogProduct(uiTitles.WindowTitle, uiTitles.DialogTitle, uiTitles.DialogSubTitle);
            default:
                throw new ArgumentException("Unknown dialog type",nameof(uiTitles.DialogType));
        }
    }
}
