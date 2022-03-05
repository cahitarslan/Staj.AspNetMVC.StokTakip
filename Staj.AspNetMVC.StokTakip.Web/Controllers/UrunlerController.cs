using Staj.AspNetMVC.StokTakip.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace Staj.AspNetMVC.StokTakip.Web.Controllers
{
    [Authorize]
    public class UrunlerController : Controller
    {
        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();
        public ActionResult Index(string ara)
        {
            var model = entity.Urunler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(I => I.UrunAdi.ToLower().Contains(ara.ToLower()) || I.BarkodNo.ToLower().Contains(ara.ToLower())).ToList();
            }
            return View(model);
        }

        private void Yenile(Urunler model)
        {
            List<Kategoriler> kategoriList = entity.Kategoriler.OrderBy(I => I.KategoriAdi).ToList();
            model.KategoriListesi = (from x in kategoriList
                                     select new SelectListItem
                                     {
                                         Text = x.KategoriAdi,
                                         Value = x.Id.ToString()

                                     }).ToList();

            List<Birimler> birimList = entity.Birimler.OrderBy(I => I.BirimAdi).ToList();
            model.BirimListesi = (from x in birimList
                                  select new SelectListItem
                                  {
                                      Text = x.BirimAdi,
                                      Value = x.Id.ToString()
                                  }).ToList();
        }

        [HttpGet]
        public ActionResult UrunEkle()
        {
            var model = new Urunler();
            Yenile(model);
            return View(model);
        }

      

        [HttpPost]
        public ActionResult UrunEkle(Urunler urun)
        {
            if (!ModelState.IsValid)
            {
                var model = new Urunler();
                Yenile(model);
                return View(model);
            }
            entity.Entry(urun).State = System.Data.Entity.EntityState.Added;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult MarkaGetir(int id)
        {
            var model = new Urunler();
            List<Markalar> markaListe = entity.Markalar.Where(I => I.KategoriId == id).OrderBy(I => I.MarkaAdi).ToList();
            model.MarkaListesi = (from x in markaListe
                                  select new SelectListItem
                                  {
                                      Text = x.MarkaAdi,
                                      Value = x.Id.ToString()
                                  }).ToList();
            model.MarkaListesi.Insert(0, new SelectListItem { Text = "Seçiniz", Value = "" });
            return Json(model.MarkaListesi, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MiktarEkle(int id)
        {
            var model = entity.Urunler.Find(id);
            model.Miktari = null;
            return View(model);
        }

        [HttpPost]
        public ActionResult MiktarEkle(Urunler urun)
        {
            var model = entity.Urunler.Find(urun.Id);
            model.Miktari += urun.Miktari;
            if (model.Miktari == null)
            {
                ModelState.AddModelError("Miktari","Lütfen eklemek istediğiniz miktarı kutucuğa giriniz");
                return View(model);
            }
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UrunGuncellemeGetir(int id)
        {
            var model = entity.Urunler.Find(id);
            Yenile(model);
            List<Markalar> markaList = entity.Markalar.Where(I => I.KategoriId == model.KategoriId).OrderBy(I => I.MarkaAdi).ToList();
            model.MarkaListesi = (from x in markaList
                                 select new SelectListItem
                                 {
                                     Text = x.MarkaAdi,
                                     Value = x.Id.ToString()
                                 }).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult UrunGuncelle(Urunler u)
        {
            if (!ModelState.IsValid)
            {
                var model = entity.Urunler.Find(u.Id);
                Yenile(model);
                List<Markalar> markaList = entity.Markalar.Where(I => I.KategoriId == model.KategoriId).OrderBy(I => I.MarkaAdi).ToList();
                model.MarkaListesi = (from x in markaList
                                     select new SelectListItem
                                     {
                                         Text = x.MarkaAdi,
                                         Value = x.Id.ToString()
                                     }).ToList();
                return View(model);
            }

            entity.Entry(u).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UrunSilmeGetir(int id)
        {
            var model = entity.Urunler.Find(id);
            if (model == null) return HttpNotFound();
            return View(model);

        }
        public ActionResult UrunSil(Urunler urun)
        {
            entity.Entry(urun).State = System.Data.Entity.EntityState.Deleted;
            entity.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ExcelDisaAktar()
        {
            try
            {
                var liste = entity.Urunler.ToList();
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "Id";
                worksheet.Cells[1, 2] = "Ürün Adı";
                worksheet.Cells[1, 3] = "Barkod No";
                worksheet.Cells[1, 4] = "Fiyatı";
                worksheet.Cells[1, 5] = "Miktarı";
                worksheet.Cells[1, 6] = "Tarih";

                int row = 2;
                foreach (var item in liste)
                {
                    worksheet.Cells[row, 1] = item.Id;
                    worksheet.Cells[row, 2] = item.UrunAdi;
                    worksheet.Cells[row, 3] = item.BarkodNo;
                    worksheet.Cells[row, 4] = item.SatisFiyati;
                    worksheet.Cells[row, 5] = item.Miktari;
                    worksheet.Cells[row, 6] = item.Tarih;

                    worksheet.Cells[row, 2].ColumnWidth = 15;
                    worksheet.Cells[row, 4].ColumnWidth = 15;
                    worksheet.Cells[row, 6].ColumnWidth = 15;
                    row++;
                }
                var heading = worksheet.get_Range("A1", "F1");
                heading.Font.Bold = true;
                heading.Font.Size = 13;
                heading.Font.Color = Color.Blue;

                var range_Fiyat = worksheet.get_Range("D2", "D" + row);
                range_Fiyat.NumberFormat = "#,###,###.00";

                var range_Tarih = worksheet.get_Range("F2", "F" + row);
                range_Tarih.NumberFormat = "dd.MM.yyyy";

                workbook.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Ürün Listesi - " + Guid.NewGuid() + ".xlsx");
                workbook.Close();
                application.Quit();


                ViewBag.Mesaj = "İşlem başarılı. Dosya masaüstüne konumlandırıldı.";
            }
            catch (Exception)
            {
                ViewBag.Mesaj = "İşlem başarısız. Bir hata meydana geldi.";
            }
            return Json(ViewBag.Mesaj, JsonRequestBehavior.AllowGet);
        }

    }
}