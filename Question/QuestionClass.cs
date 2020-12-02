using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Question
{
    public abstract class Qustions
    {
        /// <summary>
        /// This abstract Method And override in all types of Question 
        /// </summary>
        public string NewText { get; set; }
        public int Order { get; set; }
        public int Id { get; set; }
        public Global.TypeOfQuestion TypeOfQuestion { get; set; }
    }
}
