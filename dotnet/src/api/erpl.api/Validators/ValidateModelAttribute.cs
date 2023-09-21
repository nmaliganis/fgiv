using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace erpl.api.Validators;

internal class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result = new BadRequestObjectResult(context.ModelState);
    }

}//Class : ValidateModelAttribute