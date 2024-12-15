using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOdev.Models;

namespace WebOdev.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        KitaplikEntities db = new KitaplikEntities();
        public ActionResult books()
        {
            var kitaplar = db.Kitap.ToList();
            return View(kitaplar);
        }

        public ActionResult addBook()
        {
            return View("addBook");
        }

        [HttpPost]
        public ActionResult addBook(Kitap newBook)
        {
            bool isBookExist = db.Kitap.Any(k => k.KitapAdi == newBook.KitapAdi);

            if (isBookExist)
            {
                ViewBag.Message = "Bu kitap zaten mevcut!";
                return View();
            }
            else
            {
                var maxId = db.Kitap.Max(k => k.Id);
                newBook.Id = maxId + 1;
                db.Kitap.Add(newBook);
                db.SaveChanges();
                return RedirectToAction("books");
            }
        }

        public ActionResult removeBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult removeBook(Kitap model)
        {
            var newBook = model.KitapAdi;
            int sayi = Int32.Parse(model.Stok);

            bool isBookExist = db.Kitap.Any(k => k.KitapAdi == newBook);
            if (isBookExist)
            {
                var kitap = db.Kitap.FirstOrDefault(k => k.KitapAdi == newBook);

                if (kitap != null && Int32.Parse(kitap.Stok) >= sayi)
                {
                    var stokAdedi = Int32.Parse(kitap.Stok);
                    stokAdedi -= sayi;
                    kitap.Stok = stokAdedi.ToString();

                    if (kitap.Stok == "0")
                    {
                        db.Kitap.Remove(kitap);
                    }
                    db.SaveChanges();
                }
                else if (Int32.Parse(kitap.Stok) < sayi)
                {
                    ViewBag.Message = "Kitap stok adedi aşıldı!!!";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Böyle bir kitap bulunamadı!";
                return View();
            }
            return RedirectToAction("books");
        }
    }
}