using System;

using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
namespace DirScanner
{
    public partial class GUI : Form
    {
        private BindingSource DisplayList { get; set; }
        int run = 0; // stops the index changed listeners from kicking off on the first run

        public GUI()
        {
            InitializeComponent();
            this.Text = "DirectoryScanner v2.0";    
            setCUCodeBox();
            setFileTypeBox();

            //now that we have index values to use for thefiletype and CUcode boxes,
            //we can let the program run as normal for listener events
            run = 1;

            //update the DGV with the query data filters.
            updateFileDGV();
            SetupBaseDGVFormat(fileDGV);
        }
        //---------------------------------------------------------------------------//
        // DataGridView update / query functions
        //---------------------------------------------------------------------------//
        private void updateFileDGV()
        {
            //grab the correct query by conditions, then update the DGV
            string query = getQueryFilesByTypeCUandSearch();
            setDataTableDGV(query, fileDGV);
        }

        //ALL files -- folders/files with any ext.
        //Files only -- excludes folders
        //otherwise, try to use the filename search, cuSelection box, and the fileType conditions.
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
            //all queries order by fileScannedAt most recent.
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
            dgv.ReadOnly = true;
            
            dgv.RowHeadersVisible = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToOrderColumns = false;
            //dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToDeleteRows = false;

            adjustColWidth(dgv, 0, 50); //fileId
            adjustColWidth(dgv, 1, 100);//fileName
            adjustColWidth(dgv, 2, 100);//fileDir
            adjustColWidth(dgv, 3, 50);//fileType

            adjustColWidth(dgv, 4, 130);//lastMod
            adjustColWidth(dgv, 5, 130);//fileCreated
            adjustColWidth(dgv, 6, 130);//fileScannedAt
            adjustColWidth(dgv, 7, 60);//fileSize

            dgv.Columns[4].DefaultCellStyle.Format = "MM-dd-yyyy hh:mm:ss tt";
            dgv.Columns[5].DefaultCellStyle.Format = "MM-dd-yyyy hh:mm:ss tt";
            ChangeDGVTimeToLocal(dgv, 6);
        }
        private void adjustColWidth(DataGridView dgv, int col, int width) 
        {
            dgv.Columns[col].MinimumWidth = width;
            dgv.Columns[col].Width = width;
            dgv.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }
        private void setDataTableDGV( string query, DataGridView dgv) 
        {
            connection c = new connection();
            DisplayList = new BindingSource();

            //always clear the datatable
            c.dt = new DataTable();   
            //fetch the datatable and add to the DGV
            c.DisplayDBtoDGV(c.Conn, query);

            //if it found data, display it formatted
            if (c.dt.Rows.Count >= 1)
            {
                DisplayList = new BindingSource();
                DisplayList.DataSource = c.dt;
                dgv.AutoGenerateColumns = true;
                dgv.DataSource = DisplayList;

                            
            }
            else {
                //otherwise show a blank table on the DGV
                dgv.DataSource = new DataTable();             
            }
        }
//---------------------------------------------------------------------------//
//returns a string of the UTC converted to local time
//---------------------------------------------------------------------------//
        public String ConvertUTCDateTimeToLocal(string DT)
        {
            //given a string, return the local version of it.
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
                //for each column specified in the list, change the time to local
                foreach (int col in columns)
                {
                    //Modify each row's value from UTC to local for display only
                    dgv.Rows[i].Cells[col].Value = ConvertUTCDateTimeToLocal(dgv.Rows[i].Cells[col].Value.ToString());
                }
            }

            //format the column dataTime for each column in the list.
            foreach (int col in columns)
            {
                dgv.Columns[col].DefaultCellStyle.Format = "MM-dd-yyyy hh:mm:ss tt";
            }
        }

//---------------------------------------------------------------------------//
//Change Events
//avoid running these the first run so we can populate the box selection for CU/FileType indexes.
//---------------------------------------------------------------------------//
        private void fileDGV_SelectionChanged(object sender, EventArgs e)
        {
            updateFileText();
        }
        //any changes to index or text will update the datagridview query and produce a new results table
        private void cuSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (run != 0)   searchFilesForInput();     
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
//Button Click Events
//---------------------------------------------------------------------------//
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
        private void DirSelect_Click(object sender, EventArgs e)
        {
            if (DirSelect.Text == "Select Directory") dirSelectDialog();
            else saveFileToDirectory();
        }
        private void addCode_Click(object sender, EventArgs e)
        {
            string path = dirBox.Text;
            if (path != "") updateCodeBox();

        }
        private void scanDirBt_Click(object sender, EventArgs e)
        {
            //check if the directory box is empty first
            if (dirBox.Text != "")
            {
                //set our path, add it to the selection options for queries.             
                updateCodeBox();
                //set up the Process call to run the python script and do the search at this location
                RunPythonScanProcess(dirBox.Text, "scanDir");
            }
            else { ShowMsgBox("Directory Selection cannot be empty."); }
        }
        //---------------------------------------------------------------------------//
        private void dirBox_Click(object sender, EventArgs e)
        {
            dirSelectDialog();
        }
//---------------------------------------------------------------------------//
//  
//---------------------------------------------------------------------------//


        private int getSelectedFileID()
        {
            int fID = Int32.Parse(fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[0].Value.ToString());
            return fID;
        }
        private string getSelectedFileName()
        {
            string fName = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[1].Value.ToString();
            return fName;
        }
        private string getSelectedFileDir()
        {
            string fDir = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[2].Value.ToString();
            return fDir;
        }

        //---------------------------------------------------------------------------//
        //Event Helper Functions
        //---------------------------------------------------------------------------//
        public void searchFilesForInput()
        {
            if (run != 0)
            {
                string query = getQueryFilesByTypeCUandSearch();
                setDataTableDGV(query, fileDGV);
            }
        }

        private void updateFileText()
        {           
            if (fileDGV.Rows.Count >= 1)
            {
                connection c = new connection();

                int fID = getSelectedFileID();

                string query = $"select fileTxt from filecontent where fileId = {fID}";
                 
                fileTextBox.Text = (fileTextBox.Text == "") ? "Unable to read File." : c.GetQueryTextField(c.Conn, query);
                fileNameLabel.Text = getSelectedFileName();
                c.Conn.Close();
            }
        }
        private void updateCodeBox()
        {
            string path = dirBox.Text;
            Console.WriteLine($"Evaluating: {cuSelectionBox.Items.Contains(path.ToString())}");

            if (cuSelectionBox.Items.Contains(path.ToString()) == false)
            {
                cuSelectionBox.Items.Add(path);
                cuSelectionBox.SelectedIndex = cuSelectionBox.Items.Count - 1;
            }
        }
//---------------------------------------------------------------------------//
//  
//---------------------------------------------------------------------------//
        private void saveFileToDirectory() 
        {
            DialogResult dr = MessageBox.Show($"Save this version of {fileNameLabel.Text} to {dirBox.Text}?", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes && dirBox.Text != "")
            {
                int fID         = getSelectedFileID();
                string fileName = getSelectedFileName();
                string path     = dirBox.Text;

                fileInteract();

                if (path != "") 
                updateCodeBox();

                DirSelect.Text = "Select Directory";
                dirBox.Text = "";
            }
        }
        private void ArchiveFileIn(string dir) 
        {
            RunPythonScanProcess(dir, "scanFile");
        }

        private void writeFile() 
        {
            string path = dirBox.Text + fileNameLabel.Text;
            try
            {
                //create the new one / overwrite it.
                File.WriteAllText(path, fileTextBox.Text);              
            }
            catch (UnauthorizedAccessException ex)
            { Console.WriteLine($"Unable to write file {fileNameLabel.Text} at {path}"); }
        }
//---------------------------------------------------------------------------//
//  
//---------------------------------------------------------------------------//       
        private void fileInteract() {
            //path of file as chosen by user directory input and selected file at runtime
            string path = dirBox.Text + fileNameLabel.Text;

            //if the file exists at this path
            if (File.Exists(path))
            {      
                //archive the old one to the DB
                ArchiveFileIn(getSelectedFileDir());
                writeFile();
                ArchiveFileIn(dirBox.Text);
            }
            //If it doesn't exist, Create the file at the selected directory.
            else
            {                           
                    writeFile();
                    //then, scan it to the database.
                    ArchiveFileIn(dirBox.Text);               
            }
            //set the directory scan box back to empty.
            dirBox.Text = "";
        }
//---------------------------------------------------------------------------//
//  
//---------------------------------------------------------------------------//

        //allows the user to select and standardize the path directory format
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
            //after the selection, switch the button back so we can save the file here.
            DirSelect.Text = "Save File";
        }
        //---------------------------------------------------------------------------//
        //  
        //---------------------------------------------------------------------------//

        //given a string, display it within a MessageBox component
        public void ShowMsgBox(string msg)
        {
            System.Windows.Forms.MessageBox.Show(msg);
        }

        private void RunPythonScanProcess(string dir, string mode) 
        {
            string progPath = @"C:\Python\python.exe";
            string args = string.Format(@"C:\Python\directoryScraper.py {0} {1} {2}", dir, fileNameLabel.Text, mode);

            RunProcess(progPath, args); //scan the directory.
        }
        //if the user wants to clicks the button to scan a directory
       

        private void RunProcess(string progPath, string args)
        {
            Console.WriteLine("Running process!");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(progPath, args);

            p.StartInfo.UseShellExecute = true;
            //p.StartInfo.RedirectStandardOutput = true;
            Process.Start(p.StartInfo);

            try { p.WaitForExit(100); }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Process Failing on " + ex.Message.ToString());
            }
        }
//---------------------------------------------------------------------------//
//  
//---------------------------------------------------------------------------//






        /*        
        public void QueryByParam()
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
