
namespace DirScanner
{
    partial class Launch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launch));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.name = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.name)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(1, 161);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(482, 10);
            this.progressBar.TabIndex = 0;
            // 
            // name
            // 
            this.name.Image = ((System.Drawing.Image)(resources.GetObject("name.Image")));
            this.name.Location = new System.Drawing.Point(131, 45);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(225, 60);
            this.name.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.name.TabIndex = 1;
            this.name.TabStop = false;
            // 
            // Launch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(484, 172);
            this.Controls.Add(this.name);
            this.Controls.Add(this.progressBar);
            this.Name = "Launch";
            this.Text = "Launch";
            this.Load += new System.EventHandler(this.Launch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.name)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.PictureBox name;
    }
}