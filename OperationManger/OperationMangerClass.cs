using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseConnection;
using Question;
using BaseLog;
using System.Threading;
using System.Windows.Forms;
using System.Configuration; 
namespace OperationManger
{
    public class Operation
    {
        private delegate int SafeCallDelegate(ref List<Qustion> ListOfQuestion);
        
        public static void RefrshData(ref DataGridView ListOfQuestion)
        {
            DataGridView ListOfQuestionTemp = ListOfQuestion; 
            Thread ThreadForRefresh = new Thread(delegate () { GetDatafORefresh(ref ListOfQuestionTemp); });
            ThreadForRefresh.IsBackground = true;
            ThreadForRefresh.Start();
        }
        private static void GetDatafORefresh(ref DataGridView ListOfQuestion)
        {
            while (true)
            {
                List<Qustion> Temp = new List<Qustion>(); 
                var DelegateFunction = new SafeCallDelegate(DataBaseConnections.GetQuestionFromDataBase);
                ListOfQuestion.Invoke(DelegateFunction,Temp);
                ListOfQuestion.Rows.Clear();
                foreach (Qustion temp in Temp)
                {
                    if (temp != null)
                    {
                        int Index = ListOfQuestion.Rows.Add();
                        ListOfQuestion.Rows[Index].Cells[0].Value = temp.NewText;
                        ListOfQuestion.Rows[Index].Cells[2].Value = temp.Order;
                        ListOfQuestion.Rows[Index].Cells[1].Value = temp.TypeOfQuestion;
                    }
                }
                int TimeSleep = Convert.ToInt32(ConfigurationManager.AppSettings["Thread.Sleep.Value"]);
                Thread.Sleep(TimeSleep);
            }
        }
        public static void RefreshData()
        {
            try
            {
                Thread ThreadForRefresh = new Thread(RefreshData);
                ThreadForRefresh.IsBackground = true;
                ThreadForRefresh.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resource1.ErrorModels);
                GenralVariables.Errors.Log(ex);
            }
        }
        public static int AddQustion(Qustion NewQuestion)
        {
            try
            {
                int result = GenralVariables.NoData; 
                switch (NewQuestion.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        result = DataBaseConnections.AddNewSlider(NewQuestion);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Smily:
                        result =  DataBaseConnections.AddNewSmile(NewQuestion);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Stars:
                        result=  DataBaseConnections.AddNewStar(NewQuestion);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    default:
                        NewQuestion = null; 
                        return GenralVariables.NoData; 
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Properties.Resource1.ErrorModels); 
                return GenralVariables.Error;
            }
        }
        public static int EditQustion(Qustion Question)
        {
            try
            {
                int result = GenralVariables.NoData;
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        result = DataBaseConnections.EditSlider(Question);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Smily:
                        result= DataBaseConnections.EditSmile(Question);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Stars:
                        result= DataBaseConnections.EditStar(Question);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    default:
                        return GenralVariables.NoData;
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Properties.Resource1.ErrorModels);
                return GenralVariables.Error;
            }
        }
        public static int DeleteQustion(Qustion Question)
        {
            try
            {
                int result = GenralVariables.NoData;
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        result = DataBaseConnections.DeleteSlider(Question);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Smily:
                        result = DataBaseConnections.DeleteSmile(Question);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    case TypeOfQuestion.Stars:
                        result = DataBaseConnections.DeleteStar(Question);
                        if (result == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        return GenralVariables.NoData;
                    default:
                        return GenralVariables.NoData;
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Properties.Resource1.ErrorModels);
                return GenralVariables.Error; 
            }
        }
        public static int GetQustion(ref List<Qustion> TempList)
        {
            try
            {
                  return DataBaseConnections.GetQuestionFromDataBase(ref TempList);
                  

            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(Properties.Resource1.ErrorModels);
                return GenralVariables.Error;
            }
        }

    }
}
