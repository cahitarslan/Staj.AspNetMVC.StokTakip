using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BirimlerController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();

        public ActionResult Index()
        {
            return View(entity.Birimler.ToList());
        }

        [HttpGet]
        public ActionResult BirimEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BirimEkle(Birimler birim)
        {
            if (!ModelState.IsValid) return View("BirimEkle");
            entity.Birimler.Add(birim);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BirimGuncellemeGetir(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var model = entity.Birimler.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult BirimGuncelle(Birimler birim)
        {
            entity.Entry(birim).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BirimSilmeGetir(int id)
        {
            var model = entity.Birimler.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult BirimSil(Birimler birim)
        {
            entity.Entry(birim).State = System.Data.Entity.EntityState.Deleted;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}