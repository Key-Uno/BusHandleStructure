using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UseCase.Config;

namespace NET6.Controllers
{
    public class BaseController : Controller
    {
        private readonly UseCaseBus bus;

        public BaseController(UseCaseBus bus)
        {
            this.bus = bus;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
