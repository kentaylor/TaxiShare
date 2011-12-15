using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;


namespace Taxishare.Mapping
{
    class Marker
    {
        private int type;
        private string color;
        private string label;
        private string coords;
        private string url;

        public Marker()
        {
            type = 1;
            color = "red";
        }
        public Marker(int t, string uc, string l, string cds)
        {
            if (t == 1)
            {
                type = t;
                color = uc;
                label = l;
                coords = cds;
            }
            else
            {
                type = t;
                url = uc;
                label = l;
                coords = cds;
            }
        }

        public void setType(int i)
        {
            type = i;
        }
        public int getType()
        {
            return type;
        }

        public void setColor(string c)
        {
            color = c;
        }
        public string getColor()
        {
            return color;
        }

        public void setLabel(string l)
        {
            label = l;
        }
        public string getLabel()
        {
            return label;
        }

        public void setCoords(string c)
        {
            coords = c;
        }
        public string getCoords()
        {
            return coords;
        }

        public void setUrl(string u)
        {
            url = u;
        }
        public string getUrl()
        {
            return url;
        }

        public string toString()
        {
            string str ="";
            switch (type)
            {
                case 1:
                    str = "color:" + color + "|label:" + label + "|" + coords;
                    break;
                case 2:
                    str = "icon:" + url + "|label:" + label + "|" + coords;
                    break;
            }
            return str;
        }
    
    
    
    
    }
}
