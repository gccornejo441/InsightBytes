using InsightBytes.Services.Units;
using InsightBytes.Services.UnitViewModels;

namespace InsightBytes.Services.Factory;
public class DialogWorker
{
    public IDialogUnit CreateDialog(string dialogType)
    {
        switch (dialogType)
        {
            case "Download":
                return new DownloadDialogViewModel();
            case "Warning":
                return new WarningDialogProduct();
            default:
                throw new ArgumentException("Unknown dialog type",nameof(dialogType));
        }
    }
}
