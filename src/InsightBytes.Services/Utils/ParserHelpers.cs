
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace InsightBytes.Services.Utils;

public class ParserHelpers
{
    /// <summary>
    /// Retrieves the root node of the file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>
    /// Root node
    /// </returns>
    public async Task<SyntaxNode> GetNodeFromFileAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var fileContent = await File.ReadAllTextAsync(filePath, cancellationToken);
                var syntaxNode = await CSharpSyntaxTree.ParseText(fileContent).GetRootAsync();
                return syntaxNode;
            }
            else
            {
                throw new FileNotFoundException("File not found", filePath);
            }
        }
        catch (Exception ex)
        {
        }

        return null;
    }
}
