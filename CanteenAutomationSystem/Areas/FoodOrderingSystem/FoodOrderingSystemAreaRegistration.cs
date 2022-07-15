using System.Web.Mvc;

namespace CanteenAutomationSystem.Areas.FoodOrderingSystem
{
    public class FoodOrderingSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FoodOrderingSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FoodOrderingSystem_default",
                "FoodOrderingSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}