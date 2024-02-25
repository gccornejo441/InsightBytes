using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InsightBytes.Services.Models;

using Microsoft.CodeAnalysis;

namespace InsightBytes.Services.ServiceUnits;

public interface IAnalysisService
{
    public Task<List<MethodSignature>> GetMethodSignaturesAsync(string filePath);

    public Task<List<MethodSignature>> GetMethodSignaturesAsync(string filePath,CancellationToken cancellationToken);

}
