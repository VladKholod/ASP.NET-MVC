using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace GuessTheNumberMvc.Controllers
{
    public class HomeController : Controller
    {
        private const string Status = "Status";
        private const string ThinkedNumber = "ThinkedNumber";
        private const string Owner = "Owner";
        private const string Winner = "Winner";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PingStatus()
        {
            var status = System.Web.HttpContext.Current.Application.Get(Status);
            if (status == null)
            {
                System.Web.HttpContext.Current.Application.Set(Status, "Game not started yet!");
                return Json(new
                {
                    Status = "Game not started yet!"
                }, JsonRequestBehavior.AllowGet);
            }

            var winner = System.Web.HttpContext.Current.Application.Get(Winner);
            var thinkedNumber = System.Web.HttpContext.Current.Application.Get(ThinkedNumber);

            return Json(new
            {
                Status = status,
                Winner = winner,
                ThinkedNumber = thinkedNumber
            },JsonRequestBehavior.AllowGet);
        }

        private void CleanAttemptsList()
        {
            var keysList = System.Web.HttpContext.Current.Application.AllKeys.ToList();
            keysList.Remove(Owner);
            keysList.Remove(Status);
            keysList.Remove(Winner);
            keysList.Remove(ThinkedNumber);

            foreach (var key in keysList)
            {
                System.Web.HttpContext.Current.Application.Remove(key);
            }
        }

        public ActionResult ThinkupNumber(int number, string userName)
        {
            var owner = System.Web.HttpContext.Current.Application.Get(Owner);

            System.Web.HttpContext.Current.Application.Set(ThinkedNumber, number);
            System.Web.HttpContext.Current.Application.Set(Owner, userName);
            System.Web.HttpContext.Current.Application.Set(Status,"Game started!");
            System.Web.HttpContext.Current.Application.Set(Winner, null);

            var status = "Game started!";
            if (owner != null)
            {
                status = "New game started!";
            }

            CleanAttemptsList();

            return Json(new
            {
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GuessNumber(int number, string userName)
        {
            var thinkedNumber = (int)System.Web.HttpContext.Current.Application.Get(ThinkedNumber);
            var owner = System.Web.HttpContext.Current.Application.Get(Owner);
            var status = System.Web.HttpContext.Current.Application.Get(Status);
            var history = (List<int>)System.Web.HttpContext.Current.Application.Get(userName);
            var winner = System.Web.HttpContext.Current.Application.Get(Winner);

            if (owner == null && winner == null)
            {
                return Json(new
                {
                    Status = "Game not started yet!"
                }, JsonRequestBehavior.AllowGet);
            }

            if (winner != null)
            {
                return Json(new
                {
                    Status = string.Format("Winner is {0}. Thinked number is {1}",winner,thinkedNumber)
                }, JsonRequestBehavior.AllowGet);
            }

            if (userName == owner.ToString())
            {
                return Json(new
                {
                    Status = status,
                    Message = "You can't play, cuz you are owner!"
                }, JsonRequestBehavior.AllowGet);
            }

            if (history == null)
            {
                history = new List<int>();
            }
            history.Add(number);

            System.Web.HttpContext.Current.Application.Set(userName, history);

            if (number == thinkedNumber)
            {
                System.Web.HttpContext.Current.Application.Set(Winner, userName);
                return Json(new
                {
                    Status = "You won!"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Message = number > thinkedNumber ? "Thinked number less than your!" : "Thinked number bigger than your!",
                Status = status,
                History = history
            }, JsonRequestBehavior.AllowGet);
        }
    }
}