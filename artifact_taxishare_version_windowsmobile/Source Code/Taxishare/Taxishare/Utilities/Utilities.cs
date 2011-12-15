using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile;
using Microsoft.WindowsMobile.Telephony;
using Microsoft.WindowsMobile.PocketOutlook;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Net;

namespace Taxishare.Utilities
{
    class Utilities
    {
        //making calls
        public void makeCall(string phoneNum)
        {
            Phone myPhone = new Microsoft.WindowsMobile.Telephony.Phone();
            myPhone.Talk(phoneNum+"\0"); 
        }

        
        //send request to SMS server
        public bool sendRequest(string curLoc, string destLoc)
        {
            try
            {
                SmsMessage s = new SmsMessage("0416907025", "FROM " + curLoc + " TO " + destLoc);
                s.Send();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
