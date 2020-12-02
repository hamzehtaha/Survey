using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using BaseLog; 
namespace Question
{
    public class Slider : Qustions
    {
        /// <summary>
        /// Class Slider inhertaed Qustion and have 3 constructor 
        /// </summary>
        public Slider(int Id, int IdForType, string NewText, Global.TypeOfQuestion TypeOfQuestion, int Order, int StartValue, int EndValue, string StartCaption, string EndCaption)
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
                StaticObjects.Erros.Log(ex);
            }
        }
        public Slider(string NewText, Global.TypeOfQuestion TypeOfQuestion, int IdForType, int Order, int StartValue, int EndValue, string StartCaption, string EndCaption)
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
                StaticObjects.Erros.Log(ex);
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
    }
}
