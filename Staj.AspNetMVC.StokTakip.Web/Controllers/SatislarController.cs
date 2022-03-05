using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [Authorize]
    public class SatislarController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();
        public ActionResult Index()
        {
            var model = entity.Satislar.ToList();
            return View(model);
        }

        public ActionResult SatinAl(int id)
        {
            var model = entity.Sepet.FirstOrDefault(I => I.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SatinAlPost(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = entity.Sepet.FirstOrDefault(I => I.Id == id);
                    var urun = entity.Urunler.FirstOrDefault(I => I.Id == model.UrunId);
                    urun.Miktari -= model.Miktari;
                    var satis = new Satislar
                    {
                        KullaniciId = model.KullaniciId,
                        UrunId = model.UrunId,
                        SepetId = model.Id,
                        BarkodNo = model.Urunler.BarkodNo,
                        BirimFiyati = model.BirimFiyati,
                        Miktari = model.Miktari,
                        ToplamFiyati = model.ToplamFiyati,
                        KDV = model.Urunler.KDV,
                        BirimId = model.Urunler.BirimId,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    entity.Sepet.Remove(model);
                    entity.Satislar.Add(satis);
                    entity.SaveChanges();
                    ViewBag.Islem = "Satın alma işlemi başarılı...";
                }
            }
            catch (Exception)
            {
                ViewBag.Islem = "Satın alma esnasında bir hata meydana geldi !";
            }
            return View("Islem");
        }

        public ActionResult TamaminiSatinAl(decimal? tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciAdi = User.Identity.Name;
                var kullanici = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == kullaniciAdi);
                var model = entity.Sepet.Where(I => I.KullaniciId == kullanici.Id).ToList();
                var kullaniciSepet = entity.Sepet.FirstOrDefault(I => I.KullaniciId == kullanici.Id);
                if (model != null)
                {
                    if (kullaniciSepet == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmuyor";
                    }
                    else if (kullaniciSepet != null)
                    {
                        tutar = entity.Sepet.Where(I => I.KullaniciId == kullaniciSepet.KullaniciId).Sum(I => I.ToplamFiyati);
                        ViewBag.Tutar = "Toplam Tutar: " + tutar + " TL";
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult TamaminiSatinAlPost()
        {
            var userName = User.Identity.Name;
            var kullanici = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == userName);
            var model = entity.Sepet.Where(I => I.KullaniciId == kullanici.Id).ToList();
            int satir = 0;
            try
            {
                foreach (var item in model)
                {
                    var satis = new Satislar
                    {
                        KullaniciId = model[satir].KullaniciId,
                        UrunId = model[satir].UrunId,
                        SepetId = model[satir].Id,
                        BarkodNo = model[satir].Urunler.BarkodNo,
                        BirimFiyati = model[satir].BirimFiyati,
                        Miktari = model[satir].Miktari,
                        ToplamFiyati = model[satir].ToplamFiyati,
                        KDV = model[satir].Urunler.KDV,
                        BirimId = model[satir].Urunler.BirimId,
                        Tarih = DateTime.Now,
                        Saat = DateTime.Now
                    };
                    entity.Satislar.Add(satis);
                    satir++;
                }
                foreach (var item in model)
                {
                    var urun = entity.Urunler.FirstOrDefault(I => I.Id == item.UrunId);
                    if (urun != null)
                    {
                        urun.Miktari -= item.Miktari;
                    }
                }
                ViewBag.Islem = "Satın alma işlemi başarılı...";
                entity.Sepet.RemoveRange(model);
                entity.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Islem = "Satın alma esnasında bir hata meydana geldi !";
            }
            return View("Islem");
        }

    }
}