
namespace DirScanner
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.fileTextBox = new System.Windows.Forms.RichTextBox();
            this.cuSelectionBox = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fileDGV = new System.Windows.Forms.DataGridView();
            this.DirSelect = new System.Windows.Forms.Button();
            this.dirBox = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.searchText = new System.Windows.Forms.TextBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.fileTypeSelection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.cancelBt = new System.Windows.Forms.PictureBox();
            this.scanDirBt = new System.Windows.Forms.Button();
            this.addCode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cancelBt)).BeginInit();
            this.SuspendLayout();
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(672, 80);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.ReadOnly = true;
            this.fileTextBox.Size = new System.Drawing.Size(488, 486);
            this.fileTextBox.TabIndex = 0;
            this.fileTextBox.Text = "";
            // 
            // cuSelectionBox
            // 
            this.cuSelectionBox.FormattingEnabled = true;
            this.cuSelectionBox.Location = new System.Drawing.Point(481, 53);
            this.cuSelectionBox.Name = "cuSelectionBox";
            this.cuSelectionBox.Size = new System.Drawing.Size(268, 21);
            this.cuSelectionBox.TabIndex = 1;
            this.cuSelectionBox.SelectedIndexChanged += new System.EventHandler(this.cuSelectionBox_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 46);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // fileDGV
            // 
            this.fileDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.fileDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fileDGV.Location = new System.Drawing.Point(0, 79);
            this.fileDGV.Name = "fileDGV";
            this.fileDGV.ReadOnly = true;
            this.fileDGV.Size = new System.Drawing.Size(672, 489);
            this.fileDGV.TabIndex = 3;
            this.fileDGV.SelectionChanged += new System.EventHandler(this.fileDGV_SelectionChanged);
            // 
            // DirSelect
            // 
            this.DirSelect.Location = new System.Drawing.Point(69, 53);
            this.DirSelect.Name = "DirSelect";
            this.DirSelect.Size = new System.Drawing.Size(101, 20);
            this.DirSelect.TabIndex = 5;
            this.DirSelect.Text = "Select Directory";
            this.DirSelect.UseVisualStyleBackColor = true;
            this.DirSelect.Click += new System.EventHandler(this.DirSelect_Click);
            // 
            // dirBox
            // 
            this.dirBox.Location = new System.Drawing.Point(176, 53);
            this.dirBox.Name = "dirBox";
            this.dirBox.ReadOnly = true;
            this.dirBox.Size = new System.Drawing.Size(268, 20);
            this.dirBox.TabIndex = 8;
            this.dirBox.Click += new System.EventHandler(this.dirBox_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // searchText
            // 
            this.searchText.Location = new System.Drawing.Point(154, 586);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(193, 20);
            this.searchText.TabIndex = 9;
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.fileNameLabel.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileNameLabel.Location = new System.Drawing.Point(796, 54);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(103, 23);
            this.fileNameLabel.TabIndex = 10;
            this.fileNameLabel.Text = "File Name";
            // 
            // fileTypeSelection
            // 
            this.fileTypeSelection.FormattingEnabled = true;
            this.fileTypeSelection.Location = new System.Drawing.Point(481, 12);
            this.fileTypeSelection.Name = "fileTypeSelection";
            this.fileTypeSelection.Size = new System.Drawing.Size(121, 21);
            this.fileTypeSelection.TabIndex = 11;
            this.fileTypeSelection.SelectedIndexChanged += new System.EventHandler(this.fileTypeSelection_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.GhostWhite;
            this.label1.Location = new System.Drawing.Point(12, 589);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Enter a file name to search:";
            // 
            // cancelBt
            // 
            this.cancelBt.BackColor = System.Drawing.Color.Transparent;
            this.cancelBt.Image = ((System.Drawing.Image)(resources.GetObject("cancelBt.Image")));
            this.cancelBt.Location = new System.Drawing.Point(450, 51);
            this.cancelBt.Name = "cancelBt";
            this.cancelBt.Size = new System.Drawing.Size(25, 24);
            this.cancelBt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.cancelBt.TabIndex = 13;
            this.cancelBt.TabStop = false;
            this.cancelBt.Click += new System.EventHandler(this.cancelBt_Click);
            // 
            // scanDirBt
            // 
            this.scanDirBt.Location = new System.Drawing.Point(310, 13);
            this.scanDirBt.Name = "scanDirBt";
            this.scanDirBt.Size = new System.Drawing.Size(134, 22);
            this.scanDirBt.TabIndex = 14;
            this.scanDirBt.Text = "Scan for Changes";
            this.scanDirBt.UseVisualStyleBackColor = true;
            this.scanDirBt.Click += new System.EventHandler(this.scanDirBt_Click);
            // 
            // addCode
            // 
            this.addCode.Location = new System.Drawing.Point(176, 12);
            this.addCode.Name = "addCode";
            this.addCode.Size = new System.Drawing.Size(128, 24);
            this.addCode.TabIndex = 15;
            this.addCode.Text = "Add to Directory List";
            this.addCode.UseVisualStyleBackColor = true;
            this.addCode.Click += new System.EventHandler(this.addCode_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1160, 623);
            this.Controls.Add(this.addCode);
            this.Controls.Add(this.scanDirBt);
            this.Controls.Add(this.cancelBt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileTypeSelection);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.dirBox);
            this.Controls.Add(this.DirSelect);
            this.Controls.Add(this.fileDGV);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cuSelectionBox);
            this.Controls.Add(this.fileTextBox);
            this.Name = "GUI";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cancelBt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox fileTextBox;
        private System.Windows.Forms.ComboBox cuSelectionBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView fileDGV;
        private System.Windows.Forms.Button DirSelect;
        private System.Windows.Forms.TextBox dirBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.ComboBox fileTypeSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.PictureBox cancelBt;
        private System.Windows.Forms.Button scanDirBt;
        private System.Windows.Forms.Button addCode;
    }
}

