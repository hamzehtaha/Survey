using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationManger;
using Question;
using SurveyWebSite.Models;
using OperationManger;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace SurveyWebSite.Controllers
{
    public class QuestionController : Controller
    {
        public static FormCollection Form = new FormCollection();
        private static BaseLog.Logger Logger = new BaseLog.Logger();
        public static string  SessionID = "" ;
        private static List<Qustion> ListOfQuestions = new List<Qustion>(); 
        // GET: Question
        /// <summary>
        /// This Home View get all question from my list in manger to show it
        /// </summary>
        public ActionResult Home()
        {
            try
            {
                string sessionID = HttpContext.Session.SessionID;
                SessionID = sessionID; 
                Operation.SessionFlags.Add(sessionID, false); 
                var ListOfQuestion = Operation.GetAllQuestion();
                ListOfQuestions = Operation.ListOfAllQuestion; 

                return View(ListOfQuestion);
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorHome });
            }
        }

        /// <summary>
        /// This for refresh my partail view atfer any edit in list 
        /// </summary>
        /// <returns></returns>
        public ActionResult RefreshData()
        {
            try
            {
                if (Operation.SessionFlags[SessionID])
                return PartialView(@SurveyWebSite.Resources.Constants.PartailList, Operation.ListOfAllQuestion);
                else
                    return PartialView(@SurveyWebSite.Resources.Constants.PartailList, ListOfQuestions);

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorRefrsh });
            }
        }
        /// <summary>
        /// This create view for get a question to add it 
        /// 1 mean slider question 
        /// 2 mean Smile question 
        /// 3 mean Star question
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(int Type)
        {
            try
            {
                switch (Type) {
                    case 1:
                        return View(new Slider());
                    case 2:
                        return View(new Smiles());
                    case 3:
                        return View(new Stars());
                    default:
                        return View(@SurveyWebSite.Resources.Messages.ErrorCreate);
                }

                
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorCreate});
            }
        }
        /// <summary>
        /// After enter the data and check of validate then call model binder to build the object 
        /// then add it in database using manger class
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([ModelBinder(typeof(QustionModelBinder))]Qustion NewQuestion)
        {
            try
            {
                int ResultOfCheck = Operation.CheckTheData(NewQuestion);
                if (ResultOfCheck == OperationManger.GenralVariables.Succeeded)
                {
                   int ResultOfCreate =  Operation.AddQustion(NewQuestion);
                    if (ResultOfCreate == OperationManger.GenralVariables.Succeeded)
                    {
                        ModelState.Clear();
                        return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
                    }
                    else
                    {
                        string Error = Operation.CheckMessageError(ResultOfCreate);
                        return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage =Error}); 
                    }
                }
                else
                {
                    ViewBag.FailMessage = Operation.CheckMessageError(ResultOfCheck);
                    return View(NewQuestion); 
                }
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorCreate });
            }
        }
        /// <summary>
        /// This function take ID to show the data will delete it
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var QuestionWillDelete = Operation.SelectById(id);
                if (QuestionWillDelete == null)
                {
                    return View(@SurveyWebSite.Resources.Constants.ErrorNotFound);
                }
                return View(QuestionWillDelete);
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorDelete });
            }
        }
        /// <summary>
        /// After user press Yes will go to delete post to delete it from database using mnager
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id ,FormCollection form)
        {
            try
            {
                var QuestionWillDelete = Operation.SelectById(id);
                int ResultOfDelete =  Operation.DeleteQustion(QuestionWillDelete);
                if (ResultOfDelete == OperationManger.GenralVariables.Succeeded)
                {
                    return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
                }
                else
                {
                    string Error = Operation.CheckMessageError(ResultOfDelete);
                    return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = Error });
                }
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorDelete });
            }
        }
        /// <summary>
        /// This get method take id the question to show the data  
        /// </summary>
        [HttpGet]
        public ActionResult Edit (int Id)
        {
            try
            {
                var ObjectWillEdit = Operation.SelectById(Id);
                if (ObjectWillEdit == null)
                {
                    return HttpNotFound();
                }
                if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Slider)
                {
                    var SliderEdit = (Slider)ObjectWillEdit;
                    Form[SurveyWebSite.Resources.Constants.ID] = SliderEdit.Id.ToString();
                    Form[SurveyWebSite.Resources.Constants.IdForType] = SliderEdit.IdForType.ToString();
                    return View(SliderEdit);
                }
                else if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Smily)
                {
                    var SmileEdit = (Smiles)ObjectWillEdit;
                    Form[SurveyWebSite.Resources.Constants.ID] = SmileEdit.Id.ToString();
                    Form[SurveyWebSite.Resources.Constants.IdForType] = SmileEdit.IdForType.ToString();
                    return View(SmileEdit);
                }
                else if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Stars)
                {
                    var StarForEdit = (Stars)ObjectWillEdit;
                    Form[SurveyWebSite.Resources.Constants.ID] = StarForEdit.Id.ToString();
                    Form[SurveyWebSite.Resources.Constants.IdForType] = StarForEdit.IdForType.ToString();
                    return View(StarForEdit);
                }
                return View();
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorEdit });
            }
                
        }
        /// <summary>
        /// Edit post when user press yes will check the validate data then call edit function from manger
        /// </summary>
        /// <param name="NewQuestion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([ModelBinder(typeof(QustionModelBinder))] Qustion NewQuestion)
        {
            try
            {
                NewQuestion.Id = Convert.ToInt32(Form[SurveyWebSite.Resources.Constants.ID]);
                int ResultOfCheck = Operation.CheckTheData(NewQuestion);
                if (ResultOfCheck == OperationManger.GenralVariables.Succeeded)
                {
                    int ResultOfEdit = Operation.EditQustion(NewQuestion);
                    if (ResultOfEdit == OperationManger.GenralVariables.Succeeded)
                    {
                        ModelState.Clear();
                        return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
                    }
                    else
                    {
                        string Error = Operation.CheckMessageError(ResultOfEdit);
                        return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = Error });
                    }
                }
                else
                {
                    ViewBag.FailMessage = Operation.CheckMessageError(ResultOfCheck);
                    return View(NewQuestion);
                }
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorEdit });
            }
        }
        /// <summary>
        /// To change lanuage and take the language  
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeLanuage (string Language)
        {
            try
            {
                if (!String.IsNullOrEmpty(Language))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Language);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                }
                HttpCookie cookie = new HttpCookie(SurveyWebSite.Resources.Constants.Languages);
                cookie.Value = Language;
                Response.Cookies.Add(cookie);
                return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorChangeLanguage });
            }
        }
        /// <summary>
        /// This errro view take the error and show it in error view any error 
        /// </summary>
        public ActionResult ErrorView (string ErrorMessage)
        {
            try
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View(); 
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View(); 
            }
        }
    }
}