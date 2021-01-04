using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;
using Xunit;
namespace Question
{
    
   

    public abstract class Qustion
    {
        /// <summary>
        /// This abstract Method And override in all types of Question 
        /// </summary>
        [Required(ErrorMessageResourceName = "QuestionIsEmptyMessage", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string NewText { get; set; }
        [Required(ErrorMessage = "Order is Empty")]
        [Range(1, 10000, ErrorMessageResourceName = "TheOrderNumberLong", ErrorMessageResourceType = typeof(Resources.Messages))]
        public int Order { get; set; }
        public int Id { get; set; }
        public TypeOfQuestion TypeOfQuestion { get; set; }
        public abstract override bool Equals(Object NewObject); 
    }
}
