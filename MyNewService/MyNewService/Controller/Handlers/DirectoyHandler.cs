using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using System.Text.RegularExpressions;
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// class implementing the IDirectoryHandler interface
    /// </summary>
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IController m_controller;    // The Image Processing Controller
        private ILoggingService m_logging;   //logging service object
        private FileSystemWatcher m_dirWatcher;   // The Watcher of the Dir
        private string m_path;       // The Path of directory being watched
        private readonly string[] fileExtensionsToHandle = { ".jpg", ".png", ".gif", ".bmp", ".BMP", ".jpeg" , ".JPG", ".PNG", ".GIF"};
        #endregion

        /// <summary>
        /// DirectoyHandler class constructor
        /// </summary>
        /// <param name="controller">objec implementing the IController interface</param>
        /// <param name="logger">object implementing the ILoggingService interface</param>
        public DirectoyHandler(IController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            this.m_dirWatcher = null;
            this.m_path = null;
            m_logging.Log("In directory handler constructor finished", MessageTypeEnum.INFO);
        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    // The Event That Notifies that the Directory is being closed

        /// <summary>
        /// method handling commands being sent to the directory handler object
        /// </summary>
        /// <param name="sender">the sender of the command</param>
        /// <param name="e">arguments and info of the sent command</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            m_logging.Log("In directory handler received command with id: " + e.CommandID, MessageTypeEnum.INFO);

            //right now with switch case because its a general command and not only close
            switch (e.CommandID)
            {
                //close command
                case CommandEnum.CloseCommand:
                    this.CloseDirectory();
                    DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(this.m_path, "closing"));
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// method that receives path to a directory to be watched and initializes all the needed parts to do so
        /// </summary>
        /// <param name="dirPath">the path to the directory needed to be watched</param>
        public void StartHandleDirectory(string dirPath)
        {
            //checking if the requested directory to monitor exists
            if (!Directory.Exists(dirPath))
            {
                //if path to monitor doesn't exist write error message to log and return
                m_logging.Log("In directory handler, requested path to monitor doesn't exist.", MessageTypeEnum.FAIL);
                return;
            }
            this.m_logging.Log("In directory handler starting to handle the directory in path: " + dirPath, MessageTypeEnum.INFO);
            this.m_path = dirPath;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime, //notifies for creation of new file
                EnableRaisingEvents = true,
            };
            this.m_logging.Log("In directory handler created the files system watcher", MessageTypeEnum.INFO);

            this.m_dirWatcher.Created += new FileSystemEventHandler(delegate (object sender, FileSystemEventArgs e) 
            {
                System.Threading.Thread.Sleep(10);
                //checking (filtering) if the new fle in the dorectory is of the types the handler listens to
                if (fileExtensionsToHandle.Contains(Path.GetExtension(e.FullPath)))
                {
                    string pathToFile = m_path + @"\" + e.Name;
                    //waiting for the file to be able to work on it
                    while (FileLocked(pathToFile))
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                    string[] args = new string[4];
                    args[0] = this.m_path;
                    args[1] = e.Name;
                    DateTime date = GetDateTakenFromImage(e.FullPath);
                    args[2] = date.Year.ToString();
                    args[3] = date.Month.ToString();
                    bool result;
                    this.m_controller.ExecuteCommand(CommandEnum.NewFileCommand, args, out result);
                    m_logging.Log("In directory handler, finished working on the new file received", MessageTypeEnum.INFO);
                }
            });
        }

        /// <summary>
        /// checks if the file currently locked from access
        /// </summary>
        /// <param name="path">the path to the file needed to be checked for accessibility</param>
        /// <returns>true if the file unavailable for work and false otherwise</returns>
        private bool FileLocked(string path)
        {
            FileStream stream = null;
            try
            {
                //trying to open it (checking if the file is available)
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (Exception exc)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }
            return false;
        }

        /// <summary>
        /// method for closing the directory handler object
        /// </summary>
        private void CloseDirectory()
        {
            
            this.m_logging.Log("closing Directory Handler of directory in path: " + this.m_path, MessageTypeEnum.INFO);
            //release and dispose of the filesSystemWatcher object
            if (m_dirWatcher != null)
            {
                m_dirWatcher.EnableRaisingEvents = false;
                m_dirWatcher.Dispose();
            }
        }

        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage collector. used for the GetDateTakenFromImage method
        private static Regex r = new Regex(":");

        /// <summary>
        /// retrieves the datetime from the image at the received path
        /// </summary>
        /// <param name="path">the path to the image to have its takendate extracted</param>
        /// <returns>the datetime objects describing its datetaken property</returns>
        private static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
            {
                System.Drawing.Imaging.PropertyItem propItem = null;
                if (myImage.PropertyIdList.Any(x => x == 36867) || myImage.PropertyIdList.Any(x => x == 306))
                {
                    if (myImage.PropertyIdList.Any(x => x == 36867))
                    {
                        propItem = myImage.GetPropertyItem(36867);
                    }
                    else
                    {
                        propItem = myImage.GetPropertyItem(306);
                    }
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
                //if no property of date taken on the file (picture) take the date of creation
                return new FileInfo(path).CreationTime;
            }
        }

        /// <summary>
        /// private method for debugging purposes. Adjust the path for the txt file to be written to 
        /// </summary>
        /// <param name="msg">msg to be written</param>
        private static void Logm(string msg)
        {
            File.AppendAllText(@"D:\Users\user\Desktop\msglog.txt", msg + Environment.NewLine);
        }
    }
}
