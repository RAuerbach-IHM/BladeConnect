using GalaSoft.MvvmLight;
using System;
using System.IO;

namespace EW
{
    public class AppSettings : ObservableObject
    {
        public AppSettings()
        {
            ServerVersion = "1 SQLite 64";
            ServerBuild = "200110";

            //Connection String
            string dbLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "iHeartMedia", "SBInterface", "Database");
            connectionStringSQLite = string.Concat("Data Source=", dbLocation ,"\\ihmSwitchBladeInterface.db; Version=3; New=True; Compress=True; ");
    }


        private string serverIPAddress = "";
        private string serverComputerName = "";
        private string serverName = "iHeartMedia SwitchBlade Interface";
        private string serverMarket = "";
        private int port = 4224;
        private int vbPort = 6789;
        private string connectionString = @"Data Source=localhost;Initial Catalog=IHMSwitchBladeInterfaceDB;Integrated Security=SSPI";

        private string connectionStringSQLite = "Data Source=ihmSwitchBladeInterface.db; Version=3; New=True; Compress=True; ";
        //@"Data Source=DT00008452;Initial Catalog=IHMSwitchBladeInterfaceDB;Integrated Security=True"

        public string ServerVersion { get; set; }
        public string ServerBuild { get; set; }

        public string ConnectionString_CreateDB { get; set; }

        public String ServerName  //0 - 16  (0 = None)
        {
            get
            {
                return serverName;
            }

            set
            {
                serverName = value;
                RaisePropertyChanged(() => ServerName);
            }
        }
        public String ServerMarket  //0 - 16  (0 = None)
        {
            get
            {
                return serverMarket;
            }

            set
            {
                serverMarket = value;
                RaisePropertyChanged(() => ServerMarket);
            }
        }
        public String ServerIPAddress  //0 - 16  (0 = None)
        {
            get
            {
                return serverIPAddress;
            }

            set
            {
                serverIPAddress = value;
                RaisePropertyChanged(() => ServerIPAddress);

                //ConnectionString = @"Data Source=" + ServerIPAddress + ";Initial Catalog=IHMSwitchBladeInterfaceDB;Integrated Security=True";
            }
        }

        public String ServerComputerName
        {
            get
            {
                return serverComputerName;
            }

            set
            {
                serverComputerName = value;
                RaisePropertyChanged(() => ServerComputerName);

                //ConnectionString = @"Data Source=" + ServerComputerName + ";Initial Catalog=IHMSwitchBladeInterfaceDB;Integrated Security=SSPI";
                //ConnectionString_CreateDB = @"Data Source=" + ServerComputerName + ";Integrated Security=True";
                ConnectionString = @"Data Source=localhost;Initial Catalog=IHMSwitchBladeInterfaceDB;Integrated Security=SSPI";
                ConnectionString_CreateDB = @"Server=localhost; Integrated security = SSPI; database = master";
            }
        }

        public int Port  //0 - 16  (0 = None)
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
                RaisePropertyChanged(() => Port);
            }
        }
        public int VBPort  //0 - 16  (0 = None)
        {
            get
            {
                return vbPort;
            }

            set
            {
                vbPort = value;
                RaisePropertyChanged(() => VBPort);
            }
        }
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                connectionString = value;
                RaisePropertyChanged(() => ConnectionString);
            }
        }
        public string ConnectionStringSQLite
        {
            get
            {
                return connectionStringSQLite;
            }

            set
            {
                connectionStringSQLite = value;
                RaisePropertyChanged(() => ConnectionStringSQLite);
            }
        }
    }
}
