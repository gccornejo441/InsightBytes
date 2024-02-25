using InsightBytes.Services.Models;
using InsightBytes.Services.Utils;
using InsightBytes.Utilities.ExceptionLibrary;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InsightBytes.Services.ServiceUnits;

public class AnalysisService : IAnalysisService
{
    ParserHelpers _parserHelpers;

    public AnalysisService(ParserHelpers parserHelpers) 
    { 
        _parserHelpers = parserHelpers;
    }


    /// <summary>
    /// Retrieves all methods from a specified file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of <see cref="MethodSignature"/> objects.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="FileIOException"></exception>
    public async Task<List<MethodSignature>> GetMethodSignaturesAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found",filePath);

            var root = await _parserHelpers.GetNodeFromFileAsync(filePath,cancellationToken);

            var methodSignatures = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(
                    method =>
                    {
                        var location = method.GetLocation();
                        var lineSpan = location.GetLineSpan();
                        var lineNumber = lineSpan.StartLinePosition.Line + 1;
                        var timeStamp = DateTime.Now.ToString("HH:mm:ss");
                        var signature = $"{method.Modifiers} {method.ReturnType} {method.Identifier.ValueText}{method.ParameterList}";

                        return new MethodSignature(lineNumber,timeStamp,signature);
                    })
                .ToList();

            return methodSignatures;
        }
        catch (IOException ex)
        {
            throw new FileIOException(ex.Message,ex);
        }
    }

    /// <summary>
    /// Retrieves all methods from a specified file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>List of <see cref="MethodSignature"/> objects.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="FileIOException"></exception>
    public async Task<List<MethodSignature>> GetMethodSignaturesAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found",filePath);

            var fileContent = await File.ReadAllTextAsync(filePath);

            var syntaxNode = await CSharpSyntaxTree.ParseText(fileContent).GetRootAsync();
            var methodSignatures = syntaxNode.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(
                    method =>
                    {
                        var location = method.GetLocation();
                        var lineSpan = location.GetLineSpan();
                        var lineNumber = lineSpan.StartLinePosition.Line + 1;
                        var timeStamp = DateTime.Now.ToString("HH:mm:ss");
                        var signature = $"{method.Modifiers} {method.ReturnType} {method.Identifier.ValueText}{method.ParameterList}";

                        return new MethodSignature(lineNumber,timeStamp,signature);
                    })
                .ToList();

            return methodSignatures;
        }
        catch (IOException ex)
        {
            throw new FileIOException(ex.Message,ex);
        }
    }
}
