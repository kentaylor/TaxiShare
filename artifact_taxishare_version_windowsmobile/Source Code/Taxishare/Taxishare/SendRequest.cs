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
using Microsoft.WindowsMobile.Status;
using Microsoft.WindowsMobile.Samples.Location;


namespace Taxishare
{
    public partial class SendRequest : Form
    {
        private Point pt;
        private Mapping.GeoUtils gu;
        private Mapping.MapUtils mu;
        private Mapping.Map currMap;
        private Mapping.Marker currMarker;
        private Mapping.Location currCentre;
        private Mapping.Location startLoc;
        private Mapping.Location destLoc;
        private Point p;
        private Mapping.Geo g;
        private int zoom = 10;
        Gps gps = new Gps();

        public SendRequest()
        {
            InitializeComponent();
        }

        private void MapTest_Load(object sender, EventArgs e)
        {
            tbDest.Text = "Enter Destination";
            tbLoc.Text = "Enter Location";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            p = new Point();
            currMap = new Mapping.Map();
            currMarker = new Mapping.Marker();
            mu = new Mapping.MapUtils();
            gu = new Mapping.GeoUtils();
            g = new Mapping.Geo();
            //check phone radio is on.
            if (SystemState.PhoneRadioOff == false)
            {
                Cell.CellUtils cu = new Cell.CellUtils();
                Cell.CellTower ct = cu.getTowerInfo();
                g.setLatLng(cu.getCoords(ct));
                g.setLat(cu.getLat());
                g.setLng(cu.getLng());
                currCentre = gu.getGeoLocation(g);
                currMarker = new Mapping.Marker(1, "red", "S", currCentre.getCoords());
                //update location textbox
                tbLoc.Text = currCentre.getDisplay_address();
            }
            else
            {
                g.setAddress("ANU");
                currCentre = gu.getLocation(g);
            }
           
            //initialize map settings
            startLoc = new Mapping.Location();
            destLoc = new Mapping.Location();


            currMap.setMapType("rmap");
            currMap.setCenter(currCentre.getCoords());
            currMap.setSensor("false");
            currMap.setSize(480, 300);
            currMap.setZoom(zoom.ToString());
            //currMarker = new Mapping.Marker(1, "red", "S", currCentre.getCoords());
            currMap.clearMarkers();
            currMap.setMarkers(currMarker.toString());
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));

            //add start location
            startLoc = new Mapping.Location();
            startLoc.setReady(1); 
            startLoc.setLat(currCentre.getLat());
            startLoc.setLng(currCentre.getLng());
            startLoc.setCoords(currCentre.getCoords());

            //tbLoc.Text = currCentre.getDisplay_address();
            
        }

        private void zoomOutCI_Click_1(object sender, EventArgs e)
        {

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (gps.Opened)
            {
                gps.Close();
            }
            this.Dispose();
            this.Close();
        }

        private void mapContext_Popup(object sender, EventArgs e)
        {
            p.X = MousePosition.X;
            p.Y = MousePosition.Y;
        }

        private void zoomInCI_Click(object sender, EventArgs e)
        {

        }

        private void setLocCI_Click(object sender, EventArgs e)
        {
            pt = new Point();
            pt = pictureBox1.PointToClient(p);
            gu = new Mapping.GeoUtils();
            mu = new Mapping.MapUtils();

            //translate pixels tapped on to GPS coordinates
            double lat = currCentre.getLat();
            double lng = currCentre.getLng();
            double newLat = Mapping.CoordTranslate.adjustLatByPixels(lat, pt.Y - 150, zoom);
            double newLng = Mapping.CoordTranslate.adjustLonByPixels(lng, pt.X - 240, zoom);

            //clear previous markers from the array
            currMap.clearMarkers();

            //add the new loc marker to the map
            currMarker = new Mapping.Marker(1, "red", "S", (newLat + "," + newLng));
            currMap.setMarkers(currMarker.toString());

            //check if need to add existing destination marker
            if (destLoc.getReady() == 1)
            {
                currMarker = new Mapping.Marker(1, "yellow", "D", destLoc.getCoords());
                currMap.setMarkers(currMarker.toString());
            }

            //generate map
            currMap.setCenter((newLat + "," + newLng));
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();

            //update start location object
            startLoc.setLat(newLat);
            startLoc.setLng(newLng);
            startLoc.setCoords((newLat + "," + newLng));
            startLoc.setReady(1);

            //update centre location
            currCentre.setLat(newLat);
            currCentre.setLng(newLng);
            currCentre.setCoords((newLat + "," + newLng));

            //update address to textbox
            Mapping.Geo tempG = new Mapping.Geo();
            tempG.setLat(newLat);
            tempG.setLatLng((newLat + "," + newLng));
            Mapping.Location tempLoc = gu.getGeoLocation(tempG);
            tbLoc.Text = tempLoc.getDisplay_address();
        }

        private void setDestCI_Click(object sender, EventArgs e)
        {
            pt = new Point();
            pt = pictureBox1.PointToClient(p);
            gu = new Mapping.GeoUtils();
            mu = new Mapping.MapUtils();

            //translate pixels tapped on to GPS coordinates
            double lat = currCentre.getLat();
            double lng = currCentre.getLng();
            double newLat = Mapping.CoordTranslate.adjustLatByPixels(lat, pt.Y - 150, zoom);
            double newLng = Mapping.CoordTranslate.adjustLonByPixels(lng, pt.X - 240, zoom);

            //clear previous markers from the array
            currMap.clearMarkers();

            //add the new loc marker to the map
            currMarker = new Mapping.Marker(1, "yellow", "D", (newLat + "," + newLng));
            currMap.setMarkers(currMarker.toString());

            //check if need to add existing destination marker
            if (startLoc.getReady() == 1)
            {
                currMarker = new Mapping.Marker(1, "red", "S", startLoc.getCoords());
                currMap.setMarkers(currMarker.toString());
            }

            //generate map
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();

            //update destination location object
            destLoc.setLat(newLat);
            destLoc.setLng(newLng);
            destLoc.setCoords((newLat + "," + newLng));
            destLoc.setReady(1);

            //update address to textbox
            Mapping.Geo tempG = new Mapping.Geo();
            tempG.setLat(newLat);
            tempG.setLatLng((newLat + "," + newLng));
            Mapping.Location tempLoc = gu.getGeoLocation(tempG);
            tbDest.Text = tempLoc.getDisplay_address();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //clear everything
            mu = new Mapping.MapUtils();
            currMap.clearMarkers();
            tbLoc.Text = "Enter Location";
            tbDest.Text = "Enter Destination";
            //refresh map
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            //update status of start and destination locations
            destLoc.setReady(0);
            startLoc.setReady(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if ( !(tbLoc.Text.Equals("Enter Location")) && !(tbDest.Text.Equals("Enter Destination")) )
            {
                mu = new Mapping.MapUtils();

                //get current location
                Mapping.Geo g = new Mapping.Geo();
                gu = new Mapping.GeoUtils();
                g.setAddress(tbLoc.Text);
                startLoc = gu.getLocation(g);
                if (startLoc == null)
                {
                    MessageBox.Show("No matches found for current location.");
                }
                else
                {
                    startLoc.setReady(1);

                    //get intended destination
                    g.setAddress(tbDest.Text);
                    destLoc = gu.getLocation(g);
                    if (destLoc == null)
                    {
                        MessageBox.Show("No matches found for destination.");
                    }
                    else
                    {
                        destLoc.setReady(1);

                        //get avg map centre
                        string avgLoc = gu.getAvgLoc(startLoc.getLat() + destLoc.getLat(), startLoc.getLng() + destLoc.getLng());

                        //update display map
                        //clear previous markers from the array
                        currMap.clearMarkers();
                        //add the new loc markers to the map
                        currMarker = new Mapping.Marker(1, "yellow", "D", destLoc.getCoords());
                        currMap.setMarkers(currMarker.toString());
                        currMarker = new Mapping.Marker(1, "red", "S", startLoc.getCoords());
                        currMap.setMarkers(currMarker.toString());
                        currMap.setCenter(avgLoc);

                        //generate map
                        pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
                        pictureBox1.Refresh();

                        //update current centre
                        currCentre.setCoords(avgLoc);
                        currCentre.setLat((startLoc.getLat() + destLoc.getLat()) / 2);
                        currCentre.setLng((startLoc.getLng() + destLoc.getLng()) / 2);
                    }
                    
                }
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            //clear everything
            mu = new Mapping.MapUtils();
            currMap.clearMarkers();
            tbLoc.Text = "Enter Location";
            tbDest.Text = "Enter Destination";
            //refresh map
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            //update status of start and destination locations
            destLoc.setReady(0);
            startLoc.setReady(0);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Utilities.Utilities ut = new Utilities.Utilities();

            if (!(tbLoc.Text.Equals("Enter Location")) || !(tbDest.Text.Equals("Enter Destination")) || !(startLoc.getReady() == 0) || !(destLoc.getReady() == 0))
            {
                string confirm = "Are you are heading from " + tbLoc.Text + " to " + tbDest.Text + "?";
                if (Sense.SenseMessageBox.Show(confirm, "Confirm Request", Sense.SenseMessageBox.SenseMessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //send request through sms
                    if (ut.sendRequest(tbLoc.Text, tbDest.Text))
                    {
                        string str = "Your request has been sent to the TaxiShare system.";
                        if (Sense.SenseMessageBox.Show(str, "Request Confirmed", Sense.SenseMessageBox.SenseMessageBoxButtons.OK) == DialogResult.OK)
                        {

                            this.Dispose();
                            this.Close();
                        }
                    }
                    else
                    {
                        string str = "Your request has failed. Click OK to go back to the main menu.";
                        if (Sense.SenseMessageBox.Show(str, "Request Failed", Sense.SenseMessageBox.SenseMessageBoxButtons.OK) == DialogResult.OK)
                        {
                            this.Dispose();
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                string str = "Please complete the request before confirming.";
                if (Sense.SenseMessageBox.Show(str, "Request Failed", Sense.SenseMessageBox.SenseMessageBoxButtons.OK) == DialogResult.OK)
                {
                }
            }
        }


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            pt = new Point();
            pt = pictureBox1.PointToClient(MousePosition);
            gu = new Mapping.GeoUtils();
            mu = new Mapping.MapUtils();

            //translate pixels tapped on to GPS coordinates
            double lat = currCentre.getLat();
            double lng = currCentre.getLng();
            double newLat = Mapping.CoordTranslate.adjustLatByPixels(lat, pt.Y - 150, zoom);
            double newLng = Mapping.CoordTranslate.adjustLonByPixels(lng, pt.X - 240, zoom);
            //update current centre object
            currCentre.setLat(newLat);
            currCentre.setLng(newLng);
            currCentre.setCoords(newLat + "," + newLng);
            currMap.setCenter(currCentre.getCoords());
            //refresh map
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();
        }

        private void centreMapCI_Click(object sender, EventArgs e)
        {
            pt = new Point();
            pt = pictureBox1.PointToClient(p);
            gu = new Mapping.GeoUtils();
            mu = new Mapping.MapUtils();

            //translate pixels tapped on to GPS coordinates
            double lat = currCentre.getLat();
            double lng = currCentre.getLng();
            double newLat = Mapping.CoordTranslate.adjustLatByPixels(lat, pt.Y - 150, zoom);
            double newLng = Mapping.CoordTranslate.adjustLonByPixels(lng, pt.X - 240, zoom);
            //update current centre object
            currCentre.setLat(newLat);
            currCentre.setLng(newLng);
            currCentre.setCoords(newLat + "," + newLng);
            currMap.setCenter(currCentre.getCoords());
            //refresh map
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();
        }



        private void miNext_Click(object sender, EventArgs e)
        {

        }

        private void miGetLoc_Click(object sender, EventArgs e)
        {
            if (gps.GetPosition().LatitudeValid)
            { //Has position: do something with the data } 
                genMap(gps.GetPosition().Latitude, gps.GetPosition().Longitude);
            }
            else
            {
                MessageBox.Show("GPS is not ready...");
            }
        }

        private void genMap(double lat, double lng)
        {
            
            p = new Point();
            currMap = new Mapping.Map();
            currMarker = new Mapping.Marker();
            mu = new Mapping.MapUtils();
            gu = new Mapping.GeoUtils();
            g = new Mapping.Geo();

            g.setLatLng(lat + "," + lng);
            g.setLat(lat);
            g.setLng(lng);
            currCentre = gu.getGeoLocation(g);

            //update start location object
            startLoc.setLat(currCentre.getLat());
            startLoc.setLng(currCentre.getLng());
            startLoc.setCoords(currCentre.getCoords());
            startLoc.setDisplay_address(currCentre.getDisplay_address());
            startLoc.setReady(1);

            currMap.setMapType("hyb");
            currMap.setCenter(currCentre.getCoords());
            currMap.setSensor("false");
            currMap.setSize(480, 300);
            currMap.setZoom(zoom.ToString());
            currMap.clearMarkers();
            currMarker = new Mapping.Marker(1, "red", "S", startLoc.getCoords());
            
            if (destLoc.getReady() == 1)
            {
                currMarker = new Mapping.Marker(1, "yellow", "D", startLoc.getCoords());
            }

            currMap.setMarkers(currMarker.toString());
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();

            //update location textbox
            tbLoc.Text = startLoc.getDisplay_address();
        }

        private void miOnGPS_Click(object sender, EventArgs e)
        {
            if (!gps.Opened)
            {
                gps.Open();
            }

            miOnGPS.Enabled = false;
            miOffGPS.Enabled = true;
            miGetLoc.Enabled = true;
        }

        private void miOffGPS_Click(object sender, EventArgs e)
        {
            if (gps.Opened)
            {
                gps.Close();
            }

            miOnGPS.Enabled = true;
            miOffGPS.Enabled = false;
            miGetLoc.Enabled = false;
        }

        private void miGetCellLoc_Click(object sender, EventArgs e)
        {
            p = new Point();
            currMap = new Mapping.Map();
            currMarker = new Mapping.Marker();
            mu = new Mapping.MapUtils();
            gu = new Mapping.GeoUtils();
            g = new Mapping.Geo();

            Cell.CellUtils cu = new Cell.CellUtils();
            Cell.CellTower ct = cu.getTowerInfo();
            g.setLatLng(cu.getCoords(ct));
            g.setLat(cu.getLat());
            g.setLng(cu.getLng());
            currCentre = gu.getGeoLocation(g);

            //update start location object
            startLoc.setLat(currCentre.getLat());
            startLoc.setLng(currCentre.getLng());
            startLoc.setCoords(currCentre.getCoords());
            startLoc.setDisplay_address(currCentre.getDisplay_address());
            startLoc.setReady(1);

            currMap.setMapType("hyb");
            currMap.setCenter(currCentre.getCoords());
            currMap.setSensor("false");
            currMap.setSize(480, 300);
            currMap.setZoom(zoom.ToString());
            currMap.clearMarkers();
            currMarker = new Mapping.Marker(1, "red", "S", startLoc.getCoords());

            if (destLoc.getReady() == 1)
            {
                currMarker = new Mapping.Marker(1, "yellow", "D", destLoc.getCoords());
            }

            currMap.setMarkers(currMarker.toString());
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();

            //update location textbox
            tbLoc.Text = startLoc.getDisplay_address();
        }

        private void zoomInCI1_Click(object sender, EventArgs e)
        {
            zoomIn(1);
        }

        private void zoomInCI3_Click(object sender, EventArgs e)
        {
            zoomIn(3);
        }

        private void zoomInCI5_Click(object sender, EventArgs e)
        {
            zoomIn(5);
        }

        private void zoomOutCI1_Click(object sender, EventArgs e)
        {
            zoomOut(1);
        }

        private void zoomOutCI3_Click(object sender, EventArgs e)
        {
            zoomOut(3);
        }

        private void zoomOutCI5_Click(object sender, EventArgs e)
        {
            zoomOut(5);
        }


        private void zoomOut(int zo)
        {
            //get the current zoom and decrement by 5
            zoom = Convert.ToInt32(currMap.getZoom());

            //set zoom boundaries
            if ((zoom - zo) < 0)
            {
                zoom = 0;
            }
            else
            {
                zoom = zoom - zo;
            }

            currMap.setZoom(zoom.ToString());
            //repaint image
            Mapping.MapUtils mu = new Mapping.MapUtils();
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();
        }


        private void zoomIn(int zi)
        {
            //get the current zoom and increment by 1
            zoom = Convert.ToInt32(currMap.getZoom());
            zoom = zoom + zi;

            //set zoom boundaries
            if ((zoom + zi) > 21)
            {
                zoom = 21;
            }
            else
            {
                zoom = zoom + zi;
            }

            currMap.setZoom(zoom.ToString());
            //repaint image
            mu = new Mapping.MapUtils();
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();
        }

        private void miMap_Click(object sender, EventArgs e)
        {
            currMap.setMapType("rmap");
            //repaint image
            Mapping.MapUtils mu = new Mapping.MapUtils();
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();
        }

        private void miHybrid_Click(object sender, EventArgs e)
        {
            currMap.setMapType("hyb");
            //repaint image
            Mapping.MapUtils mu = new Mapping.MapUtils();
            pictureBox1.Image = mu.getMapImage(mu.generateMap(currMap));
            pictureBox1.Refresh();
        }

    }
}