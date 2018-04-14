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
        private string m_path;                              // The Path of directory
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
            this.m_dirWatcher./*Changed*/Created += new FileSystemEventHandler(delegate (object sender, FileSystemEventArgs e) 
            {
                this.m_logging.Log("111111new file in directory of path: " + this.m_path, MessageTypeEnum.INFO);
                System.Threading.Thread.Sleep(10);
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
        }

        //!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        private static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
            {
                System.Drawing.Imaging.PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
        //!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#!#

    }
}
