namespace SalesMap
{
    partial class SalesMapSearch
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelState = new System.Windows.Forms.Label();
            this.labelOR = new System.Windows.Forms.Label();
            this.labelRepresentative = new System.Windows.Forms.Label();
            this.pictureBackground = new System.Windows.Forms.PictureBox();
            this.comboBoxState = new System.Windows.Forms.ComboBox();
            this.comboBoxRepresentative = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelRepResult = new System.Windows.Forms.Label();
            this.labelRegionResult = new System.Windows.Forms.Label();
            this.labelContactResult = new System.Windows.Forms.Label();
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.labelContactResult2 = new System.Windows.Forms.Label();
            this.labelRepResult2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(9, 260);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 247);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labelState.Location = new System.Drawing.Point(21, 29);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(82, 13);
            this.labelState.TabIndex = 3;
            this.labelState.Text = "State/Province:";
            // 
            // labelOR
            // 
            this.labelOR.AutoSize = true;
            this.labelOR.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labelOR.Location = new System.Drawing.Point(77, 49);
            this.labelOR.Name = "labelOR";
            this.labelOR.Size = new System.Drawing.Size(41, 13);
            this.labelOR.TabIndex = 5;
            this.labelOR.Text = "-- OR --";
            // 
            // labelRepresentative
            // 
            this.labelRepresentative.AutoSize = true;
            this.labelRepresentative.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.labelRepresentative.Location = new System.Drawing.Point(21, 71);
            this.labelRepresentative.Name = "labelRepresentative";
            this.labelRepresentative.Size = new System.Drawing.Size(90, 13);
            this.labelRepresentative.TabIndex = 6;
            this.labelRepresentative.Text = "Sales Rep Name:";
            // 
            // pictureBackground
            // 
            this.pictureBackground.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pictureBackground.Location = new System.Drawing.Point(12, 12);
            this.pictureBackground.Name = "pictureBackground";
            this.pictureBackground.Size = new System.Drawing.Size(263, 95);
            this.pictureBackground.TabIndex = 8;
            this.pictureBackground.TabStop = false;
            // 
            // comboBoxState
            // 
            this.comboBoxState.FormattingEnabled = true;
            this.comboBoxState.Location = new System.Drawing.Point(109, 26);
            this.comboBoxState.Name = "comboBoxState";
            this.comboBoxState.Size = new System.Drawing.Size(151, 21);
            this.comboBoxState.TabIndex = 11;
            this.comboBoxState.SelectedIndexChanged += new System.EventHandler(this.comboBoxState_SelectedIndexChanged);
            // 
            // comboBoxRepresentative
            // 
            this.comboBoxRepresentative.FormattingEnabled = true;
            this.comboBoxRepresentative.Location = new System.Drawing.Point(109, 68);
            this.comboBoxRepresentative.Name = "comboBoxRepresentative";
            this.comboBoxRepresentative.Size = new System.Drawing.Size(151, 21);
            this.comboBoxRepresentative.TabIndex = 12;
            this.comboBoxRepresentative.SelectedIndexChanged += new System.EventHandler(this.comboBoxRepresentative_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SalesMap.Properties.Resources.settings;
            this.pictureBox2.Location = new System.Drawing.Point(320, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 25);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // labelRepResult
            // 
            this.labelRepResult.AutoSize = true;
            this.labelRepResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRepResult.Location = new System.Drawing.Point(12, 121);
            this.labelRepResult.Name = "labelRepResult";
            this.labelRepResult.Size = new System.Drawing.Size(75, 16);
            this.labelRepResult.TabIndex = 14;
            this.labelRepResult.Text = "Sales Rep:";
            // 
            // labelRegionResult
            // 
            this.labelRegionResult.AutoSize = true;
            this.labelRegionResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRegionResult.Location = new System.Drawing.Point(12, 236);
            this.labelRegionResult.Name = "labelRegionResult";
            this.labelRegionResult.Size = new System.Drawing.Size(55, 16);
            this.labelRegionResult.TabIndex = 15;
            this.labelRegionResult.Text = "Region:";
            // 
            // labelContactResult
            // 
            this.labelContactResult.AutoSize = true;
            this.labelContactResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContactResult.Location = new System.Drawing.Point(21, 137);
            this.labelContactResult.Name = "labelContactResult";
            this.labelContactResult.Size = new System.Drawing.Size(56, 16);
            this.labelContactResult.TabIndex = 16;
            this.labelContactResult.Text = "Contact:";
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Image = global::SalesMap.Properties.Resources.Map;
            this.pictureBoxMap.Location = new System.Drawing.Point(286, 0);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMap.TabIndex = 17;
            this.pictureBoxMap.TabStop = false;
            this.pictureBoxMap.Click += new System.EventHandler(this.pictureBoxMap_Click);
            // 
            // labelContactResult2
            // 
            this.labelContactResult2.AutoSize = true;
            this.labelContactResult2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContactResult2.Location = new System.Drawing.Point(21, 178);
            this.labelContactResult2.Name = "labelContactResult2";
            this.labelContactResult2.Size = new System.Drawing.Size(56, 16);
            this.labelContactResult2.TabIndex = 20;
            this.labelContactResult2.Text = "Contact:";
            // 
            // labelRepResult2
            // 
            this.labelRepResult2.AutoSize = true;
            this.labelRepResult2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRepResult2.Location = new System.Drawing.Point(12, 162);
            this.labelRepResult2.Name = "labelRepResult2";
            this.labelRepResult2.Size = new System.Drawing.Size(100, 16);
            this.labelRepResult2.TabIndex = 18;
            this.labelRepResult2.Text = "2nd Sales Rep:";
            // 
            // SalesMapSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 515);
            this.Controls.Add(this.labelContactResult2);
            this.Controls.Add(this.labelRepResult2);
            this.Controls.Add(this.pictureBoxMap);
            this.Controls.Add(this.labelContactResult);
            this.Controls.Add(this.labelRegionResult);
            this.Controls.Add(this.labelRepResult);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.comboBoxRepresentative);
            this.Controls.Add(this.comboBoxState);
            this.Controls.Add(this.labelRepresentative);
            this.Controls.Add(this.labelOR);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBackground);
            this.Name = "SalesMapSearch";
            this.Text = "Sales Map Search";
            this.Load += new System.EventHandler(this.SalesMapSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label labelOR;
        private System.Windows.Forms.Label labelRepresentative;
        private System.Windows.Forms.PictureBox pictureBackground;
        private System.Windows.Forms.ComboBox comboBoxState;
        private System.Windows.Forms.ComboBox comboBoxRepresentative;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelRepResult;
        private System.Windows.Forms.Label labelRegionResult;
        private System.Windows.Forms.Label labelContactResult;
        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.Label labelContactResult2;
        private System.Windows.Forms.Label labelRepResult2;
    }
}

