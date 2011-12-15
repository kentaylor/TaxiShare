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
<div id = "cities" align = "right">
    <tr>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_adelaide.php">Adelaide</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_brisbane.php">Brisbane</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_cairns.php">Cairns</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_canberra.php">Canberra</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_darwin.php">Darwin</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_hobart.php">Hobart</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_melbourne.php">Melbourne</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_newcastle.php">Newcastle</a></font>
        </td>
        <td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com/sms/taxisharemap_sydney.php">Sydney</a></font>
        </td>
 	<td>
           <font face="Arial" size="2"><a href="http://taxi.urvoting.com">TaxiShare</a></font>
        </td>
      </tr>
</div>
<div>
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
	var address = "Cairns Australia";
	var centerLat;
	var centerLng;
	var airportname ="Cairns Airport";
	var cur_sms_format;


//<![CDATA[

function load() {
 if (GBrowserIsCompatible()) {
		map = new GMap2(document.getElementById("map"));
		map.setCenter(new GLatLng(145.7746696472168, -16.92397191034689), 12);  
                map.setUIToDefault();
                showStartingMarker();
      	}
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
	
	function removepin(sms_format) {
                geocoder.getLatLng(
			sms_format + "Australia",
			function(point) {
				if (!point) {
					// do something
				} else {
                                	var centerLat =-16.92397191034689;
                                        var centerLng = 145.7746696472168;

                                      if ((point.lng() >= centerLng - 0.40) && (point.lng() <= centerLng + 0.40))
                                      {
                                         if ((point.lat() >= centerLat - 0.40) && (point.lat() <= centerLat + 0.40))
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
		);
	}

	function parseMapData() {
             $.each(rdata, function(i,item) {
                  showAddress(item.phone_number,item.sms_format, item.status_code, item.date_time, item.loc_start);
             });

	}

        function showAddress(phone_number,sms_format, status_code, date_time, loc_start) {
                geocoder.getLatLng(
			sms_format + "Australia",
			function(point) {
				if (!point) {
					// do something
				} else {
					var centerLat = -16.92397191034689;
                                        var centerLng = 145.7746696472168;

                                      if ((point.lng() >= centerLng - 0.40) && (point.lng() <= centerLng + 0.40))
                                      {
                                         if ((point.lat() >= centerLat - 0.40) && (point.lat() <= centerLat + 0.40))
                                        {
                                                 //Start creating marker
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

                                                 if (!inMap) {  //alert ("NOT inMap");
                                                   var datalocation = rdatadetail;
                                                   var html = "";
                                                   var count =  0;
                                                   var countLeft = 0;
                                                   var countGreen = 0;
                                                   var imgIcon = "";
                                                   var strcontactname = "";

                                                   side_bar_detail = "";
                                                   $.each(datalocation, function(j,item) {
                                                     if (item.sms_format == sms_format && count < 4)
                                                     {
                                                        if (item.contact_name == null){
                                                            strcontactname = "";
                                                        }
                                                        else {
                                                            strcontactname = item.contact_name;
                                                        }

                                                        if (item.status_code == "0" ) {
                                                           html = html + '<div style="font-family:arial;font-size:21px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:red;">' + 'from ' + item.loc_start + "  " + item.date_time + '</div><div> </div>';
                                                           //side_bar_detail = side_bar_detail + '<div style="font-family:Verdana;font-size:13px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:verdana;font-size:10px;text-align:right;color:red;">' + item.date_time + '</div><div> </div>'
                                                        }
                                                        else {
                                                           countGreen = countGreen + 1;
                                                           html = html + '<div style="font-family:arial;font-size:21px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:green;">' + 'from ' + item.loc_start  + "  " + item.date_time + '</div><div> </div>';
                                                           //side_bar_detail = side_bar_detail + '<div style="font-family:verdana;font-size:13px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:verdana;font-size:10px;text-align:right;color:green;">' + item.date_time + '</div><div> </div>';
                                                        }
                                                           count = count + 1;
                                                     }
                                                      //display all data for left panel
                                                      if (item.sms_format == sms_format && countLeft < 100)
                                                      {
                                                          if (item.status_code == "0" ) {
                                                               side_bar_detail = side_bar_detail + '<div style="font-family:Verdana;font-size:13px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:verdana;font-size:10px;text-align:right;color:red;">' + "from " + item.loc_start + "  " + item.date_time + '</div><div> </div>'
                                                          }
                                                            else {
                                                               side_bar_detail = side_bar_detail + '<div style="font-family:verdana;font-size:13px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:verdana;font-size:10px;text-align:right;color:green;">' + "from " + item.loc_start + "  "  + item.date_time + '</div><div> </div>';
                                                          }
                                                          countLeft = countLeft + 1;
                                                      }
                                                   });

                                                   //var letter = String.fromCharCode("A".charCodeAt(0) + (arrOfmarkers.length));
                                                   var letter = String.fromCharCode ("A".charCodeAt(0) + totalPin);

						   if( countGreen % count == 0 && countGreen > 0){
                                                      imgIcon = "http://maps.google.com/mapfiles/marker_green" + letter + ".png";
                                                      html = html + '<div style="font-family:arial;font-size:23px;text-align:left;color:green;">' + sms_format + '</div>';
                                                    }
                                                    else{
                                                      imgIcon = "http://maps.google.com/mapfiles/marker" + letter + ".png";
                                                      html = html + '<div style="font-family:arial;font-size:23px;text-align:left;color:red;">' + sms_format + '</div>';
                                                    }
 
                                                    var Icon = new GIcon(G_DEFAULT_ICON, imgIcon);
                                                    Icon.printImage = "http://maps.google.com/mapfiles/marker"+letter+"ie.gif"
                                                    Icon.mozPrintImage = "http://maps.google.com/mapfiles/marker"+letter+"ff.gif"

                                                    var marker = new GMarker(loc,{icon:Icon});
                                                    arrOfmarkers.push(marker);
                                                    marker.title =  sms_format;

                                                    map.addOverlay(marker);
                                                    totalPin = totalPin + 1;

                                                   //side_bar_html += '<font size="4" face="Verdana" ><b>'+letter+'<\/b> <a href="javascript:onleftpanelclick(' + (arrOfmarkers.length-1) + ')">' + marker.title + '<\/a><br>';
                                                   //side_bar_html += '<font size="3" face="Arial" ><a href="javascript:onleftpanelclick(' + (arrOfmarkers.length-1) + ')"><b>' + letter + " " + marker.title + '<\/b><\/a><br>';
							  
                                                   side_bar_html += '<font size="3" face="Arial" ><a href="javascript:onleftpanelclick(' + (totalPin -1) + ')"><b>' + letter + " " + marker.title + '<\/b><\/a><br>';
							  
                                    		  GEvent.addListener(marker, "mouseover", function() {
                                                        marker.openInfoWindowHtml(html);
                                                   });

                                                       side_bar_html = side_bar_html + side_bar_detail;
                                                       document.getElementById("side_bar").innerHTML = side_bar_html;
                                                 }
                                                 else {
                                                    //alert ("inMap");
                                                    //map.removeOverlay(arrOfmarkers[count]);
                                                    //arrOfmarkers.splice(count, 1);
                                                 }
                                                //end of creating marker
                                        }
                                      }
				}
			}
		);
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
                                              html = '<div style="font-family:arial;font-size:28px;text-align:left;color:green;">' + airportname + '</div>';
                                              marker.openInfoWindowHtml(html);
                                        });
				}
			}
		);
	}

	/// remove pin

	function grabDataRemove(){
          var k;
          var statusremove;
          var markertitle;

          for (m=1; m < gmarkers.length; m++){
             statusremove = true;
             markertitle = gmarkers[m].title;
             //alert("marker title: " + gmarkers[m].title);
             //alert("total markers: " + gmarkers.length);
             //alert("total prev : " + prev_gmarkers.length);

             for (k=1; k < prev_gmarkers.length; k++) {
               if (prev_gmarkers[k].title == markertitle){
                  //alert("prev title: " + prev_gmarkers[k].title);
                  statusremove = false;
               }
             }

             if (statusremove == true && prev_gmarkers.length > 0 ){
                //alert("marker remove is " + gmarkers[m].title);
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
             //alert("p: " + p);
             prev_gmarkers[p] = gmarkers[p];
          }
        }

        // end remove pin
         
          var markercounter = 0;
          function runningbubble(){
            var arrOfrunning =  arrOfmarkers;
            var runningdata =  rdatadetail;
            var countGreen = 0;
            var strcontactname = "";
            if (arrOfmarkers.length > 0)
            {
              //html = '<div style="font-family:arial;font-size:28px;text-align:left;color:red;">' + "TEST MARKER NO " + markercounter + '</div>';
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
                     html = html + '<div style="font-family:arial;font-size:20px;text-align:left;color:red;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:red;">' + "from " + item.loc_start + "  " + item.date_time + '</div><div> </div>';
                  }
                  else {
		     countGreen = countGreen  + 1;
                     html = html + '<div style="font-family:arial;font-size:20px;text-align:left;color:green;">' + strcontactname + " +" + item.phone_number + '</div>' + '<div style="font-family:arial;font-size:12px;text-align:right;color:green;">' + "from " + item.loc_start + "  " + item.date_time + '</div><div> </div>';
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





















