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
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.textBoxEdit = new System.Windows.Forms.RichTextBox();
            this.linkLabelUpdate = new System.Windows.Forms.LinkLabel();
            this.checkBoxAutoUpdates = new System.Windows.Forms.CheckBox();
            this.labelOffSMR = new System.Windows.Forms.Label();
            this.buttonVariables = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxSignature = new System.Windows.Forms.CheckBox();
            this.linkLabelSignature = new System.Windows.Forms.LinkLabel();
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
            this.buttonSave.Location = new System.Drawing.Point(300, 190);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(88, 28);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save && Close";
            this.toolTip1.SetToolTip(this.buttonSave, "Save changes\r\n\r\n(Ctrl + Click to \"factory reset\" program)");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // linkLabelGitHub
            // 
            this.linkLabelGitHub.AutoSize = true;
            this.linkLabelGitHub.Location = new System.Drawing.Point(9, 188);
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.Size = new System.Drawing.Size(146, 13);
            this.linkLabelGitHub.TabIndex = 6;
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.Text = "Wiki/Bugs/Feature Requests";
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
            this.linkLabelUpdate.Location = new System.Drawing.Point(9, 204);
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
            this.checkBoxAutoUpdates.Location = new System.Drawing.Point(123, 204);
            this.checkBoxAutoUpdates.Name = "checkBoxAutoUpdates";
            this.checkBoxAutoUpdates.Size = new System.Drawing.Size(169, 17);
            this.checkBoxAutoUpdates.TabIndex = 10;
            this.checkBoxAutoUpdates.Text = "Check for updates on startup?";
            this.checkBoxAutoUpdates.UseVisualStyleBackColor = true;
            // 
            // labelOffSMR
            // 
            this.labelOffSMR.AutoSize = true;
            this.labelOffSMR.Location = new System.Drawing.Point(13, 47);
            this.labelOffSMR.Name = "labelOffSMR";
            this.labelOffSMR.Size = new System.Drawing.Size(100, 13);
            this.labelOffSMR.TabIndex = 11;
            this.labelOffSMR.Text = "Edit Off SMR Email:";
            // 
            // buttonVariables
            // 
            this.buttonVariables.Location = new System.Drawing.Point(356, 37);
            this.buttonVariables.Name = "buttonVariables";
            this.buttonVariables.Size = new System.Drawing.Size(30, 23);
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
            // checkBoxSignature
            // 
            this.checkBoxSignature.AutoSize = true;
            this.checkBoxSignature.Location = new System.Drawing.Point(225, 157);
            this.checkBoxSignature.Name = "checkBoxSignature";
            this.checkBoxSignature.Size = new System.Drawing.Size(155, 17);
            this.checkBoxSignature.TabIndex = 13;
            this.checkBoxSignature.Text = "Use Signature Workaround";
            this.toolTip1.SetToolTip(this.checkBoxSignature, "Fix signature problems for Off SMR emails\r\n(MAY HAVE BUGS)");
            this.checkBoxSignature.UseVisualStyleBackColor = true;
            this.checkBoxSignature.CheckedChanged += new System.EventHandler(this.checkBoxSignature_CheckedChanged);
            // 
            // linkLabelSignature
            // 
            this.linkLabelSignature.AutoSize = true;
            this.linkLabelSignature.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.linkLabelSignature.Location = new System.Drawing.Point(375, 157);
            this.linkLabelSignature.Name = "linkLabelSignature";
            this.linkLabelSignature.Size = new System.Drawing.Size(13, 13);
            this.linkLabelSignature.TabIndex = 14;
            this.linkLabelSignature.TabStop = true;
            this.linkLabelSignature.Text = "?";
            this.toolTip1.SetToolTip(this.linkLabelSignature, "Pull up an explanation of why there needs to be a \"signature workaround\"");
            this.linkLabelSignature.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSignature_LinkClicked);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 228);
            this.Controls.Add(this.linkLabelSignature);
            this.Controls.Add(this.checkBoxSignature);
            this.Controls.Add(this.buttonVariables);
            this.Controls.Add(this.labelOffSMR);
            this.Controls.Add(this.checkBoxAutoUpdates);
            this.Controls.Add(this.linkLabelUpdate);
            this.Controls.Add(this.textBoxEdit);
            this.Controls.Add(this.linkLabelGitHub);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxMapLocation);
            this.Controls.Add(this.labelMapLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelMapLocation;
        private System.Windows.Forms.TextBox textBoxMapLocation;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
        private System.Windows.Forms.RichTextBox textBoxEdit;
        private System.Windows.Forms.LinkLabel linkLabelUpdate;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdates;
        private System.Windows.Forms.Label labelOffSMR;
        private System.Windows.Forms.Button buttonVariables;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxSignature;
        private System.Windows.Forms.LinkLabel linkLabelSignature;
    }
}