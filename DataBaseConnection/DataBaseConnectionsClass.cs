using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Question;

using BaseLog; 
namespace DataBaseConnection
{
    /// <summary>
    /// This Class For Data Base and get Connection with data base  and actions 
    /// with database using only this class
    /// </summary>
    public class DataBaseConnections
    {
        /// <summary>
        /// This strings attrubites for connection string 
        /// and concatnate and bulid connection string 
        /// </summary>
        private static string ServerName = ConfigurationManager.AppSettings["Server"];
        private static string ProviderName = ConfigurationManager.AppSettings["ProviderName"];
        private static string Database = ConfigurationManager.AppSettings["Database"];
        private static string UserId = ConfigurationManager.AppSettings["UserId"];
        private static string Password = ConfigurationManager.AppSettings["Password"];
        private static string connectionString = "Data Source=" + ServerName + "; Initial Catalog =" + Database + "; User ID = " + UserId + "; Password=" + Password;
        /// <summary>
        /// This string for value to add or edit or delete in database opeartions
        /// </summary>
        private const string NewQuestionText = "@Qustions_text";
        private const string NewQuestionType = "@Type_Of_Qustion";
        private const string NewQuestionOrder = "@Qustion_order";
        private const string NewStartValue = "@Start_Value";
        private const string NewEndValue = "@End_Value";
        private const string NewStartValueCaption = "@Start_Value_Cap";
        private const string NewEndValueCaption = "@End_Value_Cap";
        private const string NewNumberOfSmily = "@Number_of_smily";
        private const string QustionIdDataBase = "@Qus_ID";
        private const string NewNumberOfStars = "@Number_Of_Stars";
        private const string IdQuestion = "ID";
        private const string IdQuestionWithAt = "@ID";
        private const string QustionsTetForShow = "Qustions_text";
        private const string TypeOfQustionForShow = "Type_Of_Qustion";
        private const string QustionOrderForShow = "Qustion_order";
        /// <summary>
        /// This string for SQL statement in database (INSERT,UPDATE,DELETE,SELECT)
        /// </summary>
        private const string JoinSmileAndQustion = "select Qustions.ID,Smily.ID ,Qustions.Qustions_text,Qustions.Qustion_order,Smily.Number_of_smily from Qustions INNER JOIN Smily ON Smily.Qus_ID = Qustions.ID";
        private const string JoinSliderAndQuestion = "select Qustions.ID, Slider.ID ,Qustions.Qustions_text,Qustions.Qustion_order,Slider.Start_Value,Slider.End_Value,Slider.Start_Value_Cap,Slider.End_Value_Cap from Qustions INNER JOIN Slider ON Slider.Qus_ID = Qustions.ID;";
        private const string JoinStarsAndQuestion = "select Qustions.ID,Stars.ID ,Qustions.Qustions_text,Qustions.Qustion_order,Stars.Number_Of_Stars from Qustions INNER JOIN Stars ON Stars.Qus_ID = Qustions.ID;";
        private const string ProcdureQuestionSelectForMax = "select max(ID) as ID from Qustions";
        private const string DeleteStarString = "DELETE FROM Stars Where ID = @ID;";
        private const string UpdateSlider = "UPDATE Slider SET Start_Value =@Start_value, End_Value = @End_Value,Start_Value_Cap =@Start_Value_Cap, End_Value_Cap = @End_Value_Cap where Qus_ID = @ID;";
        private const string UpdateSmile = "UPDATE Smily SET Number_of_smily = @Number_of_smily where Qus_ID = @ID;";
        private const string UpdateStar = "UPDATE Stars SET Number_Of_Stars = @Number_Of_Stars where Qus_ID = @ID;";
        private const string DeleteSliderString = "DELETE FROM Slider Where ID = @ID;";
        private const string DeleteSmilyString = "DELETE FROM Smily Where ID = @ID;";
        private const string InsertInSlider = "INSERT INTO Slider(Start_Value,End_Value,Start_Value_Cap,End_Value_Cap,Qus_ID) VALUES(@Start_Value,@End_Value, @Start_Value_Cap,@End_Value_Cap,@Qus_ID);";
        private const string InsertInSmile = "INSERT INTO Smily(Number_of_smily,Qus_ID) VALUES(@Number_of_smily,@Qus_ID);";
        private const string InsertInStar = "INSERT INTO Stars(Number_Of_Stars,Qus_ID) VALUES(@Number_Of_Stars,@Qus_ID);";
        private const string DeleteQustionAttrubites = "DELETE FROM Qustions Where ID = @ID;";
        private const string UpdateQuestion = "update Qustions Set Qustions_text = @Qustions_text, Qustion_order=@Qustion_order where ID = @ID;";
        private const string InsertIntoQustion = "INSERT INTO Qustions(Qustions_text, Type_Of_Qustion,Qustion_order) VALUES(@Qustions_text,@Type_Of_Qustion,@Qustion_order);";
        /// <summary>
        /// This functions for add or edit and delete and select from database,
        /// And connections in database 
        /// </summary>
        /// <summary>
        /// For Add Qustion return new OBJECT OF QUESTION
        /// </summary>
        private static int AddQustionInDataBase(Qustion Question,out int Id)
        {
            
            try
            {
                 Id = -1;
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand ComandForInsertQustion = new SqlCommand(InsertIntoQustion, Connection);
                    ComandForInsertQustion.CommandText = InsertIntoQustion;
                    ComandForInsertQustion.Parameters.AddWithValue(NewQuestionText, Question.NewText);
                    ComandForInsertQustion.Parameters.AddWithValue(NewQuestionType, Question.TypeOfQuestion);
                    ComandForInsertQustion.Parameters.AddWithValue(NewQuestionOrder, Question.Order);
                    ComandForInsertQustion.Connection.Open();
                    ComandForInsertQustion.ExecuteNonQuery();
                }
                Id = SelectIdType(TypeOfQuestion.Qustions); 
                return 1; 
            }
            catch (Exception ex)
            {
                Id = -1;
                Qustion.Errors.Log(ex);
                return 0; 
            }
            
        }
        private static int SelectIdType (TypeOfQuestion TypeOfQustion)
        {
            try
            {
                string SelectIdTypeStatment = "select max(ID) as ID from " + TypeOfQustion.ToString();
                int Id = -1; 
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandForSelectIdType = new SqlCommand(SelectIdTypeStatment, Connection);
                    CommandForSelectIdType.Connection.Open();
                    SqlDataReader Reader = CommandForSelectIdType.ExecuteReader();
                    while (Reader.Read())
                        Id = Convert.ToInt32(Reader[IdQuestion]);
                    Reader.Close();
                }
                return Id; 
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int AddNewSlider(Qustion Qustion,out Qustion NewQuestion)
        {
            try
            {
                Slider SliderQuestion = (Slider)Qustion;
                int Id; 
                if (AddQustionInDataBase(SliderQuestion, out Id)!=0)
                {
                    using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                    {
                        SqlCommand CommandForInsertSlider = new SqlCommand(InsertInSlider, Connection);
                        CommandForInsertSlider.Parameters.AddWithValue(NewStartValue, SliderQuestion.StartValue);
                        CommandForInsertSlider.Parameters.AddWithValue(NewEndValue, SliderQuestion.EndValue);
                        CommandForInsertSlider.Parameters.AddWithValue(NewStartValueCaption, SliderQuestion.StartCaption);
                        CommandForInsertSlider.Parameters.AddWithValue(NewEndValueCaption, SliderQuestion.EndCaption);
                        CommandForInsertSlider.Parameters.AddWithValue(QustionIdDataBase, Id);
                        SliderQuestion.Id = Id;
                        CommandForInsertSlider.Connection.Open();
                        CommandForInsertSlider.ExecuteNonQuery();
                        int IdForType = SelectIdType(TypeOfQuestion.Slider); 
                        if (IdForType > 0)
                            SliderQuestion.IdForType = IdForType; 
                        NewQuestion = SliderQuestion;  
                        return 1; 
                    }

                }
                NewQuestion = null; 
                return 0; 
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                NewQuestion = null;
                return 0;
            }
        }
        public static int AddNewSmile(Qustion Qustion,out Qustion NewQuestion)
        {
            try
            {
                int Id = -1;
                if (AddQustionInDataBase(Qustion, out Id)!=0)
                {
                    Smiles SmileQuestion = (Smiles)Qustion;
                    using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                    {
                        SqlCommand CommandForInsertSmile = new SqlCommand(InsertInSmile, Connection);
                        CommandForInsertSmile.Parameters.AddWithValue(NewNumberOfSmily, SmileQuestion.NumberOfSmiles);
                        CommandForInsertSmile.Parameters.AddWithValue(QustionIdDataBase, Id);
                        CommandForInsertSmile.Connection.Open();
                        CommandForInsertSmile.ExecuteNonQuery();
                        CommandForInsertSmile.Parameters.Clear();
                        SmileQuestion.Id = Id;
                        int IdForType = SelectIdType(TypeOfQuestion.Smily);
                        if (IdForType > 0)
                            SmileQuestion.IdForType = IdForType;
                        NewQuestion = SmileQuestion; 
                        return 1;
                    }
                }
                NewQuestion = null;
                return 0;
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                NewQuestion = null;
                return 0;

            }
            
        }
        public static int AddNewStar(Qustion Qustion,out Qustion NewQuestion)
        {
            try
            {
                int Id;
                if (AddQustionInDataBase(Qustion, out Id)!=0)
                {
                    Stars StarQuestion = (Stars)Qustion;
                    using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                    {
                        SqlCommand CommandForInsertStar = new SqlCommand(InsertInStar, Connection);
                        CommandForInsertStar.Parameters.AddWithValue(NewNumberOfStars, StarQuestion.NumberOfStars);
                        CommandForInsertStar.Parameters.AddWithValue(QustionIdDataBase, Id);
                        CommandForInsertStar.Connection.Open();
                        CommandForInsertStar.ExecuteNonQuery();
                        CommandForInsertStar.Parameters.Clear();
                        StarQuestion.Id = Id;
                        int IdForType = SelectIdType(TypeOfQuestion.Stars);
                        if (IdForType > 0)
                            StarQuestion.IdForType = IdForType;
                        NewQuestion = StarQuestion; 
                        return 1;
                    }
                }
                NewQuestion = null;
                return 0;
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                NewQuestion = null;
                return 0;
            }
            
        }
        /// <summary>
        /// For Edit Question retrun object after edited
        /// </summary>
        private static int EditQuestion(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandForUpdateQustion = new SqlCommand(UpdateQuestion, Connection);
                    CommandForUpdateQustion.Parameters.AddWithValue(NewQuestionText, Question.NewText);
                    CommandForUpdateQustion.Parameters.AddWithValue(NewQuestionOrder, Question.Order);
                    CommandForUpdateQustion.Parameters.AddWithValue(IdQuestion, Question.Id);
                    CommandForUpdateQustion.Connection.Open();
                    CommandForUpdateQustion.ExecuteNonQuery();
                    CommandForUpdateQustion.Parameters.Clear();
                    return 1; 
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int EditSlider(Qustion Qustion)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Slider SliderForEdit = (Slider)Qustion;
                    if (EditQuestion(SliderForEdit) != 0)
                    {
                        SqlCommand CommandForUpdateSlider = new SqlCommand(UpdateSlider, Connection);
                        CommandForUpdateSlider.Parameters.AddWithValue(NewStartValue, SliderForEdit.StartValue);
                        CommandForUpdateSlider.Parameters.AddWithValue(NewEndValue, SliderForEdit.EndValue);
                        CommandForUpdateSlider.Parameters.AddWithValue(NewStartValueCaption, SliderForEdit.StartCaption);
                        CommandForUpdateSlider.Parameters.AddWithValue(NewEndValueCaption, SliderForEdit.EndCaption);
                        CommandForUpdateSlider.Parameters.AddWithValue(IdQuestion, SliderForEdit.Id);
                        CommandForUpdateSlider.Connection.Open();
                        CommandForUpdateSlider.ExecuteNonQuery();
                        CommandForUpdateSlider.Parameters.Clear();
                        return 1;
                    }
                    return 0; 
                    
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int EditSmile(Qustion Qustion)
        {
            try
            {
                Smiles SmileForEdit = (Smiles)Qustion;
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    if (EditQuestion(SmileForEdit) != 0)
                    {
                        SqlCommand CommandForUpdateSmile = new SqlCommand(UpdateSmile, Connection);
                        CommandForUpdateSmile.Parameters.AddWithValue(NewNumberOfSmily, SmileForEdit.NumberOfSmiles);
                        CommandForUpdateSmile.Parameters.AddWithValue(IdQuestion, SmileForEdit.Id);
                        CommandForUpdateSmile.Connection.Open();
                        CommandForUpdateSmile.ExecuteNonQuery();
                        CommandForUpdateSmile.Parameters.Clear();
                        return 1;
                    }
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int EditStar(Qustion Qustion)
        {
            try
            {
                Stars StarForEdit = (Stars)Qustion;
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    if (EditQuestion(StarForEdit) != 0)
                    {
                        SqlCommand CommandForUpdateStar = new SqlCommand(UpdateStar, Connection);
                        CommandForUpdateStar.Parameters.AddWithValue(NewNumberOfStars, StarForEdit.NumberOfStars);
                        CommandForUpdateStar.Parameters.AddWithValue(IdQuestion, StarForEdit.Id);
                        CommandForUpdateStar.Connection.Open();
                        CommandForUpdateStar.ExecuteNonQuery();
                        CommandForUpdateStar.Parameters.Clear();
                        return 1;
                    }
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        /// <summary>
        /// For Delete Question and return 1 if deleted and if not return 0 
        /// </summary>
        private static int DeleteQustion(int Id)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Console.WriteLine(Id);
                    SqlCommand CommandFroDeleteQustion = new SqlCommand(DeleteQustionAttrubites, Connection);
                    CommandFroDeleteQustion.Parameters.AddWithValue(IdQuestion,Id);
                    CommandFroDeleteQustion.Connection.Open();
                    CommandFroDeleteQustion.ExecuteNonQuery();
                    CommandFroDeleteQustion.Parameters.Clear();
                    return 1; 
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0; 
            }
        }
        public static int DeleteSlider(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Slider QustionWillDeleteSlider = (Slider)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(DeleteSliderString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(IdQuestionWithAt, QustionWillDeleteSlider.IdForType);
                    CommandForDeleteQustion.Connection.Open();
                    CommandForDeleteQustion.ExecuteNonQuery();
                    CommandForDeleteQustion.Parameters.Clear();
                    DeleteQustion(Question.Id);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int DeleteSmile(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Smiles QustionWillDeleteSmile = (Smiles)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(DeleteSmilyString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(IdQuestionWithAt, QustionWillDeleteSmile.IdForType);
                    CommandForDeleteQustion.Connection.Open();
                    CommandForDeleteQustion.ExecuteNonQuery();
                    CommandForDeleteQustion.Parameters.Clear();
                    if (DeleteQustion(Question.Id)!=0)
                    return 1;
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        public static int DeleteStar(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Stars QustionWillDeleteStar = (Stars)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(DeleteStarString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(IdQuestionWithAt, QustionWillDeleteStar.IdForType);
                    Console.WriteLine(QustionWillDeleteStar.IdForType + ""); 
                    CommandForDeleteQustion.Connection.Open();
                    CommandForDeleteQustion.ExecuteNonQuery();
                    CommandForDeleteQustion.Parameters.Clear();
                    if (DeleteQustion(QustionWillDeleteStar.Id)!=0)
                    return 1;
                    return 0; 

                }
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return 0;
            }
        }
        /// <summary>
        /// Return list of question from database
        /// </summary>
        public static List<Qustion> GetQuestionFromDataBase()
        {
            try
            {
                List<Qustion> TempListOfQustion = new List<Qustion>();
                SqlDataReader DataReader = null;
                Smiles NewSmile = null;
                Slider NewSlider = null;
                Stars NewStars = null;
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandForJoinQustion = new SqlCommand(JoinSmileAndQustion, Connection);
                    CommandForJoinQustion.Connection.Open();
                    DataReader = CommandForJoinQustion.ExecuteReader();
                    while (DataReader.Read())
                    {
                        NewSmile = new Smiles();
                        NewSmile.Id = Convert.ToInt32(DataReader.GetValue(0));
                        NewSmile.IdForType = Convert.ToInt32(DataReader.GetValue(1));
                        NewSmile.NewText = DataReader.GetValue(2).ToString();
                        NewSmile.Order = Convert.ToInt32(DataReader.GetValue(3));
                        NewSmile.NumberOfSmiles = Convert.ToInt32(DataReader.GetValue(4));
                        NewSmile.TypeOfQuestion = TypeOfQuestion.Smily;
                        TempListOfQustion.Add(NewSmile);
                    }
                    DataReader.Close();
                    CommandForJoinQustion.CommandText = JoinSliderAndQuestion;
                    DataReader = CommandForJoinQustion.ExecuteReader();
                    while (DataReader.Read())
                    {
                        NewSlider = new Slider();
                        NewSlider.Id = Convert.ToInt32(DataReader.GetValue(0));
                        NewSlider.IdForType = Convert.ToInt32(DataReader.GetValue(1));
                        NewSlider.NewText = DataReader.GetValue(2).ToString();
                        NewSlider.Order = Convert.ToInt32(DataReader.GetValue(3));
                        NewSlider.TypeOfQuestion =TypeOfQuestion.Slider;
                        NewSlider.StartValue = Convert.ToInt32(DataReader.GetValue(4));
                        NewSlider.EndValue = Convert.ToInt32(DataReader.GetValue(5));
                        NewSlider.StartCaption = DataReader.GetValue(6).ToString();
                        NewSlider.EndCaption = DataReader.GetValue(7).ToString();
                        TempListOfQustion.Add(NewSlider);
                    }
                    DataReader.Close();
                    CommandForJoinQustion.CommandText = JoinStarsAndQuestion;
                    DataReader = CommandForJoinQustion.ExecuteReader();
                    while (DataReader.Read())
                    {
                        NewStars = new Stars();
                        NewStars.Id = Convert.ToInt32(DataReader.GetValue(0));
                        NewStars.IdForType = Convert.ToInt32(DataReader.GetValue(1));
                        NewStars.NewText = DataReader.GetValue(2).ToString();
                        NewStars.Order = Convert.ToInt32(DataReader.GetValue(3));
                        NewStars.NumberOfStars = Convert.ToInt32(DataReader.GetValue(4));
                        NewStars.TypeOfQuestion = TypeOfQuestion.Stars;
                        TempListOfQustion.Add(NewStars);
                    }
                }
                return TempListOfQustion;
            }
            catch (Exception ex)
            {
                Qustion.Errors.Log(ex);
                return null;
            }
        }
    }
}
