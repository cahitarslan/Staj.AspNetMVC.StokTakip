//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Staj.AspNetMVC.StokTakip.Web.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Urunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urunler()
        {
            this.Sepet = new HashSet<Sepet>();
            this.Satislar = new HashSet<Satislar>();
            this.MarkaListesi = new List<SelectListItem>();
            MarkaListesi.Insert(0, new SelectListItem { Text = "Önce kategori seçilmelidir", Value = "" });
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı alanı boş geçilemez")]
        [Display(Name = "Kategori Adı")]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Marka adı alanı boş geçilemez")]
        [Display(Name = "Marka Adı")]
        public int MarkaId { get; set; }

        [Required(ErrorMessage = "Ürün adı alanı boş geçilemez")]
        [Display(Name = "Ürün Adı")]
        public string UrunAdi { get; set; }

        [Required(ErrorMessage = "Barkod no alanı boş geçilemez")]
        [Display(Name = "Barkod No")]
        public string BarkodNo { get; set; }

        [Required(ErrorMessage = "Alış fiyatı alanı boş geçilemez")]
        [Display(Name = "Alış Fiyatı")]
        public decimal AlisFiyati { get; set; }

        [Required(ErrorMessage = "Satış fiyatı alanı boş geçilemez")]
        [Display(Name = "Satış Fiyatı")]
        public decimal SatisFiyati { get; set; }

        [Required(ErrorMessage = "Miktar alanı boş geçilemez")]
        [Display(Name = "Miktar")]
        public decimal? Miktari { get; set; }

        [Required(ErrorMessage = "KDV alanı boş geçilemez")]
        [Range(0, 100, ErrorMessage = "0-100 arası sayı girilmelidir")]
        public int KDV { get; set; }

        [Required(ErrorMessage = "Birim adı alanı boş geçilemez")]
        [Display(Name = "Birim Adı")]
        public int BirimId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Tarih alanı boş geçilemez")]

        public System.DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Açıklama alanı boş geçilemez")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        public virtual Birimler Birimler { get; set; }
        public virtual Kategoriler Kategoriler { get; set; }
        public virtual Markalar Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sepet> Sepet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satislar> Satislar { get; set; }

        public List<SelectListItem> KategoriListesi { get; set; }
        public List<SelectListItem> MarkaListesi { get; set; }
        public List<SelectListItem> BirimListesi { get; set; }
    }
}
