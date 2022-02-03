﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace DirScanner
{
    public partial class GUI : Form
    {
        private BindingSource DisplayList { get; set; }
        int run = 0;
        public GUI()
        {
            InitializeComponent();
            this.Text = "DirectoryScanner v1.0";    
            setCUCodeBox();
            setFileTypeBox();
            run = 1;
            updateFileDGV();
        }
        //---------------------------------------------------------------------------//
        // DataGridView update / query functions
        //---------------------------------------------------------------------------//
        private void updateFileDGV()
        {
            string query = getQueryFilesByTypeCUandSearch();
            Console.WriteLine("query =" + query);
            setDataTableDGV(query, fileDGV);
        }

        private string getQueryFilesByTypeCUandSearch()
        {
            string query = "";
            if (fileTypeSelection.SelectedItem.ToString() == "ALL")
            {
                query = $"select * from files where fileName LIKE '%{searchText.Text}%' and fileDir LIKE '%{cuSelectionBox.SelectedItem.ToString().Replace('\\','%')}%'";
            }
            else if (fileTypeSelection.SelectedItem.ToString() == "Files Only")
            {
                query = $"select * from files where fileName LIKE '%{searchText.Text}%' and fileDir LIKE '%{cuSelectionBox.SelectedItem.ToString().Replace('\\', '%')}%' and fileType != 'folder'";
            }
            else
            {
                query = $"select * from files where fileName LIKE '%{searchText.Text}%' and fileDir LIKE '%{cuSelectionBox.SelectedItem.ToString().Replace('\\', '%')}%' and fileType = '{fileTypeSelection.SelectedItem.ToString()}'";
            }
            query += " order by fileScannedAt desc";
            return query;
        }
        //---------------------------------------------------------------------------//
        // Dropdown selection initialization functions
        //---------------------------------------------------------------------------//
        private void setCUCodeBox() 
        {
            string[] list = { "CCCU", "ECCU", "EXCU", "MYCU", "TRYCU" };
            foreach (string item in list) cuSelectionBox.Items.Add(item);
            cuSelectionBox.SelectedIndex = 0;
        }

        private void setFileTypeBox() 
        {
            string[] list = { "ALL", "Files Only", "folder","txt", "log", "xml", "ach", "dat", "config", "ini", "html", "htm", "css", "js", "bat", "cif", "cs", "resx", "md", "csv","py" };
            foreach (string item in list)  fileTypeSelection.Items.Add(item);
            fileTypeSelection.SelectedIndex = 0;
        }
        //---------------------------------------------------------------------------//
        // DataGridView functions
        //---------------------------------------------------------------------------//
        private void SetupBaseDGVFormat(DataGridView dgv)
        {

            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSeaGreen;
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            // dgv.ReadOnly = true;
            // dgv.AutoResizeColumns();
            // dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            dgv.RowHeadersVisible = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToOrderColumns = false;
            //dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToDeleteRows = false;
             
            // dgv.Columns[1].Resizable = DataGridViewTriState.True;
            // dgv.Columns[2].Resizable = DataGridViewTriState.True;
            dgv.Columns[1].Width = 50;
            dgv.Columns[2].Width = 50;
            ChangeDGVTimeToLocal(dgv, 6);
        }

        private void setDataTableDGV( string query, DataGridView dgv) 
        {
            connection c = new connection();
            DisplayList = new BindingSource();
            c.dt = new DataTable();       
            c.DisplayDBtoDGV(c.Conn, query);
            if (c.dt.Rows.Count >= 1)
            {
                DisplayList = new BindingSource();
                DisplayList.DataSource = c.dt;
                dgv.AutoGenerateColumns = true;
                dgv.DataSource = DisplayList;

                SetupBaseDGVFormat(dgv);//format the datagridview               
            }
            else {
                dgv.DataSource = new DataTable();             
            }
        }
        //---------------------------------------------------------------------------//
        //returns a string of the UTC converted to local time
        //---------------------------------------------------------------------------//
        public String ConvertUTCDateTimeToLocal(string DT)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            return DateTime.Parse(DT, ci).ToLocalTime().ToString();
        }
        //---------------------------------------------------------------------------//
        //given a DataGridView and columns, convert the displayed time to local
        //then, change each column to format the date as '2021-10-08 00:00:00 AM'
        //---------------------------------------------------------------------------//
        public void ChangeDGVTimeToLocal(DataGridView dgv, params int[] columns)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                foreach (int col in columns)
                {
                    //Modify each row's value from UTC to local for display only
                    dgv.Rows[i].Cells[col].Value = ConvertUTCDateTimeToLocal(dgv.Rows[i].Cells[col].Value.ToString());
                }
            }

            //format the column dataTime
            foreach (int col in columns)
            {
                dgv.Columns[col].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
            }
        }
        //---------------------------------------------------------------------------//
        //Button and Change Events
        //---------------------------------------------------------------------------//
        public void searchFilesForInput()
        {
            if (run != 0)
            {
                string query = getQueryFilesByTypeCUandSearch();
                setDataTableDGV(query, fileDGV);
            }
        }

        private void cuSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (run != 0) 
            {
                searchFilesForInput();
            }

        }
        private void fileDGV_SelectionChanged(object sender, EventArgs e)
        {
            updateFileText();
        }

        private void fileTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (run != 0)    updateFileDGV();
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            if (run != 0)   updateFileDGV();
        }

      


        //---------------------------------------------------------------------------//
        // File IO functions and python integrated functions
        //---------------------------------------------------------------------------//
        private void updateFileText()
        {           
            if (fileDGV.Rows.Count >= 1)
            {
                connection c = new connection();
               
                var fID = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[0].Value;

                string query = $"select fileTxt from filecontent where fileId = {fID}";
                 
                fileTextBox.Text = (fileTextBox.Text == "") ? "Unable to read File." : c.GetQueryTextField(c.Conn, query);
                fileNameLabel.Text = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[1].Value.ToString();
                c.Conn.Close();
            }
        }

        private void DirSelect_Click(object sender, EventArgs e)
        {
            if (DirSelect.Text == "Select Directory") dirSelectDialog();
            else saveFileToDirectory(); 
        }
        private void saveFileToDirectory() 
        {
            DialogResult dr = MessageBox.Show($"Save this version of {fileNameLabel.Text} to {dirBox.Text}?", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {        
            var fID = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[0].Value;
            string fileName = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[1].Value.ToString();
            string path = dirBox.Text;
            fileInteract();
            if(path != "")  cuSelectionBox.Items.Add(path);

            DirSelect.Text = "Select Directory";
            dirBox.Text = "";
            }
        }

        private void startFileScan(string dirPath) 
        {            
                string progPath = @"C:\Python\python.exe";  
                string args = string.Format(@"C:\Python\directoryScraper.py {0} {1} {2}", dirPath, fileNameLabel.Text, "scanFile");
                RunProcess(progPath,args); //scan the old one first for changes to archive it              
        }

        private void RunProcess(string progPath, string args)
        {               
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo(progPath, args);
            
                    p.StartInfo.UseShellExecute = true;
                    //p.StartInfo.RedirectStandardOutput = true;
                    Process.Start(p.StartInfo);

            try { p.WaitForExit(100000);}
            catch (InvalidOperationException ex)  {
            Console.WriteLine("Process Failing on " + ex.Message.ToString());
                
            } 
        }


        private void fileInteract() {
            //path of file as chosen by user directory input and selected file at runtime
            string path = dirBox.Text + fileNameLabel.Text;

            
            if (File.Exists(path))
            {            
                string dirPathNew = dirBox.Text;
                string dirPathOld = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[2].Value.ToString();

                //archive the old one
                startFileScan(dirPathOld);
                File.WriteAllText(path, fileTextBox.Text);
                //scan in the newly written one
                startFileScan(dirPathNew);

                //shows message if test file exists already
            }
            //Create the file if it doesn't exist
            else
            {
                File.WriteAllText(path, fileTextBox.Text);
                string dirPathNew = dirBox.Text;
                startFileScan(dirPathNew);
            }
            dirBox.Text = "";
        }
        private void dirSelectDialog() 
        {
            using (var folder = new FolderBrowserDialog())
            {
                DialogResult result = folder.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    dirBox.Text = folder.SelectedPath.ToString();
                    
                    int lastChar = dirBox.Text.Length-1;
                    if (dirBox.Text[lastChar].Equals("\\") == false)
                        dirBox.Text += "\\";                  
                }
            }
            DirSelect.Text = "Save File";
        }

        private void cancelBt_Click(object sender, EventArgs e)
        {
            if (DirSelect.Text == "Select Directory") 
            { }
            else
            {
                DirSelect.Text = "Select Directory";
                dirBox.Text = "";
            }
        }

        private void scanDirBt_Click(object sender, EventArgs e)
        {
                string path = dirBox.Text;
                if (path != "") cuSelectionBox.Items.Add(path);
                string progPath = @"C:\Python\python.exe";
                string args = string.Format(@"C:\Python\directoryScraper.py {0} {1} {2}", dirBox.Text, fileNameLabel.Text, "scanDir");
                RunProcess(progPath, args); //scan the odirectory.
                cuSelectionBox.SelectedIndex = cuSelectionBox.Items.Count-1;
        }

        private void searchDirText_TextChanged(object sender, EventArgs e)
        {

        }




        /*        
        public void Update()
        {
            connection c = new connection();

            long tempAddrId = 0;
            //save this addressId for later.
            int fileId = c.GetIntQuery(c.Conn, $"SELECT addressId FROM customer where fileId = @fileId;");


            //first, modify the address database based on their AddressId above
            string query =
            $"UPDATE files SET fileId = @fileId, "
            + $"where fileId = {fileId};";

            c.Cmd = new MySql.Data.MySqlClient.MySqlCommand(query, c.Conn);
            //Address table params / values
            c.Cmd.Parameters.AddWithValue("@fileId", fileId);
            

            c.SendParamQueryToDB(c.Conn, query, c.Cmd);
        }
         */


    }
}
