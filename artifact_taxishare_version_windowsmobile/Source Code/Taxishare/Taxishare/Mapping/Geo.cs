using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Taxishare.Mapping
{
    class Geo
    {
        
        //type parameter
        private int type;
        
        //Normal geocoding parameters
        private string address;

        //reverse geocoding parameters
        private string latLng;
        private string sensor;
        private double lat;
        private double lng;

        public Geo()
        {
            sensor = "false";
        }

        //getter and setter methods for geocoding.
        
        public string getAddress()
        {
            return address;
        }
        public void setAddress(string add)
        {
            add = add.Replace(" ", "+");
            address = add;
        }

        public int getType()
        {
            return type;
        }
        public void setType(int i)
        {
            type = i;
        }

        public string getSensor()
        {
            return sensor;
        }
        public void setSensor(string s)
        {
            sensor = s;
        }

        public string getLatLng()
        {
            return latLng;
        }
        public void setLatLng(String ll)
        {
            latLng = ll;
        }

        public double getLat()
        {
            return lat;
        }

        public void setLat(double lt)
        {
            lat = lt;
        }

        public double getLng()
        {
            return lng;
        }

        public void setLng(double lg)
        {
            lng = lg;
        }
    }
}
