using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

/* 
 * Portions of code were modified and adapted for use in the TaxiShare client from 
 * http://www.koders.com/csharp/fid1082DBFC02F278AB454637F47D905067E093FA6B.aspx?s=zoom 
 * All credits go to Neil Young (neil.young@freenet.de) 
 *
 * Code to get Cell Tower data is based on code from 
 * http://dalelane.co.uk/blog/?p=241
 * All credits go to Dale Lane
 */

namespace Taxishare.Cell
{
    class CellUtils
    {
        private static RILCELLTOWERINFO _towerDetails;
        private static AutoResetEvent waithandle = new AutoResetEvent(false);
        private double lat;
        private double lng;
        #region pInvoke
        public delegate void RILRESULTCALLBACK(uint dwCode, IntPtr hrCmdID, IntPtr lpData, uint cbData, uint dwParam);
        public delegate void RILNOTIFYCALLBACK(uint dwCode, IntPtr lpData, uint cbData, uint dwParam);
        //public static IntPtr hRil;
        
        [StructLayout(LayoutKind.Explicit)]
        class RILCELLTOWERINFO
        {
            [FieldOffset(0)]
            uint dwSize;
            [FieldOffset(4)]
            uint dwParams;
            [FieldOffset(8)]
            public uint dwMobileCountryCode;
            [FieldOffset(12)]
            public uint dwMobileNetworkCode;
            [FieldOffset(16)]
            public uint dwLocationAreaCode;
            [FieldOffset(20)]
            public uint dwCellID;
            [FieldOffset(24)]
            uint dwBaseStationID;
            [FieldOffset(28)]
            uint dwBroadcastControlChannel;
            [FieldOffset(32)]
            uint dwRxLevel;
            [FieldOffset(36)]
            uint dwRxLevelFull;
            [FieldOffset(40)]
            uint dwRxLevelSub;
            [FieldOffset(44)]
            uint dwRxQuality;
            [FieldOffset(48)]
            uint dwRxQualityFull;
            [FieldOffset(52)]
            uint dwRxQualitySub;
            /* More minor interesting fields below */
        }

        [DllImport("ril.dll")]
        private static extern IntPtr RIL_Initialize(uint dwIndex, RILRESULTCALLBACK pfnResult, RILNOTIFYCALLBACK pfnNotify, 
            uint dwNotificationClasses, uint dwParam, out IntPtr lphRil);
        [DllImport("ril.dll", EntryPoint = "RIL_GetCellTowerInfo")]
        private static extern IntPtr RIL_GetCellTowerInfo(IntPtr hRil);
        [DllImport("ril.dll", EntryPoint = "RIL_Hangup")]
        private static extern IntPtr RIL_Hangup(IntPtr hRil);
        [DllImport("ril.dll")]
        private static extern IntPtr RIL_Deinitialize(IntPtr hRil);

        #endregion

        public CellTower getTowerInfo()
        {
            IntPtr radioInterfaceLayerHandle = IntPtr.Zero;
            IntPtr radioResponseHandle = IntPtr.Zero;

            // Initialize the radio layer with a result callback parameter.
            radioResponseHandle = RIL_Initialize(1, new RILRESULTCALLBACK(CellDataCallback),
                null, 0, 0, out radioInterfaceLayerHandle);

            // The initialize API call will always return 0 if initialization is successful.
            if (radioResponseHandle != IntPtr.Zero)
            {
                return null;
            }

            // Query for the current tower data.
            radioResponseHandle = RIL_GetCellTowerInfo(radioInterfaceLayerHandle);

            // Wait for cell tower info to be returned since RIL_GetCellTowerInfo invokes the
            // callback method asynchronously.
            waithandle.WaitOne();

            // Release the RIL handle
            RIL_Deinitialize(radioInterfaceLayerHandle);

            // Convert the raw tower data structure data into a CellTower object
            CellTower ct = new CellTower();
            ct.setTowerId(Convert.ToInt32(_towerDetails.dwCellID));
            ct.setLocationAreaCode(Convert.ToInt32(_towerDetails.dwLocationAreaCode));
            ct.setMobileCountryCode(Convert.ToInt32(_towerDetails.dwMobileCountryCode));
            ct.setMobileNetworkCode(Convert.ToInt32(_towerDetails.dwMobileNetworkCode));

            return ct;
           
        }

        public byte[] PostData(int MCC, int MNC, int LAC, int CID, bool shortCID)
        {
            /* The shortCID parameter follows heuristic experiences:
             * Sometimes UMTS CIDs are build up from the original GSM CID (lower 4 hex digits)
             * and the RNC-ID left shifted into the upper 4 digits.
             */
            byte[] pd = new byte[] {
                0x00, 0x0e,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,

                0x1b,
                0x00, 0x00, 0x00, 0x00, // Offset 0x11
                0x00, 0x00, 0x00, 0x00, // Offset 0x15
                0x00, 0x00, 0x00, 0x00, // Offset 0x19
                0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, // Offset 0x1f
                0x00, 0x00, 0x00, 0x00, // Offset 0x23
                0x00, 0x00, 0x00, 0x00, // Offset 0x27
                0x00, 0x00, 0x00, 0x00, // Offset 0x2b
                0xff, 0xff, 0xff, 0xff,
                0x00, 0x00, 0x00, 0x00
            };

            bool isUMTSCell = ((Int64)CID > 65535);

            if (isUMTSCell)
                Console.WriteLine("UMTS CID. {0}", shortCID ? "Using short CID to resolve." : "");
            else
                Console.WriteLine("GSM CID given.");

            if (shortCID)
                CID &= 0xFFFF;      /* Attempt to resolve the cell using the GSM CID part */

            if ((Int64)CID > 65536) /* GSM: 4 hex digits, UTMS: 6 hex digits */
                pd[0x1c] = 5;
            else
                pd[0x1c] = 3;

            pd[0x11] = (byte)((MNC >> 24) & 0xFF);
            pd[0x12] = (byte)((MNC >> 16) & 0xFF);
            pd[0x13] = (byte)((MNC >> 8) & 0xFF);
            pd[0x14] = (byte)((MNC >> 0) & 0xFF);

            pd[0x15] = (byte)((MCC >> 24) & 0xFF);
            pd[0x16] = (byte)((MCC >> 16) & 0xFF);
            pd[0x17] = (byte)((MCC >> 8) & 0xFF);
            pd[0x18] = (byte)((MCC >> 0) & 0xFF);

            pd[0x27] = (byte)((MNC >> 24) & 0xFF);
            pd[0x28] = (byte)((MNC >> 16) & 0xFF);
            pd[0x29] = (byte)((MNC >> 8) & 0xFF);
            pd[0x2a] = (byte)((MNC >> 0) & 0xFF);

            pd[0x2b] = (byte)((MCC >> 24) & 0xFF);
            pd[0x2c] = (byte)((MCC >> 16) & 0xFF);
            pd[0x2d] = (byte)((MCC >> 8) & 0xFF);
            pd[0x2e] = (byte)((MCC >> 0) & 0xFF);

            pd[0x1f] = (byte)((CID >> 24) & 0xFF);
            pd[0x20] = (byte)((CID >> 16) & 0xFF);
            pd[0x21] = (byte)((CID >> 8) & 0xFF);
            pd[0x22] = (byte)((CID >> 0) & 0xFF);

            pd[0x23] = (byte)((LAC >> 24) & 0xFF);
            pd[0x24] = (byte)((LAC >> 16) & 0xFF);
            pd[0x25] = (byte)((LAC >> 8) & 0xFF);
            pd[0x26] = (byte)((LAC >> 0) & 0xFF);

            return pd;
        }

        public string getCoords(CellTower ct)
        {
            try
            {
                String url = "http://www.google.com/glm/mmap";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
                req.Method = "POST";

                byte[] pd = PostData(ct.getMobileCountryCode(), ct.getMobileNetworkCode(), ct.getLocationAreaCode(), ct.getTowerId(), false);

                req.ContentLength = pd.Length;
                req.ContentType = "application/binary";
                Stream outputStream = req.GetRequestStream();
                outputStream.Write(pd, 0, pd.Length);
                outputStream.Close();

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                byte[] ps = new byte[res.ContentLength];
                int totalBytesRead = 0;
                while (totalBytesRead < ps.Length)
                {
                    totalBytesRead += res.GetResponseStream().Read(ps, totalBytesRead, ps.Length - totalBytesRead);
                }

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    short opcode1 = (short)(ps[0] << 8 | ps[1]);
                    byte opcode2 = ps[2];
                    System.Diagnostics.Debug.Assert(opcode1 == 0x0e);
                    System.Diagnostics.Debug.Assert(opcode2 == 0x1b);
                    int ret_code = (int)((ps[3] << 24) | (ps[4] << 16) | (ps[5] << 8) | (ps[6]));

                    if (ret_code == 0)
                    {
                        double tlat = ((double)((ps[7] << 24) | (ps[8] << 16) | (ps[9] << 8) | (ps[10]))) / 1000000;
                        double tlon = ((double)((ps[11] << 24) | (ps[12] << 16) | (ps[13] << 8) | (ps[14]))) / 1000000;
                        lat = tlat;
                        lng = tlon;
                        return (tlat + "," + tlon);

                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
                    
            }
            catch (Exception)
            {
                return null;
            }
        }
        public double getLat()
        {
            return lat;
        }
        public double getLng()
        {
            return lng;
        }
        public static void CellDataCallback(uint dwCode, IntPtr hrCmdID, IntPtr lpData, uint cbData, uint dwParam)
        {
            // Refresh the current tower details
            _towerDetails = new RILCELLTOWERINFO();

            // Copy result returned from RIL into structure
            Marshal.PtrToStructure(lpData, _towerDetails);

            // notify caller function that we have a result
            waithandle.Set();
        }
    }


}
