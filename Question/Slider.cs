using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;
namespace Question
{
    public class Slider : Qustion
    {
        /// <summary>
        /// Class Slider inhertaed Qustion and have 3 constructor 
        /// </summary>
        public Slider(int Id, int IdForType, string NewText, Question.TypeOfQuestion TypeOfQuestion, int Order, int StartValue, int EndValue, string StartCaption, string EndCaption)
        {
            try
            {
                this.NewText = NewText;
                this.Order = Order;
                this.StartValue = StartValue;
                this.EndValue = EndValue;
                this.StartCaption = StartCaption;
                this.EndCaption = EndCaption;
                this.Id = Id;
                this.TypeOfQuestion = TypeOfQuestion;
                this.IdForType = IdForType;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        public Slider(string NewText, Question.TypeOfQuestion TypeOfQuestion, int IdForType, int Order, int StartValue, int EndValue, string StartCaption, string EndCaption)
        {
            try
            {
                this.NewText = NewText;
                this.Order = Order;
                this.StartValue = StartValue;
                this.EndValue = EndValue;
                this.StartCaption = StartCaption;
                this.EndCaption = EndCaption;
                this.TypeOfQuestion = TypeOfQuestion;
                this.IdForType = IdForType;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        public Slider()
        {

        }
        public int IdForType { get; set; }
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public string StartCaption { get; set; }
        public string EndCaption { get; set; }
        /// <summary>
        /// Check if tow object type slider is equal 
        /// </summary>
        public override bool Equals(Object NewObject)
        {
            try
            {
                Slider Object2 = (Slider)NewObject;
                Slider Object1 = (Slider)this;
                if (Object1.Order == Object2.Order && Object1.StartValue == Object2.StartValue && Object1.StartCaption == Object2.StartCaption && Object1.EndValue == Object2.EndValue && Object1.EndCaption == Object2.EndCaption && Object1.NewText == Object2.NewText)
                    return false;
                return true;
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return false;
            }
        }
    }
}
