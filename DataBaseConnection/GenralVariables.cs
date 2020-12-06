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
        public static string ServerName = ConfigurationManager.AppSettings["Server"];
        public static string ProviderName = ConfigurationManager.AppSettings["ProviderName"];
        public static string Database = ConfigurationManager.AppSettings["Database"];
        public static string UserId = ConfigurationManager.AppSettings["UserId"];
        public static string Password = ConfigurationManager.AppSettings["Password"];
        public static string connectionString = "Data Source=" + ServerName + "; Initial Catalog =" + Database + "; User ID = " + UserId + "; Password=" + Password;
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
    }
}
