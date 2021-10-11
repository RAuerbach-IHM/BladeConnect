using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using static SwitchBladeInterface.API.Enums.Enums;

namespace SwitchBladeInterface.API.NetworkServices
{
    public class NetworkClientService : IDisposable
    {
        public NetworkClientService()
        {
            //Messenger.Default.Register<NetworkClientSendMessage>(this, OnMessageReceived_SendMessage);

            parseDataReceived = new ParseDataReceivedService();
        }

        TcpClient Client_TCP;
        //UdpClient Client_UDP;

        NetworkStream Stream;
        //List<RemoteEvent> RemoteEvents;
        //NetworkClientReceiveMessage networkClientReceivedMessage;

        ParseDataReceivedService parseDataReceived;
        Device _wnipDevice;
        System.Timers.Timer KeepAliveTimer;
        System.Timers.Timer WheatNetSubscribeTimer;
        //System.Timers.Timer WheatNetUnsubscribeTimer;

        int WheatNetSubscribeTimerCount = 0;
        //int WheatNetUnsubscribeTimerCount = 0;

        //WheatNetSubscribeTimer wheatNetSubscribeTimer;

        //public void StartClient(WNIPDevice wnipDevice, List<RemoteEvent> remoteEvents)
        public void StartClient(Device wnipDevice)
        {
            //TODO - Stop Client if it is running


            switch (wnipDevice.Type)
            {
                case (int)DEVICE_TYPE.WHEATNET_BLADE:
                    Console.WriteLine("WheatNetTCPClient HIT");
                    _wnipDevice = wnipDevice;
                    //RemoteEvents = remoteEvents;
                    CreateClient_TCP(_wnipDevice.WNIP_Address.Trim(), _wnipDevice.Port);
                    StartKeepAliveTimer();
                    //ReceiveMessage();

                    break;
                /*
                case (int)DEVICE_TYPE.WHEATNET_SWITCHBLADE:
                    Console.WriteLine("WheatNetTCPClient_UDP HIT");
                    _wnipDevice = wnipDevice;
                    //RemoteEvents = remoteEvents;
                    //CreateClient_UDP(_wnipDevice.WNIP_Address.Trim(), _wnipDevice.Port);
                    break;
                */
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }

        public void StartClient(string ip, int port, int type)
        {
            //TODO - Stop Client if it is running


            switch (type)
            {
                case (int)DEVICE_TYPE.WHEATNET_BLADE:
                    Console.WriteLine("WheatNetTCPClient HIT");
                    //_wnipDevice = wnipDevice;
                    //RemoteEvents = remoteEvents;
                    CreateClient_TCP(ip, port);
                    StartKeepAliveTimer();
                    //ReceiveMessage();

                    break;
                    /*
                case (int)DEVICE_TYPE.WHEATNET_SWITCHBLADE:
                    Console.WriteLine("WheatNetTCPClient_UDP HIT");
                    //_wnipDevice = wnipDevice;
                    //RemoteEvents = remoteEvents;
                    CreateClient_UDP(ip, port);
                    break;
*/
            default:
                    Console.WriteLine("Default case");
                    break;
            }

        }

        public void CloseClient()
        {
            try
            {
                if (Stream != null)
                {
                    // Close everything.
                    Stream.Close();
                    Client_TCP.Close();

                    Stream = null;
                }
            }
            catch (Exception)
            {
                ;
            }
        }


        private void CreateClient_TCP(string server, int port)
        {
            try
            {
                Client_TCP = new TcpClient(server, port);
                int test = Client_TCP.ReceiveBufferSize;

                Stream = Client_TCP.GetStream();

                StartSubscribeTimer();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                Console.WriteLine("Connection Error (Null)" + e.ToString());
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Console.WriteLine("Connection Error (Socket) " + e.ToString());
            }


        }

        /*
        private void CreateClient_UDP(string server, int port)
        {
            try
            {
                Client_UDP = new UdpClient(server, port);


                Console.WriteLine("WheatNetTCPClient_UDP Started");

                //MessageBox.Show("Connected - UDP");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                Console.WriteLine("UDP Connection Error (Null)" + e.ToString());
            }
            catch (SocketException e)
            {
                Console.WriteLine("SOCKET ERROR - Server: " + server + " Port: " + port);
                Console.WriteLine("SocketException: {0}", e);
                Console.WriteLine("UDP Connection Error (Socket) " + e.ToString());
            }
        }
        */
        private void StartKeepAliveTimer()
        {
            KeepAliveTimer = new System.Timers.Timer(90000); //1:30 Minutes
            KeepAliveTimer.Elapsed += (sender, e) => Timer_Tick();
            KeepAliveTimer.Start();

            Console.WriteLine("WheatNetTCPClient Timer Started");
        }

        private void Timer_Tick()
        {
            //if (RemoteEvents.Count < 1)
            //    return;

            string message = "<SYS?VERSION>";

            //_message = String.Concat("<", RemoteEvents[0].EventReceived.Replace("EVENT", "SUB"), "|LVL:1>");

            SendMessage(message);

            Console.WriteLine("Keep Alive - " + message);
        }

        private void StartSubscribeTimer()
        {
            WheatNetSubscribeTimerCount = 0;

            WheatNetSubscribeTimer = new System.Timers.Timer(2000); //2 Seconds
            WheatNetSubscribeTimer.Elapsed += (sender, e) => SubscribeTimer_Tick();
            WheatNetSubscribeTimer.Start();
        }

        private void SubscribeTimer_Tick()
        {
            switch (WheatNetSubscribeTimerCount)
            {
                case 0:
                    ;//Do Nothing
                    break;
                case 1:
                    string message = "<DSTSUB:FFFFFFFF|NAME:1>";
                    SendMessage(message);
                    break;
                case 2:
                    message = "<SRCSUB:FFFFFFFF|NAME:1>";
                    SendMessage(message);
                    break;
                case 3:
                    message = "<DSTSUB:FFFFFFFF|LOCATION:1>";
                    SendMessage(message);
                    break;
                case 4:
                    message = "<SRCSUB:FFFFFFFF|LOCATION:1>";
                    SendMessage(message);
                    break;
                case 5:
                    message = "<DSTSUB:FFFFFFFF|SRC:1>";
                    SendMessage(message);
                    break;
                default:
                    WheatNetSubscribeTimer.Stop();
                    break;
            }
            WheatNetSubscribeTimerCount += 1;
        }

      

        public void SendMessage(string message)
        {
            //message = "<SLIOSUB:1|LVL:1>";
            try
            {
                if (Stream == null)
                    return;

                if (message == null)
                    return;

                if (message == "")
                    return;

                //MessageBox.Show("Try To Send");

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                Stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                //MessageBox.Show("Sent " + message);

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                Console.WriteLine("Error Null" + e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Console.WriteLine("Error - Socket Exception " + e);
            }
        }

        /*
        public void SendMessage_UDP(string message)
        {
            //MessageBox.Show("Message = " + message);

            //message = "<SLIOSUB:1|LVL:1>";
            try
            {

                if (message == null)
                    return;

                if (message == "")
                    return;

                if (Client_UDP == null)
                    return;

                //MessageBox.Show("Try To Send_UDP");

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                Client_UDP.Send(data, data.Length);

                Console.WriteLine("Sent: {0}", message);

                //MessageBox.Show("Sent UDP Client" + message);

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                Console.WriteLine("Error Null" + e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Console.WriteLine("Error - Socket Exception " + e);
            }
        }
        */

        public void ReceiveMessage()
        {
            var t = Task.Factory.StartNew(async () =>
            {

                //Byte[] data;

                try
                {
                    while (Stream != null)
                    {
                        // Receive the TcpServer.response.
                        // Buffer to store the response bytes.
                        Byte[] data = new Byte[5120];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.

                        int bytes = await Stream.ReadAsync(data, 0, data.Length);
                        //Int32 bytes = Stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                        //SendMessage to application
                        parseDataReceived.Parse(_wnipDevice.ID, responseData);

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

                    Console.WriteLine("TcpClient Closed");
                }
            });


        }
        public void Shutdown()
        {
            //Stop Timer
            KeepAliveTimer.Stop();

            //Unsubscribe to all SLIOs
            //UnsubscribeToEvents();

            //Shutdown client
            CloseClient();
        }

        /*
        private void OnMessageReceived_SendMessage(NetworkClientSendMessage obj)
        {
            if (obj.WNIPDevice.IP == _wnipDevice.IP && obj.WNIPDevice.Port == _wnipDevice.Port)
            {
                //SendMessage
                SendMessage(obj.Message);
            }
        }
        */

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (Client_TCP != null)
                        Client_TCP.Dispose();

                    /*
                    if (Client_UDP != null)
                        Client_UDP.Dispose();
                        */
                    if (KeepAliveTimer != null)
                        KeepAliveTimer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WheatNetTCPClient()
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