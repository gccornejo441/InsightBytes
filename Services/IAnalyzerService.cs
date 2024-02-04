using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetnMethods.Services;
public interface IAnalyzerService
{
    public Task<List<string>> GetAllMethodByNamesAsync(string fileName);
}
