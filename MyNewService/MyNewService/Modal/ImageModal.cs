using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Logging;
using Logging.Modal;
using System.Drawing;

namespace ImageService.Modal
{
    class ImageModal:IImageModal
    {
        private ILoggingService logger;
        private string outPutDir, outPutDirThumbnail;
        private int thumbnailSize;

        ImageModal(string path, int size)
        {
            outPutDir = path;
            outPutDirThumbnail = path + @"Thumbnail";
            thumbnailSize = size;
            if (!(CreateFolder(outPutDir + @"Thumbnail")))
            {
                this.logger.Log("unable to create folder", MessageTypeEnum.FAIL);
            }
        }

        public void SetUpLogger(ILoggingService log)
        {
            this.logger = log;
        }

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
                    result = CreateFolder(outPutDir + args[2].ToString());
                    result = CreateFolder(outPutDirThumbnail + args[2].ToString());

                }
                if (result && !Directory.Exists(yearPath + @"\" + args[3].ToString()))
                {
                    result = CreateFolder(yearPath + args[3].ToString());
                    result = CreateFolder(this.outPutDirThumbnail + @"\" + args[2].ToString() + args[3].ToString());

                }
                if (!result)
                {
                    this.logger.Log("unable to create folder", MessageTypeEnum.FAIL);
                    return ("Faild");
                }
                try
                {
                    File.Move(imagePath, yearPath + @"\" + args[3].ToString());
                    Image image = Image.FromFile(imagePath);
                    Image thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, ()=>false , IntPtr.Zero);
                    thumb.Save(Path.ChangeExtension(this.outPutDirThumbnail + @"\" + args[2].ToString() + args[3].ToString(), "thumb"));
                    this.logger.Log("file movement finished succefully", MessageTypeEnum.INFO); // write info about the file!!!!!!!!!!!!!!!!!!!!!!
                    return ("success");
                } catch (Exception e) { result = false;
                    this.logger.Log("unable to move file", MessageTypeEnum.FAIL); // write info about the file!!!!!!!!!!!!!!!!!!!!!!!!!
                    return ("failed");
                }
            }
            else
            {
                result = false;
                this.logger.Log("No such file", MessageTypeEnum.FAIL); // write more info about the file!!!!!!!!!!!!!!
                return ("failed");
            }
        }

        private bool CreateFolder(string path)
        {   try {
                Directory.CreateDirectory(path);
            } catch  (Exception e)  { return false; }
            return true;
        }

    }
}
