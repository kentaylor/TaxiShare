using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using StedySoft.SenseSDK;
using StedySoft.SenseSDK.DrawingCE;

namespace Taxishare.Mapping
{
    class MapUtils
    {
        //creates the right format for the map markers
        public static string formatMarker(string color, string label, string coord)
        {
            return "markers=color:" + color + "|" + "label:" + label;
        }
        
        //generates the url for the map request
        public string generateMap(Map map)
        {
            string str; 
            string start = "http://maps.google.com/maps/api/staticmap?";

            str = start;

            if (!map.getCenter().Equals("-1"))
            {
                str += "center=" + map.getCenter() + "&";
            }

            if (!map.getZoom().Equals("-1"))
            {
                str += "zoom=" + map.getZoom() + "&";
            }

            if (!map.getSize().Equals("-1"))
            {
                str += "size=" + map.getSize() + "&";
            }

            if (!map.getMapType().Equals("-1"))
            { 
                str += "maptype=" + map.getMapType() + "&";
            }

            if (!(map.getMarkers().Count == 0))
            {
                int count = map.getMarkers().Count;

                for (int i = 0; i < count; i++)
                {
                   str += "markers=" + map.getMarkers()[i] + "&";
                }
            }

            if (!map.getMobile().Equals("-1"))
            {
                str += "mobile=" + map.getMobile() + "&";
            }

            str += "sensor=" + map.getSensor();    

            return str;
        }
        //gets map image
        public Bitmap getMapImage(string url)
        {
            
            try
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();
                Bitmap image = new Bitmap(responseStream);
                responseStream.Close();
                response.Close();
                return image;

            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("There was an error opening the image file." + "Check the URL");
                return null;
            }
            
        }
    }
}
