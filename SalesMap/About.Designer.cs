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
            this.SuspendLayout();
            // 
            // richTextBoxChangelog
            // 
            this.richTextBoxChangelog.Location = new System.Drawing.Point(12, 90);
            this.richTextBoxChangelog.Name = "richTextBoxChangelog";
            this.richTextBoxChangelog.ReadOnly = true;
            this.richTextBoxChangelog.Size = new System.Drawing.Size(355, 116);
            this.richTextBoxChangelog.TabIndex = 0;
            this.richTextBoxChangelog.Text = "";
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
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 247);
            this.Controls.Add(this.linkLabelUpdate);
            this.Controls.Add(this.linkLabelFeedback);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.richTextBoxChangelog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "About";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxChangelog;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.LinkLabel linkLabelUpdate;
        private System.Windows.Forms.LinkLabel linkLabelFeedback;
    }
}