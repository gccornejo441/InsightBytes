using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightBytes.Services.Models;
public class UiTitlesModel
{
    public string DialogType { get; set; }
    public string WindowTitle { get; set; }
    public string DialogTitle { get; set; }
    public string DialogSubTitle { get; set; }

    public UiTitlesModel(string dialogType, string windowTitle, string dialogTitle, string dialogSubTitle)
    {
        DialogType = dialogType;
        WindowTitle = windowTitle;
        DialogTitle = dialogTitle;
        DialogSubTitle = dialogSubTitle;
    }

    public void Deconstruct(out string dialogType, out string windowTitle, out string dialogTitle, out string dialogSubTitle)
    {
        dialogType = DialogType;
        windowTitle = WindowTitle;
        dialogTitle = DialogTitle;
        dialogSubTitle = DialogSubTitle;
    }

}
