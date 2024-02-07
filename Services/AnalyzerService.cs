using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetnMethods.Services;

public class AnalyzerService : IAnalyzerService
{
    public async Task<List<string>> GetAllMethodByNamesAsync(string fileName)
    {
        var fileContent = await File.ReadAllTextAsync(fileName);
        var tree = CSharpSyntaxTree.ParseText(fileContent);
        var root = tree.GetRoot();

        var syntaxTree = CreateSyntaxTree(fileContent);

        var methodSignatures = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .Select(
                method =>
                {
                    var location = method.GetLocation();
                    var lineSpan = location.GetLineSpan();
                    var lineNumber = lineSpan.StartLinePosition.Line + 1;

                    var signature = $"{method.Modifiers} {method.ReturnType} {method.Identifier.ValueText}{method.ParameterList}";

                    return $"{lineNumber} [{DateTime.Now:HH:mm:ss}] : {signature}";
                })
            .ToList();

        return methodSignatures;
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
            encoding: encoding ?? Encoding.UTF8, // Default to UTF-8 if no encoding is specified.
            cancellationToken: cancellationToken);

        return syntaxTree;
    }
}
