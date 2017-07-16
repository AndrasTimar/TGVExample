using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AndrasTimarTGV.Util.Filters
{
    public class ModelStateValidityActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
                context.Result = new ContentResult() {
                    Content = "Model state not valid",
                    StatusCode = 400
                };
            base.OnActionExecuting(context);
        }  
    }
}
