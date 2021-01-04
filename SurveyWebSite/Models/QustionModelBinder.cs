using Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWebSite.Models
{
    public class QustionModelBinder : DefaultModelBinder
    {
        private static BaseLog.Logger Logger = new BaseLog.Logger();
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try {
                if (bindingContext.ModelType == typeof(Qustion))
                {
                    
                    HttpRequestBase request = controllerContext.HttpContext.Request;
                    string text = request.Form.Get("NewText");
                    string order = request.Form.Get("Order"); 
                    if (request.Form.Get("StartValue") != null)
                    {
                        string StartValue = request.Form.Get("StartValue");
                        string EndValue = request.Form.Get("EndValue");
                        string StarCaption = request.Form.Get("StartCaption");
                        string EndCaption = request.Form.Get("EndCaption");

                        Slider NewSlider = new Slider();
                        NewSlider.NewText = text;
                        if (!String.IsNullOrEmpty(order))
                        {
                            NewSlider.Order = Convert.ToInt32(order);
                        }
                        NewSlider.TypeOfQuestion = TypeOfQuestion.Slider;
                        if (!String.IsNullOrEmpty(StartValue))
                        {
                            NewSlider.StartValue = Convert.ToInt32(StartValue);
                        }
                        if (!String.IsNullOrEmpty(EndValue))
                        {
                            NewSlider.EndValue = Convert.ToInt32(EndValue);
                        }
                        NewSlider.StartCaption = StarCaption;
                        NewSlider.EndCaption = EndCaption;
                        return NewSlider;
                    } else if (request.Form.Get("NumberOfSmiles") != null)
                    {
                        
                        string NumberOfSmile = request.Form.Get("NumberOfSmiles");
                        Smiles NewSmile = new Smiles();
                        NewSmile.NewText = text;
                        if (!String.IsNullOrEmpty(order))
                        {
                            NewSmile.Order = Convert.ToInt32(order);
                        }
                        if (!String.IsNullOrEmpty(order))
                        {
                            NewSmile.NumberOfSmiles = Convert.ToInt32(NumberOfSmile);
                        }
                        NewSmile.TypeOfQuestion = TypeOfQuestion.Smily;
                        return NewSmile;
                    } else if (request.Form.Get("NumberOfStars") != null)
                    {
                        
                        string NumberOfStar = request.Form.Get("NumberOfStars");
                        Stars NewStar = new Stars();
                        NewStar.NewText = text;
                        if (!String.IsNullOrEmpty(order))
                        {
                            NewStar.Order = Convert.ToInt32(order);
                        }
                        if (!String.IsNullOrEmpty(order))
                        {
                            NewStar.NumberOfStars = Convert.ToInt32(NumberOfStar);
                        }
                        NewStar.TypeOfQuestion = TypeOfQuestion.Stars;
                        return NewStar;
                    }else
                    {
                        Slider obj = new Slider();
                        obj.TypeOfQuestion = TypeOfQuestion.Qustions;
                        return obj; 

                    }

                }
                return base.BindModel(controllerContext, bindingContext);
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return base.BindModel(controllerContext, bindingContext);
            }
            }
    }
}