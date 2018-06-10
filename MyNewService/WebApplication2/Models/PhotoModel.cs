using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhotoModel
    {
        public List<PhotoData> GetPhotos()
        {
            List<PhotoData> list = new List<PhotoData>();
            try
            {
                string pathRoot = "/PhotosDirectories/Gallery";
                string[] dirsYear = Directory.GetDirectories(HttpContext.Current.Server.MapPath(pathRoot));

                foreach (string yearDir in dirsYear)
                {
                    if (int.TryParse(yearDir, out int result))
                    {
                        string yearName = Path.GetFileName(yearDir);
                        string pathYear = pathRoot + "/" + yearName;

                        string[] dirsMonth = Directory.GetDirectories(HttpContext.Current.Server.MapPath(pathYear));

                        foreach (string monthDir in dirsMonth)
                        {
                            string monthName = Path.GetFileName(monthDir);
                            string pathMonth = pathYear + "/" + monthName;

                            string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(pathMonth));

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

        public bool DeletePhoto(PhotoData data)
        {
            return true;
        }
    }
}