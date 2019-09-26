using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models.ModelBinders
{
    //public class WineModelBinder : IModelBinder
    //{
    //    public object BindModelAsnyc(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var request = controllerContext.HttpContext.Request;

    //        string first_name = request.Form.("first_name");
    //        string middle_name = request.Form.Get("middle_name");
    //        string last_name = request.Form.Get("last_name");
    //        int Age = Convert.ToInt32(request.Form.Get("age"));
    //        return new Person { full_name = first_name + middle_name + last_name, Age = Age };
    //    }
    //}
}
