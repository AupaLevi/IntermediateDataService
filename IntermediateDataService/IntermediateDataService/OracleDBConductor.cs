using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class OracleDBConductor
    {
        private OracleConnection connection;
        private ProjectStringPool projectStringPool = new ProjectStringPool();
        private string sql;
        private string updateSQLString;
        private string oraResult;

        public OracleDBConductor()
        {
            Initializer();
        }

        private void Initializer()
        {
            string connectionString;
            //connectionString = GetConnectionString("192.168.168.249", "1521", "topprod", "aupa_prod", "Cc123456");
            connectionString = projectStringPool.getOracleConnectionString("192.168.168.249", "1521", "toptest", "aupa_prod", "aupa_prod");
            connection = new OracleConnection(connectionString);
        }

        private bool OpenOracleConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("Oracle Side Exception :" + ex.Message);
                return false;
                throw;
            }
        }

        private bool CloseOracleConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("CloseOracleConnectionException :" + ex.Message);
                return false;
                throw;
            }
        }

        public DataTable GetDataTable(String sql)
        {
            DataTable dataTable = null;
            OpenOracleConnection();
            try
            {
                string name = connection.ServiceName;
                CommonUntil commonUntil = new CommonUntil();
                //int year = commonUntil.getYear();
                OracleCommand command = new OracleCommand(sql, this.connection);
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                //OracleParameter[] parameters = new OracleParameter[] {
                //    new OracleParameter("val01",year),
                //    new OracleParameter("val02",month)
                //};
                //command.Parameters.AddRange(parameters);
               //sql = "SELECT * FROM tc_ome_file " +
               //        " WHERE tc_ome06 = '1' ";
                //OracleCommand command = new OracleCommand(sql, this.connection);
                OracleDataReader oracleDataReader = command.ExecuteReader();

                dataTable = new DataTable();
                dataTable.Load(oracleDataReader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OraDBConductor Excetpion" + ex.Message);
                PostalService postalService = new PostalService();
                postalService.SendMail("Levi.Huang@aupa.com.tw", "Intermediate Data Copier Alert", ex.Message);
            }
            finally
            {
                CloseOracleConnection();
            }


            return dataTable;
        }

        public String ProcessedResultPostBack(String Key1)
        {
            oraResult = "SUCCESS";

            sql = "update tc_ome_file set TC_OME06 = '1'" +
                     "where TC_OME06 = '2'  ";
            OracleCommand command = new OracleCommand(sql, connection);
            command.Connection.Open();
            OpenOracleConnection();
            try
            {
                //command.Parameters.Add("@tc_ome06", Key1);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Post Back Exception" + ex.Message);
                oraResult = "FAIL";
            }
            finally
            {
                connection.Close();
            }
            return oraResult;
        }

        public List<OraTc_OmeObject> UpdateTc_OmeDB(List<OraTc_OmeObject> OraTc_Omes)
        {
            sql = "";
            ProjectStringPool stringPool = new ProjectStringPool();
            List<OraTc_OmeObject> updatedTc_OmeObjects = new List<OraTc_OmeObject>();
            sql = stringPool.getUpdateTc_OmeStatus();

            oraResult = "SUCCESS";
            OpenOracleConnection();
            if (OraTc_Omes.Count > 0)
            {
                foreach (OraTc_OmeObject oraTc_Ome in OraTc_Omes)
                {
                    try
                    {
                        OracleCommand command = connection.CreateCommand();
                        command.Connection = connection;
                        command.CommandText = sql;

                        command.Parameters.Add("@val01", oraTc_Ome.Tc_ome06);
                        

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Write("  Exception : " + ex.Message);
                        oraResult = "FAIL";
                    }
                    finally
                    {
                        if (oraResult == "SUCCESS")
                        {
                            updatedTc_OmeObjects.Add(oraTc_Ome);
                        }    
                    }
                }
            }

            CloseOracleConnection();

            return updatedTc_OmeObjects;
        }
        /*
           static private string GetConnectionString(string host, string port, string sid, string user, string pass)
           {
               return String.Format(
                   "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})" +
                   "(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};",
                   host, port, sid, user, pass);
           }
       */


    }
}