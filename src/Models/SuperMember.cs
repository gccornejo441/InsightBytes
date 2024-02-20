using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetnMethods.Models;
public class SuperMember
{
    public int Id { get; set; }
    public string ProjectName { get; set; }
    public string DocumentName { get; set; }
    public string ClassName { get; set; }
    public string MemberName { get; set; }
    public List<string> MemberParameters { get; set; }
    public List<string> ReferenceLocationsByFilePath { get; set; }
    public List<string> MethodReturnTypes { get; set; }
    public bool IsObsolete { get; set; }
}

public class MethodInsight
{
    public SuperMember SuperMemberDetails { get; set; } = new SuperMember();
    public string Accessibility { get; set; }
    public string MethodSignature { get; set; }
    public string DocumentationComments { get; set; }
    public List<string> Attributes { get; set; }
    public List<string> CalledMethods { get; set; }
    public DateTime LastModified { get; set; }
    public int LinesOfCode { get; set; }
    public string TestCoverage { get; set; }
    public string DeprecationDetails { get; set; }

    public MethodInsight(SuperMember superMember)
    {
        SuperMemberDetails = superMember;
    }

}