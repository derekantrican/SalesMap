namespace SalesMap
{
    partial class North_South
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
            this.buttonNorthRep = new System.Windows.Forms.Button();
            this.buttonSouthRep = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonNorthRep
            // 
            this.buttonNorthRep.Location = new System.Drawing.Point(12, 68);
            this.buttonNorthRep.Name = "buttonNorthRep";
            this.buttonNorthRep.Size = new System.Drawing.Size(121, 23);
            this.buttonNorthRep.TabIndex = 0;
            this.buttonNorthRep.Text = "North";
            this.buttonNorthRep.UseVisualStyleBackColor = true;
            this.buttonNorthRep.Click += new System.EventHandler(this.buttonNorthRep_Click);
            // 
            // buttonSouthRep
            // 
            this.buttonSouthRep.Location = new System.Drawing.Point(145, 68);
            this.buttonSouthRep.Name = "buttonSouthRep";
            this.buttonSouthRep.Size = new System.Drawing.Size(121, 23);
            this.buttonSouthRep.TabIndex = 1;
            this.buttonSouthRep.Text = "South";
            this.buttonSouthRep.UseVisualStyleBackColor = true;
            this.buttonSouthRep.Click += new System.EventHandler(this.buttonSouthRep_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(12, 9);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(256, 32);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "Please choose one of the representatives\r\nfor this region:";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // North_South
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 103);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttonSouthRep);
            this.Controls.Add(this.buttonNorthRep);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "North_South";
            this.ShowIcon = false;
            this.Text = "Choose a Sales Rep";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNorthRep;
        private System.Windows.Forms.Button buttonSouthRep;
        private System.Windows.Forms.Label labelMessage;
    }
}