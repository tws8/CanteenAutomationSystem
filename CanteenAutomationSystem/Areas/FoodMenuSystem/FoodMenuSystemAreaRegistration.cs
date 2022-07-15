using System.Web.Mvc;

namespace CanteenAutomationSystem.Areas.FoodMenuSystem
{
    public class FoodMenuSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FoodMenuSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FoodMenuSystem_default",
                "FoodMenuSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}