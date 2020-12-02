using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseConnection;
using Question;
using BaseLog;
using Global; 
namespace OperationManger
{
    public class Operation
    {
        public static Qustion AddQustion(Qustion Question)
        {
            try
            {
                switch (Question.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        return DataBaseConnections.AddNewSlider(Question);
                    case TypeOfQuestion.Smily:
                        return DataBaseConnections.AddNewSmile(Question);
                    default:
                        return DataBaseConnections.AddNewStar(Question);
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
                return null;
            }
        }
        public static Qustion EditQustion(Qustion Question)
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
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
                return null;
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
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
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
                StaticObjects.Erros.Log(ex);
                return null;
            }
        }

    }
}
