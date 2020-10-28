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
            OraEi_OmeObject oraEi_OmeObject;
            OraTc_OmeObject oraTc_OmeObject;

            List<OraEi_OmeObject> goodSQLServerEi_OmeObjects;
            List<OraEi_OmeObject> insertedEi_OmeObjects;

            List<OraTc_OmeObject> updateSQLServerTc_OmeObjects;

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

                goodSQLServerEi_OmeObjects = new List<OraEi_OmeObject>();
                updateSQLServerTc_OmeObjects = new List<OraTc_OmeObject>();

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        oraEi_OmeObject = new OraEi_OmeObject();
                        oraTc_OmeObject = new OraTc_OmeObject();
                        oraResult = "Y";

                        try
                        {
                            oraEi_OmeObject.Ei_ome01 = row[dataTable.Columns["ome01"]].ToString();
                            oraEi_OmeObject.Ei_ome02 = (row[dataTable.Columns["ome02"]]) == DBNull.Value ? "" :
                                Convert.ToDateTime(row[dataTable.Columns["ome02"]]).ToString("yyyy-MM-dd");
                            oraEi_OmeObject.Ei_ome04 = row[dataTable.Columns["ome04"]].ToString();
                            oraEi_OmeObject.Ei_ome05 = row[dataTable.Columns["ome042"]].ToString();
                            oraEi_OmeObject.Ei_ome06 = row[dataTable.Columns["ome043"]].ToString();
                            oraEi_OmeObject.Ei_ome07 = row[dataTable.Columns["ome044"]].ToString();
                            oraEi_OmeObject.Ei_ome08 = row[dataTable.Columns["ome16"]].ToString();
                            oraEi_OmeObject.Ei_ome09 = row[dataTable.Columns["ome21"]].ToString();
                            oraEi_OmeObject.Ei_ome10 = (row[dataTable.Columns["ome211"]]) == DBNull.Value ? 0 :
                                   Convert.ToDecimal(row[dataTable.Columns["ome211"]]);
                            oraEi_OmeObject.Ei_ome11 = (row[dataTable.Columns["ome59"]]) == DBNull.Value ? 0 :
                                    Convert.ToDecimal(row[dataTable.Columns["ome59"]]);
                            oraEi_OmeObject.Ei_ome12 = (row[dataTable.Columns["ome59x"]]) == DBNull.Value ? 0 :
                                   Convert.ToDecimal(row[dataTable.Columns["ome59x"]]);

                            oraEi_OmeObject.Ei_ome13 = (row[dataTable.Columns["ome59t"]]) == DBNull.Value ? 0 :
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
                                dataCount = dataSecuricor.SelectEi_OmeRowCounts(oraEi_OmeObject.Ei_ome01);
                                if (dataCount == 0)
                                {
                                    goodSQLServerEi_OmeObjects.Add(oraEi_OmeObject);
                                    updateSQLServerTc_OmeObjects.Add(oraTc_OmeObject);
                                }
                            }
                        }//End of try-catch-finally
                        //}//End of foreach
                    }//End of if else
                    actionResult = "FAILED";
                    
                    insertedEi_OmeObjects = new List<OraEi_OmeObject>();
                    updatedTc_OmeObjects = new List<OraTc_OmeObject>();

                    if (goodSQLServerEi_OmeObjects.Count > 0)
                    {
                        foreach (OraEi_OmeObject ei_InsOme in goodSQLServerEi_OmeObjects)
                        {
                            SQLServerConductor sqlServerConductor = new SQLServerConductor();
                            actionResult = sqlServerConductor.InsertEi_OmeSQLServer(ei_InsOme);
                            if (actionResult == "SUCCESS")
                            {
                                insertedEi_OmeObjects.Add(ei_InsOme);
                            }
                        }
                    }

                    if (updateSQLServerTc_OmeObjects.Count > 0)
                    {
                        foreach (OraEi_OmeObject ei_ome in goodSQLServerEi_OmeObjects)
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
