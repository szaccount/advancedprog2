using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageService
{
    class ImageModal:IImageModal
    {
        //private Ilogger logger;
        private string outPutDir;
        private int thumbnailSize;

        ImageModal(string path, int size)
        {
            outPutDir = path;
            thumbnailSize = size;
        }

        /*public setUpLogger(ILogger log)
        {
            logger = log;
        }*/
        public string AddFile(string[] args, out bool result)
        {
            // args[0] = DirectoryPath to the image
            // args[1] = picture name
            // args[2] = year number
            // args[3] = month number 
            result = true;
            string imagePath = args[0] + @"\" + args[1].ToString();
            string yearPath = this.outPutDir + @"\" + args[2].ToString();
            if (File.Exists(imagePath)) {
                if (!Directory.Exists(yearPath))
                {
                    result = createFolder(outPutDir + args[2].ToString());
                    
                    if (result && !Directory.Exists(yearPath + @"\" + args[3].ToString()))
                    {
                        result = createFolder(yearPath + args[3].ToString());
                    }
                }
                if (!result)
                    // logger.newMsg("unable to create folder")
                    // return 
                try
                {
                    File.Move(imagePath, yearPath + @"\" + args[3].ToString());
                } catch (Exception e) { result = false;
                        // logger.newMsg("unable to move file")
                    }
            }
            else
            {
                result = false;
                // logger.newMsg("No such file")

            }
            //return
        }

        public bool createFolder(string path)
        {   try {
                Directory.CreateDirectory(path);
            } catch  (Exception e)  { return false; }
            return true;
        }        
    }
}
