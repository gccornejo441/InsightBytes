using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GetnMethods.Models;

using Microsoft.CodeAnalysis;

namespace GetnMethods.Services;

public interface IAnalyzerService
{
    public Task<List<MethodSignature>> GetMethodSignatures(string fileName);

    public Task<Document> GetProjectByDocumentNameAsync(Project project, string documentName);

    public Task<List<SuperMember>> GetBaseMethodDataByProjectName(Project project, string projectName);
}
