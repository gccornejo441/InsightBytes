using GetnMethods.Models;
using GetnMethods.Utils;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetnMethods.Services;

public class AnalyzerService : IAnalyzerService
{
    ParserHelpers _parserHelpers;

    public AnalyzerService(ParserHelpers parserHelpers) 
    { 
        _parserHelpers = parserHelpers; 
    }

    /// <summary>
    /// Create a syntax tree from the source code.
    /// </summary>
    /// <param name="sourceCode"></param>
    /// <param name="parseOptions"></param>
    /// <param name="filePath"></param>
    /// <param name="encoding"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    static SyntaxTree CreateSyntaxTree(
        string sourceCode,
        CSharpParseOptions parseOptions = null,
        string filePath = "",
        Encoding encoding = null,
        CancellationToken cancellationToken = default)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
            text: sourceCode,
            options: parseOptions ?? CSharpParseOptions.Default,
            path: filePath,
            encoding: encoding ?? Encoding.UTF8,
            cancellationToken: cancellationToken);

        return syntaxTree;
    }

    public Task<List<SuperMember>> GetBaseMethodDataByProjectName(Project project, string projectName)
    { throw new NotImplementedException(); }



    /// <summary>
    /// Retrieves all methods 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>
    /// Returns a list of <see cref="MethodSignature"/> objects.
    /// </returns>
    public async Task<List<MethodSignature>> GetMethodSignatures(string filePath)
    {
        var root = await _parserHelpers.GetNodeFromFileAsync(filePath);
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

                    return new MethodSignature(lineNumber, timeStamp, signature);
                })
            .ToList();

        return methodSignatures;
    }

    public Task<Document> GetProjectByDocumentNameAsync(Project project, string documentName)
    { throw new NotImplementedException(); }
}
