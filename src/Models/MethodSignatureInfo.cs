using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GetnMethods.Models;
public class MethodSignature
{
    public int LineNumber { get; set; }
    public string TimeStamp { get; set; }
    public string Signature { get; set; }

    /// <summary>
    /// Class ctor
    /// </summary>
    /// <param name="lineNumber"></param>
    /// <param name="timeStamp"></param>
    /// <param name="signature"></param>
    public MethodSignature(int lineNumber, string timeStamp, string signature)
    {
        LineNumber = lineNumber;
        TimeStamp = timeStamp;
        Signature = signature;
    }

    /// <summary>
    /// Override method to print the object's info.
    /// </summary>
    /// <returns>
    /// "Line number [TimeStamp] : MethodSignature"
    /// </returns>
    public override string ToString()
    {
        return $"{LineNumber} [{TimeStamp}] : {Signature}";
    }
}
