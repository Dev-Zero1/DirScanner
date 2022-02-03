using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.IO;
using System.ComponentModel;


namespace DirScanner
{
    public class connection
    {

        //---------------------------------------------------------------------------//
        // Start  - Class Member variables 
        //---------------------------------------------------------------------------//
        public MySqlConnection Conn { get; private set; }
        public String ConnStr { get; private set; }
        public String Query { get; private set; }

        public MySqlCommand Cmd { get; set; }
        public MySqlDataAdapter Adapter { get; private set; }

        public DataSet dataSet { get; private set; }
        public DataTable dt { get; set; }
        // public BindingList<object> Data { get; private set; }

        //---------------------------------------------------------------------------//
        // End      - Class Member variables 
        // Start    - Class constructors
        //---------------------------------------------------------------------------//
        public connection()
        {
            //  Data = new BindingList<object>();
            ConnStr = ConfigurationManager.ConnectionStrings["MyMySqlKey"].ConnectionString;

            try
            {
                Conn = new MySqlConnection(ConnStr);
                Console.WriteLine("Connection established successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Cannot open connection! ");
            }
            finally
            {
                //Console.WriteLine("Cannot open connection ! ");
                if (Conn.State == System.Data.ConnectionState.Open)
                {
                    //Conn.Close();
                }
            }
        }
        //---------------------------------------------------------------------------//
        // End      - Class constructors
        // Start    - Class DB interaction functions 
        //---------------------------------------------------------------------------//

        public void DisplayDBtoDGV(MySqlConnection connection, string cmd)
        {

            Adapter = new MySqlDataAdapter();
            Adapter.SelectCommand = new MySqlCommand(cmd, Conn);
            dt = new DataTable();
            Adapter.Fill(dt);

            //Close the connection 
            Conn.Close();
        }
        //---------------------------------------------------------------------------//
        //
        //---------------------------------------------------------------------------//
        public void SendQueryToDB(MySqlConnection connection, string cmd)
        {

            MySqlCommand cmdName = new MySqlCommand(cmd, connection);

            this.Conn.Open();
            cmdName.ExecuteNonQuery();

            //Close the connection 
            Conn.Close();
        }
        //---------------------------------------------------------------------------//
        //
        //---------------------------------------------------------------------------//
        public void SendParamQueryToDB(MySqlConnection connection, string query, MySqlCommand cmd)
        {
            this.Conn.Open();

            cmd.ExecuteNonQuery();
            Conn.Close();
        }
        //---------------------------------------------------------------------------//
        //Data queries for int results, list results, datetime, and string results.
        //---------------------------------------------------------------------------//
        public int GetIntQuery(MySqlConnection connection, string cmd)
        {
            int QueryResult = 0;
            Conn.Open();
            MySqlCommand cmdName = new MySqlCommand(cmd, connection);

            MySqlDataReader row = cmdName.ExecuteReader(CommandBehavior.CloseConnection);

            //while there is data coming in 
            while (row.Read())
            {
                //Look at each field in the row, add it to the list
                //The list will be used for a DataGridView
                for (int i = 0; i < row.FieldCount; i++)
                {
                    //Get which type of data it is for each field ran in this for loop
                    Type DT = row.GetFieldType(i);

                    //Translate the one field received
                    if ((DT == typeof(System.Int32) || DT == typeof(System.Int64)))
                    {
                        QueryResult = row.GetInt32(i);
                    }
                }
            }

            //Close the connection and reader
            Conn.Close();
            row.Close();
            return QueryResult;
        }
        //---------------------------------------------------------------------------//
        //
        //---------------------------------------------------------------------------//
        public BindingList<string> GetListOfQuery(MySqlConnection connection, string cmd)
        {
            BindingList<string> QueryResult = new BindingList<string>();

            Conn.Open();
            MySqlCommand cmdName = new MySqlCommand(cmd, connection);

            MySqlDataReader row = cmdName.ExecuteReader(CommandBehavior.CloseConnection);

            //while there is data coming in 
            while (row.Read())
            {
                //Look at each field in the row, add it to the list
                //The list will be used for a DataGridView
                for (int i = 0; i < row.FieldCount; i++)
                {
                    //Get which type of data it is for each field ran in this for loop
                    Type DT = row.GetFieldType(i);

                    //Translate all displayed DataGridView data into strings
                    if ((DT == typeof(System.Int32) || DT == typeof(System.Int64)))
                    {
                        QueryResult.Add(row.GetInt64(i).ToString());
                    }
                    else if (DT == typeof(System.String))
                    {
                        QueryResult.Add(row.GetString(i));
                    }
                    else if (DT == typeof(System.Double))
                    {
                        QueryResult.Add(row.GetDouble(i).ToString());
                    }
                    else if (DT == typeof(System.DateTime))
                    {
                        QueryResult.Add(row.GetDateTime(i).ToString());
                    }
                }
            }

            //Close the connection and reader
            Conn.Close();
            row.Close();

            //QueryResult is a List<String> which is used by a DataGridView
            return QueryResult;
        }
        //---------------------------------------------------------------------------//
        //
        //---------------------------------------------------------------------------//
        public string GetQueryTextField(MySqlConnection connection, string cmd)
        {
            string QueryResult = "";
            Conn.Open();
            MySqlCommand cmdName = new MySqlCommand(cmd, connection);
            MySqlDataReader row = cmdName.ExecuteReader(CommandBehavior.CloseConnection);

            //while there is data coming in 
            while (row.Read())
            {
                //Look at each field in the row, add it to the list
                //The list will be used for a DataGridView
                for (int i = 0; i < row.FieldCount; i++)
                {
                    //Get which type of data it is for each field ran in this for loop
                    Type DT = row.GetFieldType(i);

                    //Translate the one field received
                    if ((DT == typeof(System.Int32) || DT == typeof(System.Int64)))
                    {
                        QueryResult = row.GetInt64(i).ToString();
                    }
                    else if (DT == typeof(System.String))
                    {
                        QueryResult = row.GetString(i);
                    }
                    else if (DT == typeof(System.Double))
                    {
                        QueryResult = row.GetDouble(i).ToString();
                    }
                    else if (DT == typeof(System.DateTime))
                    {
                        QueryResult = row.GetDateTime(i).ToString();
                    }
                }
            }
            //Close the connection and reader
            Conn.Close();
            row.Close();

            //QueryResult is a List<String> which is used by a DataGridView
            return QueryResult;
        }
        //---------------------------------------------------------------------------//
        //
        //---------------------------------------------------------------------------//
        public DateTime GetQueryDateTimeField(MySqlConnection connection, string cmd)
        {
            DateTime QueryResult = new DateTime();
            Conn.Open();
            MySqlCommand cmdName = new MySqlCommand(cmd, connection);

            MySqlDataReader row = cmdName.ExecuteReader(CommandBehavior.CloseConnection);

            //while there is data coming in 
            while (row.Read())
            {
                //Look at each field in the row, add it to the list
                //The list will be used for a DataGridView
                for (int i = 0; i < row.FieldCount; i++)
                {
                    //Get which type of data it is for each field ran in this for loop
                    Type DT = row.GetFieldType(i);
                    if (DT == typeof(System.DateTime))
                    {
                        QueryResult = row.GetDateTime(i);
                    }
                }
            }

            //Close the connection and reader
            Conn.Close();
            row.Close();

            //QueryResult is a List<String> which is used by a DataGridView
            return QueryResult;
        }
        //---------------------------------------------------------------------------//
        //
        //---------------------------------------------------------------------------//
        public Boolean GetQueryBoolField(MySqlConnection connection, string cmd)
        {
            Boolean QueryResult = new Boolean();
            Conn.Open();
            MySqlCommand cmdName = new MySqlCommand(cmd, connection);

            MySqlDataReader row = cmdName.ExecuteReader(CommandBehavior.CloseConnection);

            //while there is data coming in 
            while (row.Read())
            {
                //Look at each field in the row, add it to the list
                //The list will be used for a DataGridView
                for (int i = 0; i < row.FieldCount; i++)
                {
                    //Get which type of data it is for each field ran in this for loop
                    Type DT = row.GetFieldType(i);
                    if (DT == typeof(System.Boolean))
                    {
                        QueryResult = row.GetBoolean(i);
                    }
                }
            }
            //Close the connection and reader
            Conn.Close();
            row.Close();

            //QueryResult is a List<String> which is used by a DataGridView
            return QueryResult;
        }
        //---------------------------------------------------------------------------//
        // End  - Class DB interaction functions
        //---------------------------------------------------------------------------//

    }//end Class - Connection
}//end Namespace - DesktopSchedulingApp_WGU
