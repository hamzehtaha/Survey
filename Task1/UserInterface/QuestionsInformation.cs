using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Survey;
using System.Threading;
using System.Diagnostics;
using Task1;
using Question;
using BaseLog;
using DataBaseConnection;
using OperationManger; 
namespace Survey
{
    public partial class QuestionsInformation : Form
    {
        /// <summary>
        /// privtae objects for add,edit and delete 
        /// </summary>
        private Qustion QustionForOperations = null;
        public static Qustion ReturnNewQuestion { get; set; }
        private TypeOfChoice AddOrEditChoice;
        private const string ErrorString = "Error";
        /// <summary>
        /// This constructor for hide and if i choose edit will show the variable for types of question
        /// </summary>
        public QuestionsInformation(Qustion QustionForOperations, TypeOfChoice AddOrEdit)
        {
            try
            {
                InitializeComponent();
                InitHide();
                this.QustionForOperations = QustionForOperations;
                NewText.Focus();
                AddOrEditChoice = AddOrEdit;
                switch (AddOrEdit)
                {
                    case TypeOfChoice.Edit:
                    //For Change Ttitle 
                    this.Text = Survey.Properties.Resource1.TitleOfQuestionEdit;
                    ShowDataForEdit();
                    if (QustionForOperations != null)
                    {
                            switch (QustionForOperations.TypeOfQuestion) 
                            {
                                case TypeOfQuestion.Slider:
                                    ShowForSlider();
                                    break;
                                case TypeOfQuestion.Smily:
                                    ShowForSmiles();
                                    break;
                                case TypeOfQuestion.Stars:
                                    ShowForStars();
                                    break; 
                            }
                    }
                        break;
                    case TypeOfChoice.Add:
                        this.Text = Survey.Properties.Resource1.TitleOfQuestionAdd;
                        GroupOfTypes.Visible = true;
                        InitHide();
                        break; 
                }
                
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// this when choose the slider the slider attrubites will appear
        /// </summary>
        private void ShowForSlider()
        {
            try
            {
                InitHide();
                GroupOfSlider.Visible = true; 
            }catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// this when choose the smile the smile attrubites will appear
        /// </summary>
        private void ShowForSmiles()
        {
            try
            {
                InitHide();
                GroupOfSmile.Visible = true;
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// this when choose the stars the stars attrubites will appear
        /// </summary>
        private void ShowForStars()
        {
            try
            {
                InitHide();
                   GroupOfStars.Visible = true;
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// This old data in object and appear when press edit question 
        /// </summary>
        private void ShowDataForEdit()
        {
            try
            {
                GroupOfTypes.Visible = true;
                GroupOfTypes.Enabled = false;
                switch (QustionForOperations.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        Slider EditSlider = (Slider)QustionForOperations;
                        NewText.Text = EditSlider.NewText;
                        NewOrder.Value = EditSlider.Order;
                        NewStartValue.Value = EditSlider.StartValue;
                        NewEndValue.Value = EditSlider.EndValue;
                        NewStartValueCaption.Text = EditSlider.StartCaption;
                        NewEndValueCaption.Text = EditSlider.EndCaption;
                        SliderRadio.Checked = true;
                        break;
                    case TypeOfQuestion.Stars:
                        Stars EditStar = (Stars)QustionForOperations;
                        NewText.Text = EditStar.NewText;
                        NewOrder.Value = EditStar.Order;
                        NewNumberOfStars.Value = EditStar.NumberOfStars;
                        StarsRadio.Checked = true;
                        break;
                    case TypeOfQuestion.Smily:
                        Smiles EditSmile = (Smiles)QustionForOperations;
                        NewText.Text = EditSmile.NewText;
                        NewOrder.Value = EditSmile.Order;
                        NewNumberOfSmiles.Value = EditSmile.NumberOfSmiles;
                        SmilyRadio.Checked = true;
                        break; 
                }
            }catch(Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// to check the string is number or not ?
        /// </summary>
        private bool IsNumber(string Number)
        {
            try
            {
                return int.TryParse(Number, out int N);
            }
            catch(Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return false;
            }
        }
        /// <summary>
        /// This Function to Check validation of data 
        /// </summary>
        private bool CheckTheData(Qustion TypeQuestion)
        {
            try
            {
                if (TypeQuestion.NewText == "")
                {
                    MessageBox.Show(Survey.Properties.Resource1.QuestionIsEmptyMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
                else if (IsNumber(TypeQuestion.NewText))
                {
                    MessageBox.Show(Survey.Properties.Resource1.QuestionIsJustANumberMessage,ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                else if (TypeQuestion.Order <= 0)
                {
                    MessageBox.Show(Survey.Properties.Resource1.NewOrderLessThanZeroMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (TypeQuestion is Slider)
                {
                    Slider SliderCheck = (Slider)TypeQuestion;
                    if (SliderCheck.StartValue <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartValueLessThanZeroMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndValue <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndValueLessThanZeroMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartValue > 100)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartValueGreaterThanOneHundredMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndValue > 100)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndValueGreaterThanOneHundredMessage,ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartValue >= SliderCheck.EndValue)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.TheEndValueSholudGreaterThanStartValueMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartCaption == "")
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartCaptionEmptyMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(SliderCheck.StartCaption))
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartCaptionJustNumberMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndCaption == "")
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndCaptionEmptyMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(SliderCheck.EndCaption))
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndCaptionJustNumberMessage, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion is Smiles)
                {
                    Smiles SmilesCheck = (Smiles)TypeQuestion;
                    if (SmilesCheck.NumberOfSmiles <= 1 || SmilesCheck.NumberOfSmiles > 5)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.NumberOfSmileBetweenFiveAndTow, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion is Stars)
                {
                    Stars StarCheck = (Stars)TypeQuestion;
                    if (StarCheck.NumberOfStars <= 0 || StarCheck.NumberOfStars > 10)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.NumberOfStrasBetweenTenAndOne, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);

                return false;
            }
            return true;
        }
        /// <summary>
        /// This For Hide panel and radio Button
        /// </summary>
        private void InitHide()
        {
            try
            {
                GroupOfSlider.Visible = false;
                GroupOfSmile.Visible = false;
                GroupOfStars.Visible = false; 
            }catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }

        }
        /// <summary>
        ///  // for radio Button cahnges 
        /// </summary>
        private void Slider_CheckedChange(object sender, EventArgs e)
        {
           
            try
            {
                if (SliderRadio.Checked == true)
                {
                    ShowForSlider();
                }
            } catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// for radio Button cahnges 
        /// </summary>
        private void Smily_CheckedChange(object sender, EventArgs e)
        {
            
            try
            {
                if (SmilyRadio.Checked == true)
                {
                    ShowForSmiles(); 
                }
            } catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// for radio Button cahnges
        /// </summary>
        private void Stars_CheckedChange(object sender, EventArgs e)
        {
            try
            {
                if (StarsRadio.Checked == true)
                {
                    ShowForStars(); 

                }
            } catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// This Function For User know is data is edited or Added
        /// </summary>
        private void DataEnter()
        {
            try
            {
                MessageBox.Show(Survey.Properties.Resource1.DataIsEnterd);
                this.Close();
            }catch(Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }

        }
        /// <summary>
        /// when i press save button go to this function and from AddOrEdit var will know i edit or add the question 
        /// </summary>
        private Qustion AddAttrubitesForQuestion (Qustion NewQuestion)
        {
            try
            {
                NewQuestion.NewText = NewText.Text;
                NewQuestion.Order = Convert.ToInt32(NewOrder.Value);
                return NewQuestion;
            }catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return null; 
            }
        }
        private void CheckAndAddQuestion(Qustion NewQuestion)
        {
            try
            {
                if (CheckTheData(NewQuestion))
                {
                    if (Operation.AddQustion(NewQuestion, out QustionForOperations) == 1)
                    {
                        DataEnter();
                        ReturnNewQuestion = QustionForOperations;
                    }
                }
            }catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            try {
                switch (AddOrEditChoice)
                {
                    case TypeOfChoice.Add:
                        if (SliderRadio.Checked)
                        {
                            Slider NewQuestion = new Slider();
                            NewQuestion = (Slider)AddAttrubitesForQuestion(NewQuestion);
                            NewQuestion.TypeOfQuestion = TypeOfQuestion.Slider; 
                            NewQuestion.StartValue = Convert.ToInt32(NewStartValue.Text);
                            NewQuestion.EndValue = Convert.ToInt32(NewEndValue.Text);
                            NewQuestion.StartCaption = NewStartValueCaption.Text;
                            NewQuestion.EndCaption = NewEndValueCaption.Text;
                            CheckAndAddQuestion(NewQuestion); 
                        }
                        else if (SmilyRadio.Checked)
                        {
                            Smiles NewQuestion = new Smiles();
                            NewQuestion = (Smiles)AddAttrubitesForQuestion(NewQuestion);
                            NewQuestion.TypeOfQuestion = TypeOfQuestion.Smily;
                            NewQuestion.NumberOfSmiles = Convert.ToInt32(NewNumberOfSmiles.Text);
                            CheckAndAddQuestion(NewQuestion);
                        }
                        else if (StarsRadio.Checked)
                        {
                            Stars NewQuestion = new Stars();
                            NewQuestion = (Stars)AddAttrubitesForQuestion(NewQuestion);
                            NewQuestion.TypeOfQuestion = TypeOfQuestion.Stars;
                            NewQuestion.NumberOfStars = Convert.ToInt32(NewNumberOfStars.Text);
                            CheckAndAddQuestion(NewQuestion);
                        }
                        else
                            MessageBox.Show(Survey.Properties.Resource1.NotChooseTheType, ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case TypeOfChoice.Edit:
                        switch (QustionForOperations.TypeOfQuestion) 
                        {
                            case TypeOfQuestion.Slider:
                                Slider SliderForEdit = (Slider)QustionForOperations;
                                SliderForEdit = (Slider)AddAttrubitesForQuestion(SliderForEdit); 
                                SliderForEdit.StartValue = Convert.ToInt32(NewStartValue.Value);
                                SliderForEdit.EndValue = Convert.ToInt32(NewEndValue.Value);
                                SliderForEdit.StartCaption = NewStartValueCaption.Text;
                                SliderForEdit.EndCaption = NewEndValueCaption.Text;
                                if (CheckTheData(SliderForEdit))
                                {
                                    if (Operation.EditQustion(SliderForEdit) != 0) {
                                        ReturnNewQuestion = SliderForEdit;
                                        MessageBox.Show(Properties.Resource1.TheEditMessage);
                                        this.Close();
                                    }

                                }
                                break;
                            case TypeOfQuestion.Smily:
                                Smiles SmileForEdit = (Smiles)QustionForOperations;
                                SmileForEdit= (Smiles)AddAttrubitesForQuestion(SmileForEdit); 
                                SmileForEdit.NumberOfSmiles = Convert.ToInt32(NewNumberOfSmiles.Value);
                                if (CheckTheData(SmileForEdit))
                                {
                                    if (Operation.EditQustion(SmileForEdit) != 0)
                                    {
                                        ReturnNewQuestion = SmileForEdit;
                                        MessageBox.Show(Properties.Resource1.TheEditMessage);
                                        this.Close();
                                    }
                                }
                                break;
                            case TypeOfQuestion.Stars:
                                Stars StarForEdit = (Stars)QustionForOperations;
                                StarForEdit = (Stars)AddAttrubitesForQuestion(StarForEdit);
                                StarForEdit.NumberOfStars = Convert.ToInt32(NewNumberOfStars.Value);
                                if (CheckTheData(StarForEdit))
                                {
                                    if (Operation.EditQustion(StarForEdit) != 0)
                                    {
                                        ReturnNewQuestion = StarForEdit;
                                        MessageBox.Show(Properties.Resource1.TheEditMessage);
                                        this.Close();
                                    }
                                }
                                break; 
                        }
                        break; 
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// for close this page
        /// </summary>
        private void Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }catch(Exception ex)
            {
                Qustion.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        private void textBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void NewNumberOfSmiles_ValueChanged(object sender, EventArgs e)
        {
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void QuestionsInformation_Load(object sender, EventArgs e)
        {

        }
        private void questionDetalis1_Load(object sender, EventArgs e)
        {

        }
    }
}
