using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class SQLServerDataSecuricor
    {
        private SqlConnection sqlConnection;

        string sql;
        int dataCount;

        public SQLServerDataSecuricor()
        {
            Initializer();
        }

        private void Initializer()
        {
            SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();
            Builder.DataSource = "AUPA-EAI\\SQLEXPRESSINV";
            Builder.InitialCatalog = "EI_intermediate";
            Builder.UserID = "sa";
            Builder.Password = "#Aupa=234";
            String sqlConnectionString = Builder.ConnectionString;
            sqlConnection = new SqlConnection(sqlConnectionString);
        }
        private bool OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 53:
                        Console.WriteLine("40 - 無法開啟至 SQL Server 的連接");
                        break;
                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                //MessageBox.Show(ex.Message);
                Console.WriteLine("MySqlException :" + ex.Message);
                return false;
            }
        }

        public int SelectEi_OmeRowCounts(string Key1)
        {
            OpenConnection();

            dataCount = 0;
            try
            {
                sql = "SELECT COUNT(ei_ome01) FROM ei_ome_file " +
                " WHERE ei_ome01='" + Key1 + "'";
                
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sql;
                //sqlCommand.Parameters.AddWithValue("@val01", Key1);


                dataCount = Convert.ToInt16(sqlCommand.ExecuteScalar());
                if (dataCount == -1)
                {
                    dataCount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLServer Data Secure Error : " + ex.Message);
                sqlConnection.Close();
            }
            finally
            {
                sqlConnection.Close();
            }
            return dataCount;
        }

    }
}
