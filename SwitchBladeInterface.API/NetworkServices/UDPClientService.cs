using SwitchBladeInterface.API.Entities;
using System;
using System.Net.Sockets;

namespace SwitchBladeInterface.API.NetworkServices
{
    public class UDPClientService
    {
        UdpClient Client_UDP;
        
        public void Send(Device device, string message)
        {
            if(string.IsNullOrEmpty(message) || device == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(device.IP_Address)) {
                return;
            }
            CreateClient_UDP(device.IP_Address.Trim(), device.Port, message);
;        }
        private void CreateClient_UDP(string server, int port, string message)
        {
            try
            {
                Client_UDP = new UdpClient(server, port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                Client_UDP.Send(data, data.Length);

                Client_UDP.Close();
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
        
    }
}
