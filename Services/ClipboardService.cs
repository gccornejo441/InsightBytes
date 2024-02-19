using System.Threading.Tasks;

using Avalonia.Controls;

namespace GetnMethods.Services;

public static class ClipboardService
{
    public static TopLevel Owner { get; set; }

    public static Task SetTextAsync(string text) =>
        Owner.Clipboard.SetTextAsync(text);
}
