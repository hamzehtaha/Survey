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
using System.Windows.Forms;
using System.Threading;

namespace DataBaseConnection
{
    /// <summary>
    /// This Class For Data Base and get Connection with data base  and actions 
    /// with database using only this class
    /// </summary>
    public class DataBaseConnections
    {
        
        /// <summary>
        /// This function for concatneate attrubites for bulid connection string 
        /// </summary>
        private static int BuildConnectionString()
        {
            try
            {
                GenralVariables.ServerName = ConfigurationManager.AppSettings["Server"];
                GenralVariables.ProviderName = ConfigurationManager.AppSettings["ProviderName"];
                GenralVariables.Database = ConfigurationManager.AppSettings["Database"];
                GenralVariables.UserName = ConfigurationManager.AppSettings["UserName"];
                GenralVariables.Password = ConfigurationManager.AppSettings["Password"];
                GenralVariables.ConnectionString = "Data Source=" + GenralVariables.ServerName + "; Initial Catalog =" + GenralVariables.Database + "; User ID = " + GenralVariables.UserName + "; Password=" + GenralVariables.Password;
                return GenralVariables.Succeeded; 
            }catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData); 
                return GenralVariables.Error; 
            }
        }
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
                BuildConnectionString();
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand ComandForInsertQustion = new SqlCommand(GenralVariables.InsertIntoQustion, Connection);
                        ComandForInsertQustion.CommandText = GenralVariables.InsertIntoQustion;
                        ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionText, Question.NewText);
                        ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionType, Question.TypeOfQuestion);
                        ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionOrder, Question.Order);
                        ComandForInsertQustion.Connection.Open();
                        int result = ComandForInsertQustion.ExecuteNonQuery();
                        if (result >= 1 && SelectIdType(TypeOfQuestion.Qustions, ref Id) == GenralVariables.Succeeded)
                        {
                            return GenralVariables.Succeeded;
                        }
                    }
                return GenralVariables.NoData; 
                 
            }
            catch (Exception ex)
            {
                Id = -1;
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
            
        }
        private static int SelectIdType (TypeOfQuestion TypeOfQustion, ref int Id)
        {
            try
            {
                    string SelectIdTypeStatment = GenralVariables.SelectMaxId + TypeOfQustion.ToString();
                    Id = -1;
                    BuildConnectionString();
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand CommandForSelectIdType = new SqlCommand(SelectIdTypeStatment, Connection);
                        CommandForSelectIdType.Connection.Open();
                        SqlDataReader Reader = CommandForSelectIdType.ExecuteReader();
                        while (Reader.Read())
                            Id = Convert.ToInt32(Reader[GenralVariables.IdQuestion]);
                        Reader.Close();
                        if (Id != -1)
                            return GenralVariables.Succeeded;

                    }
                return GenralVariables.NoData;

            }
            catch (Exception ex)
            {
                Id = -1; 
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error;
            }
        }
        public static int AddNewSlider(Qustion NewQuestion)
        {
            try
            {
                    Slider SliderQuestion = (Slider)NewQuestion;
                    int Id;
                    BuildConnectionString();
                    if (AddQustionInDataBase(NewQuestion, out Id) == GenralVariables.Succeeded)
                    {
                        using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                        {
                            SqlCommand CommandForInsertSlider = new SqlCommand(GenralVariables.InsertInSlider, Connection);
                            CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewStartValue, SliderQuestion.StartValue);
                            CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewEndValue, SliderQuestion.EndValue);
                            CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewStartValueCaption, SliderQuestion.StartCaption);
                            CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.NewEndValueCaption, SliderQuestion.EndCaption);
                            CommandForInsertSlider.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
                            SliderQuestion.Id = Id;
                            CommandForInsertSlider.Connection.Open();
                            int result = CommandForInsertSlider.ExecuteNonQuery();
                            if (result >= 1)
                            {
                                if (SelectIdType(TypeOfQuestion.Slider, ref Id) == GenralVariables.Succeeded)
                                {
                                    SliderQuestion.IdForType = Id;
                                    NewQuestion = SliderQuestion;
                                    return GenralVariables.Succeeded;
                                }
                            }
                    }
                }
                return GenralVariables.NoData; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error;
            }
        }
        public static int AddNewSmile(Qustion NewQuestion)
        {
            try
            {
                BuildConnectionString();
                int Id = -1;
                    if (AddQustionInDataBase(NewQuestion, out Id) == GenralVariables.Succeeded)
                    {
                        Smiles SmileQuestion = (Smiles)NewQuestion;
                        using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                        {
                            SqlCommand CommandForInsertSmile = new SqlCommand(GenralVariables.InsertInSmile, Connection);
                            CommandForInsertSmile.Parameters.AddWithValue(GenralVariables.NewNumberOfSmily, SmileQuestion.NumberOfSmiles);
                            CommandForInsertSmile.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
                            CommandForInsertSmile.Connection.Open();
                            int result = CommandForInsertSmile.ExecuteNonQuery();
                            CommandForInsertSmile.Parameters.Clear();
                            if (result >= 1)
                            {
                                SmileQuestion.Id = Id;
                                if (SelectIdType(TypeOfQuestion.Smily, ref Id) == GenralVariables.Succeeded)
                                {
                                    SmileQuestion.IdForType = Id;
                                    NewQuestion = SmileQuestion;
                                    return GenralVariables.Succeeded;
                                }
                            }
                    }
                }
                return GenralVariables.NoData;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                NewQuestion = null;
                return GenralVariables.Error; 

            }
            
        }
        public static int AddNewStar(Qustion NewQuestion)
        {
            try
            {
                    int Id;
                BuildConnectionString();
                if (AddQustionInDataBase(NewQuestion, out Id) == GenralVariables.Succeeded)
                    {
                        Stars StarQuestion = (Stars)NewQuestion;
                        using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                        {
                            SqlCommand CommandForInsertStar = new SqlCommand(GenralVariables.InsertInStar, Connection);
                            CommandForInsertStar.Parameters.AddWithValue(GenralVariables.NewNumberOfStars, StarQuestion.NumberOfStars);
                            CommandForInsertStar.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
                            CommandForInsertStar.Connection.Open();
                            int result = CommandForInsertStar.ExecuteNonQuery();
                            CommandForInsertStar.Parameters.Clear();
                            if (result >= 1)
                            {
                                StarQuestion.Id = Id;
                                if (SelectIdType(TypeOfQuestion.Stars, ref Id) == GenralVariables.Succeeded)
                                {
                                    StarQuestion.IdForType = Id;
                                    NewQuestion = StarQuestion;
                                    return GenralVariables.Succeeded;
                                }
                            }
                    }
                }
                return GenralVariables.NoData; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
            
        }
        /// <summary>
        /// For Edit Question retrun object after edited
        /// </summary>
        private static int EditQuestion(Qustion Question)
        {
            try
            {
                BuildConnectionString();
                using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand CommandForUpdateQustion = new SqlCommand(GenralVariables.UpdateQuestion, Connection);
                        CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.NewQuestionText, Question.NewText);
                        CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.NewQuestionOrder, Question.Order);
                        CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.IdQuestion, Question.Id);
                        CommandForUpdateQustion.Connection.Open();
                        int result = CommandForUpdateQustion.ExecuteNonQuery();
                        CommandForUpdateQustion.Parameters.Clear();
                        if (result >= 1)
                            return GenralVariables.Succeeded;
                    }
                return GenralVariables.NoData; 
            }

            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error;
            }
        }
        public static int EditSlider(Qustion Qustion)
        {
            try
            {
                BuildConnectionString();
                using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Slider SliderForEdit = (Slider)Qustion;
                        if (EditQuestion(SliderForEdit) == GenralVariables.Succeeded)
                        {
                            SqlCommand CommandForUpdateSlider = new SqlCommand(GenralVariables.UpdateSlider, Connection);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewStartValue, SliderForEdit.StartValue);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewEndValue, SliderForEdit.EndValue);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewStartValueCaption, SliderForEdit.StartCaption);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewEndValueCaption, SliderForEdit.EndCaption);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.IdQuestion, SliderForEdit.Id);
                            CommandForUpdateSlider.Connection.Open();
                            int result = CommandForUpdateSlider.ExecuteNonQuery();
                            CommandForUpdateSlider.Parameters.Clear();
                            if (result >= 1)
                                return GenralVariables.Succeeded;
                        }
                }
                return GenralVariables.NoData; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
        }
        public static int EditSmile(Qustion Qustion)
        {
            try
            {
                BuildConnectionString();
                Smiles SmileForEdit = (Smiles)Qustion;
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        if (EditQuestion(SmileForEdit) == GenralVariables.Succeeded)
                        {
                            SqlCommand CommandForUpdateSmile = new SqlCommand(GenralVariables.UpdateSmile, Connection);
                            CommandForUpdateSmile.Parameters.AddWithValue(GenralVariables.NewNumberOfSmily, SmileForEdit.NumberOfSmiles);
                            CommandForUpdateSmile.Parameters.AddWithValue(GenralVariables.IdQuestion, SmileForEdit.Id);
                            CommandForUpdateSmile.Connection.Open();
                            int result = CommandForUpdateSmile.ExecuteNonQuery();
                            CommandForUpdateSmile.Parameters.Clear();
                            if (result >= 1)
                                return GenralVariables.Succeeded;
                        }
                }
                return GenralVariables.NoData; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
        }
        public static int EditStar(Qustion Qustion)
        {
            try
            {
                BuildConnectionString();
                Stars StarForEdit = (Stars)Qustion;
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        if (EditQuestion(StarForEdit) == GenralVariables.Succeeded)
                        {
                            SqlCommand CommandForUpdateStar = new SqlCommand(GenralVariables.UpdateStar, Connection);
                            CommandForUpdateStar.Parameters.AddWithValue(GenralVariables.NewNumberOfStars, StarForEdit.NumberOfStars);
                            CommandForUpdateStar.Parameters.AddWithValue(GenralVariables.IdQuestion, StarForEdit.Id);
                            CommandForUpdateStar.Connection.Open();
                            int result = CommandForUpdateStar.ExecuteNonQuery();
                            CommandForUpdateStar.Parameters.Clear();
                            if (result >= 1)
                                return GenralVariables.Succeeded;
                        }
                }
                return GenralVariables.NoData; 

            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
        }
        /// <summary>
        /// For Delete Question and return 1 if deleted and if not return 0 
        /// </summary>
        private static int DeleteQustion(int Id)
        {
            try
            {
                BuildConnectionString();
                using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand CommandFroDeleteQustion = new SqlCommand(GenralVariables.DeleteQustionAttrubites, Connection);
                        CommandFroDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestion, Id);
                        CommandFroDeleteQustion.Connection.Open();
                        int result = CommandFroDeleteQustion.ExecuteNonQuery();
                        CommandFroDeleteQustion.Parameters.Clear();
                        if (result >= 1)
                        {
                            return GenralVariables.Succeeded;
                        }
                }
                return GenralVariables.NoData; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
        }
        public static int DeleteSlider(Qustion Question)
        {
            try
            {
                BuildConnectionString();
                using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Slider QustionWillDeleteSlider = (Slider)Question;
                        SqlCommand CommandForDeleteQustion = null;
                        CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteSliderString, Connection);
                        CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteSlider.IdForType);
                        CommandForDeleteQustion.Connection.Open();
                        int result = CommandForDeleteQustion.ExecuteNonQuery();
                        CommandForDeleteQustion.Parameters.Clear();
                        if (DeleteQustion(Question.Id) == GenralVariables.Succeeded && result >= 1)
                        {
                            return GenralVariables.Succeeded;
                        }
                        return GenralVariables.NoData;
                }
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
        }
        public static int DeleteSmile(Qustion Question)
        {
            try
            {
                BuildConnectionString();
                using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Smiles QustionWillDeleteSmile = (Smiles)Question;
                        SqlCommand CommandForDeleteQustion = null;
                        CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteSmilyString, Connection);
                        CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteSmile.IdForType);
                        CommandForDeleteQustion.Connection.Open();
                        int result = CommandForDeleteQustion.ExecuteNonQuery();
                        CommandForDeleteQustion.Parameters.Clear();
                        if (DeleteQustion(Question.Id) == GenralVariables.Succeeded && result >= 1)
                        {
                            return GenralVariables.Succeeded;
                        }
                }
                return GenralVariables.NoData; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error;
            }
        }
        public static int DeleteStar(Qustion Question)
        {
            try
            {
                BuildConnectionString();
                using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Stars QustionWillDeleteStar = (Stars)Question;
                        SqlCommand CommandForDeleteQustion = null;
                        CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteStarString, Connection);
                        CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteStar.IdForType);
                        CommandForDeleteQustion.Connection.Open();
                        int result = CommandForDeleteQustion.ExecuteNonQuery();
                        CommandForDeleteQustion.Parameters.Clear();
                        if (DeleteQustion(Question.Id) == GenralVariables.Succeeded && result >= 1)
                        {
                            return GenralVariables.Succeeded;
                        }
                }
                return GenralVariables.NoData;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error; 
            }
        }
        /// <summary>
        /// Return list of question from database
        /// </summary>
        /// 
        public static int GetQuestionFromDataBase(ref List<Qustion> TempListOfQustion)
        {
            try
            {
                BuildConnectionString();
                    SqlDataReader DataReader = null;
                    Smiles NewSmile = null;
                    Slider NewSlider = null;
                    Stars NewStars = null;
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
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
                            NewSlider.TypeOfQuestion = TypeOfQuestion.Slider;
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
                return GenralVariables.Succeeded ;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex);
                MessageBox.Show(DataBaseConnection.Properties.Resource1.ErrorData);
                return GenralVariables.Error;
            }
        }
        public static DataGridView Fun()
        {
            return null; 
        }
    }
}
