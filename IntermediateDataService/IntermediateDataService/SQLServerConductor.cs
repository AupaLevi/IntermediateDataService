using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class SQLServerConductor
    {
        private SqlConnection sqlConnection;
        private string actionResult;
        private int deletedRows;

        string sql;

        public SQLServerConductor()
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
                Console.Write("MySqlException :" + ex.Message);
                return false;
            }
        }

        public String InsertEi_OmeSQLServer(SqlEi_OmeObject sqlEi_Omes)
        {
            sql = "";
            ProjectStringPool stringPool = new ProjectStringPool();
            //List<OraEi_OmeObject> insertedEi_OmeObjects = new List<OraEi_OmeObject>();
            sql = stringPool.getInsSQLServerEi_OmeSQL();

            actionResult = "SUCCESS";
            OpenConnection();


            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sql;

                sqlCommand.Parameters.AddWithValue("@val01", sqlEi_Omes.Ei_ome01);
                sqlCommand.Parameters.AddWithValue("@val02", sqlEi_Omes.Ei_ome02);
                sqlCommand.Parameters.AddWithValue("@val03", sqlEi_Omes.Ei_ome04);
                sqlCommand.Parameters.AddWithValue("@val04", sqlEi_Omes.Ei_ome05);
                sqlCommand.Parameters.AddWithValue("@val05", sqlEi_Omes.Ei_ome06);
                sqlCommand.Parameters.AddWithValue("@val06", sqlEi_Omes.Ei_ome07);
                sqlCommand.Parameters.AddWithValue("@val07", sqlEi_Omes.Ei_ome08);
                sqlCommand.Parameters.AddWithValue("@val08", sqlEi_Omes.Ei_ome09);
                sqlCommand.Parameters.AddWithValue("@val09", sqlEi_Omes.Ei_ome10);
                sqlCommand.Parameters.AddWithValue("@val010", sqlEi_Omes.Ei_ome11);
                sqlCommand.Parameters.AddWithValue("@val011", sqlEi_Omes.Ei_ome12);
                sqlCommand.Parameters.AddWithValue("@val012", sqlEi_Omes.Ei_ome13);
                /*
                sqlCommand.Parameters.AddWithValue("@val13", sqlEi_Omes.Ei_ome14);
                sqlCommand.Parameters.AddWithValue("@val14", sqlEi_Omes.Ei_ome15);
                sqlCommand.Parameters.AddWithValue("@val15", sqlEi_Omes.Ei_omevoid);
                sqlCommand.Parameters.AddWithValue("@val16", sqlEi_Omes.Ei_omevoidu);
                sqlCommand.Parameters.AddWithValue("@val17", sqlEi_Omes.Ei_omevoidd);
                sqlCommand.Parameters.AddWithValue("@val18", sqlEi_Omes.Ei_omevoidm);
                sqlCommand.Parameters.AddWithValue("@val19", sqlEi_Omes.Tc_omevoids);
                sqlCommand.Parameters.AddWithValue("@val20", sqlEi_Omes.Ei_omecncl);
                sqlCommand.Parameters.AddWithValue("@val21", sqlEi_Omes.Ei_omecnclu);
                sqlCommand.Parameters.AddWithValue("@val22", sqlEi_Omes.Ei_omecncld);
                sqlCommand.Parameters.AddWithValue("@val23", sqlEi_Omes.Ei_omecnclm);
                sqlCommand.Parameters.AddWithValue("@val24", sqlEi_Omes.Tc_omecncls);
                */
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write("SQLServer Ins Ei_Ome Exception : " + ex.Message);
                actionResult = "FAIL";

            }
            finally
            {
                CloseConnection();
            }
            return actionResult;
        }

    }

}

