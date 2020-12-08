using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;
using System.Windows.Forms;

namespace OperationManger
{
    public class GenralVariables
    {
        public static Logger Errors = new Logger();
        public const int Succeeded = 0;
        public const int NoData = -1;
        public const int Error = -2;
        public static DataGridView ListOfQuestion = new DataGridView();
    }
}
