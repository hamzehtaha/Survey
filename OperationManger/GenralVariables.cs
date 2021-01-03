using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;

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

        public const int TextIsEmpty = 405;
        public const int TextIsNumber = 406;
        public const int OrderLessThanZero = 407;
        public const int StartValueLessThanZero = 408;
        public const int StartValueGreaterThanOneHundered = 409;
        public const int EndValueGreaterThanOneHundered = 410;
        public const int EndValueLessThanZero = 411;
        public const int StartValueGreaterThanEndValue = 412;
        public const int StartCaptionJustNumbers = 413;
        public const int EndCaptionJustNumbers = 414;
        public const int EndCaptionIsEmtpty = 415;
        public const int StartCaptionIsEmtpty = 416;
        public const int NumberOfSmileInvalid = 417;
        public const int NumberOfStarsInvalid = 418;




    }
}
