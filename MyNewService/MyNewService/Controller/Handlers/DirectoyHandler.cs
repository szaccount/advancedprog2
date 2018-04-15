using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using Logging.Modal;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using System.Text.RegularExpressions;
using ImageService.Modal.Event;

namespace ImageService.Controller.Handlers
{
    //###############################################################################################################
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                     // The Path of directory
        private readonly string[] fileExtensionsToHandle = { ".jpg", ".png", ".gif", ".bmp", ".BMP", ".jpeg" }; //jpeg also? !!!!14.04-15.04!!!!?@?!@?!@?!@?!?@?!?@
        #endregion

        public DirectoyHandler(IController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            this.m_path = null;
            m_logging.Log("In directory handler constructor finished", MessageTypeEnum.INFO);
        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    // The Event That Notifies that the Directory is being closed

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            m_logging.Log("In directory handler received command with id: " + e.CommandID, MessageTypeEnum.INFO);

            //right now with switch case because its a general command and not only close as it should be!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                IncludeSubdirectories = true, // maybe delete this, becaus this can problems with the path used 14.04 !@!$!@!@#!@#@!#!@!!@!@@!!!!!!
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime, //notifies for creation of new file
                EnableRaisingEvents = true,
            };
            this.m_logging.Log("In directory handler created the files system watcher", MessageTypeEnum.INFO);
            this.m_logging.Log("got hereeeeeeee", MessageTypeEnum.WARNING);

            this.m_dirWatcher.Created += new FileSystemEventHandler(delegate (object sender, FileSystemEventArgs e) 
            {
                System.Threading.Thread.Sleep(10);
                //checking (filtering) if the new fle in the dorectory is of the types the handler listens to
                if (fileExtensionsToHandle.Contains(Path.GetExtension(e.FullPath)))
                {
                    this.m_logging.Log("12121212new file in directory of path: " + this.m_path, MessageTypeEnum.INFO);
                    string pathToFile = m_path + @"\" + e.Name;
                    this.m_logging.Log("here11111111111", MessageTypeEnum.INFO);
                    while (FileLocked(pathToFile))
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                    this.m_logging.Log("here2222222222222", MessageTypeEnum.INFO);
                    string[] args = new string[4];
                    this.m_logging.Log("here3333333333333", MessageTypeEnum.INFO);
                    args[0] = this.m_path;
                    this.m_logging.Log("here444444444444", MessageTypeEnum.INFO);
                    args[1] = e.Name;
                    this.m_logging.Log("here5555555555555", MessageTypeEnum.INFO);
                    DateTime date = GetDateTakenFromImage(e.FullPath);
                    this.m_logging.Log("here77777777777", MessageTypeEnum.INFO);
                    args[2] = date.Year.ToString();
                    this.m_logging.Log("here888888888888", MessageTypeEnum.INFO);
                    args[3] = date.Month.ToString();
                    this.m_logging.Log("here999999999999", MessageTypeEnum.INFO);
                    bool result;
                    this.m_controller.ExecuteCommand(CommandEnum.NewFileCommand, args, out result); // !!!!! maybe declare the bool variable outside if I want to do something with it !!!!!!!!!!!!!! 
                    m_logging.Log("In directory handler, finished working on the new file received", MessageTypeEnum.INFO);
                }
            });
        }

        //checks if the file currently locked from access
        private bool FileLocked(string path)
        {
            FileStream stream = null;
            try
            {
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

        private void CloseDirectory() //13.4.18 !!!!!!!!!!!!!!!!!!!!!!!!! should also stop the files system watcher, no? !!!!!!!!!!!!!!!!!!!!
        {
            
            this.m_logging.Log("closing Directory Handler of directory in path: " + this.m_path, MessageTypeEnum.INFO);
            //two lines added at 14.04-15.04 !@!$!#@!!!!!!!!!!!!@#!$#@$#$@%#!@#!@$#%!%@$!!!!!!!!!!!!!!!!!!!!!!!!!!!
            m_dirWatcher.EnableRaisingEvents = false;
            m_dirWatcher.Dispose();
        }

        //!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        private static DateTime GetDateTakenFromImage(string path)
        {
            Logm("here5151515151515151");
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
            {
                Logm("here52525252525252");
                System.Drawing.Imaging.PropertyItem propItem = null;
                if (myImage.PropertyIdList.Any(x => x == 36867) || myImage.PropertyIdList.Any(x => x == 306))
                {
                    Logm("here5353535353535353");
                    if (myImage.PropertyIdList.Any(x => x == 36867))
                    {
                        Logm("here545454545454545454");
                        propItem = myImage.GetPropertyItem(36867);
                    }
                    else
                    {
                        Logm("here56565656565656565656");
                        propItem = myImage.GetPropertyItem(306);
                    }
                    Logm("here57575757575757575757");
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
                Logm("here585858585858585858");
                //if no property of date taken on the file (picture) take the date of last write
                return new FileInfo(path).CreationTime;
                //return new FileInfo(path).LastWriteTime;
            }
        }
        //!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#

        //$#@$#@$!@#!@!@#!@$@!$!@$! delete!!!!!!!!!!!!!!!!! #$@$!$!@#!@#!@#@!#@!$!@$@!#!#!@#!@#
        private static void Logm(string msg)
        {
            File.AppendAllText(@"D:\Users\seanz\Desktop\log.txt", msg + Environment.NewLine);
        }
        //$#@$#@$!@#!@!@#!@$@!$!@$! delete!!!!!!!!!!!!!!!!! #$@$!$!@#!@#!@#@!#@!$!@$@!#!#!@#!@#
    }
}
