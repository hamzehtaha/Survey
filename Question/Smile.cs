using System;
using System.Collections.Generic;
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
        public Smiles() { }
        public int NumberOfSmiles { get; set; }
        public int IdForType { get; set; }

    }
}
