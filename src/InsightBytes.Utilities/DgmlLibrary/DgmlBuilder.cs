using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InsightBytes.Utilities.Model;

namespace InsightBytes.Utilities.DgmlLibrary;

public interface IGraphAnalysis
{
    void Execute(DirectedGraph directedGraph);
}

public class DgmlBuilder
{
    private readonly List<IGraphAnalysis> _graphAnalyses;

    public DgmlBuilder(params IGraphAnalysis[] graphAnalyses)
    {
        _graphAnalyses = graphAnalyses.ToList();
    }


    //public static string CreateDgml(List<User> users)
    //{
    //    StringBuilder dgml = new StringBuilder();
    //    dgml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    //    dgml.AppendLine("<DirectedGraph xmlns=\"http://schemas.microsoft.com/vs/2009/dgml\">");
    //    dgml.AppendLine("  <Nodes>");
    //    foreach (var user in users)
    //    {
    //        dgml.AppendLine($"    <Node Id=\"{user.ID}\" Label=\"{user.Name}\" />");
    //    }
    //    dgml.AppendLine("  </Nodes>");
    //    dgml.AppendLine("  <Links>");
    //    foreach (var user in users)
    //    {
    //        dgml.AppendLine($"    <Link Source=\"{user.ID}\" Target=\"{user.Age}\" />");
    //    }
    //    dgml.AppendLine("  </Links>");
    //    dgml.AppendLine("</DirectedGraph>");
    //    return dgml.ToString();
    //}
}
