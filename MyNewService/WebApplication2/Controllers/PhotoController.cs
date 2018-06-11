using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PhotoController : Controller
    {
        private string rootPathGallery;
        private string rootPathThumbnails;

        static PhotoModel model = new PhotoModel();

        public PhotoController()
        {
            this.rootPathGallery = "/PhotosDirectories/Gallery";
            this.rootPathThumbnails = rootPathGallery + "/Thumbnail";
        }

        // GET: Photo
        public ActionResult Index()
        {
            List<PhotoData> photos = model.GetPhotos();
            ViewBag.RootPath = this.rootPathThumbnails;
            return View(photos);
        }

        public ActionResult PhotoView(string photoName, string year, string month)
        {
            ViewBag.RootPath = this.rootPathGallery;
            return View(new PhotoData() { Name = photoName, Year = year, Month = month});
        }

        public ActionResult DeletePhoto(string photoName, string year, string month)
        {
            if (photoName != "")
                model.DeletePhoto(new PhotoData() { Name = photoName, Year = year, Month = month }, this.rootPathGallery);
            ViewBag.RootPath = this.rootPathThumbnails;
            return RedirectToAction("Index");
            
        }

        public ActionResult PreviewBeforeDelete(string photoName, string year, string month)
        {
            ViewBag.RootPath = this.rootPathThumbnails;
            //user wants to delete the photo finally
            if (photoName != "")
            {
                return View(new PhotoData() { Name = photoName, Year = year, Month = month });
            }
            return RedirectToAction("Index");
        }
    }
}