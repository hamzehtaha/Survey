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
                    this.Text = Survey.Properties.Messages.TitleOfQuestionEdit;
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
                        this.Text = Survey.Properties.Messages.TitleOfQuestionAdd;
                        GroupOfTypes.Visible = true;
                        InitHide();
                        break; 
                }
                
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
                return false;
            }
        }
        /// <summary>
        /// This Function to Check validation of data 
        /// </summary>
        private bool CheckTheData(TypeOfQuestion TypeQuestion)
        {
            try
            {
                if (NewText.Text == "")
                {
                    MessageBox.Show(Survey.Properties.Messages.QuestionIsEmptyMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
                else if (IsNumber(NewText.Text))
                {
                    MessageBox.Show(Survey.Properties.Messages.QuestionIsJustANumberMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                else if (NewOrder.Value <= 0)
                {
                    MessageBox.Show(Survey.Properties.Messages.NewOrderLessThanZeroMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (TypeQuestion == TypeOfQuestion.Slider)
                {
                    if (NewStartValue.Value <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Messages.StartValueLessThanZeroMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (NewEndValue.Value <= 0)
                    {
                        MessageBox.Show(Survey.Properties.Messages.EndValueLessThanZeroMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (NewStartValue.Value > 100)
                    {
                        MessageBox.Show(Survey.Properties.Messages.StartValueGreaterThanOneHundredMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (NewEndValue.Value > 100)
                    {
                        MessageBox.Show(Survey.Properties.Messages.EndValueGreaterThanOneHundredMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (NewStartValue.Value >= NewEndValue.Value)
                    {
                        MessageBox.Show(Survey.Properties.Messages.TheEndValueSholudGreaterThanStartValueMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (NewStartValueCaption.Text == "")
                    {
                        MessageBox.Show(Survey.Properties.Messages.StartCaptionEmptyMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(NewStartValueCaption.Text))
                    {
                        MessageBox.Show(Survey.Properties.Messages.StartCaptionJustNumberMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (NewStartValueCaption.Text == "")
                    {
                        MessageBox.Show(Survey.Properties.Messages.EndCaptionEmptyMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (IsNumber(NewStartValueCaption.Text))
                    {
                        MessageBox.Show(Survey.Properties.Messages.EndCaptionJustNumberMessage, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion == TypeOfQuestion.Smily)
                {
                    if (NewNumberOfSmiles.Value <= 1 || NewNumberOfSmiles.Value > 5)
                    {
                        MessageBox.Show(Survey.Properties.Messages.NumberOfSmileBetweenFiveAndTow, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (TypeQuestion == TypeOfQuestion.Stars)
                {
                    if (NewNumberOfStars.Value <= 0 || NewNumberOfStars.Value > 10)
                    {
                        MessageBox.Show(Survey.Properties.Messages.NumberOfStrasBetweenTenAndOne, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);

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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }
        /// <summary>
        /// This Function For User know is data is edited or Added
        /// </summary>
        private void DataEnter()
        {
            try
            {
                MessageBox.Show(Survey.Properties.Messages.DataIsEnterd);
                this.DialogResult = DialogResult.OK; 
                this.Close();
            }catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }

        }
        private bool CheckMessageError(int ResultNumber)
        {
            try
            {
                if (ResultNumber == GenralVariables.Succeeded)
                    return true;
                else if (ResultNumber == OperationManger.GenralVariables.ErrorInManger)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorManger);
                    return false;
                }
                else if (ResultNumber == OperationManger.GenralVariables.ErrorInMangerAdd)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorMangerAddQuestion);
                    return false;
                }
                else if (ResultNumber == OperationManger.GenralVariables.ErrorInMangerDelete)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorMangerDeleteQuestion);
                    return false;
                }
                else if (ResultNumber == OperationManger.GenralVariables.ErrorInMangerEdit)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorMangerEditQuestion);
                    return false;
                }
                else if (ResultNumber == OperationManger.GenralVariables.ErrorInMangerGetQuestion)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorMangerGetQuestion);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInDataBase)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBase);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorConnectionString)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBaseConnectionString);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInAddQuestion)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBaseAddQuestion);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInDeleteQuestion)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBaseDeleteQuestion);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInEditQuestion)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBaseEditQuestion);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInGetQuestion)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBaseGetQuestion);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInSelectionQuestion)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorDataBaseSelectQuestion);
                    return false;
                }
                else if (ResultNumber == GenralVariables.ErrorInOperation)
                {
                    MessageBox.Show(Survey.Properties.Messages.ErrorInOperation);
                    return false;
                }
                else
                {
                    MessageBox.Show(Survey.Properties.Messages.MessageError);
                    return false;
                }
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
                return false; 
            }
            
        }
        /// <summary>
        /// when i press save button go to this function and from AddOrEdit var will know i edit or add the question 
        /// </summary>
        private int CheckAndAddQuestion(Qustion NewQuestion)
        {
            try
            {
                if (CheckTheData(NewQuestion.TypeOfQuestion))
                {
                    if (CheckMessageError(Operation.AddQustion(NewQuestion)))
                    {
                        DataEnter();
                        ReturnNewQuestion = NewQuestion;
                        return GenralVariables.Succeeded;
                    }
                }
                return GenralVariables.NoData;
            }catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                    MessageBox.Show(Survey.Properties.Messages.NotChooseTheType, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return GenralVariables.NoData; 
                }
            }catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                        if (CheckTheData(TypeOfQuestion.Slider))
                        {
                            Slider SliderForEdit = (Slider)ReturnNewQuestion;
                            SliderForEdit = (Slider)AddAttrubitesForQuestion(SliderForEdit);
                            if (AddAttrubitesForSlider(ref SliderForEdit) == GenralVariables.Succeeded)
                            {
                                if (CheckMessageError(Operation.EditQustion(SliderForEdit)))
                                {
                                    ReturnNewQuestion = SliderForEdit;
                                    this.DialogResult = DialogResult.OK;
                                    MessageBox.Show(Properties.Messages.TheEditMessage);
                                    this.Close();
                                    return GenralVariables.Succeeded;
                                }
                            }
                        }
                           return GenralVariables.NoData;
                    case TypeOfQuestion.Smily:
                        if (CheckTheData(TypeOfQuestion.Smily))
                        {
                            Smiles SmileForEdit = (Smiles)ReturnNewQuestion;
                            SmileForEdit = (Smiles)AddAttrubitesForQuestion(SmileForEdit);
                            AddAttrubitesForSmile(ref SmileForEdit);
                            if (CheckMessageError(Operation.EditQustion(SmileForEdit)))
                            {
                                ReturnNewQuestion = SmileForEdit;
                                this.DialogResult = DialogResult.OK;
                                MessageBox.Show(Properties.Messages.TheEditMessage);
                                this.Close();
                                return GenralVariables.Succeeded;
                            }
                        }
                        return GenralVariables.NoData;
                        
                    case TypeOfQuestion.Stars:
                        if (CheckTheData(TypeOfQuestion.Stars))
                        {
                            Stars StarForEdit = (Stars)ReturnNewQuestion;
                            StarForEdit = (Stars)AddAttrubitesForQuestion(StarForEdit);
                            AddAttrubitesForStar(ref StarForEdit);
                            if (CheckMessageError(Operation.EditQustion(StarForEdit)))
                            {
                                ReturnNewQuestion = StarForEdit;
                                MessageBox.Show(Properties.Messages.TheEditMessage);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                                return GenralVariables.Succeeded;
                            }
                        }
                         
                        return GenralVariables.NoData;
                    default:
                        this.DialogResult = DialogResult.Cancel; 
                        return GenralVariables.NoData;
                        
                }
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                Save.DialogResult = DialogResult.Cancel;
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
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
