using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationManger;
using Question; 
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
        public ActionResult Create()
        {
            return View(); 
        }


        public ActionResult CreateSlider()
        {
            Slider NewSlider = new Slider();
            return View(NewSlider);
        }
        [HttpPost]
        public ActionResult CreateSlider(Slider NewSlider)
        {
            Operation.AddQustion(NewSlider);
            ModelState.Clear();
            ViewBag.SuccessMessage = "The Question is Added";
            return View("CreateSlider", new Slider());
        }


        public ActionResult CreateSmile()
        {
            Smiles NewSmile = new Smiles();
            
            return View(NewSmile); 
        }
        [HttpPost]
        public ActionResult CreateSmile(Smiles NewSmile)
        {
            NewSmile.TypeOfQuestion = TypeOfQuestion.Smily;
            int result  = Operation.AddQustion(NewSmile);
            ModelState.Clear();
            ViewBag.SuccessMessage = "The Question is Added";
            return View("CreateSmile", new Smiles());
        }

        public ActionResult CreateStar()
        {
            Stars NewStar = new Stars();
            return View(NewStar);
        }
        [HttpPost]
        public ActionResult CreateStar(Stars NewStar)
        {
            NewStar.TypeOfQuestion = TypeOfQuestion.Stars; 
            Operation.AddQustion(NewStar);
            ModelState.Clear();
            ViewBag.SuccessMessage = "The Question is Added" + NewStar.TypeOfQuestion;
            return View("CreateStar", new Stars());
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






    }
}