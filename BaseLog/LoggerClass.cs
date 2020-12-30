using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BaseLog
{
    /// <summary>
    /// This Class For print errors in log file 
    /// </summary>
    public  class Logger
    {
        private string CurrentDirectory
        {
            get;
            set;
        }
        private string FileName
        {
            get;
            set;
        }
        private string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// In constructor the name and the location of file  
        /// </summary>
        public Logger()
        {
            try
            {
                this.CurrentDirectory = Directory.GetCurrentDirectory();
                Console.WriteLine(Directory.GetCurrentDirectory()); 
                this.FileName = "Log.txt";
                this.FilePath = "D:\\Log.txt";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// This function for print in log file will print message of exception and stack trace and line number
        /// </summary>
        public void Log(string Message)
        {
            try
            {
               StreamWriter Writer = new StreamWriter(this.FilePath);
                StackTrace StackTraceForPrintDetailsOfErrors = new StackTrace(true);
                Writer.Write("\r\nLog Entry : ");
                Writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                Writer.WriteLine("{0}", Message);
                Writer.WriteLine("{0} {1}", "Tracing the error : ", StackTraceForPrintDetailsOfErrors.ToString().Trim());
                Writer.WriteLine("------------------------------------");
                Writer.Close(); 
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                
            }
        }
      
    }
}
