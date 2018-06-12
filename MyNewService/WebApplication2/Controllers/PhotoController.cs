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
        private bool pathToGalleryReceived;

        static PhotoModel model = new PhotoModel();
        static ConfigModel configModel = new ConfigModel();

        public PhotoController()
        {
            ConfigData config = configModel.GetConfig();
            if (config.OutputDirectory != null && config.OutputDirectory != "")
            {
                this.pathToGalleryReceived = true;
                this.rootPathGallery = config.OutputDirectory;
                this.rootPathThumbnails = rootPathGallery + "/Thumbnail";
            }
            else
            {
                pathToGalleryReceived = false;
                this.rootPathGallery = "/PhotosDirectories/Gallery";
                this.rootPathThumbnails = rootPathGallery + "/Thumbnail";
            }
        }

        // GET: Photo
        public ActionResult Index()
        {
            List<PhotoData> photos = model.GetPhotos(this.rootPathGallery);
            ViewBag.RootPath = this.rootPathThumbnails;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            return View(photos);
        }

        public ActionResult PhotoView(string photoName, string year, string month)
        {
            ViewBag.RootPath = this.rootPathGallery;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            return View(new PhotoData() { Name = photoName, Year = year, Month = month});
        }

        public ActionResult DeletePhoto(string photoName, string year, string month)
        {
            if (photoName != "")
                model.DeletePhoto(new PhotoData() { Name = photoName, Year = year, Month = month }, this.rootPathGallery);
            ViewBag.RootPath = this.rootPathThumbnails;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            return RedirectToAction("Index");
            
        }

        public ActionResult PreviewBeforeDelete(string photoName, string year, string month)
        {
            ViewBag.RootPath = this.rootPathThumbnails;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            //user wants to delete the photo finally
            if (photoName != "")
            {
                return View(new PhotoData() { Name = photoName, Year = year, Month = month });
            }
            return RedirectToAction("Index");
        }
    }
}