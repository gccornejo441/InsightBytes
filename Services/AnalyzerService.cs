using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GetnMethods.Services;
public class AnalyzerService : IAnalyzerService
{
    public async Task<List<string>> GetAllMethodByNamesAsync(string fileName)
    {
        var fileContent = await File.ReadAllTextAsync(fileName);
        var tree = CSharpSyntaxTree.ParseText(fileContent);
        var root = tree.GetRoot();

        var methodSignatures = root.DescendantNodes()
        .OfType<MethodDeclarationSyntax>()
        .Select(method =>
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


}
