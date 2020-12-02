using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Survey;
using System.Windows.Forms;
using System.Diagnostics;
namespace Task1
{
    class Stars : Qustions
    {
        /// <summary>
        /// Class Stars inhertaed Qustion and have 3 constructor 
        /// </summary>
        public Stars(int Id, int IdForType, string NewText, string TypeOfQuestion, int Order, int NumberOfStars)
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
                StaticObjects.Erros.Log(ex);
            }
        }
        public Stars(int IdForType, string NewText, string TypeOfQuestion, int Order, int NumberOfStars)
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
                StaticObjects.Erros.Log(ex);
            }
        }
        public Stars()
        {

        }
        public int NumberOfStars { get; set; }
        public int IdForType { get; set;  }
        


    }
}
