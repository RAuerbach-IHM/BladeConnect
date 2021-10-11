using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace NetworkClientLib
{
    public class UDPListenerClient
    {
        public UDPListenerClient()
        {
            parseUDPListener = new ParseUDPListener();
        }

        bool listen = false;
        readonly ParseUDPListener parseUDPListener;
        IPEndPoint listenEndpoint;

        long _deviceID;

        public void StartClient(long deviceID, string serverIPAddress)
        {
            if (serverIPAddress == "")
                return;

            _deviceID = deviceID;

            listen = true;
            Console.WriteLine("WheatNetUDPListenerClient_UDP HIT");

            listenEndpoint = new IPEndPoint(IPAddress.Parse(serverIPAddress), 52590);

            CreateListenerAsync();
        }

        public void CloseClient()
        {
            listen = false;
        }


        private void CreateListenerAsync()
        {
            listen = true;
            string message = "";
            try
            {
                Task.Run(async () =>
                {
                    using (var udpClient = new UdpClient(listenEndpoint))
                    {
                        while (listen)
                        {
                            var receivedResults = await udpClient.ReceiveAsync();

                            message = (Encoding.ASCII.GetString(receivedResults.Buffer));
                            parseUDPListener.Parse(_deviceID, message);
                        }
                    }
                });



            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                MessageBox.Show("UDP Listener Connection Error (Null)" + e.ToString());
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                MessageBox.Show("UDP Listener Connection Error (Socket) " + e.ToString());
            }
            catch(Exception e)
            {
                MessageBox.Show("UDP Listener Connection Error (General) " + e.ToString());
            }
        }

        public void Shutdown()
        {
            //Shutdown client
            CloseClient();
        }
    }
}

