using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [Authorize]
    public class SepetController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();
        public ActionResult Index(decimal? tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciAdi = User.Identity.Name;
                var kullanici = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == kullaniciAdi);
                var model = entity.Sepet.Where(I => I.KullaniciId == kullanici.Id).ToList();
                var kid = entity.Sepet.FirstOrDefault(I => I.KullaniciId == kullanici.Id);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmuyor";
                    }
                    else if (kid != null)
                    {
                        tutar = entity.Sepet.Where(I => I.KullaniciId == kid.KullaniciId).Sum(I => I.ToplamFiyati);
                        ViewBag.Tutar = "Toplam Tutar: " + tutar + " TL";
                    }
                    return View(model);

                }
            }
            return HttpNotFound();
        }

        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciAdi = User.Identity.Name;
                var model = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == kullaniciAdi);
                var u = entity.Urunler.Find(id);
                var sepet = entity.Sepet.FirstOrDefault(I => I.KullaniciId == model.Id && I.UrunId == id);
                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Miktari++;
                        sepet.ToplamFiyati = u.SatisFiyati * sepet.Miktari;
                        entity.SaveChanges();
                        return RedirectToAction("Index", "Urunler");
                    }

                    var s = new Sepet
                    {
                        KullaniciId = model.Id,
                        UrunId = u.Id,
                        Miktari = 1,
                        BirimFiyati = u.SatisFiyati,
                        ToplamFiyati = u.SatisFiyati,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    entity.Entry(s).State = System.Data.Entity.EntityState.Added;
                    entity.SaveChanges();
                    return RedirectToAction("Index", "Urunler");
                }
            }
            return HttpNotFound();
        }


        public ActionResult ToplamUrunSayisi(int? toplam)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == User.Identity.Name);
                toplam = entity.Sepet.Where(I => I.KullaniciId == model.Id).Count();
                ViewBag.Count = toplam;
                if (toplam == 0)
                {
                    ViewBag.Count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }

        public ActionResult Arttir(int id)
        {
            var model = entity.Sepet.Find(id);
            model.Miktari++;
            model.ToplamFiyati = model.BirimFiyati * model.Miktari;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Azalt(int id)
        {
            var model = entity.Sepet.Find(id);
            if (model.Miktari == 1)
            {
                entity.Sepet.Remove(model);
                entity.SaveChanges();
            }
            model.Miktari--;
            model.ToplamFiyati = model.BirimFiyati * model.Miktari;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public void DinamikMiktar(int id, decimal miktari)
        {
            var model = entity.Sepet.Find(id);
            model.Miktari = miktari;
            model.ToplamFiyati = model.BirimFiyati * model.Miktari;
            entity.SaveChanges();
        }

        public ActionResult SepetSil(int id)
        {
            var model = entity.Sepet.Find(id);
            entity.Sepet.Remove(model);
            entity.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult SepetTamaminiSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciAdi = User.Identity.Name;
                var model = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi.Equals(kullaniciAdi));
                var silinecekler = entity.Sepet.Where(I => I.KullaniciId.Equals(model.Id));
                entity.Sepet.RemoveRange(silinecekler);
                entity.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    }
}