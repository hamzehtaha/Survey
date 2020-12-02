using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Question
{
    public enum TypeOfQuestion
    {
        Slider,
        Smily,
        Stars
    }
    public enum TypeOfChoice
    {
        Add,
        Edit
    }

    public abstract class Qustion
    {
        /// <summary>
        /// This abstract Method And override in all types of Question 
        /// </summary>
        public string NewText { get; set; }
        public int Order { get; set; }
        public int Id { get; set; }
        public TypeOfQuestion TypeOfQuestion { get; set; }
    }
}
