using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GetnMethods.Products;
using GetnMethods.Products.ProductViewModels;

namespace GetnMethods.Yard;
public class DialogWorker
{
    public IDialogProduct CreateDialog(string dialogType)
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
