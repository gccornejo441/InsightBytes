using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InsightBytes.Services.Models;

using Microsoft.CodeAnalysis;

namespace InsightBytes.Services.ServiceUnits;

public interface IAnalyzerService
{
    public Task<List<MethodSignature>> GetMethodSignatures(string fileName);

    public Task<Document> GetProjectByDocumentNameAsync(Project project, string documentName);

}
