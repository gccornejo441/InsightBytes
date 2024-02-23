﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InsightBytes.Services.Models;

using Microsoft.CodeAnalysis;

namespace InsightBytes.Services.ServiceUnits;

public interface IAnalyzerService
{
    public Task<List<MethodSignature>> GetMethodSignatures(string filePath);

    public Task<List<MethodSignature>> GetMethodSignatures(string filePath,CancellationToken cancellationToken);

}