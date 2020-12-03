using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseConnection;
using Question;
using BaseLog;
 
namespace OperationManger
{
    public class Operation
    {
        public static int  AddQustion(Qustion Question,out Qustion NewQuestion)
        {
            try
            {
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        return DataBaseConnections.AddNewSlider(Question,out NewQuestion);
                    case TypeOfQuestion.Smily:
                        return DataBaseConnections.AddNewSmile(Question, out NewQuestion);
                    default:
                        return DataBaseConnections.AddNewStar(Question, out NewQuestion);
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                NewQuestion = null; 
                return 0;
            }
        }
        public static int EditQustion(Qustion Question)
        {
            try
            {
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        return DataBaseConnections.EditSlider(Question);
                    case TypeOfQuestion.Smily:
                        return DataBaseConnections.EditSmile(Question);
                    default:
                        return DataBaseConnections.EditStar(Question);
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int DeleteQustion(Qustion Question)
        {
            try
            {
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        return DataBaseConnections.DeleteSlider(Question);
                    case TypeOfQuestion.Smily:
                        return DataBaseConnections.DeleteSmile(Question);
                    default:
                        return DataBaseConnections.DeleteStar(Question);
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static List<Qustion> GetQustion()
        {
            try
            {
                return DataBaseConnections.GetQuestionFromDataBase();
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return null;
            }
        }

    }
}
