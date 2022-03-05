using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MarkalarController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();
        public ActionResult Index()
        {
            var model = entity.Markalar.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult MarkaEkle()
        {
            KategorileriGetir();
            return View();
        }

        private void KategorileriGetir()
        {
            List<SelectListItem> selectItems = new List<SelectListItem>(from x in entity.Kategoriler
                                                                        select new SelectListItem
                                                                        {
                                                                            Value = x.Id.ToString(),
                                                                            Text = x.KategoriAdi
                                                                        }).ToList();
            ViewBag.l = selectItems;
        }

        [HttpPost]
        public ActionResult MarkaEkle(Markalar marka)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.KategoriId = new SelectList(entity.Kategoriler, "Id", "KategoriAdi", marka.KategoriId);
                return View();
            }
            entity.Entry(marka).State = System.Data.Entity.EntityState.Added;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MarkaGuncellemeGetir(int id)
        {
            KategorileriGetir();
            var model = entity.Markalar.Find(id);
            return View(model);
        }

        public ActionResult MarkaGuncelle(Markalar marka)
        {
            if (!ModelState.IsValid)
            {
                KategorileriGetir();
                return View("MarkaGuncellemeGetir");
            }
            entity.Entry(marka).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult MarkaSilmeGetir(int id)
        {
            var model = entity.Markalar.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult MarkaSil(Markalar marka)
        {
            entity.Entry(marka).State = System.Data.Entity.EntityState.Deleted;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Urunler(int id)
        {
            var model = entity.Urunler.Where(I => I.Markalar.Id == id).ToList();
            var kategori = entity.Markalar.Where(I => I.Id == id).Select(I => I.Kategoriler.KategoriAdi).FirstOrDefault();
            var marka = entity.Markalar.Where(I => I.Id == id).Select(I => I.MarkaAdi).FirstOrDefault();
            ViewBag.k = kategori;
            ViewBag.m = marka;
            return View(model);
        }
    }
}