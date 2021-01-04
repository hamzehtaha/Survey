using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;
namespace Question
{
    public class Stars : Qustion
    {
        /// <summary>
        /// Class Stars inhertaed Qustion and have 3 constructor 
        /// </summary>
        public Stars(int Id, int IdForType, string NewText, Question.TypeOfQuestion TypeOfQuestion, int Order, int NumberOfStars)
        {
            try
            {
                this.NewText = NewText;
                this.NumberOfStars = NumberOfStars;
                this.Order = Order;
                this.TypeOfQuestion = TypeOfQuestion;
                this.Id = Id;
                this.IdForType = IdForType;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        public Stars(int IdForType, string NewText, Question.TypeOfQuestion TypeOfQuestion, int Order, int NumberOfStars)
        {
            try
            {
                this.NewText = NewText;
                this.NumberOfStars = NumberOfStars;
                this.Order = Order;
                this.TypeOfQuestion = TypeOfQuestion;
                this.IdForType = IdForType;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        public Stars()
        {
            try
            {
                this.TypeOfQuestion = TypeOfQuestion.Stars;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        [Required(ErrorMessageResourceName = "EmptyNumberOfStar", ErrorMessageResourceType = typeof(Resources.Messages))]
        [Range(1,10, ErrorMessageResourceName = "NumberOfStrasBetweenTenAndOne", ErrorMessageResourceType = typeof(Resources.Messages))]
        public int NumberOfStars { get; set; }
        public int IdForType { get; set; }
        public override bool Equals(Object NewObject)
        {
            try
            {
                Stars Object2 = (Stars)NewObject;
                Stars Object1 = (Stars)this;
                if (Object1.Order == Object2.Order && Object1.NumberOfStars == Object2.NumberOfStars && Object1.NewText == Object2.NewText)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return false;

            }
        }


    }
}
