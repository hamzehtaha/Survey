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
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorConnectionString; 
            }
        }
        /// <summary>
        /// This function to select a question from database using his type from manger.
        /// </summary>
        private static int SelectIdType(TypeOfQuestion TypeOfQustion, ref int Id)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    string SelectIdTypeStatment = GenralVariables.SelectMaxId + TypeOfQustion.ToString();
                    Id = -1;
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
                    return GenralVariables.ErrorInDataBase;
                }
                return ResultOfBulid;
            }
            catch (Exception ex)
            {
                Id = -1;
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInSelectionQuestion;
            }
        }
        /// <summary>
        /// This functions for add or edit and delete and select from database,
        /// And connections in database 
        /// </summary>

        /// <summary>
        /// This functions for add a question in database get data from manger 
        /// and return 0 if succeeded
        /// </summary>
        private static int AddQustionInDataBase(Qustion Question,out int Id)
        {
            try
            {
                Id = -1;
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand ComandForInsertQustion = new SqlCommand(GenralVariables.InsertIntoQustion, Connection);
                        ComandForInsertQustion.CommandText = GenralVariables.InsertIntoQustion;
                        ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionText, Question.NewText);
                        ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionType, Question.TypeOfQuestion);
                        ComandForInsertQustion.Parameters.AddWithValue(GenralVariables.NewQuestionOrder, Question.Order);
                        ComandForInsertQustion.Connection.Open();
                        int NumberOfRowsaffected = ComandForInsertQustion.ExecuteNonQuery();
                        if (NumberOfRowsaffected >= 1)
                        {
                            if (SelectIdType(TypeOfQuestion.Qustions, ref Id) == GenralVariables.Succeeded)
                            return GenralVariables.Succeeded;
                        }else
                        {
                            GenralVariables.Errors.Log(GenralVariables.ErrorAddQuestionInLog);
                            return GenralVariables.ErrorInOperation;
                        }
                    }
                    return GenralVariables.ErrorInDataBase;
                }
                return ResultOfBulid; 
            }
            catch (Exception ex)
            {
                Id = -1;
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInAddQuestion; 
            }
            
        }
        public static int AddNewSlider(Qustion NewQuestion)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    Slider SliderQuestion = (Slider)NewQuestion;
                    int Id;
                    int ResultOfAdd = AddQustionInDataBase(NewQuestion, out Id);
                    if (ResultOfAdd == GenralVariables.Succeeded)
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
                            int NumberOfRowsaffected = CommandForInsertSlider.ExecuteNonQuery();
                            if (NumberOfRowsaffected >= 1)
                            {
                                if (SelectIdType(TypeOfQuestion.Slider, ref Id) == GenralVariables.Succeeded)
                                {
                                    SliderQuestion.IdForType = Id;
                                    NewQuestion = SliderQuestion;
                                    return GenralVariables.Succeeded;
                                }
                            }
                            else
                            {
                                GenralVariables.Errors.Log(GenralVariables.ErrorAddSliderInLog);
                                return GenralVariables.ErrorInOperation;
                            }
                        }
                        return GenralVariables.ErrorInDataBase;
                    }
                    return ResultOfAdd;
                }
                return ResultOfBulid; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInAddQuestion;
            }
        }
        public static int AddNewSmile(Qustion NewQuestion)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    Smiles SmileQuestion = (Smiles)NewQuestion;
                    int Id;
                    int ResultOfAdd = AddQustionInDataBase(NewQuestion, out Id);
                    if (ResultOfAdd == GenralVariables.Succeeded)
                    {
                        using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                        {
                            SqlCommand CommandForInsertSmile = new SqlCommand(GenralVariables.InsertInSmile, Connection);
                            CommandForInsertSmile.Parameters.AddWithValue(GenralVariables.NewNumberOfSmily, SmileQuestion.NumberOfSmiles);
                            CommandForInsertSmile.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
                            SmileQuestion.Id = Id;
                            CommandForInsertSmile.Connection.Open();
                            int NumberOfRowsaffected = CommandForInsertSmile.ExecuteNonQuery();
                            if (NumberOfRowsaffected >= 1)
                            {
                                if (SelectIdType(TypeOfQuestion.Slider, ref Id) == GenralVariables.Succeeded)
                                {
                                    SmileQuestion.IdForType = Id;
                                    NewQuestion = SmileQuestion;
                                    return GenralVariables.Succeeded;
                                }
                            }
                            else
                            {
                                GenralVariables.Errors.Log(GenralVariables.ErrorAddSliderInLog);
                                return GenralVariables.ErrorInOperation;
                            }
                        }
                        return GenralVariables.ErrorInDataBase;
                    }
                    return ResultOfAdd;
                }
                return ResultOfBulid;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInAddQuestion;
            }
        }
        public static int AddNewStar(Qustion NewQuestion)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    Stars StarQuestion = (Stars)NewQuestion;
                    int Id;
                    int ResultOfAdd = AddQustionInDataBase(NewQuestion, out Id);
                    if (ResultOfAdd == GenralVariables.Succeeded)
                    {
                        using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                        {
                            SqlCommand CommandForInsertStar = new SqlCommand(GenralVariables.InsertInStar, Connection);
                            CommandForInsertStar.Parameters.AddWithValue(GenralVariables.NewNumberOfStars, StarQuestion.NumberOfStars);
                            CommandForInsertStar.Parameters.AddWithValue(GenralVariables.QustionIdDataBase, Id);
                            StarQuestion.Id = Id;
                            CommandForInsertStar.Connection.Open();
                            int NumberOfRowsaffected = CommandForInsertStar.ExecuteNonQuery();
                            if (NumberOfRowsaffected >= 1)
                            {
                                if (SelectIdType(TypeOfQuestion.Slider, ref Id) == GenralVariables.Succeeded)
                                {
                                    StarQuestion.IdForType = Id;
                                    NewQuestion = StarQuestion;
                                    return GenralVariables.Succeeded;
                                }
                            }
                            else
                            {
                                GenralVariables.Errors.Log(GenralVariables.ErrorAddSliderInLog);
                                return GenralVariables.ErrorInOperation;
                            }
                        }
                        return GenralVariables.ErrorInDataBase;
                    }
                    return ResultOfAdd;
                }
                return ResultOfBulid;
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInAddQuestion;
            }

        }
        /// <summary>
        /// This functions for edit a question in database and get a new data from manger
        /// and return 0 if succeeded
        /// </summary>
        private static int EditQuestion(Qustion Question)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand CommandForUpdateQustion = new SqlCommand(GenralVariables.UpdateQuestion, Connection);
                        CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.NewQuestionText, Question.NewText);
                        CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.NewQuestionOrder, Question.Order);
                        CommandForUpdateQustion.Parameters.AddWithValue(GenralVariables.IdQuestion, Question.Id);
                        CommandForUpdateQustion.Connection.Open();
                        int NumberOfRowsaffected = CommandForUpdateQustion.ExecuteNonQuery();
                        CommandForUpdateQustion.Parameters.Clear();
                        if (NumberOfRowsaffected >= 1)
                            return GenralVariables.Succeeded;
                        else
                        {
                            GenralVariables.Errors.Log(GenralVariables.ErrorEditQuestionInLog);
                            return GenralVariables.ErrorInOperation;
                        }
                    }
                    
                }
                return ResultOfBulid; 
            }

            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInEditQuestion;
            }
        }
        public static int EditSlider(Qustion Qustion)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Slider SliderForEdit = (Slider)Qustion;
                        int ResultOfEdit = EditQuestion(SliderForEdit);
                        if (ResultOfEdit == GenralVariables.Succeeded)
                        {
                            SqlCommand CommandForUpdateSlider = new SqlCommand(GenralVariables.UpdateSlider, Connection);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewStartValue, SliderForEdit.StartValue);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewEndValue, SliderForEdit.EndValue);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewStartValueCaption, SliderForEdit.StartCaption);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.NewEndValueCaption, SliderForEdit.EndCaption);
                            CommandForUpdateSlider.Parameters.AddWithValue(GenralVariables.IdQuestion, SliderForEdit.Id);
                            CommandForUpdateSlider.Connection.Open();
                            int NumberOfRowsaffected = CommandForUpdateSlider.ExecuteNonQuery();
                            CommandForUpdateSlider.Parameters.Clear();
                            if (NumberOfRowsaffected >= 1)
                                return GenralVariables.Succeeded;
                            else
                            {
                                GenralVariables.Errors.Log(GenralVariables.ErrorEditSliderInLog);
                                return GenralVariables.ErrorInOperation;
                            }
                        }
                        return ResultOfEdit;
                    }
                }
                return ResultOfBulid; 
                 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInEditQuestion; 
            }
        }
        public static int EditSmile(Qustion Qustion)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    Smiles SmileForEdit = (Smiles)Qustion;
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        int ResultOfEdit = EditQuestion(SmileForEdit);
                        if (ResultOfEdit == GenralVariables.Succeeded)
                        {
                            SqlCommand CommandForUpdateSmile = new SqlCommand(GenralVariables.UpdateSmile, Connection);
                            CommandForUpdateSmile.Parameters.AddWithValue(GenralVariables.NewNumberOfSmily, SmileForEdit.NumberOfSmiles);
                            CommandForUpdateSmile.Parameters.AddWithValue(GenralVariables.IdQuestion, SmileForEdit.Id);
                            CommandForUpdateSmile.Connection.Open();
                            int NumberOfRowsaffected = CommandForUpdateSmile.ExecuteNonQuery();
                            CommandForUpdateSmile.Parameters.Clear();
                            if (NumberOfRowsaffected >= 1)
                                return GenralVariables.Succeeded;
                            else
                            {
                                GenralVariables.Errors.Log(GenralVariables.ErrorEditSmileInLog);
                                return GenralVariables.ErrorInOperation;
                            }
                        }
                        return ResultOfEdit;

                    }
                }
                return ResultOfBulid; 
                
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInEditQuestion; 
            }
        }
        public static int EditStar(Qustion Qustion)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    Stars StarForEdit = (Stars)Qustion;
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        int ResultOfEdit = EditQuestion(StarForEdit);
                        if (ResultOfEdit == GenralVariables.Succeeded)
                        {
                            SqlCommand CommandForUpdateStar = new SqlCommand(GenralVariables.UpdateStar, Connection);
                            CommandForUpdateStar.Parameters.AddWithValue(GenralVariables.NewNumberOfStars, StarForEdit.NumberOfStars);
                            CommandForUpdateStar.Parameters.AddWithValue(GenralVariables.IdQuestion, StarForEdit.Id);
                            CommandForUpdateStar.Connection.Open();
                            int NumberOfRowsaffected = CommandForUpdateStar.ExecuteNonQuery();
                            CommandForUpdateStar.Parameters.Clear();
                            if (NumberOfRowsaffected >= 1)
                                return GenralVariables.Succeeded;
                            else
                            {
                                GenralVariables.Errors.Log(GenralVariables.ErrorEditStarInLog);
                                return GenralVariables.ErrorInOperation; 
                            }
                        }
                        return ResultOfEdit;
                    }
                }
                return ResultOfBulid; 
                 

            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInEditQuestion; 
            }
        }
        /// <summary>
        /// This functions for delete a question from database and get a question to delete it from manger
        /// and return 0 if succeeded
        /// </summary>
        private static int DeleteQustion(int Id)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        SqlCommand CommandFroDeleteQustion = new SqlCommand(GenralVariables.DeleteQustionAttrubites, Connection);
                        CommandFroDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestion, Id);
                        CommandFroDeleteQustion.Connection.Open();
                        int NumberOfRowsaffected = CommandFroDeleteQustion.ExecuteNonQuery();
                        CommandFroDeleteQustion.Parameters.Clear();
                        if (NumberOfRowsaffected >= 1)
                        {
                            return GenralVariables.Succeeded;
                        }
                        else
                        {
                            GenralVariables.Errors.Log(GenralVariables.ErrorDeleteQuestionInLog);
                            return GenralVariables.ErrorInOperation; 
                        }
                    }
                    
                }
                return ResultOfBulid; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInDeleteQuestion; 
            }
        }
        public static int DeleteSlider(Qustion Question)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Slider QustionWillDeleteSlider = (Slider)Question;
                        SqlCommand CommandForDeleteQustion = null;
                        CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteSliderString, Connection);
                        CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteSlider.IdForType);
                        CommandForDeleteQustion.Connection.Open();
                        int NumberOfRowsaffected = CommandForDeleteQustion.ExecuteNonQuery();
                        CommandForDeleteQustion.Parameters.Clear();
                        int ResultOfDelete = DeleteQustion(Question.Id);
                        if (NumberOfRowsaffected >= 1)
                        {
                            if (ResultOfDelete == GenralVariables.Succeeded)
                            {
                                return GenralVariables.Succeeded;
                            }
                        }
                        else
                        {
                            GenralVariables.Errors.Log(GenralVariables.ErrorDeleteSliderInLog);
                            return GenralVariables.ErrorInOperation;
                        }
                        return ResultOfDelete;
                    }
                }
                return ResultOfBulid; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInDeleteQuestion; 
            }
        }
        public static int DeleteSmile(Qustion Question)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Smiles QustionWillDeleteSmile = (Smiles)Question;
                        SqlCommand CommandForDeleteQustion = null;
                        CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteSmilyString, Connection);
                        CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteSmile.IdForType);
                        CommandForDeleteQustion.Connection.Open();
                        int NumberOfRowsaffected = CommandForDeleteQustion.ExecuteNonQuery();
                        CommandForDeleteQustion.Parameters.Clear();
                        int ResultOfDelete = DeleteQustion(Question.Id);
                        if (NumberOfRowsaffected >= 1)
                        {
                            if (ResultOfDelete == GenralVariables.Succeeded)
                            {
                                return GenralVariables.Succeeded;
                            }
                        }
                        else
                        {
                            GenralVariables.Errors.Log(GenralVariables.ErrorDeleteSmileInLog);
                            return GenralVariables.ErrorInOperation;
                        }
                        return ResultOfDelete;
                    }
                }
                return ResultOfBulid; 
                 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInDeleteQuestion;
            }
        }
        public static int DeleteStar(Qustion Question)
        {
            try
            {
                int ResultOfBulid = BuildConnectionString();
                if (ResultOfBulid == GenralVariables.Succeeded)
                {
                    using (SqlConnection Connection = new SqlConnection(GenralVariables.ConnectionString))
                    {
                        Stars QustionWillDeleteStar = (Stars)Question;
                        SqlCommand CommandForDeleteQustion = null;
                        CommandForDeleteQustion = new SqlCommand(GenralVariables.DeleteStarString, Connection);
                        CommandForDeleteQustion.Parameters.AddWithValue(GenralVariables.IdQuestionWithAt, QustionWillDeleteStar.IdForType);
                        CommandForDeleteQustion.Connection.Open();
                        int NumberOfRowsaffected = CommandForDeleteQustion.ExecuteNonQuery();
                        CommandForDeleteQustion.Parameters.Clear();
                        int ResultOfDelete = DeleteQustion(Question.Id);
                        if (NumberOfRowsaffected >= 1)
                        {
                            if (ResultOfDelete == GenralVariables.Succeeded)
                            {
                                return GenralVariables.Succeeded;
                            }
                        }
                        else
                        {
                            GenralVariables.Errors.Log(GenralVariables.ErrorDeleteStarInLog);
                            return GenralVariables.ErrorInOperation;
                        }
                        return ResultOfDelete;
                    }
                }
                return ResultOfBulid;  
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInDeleteQuestion; 
            }
        }
        /// <summary>
        /// Get all my question from database as list and return the list to manger
        /// </summary>
        public static int GetQuestionFromDataBase(ref List<Qustion> TempListOfQustion)
        {
            try
            {
                int ResultInBulid = BuildConnectionString();
                if (ResultInBulid == GenralVariables.Succeeded)
                {
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
                    return GenralVariables.Succeeded;
                }
                return ResultInBulid; 
            }
            catch (Exception ex)
            {
                GenralVariables.Errors.Log(ex.Message);
                return GenralVariables.ErrorInGetQuestion;
            }
        }

        
    }
}
