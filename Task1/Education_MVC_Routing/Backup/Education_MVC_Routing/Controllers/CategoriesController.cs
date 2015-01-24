using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Education_MVC_Routing.Controllers
{
    public class CategoriesController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Category(string categoryName)
        {
            return View();
        }

        public ActionResult SubCategories(string categoryName)
        {
            return View();
        }

        public ActionResult SubCategory(string categoryName, string subCategoryName)
        {
            return View();
        }
    }
}
