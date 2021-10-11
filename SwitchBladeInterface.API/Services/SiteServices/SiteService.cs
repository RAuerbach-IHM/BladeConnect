using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.SiteServices
{
    public class SiteService
    {
        /// <summary>
        /// Compares two IP addresses for equality.
        /// </summary>
        /// <param name="IPAddr1">The first IP to compare</param>
        /// <param name="IPAddr2">The second IP to compare</param>
        /// <returns>True if equal, false if not.</returns>
        static public bool AreEqual(string IPAddr1, string IPAddr2)
        {
            // convert to long in case there is any zero padding in
            // the strings
            return IPAddressToLongBackwards(IPAddr1) ==
               IPAddressToLongBackwards(IPAddr2);
        }

        /// <summary>
        /// Compares two string representations of an Ip address to
        /// see if one is greater than the other
        /// </summary>
        /// <param name="ToCompare">The IP address on the left hand
        /// side of the greater than operator</param>
        /// <param name="CompareAgainst">The Ip address on the right
        /// hand side of the greater than operator</param>
        /// <returns>True if ToCompare is greater than CompareAgainst,
        /// else false</returns>
        public bool IsGreater(string ToCompare,
                                     string CompareAgainst)
        {
            // convert to long in case there is any zero padding in
            // the strings
            return IPAddressToLongBackwards(ToCompare) >
               IPAddressToLongBackwards(CompareAgainst);
        }

        /// <summary>
        /// Compares two string representations of an Ip address to
        /// see if one is less than the other
        /// </summary>
        /// <param name="ToCompare">The IP address on the left hand
        /// side of the less than operator</param>
        /// <param name="CompareAgainst">The Ip address on the right
        /// hand side of the less than operator</param>
        /// <returns>True if ToCompare is greater than CompareAgainst,
        /// else false</returns>
        static public bool IsLess(string ToCompare,
                                  string CompareAgainst)
        {
            // convert to long in case there is any zero padding in
            // the strings
            return IPAddressToLongBackwards(ToCompare) <
               IPAddressToLongBackwards(CompareAgainst);
        }


        /// <summary>
        /// Compares two string representations of an Ip address to
        /// see if one is greater than or equal to the other.
        /// </summary>
        /// <param name="ToCompare">The IP address on the left hand
        /// side of the greater than or equal operator</param>
        /// <param name="CompareAgainst">The Ip address on the right
        /// hand side of the greater than or equal operator</param>
        /// <returns>True if ToCompare is greater than or equal to
        /// CompareAgainst, else false</returns>
        public bool IsGreaterOrEqual(string ToCompare,
                                            string CompareAgainst)
        {
            // convert to long in case there is any zero padding in
            // the strings
            return IPAddressToLongBackwards(ToCompare) >= IPAddressToLongBackwards(CompareAgainst);
        }

        /// <summary>
        /// Compares two string representations of an Ip address to
        /// see if one is less than or equal to the other.
        /// </summary>
        /// <param name="ToCompare">The IP address on the left hand
        /// side of the less than or equal operator</param>
        /// <param name="CompareAgainst">The Ip address on the right
        /// hand side of the less than or equal operator</param>
        /// <returns>True if ToCompare is greater than or equal to
        /// CompareAgainst, else false</returns>
        static public bool IsLessOrEqual(string ToCompare,
                                         string CompareAgainst)
        {
            // convert to long in case there is any zero padding in the strings
            return IPAddressToLongBackwards(ToCompare) <= IPAddressToLongBackwards(CompareAgainst);
        }

        public bool IsBetweenOrEqual(string ToCompare, string CompareAgainstLow, string CompareAgainstHigh)
        {
            // convert to long in case there is any zero padding in
            // the strings
            return (IPAddressToLongBackwards(ToCompare) >= IPAddressToLongBackwards(CompareAgainstLow) && IPAddressToLongBackwards(ToCompare) <= IPAddressToLongBackwards(CompareAgainstHigh));
        }

        /// <summary>
        /// Converts a uint representation of an Ip address to a
        /// string.
        /// </summary>
        /// <param name="IPAddr">The IP address to convert</param>
        /// <returns>A string representation of the IP address.</returns>
        static public string LongToIPAddress(uint IPAddr)
        {
            return new System.Net.IPAddress(IPAddr).ToString();
        }

        /// <summary>
        /// Converts a string representation of an IP address to a
        /// uint. This encoding is proper and can be used with other
        /// networking functions such
        /// as the System.Net.IPAddress class.
        /// </summary>
        /// <param name="IPAddr">The Ip address to convert.</param>
        /// <returns>Returns a uint representation of the IP
        /// address.</returns>
        static public uint IPAddressToLong(string IPAddr)
        {
            System.Net.IPAddress oIP = System.Net.IPAddress.Parse(IPAddr);
            byte[] byteIP = oIP.GetAddressBytes();


            uint ip = (uint)byteIP[3] << 24;
            ip += (uint)byteIP[2] << 16;
            ip += (uint)byteIP[1] << 8;
            ip += (uint)byteIP[0];

            return ip;
        }


        /// <summary>
        /// This encodes the string representation of an IP address
        /// to a uint, but backwards so that it can be used to
        /// compare addresses. This function is used internally
        /// for comparison and is not valid for valid encoding of
        /// IP address information.
        /// </summary>
        /// <param name="IPAddr">A string representation of the IP
        /// address to convert</param>
        /// <returns>Returns a backwards uint representation of the
        /// string.</returns>
        static private uint IPAddressToLongBackwards(string IPAddr)
        {
            System.Net.IPAddress oIP = System.Net.IPAddress.Parse(IPAddr);
            byte[] byteIP = oIP.GetAddressBytes();


            uint ip = (uint)byteIP[0] << 24;
            ip += (uint)byteIP[1] << 16;
            ip += (uint)byteIP[2] << 8;
            ip += (uint)byteIP[3];

            return ip;
        }
    }
}
