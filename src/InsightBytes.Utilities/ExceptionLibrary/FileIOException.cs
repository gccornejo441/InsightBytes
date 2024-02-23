using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightBytes.Utilities.ExceptionLibrary;
public class FileIOException : Exception
{
    public FileIOException(string message, Exception exception) : base(message, exception)
    {

    }
}
