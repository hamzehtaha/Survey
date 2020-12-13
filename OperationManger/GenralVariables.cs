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
        public const int ErrorInManger = 400;
        public const int ErrorInMangerAdd = 401;
        public const int ErrorInMangerEdit = 402;
        public const int ErrorInMangerDelete = 403;
        public const int ErrorInMangerGetQuestion = 404;
    }
}
