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
                    int order = Convert.ToInt32(request.Form.Get("Order"));
                    if (request.Form.Get("StartValue") != null)
                    {
                        int StartValue = Convert.ToInt32(request.Form.Get("StartValue"));
                        int EndValue = Convert.ToInt32(request.Form.Get("EndValue"));
                        string StarCaption = request.Form.Get("StartCaption");
                        string EndCaption = request.Form.Get("EndCaption");
                        Slider NewSlider = new Slider();
                        NewSlider.NewText = text;
                        NewSlider.Order = order;
                        NewSlider.TypeOfQuestion = TypeOfQuestion.Slider;
                        NewSlider.StartValue = StartValue;
                        NewSlider.EndValue = EndValue;
                        NewSlider.StartCaption = StarCaption;
                        NewSlider.EndCaption = EndCaption;
                        return NewSlider;
                    } else if (request.Form.Get("NumberOfSmiles") != null)
                    {
                        int NumberOfSmile = Convert.ToInt32(request.Form.Get("NumberOfSmiles"));
                        Smiles NewSmile = new Smiles();
                        NewSmile.NewText = text;
                        NewSmile.Order = order;
                        NewSmile.TypeOfQuestion = TypeOfQuestion.Smily;
                        NewSmile.NumberOfSmiles = NumberOfSmile;
                        return NewSmile;
                    } else if (request.Form.Get("NumberOfStars") != null)
                    {
                        int NumberOfStar = Convert.ToInt32(request.Form.Get("NumberOfStars"));
                        Stars NewStar = new Stars();
                        NewStar.NewText = text;
                        NewStar.Order = order;
                        NewStar.TypeOfQuestion = TypeOfQuestion.Stars;
                        NewStar.NumberOfStars = NumberOfStar;
                        return NewStar;
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