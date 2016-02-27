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
            this.labelMapLocation = new System.Windows.Forms.Label();
            this.textBoxMapLocation = new System.Windows.Forms.TextBox();
            this.buttonRegions = new System.Windows.Forms.Button();
            this.buttonSalesReps = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.textBoxEdit = new System.Windows.Forms.RichTextBox();
            this.linkLabelUpdate = new System.Windows.Forms.LinkLabel();
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
            // 
            // buttonRegions
            // 
            this.buttonRegions.Location = new System.Drawing.Point(12, 40);
            this.buttonRegions.Name = "buttonRegions";
            this.buttonRegions.Size = new System.Drawing.Size(75, 23);
            this.buttonRegions.TabIndex = 3;
            this.buttonRegions.Text = "Edit Regions";
            this.buttonRegions.UseVisualStyleBackColor = true;
            this.buttonRegions.Click += new System.EventHandler(this.buttonRegions_Click);
            // 
            // buttonSalesReps
            // 
            this.buttonSalesReps.Location = new System.Drawing.Point(87, 40);
            this.buttonSalesReps.Name = "buttonSalesReps";
            this.buttonSalesReps.Size = new System.Drawing.Size(92, 23);
            this.buttonSalesReps.TabIndex = 4;
            this.buttonSalesReps.Text = "Edit Sales Reps";
            this.buttonSalesReps.UseVisualStyleBackColor = true;
            this.buttonSalesReps.Click += new System.EventHandler(this.buttonSalesReps_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(311, 157);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // linkLabelGitHub
            // 
            this.linkLabelGitHub.AutoSize = true;
            this.linkLabelGitHub.Location = new System.Drawing.Point(9, 157);
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.Size = new System.Drawing.Size(146, 13);
            this.linkLabelGitHub.TabIndex = 6;
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.Text = "Bugs/Feature Requests/Wiki";
            this.linkLabelGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHub_LinkClicked);
            // 
            // textBoxEdit
            // 
            this.textBoxEdit.Location = new System.Drawing.Point(12, 63);
            this.textBoxEdit.Name = "textBoxEdit";
            this.textBoxEdit.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxEdit.Size = new System.Drawing.Size(374, 88);
            this.textBoxEdit.TabIndex = 7;
            this.textBoxEdit.Text = "";
            // 
            // linkLabelUpdate
            // 
            this.linkLabelUpdate.AutoSize = true;
            this.linkLabelUpdate.Location = new System.Drawing.Point(9, 173);
            this.linkLabelUpdate.Name = "linkLabelUpdate";
            this.linkLabelUpdate.Size = new System.Drawing.Size(91, 13);
            this.linkLabelUpdate.TabIndex = 8;
            this.linkLabelUpdate.TabStop = true;
            this.linkLabelUpdate.Text = "Check for Update";
            this.linkLabelUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUpdate_LinkClicked);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 192);
            this.Controls.Add(this.linkLabelUpdate);
            this.Controls.Add(this.textBoxEdit);
            this.Controls.Add(this.linkLabelGitHub);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonSalesReps);
            this.Controls.Add(this.buttonRegions);
            this.Controls.Add(this.textBoxMapLocation);
            this.Controls.Add(this.labelMapLocation);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelMapLocation;
        private System.Windows.Forms.TextBox textBoxMapLocation;
        private System.Windows.Forms.Button buttonRegions;
        private System.Windows.Forms.Button buttonSalesReps;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
        private System.Windows.Forms.RichTextBox textBoxEdit;
        private System.Windows.Forms.LinkLabel linkLabelUpdate;
    }
}