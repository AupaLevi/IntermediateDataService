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
        public int DeleteOmeRows(int year, int month)
        {
            sql = "";
            ProjectStringPool stringPool = new ProjectStringPool();
            sql = stringPool.getDeleteOmeDataSQL();

            CommonUntil commonUntil = new CommonUntil();
            //int year = commonUntil.getYear();
            //int month = commonUntil.getMonth() - 1;
            try
            {
                OpenConnection();

                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@val01", year);
                sqlCommand.Parameters.AddWithValue("@val02", month);

                this.deletedRows = sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                PostalService postalService = new PostalService();
                postalService.SendMail("levi.huang@aupa.com.tw", "Intermediate Data Copier Alert", ex.Message);
                this.deletedRows = 0;
            }
            finally
            {
                CloseConnection();
            }

            return this.deletedRows;
        }

        public List<OraEi_OmeObject> InsertEi_OmeSQLServer(List<OraEi_OmeObject> oraEi_Omes)
        {
            sql = "";
            ProjectStringPool stringPool = new ProjectStringPool();
            List<OraEi_OmeObject> insertedEi_OmeObjects = new List<OraEi_OmeObject>();
            sql = stringPool.getInsSQLServerEi_OmeSQL();

            actionResult = "SUCCESS";
            OpenConnection();
            if (oraEi_Omes.Count > 0)
            {
                foreach (OraEi_OmeObject ei_ome in oraEi_Omes)
                {
                    try
                    {
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = sql;

                        sqlCommand.Parameters.AddWithValue("@val01", ei_ome.Ei_ome01);
                        sqlCommand.Parameters.AddWithValue("@val02", ei_ome.Ei_ome02);
                        sqlCommand.Parameters.AddWithValue("@val03", ei_ome.Ei_ome04);
                        sqlCommand.Parameters.AddWithValue("@val04", ei_ome.Ei_ome05);
                        sqlCommand.Parameters.AddWithValue("@val05", ei_ome.Ei_ome06);
                        sqlCommand.Parameters.AddWithValue("@val06", ei_ome.Ei_ome07);
                        sqlCommand.Parameters.AddWithValue("@val07", ei_ome.Ei_ome08);
                        sqlCommand.Parameters.AddWithValue("@val08", ei_ome.Ei_ome09);
                        sqlCommand.Parameters.AddWithValue("@val09", ei_ome.Ei_ome10);
                        sqlCommand.Parameters.AddWithValue("@val010", ei_ome.Ei_ome11);
                        sqlCommand.Parameters.AddWithValue("@val011", ei_ome.Ei_ome12);
                        sqlCommand.Parameters.AddWithValue("@val012", ei_ome.Ei_ome13);
                        /*
                        sqlCommand.Parameters.AddWithValue("@val13", ei_ome.Ei_ome14);
                        sqlCommand.Parameters.AddWithValue("@val14", ei_ome.Ei_ome15);
                        sqlCommand.Parameters.AddWithValue("@val15", ei_ome.Ei_omevoid);
                        sqlCommand.Parameters.AddWithValue("@val16", ei_ome.Ei_omevoidu);
                        sqlCommand.Parameters.AddWithValue("@val17", ei_ome.Ei_omevoidd);
                        sqlCommand.Parameters.AddWithValue("@val18", ei_ome.Ei_omevoidm);
                        sqlCommand.Parameters.AddWithValue("@val19", ei_ome.Tc_omevoids);
                        sqlCommand.Parameters.AddWithValue("@val20", ei_ome.Ei_omecncl);
                        sqlCommand.Parameters.AddWithValue("@val21", ei_ome.Ei_omecnclu);
                        sqlCommand.Parameters.AddWithValue("@val22", ei_ome.Ei_omecncld);
                        sqlCommand.Parameters.AddWithValue("@val23", ei_ome.Ei_omecnclm);
                        sqlCommand.Parameters.AddWithValue("@val24", ei_ome.Tc_omecncls);
                        */
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Write("SQLServer Ins Ei_Ome Exception : " + ex.Message);
                        actionResult = "FAIL";
                        break;
                    }
                    finally
                    {
                        if (actionResult == "SUCCESS")
                        {
                            insertedEi_OmeObjects.Add(ei_ome);
                        }
                    }
                }
            }
            return insertedEi_OmeObjects;
        }
    }
}
