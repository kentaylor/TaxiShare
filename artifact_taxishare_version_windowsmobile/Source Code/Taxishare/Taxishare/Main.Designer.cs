namespace Taxishare
{
    partial class Main
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
            this.mainTitle = new StedySoft.SenseSDK.SenseHeaderControl();
            this.senseListCtrl = new StedySoft.SenseSDK.SenseListControl();
            this.miExit = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.miExit);
            // 
            // mainTitle
            // 
            this.mainTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainTitle.Location = new System.Drawing.Point(0, 0);
            this.mainTitle.Name = "mainTitle";
            this.mainTitle.Size = new System.Drawing.Size(480, 25);
            this.mainTitle.TabIndex = 0;
            this.mainTitle.Text = "Taxishare System Menu";
            // 
            // senseListCtrl
            // 
            this.senseListCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.senseListCtrl.FocusedItem = null;
            this.senseListCtrl.IsSecondaryScrollType = false;
            this.senseListCtrl.Location = new System.Drawing.Point(0, 25);
            this.senseListCtrl.Name = "senseListCtrl";
            this.senseListCtrl.ShowScrollIndicator = true;
            this.senseListCtrl.Size = new System.Drawing.Size(480, 511);
            this.senseListCtrl.TabIndex = 1;
            this.senseListCtrl.TopIndex = 0;
            this.senseListCtrl.Velocity = 0.55F;
            this.senseListCtrl.Click += new System.EventHandler(this.senseListCtrl_Click);
            // 
            // miExit
            // 
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(480, 536);
            this.Controls.Add(this.senseListCtrl);
            this.Controls.Add(this.mainTitle);
            this.Location = new System.Drawing.Point(0, 52);
            this.Menu = this.mainMenu1;
            this.Name = "Main";
            this.Text = "[TS v0.1]";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private StedySoft.SenseSDK.SenseHeaderControl mainTitle;
        private StedySoft.SenseSDK.SenseListControl senseListCtrl;
        private System.Windows.Forms.MenuItem miExit;
      

    }
}