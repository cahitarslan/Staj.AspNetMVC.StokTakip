using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KategorilerController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();

        public ActionResult Index()
        {
            return View(entity.Kategoriler.ToList());
        }

        [HttpGet]
        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult KategoriEkle(Kategoriler kategori)
        {
            if (!ModelState.IsValid) return View("KategoriEkle");
            entity.Kategoriler.Add(kategori);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult KategoriGuncellemeGetir(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var model = entity.Kategoriler.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }


        public ActionResult KategoriGuncelle(Kategoriler kategori)
        {
            entity.Entry(kategori).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult KategoriSilmeGetir(int id)
        {
            var model = entity.Kategoriler.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult KategoriSil(Kategoriler kategori)
        {
            entity.Entry(kategori).State = System.Data.Entity.EntityState.Deleted;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Markalar(int id)
        {
            var model = entity.Markalar.Where(I => I.Kategoriler.Id == id).ToList();
            var kategori = entity.Kategoriler.Where(I => I.Id == id).Select(I => I.KategoriAdi).FirstOrDefault();
            ViewBag.viewBagKategori = kategori;
            return View(model);
        }

        public ActionResult Urunler(int id)
        {
            var model = entity.Urunler.Where(I => I.Kategoriler.Id == id).ToList();
            var kategori = entity.Kategoriler.Where(I => I.Id == id).Select(I => I.KategoriAdi).FirstOrDefault();
            ViewBag.viewBagKategori = kategori;
            return View(model);
        }
    }
}