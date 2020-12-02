using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey
{
    /// <summary>
    /// This Abstract Class For Logger 
    /// </summary>
    public abstract class  AbstractLog
    {
        public abstract void Log(Exception ex);
    }
}
