using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

/* 
 * This class ported to C# and adapted for use in the TaxiShare client from 
 * http://svn.appelsiini.net/svn/javascript/trunk/google_maps_nojs/Google/Maps.php
 * All credits go to Mika Tuupola (http://www.appelsiini.net/) 
 *
 */

namespace Taxishare.Mapping
{
    //methods that deal with tanslations from pixel to coord and coord to pixels
    class CoordTranslate
    {
       static double googleOffset = 268435456;
       static double googleOffsetRadius = googleOffset / Math.PI;
       static double p180 = Math.PI/180;

       static private double preLonToX1 = googleOffsetRadius * (Math.PI/180);

       public static double LonToX( double lon ) 
       {
         return googleOffset + preLonToX1 * lon;
       }

       public static double LatToY( double lat ) 
       {
         return googleOffset - googleOffsetRadius * Math.Log((1 + Math.Sin(lat * p180)) / (1 - Math.Sin(lat * p180))) / 2;
       }

       public static double XToLon( double x) 
       {
         return ((x - googleOffset) / googleOffsetRadius) * 180/ Math.PI;
       }

       public static double YToLat( double y) 
       {
         return (Math.PI / 2 - 2 * Math.Atan(Math.Exp((y - googleOffset) / googleOffsetRadius))) * 180 / Math.PI;
       }
        
       public static double adjustLonByPixels( double lon, int delta, int zoom) 
       {
         return XToLon(LonToX(lon) + (delta << (21 - zoom)));
       }

       public static double adjustLatByPixels( double lat,  int delta, int zoom) 
       {
         return YToLat(LatToY(lat) + (delta << (21 - zoom)));
       }
    }
}
