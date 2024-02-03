using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GetAllMethods.Services;
public class AnalyzerService : IAnalyzerService
{
    public async Task<List<string>> GetAllMethodByNamesAsync(string fileName)
    {
        var fileContent = await File.ReadAllTextAsync(fileName);
        var tree = CSharpSyntaxTree.ParseText(fileContent);
        var root = tree.GetRoot();

        var methodSignatures = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Select(method =>
                    $"{method.Modifiers} {method.ReturnType} {method.Identifier.ValueText}{method.ParameterList}")
                .ToList(); 
        
        return methodSignatures;
    }


}
