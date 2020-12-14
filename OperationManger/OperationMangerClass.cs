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
namespace OperationManger
{
    public class Operation
    {

        public  delegate void ShowDataDelegate();
        public static ShowDataDelegate PutListToShow;
        public static List<Qustion> ListOfAllQuestion = new List<Qustion>();
        private static int TimeForChangeData = Convert.ToInt32(ConfigurationManager.AppSettings["TimeDataChange"]);
        public static Boolean StillRefresh = true;


        /// <summary>
        /// This function for start thread to call function GetAllQuestionAndCheckForRefresh
        /// </summary>
        public static void RefreshData()
        {
            try
            {
                Thread ThreadForRefresh = new Thread(GetAllQuestionAndCheckForRefresh);
                ThreadForRefresh.IsBackground = true;
                ThreadForRefresh.Start(); 

            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
            }
        }
        /// <summary>
        /// this for check if my tow object are equals or not
        /// return true if objects are not equal and false if objects are equal for refresh data
        /// </summary>
        public static bool IsEqual(Qustion OldObject, Qustion NewObject)
        {
            try
            {

                if (OldObject.TypeOfQuestion == TypeOfQuestion.Slider && NewObject.TypeOfQuestion == TypeOfQuestion.Slider)
                {
                    Slider SliderobjCompareOld = (Slider)OldObject;
                    Slider SliderobjCompareNew = (Slider)NewObject;
                    if (SliderobjCompareOld.Id == SliderobjCompareNew.Id && SliderobjCompareNew.Order == SliderobjCompareOld.Order && SliderobjCompareNew.StartValue == SliderobjCompareOld.StartValue && SliderobjCompareNew.StartCaption == SliderobjCompareOld.StartCaption && SliderobjCompareNew.EndValue == SliderobjCompareOld.EndValue && SliderobjCompareNew.EndCaption == SliderobjCompareOld.EndCaption && SliderobjCompareNew.NewText == SliderobjCompareOld.NewText)
                        return false;
                    return true;
                }
                if (OldObject.TypeOfQuestion == TypeOfQuestion.Smily && NewObject.TypeOfQuestion == TypeOfQuestion.Smily)
                {

                    Smiles SmilesobjCompareOld = (Smiles)OldObject;
                    Smiles SmilesobjCompareNew = (Smiles)NewObject;
                    if (SmilesobjCompareOld.Id == SmilesobjCompareNew.Id && SmilesobjCompareOld.Order == SmilesobjCompareNew.Order && SmilesobjCompareOld.NumberOfSmiles == SmilesobjCompareNew.NumberOfSmiles && SmilesobjCompareNew.NewText.Equals(SmilesobjCompareOld.NewText))
                        return false;
                    return true;
                }
                if (OldObject.TypeOfQuestion == TypeOfQuestion.Stars && NewObject.TypeOfQuestion == TypeOfQuestion.Stars)
                {
                    Stars StarsobjCompareOld = (Stars)OldObject;
                    Stars StarsobjCompareNew = (Stars)NewObject;
                    if (StarsobjCompareOld.Id == StarsobjCompareNew.Id && StarsobjCompareOld.Order == StarsobjCompareNew.Order && StarsobjCompareNew.NumberOfStars == StarsobjCompareOld.NumberOfStars && StarsobjCompareNew.NewText == StarsobjCompareOld.NewText)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return false; 
            }
        }


        /// <summary>
        /// this function for refresh my list if my list is changed 
        /// using IsEqual function to check if my list is changed or not.
        /// </summary>
        public static void GetAllQuestionAndCheckForRefresh()
        {
            try
            {
                List<Qustion> TempListOfQuestion = new List<Qustion>(); 
                while (StillRefresh)
                {
                    TempListOfQuestion.Clear();
                    DataBaseConnections.GetQuestionFromDataBase(ref TempListOfQuestion);
                    bool IsDifferntList = false;
                    if (TempListOfQuestion.Count == ListOfAllQuestion.Count)
                    {
                        for (int i = 0; i < TempListOfQuestion.Count; ++i)
                        {
                            if (IsEqual(TempListOfQuestion[i], ListOfAllQuestion[i]))
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
                        PutListToShow();
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
                  return DataBaseConnections.GetQuestionFromDataBase(ref ListOfAllQuestion);
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInMangerGetQuestion;
            }
        }

    }
}
