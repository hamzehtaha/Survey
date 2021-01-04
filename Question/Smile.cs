using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;
namespace Question
{
    public class Smiles : Qustion
    {
        /// <summary>
        /// Class Smile inhertaed Qustion and have 3 constructor 
        /// </summary>
        public Smiles(int Id, int IdForType, string NewText, Question.TypeOfQuestion TypeOfQuestion, int Order, int NumberOfSmiles)
        {
            try
            {
                this.NewText = NewText;
                this.NumberOfSmiles = NumberOfSmiles;
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
        public Smiles(int idForType, string NewText, Question.TypeOfQuestion TypeOfQuestion, int Order, int NumberOfSmiles)
        {
            try
            {
                this.NewText = NewText;
                this.NumberOfSmiles = NumberOfSmiles;
                this.Order = Order;
                this.TypeOfQuestion = TypeOfQuestion;
                this.IdForType = idForType;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }

        }
        public Smiles() {
            try
            {
                this.TypeOfQuestion = TypeOfQuestion.Smily;
            }catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        [Required(ErrorMessageResourceName = "EmptyNumberOfSmile", ErrorMessageResourceType = typeof(Resources.Messages))]
        [Range(2,5, ErrorMessageResourceName = "NumberOfSmileBetweenFiveAndTow", ErrorMessageResourceType = typeof(Resources.Messages))]
        public int NumberOfSmiles { get; set; }
        public int IdForType { get; set; }
        
        public override bool Equals(Object NewObject)
        {
            try
            {
                Smiles Object2 = (Smiles)NewObject;
                Smiles Object1 = (Smiles)this;
                if (Object1.Order == Object2.Order && Object1.NumberOfSmiles == Object2.NumberOfSmiles && Object1.NewText == Object2.NewText)
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
