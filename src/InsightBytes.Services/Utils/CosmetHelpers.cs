using System.Threading.Tasks;

namespace InsightBytes.Services.Utils;
public class CosmeticHelpers
{
    public async Task SimulateLongRunningOperationAsync() { await Task.Delay(8); }

}
