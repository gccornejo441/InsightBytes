using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;

namespace GetnMethods.Utils;

public class ParserHelpers
{
    /// <summary>
    /// Retrieves the root node of the file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>
    /// Root node
    /// </returns>
    public async Task<SyntaxNode> GetNodeFromFileAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var fileContent = await File.ReadAllTextAsync(filePath);
                var syntaxNode = await CSharpSyntaxTree.ParseText(fileContent).GetRootAsync();
                return syntaxNode;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }

        return null;
    }
}
