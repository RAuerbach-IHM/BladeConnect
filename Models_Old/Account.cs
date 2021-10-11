using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Account : ObservableObject
    {
        public Account()
        {
            id = DateTime.Now.Ticks;
            firstName = "";
            lastName = "";
            userName = "";
            password = new byte[0];
            passwordRequired = false;
            stations = new List<Station>();
            showPublicPhoneBook = true;
            showPersonalPhoneBook = false;
    }
        private long id = 0;
        private string firstName = "";
        private string lastName = "";
        private string userName = "";
        private byte[] password = new byte[0];
        private bool showPublicPhoneBook = true;
        private bool showPersonalPhoneBook = false;

        private List<Station> stations;

        public bool passwordRequired { get; set; }

        [Key]
        public long ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                RaisePropertyChanged(() => ID);
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }
        public byte[] Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                RaisePropertyChanged(() => Password);
            }
        }
        public List<Station> Stations
        {
            get
            {
                return stations;
            }

            set
            {
                stations = value;
                RaisePropertyChanged(() => Stations);
            }
        }
        public bool ShowPublicPhoneBook
        {
            get
            {
                return showPublicPhoneBook;
            }

            set
            {
                showPublicPhoneBook = value;
                RaisePropertyChanged(() => ShowPublicPhoneBook);
            }
        }
        public bool ShowPersonalPhoneBook
        {
            get
            {
                return showPersonalPhoneBook;
            }

            set
            {
                showPersonalPhoneBook = value;
                RaisePropertyChanged(() => ShowPersonalPhoneBook);
            }
        }
    }
}
