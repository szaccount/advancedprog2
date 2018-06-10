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
        static PhotoModel model = new PhotoModel();

        // GET: Photo
        public ActionResult Index()
        {
            List<PhotoData> photos = model.GetPhotos();
            ViewBag.RootPath = "/PhotosDirectories/Gallery";
            return View(photos);
        }
    }
}