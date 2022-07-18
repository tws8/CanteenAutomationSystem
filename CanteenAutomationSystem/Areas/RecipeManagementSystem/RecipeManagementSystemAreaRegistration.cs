using System.Web.Mvc;

namespace CanteenAutomationSystem.Areas.RecipeManagementSystem
{
    public class RecipeManagementSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RecipeManagementSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RecipeManagementSystem_default",
                "RecipeManagementSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}