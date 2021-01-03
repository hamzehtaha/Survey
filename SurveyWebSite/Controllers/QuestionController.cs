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
        // GET: Question
        [ActionName("Home")]
        public ActionResult Home(string language)
        {
            try
            {
                var ListOfQuestion = Operation.GetAllQuestion(); 
                 if (!String.IsNullOrEmpty(language))
                 {
                     Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
                     Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                 }
                HttpCookie cookie = new HttpCookie("Languages");
                cookie.Value = language;
                Response.Cookies.Add(cookie);
                return View(ListOfQuestion);
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
                Page page = new Page();
                ScriptManager.RegisterStartupScript(page, this.GetType(), "Refresh", "GetData()", true);
                RefreshPartailView(); 
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                
            }
            
        }
        public ActionResult RefreshPartailView()
        {
            try
            {
                ViewBag.JS = "GetData();";
                return View("Index"); 
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        public ActionResult GetView()
        {
            try
            {
                return PartialView("_List", Operation.ListOfAllQuestion);
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        public ActionResult SortByText()
        {
            try
            {
                Operation.ListOfAllQuestion = Operation.ListOfAllQuestion.OrderBy(r => r.NewText).ToList(); 
                return PartialView("_List", Operation.ListOfAllQuestion);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        public ActionResult SortByOrder()
        {
            try
            {
                Operation.ListOfAllQuestion = Operation.ListOfAllQuestion.OrderBy(r => r.Order).ToList();
                return PartialView("_List", Operation.ListOfAllQuestion);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
        public ActionResult SortByType()
        {
            try
            {
                Operation.ListOfAllQuestion = Operation.ListOfAllQuestion.OrderBy(r => r.TypeOfQuestion).ToList();
                return PartialView("_List", Operation.ListOfAllQuestion);
            }
            catch (Exception ex)
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([ModelBinder(typeof(QustionModelBinder))]Qustion NewQuestion)
        {
            try
            {
                int ResultOfCheck = Operation.CheckTheData(NewQuestion);


                if (ResultOfCheck == OperationManger.GenralVariables.Succeeded)
                {
                        Operation.AddQustion(NewQuestion);
                        ModelState.Clear();
                    ViewBag.SuccessMessage = Operation.CheckMessageError(ResultOfCheck); 
                        return View();
                }
                else
                {
                    ViewBag.FailMessage = Operation.CheckMessageError(ResultOfCheck);
                    return View(); 
                }
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
                return RedirectToAction("Home");
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
                int ResultOfCheck = Operation.CheckTheData(NewQuestion);
                if (ResultOfCheck == OperationManger.GenralVariables.Succeeded)
                {
                    Operation.EditQustion(NewQuestion);
                    ModelState.Clear();
                    ViewBag.SuccessMessage = Operation.CheckMessageError(ResultOfCheck);
                    return View(NewQuestion);
                }
                else
                {
                    ViewBag.FailMessage = Operation.CheckMessageError(ResultOfCheck);
                    return View("Not Found");
                }
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View("Not Found");
            }
        }
    }
}