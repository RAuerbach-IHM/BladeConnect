using GalaSoft.MvvmLight.Messaging;
using Models;
using NetworkClientLib.Messages;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkClientLib
{
    public class WheatNetUDPClient : IDisposable
    {
        public WheatNetUDPClient()
        {
            Messenger.Default.Register<NetworkClientSendMessage>(this, OnMessageReceived_SendMessage);
        }


        UdpClient Client;

        NetworkStream Stream;

        WNIPDevice _wnipDevice;

        public void StartClient(WNIPDevice wnipDevice)
        {
            Console.WriteLine("WheatNetTCPClient_UDP HIT");
            _wnipDevice = wnipDevice;
            CreateClient(_wnipDevice.IP, _wnipDevice.Port);
        }

        public void CloseClient()
        {
            try
            {
                if (Stream != null)
                {
                    // Close everything.
                    Stream.Close();
                    Client.Close();

                    //Stream.Dispose();
                    //Client.Dispose();

                    Stream = null;
                }
            }
            catch (Exception)
            {
                ;
            }
        }



        private void CreateClient(string server, int port)
        {
            try
            {
                Client = new UdpClient(server, port);


                Console.WriteLine("WheatNetTCPClient_UDP Started");

                //MessageBox.Show("Connected - UDP");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                MessageBox.Show("UDP Connection Error (Null)" + e.ToString());
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                MessageBox.Show("UDP Connection Error (Socket) " + e.ToString());
            }
        }


        public void SendMessage(string message)
        {
            //MessageBox.Show("Message = " + message);

            //message = "<SLIOSUB:1|LVL:1>";
            try
            {

                if (message == null)
                    return;

                if (message == "")
                    return;

                //MessageBox.Show("Try To Send_UDP");

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                Client.Send(data, data.Length);

                Console.WriteLine("Sent: {0}", message);

                //MessageBox.Show("Sent UDP Client" + message);

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                MessageBox.Show("Error Null" + e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                MessageBox.Show("Error - Socket Exception " + e);
            }
        }

        public void ReceiveMessage()
        {
            var t = Task.Factory.StartNew(async () =>
            {

                //Byte[] data;

                try
                {
                    while (Stream != null)
                    {

                        //if (!Stream.DataAvailable)
                        //{
                        //    // Give up the remaining time slice.
                        //    Thread.Sleep(1);
                        //    continue;
                        //}



                        // Receive the TcpServer.response.
                        // Buffer to store the response bytes.
                        Byte[] data = new Byte[256];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.

                        int bytes = await Stream.ReadAsync(data, 0, data.Length);
                        //Int32 bytes = Stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                        //SendMessage to application
                        //networkClientReceivedMessage = new NetworkClientReceiveMessage(responseData, _wnipDevice);
                        //Messenger.Default.Send(networkClientReceivedMessage);

                        //MessageBox.Show("Received: " + _wnipDevice.IP + " - " + responseData);
                        Console.WriteLine("Received: {0}", responseData);
                    }
                }
                catch (Exception ex)
                {
                    Stream.Close();
                    Console.WriteLine("STREAM CLOSED" + ex.Message);
                }
            });


        }
        public void Shutdown()
        {
            //Shutdown client
            CloseClient();
        }

        private void OnMessageReceived_SendMessage(NetworkClientSendMessage obj)
        {
            if (obj.WNIPDevice.IP == _wnipDevice.IP && obj.WNIPDevice.Port == _wnipDevice.Port)
            {
                //SendMessage
                SendMessage(obj.Message);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WheatNetUDPClient()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

