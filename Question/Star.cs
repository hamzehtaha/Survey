﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog;
using System.Windows.Forms;
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
                MessageBox.Show(Properties.Resource1.ErrorModels);
                GenralVariables.Errors.Log(ex);
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
                MessageBox.Show(Properties.Resource1.ErrorModels);
                GenralVariables.Errors.Log(ex);
            }
        }
        public Stars()
        {

        }
        public int NumberOfStars { get; set; }
        public int IdForType { get; set; }



    }
}
