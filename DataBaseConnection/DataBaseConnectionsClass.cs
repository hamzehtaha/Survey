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
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    SqlCommand ComandForInsertQustion = new SqlCommand(GenralVariables.InsertIntoQustion, Connection);
                    ComandForInsertQustion.CommandText = GenralVariables.InsertIntoQustion;
                    ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionText, Question.NewText);
                    ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionType, Question.TypeOfQuestion);
                    ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionOrder, Question.Order);
                    ComandForInsertQustion.Connection.Open();
                    ComandForInsertQustion.ExecuteNonQuery();
                }
                Id = SelectIdType(TypeOfQuestion.Qustions); 
                return 1; 
            }
            catch (Exception ex)
            {
                Id = -1;
                GenralVariables.Errors.Log(ex);
                return 0; 
            }
            
        }
        private static int SelectIdType (TypeOfQuestion TypeOfQustion)
        {
            try
            {
                string SelectIdTypeStatment = GenralVariables.SelectMaxId + TypeOfQustion.ToString();
                int Id = -1; 
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    SqlCommand CommandForSelectIdType = new SqlCommand(SelectIdTypeStatment, Connection);
                    CommandForSelectIdType.Connection.Open();
                    SqlDataReader Reader = CommandForSelectIdType.ExecuteReader();
                    while (Reader.Read())
                        Id = Convert.ToInt32(Reader[GenralVariables.IdQuestion]);
                    Reader.Close();
                }
                return Id; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
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
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                    {
                        SqlCommand CommandForInsertSlider = new SqlCommand(GenralVariables.InsertInSlider, Connection);
                        CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewStartValue, SliderQuestion.StartValue);
                        CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewEndValue, SliderQuestion.EndValue);
                        CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewStartValueCaption, SliderQuestion.StartCaption);
                        CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewEndValueCaption, SliderQuestion.EndCaption);
                        CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
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
                GenralVariables.Errors.Log(ex);
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
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                    {
                        SqlCommand CommandForInsertSmile = new SqlCommand(GenralVariables.InsertInSmile, Connection);
                        CommandForInsertSmile.Parameters.AddWithValue(GenralVariables.NewNumberOfSmily, SmileQuestion.NumberOfSmiles);
                        CommandForInsertSmile.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
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
                GenralVariables.Errors.Log(ex);
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
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                    {
                        SqlCommand CommandForInsertStar = new SqlCommand(GenralVariables.InsertInStar, Connection);
                        CommandForInsertStar.Parameters.AddWithValue(GenralVariables.NewNumberOfStars, StarQuestion.NumberOfStars);
                        CommandForInsertStar.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
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
                GenralVariables.Errors.Log(ex);
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
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    SqlCommand CommandForUpdateQustion = new SqlCommand(GenralVariables.UpdateQuestion, Connection);
                    CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.NewQuestionText, Question.NewText);
                    CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.NewQuestionOrder, Question.Order);
                    CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.IdQuestion, Question.Id);
                    CommandForUpdateQustion.Connection.Open();
                    CommandForUpdateQustion.ExecuteNonQuery();
                    CommandForUpdateQustion.Parameters.Clear();
                    return 1; 
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                return 0;
            }
        }
        public static int EditSlider(Qustion Qustion)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    Slider SliderForEdit = (Slider)Qustion;
                    if (EditQuestion(SliderForEdit) != 0)
                    {
                        SqlCommand CommandForUpdateSlider = new SqlCommand(GenralVariables.UpdateSlider, Connection);
                        CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewStartValue, SliderForEdit.StartValue);
                        CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewEndValue, SliderForEdit.EndValue);
                        CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewStartValueCaption, SliderForEdit.StartCaption);
                        CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewEndValueCaption, SliderForEdit.EndCaption);
                        CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.IdQuestion, SliderForEdit.Id);
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
                GenralVariables.Errors.Log(ex);
                return 0;
            }
        }
        public static int EditSmile(Qustion Qustion)
        {
            try
            {
                Smiles SmileForEdit = (Smiles)Qustion;
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    if (EditQuestion(SmileForEdit) != 0)
                    {
                        SqlCommand CommandForUpdateSmile = new SqlCommand(GenralVariables.UpdateSmile, Connection);
                        CommandForUpdateSmile.Parameters.AddWithValue(GenralVariables.NewNumberOfSmily, SmileForEdit.NumberOfSmiles);
                        CommandForUpdateSmile.Parameters.AddWithValue(GenralVariables.IdQuestion, SmileForEdit.Id);
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
                GenralVariables.Errors.Log(ex);
                return 0;
            }
        }
        public static int EditStar(Qustion Qustion)
        {
            try
            {
                Stars StarForEdit = (Stars)Qustion;
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    if (EditQuestion(StarForEdit) != 0)
                    {
                        SqlCommand CommandForUpdateStar = new SqlCommand(GenralVariables.UpdateStar, Connection);
                        CommandForUpdateStar.Parameters.AddWithValue(GenralVariables.NewNumberOfStars, StarForEdit.NumberOfStars);
                        CommandForUpdateStar.Parameters.AddWithValue(GenralVariables.IdQuestion, StarForEdit.Id);
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
                GenralVariables.Errors.Log(ex);
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
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    SqlCommand CommandFroDeleteQustion = new SqlCommand(GenralVariables.DeleteQustionAttrubites, Connection);
                    CommandFroDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestion,Id);
                    CommandFroDeleteQustion.Connection.Open();
                    CommandFroDeleteQustion.ExecuteNonQuery();
                    CommandFroDeleteQustion.Parameters.Clear();
                    return 1; 
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                return 0; 
            }
        }
        public static int DeleteSlider(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    Slider QustionWillDeleteSlider = (Slider)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteSliderString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteSlider.IdForType);
                    CommandForDeleteQustion.Connection.Open();
                    CommandForDeleteQustion.ExecuteNonQuery();
                    CommandForDeleteQustion.Parameters.Clear();
                    DeleteQustion(Question.Id);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                return 0;
            }
        }
        public static int DeleteSmile(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    Smiles QustionWillDeleteSmile = (Smiles)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteSmilyString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteSmile.IdForType);
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
                GenralVariables.Errors.Log(ex);
                return 0;
            }
        }
        public static int DeleteStar(Qustion Question)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    Stars QustionWillDeleteStar = (Stars)Question;
                    SqlCommand CommandForDeleteQustion = null;
                    CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteStarString, Connection);
                    CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteStar.IdForType);
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
                GenralVariables.Errors.Log(ex);
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
                using (SqlConnection Connection = new SqlConnection(GenralVariables.connectionString))
                {
                    SqlCommand CommandForJoinQustion = new SqlCommand(GenralVariables.JoinSmileAndQustion, Connection);
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
                    CommandForJoinQustion.CommandText = GenralVariables.JoinSliderAndQuestion;
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
                    CommandForJoinQustion.CommandText = GenralVariables.JoinStarsAndQuestion;
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
                GenralVariables.Errors.Log(ex);
                return null;
            }
        }
    }
}
