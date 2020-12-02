using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Survey;
using System.Diagnostics;
namespace Task1
{
   public abstract class Qustions
    {

        /// <summary>
        /// This abstract Method And override in all types of Question 
        /// </summary>
        public string NewText { get; set;}
        public int Order { get; set; }
        public int Id { get; set; }
        public string TypeOfQuestion { get; set;}
        
        
        
    }
}
