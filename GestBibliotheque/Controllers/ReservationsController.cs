using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestBibliotheque.Controllers
{
    public class ReservationsController : Controller
    {
        // GET: ReservationsController
        public ActionResult Index()
        {
            return View();
        }

       
    }
}
