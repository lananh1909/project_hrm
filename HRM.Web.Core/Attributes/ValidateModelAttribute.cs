using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Core
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Dictionary<string, ModelErrorCollection> errors = new Dictionary<string, ModelErrorCollection>();
                StringBuilder builder = new StringBuilder();
                foreach (var modelState in context.ModelState)
                {
                    if(modelState.Value.Errors.Count > 0)
                    {
                        foreach (var error in modelState.Value.Errors)
                        {
                            builder.Append(error.ErrorMessage);
                        }
                    }
                }
                context.Result = new BadRequestObjectResult(new ErrorServiceResult()
                {
                    DevMessage = builder.ToString(),
                    UserMessage = ErrorConstants.ErrorRequiredModelMessage,
                    ErrorCode = ErrorConstants.ErrorRequiredModel,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }
    }
}
