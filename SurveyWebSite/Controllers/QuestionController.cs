using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationManger;
using Question;
using SurveyWebSite.Models;

namespace SurveyWebSite.Controllers
{
    public class QuestionController : Controller
    {
        // GET: Question
        public ActionResult Index()
        {
            var l = Operation.GetAll();
            return View(l);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(QustionModelBinder))]Qustion NewQuestion)
        {
                Operation.AddQustion(NewQuestion);
                ModelState.Clear();
                ViewBag.SuccessMessage = "The Question is Added";
            return View();
        }

        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var QuestionWillDelete = Operation.SelectById(id); 
            if (QuestionWillDelete == null)
            {
                return View("Not Found"); 
            }
            return View(QuestionWillDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id ,FormCollection form)
        {
            var QuestionWillDelete = Operation.SelectById(id);
            Operation.DeleteQustion(QuestionWillDelete);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit (int Id)
        {
            var ObjectWillEdit = Operation.SelectById(Id);
            if (ObjectWillEdit == null)
            {
                return HttpNotFound(); 
            }
            if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Slider)
            {
                var SliderEdit = (Slider)ObjectWillEdit;
                FormCollection Form = new FormCollection();
                Form["Text"] = SliderEdit.NewText;
                Form["Order"] = SliderEdit.Order.ToString();
                Form["StartV"] = SliderEdit.StartValue.ToString();
                Form["EndV"] = SliderEdit.EndValue.ToString();
                Form["StartC"] = SliderEdit.StartCaption.ToString();
                Form["EndC"] = SliderEdit.EndCaption.ToString();

                return View(SliderEdit);
            }
            else if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Smily)
            {
                var SmileEdit = (Smiles)ObjectWillEdit;
                return View(SmileEdit);
            }else if (ObjectWillEdit.TypeOfQuestion == TypeOfQuestion.Stars)
            {
                var StarForEdit = (Stars)ObjectWillEdit;
                return View(StarForEdit);
            }
            return View(); 
                
        }
        







    }
}