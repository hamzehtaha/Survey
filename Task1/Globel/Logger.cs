using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
namespace Survey
{
    /// <summary>
    /// This Class using  AbstractLog For Error 
    /// </summary>
    public class Logger 
    {
        private string CurrentDirectory {
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
                this.FileName = "Log.txt";
                this.FilePath = this.CurrentDirectory + "/" + this.FileName;
            }catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
            }
        }

        public  void Log(Exception ex)
        {
            //This Function OVERRIDE FROM AbstractLog that is write Errors and dates in log file  

            StreamWriter writer = new StreamWriter(this.FilePath);
            try
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                int LineNumber = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                string MethodName = frame.GetMethod().Name;
                writer.Write("\r\nLog Entry : ");
                writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                writer.WriteLine("{0}", ex.Message);
                writer.WriteLine("{0} {1}","Method name is", MethodName);
                writer.WriteLine("{0} {1}", "The Number of line :", LineNumber); 
                writer.WriteLine("------------------------------------");
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
