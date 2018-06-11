using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ImageService.Communication
{
    public class SynchTcpClientHandler
    {
        #region Members
        private TcpClient m_client;         // The Client Instance
        private NetworkStream m_stream;     // The Stream To Close The Stream
        private BinaryReader m_reader;            // The Reader
        private BinaryWriter m_writer;            // The Writer
        #endregion

        public bool IsConnected { get; set; }

        public SynchTcpClientHandler()
        {
            try
            {
                string[] networkData = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/NetworkData.txt"));
                string ip = networkData[0];
                int port = int.Parse(networkData[1]);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                Logm("10");
                m_client = new TcpClient();
                m_client.Connect(ep);
                Logm("11");
                m_stream = m_client.GetStream();
                Logm("12");
                m_reader = new BinaryReader(m_stream);
                Logm("13");
                m_writer = new BinaryWriter(m_stream);
                Logm("14");
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
            }
        }
        // The Function Closes The Handler
        public void Close()
        {
            IsConnected = false;
            DisposeHandler();         // Closing the channel
        }

        public string Send(string data)
        {
            Logm("synch 1");
            try
            {
                Logm("synch 2");
                m_writer.Write(data);           // Writing the Data to the Client
                Logm("synch 3");
                m_writer.Flush();                   // Sending the Line
                Logm("synch 4");
                return m_reader.ReadString();      // Getting the string from the client;                 // return the Length of the Length
            }
            catch (Exception e)
            {
                Logm("synch bad");
                IsConnected = false;
                DisposeHandler();               // Closing the Handler
                return "";
            }
        }

        // Starting to Recieve Data
        public bool Start()
        {
            
            return true;
        }

        private void DisposeHandler()
        {
            if (IsConnected)
            {
                m_writer.Close();               // Closing the Writer
                m_reader.Close();               // Closing the Reader
                m_stream.Close();               // Closing the Straem
                m_client.Close();               // Closing the Client
            }
        }

        private static void Logm(string msg)
        {
            File.AppendAllText(@"D:\Users\seanz\Desktop\msglog.txt", msg + Environment.NewLine);
        }
    }
}
