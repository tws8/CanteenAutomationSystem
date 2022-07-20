using CanteenAutomationSystem.Models;
using System.Collections.Generic;

namespace CanteenAutomationSystem.DAL
{
    public class CanteenInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CanteenContext>
    {
        protected override void Seed(CanteenContext context)
        {
            var bizDept = new List<BizDept>()
            {
                new BizDept{BizDeptID="A", BizDeptDesc="Administrator"},
                new BizDept{BizDeptID="B", BizDeptDesc="Buyer Staff"},
                new BizDept{BizDeptID="F", BizDeptDesc="Financial Staff"},
                new BizDept{BizDeptID="K", BizDeptDesc="Kitchen Staff"},
                new BizDept{BizDeptID="M", BizDeptDesc="Management"},
                new BizDept{BizDeptID="R", BizDeptDesc="Receptionist Staff"},
            };

            bizDept.ForEach(x => context.BizDepts.Add(x));
            context.SaveChanges();
            
            var bizUsers = new List<BizUser>()
            {
                new BizUser{BizUserID="ADMIN", BizUserPW="1/uHt3MTpjk=", BizUserName="Administrator", BizDeptID="A"},
                new BizUser{BizUserID="BUYER", BizUserPW="lWOwPa5b4LM=", BizUserName="Buyer Staff", BizDeptID="B"},
                new BizUser{BizUserID="FINANCE", BizUserPW="glUu08T4lcc=", BizUserName="Financial Staff", BizDeptID="F"},
                new BizUser{BizUserID="KITCHEN", BizUserPW="pb5BJUtffok=", BizUserName="Kitchen Staff", BizDeptID="K"},
                new BizUser{BizUserID="MANAGER", BizUserPW="fNbzLAIS3Pc=", BizUserName="Management", BizDeptID="M"},
                new BizUser{BizUserID="RECEPTION", BizUserPW="91R9Ww4NyY0+jjEcQl2VoA==", BizUserName="Receptionist Staff", BizDeptID="R"},
            };

            bizUsers.ForEach(x => context.BizUsers.Add(x));
            context.SaveChanges();

            var customers = new List<Customer>()
            {
                new Customer{CustID="CUST", CustPW="rPx5pIzrfro=", CustName="Customer", CustMemberStatus="N", BalCredit=0},
            };

            customers.ForEach(x => context.Customers.Add(x));
            context.SaveChanges();

            var vendors = new List<Vendor>()
            {
                new Vendor{VendorID=1, VendorName="AEON"}
            };

            vendors.ForEach(x => context.Vendors.Add(x));
            context.SaveChanges();

            var categorys = new List<Category>()
            {
                new Category{CategoryID="Drink", CategoryDesc="Drinks"},
                new Category{CategoryID="Chicken", CategoryDesc="Chicken Meat"},
            };

            categorys.ForEach(x => context.Categorys.Add(x));
            context.SaveChanges();

            var ingredients = new List<Ingredient>()
            {
                new Ingredient{IngredientID=1, IngredientDesc="Egg", IngredientQty=2},
                new Ingredient{IngredientID=2, IngredientDesc="Flour 100g", IngredientQty=1},
                new Ingredient{IngredientID=3, IngredientDesc="Sugar 12g", IngredientQty=3},
                new Ingredient{IngredientID=4, IngredientDesc="Milk 1 litre", IngredientQty=1},
                new Ingredient{IngredientID=5, IngredientDesc="Mineral Water 1 litre", IngredientQty=1},
            };

            ingredients.ForEach(x => context.Ingredients.Add(x));
            context.SaveChanges();

            var foods = new List<Food>()
            {
                new Food{FoodID=1, FoodDesc="Sky Juice", FoodPrice=(decimal)0.50, FoodRem = "Normal Water", Category="Drink", Url = "https://images.says.com/uploads/story_source/source_image/523572/9197.jpg", Status = "A"},
                new Food{FoodID=2, FoodDesc="Coffee", FoodPrice=(decimal)2.50, FoodRem = "With Sugar", Category="Drink", Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/45/A_small_cup_of_coffee.JPG/1200px-A_small_cup_of_coffee.JPG", Status = "A"},
                new Food{FoodID=3, FoodDesc="Hot Green Tea", FoodPrice=(decimal)1.50, FoodRem = "", Category="Drink", Url = "https://thumbs.dreamstime.com/z/japanese-hot-green-tea-japanese-hot-green-tea-ceramic-bowl-107066766.jpg", Status = "A"},
                new Food{FoodID=4, FoodDesc="Chicken Chop", FoodPrice=(decimal)25.50, FoodRem = "With Black Pepper Saurce", Category="Chicken", Url = "https://thumbs.dreamstime.com/b/chicken-chop-chips-15693245.jpg", Status = "A"},
                new Food{FoodID=5, FoodDesc="Chicken Cutlet", FoodPrice=(decimal)20.50, FoodRem = "", Category="Chicken", Url = "https://media-cldnry.s-nbcnews.com/image/upload/newscms/2021_35/1767081/chicken-cutlet-kb-square-210830.jpg", Status = "A"},
            };

            foods.ForEach(x => context.Foods.Add(x));
            context.SaveChanges();

            var foodDet = new List<FoodDet>()
            {
                new FoodDet{FoodID=1, FoodDetID=1, IngredientID=1, IngredientQty=5, Status="A"},
                new FoodDet{FoodID=1, FoodDetID=2, IngredientID=2, IngredientQty=1, Status="A"},
                new FoodDet{FoodID=1, FoodDetID=3, IngredientID=3, IngredientQty=10, Status="A"},
                new FoodDet{FoodID=4, FoodDetID=1, IngredientID=1, IngredientQty=1, Status="A"},
                new FoodDet{FoodID=4, FoodDetID=2, IngredientID=5, IngredientQty=2, Status="A"},
            };

            foodDet.ForEach(x => context.FoodDets.Add(x));
            context.SaveChanges();
        }
    }
}