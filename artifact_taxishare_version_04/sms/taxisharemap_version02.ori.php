<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"
    "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<title>TaxiShare Map</title>
<meta http-equiv="content-type" content="text/html; charset=utf-8"/>
<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
<META HTTP-EQUIV="Expires" CONTENT="-1">

<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js" type="text/javascript"></script>

</head>
<body onload="load()" onunload="GUnload()">
<div id="main"><font size="6" face="Verdana">TaxiShare - sms your destination to 0416 907 025</font></div>
<!--<div id="main"><font size="6" face="Verdana">TaxiShare - sms your destination to 0447 100 264</font></div>-->
      <table>
      <tr>
        <td>
           <div id="side_bar" style="border:10px solid white;width:200px;height:770px;" style="overflow:scroll;"></div>
        </td>
        <td >
           <div id="map" style="border:10px solid white;width:1000px;height:770px;"></div>
        </td>
      </tr>
    </table>
</div>

<script src="http://maps.google.com/maps?file=api&amp;v=2&amp;key=ABQIAAAAElTxcz5N5uLxF6BQmLBVpBQX9f2uJD3Wcmwboc0S9a0QOzBC_hQMyK-eJ39LEZdxWeenZSoa7zqbXQ" type="text/javascript"></script>
<script type="text/javascript">

	var map;
	var address = "Sydney Australia";
	var centerLat;
	var centerLng;
	var airportname ="Sydney Airport";
	var cur_sms_format;

//<![CDATA[

function load() {
 if (GBrowserIsCompatible()) {
		map = new GMap2(document.getElementById("map"));
                geocoder = new GClientGeocoder();
                geocoder.getLocations(address, setMapCenter);

                map.setUIToDefault();
                showStartingMarker();
      	}
}
// This function adds the point to the map

function setMapCenter(response)
{
   // Retrieve the object
   place = response.Placemark[0];
   
   // Retrieve the latitude and longitude
   point = new GLatLng(place.Point.coordinates[1],
                       place.Point.coordinates[0]);
                       
   centerLat = place.Point.coordinates[1];
   centerLng = place.Point.coordinates[0];

   // Center the map on this point
   map.setCenter(point, 13);

}


//]]>
</script>

<script>
	var rdata;
	var rdatadetail;
	var totalPin;
	var rdataremove;

	var curPos = 0;
        var counter = 0;
	var tmr;
	var geocoder = new GClientGeocoder();
	var gmarkers=[];
	var prev_gmarkers=[];

        var arrOfmarkers = [];
        var arrOfrunning = [];

        var side_bar_html = "";
        var side_bar_detail = "";

        function grabData() {
             rdataPrev = rdata;
             counter = 0 ;
             gmarkers = [];
             totalPin = 0;
             side_bar_html = "";

             $.getJSON("./getjsondatadetail.php", function(datadetail) {
                   rdatadetail = datadetail;
             });
             
             $.getJSON("./getjsondataremove.php", function(dataremove) {
                   rdataremove = dataremove;
                   parseRemoveData();
              });

             $.getJSON("./getjsondata.php", function(data) {
                   rdata = data;
                   parseMapData();
              });
        }
        
        function parseRemoveData() {
             $.each(rdataremove, function(i,itemremove) {
                  removepin(itemremove.sms_format);
             });
	}

	function removepin(sms_format){
             geocoder = new GClientGeocoder();
             geocoder.getLocations(sms_format + "Australia", setRemoveMarker);
        }

        function setRemoveMarker(response)
        {
           // Retrieve the object
           place = response.Placemark[0];

           // Retrieve the latitude and longitude
           point = new GLatLng(place.Point.coordinates[1],
                               place.Point.coordinates[0]);

                               if (!point) {
					// do something
				} else {
                                      if ((point.lng() >= centerLng - 0.20) && (point.lng() <= centerLng + 0.20))
                                      {
                                         if ((point.lat() >= centerLat - 0.10) && (point.lat() <= centerLat + 0.10))
                                        {
                                                 var count = 0;
                                                 var loc = new GLatLng(point.lat(), point.lng());

                                                 while(count < arrOfmarkers.length){
                                                    if ((loc.lat() == arrOfmarkers[count].getLatLng().lat()) && (loc.lng() == arrOfmarkers[count].getLatLng().lng())) {
                                                      map.removeOverlay(arrOfmarkers[count]);
                                                      arrOfmarkers.splice(count, 1);

                                                      break;
                                                    }
                                                    count++;
                                                 }

						side_bar_detail  = "";
   						document.getElementById("side_bar").innerHTML = side_bar_html;
                                        }
                                      }
				}
        }

	function parseMapData() {
             $.each(rdata, function(i,item) {
                  showAddress(item.phone_number,item.sms_format, item.status_code, item.date_time);
             });
	}
	
	function showAddress(phone_number,sms_format, status_code, date_time) {
             geocoder = new GClientGeocoder();
                          
             geocoder.getLocations(sms_format + "Australia", setMapMarker);
             cur_sms_format = sms_format;

             

        }
               

        function setMapMarker(response)
        {
           // Retrieve the object
           place = response.Placemark[0];

	   // Retrieve the latitude and longitude
           point = new GLatLng(place.Point.coordinates[1],
                               place.Point.coordinates[0]);

                            if (!point) {
					// do something
				} else {
				      if ((point.lng() >= centerLng - 0.20) && (point.lng() <= centerLng + 0.20))
                                      {
                                         if ((point.lat() >= centerLat - 0.10) && (point.lat() <= centerLat + 0.10))
                                        {
                                                 var inMap = false;
                                                 var count = 0;
                                                 var loc = new GLatLng(point.lat(), point.lng());

                                                 while(inMap == false && count < arrOfmarkers.length){
                                                    if ((loc.lat() == arrOfmarkers[count].getLatLng().lat()) && (loc.lng() == arrOfmarkers[count].getLatLng().lng())) {
                                                      map.removeOverlay(arrOfmarkers[count]);
                                                      arrOfmarkers.splice(count, 1);
                                                      inMap = false;
                                                      break;
                                                    }
                                                    count++;
                                                 }

                                                 if (!inMap) {
						   var datalocation = rdatadetail;
                                                   var html = "";
                                                   var count =  0;
                                                   var countLeft = 0;
                                                   var countGreen = 0;
                                                   var imgIcon = "";
                                                   var strcontactname = "";

                                                   side_bar_detail = "";
                                                   $.each(datalocation, function(j,item) {
                                                     if (item.sms_format == cur_sms_format  && count < 4)
                                                     {
                                                        if (item.contact_name == null){
                                                            strcontactname = "";
                                                        }
                                                        else {
                                                            strcontactname = item.contact_name;
                                                        }

                                                        if (item.status_code == "0" ) {
                                                           html = html + '<div style="font-family:arial;font-size:21px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:red;">' + item.date_time + '</div>';
                                                        }
                                                        else {
                                                           countGreen = countGreen + 1;
                                                           html = html + '<div style="font-family:arial;font-size:21px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:green;">' + item.date_time + '</div>';
                                                        }
                                                           count = count + 1;
                                                     }
                                                      //display all data for left panel
                                                      if (item.sms_format == cur_sms_format && countLeft < 100)
                                                      {
                                                          if (item.status_code == "0" ) {
                                                               side_bar_detail = side_bar_detail + '<div style="font-family:Verdana;font-size:13px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:verdana;font-size:10px;text-align:right;color:red;">' + item.date_time + '</div>'
                                                          }
                                                            else {
                                                               side_bar_detail = side_bar_detail + '<div style="font-family:verdana;font-size:13px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:verdana;font-size:10px;text-align:right;color:green;">' + item.date_time + '</div>';
                                                          }
                                                          countLeft = countLeft + 1;
                                                      }
                                                    });

                                                    var letter = String.fromCharCode ("A".charCodeAt(0) + totalPin);

						    if( countGreen % count == 0 && countGreen > 0){
                                                      imgIcon = "http://maps.google.com/mapfiles/marker_green" + letter + ".png";
                                                      html = html + '<div style="font-family:arial;font-size:23px;text-align:left;color:green;">' + cur_sms_format + '</div>';
                                                    }
                                                    else{
                                                      imgIcon = "http://maps.google.com/mapfiles/marker" + letter + ".png";
                                                      html = html + '<div style="font-family:arial;font-size:23px;text-align:left;color:red;">' + cur_sms_format + '</div>';
                                                    }

                                                    var Icon = new GIcon(G_DEFAULT_ICON, imgIcon);
                                                    Icon.printImage = "http://maps.google.com/mapfiles/marker"+letter+"ie.gif"
                                                    Icon.mozPrintImage = "http://maps.google.com/mapfiles/marker"+letter+"ff.gif"

                                                    var marker = new GMarker(loc,{icon:Icon});
                                                    arrOfmarkers.push(marker);
                                                    marker.title = sms_format;

                                                    map.addOverlay(marker);
                                                    totalPin = totalPin + 1;
                                                    side_bar_html += '<font size="3" face="Arial" ><a href="javascript:onleftpanelclick(' + (totalPin -1) + ')"><b>' + letter + " " + marker.title + '<\/b><\/a><br>';

                                    		    GEvent.addListener(marker, "mouseover", function() {
                                                         marker.openInfoWindowHtml(html);
                                                    });

                                                    side_bar_html = side_bar_html + side_bar_detail;
                                                    document.getElementById("side_bar").innerHTML = side_bar_html;
                                                 }
                                        }
                                      }
				}
        }

       // This function picks up the click and opens the corresponding info window
       function onleftpanelclick(i) {
        GEvent.trigger(arrOfmarkers[i], "mouseover");
       }

       function showStartingMarker() {
                geocoder.getLatLng(
			airportname + " Australia",
			function(point) {
				if (!point) {
					// do something
				} else {

                                        var icon = new GIcon();
                                        icon.image = '<? echo $mosConfig_live_site ?>'+ "http://maps.google.com/mapfiles/arrow.png";

                                        icon.iconSize = new GSize(40, 45);
                                        icon.shadowSize = new GSize(22, 20);
                                        icon.iconAnchor = new GPoint(6, 20);
                                        icon.infoWindowAnchor = new GPoint(5, 1);

                                        map.panTo(point);
    					var marker = new GMarker(point,icon);
    					marker.title = airportname;
    				        map.addOverlay(marker);

                                        var html;
    				        GEvent.addListener(marker, "mouseover", function() {
                                              html = '<div style="font-family:arial;font-size:28px;text-align:left;color:green;">' + "You Are Here " + '</div>' + '<div style="font-family:arial;font-size:17px;text-align:right;color:green;">' + airportname + '</div>';
                                              marker.openInfoWindowHtml(html);
                                        });
				}
			}
		);
	}

	function grabDataRemove(){
          var k;
          var statusremove;
          var markertitle;

          for (m=1; m < gmarkers.length; m++){
             statusremove = true;
             markertitle = gmarkers[m].title;

             for (k=1; k < prev_gmarkers.length; k++) {
               if (prev_gmarkers[k].title == markertitle){
                  statusremove = false;
               }
             }

             if (statusremove == true && prev_gmarkers.length > 0 ){
                map.removeOverlay(gmarkers[m],true);
             }
          }
        
          for (m=1; m < prev_gmarkers.length; m++){
             tmr = map.removeOverlay(prev_gmarkers[m],true);
             pauseJS(250);
          }

          if (prev_gmarkers.length > 0)
          {
             prev_gmarkers = [];
          }

          for (p=1; p < gmarkers.length; p++) {
             prev_gmarkers[p] = gmarkers[p];
          }
        }

          var markercounter = 0;
          function runningbubble(){
            var arrOfrunning =  arrOfmarkers;
            var runningdata =  rdatadetail;
            var countGreen = 0;
            var strcontactname = "";
            if (arrOfmarkers.length > 0)
            {
               html = "";
               count = 0;

               $.each(runningdata, function(j,item) {
               if (item.sms_format == arrOfrunning[markercounter].title && count < 4)
               {
                  if (item.contact_name == null){
                     strcontactname = "";
                  }
                  else {
                     strcontactname = item.contact_name;
                  }

                  if (item.status_code == "0" ) {
                     html = html + '<div style="font-family:arial;font-size:20px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:red;">' + item.date_time + '</div>';
                  }
                  else {
		     countGreen = countGreen  + 1;
                     html = html + '<div style="font-family:arial;font-size:20px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:green;">' + item.date_time + '</div>';
                  }
                   
                  count = count + 1;

                  }
               });

              if(countGreen % count == 0 && countGreen > 0 ){
                 html = html + '<div style="font-family:arial;font-size:23px;text-align:left;color:green;">' + arrOfrunning[markercounter].title + '</div>';
              }
                else{
                html = html + '<div style="font-family:arial;font-size:23px;text-align:left;color:red;">' + arrOfrunning[markercounter].title + '</div>';
              }

              arrOfrunning[markercounter].openInfoWindowHtml(html);
              markercounter = (markercounter + 1) % arrOfrunning.length;
            }

          }

	 $(document).ready(function(){
              grabData();
              tmr = setInterval(grabData, 20000);
              tmr = setInterval(runningbubble, 2000);
         });

</script>
</body>

</html>





















