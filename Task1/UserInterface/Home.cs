
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
        private Qustion QuestionWillDeleteOrEdit = null;
        private static List<Qustion> ListOfAllQuestion = new List<Qustion>();
        private delegate void SafeCallDelegate();
  
        public Home()
        {
            try
            {
                StartFunction();
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
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
                
                ListOfAllQuestion.Clear();
                ListOfAllQuestion = DataBaseConnections.GetQuestionFromDataBase();
                ShowData();
   
            }
            catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }

        }
        /// <summary>
        /// This Functions for thread to refresh data 
        /// </summary>
        private void NewThread()
        {
            try
            {
                Thread ThreadForRefresh = new Thread(RefreshData);
                ThreadForRefresh.IsBackground = true;
                ThreadForRefresh.Start();
            }catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        private void RefreshData()
        {
            try
            {
         
                if (IsHandleCreated)
                {
                    while (true)
                    {
                        var DelegateFunction = new SafeCallDelegate(GetListFromDataBase);
                        ListOfQuestion.Invoke(DelegateFunction);
                        int TimeSleep = Convert.ToInt32(ConfigurationManager.AppSettings["Thread.Sleep.Value"]); 
                        Thread.Sleep(TimeSleep);
                    }
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        private void GetListFromDataBase()
        {
            try
            {
                ListOfAllQuestion = Operation.GetQustion();
                ShowData();
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError); 
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
                    foreach (Qustion Temp in ListOfAllQuestion)
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
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
                return null;
            }
        }
        /// <summary>
        /// Show data function get the question from MyList and show it in datagridview
        /// </summary>
        private void ShowData()
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
            } catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
            ListOfQuestion.ClearSelection();
        }
        /// <summary>
        /// This Listener for Add button when press add button 
        /// </summary>
        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                ListOfQuestion.ClearSelection();
                QuestionsInformation QuestionsInformationPage = new QuestionsInformation(QuestionWillDeleteOrEdit, TypeOfChoice.Add);
                QuestionsInformationPage.ShowDialog();
                ListOfAllQuestion.Add(QuestionsInformation.ReturnNewQuestion);
                ShowData();
            } catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        /// <summary>
        /// This Listener for Edit button when press add button
        /// </summary>
        private void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                QuestionWillDeleteOrEdit = GetObjectSelect();
                Qustion OldObject = QuestionWillDeleteOrEdit; 
                ListOfQuestion.ClearSelection();
                if (QuestionWillDeleteOrEdit != null)
                {
                    
                    QuestionsInformation QuestionsInformationPage = new QuestionsInformation(QuestionWillDeleteOrEdit, TypeOfChoice.Edit);
                    QuestionsInformationPage.ShowDialog();
                    if (QuestionsInformation.ReturnNewQuestion != null)
                    {
                        ListOfAllQuestion.Remove(OldObject);
                        ListOfAllQuestion.Add(QuestionsInformation.ReturnNewQuestion);
                    }
                }
                else
                {
                    MessageBox.Show(Survey.Properties.Resource1.NoSelectItem, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }
        private void ListOfQuestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                QuestionWillDeleteOrEdit = GetObjectSelect();
                Qustion OldObject = QuestionWillDeleteOrEdit;
                ListOfQuestion.ClearSelection();
                if (QuestionWillDeleteOrEdit != null)
                {

                    QuestionsInformation QuestionsInformationPage = new QuestionsInformation(QuestionWillDeleteOrEdit, TypeOfChoice.Edit);
                    QuestionsInformationPage.ShowDialog();
                    if (QuestionsInformation.ReturnNewQuestion != null)
                    {
                        ListOfAllQuestion.Remove(OldObject);
                        ListOfAllQuestion.Add(QuestionsInformation.ReturnNewQuestion);
                    }
                }
                else
                {
                    MessageBox.Show(Survey.Properties.Resource1.NoSelectItem, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }

        }
        /// <summary>
        /// This Listener for delete button when press add button
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                QuestionWillDeleteOrEdit = GetObjectSelect();
                int Check = 0;
                if (QuestionWillDeleteOrEdit == null)
                {
                    MessageBox.Show(Survey.Properties.Resource1.NoSelectItem, GenralVariables.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show(Survey.Properties.Resource1.SureToDeleteMessage, GenralVariables.DELETE, MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        switch (QuestionWillDeleteOrEdit.TypeOfQuestion)
                        {
                            case TypeOfQuestion.Slider:
                                Slider SliderWillDelete = (Slider)QuestionWillDeleteOrEdit;
                                Check = Operation.DeleteQustion(SliderWillDelete);
                                ListOfAllQuestion.Remove(SliderWillDelete);
                                break;
                            case TypeOfQuestion.Smily:
                                Smiles SmileWillDelete = (Smiles)QuestionWillDeleteOrEdit;
                                Check = Operation.DeleteQustion(SmileWillDelete);
                                ListOfAllQuestion.Remove(SmileWillDelete);
                                break;
                            case TypeOfQuestion.Stars:
                                Stars StarWillDelete = (Stars)QuestionWillDeleteOrEdit;
                                Check = Operation.DeleteQustion(StarWillDelete);
                                ListOfAllQuestion.Remove(StarWillDelete);
                                break;
                        }
                        ListOfAllQuestion = Operation.GetQustion(); 
                        if (Check == 1)
                        {
                            MessageBox.Show(Survey.Properties.Resource1.TheQuestionDeleted);
                            ShowData();
                        }
                        else
                        {
                            MessageBox.Show(Survey.Properties.Resource1.TheQuestionNotDeleted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);

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
                    ListOfAllQuestion.Clear();
                }
                else
                {
                    GenralVariables.Languge = Langugaes.English.ToString();
                    ListOfAllQuestion.Clear();
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(GenralVariables.EnglishMark);
                }
                this.Controls.Clear();
                StartFunction();
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            try
            {
                NewThread();
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Survey.Properties.Resource1.MessageError);
            }
        }

        private void ListOfQuestion_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {


        }

        private void ListOfQuestion_Click(object sender, EventArgs e)
        {



        }

    }
}

