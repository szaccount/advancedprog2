using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Drawing;

namespace ImageService.Modal
{
    /// <summary>
    /// class implementing the IImageModal interface
    /// </summary>
    class ImageModal :IImageModal
    {
        #region Members
        private ILoggingService logger;
        private string outPutDir, outPutDirThumbnail;
        private int thumbnailSize;
        private bool outPutDirExist;
        private bool outPutDirThumbnailExist;
        #endregion

        /// <summary>
        /// ImageModal class constructor
        /// </summary>
        /// <param name="log">object implementing the ILoggingService interface (logger)</param>
        /// <param name="path">path to the output directory</param>
        /// <param name="size">size of the thumbnail files to be created</param>
        public ImageModal(ILoggingService log, string path, int size)
        {
            outPutDirExist = true;
            outPutDirThumbnailExist = true;
            logger = log;
            outPutDir = path;
            outPutDirThumbnail = outPutDir + @"\Thumbnail";
            thumbnailSize = size;

            try
            {
                DirectoryInfo dI = Directory.CreateDirectory(outPutDir);
                dI.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            catch (Exception exc)
            {
                this.logger.Log("In ImageModal, unable to create\\locate output folder or make it hidden", MessageTypeEnum.FAIL);
                outPutDirExist = false;
            }


            if (!outPutDirExist || !(CreateFolder(outPutDirThumbnail)))
            {
                this.logger.Log("In ImageModal, unable to create thumbnail folder", MessageTypeEnum.FAIL);
                outPutDirThumbnailExist = false;
            }
            logger.Log("In ImageModal, finished ImageModal constructor", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// setter for the logger object, to be used by this class object
        /// </summary>
        /// <param name="log">object implementing the ILoggingService interface (logger)</param>
        public void SetUpLogger(ILoggingService log)
        {
            this.logger = log;
        }

        /// <summary>
        /// The Function Addes A file to the system (moves from the source to the needed destination),
        /// based on the received args
        /// args[0] = DirectoryPath to the image
        /// args[1] = picture name
        /// args[2] = year number
        /// args[3] = month number 
        /// </summary>
        /// <param name="args">The argumens (info about the file) for the transfer</param>
        /// <param name="result">the result of the function (true for success, false for error)</param>
        /// <returns>Indication if the Addition Was Successful ("success\failed")</returns>
        public string AddFile(string[] args, out bool result)
        {
            
            logger.Log("In imageModal, received request to add new file: " + args[1] + " from path: " + args[0] + " taken at " + args[3] + "." + args[2], MessageTypeEnum.INFO);
            if (!outPutDirExist)
            {
                logger.Log("In imageModal, unable to procees received file to handle beacuse output dir doesn't exist", MessageTypeEnum.FAIL);
                result = false;
                return "failed";
            }
            result = true;
            bool tmp = true;
            string imagePath = args[0] + @"\" + args[1].ToString();
            string yearPath = this.outPutDir + @"\" + args[2].ToString();
            string monthPath = yearPath + @"\" + args[3].ToString();
            string newFilePath = monthPath + @"\" + args[1].ToString();
            string yearPathThumbnail = this.outPutDirThumbnail + @"\" + args[2].ToString();
            string monthPathThumbnail = yearPathThumbnail + @"\" + args[3].ToString();
            string newFilePathThumbnail = monthPathThumbnail + @"\" + args[1].ToString();
            string newRenamedFilePath = null;
            string newRenamedFileThumbnailPath = null;
            if (File.Exists(imagePath)) {
                if (!Directory.Exists(yearPath))
                {
                    result = CreateFolder(yearPath);
                    tmp = CreateFolder(yearPathThumbnail);
                    if (!tmp && result)
                        result = false;

                }
                if (result && !Directory.Exists(monthPath))
                {
                    result = CreateFolder(monthPath);
                    tmp = CreateFolder(monthPathThumbnail);
                    if (!tmp && result)
                        result = false;

                }
                if (!result)
                {
                    this.logger.Log("In ImageModal, unable to create needed folder, cannot finish action", MessageTypeEnum.FAIL);
                    return "Failed";
                }
                try
                {
                    //try to move the file, if a file of the same name already exists at the detination try saving with different name
                    if (!File.Exists(newFilePath))
                        File.Move(imagePath, newFilePath);
                    else
                    {
                        int index = 1;
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
                        string fileExtension = Path.GetExtension(imagePath);
                        newRenamedFilePath = monthPath + @"\" + fileNameWithoutExtension + "Renamed" + index.ToString() + fileExtension;
                        while (File.Exists(newRenamedFilePath))
                        {
                            index++;
                            newRenamedFilePath = monthPath + @"\" + fileNameWithoutExtension + "Renamed" + index.ToString() + fileExtension;
                        }
                        File.Move(imagePath, newRenamedFilePath);
                        newRenamedFileThumbnailPath = monthPathThumbnail + @"\" + fileNameWithoutExtension + "Renamed" + index.ToString() + fileExtension;

                    }
                    this.logger.Log("In ImageModal, file movement finished succefully", MessageTypeEnum.INFO);
                }
                catch (Exception e)
                {
                    result = false;
                    this.logger.Log("In ImageModal, unable to move file, reason: " + e.Message + "   " + e.StackTrace + "   " + e.ToString(), MessageTypeEnum.FAIL);
                    return "failed";
                }
                Image image = null; Image thumb = null;
                try
                {
                    if (!outPutDirThumbnailExist)
                    {
                        logger.Log("In imageModal, unable to create thumbnail file beacuse thumbnail directory doesn't exist", MessageTypeEnum.FAIL);
                        result = false;
                        return "failed";
                    }
                    image = Image.FromFile(newRenamedFilePath ?? newFilePath);
                    thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);
                    thumb.Save(Path.ChangeExtension(newRenamedFileThumbnailPath ?? newFilePathThumbnail, "thumb")); // !!!#@!#!@#@!# maybe leave the same extension? 15.4 #!@#@!$@#!@@!@!#!#!!
                    this.logger.Log("In ImageModal, thumbnail creation finished succefully", MessageTypeEnum.INFO);
                    return "success";
                }
                catch (FileNotFoundException fnfe) {
                    logger.Log("In ImageModal failed creating the thumbnail, file not fount exception has been thrown", MessageTypeEnum.FAIL);
                    return "failed";
                }
                catch (Exception e)
                {
                    result = false;
                    this.logger.Log("In ImageModal, unable create thumbnail, reason: " + e.Message + "  " + e.StackTrace + "  " + e.Source, MessageTypeEnum.FAIL);
                    return "failed";
                }
                finally
                {
                    image?.Dispose();
                    thumb?.Dispose();
                }
            }
            else
            {
                result = false;
                this.logger.Log("In ImageModal, problem with add file request, there is No such file", MessageTypeEnum.FAIL);
                return ("failed");
            }
        }

        /// <summary>
        /// This method tries to create a directory at the specified path.
        /// </summary>
        /// <param name="log">object implementing the ILoggingService interface (logger)</param>
        /// <param name="path">path to the directory needed to be created</param>
        /// <return>true if creation successful or the directory already exists at that path, and false otherwise</return>
        private bool CreateFolder(string path)
        {   try {
                Directory.CreateDirectory(path);
            } catch  (Exception e)  { return false; }
            return true;
        }

    }
}
