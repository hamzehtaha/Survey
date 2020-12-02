using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    /// <summary>
    ///  This Class Attributes for Variable use many times in my project
    /// </summary>

    public class Constant
    {
        public const string ErrorString = "Error";
        public const string DELETE = "Delete";
        public const string EnglishMark = "en-US";
        public const string ArabicMark = "ar-EG";
        public const string Done = "Done";
        public const string Not = "Not";
        public const string Empty = "";

    }
    public enum TypeOfChoice
    {
        Add,
        Edit
    }
    public enum TypeOfQuestion
    {
        Slider,
        Smily,
        Stars
    }
    public enum Langugaes
    {
        English,
        Arabic
    }
}
