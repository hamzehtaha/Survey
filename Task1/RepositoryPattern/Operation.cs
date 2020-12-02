using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1;
namespace Survey.RepositoryPattern
{
    class Operation
    {
        public static Qustions AddQustion(Qustions Question)
        {
            try
            {
                if (Question is Slider)
                {
                    return DataBaseConnections.AddNewSlider(Question);
                }
                else if (Question is Smiles)
                {
                    return DataBaseConnections.AddNewSmile(Question);
                }
                else
                {
                    return DataBaseConnections.AddNewStar(Question);
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
                return null;
            }
        }
        public static Qustions EditQustion(Qustions Question)
        {
            try
            {
                if (Question is Slider)
                {
                    return DataBaseConnections.EditSlider(Question);
                }
                else if (Question is Smiles)
                {
                    return DataBaseConnections.EditSmile(Question);
                }
                else
                {
                    return DataBaseConnections.EditStar(Question);
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
                return null;
            }
        }
        public static int DeleteQustion(Qustions Question)
        {
            try
            {
                if (Question is Slider)
                {
                    return DataBaseConnections.DeleteSlider(Question);
                }
                else if (Question is Smiles)
                {
                    return DataBaseConnections.DeleteSmile(Question);
                }
                else
                {
                    return DataBaseConnections.DeleteStar(Question);
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
                return 0;
            }
        }
        public static List<Qustions> GetQustion()
        {
            try
            {
                return DataBaseConnections.GetQuestionFromDataBase();
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return null;
            }
        }
        /// <summary>
        /// to check the string is number or not ?
        /// </summary>
        private static bool IsNumber(string Number)
        {
            return int.TryParse(Number, out int N);
        }
        /// <summary>
        /// This Function to Check validation of data 
        /// </summary>
        public static bool CheckTheData(Qustions TypeQuestion)
        {
            try
            {
                if (TypeQuestion.NewText == Constant.Empty)
                {
                    MessageBox.Show(Survey.Properties.Resource1.QuestionIsEmptyMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
                else if (IsNumber(TypeQuestion.NewText))
                {
                    MessageBox.Show(Survey.Properties.Resource1.QuestionIsJustANumberMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                else if (TypeQuestion.Order <= 0)
                {
                    MessageBox.Show(Survey.Properties.Resource1.NewOrderLessThanZeroMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (TypeQuestion is Slider)
                {
                    Slider SliderCheck = (Slider)TypeQuestion;
                    if (SliderCheck.StartValue <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartValueLessThanZeroMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndValue <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndValueLessThanZeroMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartValue > 100)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartValueGreaterThanOneHundredMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndValue > 100)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndValueGreaterThanOneHundredMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartValue >= SliderCheck.EndValue)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.TheEndValueSholudGreaterThanStartValueMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartCaption == Constant.Empty)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartCaptionEmptyMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(SliderCheck.StartCaption))
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartCaptionJustNumberMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndCaption == Constant.Empty)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndCaptionEmptyMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(SliderCheck.EndCaption))
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndCaptionJustNumberMessage, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion is Smiles)
                {
                    Smiles SmilesCheck = (Smiles)TypeQuestion;
                    if (SmilesCheck.NumberOfSmiles <= 1 || SmilesCheck.NumberOfSmiles > 5)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.NumberOfSmileBetweenFiveAndTow, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion is Stars)
                {
                    Stars StarCheck = (Stars)TypeQuestion;
                    if (StarCheck.NumberOfStars <= 0 || StarCheck.NumberOfStars > 10)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.NumberOfStrasBetweenTenAndOne, Constant.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);

                return false;
            }
            return true;
        }
    }
}

