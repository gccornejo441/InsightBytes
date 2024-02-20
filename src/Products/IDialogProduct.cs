using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using GetnMethods.Products.ProductViewModels;

namespace GetnMethods.Products;
public interface IDialogProduct : IDataContextProvider
{

    void ShowDialog();
    void CloseDialog(BaseDialogViewModel baseDialogProductViewModel);

    void SetMessage(string message);
}
