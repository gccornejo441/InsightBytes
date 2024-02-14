using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;

namespace GetnMethods.Products;
public interface IDialogProduct : IDataContextProvider
{
    void ShowDialog();
    void CloseDialog();

}
