using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseConnection;
using Question;
using BaseLog;
using System.Threading;
using System.Configuration;
using System.Web.Mvc;

namespace OperationManger
{
    public class Operation
    {

        public  delegate void ShowDataDelegate();
        public delegate ActionResult ShowDataDelegateMVC(string lang);
        public static ShowDataDelegate PutListToShow;
        public static ShowDataDelegateMVC PutListToShowMVC;
        public static Thread ThreadForRefresh; 
        public static List<Qustion> ListOfAllQuestion = new List<Qustion>();
        private static int TimeForChangeData = Convert.ToInt32(ConfigurationManager.AppSettings["TimeDataChange"]);
        public static Boolean EnableAutoRefrsh = true;
        public static bool IsDifferntList = false;
        public static string FunctionReload = "";

        private static bool IsNumber(string Number)
        {
            try
            {
                return int.TryParse(Number, out int N);
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return false;
            }
        }
        public static int CheckTheData(Qustion NewQuestion)
        {
            try
            {
                if (NewQuestion.NewText == "")
                {
                    return GenralVariables.TextIsEmpty;
                }
                else if (IsNumber(NewQuestion.NewText))
                {
                    return GenralVariables.TextIsNumber;
                }

                else if (NewQuestion.Order <= 0)
                {
                    return GenralVariables.OrderLessThanZero;
                }

                if (NewQuestion.TypeOfQuestion == TypeOfQuestion.Slider)
                {
                    Slider TempNewQuestion = (Slider)NewQuestion; 
                    if (TempNewQuestion.StartValue <= 0)
                    {
                        return GenralVariables.StartValueLessThanZero;
                    }
                    else if( TempNewQuestion.EndValue<= 0)
                    {
                        return GenralVariables.EndValueLessThanZero;
                    }
                    else if (TempNewQuestion.StartValue> 100)
                    {
                        return GenralVariables.StartValueGreaterThanOneHundered;
                    }
                    else if (TempNewQuestion.EndValue> 100)
                    {
                        return GenralVariables.EndValueGreaterThanOneHundered;
                    }
                    else if (TempNewQuestion.StartValue >= TempNewQuestion.EndValue)
                    {
                        return GenralVariables.StartValueGreaterThanEndValue;
                    }
                    else if (TempNewQuestion.StartCaption == "")
                    {

                        return GenralVariables.StartCaptionIsEmtpty;
                    }
                    else if (IsNumber(TempNewQuestion.StartCaption))
                    {
                        return GenralVariables.StartCaptionJustNumbers;
                    }
                    else if (TempNewQuestion.EndCaption == "")
                    {

                        return GenralVariables.EndCaptionIsEmtpty;
                    }
                    else if (IsNumber(TempNewQuestion.EndCaption))
                    {
                        return GenralVariables.EndCaptionJustNumbers;
                    }
                }
                else if (NewQuestion.TypeOfQuestion== TypeOfQuestion.Smily)
                {
                    Smiles TempNewQuestion = (Smiles)NewQuestion;
                    if (TempNewQuestion.NumberOfSmiles <= 1 || TempNewQuestion.NumberOfSmiles > 5)
                    {
                        return GenralVariables.NumberOfSmileInvalid;
                    }
                }
                else if (NewQuestion.TypeOfQuestion == TypeOfQuestion.Stars)
                {
                    Stars TempNewQuestion = (Stars)NewQuestion;
                    if (TempNewQuestion.NumberOfStars <= 0 || TempNewQuestion.NumberOfStars > 10)
                    {
                        return GenralVariables.NumberOfStarsInvalid;
                    }
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInManger;
            }
            return GenralVariables.Succeeded;
        }

        public static string CheckMessageError(int ResultNumber)
        {
            try
            {
                if (ResultNumber == GenralVariables.Succeeded)
                    return OperationManger.Resources.Messages.DataIsEnterd;
                else if (ResultNumber == GenralVariables.TextIsEmpty)
                    return OperationManger.Resources.Messages.QuestionIsEmptyMessage;
                else if (ResultNumber == GenralVariables.TextIsNumber)
                    return OperationManger.Resources.Messages.QuestionIsJustANumberMessage;
                else if (ResultNumber == GenralVariables.OrderLessThanZero)
                    return OperationManger.Resources.Messages.NewOrderLessThanZeroMessage;
                else if (ResultNumber == GenralVariables.StartValueLessThanZero)
                    return OperationManger.Resources.Messages.StartValueLessThanZeroMessage;
                else if (ResultNumber == GenralVariables.StartValueGreaterThanOneHundered)
                    return OperationManger.Resources.Messages.StartValueGreaterThanOneHundredMessage;
                else if (ResultNumber == GenralVariables.EndValueGreaterThanOneHundered)
                    return OperationManger.Resources.Messages.EndValueGreaterThanOneHundredMessage;
                else if (ResultNumber == GenralVariables.EndValueLessThanZero)
                    return OperationManger.Resources.Messages.EndValueLessThanZeroMessage;
                else if (ResultNumber == GenralVariables.StartValueGreaterThanEndValue)
                    return OperationManger.Resources.Messages.TheEndValueSholudGreaterThanStartValueMessage;
                else if (ResultNumber == GenralVariables.StartCaptionJustNumbers)
                    return OperationManger.Resources.Messages.StartCaptionJustNumberMessage;
                else if (ResultNumber == GenralVariables.EndCaptionJustNumbers)
                    return OperationManger.Resources.Messages.EndCaptionJustNumberMessage;
                else if (ResultNumber == GenralVariables.EndCaptionIsEmtpty)
                    return OperationManger.Resources.Messages.EndCaptionEmptyMessage;
                else if (ResultNumber == GenralVariables.StartCaptionIsEmtpty)
                    return OperationManger.Resources.Messages.StartCaptionEmptyMessage;
                else if (ResultNumber == GenralVariables.NumberOfSmileInvalid)
                    return OperationManger.Resources.Messages.NumberOfSmileBetweenFiveAndTow;
                else if (ResultNumber == GenralVariables.NumberOfStarsInvalid)
                    return OperationManger.Resources.Messages.NumberOfStrasBetweenTenAndOne;
                return OperationManger.Resources.Messages.ErrorManger;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return OperationManger.Resources.Messages.ErrorManger; 
            }

        }
        /// <summary>
        /// This function for start thread to call function GetAllQuestionAndCheckForRefresh
        /// </summary>
        public  static void RefreshData()
        {
            try
            {
                ThreadForRefresh = new Thread(CheckForRefresh);
                ThreadForRefresh.IsBackground = true;
                ThreadForRefresh.Start();

            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        /// <summary>
        /// this for check if my tow object are equals or not
        /// return true if objects are not equal and false if objects are equal for refresh data
        /// </summary>
        public static void CheckForRefresh()
        {
            try
            {
                List<Qustion> TempListOfQuestion = new List<Qustion>(); 
                while (EnableAutoRefrsh)
                {
                    TempListOfQuestion.Clear();
                    DataBaseConnections.GetQuestionFromDataBase(ref TempListOfQuestion);
                    IsDifferntList = false;
                    if (TempListOfQuestion.Count == ListOfAllQuestion.Count)
                    {
                        for (int i = 0; i < TempListOfQuestion.Count; ++i)
                        {
                            if (TempListOfQuestion[i].Equals(ListOfAllQuestion[i]))
                            {
                                IsDifferntList = true;
                                break; 
                            }
                        }
                    }
                    else
                    {
                        IsDifferntList = true; 
                    }
                    if (IsDifferntList)
                    {
                        ListOfAllQuestion = TempListOfQuestion.ToList();

                        if (PutListToShow != null) {
                            PutListToShow(); 
                        } 
                    }
                    Thread.Sleep(TimeForChangeData);
                    FunctionReload = ""; 

                }
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }

        /// <summary>
        /// Add question will receive the data from UI and send it to database and return 0 
        /// if add operation is succeeded
        /// </summary>
        public static int AddQustion(Qustion NewQuestion)
        {
            try
            {
                switch (NewQuestion.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        int ResultOfAddSlider = DataBaseConnections.AddNewSlider(NewQuestion);
                        return ResultOfAddSlider; 
                    case TypeOfQuestion.Smily:
                        int ResultOfAddSmile  =  DataBaseConnections.AddNewSmile(NewQuestion);
                        return ResultOfAddSmile;
                    case TypeOfQuestion.Stars:
                        int ResultOfAddStar=  DataBaseConnections.AddNewStar(NewQuestion);
                        return ResultOfAddStar;
                    default:
                        return GenralVariables.ErrorInManger; 
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message); 
                return GenralVariables.ErrorInMangerAdd;
            }
        }


        /// <summary>
        /// Edit question will receive new data for edit from UI and send it to database
        /// and delete 0 if succeeded
        /// </summary>
        public static int EditQustion(Qustion Question)
        {
            try
            {
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        int ResultOfEditSlider = DataBaseConnections.EditSlider(Question);
                        return ResultOfEditSlider;
                    case TypeOfQuestion.Smily:
                        int ResultOfEditSmile= DataBaseConnections.EditSmile(Question);
                        return ResultOfEditSmile;
                    case TypeOfQuestion.Stars:
                        int ResultOfStar= DataBaseConnections.EditStar(Question);
                        return ResultOfStar;
                    default:
                        return GenralVariables.ErrorInManger;
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInMangerEdit;
            }
        }


        /// <summary>
        /// delete question will receive question from UI and send it to database to delete
        /// and return 0 if delete operation is succeeded.
        /// </summary>
        public static int DeleteQustion(Qustion Question)
        {
            try
            {
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        int ResultOfDeleteSlider = DataBaseConnections.DeleteSlider(Question);
                        return ResultOfDeleteSlider;
                    case TypeOfQuestion.Smily:
                        int ResultOfDeleteSmile = DataBaseConnections.DeleteSmile(Question);
                        return ResultOfDeleteSmile;
                    case TypeOfQuestion.Stars:
                        int ResultOfDeleteStar = DataBaseConnections.DeleteStar(Question);
                        return ResultOfDeleteStar;
                    default:
                        return GenralVariables.ErrorInManger;
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInMangerDelete; 
            }
        }


        /// <summary>
        /// This function to get all question from database and put the questions in my list by refrence
        /// and return 0 if getquestion is succeeded
        /// </summary>
        public static int GetQustion(ref List<Qustion> ListOfAllQuestion)
        {
            try
            {
                ListOfAllQuestion.Clear(); 
                  return DataBaseConnections.GetQuestionFromDataBase(ref ListOfAllQuestion);
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInMangerGetQuestion;
            }
        }
        public static IEnumerable<Qustion> GetAllQuestion()
        {
            ListOfAllQuestion.Clear(); 
            GetQustion(ref ListOfAllQuestion);
            return ListOfAllQuestion; 
        }

        public static Qustion SelectById (int Id)
        {
            foreach (Qustion TempForSelect in ListOfAllQuestion)
            {
                if (Id == TempForSelect.Id)
                    return TempForSelect; 
            }
            return null; 
        }

    }
}
