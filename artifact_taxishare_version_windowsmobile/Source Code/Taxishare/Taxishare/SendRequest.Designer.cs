namespace Taxishare
{
    partial class SendRequest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.miNext = new System.Windows.Forms.MenuItem();
            this.miGetCellLoc = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.miGetLoc = new System.Windows.Forms.MenuItem();
            this.miOnGPS = new System.Windows.Forms.MenuItem();
            this.miOffGPS = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.miMap = new System.Windows.Forms.MenuItem();
            this.miHybrid = new System.Windows.Forms.MenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mapContext = new System.Windows.Forms.ContextMenu();
            this.zoomInCI = new System.Windows.Forms.MenuItem();
            this.zoomInCI1 = new System.Windows.Forms.MenuItem();
            this.zoomInCI3 = new System.Windows.Forms.MenuItem();
            this.zoomInCI5 = new System.Windows.Forms.MenuItem();
            this.zoomOutCI = new System.Windows.Forms.MenuItem();
            this.zoomOutCI1 = new System.Windows.Forms.MenuItem();
            this.zoomOutCI3 = new System.Windows.Forms.MenuItem();
            this.zoomOutCI5 = new System.Windows.Forms.MenuItem();
            this.centreMapCI = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.setLocCI = new System.Windows.Forms.MenuItem();
            this.setDestCI = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mainTitle = new StedySoft.SenseSDK.SenseHeaderControl();
            this.btnSearch = new MichyPrima.ManilaDotNetSDK.ManilaButton();
            this.btnConfirm = new MichyPrima.ManilaDotNetSDK.ManilaButton();
            this.btnClear = new MichyPrima.ManilaDotNetSDK.ManilaButton();
            this.tbLoc = new MichyPrima.ManilaDotNetSDK.ManilaNewTextBox();
            this.tbDest = new MichyPrima.ManilaDotNetSDK.ManilaNewTextBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.miNext);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Back";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // miNext
            // 
            this.miNext.MenuItems.Add(this.miGetCellLoc);
            this.miNext.MenuItems.Add(this.menuItem5);
            this.miNext.MenuItems.Add(this.miGetLoc);
            this.miNext.MenuItems.Add(this.miOnGPS);
            this.miNext.MenuItems.Add(this.miOffGPS);
            this.miNext.MenuItems.Add(this.menuItem2);
            this.miNext.MenuItems.Add(this.menuItem7);
            this.miNext.Text = "Options";
            this.miNext.Click += new System.EventHandler(this.miNext_Click);
            // 
            // miGetCellLoc
            // 
            this.miGetCellLoc.Text = "Get Cell Location";
            this.miGetCellLoc.Click += new System.EventHandler(this.miGetCellLoc_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "-";
            // 
            // miGetLoc
            // 
            this.miGetLoc.Enabled = false;
            this.miGetLoc.Text = "Get GPS Location";
            this.miGetLoc.Click += new System.EventHandler(this.miGetLoc_Click);
            // 
            // miOnGPS
            // 
            this.miOnGPS.Text = "Turn On GPS";
            this.miOnGPS.Click += new System.EventHandler(this.miOnGPS_Click);
            // 
            // miOffGPS
            // 
            this.miOffGPS.Enabled = false;
            this.miOffGPS.Text = "Turn Off GPS";
            this.miOffGPS.Click += new System.EventHandler(this.miOffGPS_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "-";
            // 
            // menuItem7
            // 
            this.menuItem7.MenuItems.Add(this.miMap);
            this.menuItem7.MenuItems.Add(this.miHybrid);
            this.menuItem7.Text = "Change View";
            // 
            // miMap
            // 
            this.miMap.Text = "Map";
            this.miMap.Click += new System.EventHandler(this.miMap_Click);
            // 
            // miHybrid
            // 
            this.miHybrid.Text = "Hybrid";
            this.miHybrid.Click += new System.EventHandler(this.miHybrid_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenu = this.mapContext;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 236);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(480, 300);
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // mapContext
            // 
            this.mapContext.MenuItems.Add(this.zoomInCI);
            this.mapContext.MenuItems.Add(this.zoomOutCI);
            this.mapContext.MenuItems.Add(this.centreMapCI);
            this.mapContext.MenuItems.Add(this.menuItem6);
            this.mapContext.MenuItems.Add(this.setLocCI);
            this.mapContext.MenuItems.Add(this.setDestCI);
            this.mapContext.MenuItems.Add(this.menuItem4);
            this.mapContext.MenuItems.Add(this.menuItem3);
            this.mapContext.Popup += new System.EventHandler(this.mapContext_Popup);
            // 
            // zoomInCI
            // 
            this.zoomInCI.MenuItems.Add(this.zoomInCI1);
            this.zoomInCI.MenuItems.Add(this.zoomInCI3);
            this.zoomInCI.MenuItems.Add(this.zoomInCI5);
            this.zoomInCI.Text = "Zoom In";
            this.zoomInCI.Click += new System.EventHandler(this.zoomInCI_Click);
            // 
            // zoomInCI1
            // 
            this.zoomInCI1.Text = "+1";
            this.zoomInCI1.Click += new System.EventHandler(this.zoomInCI1_Click);
            // 
            // zoomInCI3
            // 
            this.zoomInCI3.Text = "+3";
            this.zoomInCI3.Click += new System.EventHandler(this.zoomInCI3_Click);
            // 
            // zoomInCI5
            // 
            this.zoomInCI5.Text = "+5";
            this.zoomInCI5.Click += new System.EventHandler(this.zoomInCI5_Click);
            // 
            // zoomOutCI
            // 
            this.zoomOutCI.MenuItems.Add(this.zoomOutCI1);
            this.zoomOutCI.MenuItems.Add(this.zoomOutCI3);
            this.zoomOutCI.MenuItems.Add(this.zoomOutCI5);
            this.zoomOutCI.Text = "Zoom Out";
            this.zoomOutCI.Click += new System.EventHandler(this.zoomOutCI_Click_1);
            // 
            // zoomOutCI1
            // 
            this.zoomOutCI1.Text = "-1";
            this.zoomOutCI1.Click += new System.EventHandler(this.zoomOutCI1_Click);
            // 
            // zoomOutCI3
            // 
            this.zoomOutCI3.Text = "-3";
            this.zoomOutCI3.Click += new System.EventHandler(this.zoomOutCI3_Click);
            // 
            // zoomOutCI5
            // 
            this.zoomOutCI5.Text = "-5";
            this.zoomOutCI5.Click += new System.EventHandler(this.zoomOutCI5_Click);
            // 
            // centreMapCI
            // 
            this.centreMapCI.Text = "Centre Map";
            this.centreMapCI.Click += new System.EventHandler(this.centreMapCI_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Text = "-";
            // 
            // setLocCI
            // 
            this.setLocCI.Text = "Set Location";
            this.setLocCI.Click += new System.EventHandler(this.setLocCI_Click);
            // 
            // setDestCI
            // 
            this.setDestCI.Text = "Set Destination";
            this.setDestCI.Click += new System.EventHandler(this.setDestCI_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "Clear Markers";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 36);
            this.label1.Text = "Location:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 38);
            this.label2.Text = "Destination:";
            // 
            // mainTitle
            // 
            this.mainTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainTitle.Location = new System.Drawing.Point(0, 0);
            this.mainTitle.Name = "mainTitle";
            this.mainTitle.Size = new System.Drawing.Size(480, 25);
            this.mainTitle.TabIndex = 7;
            this.mainTitle.Text = "Send Request";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(162, 182);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(152, 48);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(326, 182);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(148, 48);
            this.btnConfirm.TabIndex = 13;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(6, 182);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(144, 48);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbLoc
            // 
            this.tbLoc.Location = new System.Drawing.Point(162, 58);
            this.tbLoc.Name = "tbLoc";
            this.tbLoc.Size = new System.Drawing.Size(312, 52);
            this.tbLoc.TabIndex = 18;
            // 
            // tbDest
            // 
            this.tbDest.Location = new System.Drawing.Point(162, 118);
            this.tbDest.Name = "tbDest";
            this.tbDest.Size = new System.Drawing.Size(312, 52);
            this.tbDest.TabIndex = 19;
            // 
            // MapTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(480, 536);
            this.Controls.Add(this.tbDest);
            this.Controls.Add(this.tbLoc);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.mainTitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Location = new System.Drawing.Point(0, 52);
            this.Menu = this.mainMenu1;
            this.Name = "MapTest";
            this.Text = "[TS v0.1]";
            this.Load += new System.EventHandler(this.MapTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenu mapContext;
        private System.Windows.Forms.MenuItem zoomInCI;
        private System.Windows.Forms.MenuItem zoomOutCI;
        private System.Windows.Forms.MenuItem setLocCI;
        private System.Windows.Forms.MenuItem setDestCI;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem centreMapCI;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private StedySoft.SenseSDK.SenseHeaderControl mainTitle;
        private MichyPrima.ManilaDotNetSDK.ManilaButton btnSearch;
        private System.Windows.Forms.MenuItem miNext;
        private MichyPrima.ManilaDotNetSDK.ManilaButton btnConfirm;
        private MichyPrima.ManilaDotNetSDK.ManilaButton btnClear;
        private MichyPrima.ManilaDotNetSDK.ManilaNewTextBox tbLoc;
        private MichyPrima.ManilaDotNetSDK.ManilaNewTextBox tbDest;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem miGetLoc;
        private System.Windows.Forms.MenuItem miOnGPS;
        private System.Windows.Forms.MenuItem miOffGPS;
        private System.Windows.Forms.MenuItem miGetCellLoc;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem zoomInCI1;
        private System.Windows.Forms.MenuItem zoomInCI5;
        private System.Windows.Forms.MenuItem zoomOutCI1;
        private System.Windows.Forms.MenuItem zoomOutCI5;
        private System.Windows.Forms.MenuItem zoomInCI3;
        private System.Windows.Forms.MenuItem zoomOutCI3;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem miMap;
        private System.Windows.Forms.MenuItem miHybrid;
    }
}