using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    /// <summary>
    /// model for the photo controller
    /// </summary>
    public class PhotoModel
    {
        /// <summary>
        /// method returns list of photo data (photos)
        /// </summary>
        /// <param name="pathToGallery">path to the gallery</param>
        /// <returns>list of photo data</returns>
        public List<PhotoData> GetPhotos(string pathToGallery)
        {
            List<PhotoData> list = new List<PhotoData>();
            try
            {
                string pathRoot = pathToGallery;
                string[] dirsYear = Directory.GetDirectories(pathRoot);

                foreach (string yearDir in dirsYear)
                {
                    int result;
                    string yearName = Path.GetFileName(yearDir);
                    if (int.TryParse(yearName, out result))
                    {
                        string pathYear = pathRoot + "/" + yearName;

                        string[] dirsMonth = Directory.GetDirectories(pathYear);

                        foreach (string monthDir in dirsMonth)
                        {
                            string monthName = Path.GetFileName(monthDir);
                            string pathMonth = pathYear + "/" + monthName;

                            string[] filePaths = Directory.GetFiles(pathMonth);

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

        /// <summary>
        /// method deletes received photo by the root gallery path received
        /// </summary>
        /// <param name="data">the photo to delete</param>
        /// <param name="pathRoot">path to gallery</param>
        /// <returns>true if completed, false otherwise</returns>
        public bool DeletePhoto(PhotoData data, string pathRoot)
        {
            string absolutePathRoot = pathRoot;
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