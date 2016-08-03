namespace SalesMap
{
    partial class Feedback
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
            this.buttonBug = new System.Windows.Forms.Button();
            this.buttonFeature = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBug
            // 
            this.buttonBug.Location = new System.Drawing.Point(12, 12);
            this.buttonBug.Name = "buttonBug";
            this.buttonBug.Size = new System.Drawing.Size(157, 23);
            this.buttonBug.TabIndex = 0;
            this.buttonBug.Text = "Submit a Bug Report";
            this.buttonBug.UseVisualStyleBackColor = true;
            // 
            // buttonFeature
            // 
            this.buttonFeature.Location = new System.Drawing.Point(12, 41);
            this.buttonFeature.Name = "buttonFeature";
            this.buttonFeature.Size = new System.Drawing.Size(157, 23);
            this.buttonFeature.TabIndex = 1;
            this.buttonFeature.Text = "Submit a Feature Request";
            this.buttonFeature.UseVisualStyleBackColor = true;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Location = new System.Drawing.Point(12, 70);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(157, 23);
            this.buttonFeedback.TabIndex = 2;
            this.buttonFeedback.Text = "General Feedback";
            this.buttonFeedback.UseVisualStyleBackColor = true;
            // 
            // Feedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 107);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.buttonFeature);
            this.Controls.Add(this.buttonBug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Feedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Feedback";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBug;
        private System.Windows.Forms.Button buttonFeature;
        private System.Windows.Forms.Button buttonFeedback;
    }
}