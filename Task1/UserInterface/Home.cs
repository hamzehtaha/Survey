
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1;
using Question;
using BaseLog;
using DataBaseConnection;
using OperationManger; 
namespace Survey
{
    
    public partial class Home : Form
    {
        private delegate void SafeCallDelegate(List<Qustion>ListOfQuestion);
        public Home()
        {
            try
            {
                StartFunction();
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }
        /// <summary>
        /// The start function for open a home page and get data is already saved in database and show it in datagridview
        /// </summary>
        private void StartFunction()
        {
            try
            {
                InitializeComponent();
                Operation.PutListToShow = Refresh;
                Operation.ListOfAllQuestion.Clear();
                if (Operation.GetQustion(ref Operation.ListOfAllQuestion) == GenralVariables.Succeeded)
                     ShowData(Operation.ListOfAllQuestion);
            }
            catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }

        }
        /// <summary>
        /// This Functions for thread to refresh data 
        /// </summary>
        private void Refresh()
        {
            try
            {
                var DelegateFunction = new SafeCallDelegate(ShowData);
                ListOfQuestion.Invoke(DelegateFunction, Operation.ListOfAllQuestion); 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }
        /// <summary>
        /// This function will return object is select in datagridview for edit and delete 
        /// </summary>
        private Qustion GetObjectSelect()
        {
            try
            {
                if (ListOfQuestion.SelectedRows.Count != 0)
                {
                    foreach (Qustion Temp in Operation.ListOfAllQuestion)
                    {
                        if (Temp.NewText.Equals(ListOfQuestion.SelectedCells[0].Value) && Temp.TypeOfQuestion.Equals(ListOfQuestion.SelectedCells[1].Value) && Temp.Order == Convert.ToInt32(ListOfQuestion.SelectedCells[2].Value))
                        {
                            return Temp;
                        }

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
                return null;
            }
        }
        /// <summary>
        /// Show data function get the question from MyList and show it in datagridview
        /// </summary>
        private void ShowData( List<Qustion> ListOfAllQuestion)
        {
            try
            {
              
                    ListOfQuestion.Rows.Clear();
                    foreach (Qustion Temp in ListOfAllQuestion)
                    {
                    if (Temp != null)
                    {
                        int Index = ListOfQuestion.Rows.Add();
                        ListOfQuestion.Rows[Index].Cells[0].Value = Temp.NewText;
                        ListOfQuestion.Rows[Index].Cells[2].Value = Temp.Order;
                        ListOfQuestion.Rows[Index].Cells[1].Value = Temp.TypeOfQuestion;
                    }
                    }
                ListOfQuestion.ClearSelection();
            } catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
            
        }
        /// <summary>
        /// This Listener for Add button when press add button 
        /// </summary>
        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                ListOfQuestion.ClearSelection();
                QuestionsInformation QuestionsInformationPage = new QuestionsInformation(TypeOfChoice.Add);
                if (QuestionsInformationPage.ShowDialog() == DialogResult.OK)
                {
                    Operation.ListOfAllQuestion.Add(QuestionsInformation.ReturnNewQuestion);
                    ShowData(Operation.ListOfAllQuestion);
                }
            } catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }
        /// <summary>
        /// This Listener for Edit button when press add button
        /// </summary>
        private void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                QuestionsInformation.ReturnNewQuestion = GetObjectSelect();
                Qustion OldObject = QuestionsInformation.ReturnNewQuestion;
                ListOfQuestion.ClearSelection();
                if (QuestionsInformation.ReturnNewQuestion != null)
                {
                    QuestionsInformation QuestionsInformationPage = new QuestionsInformation(TypeOfChoice.Edit);
                    if (QuestionsInformationPage.ShowDialog() == DialogResult.OK)
                    {
                        Operation.ListOfAllQuestion.Remove(OldObject);
                        Operation.ListOfAllQuestion.Add(QuestionsInformation.ReturnNewQuestion);
                        ShowData(Operation.ListOfAllQuestion); 
                    }
                }
                else
                {
                    MessageBox.Show(Survey.Properties.Messages.NoSelectItem, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }
        private void ListOfQuestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                QuestionsInformation.ReturnNewQuestion = GetObjectSelect();
                Qustion OldObject = QuestionsInformation.ReturnNewQuestion;
                Operation.ListOfAllQuestion.Remove(OldObject);
                ListOfQuestion.ClearSelection();
                if (QuestionsInformation.ReturnNewQuestion != null)
                {
                    QuestionsInformation QuestionsInformationPage = new QuestionsInformation(TypeOfChoice.Edit);
                    if (QuestionsInformationPage.ShowDialog() == DialogResult.OK)
                    {
                        Operation.ListOfAllQuestion.Remove(OldObject);
                        Operation.ListOfAllQuestion.Add(QuestionsInformation.ReturnNewQuestion);
                        ShowData(Operation.ListOfAllQuestion);
                    }
                }
                else
                {
                    MessageBox.Show(Survey.Properties.Messages.NoSelectItem, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }

        }
        /// <summary>
        /// This Listener for delete button when press add button
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Qustion QuestionForDelete = GetObjectSelect();
                int Check = 0;
                if (QuestionForDelete == null)
                {
                    MessageBox.Show(Survey.Properties.Messages.NoSelectItem, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show(Survey.Properties.Messages.SureToDeleteMessage, GenralVariables.DELETE, MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        switch (QuestionForDelete.TypeOfQuestion)
                        {
                            case TypeOfQuestion.Slider:
                                Slider SliderWillDelete = (Slider)QuestionForDelete;
                                Check = Operation.DeleteQustion(SliderWillDelete);
                                if (CheckMessageError(Check))
                                {
                                    
                                    Operation.ListOfAllQuestion.Remove(SliderWillDelete);
                                    ShowData(Operation.ListOfAllQuestion);
                                }
                                break;
                            case TypeOfQuestion.Smily:
                                Smiles SmileWillDelete = (Smiles)QuestionForDelete;
                                Check = Operation.DeleteQustion(SmileWillDelete);
                                if (CheckMessageError(Check))
                                {
                                    
                                    Operation.ListOfAllQuestion.Remove(SmileWillDelete);
                                    ShowData(Operation.ListOfAllQuestion);
                                }
                                break;
                            case TypeOfQuestion.Stars:
                                Stars StarWillDelete = (Stars)QuestionForDelete;
                                Check = Operation.DeleteQustion(StarWillDelete);
                                if (CheckMessageError(Check))
                                {
                                    
                                    Operation.ListOfAllQuestion.Remove(StarWillDelete);
                                    ShowData(Operation.ListOfAllQuestion);
                                }
                                break;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
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
        /// This for change language from arabic to english and english to arabic  
        /// /// </summary>
        private void changeToArabicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GenralVariables.Languge.Equals(Langugaes.English.ToString()))
                {
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(GenralVariables.ArabicMark);
                    GenralVariables.Languge = Langugaes.Arabic.ToString();
                    Operation.ListOfAllQuestion.Clear();
                }
                else
                {
                    GenralVariables.Languge = Langugaes.English.ToString();
                    Operation.ListOfAllQuestion.Clear();
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(GenralVariables.EnglishMark);
                }
                this.Controls.Clear();
                StartFunction();
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            try
            {
               //Operation.RefreshData();
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }

        private void ListOfQuestion_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {


        }

        private void ListOfQuestion_Click(object sender, EventArgs e)
        {



        }

        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
           try
           {
                Operation.EnableAutoRefrsh = false;
           }
            catch (Exception ex)
            {
                Operation.EnableAutoRefrsh = false;
                GenralVariables.Errors.Log(ex.Message);
                MessageBox.Show(Survey.Properties.Messages.MessageError);
            }
        }
    }
}

