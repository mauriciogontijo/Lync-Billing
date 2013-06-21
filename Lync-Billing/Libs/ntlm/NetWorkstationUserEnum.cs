using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Web;

    public class NetWorkstationUserEnum
    {
        [DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int NetWkstaUserEnum(
           string servername,
           int level,
           out IntPtr bufptr,
           int prefmaxlen,
           out int entriesread,
           out int totalentries,
           ref int resume_handle);

        [DllImport("netapi32.dll")]
        static extern int NetApiBufferFree(
           IntPtr Buffer);

        const int NERR_SUCCESS = 0;
        const int ERROR_MORE_DATA = 234;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WKSTA_USER_INFO_0
        {
            public string wkui0_username;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WKSTA_USER_INFO_1
        {
            public string wkui1_username;
            public string wkui1_logon_domain;
            public string wkui1_oth_domains;
            public string wkui1_logon_server;
        }
       
        public string[] ScanHost(string hostName)
        {
            List<String> Users = new List<string>();

            IntPtr bufptr = IntPtr.Zero;
            int dwEntriesread;
            int dwTotalentries = 0;
            int dwResumehandle = 0;
            int nStatus;
            Type tWui1 = typeof(WKSTA_USER_INFO_1);
            int nStructSize = Marshal.SizeOf(tWui1);
            WKSTA_USER_INFO_1 wui1;

            //this.listView1.Items.Clear();

            do
            {
                nStatus = NetWkstaUserEnum(
                  hostName, 1, out bufptr, 32768,
                  out dwEntriesread,
                  out dwTotalentries,
                  ref dwResumehandle);

                // If the call succeeds,
                if ((nStatus == NERR_SUCCESS) | (nStatus == ERROR_MORE_DATA))
                {
                    if (dwEntriesread > 0)
                    {
                        IntPtr pstruct = bufptr;

                        // Loop through the entries.
                        for (int i = 0; i < dwEntriesread; i++)
                        {
                            wui1 = (WKSTA_USER_INFO_1)Marshal.PtrToStructure(pstruct, tWui1);

                            Users.Add(wui1.wkui1_logon_domain + "\\" + wui1.wkui1_username);
                            pstruct = (IntPtr)((int)pstruct + nStructSize);
                        }
                    }
                }

                if (bufptr != IntPtr.Zero)
                    NetApiBufferFree(bufptr);

            } while (nStatus == ERROR_MORE_DATA);

            return Users.ToArray();

        }

        public string DNSLookup(string IP)
        {
            string hostname = String.Empty;

            try
            {
                IPAddress myIP = IPAddress.Parse(IP);
                IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
                hostname = GetIPHost.HostName;
            }
            catch
            {
            }

            return hostname;

        }

    }

