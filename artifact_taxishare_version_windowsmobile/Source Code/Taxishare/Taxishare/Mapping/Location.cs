using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Taxishare.Mapping
{
    class Location
    {
        private string short_address;
        private string display_address;
        private string coords;
        private double lat;
        private double lng;
        private int ready = 0;

        public void setDisplay_address(string addr)
        {
            display_address = addr;
        }
        public string getDisplay_address()
        {
            return display_address;
        }

        public void setShort_address(string addr)
        {
            short_address = addr;
        }
        public string getShort_address()
        {
            return short_address;
        }

        public void setCoords(string c)
        {
            coords = c;
        }
        public string getCoords()
        {
            return coords;
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

        public int getReady()
        {
            return ready;
        }
        public void setReady(int r)
        {
            ready = r;
        }
    }
}
