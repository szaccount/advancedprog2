using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    ///  The controller for the photos based web pages
    /// </summary>
    public class PhotoController : Controller
    {
        //the root path to the gallery
        private string rootPathGallery;
        //the root path to the thumbnails
        private string rootPathThumbnails;
        //flag that holds true if received root path is valid and false otherwise
        private bool pathToGalleryReceived;

        static PhotoModel model = new PhotoModel();
        static ConfigModel configModel = new ConfigModel();

        /// <summary>
        /// constructor of the controller, initializes the root path strings
        /// </summary>
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
        /// <summary>
        /// the main page view method
        /// </summary>
        /// <returns>view of the main page</returns>
        public ActionResult Index()
        {
            List<PhotoData> photos = model.GetPhotos(this.rootPathGallery);
            ViewBag.RootPath = this.rootPathThumbnails;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            return View(photos);
        }

        /// <summary>
        /// method for displaying view of specified photo
        /// </summary>
        /// <param name="photoName">the name of the photo to display</param>
        /// <param name="year">photo's year</param>
        /// <param name="month">photo's month</param>
        /// <returns>view of the specified photo</returns>
        public ActionResult PhotoView(string photoName, string year, string month)
        {
            ViewBag.RootPath = this.rootPathGallery;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            return View(new PhotoData() { Name = photoName, Year = year, Month = month});
        }

        /// <summary>
        /// method to delete photo and redirect to main page
        /// </summary>
        /// <param name="photoName">photo name</param>
        /// <param name="year">photo year</param>
        /// <param name="month">photo month</param>
        /// <returns>view of the main page</returns>
        public ActionResult DeletePhoto(string photoName, string year, string month)
        {
            if (photoName != "")
                model.DeletePhoto(new PhotoData() { Name = photoName, Year = year, Month = month }, this.rootPathGallery);
            ViewBag.RootPath = this.rootPathThumbnails;
            ViewBag.RootPathValid = this.pathToGalleryReceived;
            return RedirectToAction("Index");
            
        }

        /// <summary>
        /// method to display delete view of specified photo
        /// </summary>
        /// <param name="photoName">photo name</param>
        /// <param name="year">photo's year</param>
        /// <param name="month">photo's month</param>
        /// <returns>view of the photo delete page</returns>
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