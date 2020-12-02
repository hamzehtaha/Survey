using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1;
namespace Survey
{
    /// <summary>
    /// This Class For Data Base and get Connection with data base  and actions 
    /// with database using only this class
    /// </summary>
    public  class DataBaseConnections
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
        private const string SelectStarFromQuestion = "SELECT * FROM Qustions";
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
        private static int AddQustionInDataBase(Qustions Question)
        {
            int Id = -1;
            try
            {
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
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandForSelectMaxForQuestion = new SqlCommand(ProcdureQuestionSelectForMax, Connection);
                    CommandForSelectMaxForQuestion.Connection.Open();
                    SqlDataReader Reader = CommandForSelectMaxForQuestion.ExecuteReader();
                    while (Reader.Read())
                        Id = Convert.ToInt32(Reader[IdQuestion]);
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                StaticObjects.SuccOfFail = 0;
                return Id;
            }
            return Id;
        }
        public static Qustions AddNewSlider(Qustions Qustion)
        {
            Slider SliderQuestion = (Slider)Qustion;
            int Id = AddQustionInDataBase(SliderQuestion);
            if (Id != -1)
            {
                try
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
                        StaticObjects.SuccOfFail = 1;
                        return SliderQuestion;
                    }

                }
                catch (Exception ex)
                {
                    StaticObjects.Erros.Log(ex);
                    StaticObjects.SuccOfFail = 0;
                    return SliderQuestion;
                }

            }
            return SliderQuestion; 
        }
        public static Qustions AddNewSmile (Qustions Qustion)
        {
            int Id = AddQustionInDataBase(Qustion);
            Smiles SmileQuestion = (Smiles)Qustion;
            if (Id != -1)
            {
                try
                {
                    using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                    {
                        SqlCommand CommandForInsertSmile = new SqlCommand(InsertInSmile, Connection);
                        CommandForInsertSmile.Parameters.AddWithValue(NewNumberOfSmily, SmileQuestion.NumberOfSmiles);
                        CommandForInsertSmile.Parameters.AddWithValue(QustionIdDataBase, Id);
                        CommandForInsertSmile.Connection.Open();
                        CommandForInsertSmile.ExecuteNonQuery();
                        CommandForInsertSmile.Parameters.Clear();
                        SmileQuestion.Id = Id;
                        StaticObjects.SuccOfFail = 1;
                        return SmileQuestion;
                    }
                }
                catch (Exception ex)
                {
                    StaticObjects.Erros.Log(ex);
                    StaticObjects.SuccOfFail = 0;
                    return SmileQuestion;

                }
            }
            return SmileQuestion;

        }
        public static Qustions AddNewStar (Qustions Qustion)
        {
            Stars StarQuestion = (Stars)Qustion;
            int Id = AddQustionInDataBase(Qustion);
            if (Id != -1)
            {
                try
                {
                    using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                    {
                        SqlCommand CommandForInsertStar = new SqlCommand(InsertInStar, Connection);
                        CommandForInsertStar.Parameters.AddWithValue(NewNumberOfStars, StarQuestion.NumberOfStars);
                        CommandForInsertStar.Parameters.AddWithValue(QustionIdDataBase, Id);
                        CommandForInsertStar.Connection.Open();
                        CommandForInsertStar.ExecuteNonQuery();
                        CommandForInsertStar.Parameters.Clear();
                        StarQuestion.Id = Id;
                        //StaticObjects.ListOfAllQuestion.Add(StarQuestion);
                        StaticObjects.SuccOfFail = 1;
                        return StarQuestion;
                    }
                }
                catch (Exception ex)
                {
                    StaticObjects.Erros.Log(ex);
                    StaticObjects.SuccOfFail = 0;
                    return StarQuestion;
                }
            }
            return StarQuestion;
        }
        /// <summary>
        /// For Edit Question retrun object after edited
        /// </summary>
        private static void EditQuestion(Qustions Question)
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
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
            }
        }
        public static Qustions EditSlider (Qustions Qustion)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Slider SliderForEdit = (Slider)Qustion;
                    SqlCommand CommandForUpdateSlider = new SqlCommand(UpdateSlider, Connection);
                    CommandForUpdateSlider.Parameters.AddWithValue(NewStartValue, SliderForEdit.StartValue);
                    CommandForUpdateSlider.Parameters.AddWithValue(NewEndValue, SliderForEdit.EndValue);
                    CommandForUpdateSlider.Parameters.AddWithValue(NewStartValueCaption, SliderForEdit.StartCaption);
                    CommandForUpdateSlider.Parameters.AddWithValue(NewEndValueCaption, SliderForEdit.EndCaption);
                    CommandForUpdateSlider.Parameters.AddWithValue(IdQuestion, SliderForEdit.Id);
                    CommandForUpdateSlider.Connection.Open();
                    CommandForUpdateSlider.ExecuteNonQuery();
                    CommandForUpdateSlider.Parameters.Clear();
                    EditQuestion(SliderForEdit);
                    return SliderForEdit;
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return Qustion;
            }
        }
        public static Qustions EditSmile (Qustions Qustion)
        {
            try
            {
                Smiles SmileForEdit = (Smiles)Qustion;
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandForUpdateSmile = new SqlCommand(UpdateSmile, Connection);
                    CommandForUpdateSmile.Parameters.AddWithValue(NewNumberOfSmily, SmileForEdit.NumberOfSmiles);
                    CommandForUpdateSmile.Parameters.AddWithValue(IdQuestion, SmileForEdit.Id);
                    CommandForUpdateSmile.Connection.Open();
                    CommandForUpdateSmile.ExecuteNonQuery();
                    CommandForUpdateSmile.Parameters.Clear();
                    EditQuestion(SmileForEdit);
                    return SmileForEdit;
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return Qustion;
            }
        }
        public static Qustions EditStar (Qustions Qustion)
        {
            try
            {
                Stars StarForEdit = (Stars)Qustion;
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandForUpdateStar = new SqlCommand(UpdateStar, Connection);
                    CommandForUpdateStar.Parameters.AddWithValue(NewNumberOfStars, StarForEdit.NumberOfStars);
                    CommandForUpdateStar.Parameters.AddWithValue(IdQuestion, StarForEdit.Id);
                    CommandForUpdateStar.Connection.Open();
                    CommandForUpdateStar.ExecuteNonQuery();
                    CommandForUpdateStar.Parameters.Clear();
                    EditQuestion(StarForEdit);
                    return StarForEdit;
                }

            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return Qustion;
            }
        }
        /// <summary>
        /// For Delete Question and return 1 if deleted and if not return 0 
        /// </summary>
        private static void DeleteQustion(int Id)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    SqlCommand CommandFroDeleteQustion = new SqlCommand(DeleteQustionAttrubites, Connection);
                    CommandFroDeleteQustion.Parameters.AddWithValue(IdQuestion, Id);
                    CommandFroDeleteQustion.Connection.Open(); 
                    CommandFroDeleteQustion.ExecuteNonQuery();
                    CommandFroDeleteQustion.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
            }
        }
        public static int DeleteSlider (Qustions Question)
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
                StaticObjects.Erros.Log(ex);
                return 0;
            }
        }
        public static int DeleteSmile(Qustions Question)
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
                    DeleteQustion(Question.Id);
                    return 1; 
                }
            }catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return 0; 
            }
        }
        public static int DeleteStar (Qustions Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(DataBaseConnections.connectionString))
                {
                    Stars QustionWillDeleteStar = (Stars)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(DeleteStarString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(IdQuestionWithAt, QustionWillDeleteStar.IdForType);
                    CommandForDeleteQustion.Connection.Open();
                    CommandForDeleteQustion.ExecuteNonQuery();
                    CommandForDeleteQustion.Parameters.Clear();
                    DeleteQustion(Question.Id);
                    return 1; 

                }

                }catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return 0; 
            }
        }
        /// <summary>
        /// Return list of question from database
        /// </summary>
        public static List<Qustions> GetQuestionFromDataBase()
        {
            List<Qustions> TempListOfQustion = new List<Qustions>();
            SqlDataReader DataReader = null;
            Smiles NewSmile = null;
            Slider NewSlider = null;
            Stars NewStars = null;
            try
            {
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
                        NewSmile.NewText = DataReader.GetValue(2) + Constant.Empty;
                        NewSmile.Order = Convert.ToInt32(DataReader.GetValue(3));
                        NewSmile.NumberOfSmiles = Convert.ToInt32(DataReader.GetValue(4));
                        NewSmile.TypeOfQuestion = TypeOfQuestion.Smily.ToString();
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
                        NewSlider.NewText = DataReader.GetValue(2) + Constant.Empty;
                        NewSlider.Order = Convert.ToInt32(DataReader.GetValue(3));
                        NewSlider.TypeOfQuestion = TypeOfQuestion.Slider.ToString();
                        NewSlider.StartValue = Convert.ToInt32(DataReader.GetValue(4));
                        NewSlider.EndValue = Convert.ToInt32(DataReader.GetValue(5));
                        NewSlider.StartCaption = DataReader.GetValue(6) + Constant.Empty;
                        NewSlider.EndCaption = DataReader.GetValue(7) + Constant.Empty;
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
                        NewStars.NewText = DataReader.GetValue(2) + Constant.Empty;
                        NewStars.Order = Convert.ToInt32(DataReader.GetValue(3));
                        NewStars.NumberOfStars = Convert.ToInt32(DataReader.GetValue(4));
                        NewStars.TypeOfQuestion = TypeOfQuestion.Stars.ToString();
                        TempListOfQustion.Add(NewStars);
                    }
                }
                return TempListOfQustion;
            }
            catch (Exception ex)
            {
                StaticObjects.Erros.Log(ex);
                return TempListOfQustion;
            }
        }
    }
}
