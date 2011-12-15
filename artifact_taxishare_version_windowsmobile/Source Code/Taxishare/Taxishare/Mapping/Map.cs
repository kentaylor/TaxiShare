using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Taxishare.Mapping
{
    class Map
    {
        //location parameters
        private string center;
        private string zoom;
        
        //map parameters
        private string size;
        private string format;
        private string mapType;
        private string mobile;

        //feature parameters
        private ArrayList markers;

        //reporting parameter
        private string sensor;

        //initalize all variables to null
        public Map()
        {
            center = "-1";
            zoom = "-1";
            size = "-1";
            format = "-1";
            mapType = "-1";
            mobile = "-1";
            markers = new ArrayList();
            sensor = "false";
        }

        // get/set methods for geocoding
        
        public string getCenter()
        {
            return center;
        }
        public void setCenter(int x, int y)
        {
            center = x.ToString() + ", " + y.ToString();
        }
        public void setCenter(string str)
        {
            str = str.Replace(" ", "+");
            center = str;
        }

        public string getZoom()
        {
            return zoom;
        }
        public void setZoom(string x)
        {
            zoom = x;
        }

        public string getSize()
        {
            return size;
        }
        public void setSize(int width, int height)
        {
            size = width.ToString() + "x" + height.ToString();
        }

        public string getFormat()
        {
            return format;
        }
        public void setFormat(string f)
        {
            format = f;
        }

        public string getMapType()
        {
            return mapType;
        }
        public void setMapType(string type)
        {
            if (type.Equals("rmap"))
            {
                mapType = "roadmap";
            }
            else if (type.Equals("sat"))
            {
                mapType = "satelite";
            }
            else if (type.Equals("hyb"))
            {
                mapType = "hybrid";
            }
            else if (type.Equals("ter"))
            {
                mapType = "terrain";
            }
        }

        public string getMobile()
        {
            return mobile;
        }
        public void setMobile(string m)
        {
            if (m.Equals("true"))
            {
                mobile = "true";
            }
            else
            {
                mobile = "false";
            }
        }

        public ArrayList getMarkers()
        {
            return markers;
        }
        public void setMarkers(string m)
        {
            markers.Add(m);
        }
        public void clearMarkers()
        {
            markers.Clear();
        }
        public string getSensor()
        {
            return sensor;
        }
        public void setSensor(string s)
        {
            sensor = s;
        }

    }
}
