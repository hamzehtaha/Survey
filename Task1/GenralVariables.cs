using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog; 
namespace Survey
{
    public enum Langugaes
    {
        English,
        Arabic
    }
    public class GenralVariables
    {
        public static Logger Errors = new Logger();
        public static string Languge = "English";
        public const string ErrorString = "Error";
        public const string EnglishMark = "en-US";
        public const string ArabicMark = "ar-EG";
        public const string DELETE = "Delete";
        public const int Succeeded = 0;
        public const int NoData = -1;
        public const int Error = -2;
    }
}
