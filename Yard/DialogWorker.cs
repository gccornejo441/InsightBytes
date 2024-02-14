using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GetnMethods.Products;

namespace GetnMethods.Yard;
public class DialogWorker
{
    public IDialogProduct CreateDialog(string dialogType)
    {
        switch (dialogType)
        {
            case "Download":
                return new DownloadDialogProduct();
            case "Warning":
                return new WarningDialogProduct();
            default:
                throw new ArgumentException("Unknown dialog type",nameof(dialogType));
        }
    }
}
