using System.Threading.Tasks;

namespace GetnMethods.Utils;
public class CosmeticHelpers
{
    public async Task SimulateLongRunningOperationAsync() { await Task.Delay(8); }

}
