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
    /// This Class using  AbstractLog For Error 
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
        public Logger()
        {
            try
            {
                this.CurrentDirectory = Directory.GetCurrentDirectory();
                Console.WriteLine(Directory.GetCurrentDirectory()); 
                this.FileName = "Log.txt";
                this.FilePath = this.CurrentDirectory + "/" + this.FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// This Function OVERRIDE FROM AbstractLog that is write Errors and dates in log file 
        /// </summary>
        public void Log(string Message)
        {
            try
            {
                StreamWriter writer = new StreamWriter(this.FilePath);
                StackTrace st = new StackTrace(true);
                StackFrame frame = st.GetFrame(0);
                string MethodName = frame.GetMethod().Name;
                writer.Write("\r\nLog Entry : ");
                writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                writer.WriteLine("{0}", Message);
                writer.WriteLine("{0} {1}", "Method Name,Line Number:",st.ToString().Trim());
                writer.WriteLine("------------------------------------");
                writer.Close(); 
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                
            }
        }
      
    }
}
