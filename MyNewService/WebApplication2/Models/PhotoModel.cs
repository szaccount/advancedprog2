using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhotoModel
    {
        public List<PhotoData> GetPhotos(string pathToGallery)
        {
            List<PhotoData> list = new List<PhotoData>();
            try
            {
                string pathRoot = pathToGallery;
                string[] dirsYear = Directory.GetDirectories(/*HttpContext.Current.Server.MapPath(*/pathRoot/*)*/);

                foreach (string yearDir in dirsYear)
                {
                    int result;
                    string yearName = Path.GetFileName(yearDir);
                    if (int.TryParse(yearName, out result))
                    {
                        string pathYear = pathRoot + "/" + yearName;

                        string[] dirsMonth = Directory.GetDirectories(/*HttpContext.Current.Server.MapPath(*/pathYear/*)*/);

                        foreach (string monthDir in dirsMonth)
                        {
                            string monthName = Path.GetFileName(monthDir);
                            string pathMonth = pathYear + "/" + monthName;

                            string[] filePaths = Directory.GetFiles(/*HttpContext.Current.Server.MapPath(*/pathMonth/*)*/);

                            foreach (string filePath in filePaths)
                            {
                                string fileName = Path.GetFileName(filePath);
                                list.Add(new PhotoData { Name = fileName, Month = monthName, Year = yearName });
                            }
                        }
                    }
                }
                return list;
            }
            catch
            {
                return new List<PhotoData>();
            }
            
        }

        public bool DeletePhoto(PhotoData data, string pathRoot)
        {
            string absolutePathRoot = /*HttpContext.Current.Server.MapPath(*/pathRoot/*)*/;
            string thumbPath = absolutePathRoot + "/Thumbnail";
            absolutePathRoot = absolutePathRoot + "/" + data.Year;
            thumbPath = thumbPath + "/" + data.Year;
            absolutePathRoot = absolutePathRoot + "/" + data.Month;
            thumbPath = thumbPath + "/" + data.Month;
            absolutePathRoot = absolutePathRoot + "/" + data.Name;
            thumbPath = thumbPath + "/" + data.Name;
            if (File.Exists(absolutePathRoot))
            {
                try
                {
                    File.Delete(absolutePathRoot);
                    File.Delete(thumbPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}