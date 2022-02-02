using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Console.WriteLine("!!!!!!!!!!!!!!");
            
            setCUCodeBox();
            setFileTypeBox();
            run = 1;
            updateFileDGV();
            Console.WriteLine("run =" + run.ToString());

        }
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
                query = $"select * from files where fileName LIKE '%{searchText.Text}%' and fileDir LIKE '%{cuSelectionBox.SelectedItem.ToString()}%'";
            }
            else if (fileTypeSelection.SelectedItem.ToString() == "Files Only")
            {
                query = $"select * from files where fileName LIKE '%{searchText.Text}%' and fileDir LIKE '%{cuSelectionBox.SelectedItem.ToString()}%' and fileType != 'folder'";
            }
            else
            {
                query = $"select * from files where fileName LIKE '%{searchText.Text}%' and fileDir LIKE '%{cuSelectionBox.SelectedItem.ToString()}%' and fileType = '{fileTypeSelection.SelectedItem.ToString()}'";
            }
            query += " order by fileScannedAt desc";
            return query;
        }

        private void setCUCodeBox() 
        {
            string[] list = { "CCCU", "ECCU", "EXCU", "MYCU", "TRYCU" };
            foreach (string item in list) cuSelectionBox.Items.Add(item);
            cuSelectionBox.SelectedIndex = 0;
        }

        private void setFileTypeBox() 
        {
            string[] list = { "ALL", "Files Only", "folder","txt", "log", "xml", "ach", "dat", "config", "ini", "html", "htm", "css", "js", "bat", "cif", "cs", "resx", "md", "csv" };
            foreach (string item in list)  fileTypeSelection.Items.Add(item);
            fileTypeSelection.SelectedIndex = 0;
        }

        private void SetupBaseDGVFormat(DataGridView dgv)
        {

            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSeaGreen;
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
           // dgv.ReadOnly = true;
            //dgv.AutoResizeColumns();
           // dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.RowHeadersVisible = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToDeleteRows = false;
            dgv.Columns[1].Resizable = DataGridViewTriState.True;
            dgv.Columns[2].Resizable = DataGridViewTriState.True;


        }

       
        private void displayData() { 
        
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
            var fID = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[0].Value;
            string fileName = fileDGV.Rows[fileDGV.CurrentRow.Index].Cells[1].Value.ToString();
            string path = dirBox.Text;

            //check if file exists @ given path
            //if exists,
                //verify on sql server
                    //push old
                    //replace in dir
            //else open write & close
            //push data to database to record the change

            DirSelect.Text = "Select Directory";

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
