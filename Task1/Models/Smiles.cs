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
    class Smiles :Qustions 
    {
        /// <summary>
        /// Class Smile inhertaed Qustion and have 3 constructor 
        /// </summary>
        public Smiles(int Id,int IdForType, string NewText, string TypeOfQuestion, int Order,int NumberOfSmiles) {
            try
            {
                this.NewText = NewText;
                this.NumberOfSmiles = NumberOfSmiles;
                this.Order = Order;
                this.TypeOfQuestion = TypeOfQuestion;
                this.Id = Id;
                this.IdForType = IdForType;
            }catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
            }
        }
        public Smiles(int idForType, string NewText, string TypeOfQuestion, int Order, int NumberOfSmiles)
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
                StaticObjects.Erros.Log(ex);
            }

        }
        public Smiles() { }
        public int NumberOfSmiles { get; set; }
        public int IdForType { get; set; }

    }
}
