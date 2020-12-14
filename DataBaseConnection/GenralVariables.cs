using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLog; 
namespace DataBaseConnection
{
    public class GenralVariables
    {
        public static Logger Errors = new Logger();
        
        /// <summary>
        /// This strings attrubites for connection string 
        /// and concatnate and bulid connection string 
        /// </summary>
        public static string ServerName;
        public static string ProviderName;
        public static string Database;
        public static string UserName;
        public static string Password;
        public static string ConnectionString; 
        /// <summary>
        /// This string for value to add or edit or delete in database opeartions
        /// </summary>
        public const string NewQuestionText = "@Qustions_text";
        public const string NewQuestionType = "@Type_Of_Qustion";
        public const string NewQuestionOrder = "@Qustion_order";
        public const string NewStartValue = "@Start_Value";
        public const string NewEndValue = "@End_Value";
        public const string NewStartValueCaption = "@Start_Value_Cap";
        public const string NewEndValueCaption = "@End_Value_Cap";
        public const string NewNumberOfSmily = "@Number_of_smily";
        public const string QustionIdDataBase = "@Qus_ID";
        public const string NewNumberOfStars = "@Number_Of_Stars";
        public const string IdQuestion = "ID";
        public const string IdQuestionWithAt = "@ID";
        public const string QustionsTetForShow = "Qustions_text";
        public const string TypeOfQustionForShow = "Type_Of_Qustion";
        public const string QustionOrderForShow = "Qustion_order";
        /// <summary>
        /// This string for SQL statement in database (INSERT,UPDATE,DELETE,SELECT)
        /// </summary>
        public const string JoinSmileAndQustion = "select Qustions.ID,Smily.ID ,Qustions.Qustions_text,Qustions.Qustion_order,Smily.Number_of_smily from Qustions INNER JOIN Smily ON Smily.Qus_ID = Qustions.ID";
        public const string JoinSliderAndQuestion = "select Qustions.ID, Slider.ID ,Qustions.Qustions_text,Qustions.Qustion_order,Slider.Start_Value,Slider.End_Value,Slider.Start_Value_Cap,Slider.End_Value_Cap from Qustions INNER JOIN Slider ON Slider.Qus_ID = Qustions.ID;";
        public const string JoinStarsAndQuestion = "select Qustions.ID,Stars.ID ,Qustions.Qustions_text,Qustions.Qustion_order,Stars.Number_Of_Stars from Qustions INNER JOIN Stars ON Stars.Qus_ID = Qustions.ID;";
        public const string ProcdureQuestionSelectForMax = "select max(ID) as ID from Qustions";
        public const string DeleteStarString = "DELETE FROM Stars Where ID = @ID;";
        public const string UpdateSlider = "UPDATE Slider SET Start_Value =@Start_value, End_Value = @End_Value,Start_Value_Cap =@Start_Value_Cap, End_Value_Cap = @End_Value_Cap where Qus_ID = @ID;";
        public const string UpdateSmile = "UPDATE Smily SET Number_of_smily = @Number_of_smily where Qus_ID = @ID;";
        public const string UpdateStar = "UPDATE Stars SET Number_Of_Stars = @Number_Of_Stars where Qus_ID = @ID;";
        public const string DeleteSliderString = "DELETE FROM Slider Where ID = @ID;";
        public const string DeleteSmilyString = "DELETE FROM Smily Where ID = @ID;";
        public const string InsertInSlider = "INSERT INTO Slider(Start_Value,End_Value,Start_Value_Cap,End_Value_Cap,Qus_ID) VALUES(@Start_Value,@End_Value, @Start_Value_Cap,@End_Value_Cap,@Qus_ID);";
        public const string InsertInSmile = "INSERT INTO Smily(Number_of_smily,Qus_ID) VALUES(@Number_of_smily,@Qus_ID);";
        public const string InsertInStar = "INSERT INTO Stars(Number_Of_Stars,Qus_ID) VALUES(@Number_Of_Stars,@Qus_ID);";
        public const string DeleteQustionAttrubites = "DELETE FROM Qustions Where ID = @ID;";
        public const string UpdateQuestion = "update Qustions Set Qustions_text = @Qustions_text, Qustion_order=@Qustion_order where ID = @ID;";
        public const string InsertIntoQustion = "INSERT INTO Qustions(Qustions_text, Type_Of_Qustion,Qustion_order) VALUES(@Qustions_text,@Type_Of_Qustion,@Qustion_order);";
        public const string SelectMaxId = "select max(ID) as ID from ";


        /// <summary>
        /// This constant string for errors will write in log file
        /// </summary>
        public const string ErrorDeleteQuestionInLog = "Warning.... you can't delete this questions maybe already deleted from anthor application";
        public const string ErrorDeleteSliderInLog = "Warning.... you can't delete this question slider maybe already deleted from anthor application";
        public const string ErrorDeleteSmileInLog = "Warning.... you can't delete this question smile maybe already deleted from anthor application";
        public const string ErrorDeleteStarInLog = "Warning.... you can't delete this question star maybe already deleted from anthor application";
        public const string ErrorEditStarInLog = "Warning.... you can't edit this question star maybe already deleted from anthor application";
        public const string ErrorEditSmileInLog = "Warning.... you can't edit this question smile maybe already deleted from anthor application";
        public const string ErrorEditSliderInLog = "Warning.... you can't edit this question slider maybe already deleted from anthor application";
        public const string ErrorEditQuestionInLog = "Warning.... you can't edit this question maybe already deleted from anthor application";
        public const string ErrorAddStarInLog = "Warning.... you can't add this question star";
        public const string ErrorAddSmileInLog = "Warning.... you can't add this question smile";
        public const string ErrorAddSliderInLog = "Warning.... you can't add this question slider";
        public const string ErrorAddQuestionInLog = "Warning you can't add this question in database";







        /// <summary>
        /// This is return vriable 0 = Succeeded , anthor number is error
        /// </summary>
        public const int Succeeded = 0;
        public const int ErrorInDataBase = 500; 
        public const int ErrorConnectionString = 501;
        public const int ErrorInAddQuestion = 502;
        public const int ErrorInSelectionQuestion = 503;
        public const int ErrorInEditQuestion = 504;
        public const int ErrorInDeleteQuestion = 505;
        public const int ErrorInGetQuestion = 506;
        public const int ErrorInOperation = 507; 


    }
}
