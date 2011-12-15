using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Reflection;
using Microsoft.Drawing;
using StedySoft.SenseSDK;
using StedySoft.SenseSDK.DrawingCE;
using System.Xml;





namespace Taxishare
{
    public partial class Main : Form
    {
        #region Declarations
        #endregion

        #region Constructor
        public Main()
        {
            InitializeComponent();
        }
        #endregion

        #region Misc Methods

        private bool isVGA()
        {
            return StedySoft.SenseSDK.DrawingCE.Resolution.ScreenIsVGA;
        }

        private IImage getMenuIcon(string img)
        {
            IImage iimg;
            using (MemoryStream strm = (MemoryStream)Assembly.GetExecutingAssembly().GetManifestResourceStream("Taxishare.Icons." + img + ".png"))
            {
                (ImagingFactory.GetImaging()).CreateImageFromBuffer(strm.GetBuffer(), (uint)strm.Length, BufferDisposalFlag.BufferDisposalFlagNone, out iimg);
            }
            return iimg;
        }

        #endregion

        #region Events
        private void Main_Load(object sender, EventArgs e)
        {
            // set the list scroll fluidness
            this.senseListCtrl.Velocity = .80f;

            // turn off UI updating
            this.senseListCtrl.BeginUpdate();

            // create header divider item
            StedySoft.SenseSDK.SensePanelDividerItem header = new StedySoft.SenseSDK.SensePanelDividerItem();
            //create panel item
            StedySoft.SenseSDK.SensePanelItem item = new StedySoft.SenseSDK.SensePanelItem();

            //---main menu divider---//
            //header.Text = "Requests";
            //this.senseListCtrl.AddItem(header);

            //add submit taxi request panel    
            IImage iimg = this.getMenuIcon("submit_request");
            item.IThumbnail = iimg;
            item.PrimaryText = "Submit a taxishare request ";
            item.SecondaryText = "Request a taxi from your destination";
            item.Tag = "taxiReqTag";
            item.OnClick += new SensePanelItem.ClickEventHandler(OnClickSubmitRequest);
            this.senseListCtrl.AddItem(item);

            //add about panel
            item = new StedySoft.SenseSDK.SensePanelItem();
            iimg = this.getMenuIcon("about");
            item.IThumbnail = iimg;
            item.PrimaryText = "About";
            item.SecondaryText = "About the application";
            item.Tag = "aboutAppTag";
            item.OnClick += new SensePanelItem.ClickEventHandler(OnClickAbout);
            this.senseListCtrl.AddItem(item);

            //add quit panel
            item = new StedySoft.SenseSDK.SensePanelItem();
            iimg = this.getMenuIcon("quit");
            item.IThumbnail = iimg;
            item.PrimaryText = "Quit";
            item.SecondaryText = "Exit the application";
            item.Tag = "exitAppTag";
            item.OnClick += new SensePanelItem.ClickEventHandler(OnClickQuit);
            this.senseListCtrl.AddItem(item);

            //update items so they show on the list
            this.senseListCtrl.EndUpdate();
        }
        #endregion

        #region Event Listeners

        void OnClickQuit(object Sender)
        {
            Application.Exit();            
        }

        void OnClickSubmitRequest(object Sender)
        {
            SendRequest mt = new SendRequest();
            mt.Show();
        }

        void OnClickMatch(object Sender)
        {
        }

        void OnClickAbout(object Sender)
        {
            Sense.SenseMessageBox.Show("TaxiShare App [v0.1]", "About", Sense.SenseMessageBox.SenseMessageBoxButtons.OK);
        }

        #endregion

        private void senseListCtrl_Click(object sender, EventArgs e)
        {

        }

        private void miExit_Click(object sender, EventArgs e)
        {
            
            this.Dispose();
            Application.Exit();
        }

    }
}