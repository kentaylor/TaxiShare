using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;


namespace Taxishare.Mapping
{
    class GeoUtils
    {

        public Location getLocation(Geo g)
        {
           try
            {
                string request = "http://maps.google.com/maps/api/geocode/xml?";
                request += "address=" + g.getAddress() + "&" + "sensor=" + g.getSensor();

                //read in xml
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(request);

                XmlNode node = xdoc.DocumentElement;
                XmlNodeList nodeLongAddrCol = node.SelectNodes("//formatted_address");

                Location loc = new Location();

                loc.setDisplay_address(nodeLongAddrCol.Item(0).InnerText);
                string lat = node.SelectNodes("//location/lat").Item(0).InnerText;
                string lng = node.SelectNodes("//location/lng").Item(0).InnerText;
                loc.setCoords(lat + "," + lng);
                loc.setLat(Convert.ToDouble(lat));
                loc.setLng(Convert.ToDouble(lng));
                return loc;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            
        }

        public Location getGeoLocation(Geo g)
        {
            //http://maps.google.com/maps/api/geocode/xml?latlng=40.714224,-73.961452&sensor=false
            string request = "http://maps.google.com/maps/api/geocode/xml?";
            request += "latlng=" + g.getLatLng() + "&" + "sensor=" + g.getSensor();

            //read in xml
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(request);

            XmlNode node = xdoc.DocumentElement;
            XmlNodeList nodeLongAddrCol = node.SelectNodes("//formatted_address");

            XmlNodeList nodeShortAddrCol = node.SelectNodes("/GeocodeResponse/address_component[type=route]");
            Location loc = new Location();
            loc.setDisplay_address(nodeLongAddrCol.Item(0).InnerText);
            
            double lat = g.getLat();
            double lng = g.getLng();
            loc.setCoords(lat + "," + lng);
            loc.setLat(lat);
            loc.setLng(lng);
            return loc;

        }
        
        //get the average centre of the map with start and destination locations
        public string getAvgLoc(double sumLat, double sumLng)
        {
            double avgLat = sumLat/2;
            double avgLng = sumLng/2;

            return (avgLat + "," + avgLng);
        }
    }
}
