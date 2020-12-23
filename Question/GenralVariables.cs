using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;  
namespace Question
{
    public enum TypeOfQuestion
    {
        Slider,
        Smily,
        Stars,
        Qustions

    }
    public enum TypeOfChoice
    {
        Add,
        Edit
    }
    public class GenralVariables
    {

        public static Logger Errors = new Logger();
    }
}
