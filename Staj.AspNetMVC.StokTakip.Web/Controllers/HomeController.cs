using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanicilar k)
        {
            var kullanici = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == k.KullaniciAdi && I.Sifre == k.Sifre);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
                return RedirectToAction("Index", "Urunler");
            }
            ViewBag.Hata = "Kullanıcı adı veya şifre yanlış";
            return View(kullanici);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar kullanici)
        {
            var model = entity.Kullanicilar.Where(I => I.Email == kullanici.Email).FirstOrDefault();
            if (model != null)
            {
                Guid rastgele = Guid.NewGuid();
                model.Sifre = rastgele.ToString().Substring(0, 8);
                entity.SaveChanges();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("staj.stoktakip@gmail.com", "Şifre Sıfırlama")
                };
                mail.To.Add(model.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre Değiştirme İsteği";
                mail.Body += "Merhaba " + model.AdiSoyadi + " <br/><br/> Kullanıcı adınız: " + model.KullaniciAdi + "<br/> Yeni şifreniz: " + model.Sifre;
                NetworkCredential net = new NetworkCredential("staj.stoktakip@gmail.com", "Staj123stoktakip");
                client.UseDefaultCredentials = false;
                client.Credentials = net;
                client.EnableSsl = true;
                client.Send(mail);
                return RedirectToAction("Login");
            }
            ViewBag.Hata = "Böyle bir e-mail adresi bulunamadı";
            return View();
        }

        [HttpGet]
        public ActionResult Kaydol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Kaydol(Kullanicilar kullanici)
        {
            if (!ModelState.IsValid) return View();
            entity.Entry(kullanici).State = System.Data.Entity.EntityState.Added;
            entity.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult KullaniciBilgiGuncelle()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciAdi = User.Identity.Name;
                var model = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == kullaniciAdi);
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    return View(new Kullanicilar());
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult KullaniciBilgiGuncelle(Kullanicilar k)
        {
            entity.Entry(k).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}