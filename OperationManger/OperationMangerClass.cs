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
        public  delegate void ShowDataDelegate();
        public static ShowDataDelegate PutListToShow;
        public static List<Qustion> ListOfAllQuestion = new List<Qustion>();
        public static List<Qustion> TempListOfQuestion = new List<Qustion>();
        private static int TimeForChangeData = Convert.ToInt32(ConfigurationManager.AppSettings["TimeDataChange"]);
        public static Boolean Flag = true; 
        public static void RefreshData()
        {
            try
            {
                Thread ThreadForRefresh = new Thread(GetDataAndCheckForRefresh);
                ThreadForRefresh.IsBackground = true;
                ThreadForRefresh.Start(); 

            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
            }
        }
        public static bool CheckFun(Qustion obj1, Qustion obj2)
        { 
            if (obj1.TypeOfQuestion == TypeOfQuestion.Slider &&obj2.TypeOfQuestion == TypeOfQuestion.Slider)
            {
                Slider SliderobjCompare = (Slider)obj1;
                Slider SliderobjCompare2 = (Slider)obj2;
                if (SliderobjCompare.Id == SliderobjCompare2.Id && SliderobjCompare2.Order == SliderobjCompare.Order && SliderobjCompare2.StartValue == SliderobjCompare.StartValue && SliderobjCompare2.StartCaption == SliderobjCompare.StartCaption && SliderobjCompare2.EndValue == SliderobjCompare.EndValue && SliderobjCompare2.EndCaption == SliderobjCompare.EndCaption)
                    return false;
                return true; 
            }if (obj1.TypeOfQuestion == TypeOfQuestion.Smily && obj2.TypeOfQuestion == TypeOfQuestion.Smily)
            {
                Smiles SmilesobjCompare = (Smiles)obj1;
                Smiles SmilesobjCompare2 = (Smiles)obj2;
                if (SmilesobjCompare.Id == SmilesobjCompare2.Id && SmilesobjCompare.Order == SmilesobjCompare2.Order && SmilesobjCompare.NumberOfSmiles == SmilesobjCompare2.NumberOfSmiles)
                    return false;
                return true; 
            }
            if (obj1.TypeOfQuestion == TypeOfQuestion.Stars && obj2.TypeOfQuestion == TypeOfQuestion.Stars)
            {
                Stars StarsobjCompare = (Stars)obj1;
                Stars StarsobjCompare2 = (Stars)obj2;
                if (StarsobjCompare.Id == StarsobjCompare2.Id && StarsobjCompare.Order == StarsobjCompare2.Order && StarsobjCompare2.NumberOfStars == StarsobjCompare.NumberOfStars)
                    return false;
                return true; 
            }
            return false; 
        }
        public static void GetDataAndCheckForRefresh()
        {

            try
            { 
                while (Flag)
                {
                    TempListOfQuestion.Clear();
                    DataBaseConnections.GetQuestionFromDataBase(ref TempListOfQuestion);
                    bool f = false;
                    if (TempListOfQuestion.Count == ListOfAllQuestion.Count)
                    {
                        f = true; 
                        for (int i = 0; i < TempListOfQuestion.Count; ++i)
                        {
                            if (!CheckFun(TempListOfQuestion[i], ListOfAllQuestion[i]))
                                f = false;
                        }
                    }
                    else
                    {
                        f = true; 
                    }
                    if (f)
                    {
                        ListOfAllQuestion = TempListOfQuestion.ToList();
                        PutListToShow();
                    }
                    Thread.Sleep(TimeForChangeData);
                }
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
            }
        }
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
                GenralVariables.Errors.Log(ex); 
                return GenralVariables.ErrorInMangerAdd;
            }
        }
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
                GenralVariables.Errors.Log(ex);
                return GenralVariables.ErrorInMangerEdit;
            }
        }
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
                GenralVariables.Errors.Log(ex);
                return GenralVariables.ErrorInMangerDelete; 
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
                return GenralVariables.ErrorInMangerGetQuestion;
            }
        }

    }
}
