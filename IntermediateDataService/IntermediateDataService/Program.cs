using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectStringPool projectStringPool = new ProjectStringPool();
            OracleDBConductor oracleDBConductor = new OracleDBConductor();
            DataTable dataTable;
            SqlEi_OmeObject sqlEi_OmeObject;
            OraTc_OmeObject oraTc_OmeObject;

            List<SqlEi_OmeObject> goodSQLServerEi_OmeObjects;
            List<SqlEi_OmeObject> insertedEi_OmeObjects;

            List<OraTc_OmeObject> updatedTc_OmeObjects;

            string oraResult;
            int dataCount;
            string actionResult;

            //int deletedRows;

            try
            {
                string oraSQLString = projectStringPool.getSelectEi_OmeDataSQL();

                dataTable = oracleDBConductor.GetDataTable(oraSQLString);
                oraResult = "";

                goodSQLServerEi_OmeObjects = new List<SqlEi_OmeObject>();

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        sqlEi_OmeObject = new SqlEi_OmeObject();
                        oraTc_OmeObject = new OraTc_OmeObject();
                        oraResult = "Y";

                        try
                        {
                            sqlEi_OmeObject.Ei_ome01 = row[dataTable.Columns["ome01"]].ToString();
                            sqlEi_OmeObject.Ei_ome02 = (row[dataTable.Columns["ome02"]]) == DBNull.Value ? "" :
                                Convert.ToDateTime(row[dataTable.Columns["ome02"]]).ToString("yyyy-MM-dd");
                            sqlEi_OmeObject.Ei_ome04 = row[dataTable.Columns["ome04"]].ToString();
                            sqlEi_OmeObject.Ei_ome05 = row[dataTable.Columns["ome042"]].ToString();
                            sqlEi_OmeObject.Ei_ome06 = row[dataTable.Columns["ome043"]].ToString();
                            sqlEi_OmeObject.Ei_ome07 = row[dataTable.Columns["ome044"]].ToString();
                            sqlEi_OmeObject.Ei_ome08 = row[dataTable.Columns["ome16"]].ToString();
                            sqlEi_OmeObject.Ei_ome09 = row[dataTable.Columns["ome21"]].ToString();
                            sqlEi_OmeObject.Ei_ome10 = (row[dataTable.Columns["ome211"]]) == DBNull.Value ? 0 :
                                   Convert.ToDecimal(row[dataTable.Columns["ome211"]]);
                            sqlEi_OmeObject.Ei_ome11 = (row[dataTable.Columns["ome59"]]) == DBNull.Value ? 0 :
                                    Convert.ToDecimal(row[dataTable.Columns["ome59"]]);
                            sqlEi_OmeObject.Ei_ome12 = (row[dataTable.Columns["ome59x"]]) == DBNull.Value ? 0 :
                                   Convert.ToDecimal(row[dataTable.Columns["ome59x"]]);

                            sqlEi_OmeObject.Ei_ome13 = (row[dataTable.Columns["ome59t"]]) == DBNull.Value ? 0 :
                                    Convert.ToDecimal(row[dataTable.Columns["ome59t"]]);

                        }
                        catch (Exception ex)
                        {
                            oraResult = "N";
                            Console.WriteLine("Foreach Exception:" + ex.Message);
                            PostalService postalService = new PostalService();
                            postalService.SendMail("levi.huang@aupa.com.tw", "Intermediate Data Copier Alert", ex.Message);
                            break;
                        }
                        finally
                        {
                            if (oraResult == "Y")
                            {
                                SQLServerDataSecuricor dataSecuricor = new SQLServerDataSecuricor();
                                dataCount = 0;
                                dataCount = dataSecuricor.SelectEi_OmeRowCounts(sqlEi_OmeObject.Ei_ome01);
                                if (dataCount == 0)
                                {
                                    goodSQLServerEi_OmeObjects.Add(sqlEi_OmeObject);
                                }
                            }
                        }//End of try-catch-finally
                        //}//End of foreach
                    }//End of if else
                    actionResult = "FAILED";
                    
                    insertedEi_OmeObjects = new List<SqlEi_OmeObject>();
                    updatedTc_OmeObjects = new List<OraTc_OmeObject>();

                    if (goodSQLServerEi_OmeObjects.Count > 0)
                    {
                        foreach (SqlEi_OmeObject ei_InsOme in goodSQLServerEi_OmeObjects)
                        {
                            SQLServerConductor sqlServerConductor = new SQLServerConductor();
                            actionResult = sqlServerConductor.InsertEi_OmeSQLServer(ei_InsOme);
                            if (actionResult == "SUCCESS")
                            {
                                insertedEi_OmeObjects.Add(ei_InsOme);
                            }
                        }

                        foreach (SqlEi_OmeObject ei_ome in goodSQLServerEi_OmeObjects)
                        {
                            oracleDBConductor.UpdateTc_OmeDB(ei_ome.Ei_ome01);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write("Exception : " + ex.Message);
                return;
            }

          
        }
    }
}
