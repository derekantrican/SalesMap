namespace SalesMap
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.labelMapLocation = new System.Windows.Forms.Label();
            this.textBoxMapLocation = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.linkLabelFeedback = new System.Windows.Forms.LinkLabel();
            this.linkLabelUpdate = new System.Windows.Forms.LinkLabel();
            this.checkBoxAutoUpdates = new System.Windows.Forms.CheckBox();
            this.buttonVariables = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxSendLog = new System.Windows.Forms.CheckBox();
            this.pictureBoxAbout = new System.Windows.Forms.PictureBox();
            this.OffSMRPreview = new System.Windows.Forms.PictureBox();
            this.checkBoxAboutOnStartup = new System.Windows.Forms.CheckBox();
            this.GracePeriodPreview = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControlOffSMREmail = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxOffSMRBody = new System.Windows.Forms.RichTextBox();
            this.labelEditSubject = new System.Windows.Forms.Label();
            this.textBoxOffSMRSubject = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxGracePeriodBody = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGracePeriodSubject = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxSignature = new System.Windows.Forms.RichTextBox();
            this.checkBoxInternational = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAbout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffSMRPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GracePeriodPreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControlOffSMREmail.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMapLocation
            // 
            this.labelMapLocation.AutoSize = true;
            this.labelMapLocation.Location = new System.Drawing.Point(13, 13);
            this.labelMapLocation.Name = "labelMapLocation";
            this.labelMapLocation.Size = new System.Drawing.Size(94, 13);
            this.labelMapLocation.TabIndex = 1;
            this.labelMapLocation.Text = "Map File Location:";
            // 
            // textBoxMapLocation
            // 
            this.textBoxMapLocation.Location = new System.Drawing.Point(113, 10);
            this.textBoxMapLocation.Name = "textBoxMapLocation";
            this.textBoxMapLocation.Size = new System.Drawing.Size(273, 20);
            this.textBoxMapLocation.TabIndex = 2;
            this.toolTip1.SetToolTip(this.textBoxMapLocation, "Sales Map document location");
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(328, 293);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(88, 28);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save && Close";
            this.toolTip1.SetToolTip(this.buttonSave, "Save changes\r\n\r\n(Ctrl + Click to \"factory reset\" program)");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // linkLabelFeedback
            // 
            this.linkLabelFeedback.AutoSize = true;
            this.linkLabelFeedback.Location = new System.Drawing.Point(15, 285);
            this.linkLabelFeedback.Name = "linkLabelFeedback";
            this.linkLabelFeedback.Size = new System.Drawing.Size(173, 13);
            this.linkLabelFeedback.TabIndex = 6;
            this.linkLabelFeedback.TabStop = true;
            this.linkLabelFeedback.Text = "Bugs/Feature Requests/Feedback";
            this.linkLabelFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFeedback_LinkClicked);
            // 
            // linkLabelUpdate
            // 
            this.linkLabelUpdate.AutoSize = true;
            this.linkLabelUpdate.Location = new System.Drawing.Point(15, 301);
            this.linkLabelUpdate.Name = "linkLabelUpdate";
            this.linkLabelUpdate.Size = new System.Drawing.Size(91, 13);
            this.linkLabelUpdate.TabIndex = 8;
            this.linkLabelUpdate.TabStop = true;
            this.linkLabelUpdate.Text = "Check for Update";
            this.linkLabelUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUpdate_LinkClicked);
            // 
            // checkBoxAutoUpdates
            // 
            this.checkBoxAutoUpdates.AutoSize = true;
            this.checkBoxAutoUpdates.Location = new System.Drawing.Point(224, 221);
            this.checkBoxAutoUpdates.Name = "checkBoxAutoUpdates";
            this.checkBoxAutoUpdates.Size = new System.Drawing.Size(169, 17);
            this.checkBoxAutoUpdates.TabIndex = 10;
            this.checkBoxAutoUpdates.Text = "Check for updates on startup?";
            this.checkBoxAutoUpdates.UseVisualStyleBackColor = true;
            // 
            // buttonVariables
            // 
            this.buttonVariables.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVariables.Location = new System.Drawing.Point(9, 97);
            this.buttonVariables.Name = "buttonVariables";
            this.buttonVariables.Size = new System.Drawing.Size(30, 30);
            this.buttonVariables.TabIndex = 12;
            this.buttonVariables.Text = "{V}";
            this.toolTip1.SetToolTip(this.buttonVariables, "Canned email variables");
            this.buttonVariables.UseVisualStyleBackColor = true;
            this.buttonVariables.Click += new System.EventHandler(this.buttonVariables_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 1000;
            this.toolTip1.ReshowDelay = 100;
            // 
            // checkBoxSendLog
            // 
            this.checkBoxSendLog.AutoSize = true;
            this.checkBoxSendLog.Location = new System.Drawing.Point(224, 258);
            this.checkBoxSendLog.Name = "checkBoxSendLog";
            this.checkBoxSendLog.Size = new System.Drawing.Size(152, 17);
            this.checkBoxSendLog.TabIndex = 15;
            this.checkBoxSendLog.Text = "Send log file to developer?";
            this.toolTip1.SetToolTip(this.checkBoxSendLog, "Send log file to developer when program is closed");
            this.checkBoxSendLog.UseVisualStyleBackColor = true;
            // 
            // pictureBoxAbout
            // 
            this.pictureBoxAbout.Image = global::SalesMap.Properties.Resources.about;
            this.pictureBoxAbout.Location = new System.Drawing.Point(402, 1);
            this.pictureBoxAbout.Name = "pictureBoxAbout";
            this.pictureBoxAbout.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAbout.TabIndex = 18;
            this.pictureBoxAbout.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxAbout, "About");
            this.pictureBoxAbout.Click += new System.EventHandler(this.pictureBoxAbout_Click);
            // 
            // OffSMRPreview
            // 
            this.OffSMRPreview.Image = global::SalesMap.Properties.Resources.preview;
            this.OffSMRPreview.Location = new System.Drawing.Point(45, 97);
            this.OffSMRPreview.Name = "OffSMRPreview";
            this.OffSMRPreview.Size = new System.Drawing.Size(30, 30);
            this.OffSMRPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OffSMRPreview.TabIndex = 19;
            this.OffSMRPreview.TabStop = false;
            this.toolTip1.SetToolTip(this.OffSMRPreview, "Preview Email");
            this.OffSMRPreview.Click += new System.EventHandler(this.OffSMRPreview_Click);
            // 
            // checkBoxAboutOnStartup
            // 
            this.checkBoxAboutOnStartup.AutoSize = true;
            this.checkBoxAboutOnStartup.Location = new System.Drawing.Point(224, 239);
            this.checkBoxAboutOnStartup.Name = "checkBoxAboutOnStartup";
            this.checkBoxAboutOnStartup.Size = new System.Drawing.Size(207, 17);
            this.checkBoxAboutOnStartup.TabIndex = 19;
            this.checkBoxAboutOnStartup.Text = "Show About screen with new version?";
            this.toolTip1.SetToolTip(this.checkBoxAboutOnStartup, "Show about window when a new version is run for the first time");
            this.checkBoxAboutOnStartup.UseVisualStyleBackColor = true;
            // 
            // GracePeriodPreview
            // 
            this.GracePeriodPreview.Image = global::SalesMap.Properties.Resources.preview;
            this.GracePeriodPreview.Location = new System.Drawing.Point(45, 97);
            this.GracePeriodPreview.Name = "GracePeriodPreview";
            this.GracePeriodPreview.Size = new System.Drawing.Size(30, 30);
            this.GracePeriodPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.GracePeriodPreview.TabIndex = 21;
            this.GracePeriodPreview.TabStop = false;
            this.toolTip1.SetToolTip(this.GracePeriodPreview, "Preview Email");
            this.GracePeriodPreview.Click += new System.EventHandler(this.GracePeriodPreview_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(9, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 30);
            this.button1.TabIndex = 20;
            this.button1.Text = "{V}";
            this.toolTip1.SetToolTip(this.button1, "Canned email variables");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonVariables_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControlOffSMREmail);
            this.groupBox1.Location = new System.Drawing.Point(4, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 180);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit Off SMR Email (HTML)";
            // 
            // tabControlOffSMREmail
            // 
            this.tabControlOffSMREmail.Controls.Add(this.tabPage1);
            this.tabControlOffSMREmail.Controls.Add(this.tabPage3);
            this.tabControlOffSMREmail.Controls.Add(this.tabPage2);
            this.tabControlOffSMREmail.Location = new System.Drawing.Point(6, 16);
            this.tabControlOffSMREmail.Name = "tabControlOffSMREmail";
            this.tabControlOffSMREmail.SelectedIndex = 0;
            this.tabControlOffSMREmail.Size = new System.Drawing.Size(400, 159);
            this.tabControlOffSMREmail.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.OffSMRPreview);
            this.tabPage1.Controls.Add(this.textBoxOffSMRBody);
            this.tabPage1.Controls.Add(this.labelEditSubject);
            this.tabPage1.Controls.Add(this.buttonVariables);
            this.tabPage1.Controls.Add(this.textBoxOffSMRSubject);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(392, 133);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "\"Off SMR\" body";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxOffSMRBody
            // 
            this.textBoxOffSMRBody.Location = new System.Drawing.Point(3, 5);
            this.textBoxOffSMRBody.Name = "textBoxOffSMRBody";
            this.textBoxOffSMRBody.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxOffSMRBody.Size = new System.Drawing.Size(386, 88);
            this.textBoxOffSMRBody.TabIndex = 7;
            this.textBoxOffSMRBody.Text = "";
            // 
            // labelEditSubject
            // 
            this.labelEditSubject.AutoSize = true;
            this.labelEditSubject.Location = new System.Drawing.Point(96, 106);
            this.labelEditSubject.Name = "labelEditSubject";
            this.labelEditSubject.Size = new System.Drawing.Size(46, 13);
            this.labelEditSubject.TabIndex = 15;
            this.labelEditSubject.Text = "Subject:";
            // 
            // textBoxOffSMRSubject
            // 
            this.textBoxOffSMRSubject.Location = new System.Drawing.Point(142, 103);
            this.textBoxOffSMRSubject.Name = "textBoxOffSMRSubject";
            this.textBoxOffSMRSubject.Size = new System.Drawing.Size(244, 20);
            this.textBoxOffSMRSubject.TabIndex = 16;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.GracePeriodPreview);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.textBoxGracePeriodBody);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.textBoxGracePeriodSubject);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(392, 133);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "\"Grace Period\" body";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxGracePeriodBody
            // 
            this.textBoxGracePeriodBody.Location = new System.Drawing.Point(3, 5);
            this.textBoxGracePeriodBody.Name = "textBoxGracePeriodBody";
            this.textBoxGracePeriodBody.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxGracePeriodBody.Size = new System.Drawing.Size(386, 88);
            this.textBoxGracePeriodBody.TabIndex = 17;
            this.textBoxGracePeriodBody.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Subject:";
            // 
            // textBoxGracePeriodSubject
            // 
            this.textBoxGracePeriodSubject.Location = new System.Drawing.Point(142, 103);
            this.textBoxGracePeriodSubject.Name = "textBoxGracePeriodSubject";
            this.textBoxGracePeriodSubject.Size = new System.Drawing.Size(244, 20);
            this.textBoxGracePeriodSubject.TabIndex = 19;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxSignature);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(392, 133);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Signature";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxSignature
            // 
            this.richTextBoxSignature.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxSignature.Location = new System.Drawing.Point(4, 3);
            this.richTextBoxSignature.Name = "richTextBoxSignature";
            this.richTextBoxSignature.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxSignature.Size = new System.Drawing.Size(385, 127);
            this.richTextBoxSignature.TabIndex = 8;
            this.richTextBoxSignature.Text = "";
            // 
            // checkBoxInternational
            // 
            this.checkBoxInternational.AutoSize = true;
            this.checkBoxInternational.Location = new System.Drawing.Point(18, 221);
            this.checkBoxInternational.Name = "checkBoxInternational";
            this.checkBoxInternational.Size = new System.Drawing.Size(152, 17);
            this.checkBoxInternational.TabIndex = 17;
            this.checkBoxInternational.Text = "Show international results?\r\n";
            this.checkBoxInternational.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 330);
            this.Controls.Add(this.checkBoxAboutOnStartup);
            this.Controls.Add(this.pictureBoxAbout);
            this.Controls.Add(this.checkBoxInternational);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxSendLog);
            this.Controls.Add(this.checkBoxAutoUpdates);
            this.Controls.Add(this.linkLabelUpdate);
            this.Controls.Add(this.linkLabelFeedback);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxMapLocation);
            this.Controls.Add(this.labelMapLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAbout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffSMRPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GracePeriodPreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabControlOffSMREmail.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelMapLocation;
        private System.Windows.Forms.TextBox textBoxMapLocation;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.LinkLabel linkLabelFeedback;
        private System.Windows.Forms.LinkLabel linkLabelUpdate;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdates;
        private System.Windows.Forms.Button buttonVariables;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxSendLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxOffSMRSubject;
        private System.Windows.Forms.Label labelEditSubject;
        private System.Windows.Forms.TabControl tabControlOffSMREmail;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox textBoxOffSMRBody;
        private System.Windows.Forms.RichTextBox richTextBoxSignature;
        private System.Windows.Forms.PictureBox OffSMRPreview;
        private System.Windows.Forms.CheckBox checkBoxInternational;
        private System.Windows.Forms.PictureBox pictureBoxAbout;
        private System.Windows.Forms.CheckBox checkBoxAboutOnStartup;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox textBoxGracePeriodBody;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGracePeriodSubject;
        private System.Windows.Forms.PictureBox GracePeriodPreview;
        private System.Windows.Forms.Button button1;
    }
}