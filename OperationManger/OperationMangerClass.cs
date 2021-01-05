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
        public static bool IsDifferntList = false;
        public static Boolean EnableAutoRefrsh = true;
        public static Dictionary<string,bool> SessionFlags = new Dictionary<string, bool>();
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
                switch (ResultNumber)
                {
                    case GenralVariables.Succeeded:
                        return OperationManger.Resources.Messages.DataIsEnterd;
                    case GenralVariables.TextIsEmpty:
                        return OperationManger.Resources.Messages.QuestionIsEmptyMessage;
                    case GenralVariables.TextIsNumber:
                        return OperationManger.Resources.Messages.QuestionIsJustANumberMessage;
                    case GenralVariables.OrderLessThanZero:
                        return OperationManger.Resources.Messages.NewOrderLessThanZeroMessage;
                    case GenralVariables.StartValueLessThanZero:
                        return OperationManger.Resources.Messages.StartValueLessThanZeroMessage;
                    case GenralVariables.StartValueGreaterThanOneHundered:
                        return OperationManger.Resources.Messages.StartValueGreaterThanOneHundredMessage;
                    case GenralVariables.EndValueGreaterThanOneHundered:
                        return OperationManger.Resources.Messages.EndValueGreaterThanOneHundredMessage;
                    case GenralVariables.EndValueLessThanZero:
                        return OperationManger.Resources.Messages.EndValueLessThanZeroMessage;
                    case GenralVariables.StartValueGreaterThanEndValue:
                        return OperationManger.Resources.Messages.TheEndValueSholudGreaterThanStartValueMessage;
                    case GenralVariables.StartCaptionJustNumbers:
                        return OperationManger.Resources.Messages.StartCaptionJustNumberMessage;
                    case GenralVariables.EndCaptionJustNumbers:
                        return OperationManger.Resources.Messages.EndCaptionJustNumberMessage;
                    case GenralVariables.EndCaptionIsEmtpty:
                        return OperationManger.Resources.Messages.EndCaptionEmptyMessage;
                    case GenralVariables.StartCaptionIsEmtpty:
                        return OperationManger.Resources.Messages.EndCaptionEmptyMessage;
                    case GenralVariables.NumberOfSmileInvalid:
                        return OperationManger.Resources.Messages.NumberOfSmileBetweenFiveAndTow;
                    case GenralVariables.NumberOfStarsInvalid:
                        return OperationManger.Resources.Messages.NumberOfStrasBetweenTenAndOne;
                    case GenralVariables.ErrorInManger:
                        return OperationManger.Resources.Messages.ErrorManger;
                    case GenralVariables.ErrorInMangerAdd:
                        return OperationManger.Resources.Messages.ErrorMangerAddQuestion;
                    case GenralVariables.ErrorInMangerEdit:
                        return OperationManger.Resources.Messages.ErrorMangerEditQuestion;
                    case GenralVariables.ErrorInMangerDelete:
                        return OperationManger.Resources.Messages.ErrorMangerDeleteQuestion;
                    case GenralVariables.ErrorInMangerGetQuestion:
                        return OperationManger.Resources.Messages.ErrorMangerGetQuestion;
                    case DataBaseConnection.GenralVariables.ErrorInDataBase:
                        return OperationManger.Resources.Messages.ErrorDataBase;
                    case DataBaseConnection.GenralVariables.ErrorConnectionString:
                        return OperationManger.Resources.Messages.ErrorDataBaseConnectionString;
                    case DataBaseConnection.GenralVariables.ErrorInAddQuestion:
                        return OperationManger.Resources.Messages.ErrorDataBaseAddQuestion;
                    case DataBaseConnection.GenralVariables.ErrorInSelectionQuestion:
                        return OperationManger.Resources.Messages.ErrorDataBaseSelectQuestion;
                    case DataBaseConnection.GenralVariables.ErrorInEditQuestion:
                        return OperationManger.Resources.Messages.ErrorDataBaseEditQuestion;
                    case DataBaseConnection.GenralVariables.ErrorInDeleteQuestion:
                        return OperationManger.Resources.Messages.ErrorDataBaseDeleteQuestion;
                    case DataBaseConnection.GenralVariables.ErrorInGetQuestion:
                        return OperationManger.Resources.Messages.ErrorDataBaseGetQuestion;
                    case DataBaseConnection.GenralVariables.ErrorInOperation:
                        return OperationManger.Resources.Messages.ErrorInOperation;
                    default:
                        return OperationManger.Resources.Messages.ErrorManger;

                }
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
                        foreach (var key in SessionFlags.Keys.ToList())
                        {
                            SessionFlags[key] = true; 
                        }

                        if (PutListToShow != null) {
                            PutListToShow(); 
                        } 
                    }
                    Thread.Sleep(TimeForChangeData); 

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
            try
            {
                ListOfAllQuestion.Clear();
                GetQustion(ref ListOfAllQuestion);
                return ListOfAllQuestion;
            }catch(Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return ListOfAllQuestion;
            }
        }

        public static Qustion SelectById (int Id)
        {
            try
            {
                foreach (Qustion TempForSelect in ListOfAllQuestion)
                {
                    if (Id == TempForSelect.Id)
                        return TempForSelect;
                }
                return null; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return null;
            }
        }

    }
}
