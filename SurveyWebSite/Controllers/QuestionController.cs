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

namespace SurveyWebSite.Controllers
{
    public class QuestionController : Controller
    {

        public static FormCollection Form = new FormCollection();
        private static BaseLog.Logger Logger = new BaseLog.Logger();
        private static List<Qustion> MyListOfAllQuestion = new List<Qustion>();
        private static string Language = "en";
        // GET: Question
        [ActionName("Index")]

        public ActionResult Index(string language)
        {
            try
            {
                Operation.PutListToShow = AutoRefresh;
                // MyListOfAllQuestion = Operation.ListOfAllQuestion;
                Operation.GetQustion(ref MyListOfAllQuestion); 
                int C = MyListOfAllQuestion.Count; 
                 if (!String.IsNullOrEmpty(language))
                 {
                     Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
                     Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                 }
                Language = language;
                HttpCookie cookie = new HttpCookie("Languages");
                cookie.Value = language;
                Response.Cookies.Add(cookie);
                Operation.RefreshData();
                return View(MyListOfAllQuestion);
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        public void AutoRefresh()
        {
            try
            {

                RefreshPartailView();
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                
            }
            
        }

        private ActionResult RefreshPartailView()
        {
            try
            {
                return RedirectToAction("Index");
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }

        public ActionResult GetView()
        {
            try
            {
                return PartialView("_List", MyListOfAllQuestion);
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        public ActionResult BackIndex()
        {
            try
            {
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                ViewBag.JS = "ReloadPage();";
                return View();
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(QustionModelBinder))]Qustion NewQuestion)
        {
            try
            {
                
                Operation.AddQustion(NewQuestion);
                ModelState.Clear();
                ViewBag.SuccessMessage = SurveyWebSite.Resources.Resource.AddQuestion;
                return View();
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }

        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var QuestionWillDelete = Operation.SelectById(id);
                if (QuestionWillDelete == null)
                {
                    return View("Not Found");
                }
                return View(QuestionWillDelete);
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id ,FormCollection form)
        {
            try
            {
                var QuestionWillDelete = Operation.SelectById(id);
                Operation.DeleteQustion(QuestionWillDelete);
                return RedirectToAction("Index");
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
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
                    Form["Id"] = SliderEdit.Id.ToString();
                    Form["IdForType"] = SliderEdit.IdForType.ToString();
                    return View(SliderEdit);
                }
                else if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Smily)
                {
                    var SmileEdit = (Smiles)ObjectWillEdit;
                    Form["Id"] = SmileEdit.Id.ToString();
                    Form["IdForType"] = SmileEdit.IdForType.ToString();
                    return View(SmileEdit);
                }
                else if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Stars)
                {
                    var StarForEdit = (Stars)ObjectWillEdit;
                    Form["Id"] = StarForEdit.Id.ToString();
                    Form["IdForType"] = StarForEdit.IdForType.ToString();
                    return View(StarForEdit);
                }
                return View();
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
                
        }
        [HttpPost]
        public ActionResult Edit([ModelBinder(typeof(QustionModelBinder))] Qustion NewQuestion)
        {
            try
            {
                NewQuestion.Id = Convert.ToInt32(Form["Id"]);
                if (NewQuestion.TypeOfQuestion == TypeOfQuestion.Slider)
                {
                    Slider SliderForEdit = (Slider)NewQuestion;
                    SliderForEdit.IdForType = Convert.ToInt32(Form["IdForType"]);
                    Operation.EditQustion(SliderForEdit);
                    ViewBag.SuccessMessage = SurveyWebSite.Resources.Resource.EditQuestionSlider;
                    ModelState.Clear();
                    return View(NewQuestion);
                }
                else if (NewQuestion.TypeOfQuestion == TypeOfQuestion.Smily)
                {
                    Smiles SmileForEdit = (Smiles)NewQuestion;
                    SmileForEdit.IdForType = Convert.ToInt32(Form["IdForType"]);
                    Operation.EditQustion(SmileForEdit);
                    ViewBag.SuccessMessage = SurveyWebSite.Resources.Resource.EditQuestionSmile;
                    ModelState.Clear();
                    return View(NewQuestion);

                }
                else if (NewQuestion.TypeOfQuestion == TypeOfQuestion.Stars)
                {
                    Stars StarForEdit = (Stars)NewQuestion;
                    StarForEdit.IdForType = Convert.ToInt32(Form["IdForType"]);
                    Operation.EditQustion(StarForEdit);
                    ViewBag.SuccessMessage = SurveyWebSite.Resources.Resource.EditQuestionStar;
                    ModelState.Clear();
                    return View(NewQuestion);
                }
                return View("Not Found");
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
    }
}