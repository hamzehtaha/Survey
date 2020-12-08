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
        public static Qustion ReturnNewQuestion { get; set; }
        private TypeOfChoice AddOrEditChoice;
        /// <summary>
        /// This constructor for hide and if i choose edit will show the variable for types of question
        /// </summary>
        public QuestionsInformation(TypeOfChoice AddOrEdit)
        {
            try
            {
                InitializeComponent();
                InitHide();
                NewText.Focus();
                AddOrEditChoice = AddOrEdit;
                switch (AddOrEdit)
                {
                    case TypeOfChoice.Edit:
                    //For Change Ttitle 
                    this.Text = Survey.Properties.Resource1.TitleOfQuestionEdit;
                    ShowDataForEdit();
                    if (ReturnNewQuestion != null)
                    {
                            switch (ReturnNewQuestion.TypeOfQuestion) 
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
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                switch (ReturnNewQuestion.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        Slider EditSlider = (Slider)ReturnNewQuestion;
                        NewText.Text = EditSlider.NewText;
                        NewOrder.Value = EditSlider.Order;
                        NewStartValue.Value = EditSlider.StartValue;
                        NewEndValue.Value = EditSlider.EndValue;
                        NewStartValueCaption.Text = EditSlider.StartCaption;
                        NewEndValueCaption.Text = EditSlider.EndCaption;
                        SliderRadio.Checked = true;
                        break;
                    case TypeOfQuestion.Stars:
                        Stars EditStar = (Stars)ReturnNewQuestion;
                        NewText.Text = EditStar.NewText;
                        NewOrder.Value = EditStar.Order;
                        NewNumberOfStars.Value = EditStar.NumberOfStars;
                        StarsRadio.Checked = true;
                        break;
                    case TypeOfQuestion.Smily:
                        Smiles EditSmile = (Smiles)ReturnNewQuestion;
                        NewText.Text = EditSmile.NewText;
                        NewOrder.Value = EditSmile.Order;
                        NewNumberOfSmiles.Value = EditSmile.NumberOfSmiles;
                        SmilyRadio.Checked = true;
                        break; 
                }
            }catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// to check the string is number or not 
        /// </summary>
        private bool IsNumber(string Number)
        {
            try
            {
                return int.TryParse(Number, out int N);
            }
            catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex);
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
                    MessageBox.Show(Survey.Properties.Resource1.QuestionIsEmptyMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
                else if (IsNumber(TypeQuestion.NewText))
                {
                    MessageBox.Show(Survey.Properties.Resource1.QuestionIsJustANumberMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                else if (TypeQuestion.Order <= 0)
                {
                    MessageBox.Show(Survey.Properties.Resource1.NewOrderLessThanZeroMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (TypeQuestion is Slider)
                {
                    Slider SliderCheck = (Slider)TypeQuestion;
                    if (SliderCheck.StartValue <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartValueLessThanZeroMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndValue <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndValueLessThanZeroMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartValue > 100)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartValueGreaterThanOneHundredMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndValue > 100)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndValueGreaterThanOneHundredMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartValue >= SliderCheck.EndValue)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.TheEndValueSholudGreaterThanStartValueMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.StartCaption == "")
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartCaptionEmptyMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(SliderCheck.StartCaption))
                    {
                        MessageBox.Show(Survey.Properties.Resource1.StartCaptionJustNumberMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (SliderCheck.EndCaption == "")
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndCaptionEmptyMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(SliderCheck.EndCaption))
                    {
                        MessageBox.Show(Survey.Properties.Resource1.EndCaptionJustNumberMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion is Smiles)
                {
                    Smiles SmilesCheck = (Smiles)TypeQuestion;
                    if (SmilesCheck.NumberOfSmiles <= 1 || SmilesCheck.NumberOfSmiles > 5)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.NumberOfSmileBetweenFiveAndTow, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion is Stars)
                {
                    Stars StarCheck = (Stars)TypeQuestion;
                    if (StarCheck.NumberOfStars <= 0 || StarCheck.NumberOfStars > 10)
                    {
                        MessageBox.Show(Survey.Properties.Resource1.NumberOfStrasBetweenTenAndOne, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
                this.DialogResult = DialogResult.OK; 
                this.Close();
            }catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }

        }
        /// <summary>
        /// when i press save button go to this function and from AddOrEdit var will know i edit or add the question 
        /// </summary>
        private int CheckAndAddQuestion(Qustion NewQuestion)
        {
            try
            {
                if (CheckTheData(NewQuestion))
                {
                    if (Operation.AddQustion(NewQuestion) == GenralVariables.Succeeded)
                    {
                        DataEnter();
                        ReturnNewQuestion = NewQuestion; 
                        return GenralVariables.Succeeded; 
                    }
                }
                this.DialogResult = DialogResult.Cancel; 
                return GenralVariables.NoData;
            }catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return GenralVariables.Error;
            }
        }
        private Qustion AddAttrubitesForQuestion(Qustion NewQuestion)
        {
            try
            {
                NewQuestion.NewText = NewText.Text;
                NewQuestion.Order = Convert.ToInt32(NewOrder.Value);
                return NewQuestion;
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return null;
            }
        }
        /// <summary>
        /// This function for add Attrubites for any Question
        /// </summary>
        private int AddAttrubitesForSlider (ref Slider NewQuestion)
        {
            try
            {
                NewQuestion.TypeOfQuestion = TypeOfQuestion.Slider;
                NewQuestion.StartValue = Convert.ToInt32(NewStartValue.Text);
                NewQuestion.EndValue = Convert.ToInt32(NewEndValue.Text);
                NewQuestion.StartCaption = NewStartValueCaption.Text;
                NewQuestion.EndCaption = NewEndValueCaption.Text;
                return GenralVariables.Succeeded; 
            }catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return GenralVariables.Error; 
            }
        }
        /// <summary>
        /// This function for add Attrubites for type Slider
        /// </summary>
        private int AddAttrubitesForSmile (ref Smiles NewQuestion)
        {
            try
            {
                NewQuestion.TypeOfQuestion = TypeOfQuestion.Smily;
                NewQuestion.NumberOfSmiles = Convert.ToInt32(NewNumberOfSmiles.Text);
                return GenralVariables.Succeeded;
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return GenralVariables.Error;
            }
        }
        /// <summary>
        /// This function for add Attrubites for type Smile
        /// </summary>
        private int AddAttrubitesForStar (ref Stars NewQuestion)
        {
            try
            {
                NewQuestion.TypeOfQuestion = TypeOfQuestion.Stars;
                NewQuestion.NumberOfStars = Convert.ToInt32(NewNumberOfStars.Text);
                return GenralVariables.Succeeded;
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return GenralVariables.Error;
            }
        }
        /// <summary>
        /// This function for add Attrubites for type Star
        /// </summary>
        private int AddQuestionFromOperation()
        {
            try
            {
                if (SliderRadio.Checked)
                {
                    Slider NewQuestion = new Slider();
                    NewQuestion = (Slider)AddAttrubitesForQuestion(NewQuestion);
                    if (NewQuestion != null && AddAttrubitesForSlider(ref NewQuestion) == GenralVariables.Succeeded && CheckAndAddQuestion(NewQuestion) == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded; 
                    return GenralVariables.NoData; 
                }
                else if (SmilyRadio.Checked)
                {
                    Smiles NewQuestion = new Smiles();
                    NewQuestion = (Smiles)AddAttrubitesForQuestion(NewQuestion);
                    if (NewQuestion != null && AddAttrubitesForSmile(ref NewQuestion) == GenralVariables.Succeeded && CheckAndAddQuestion(NewQuestion) == GenralVariables.Succeeded)
                        return GenralVariables.Succeeded;
                    return GenralVariables.NoData;
                }
                else if (StarsRadio.Checked)
                {
                    Stars NewQuestion = new Stars();
                    NewQuestion = (Stars)AddAttrubitesForQuestion(NewQuestion);
                    if (NewQuestion != null && AddAttrubitesForStar(ref NewQuestion) == GenralVariables.Succeeded && CheckAndAddQuestion(NewQuestion) == GenralVariables.Succeeded)
                        return GenralVariables.Succeeded;
                    return GenralVariables.NoData;
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    MessageBox.Show(Survey.Properties.Resource1.NotChooseTheType, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return GenralVariables.NoData; 
                }
            }catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return GenralVariables.Error;  
            }
        }
        /// <summary>
        /// This function call functions for any type of question for ADD
        /// and call this function in SaveClick function and call anthor function for ADD 
        /// </summary>
        private int EditQuestionFromOpertion()
        {
            try {
                switch (ReturnNewQuestion.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        Slider SliderForEdit = (Slider)ReturnNewQuestion;
                        SliderForEdit = (Slider)AddAttrubitesForQuestion(SliderForEdit);
                        
                        if (AddAttrubitesForSlider(ref SliderForEdit) == GenralVariables.Succeeded && CheckTheData(SliderForEdit))
                        {
                            if (Operation.EditQustion(SliderForEdit) == GenralVariables.Succeeded)
                            {
                                ReturnNewQuestion = SliderForEdit;
                                this.DialogResult = DialogResult.OK;
                                MessageBox.Show(Properties.Resource1.TheEditMessage);
                                this.Close();
                                return GenralVariables.Succeeded;
                            }
                           
                            
                        }
                           this.DialogResult = DialogResult.Cancel;
                           return GenralVariables.NoData;
                    case TypeOfQuestion.Smily:
                        Smiles SmileForEdit = (Smiles)ReturnNewQuestion;
                        SmileForEdit = (Smiles)AddAttrubitesForQuestion(SmileForEdit);
                        AddAttrubitesForSmile(ref SmileForEdit);
                        if (CheckTheData(SmileForEdit))
                        {
                            if (Operation.EditQustion(SmileForEdit) == GenralVariables.Succeeded)
                            {
                                ReturnNewQuestion = SmileForEdit;
                                this.DialogResult = DialogResult.OK;
                                MessageBox.Show(Properties.Resource1.TheEditMessage);
                                this.Close();
                                return GenralVariables.Succeeded;
                            }
                        }
                        this.DialogResult = DialogResult.Cancel;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Stars:
                        Stars StarForEdit = (Stars)ReturnNewQuestion;
                        StarForEdit = (Stars)AddAttrubitesForQuestion(StarForEdit);
                        AddAttrubitesForStar(ref StarForEdit);
                        if (CheckTheData(StarForEdit))
                        {
                            if (Operation.EditQustion(StarForEdit) == GenralVariables.Succeeded)
                            {
                                ReturnNewQuestion = StarForEdit;
                                MessageBox.Show(Properties.Resource1.TheEditMessage);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                                return GenralVariables.Succeeded;
                            }
                        }
                        this.DialogResult = DialogResult.Cancel;
                        return GenralVariables.NoData;
                    default:
                        this.DialogResult = DialogResult.Cancel; 
                        return GenralVariables.NoData;
                        
                }
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                Save.DialogResult = DialogResult.Cancel;
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return GenralVariables.Error; 
            }
            
        }
        /// <summary>
        /// This function call functions for any type of question for EDIT
        /// and call this function in SaveClick function and call anthor function for EDIT
        /// </summary>
        private void Save_Click(object sender, EventArgs e)
        {
            try {
                switch (AddOrEditChoice)
                {
                    case TypeOfChoice.Add:
                        AddQuestionFromOperation();
                        break;
                    case TypeOfChoice.Edit:
                        EditQuestionFromOpertion();
                        break; 
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
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
                GenralVariables.Errors.Log(ex);
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
