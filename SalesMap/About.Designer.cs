namespace SalesMap
{
    partial class About
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
            this.richTextBoxChangelog = new System.Windows.Forms.RichTextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.linkLabelUpdate = new System.Windows.Forms.LinkLabel();
            this.linkLabelFeedback = new System.Windows.Forms.LinkLabel();
            this.labelApplication = new System.Windows.Forms.Label();
            this.labelDeveloper = new System.Windows.Forms.Label();
            this.linkLabelEmail = new System.Windows.Forms.LinkLabel();
            this.labelVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBoxChangelog
            // 
            this.richTextBoxChangelog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBoxChangelog.Location = new System.Drawing.Point(12, 90);
            this.richTextBoxChangelog.Name = "richTextBoxChangelog";
            this.richTextBoxChangelog.ReadOnly = true;
            this.richTextBoxChangelog.Size = new System.Drawing.Size(355, 116);
            this.richTextBoxChangelog.TabIndex = 0;
            this.richTextBoxChangelog.Text = "";
            this.richTextBoxChangelog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxChangelog_LinkClicked);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(292, 213);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // linkLabelUpdate
            // 
            this.linkLabelUpdate.AutoSize = true;
            this.linkLabelUpdate.Location = new System.Drawing.Point(12, 225);
            this.linkLabelUpdate.Name = "linkLabelUpdate";
            this.linkLabelUpdate.Size = new System.Drawing.Size(91, 13);
            this.linkLabelUpdate.TabIndex = 10;
            this.linkLabelUpdate.TabStop = true;
            this.linkLabelUpdate.Text = "Check for Update";
            this.linkLabelUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUpdate_LinkClicked);
            // 
            // linkLabelFeedback
            // 
            this.linkLabelFeedback.AutoSize = true;
            this.linkLabelFeedback.Location = new System.Drawing.Point(12, 209);
            this.linkLabelFeedback.Name = "linkLabelFeedback";
            this.linkLabelFeedback.Size = new System.Drawing.Size(173, 13);
            this.linkLabelFeedback.TabIndex = 9;
            this.linkLabelFeedback.TabStop = true;
            this.linkLabelFeedback.Text = "Bugs/Feature Requests/Feedback";
            this.linkLabelFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFeedback_LinkClicked);
            // 
            // labelApplication
            // 
            this.labelApplication.AutoSize = true;
            this.labelApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplication.Location = new System.Drawing.Point(136, 9);
            this.labelApplication.Name = "labelApplication";
            this.labelApplication.Size = new System.Drawing.Size(99, 13);
            this.labelApplication.TabIndex = 11;
            this.labelApplication.Text = "About SalesMap";
            // 
            // labelDeveloper
            // 
            this.labelDeveloper.AutoSize = true;
            this.labelDeveloper.Location = new System.Drawing.Point(9, 50);
            this.labelDeveloper.Name = "labelDeveloper";
            this.labelDeveloper.Size = new System.Drawing.Size(133, 13);
            this.labelDeveloper.TabIndex = 12;
            this.labelDeveloper.Text = "Developer: Derek Antrican";
            // 
            // linkLabelEmail
            // 
            this.linkLabelEmail.AutoSize = true;
            this.linkLabelEmail.Location = new System.Drawing.Point(10, 65);
            this.linkLabelEmail.Name = "linkLabelEmail";
            this.linkLabelEmail.Size = new System.Drawing.Size(156, 13);
            this.linkLabelEmail.TabIndex = 13;
            this.linkLabelEmail.TabStop = true;
            this.linkLabelEmail.Text = "derek.antrican@sigmanest.com";
            this.linkLabelEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEmail_LinkClicked);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(9, 36);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(48, 13);
            this.labelVersion.TabIndex = 14;
            this.labelVersion.Text = "Version: ";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 247);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.linkLabelEmail);
            this.Controls.Add(this.labelDeveloper);
            this.Controls.Add(this.labelApplication);
            this.Controls.Add(this.linkLabelUpdate);
            this.Controls.Add(this.linkLabelFeedback);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.richTextBoxChangelog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxChangelog;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.LinkLabel linkLabelUpdate;
        private System.Windows.Forms.LinkLabel linkLabelFeedback;
        private System.Windows.Forms.Label labelApplication;
        private System.Windows.Forms.Label labelDeveloper;
        private System.Windows.Forms.LinkLabel linkLabelEmail;
        private System.Windows.Forms.Label labelVersion;
    }
}