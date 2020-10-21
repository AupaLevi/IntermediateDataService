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

            List<OraEi_OmeObject> insertSQLServerEi_OmeObjects;
            List<OraEi_OmeObject> goodSQLServerEi_OmeObjects;
            List<OraEi_OmeObject> insertedEi_OmeObjects;

            List<OraTc_OmeObject> updateSQLServerTc_OmeObjects;
            List<OraTc_OmeObject> goodSQLServerTc_OmeObjects;
            List<OraTc_OmeObject> updatedTc_OmeObjects;

            string oraResult;
            int dataCount;
            //int deletedRows;

            try
            {
                string oraSQLString = projectStringPool.getSelectEi_OmeDataSQL();
                
                dataTable = oracleDBConductor.GetDataTable(oraSQLString);
                oraResult = "";

                insertSQLServerEi_OmeObjects = new List<OraEi_OmeObject>();


                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        oraEi_OmeObject = new OraEi_OmeObject();
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
                                //SQLServerDataSecuricor dataSecuricor = new SQLServerDataSecuricor();
                                //dataCount = 0;
                                //dataCount = dataSecuricor.SelectCCCxRowCounts(
                                ////    oraCCCxObject.Tc_cccx01, oraCCCxObject.Tc_cccx02, oraCCCxObject.Tc_cccx03);
                                ////if (dataCount == 0)
                                ////{
                                insertSQLServerEi_OmeObjects.Add(oraEi_OmeObject);
                                ////}
                            }
                        }//End of try-catch-finally
                        //}//End of foreach
                    }//End of if else
                     //ogaSQLServerResult = "FAILED";
                    goodSQLServerEi_OmeObjects = new List<OraEi_OmeObject>();
                    insertedEi_OmeObjects = new List<OraEi_OmeObject>();

                    if (insertSQLServerEi_OmeObjects.Count > 0)
                    {
                        SQLServerConductor sqlServerConductor = new SQLServerConductor();
                       

                        //deletedRows = sqlServerConductor.DeleteOmeRows(year, month);
                        insertedEi_OmeObjects = sqlServerConductor.InsertEi_OmeSQLServer(insertSQLServerEi_OmeObjects);
                    }

                    dataCount = 0;
                    dataCount = insertedEi_OmeObjects.Count;

                }

            }
            catch (Exception ex)
            {
                Console.Write("Exception : " + ex.Message);
                return;
            }

            // Tc_Ome Update
                try
                {
                    string oraSQLString = projectStringPool.getSelectTc_OmeDataSQL();

                    dataTable = oracleDBConductor.GetDataTable(oraSQLString);

                updateSQLServerTc_OmeObjects = new List<OraTc_OmeObject>();

                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            oraTc_OmeObject = new OraTc_OmeObject();
                            oraResult = "Y";

                            try
                            {
                                oraTc_OmeObject.Tc_ome01 = row[dataTable.Columns["tc_ome01"]].ToString();
                                oraTc_OmeObject.Tc_ome02 = (row[dataTable.Columns["tc_ome02"]]) == DBNull.Value ? "" :
                                    Convert.ToDateTime(row[dataTable.Columns["tc_ome02"]]).ToString("yyyy-MM-dd");
                                oraTc_OmeObject.Tc_ome03 = row[dataTable.Columns["tc_ome03"]].ToString();
                                oraTc_OmeObject.Tc_ome04 = row[dataTable.Columns["tc_ome04"]].ToString();
                                oraTc_OmeObject.Tc_ome05 = row[dataTable.Columns["tc_ome05"]].ToString();
                                oraTc_OmeObject.Tc_ome06 = row[dataTable.Columns["tc_ome06"]].ToString();
                                oraTc_OmeObject.Tc_Omevoid = row[dataTable.Columns["tc_omevoid"]].ToString();
                                oraTc_OmeObject.Tc_Omevoidu = row[dataTable.Columns["tc_omevoidu"]].ToString();
                                oraTc_OmeObject.Tc_Omevoidd = (row[dataTable.Columns["tc_omevoidd"]]) == DBNull.Value ? "" :
                                    Convert.ToDateTime(row[dataTable.Columns["tc_Omevoidd"]]).ToString("yyyy-MM-dd");
                                oraTc_OmeObject.Tc_Omevoidt = row[dataTable.Columns["tc_omevoidt"]].ToString();
                                oraTc_OmeObject.Tc_Omevoidm = row[dataTable.Columns["tc_omevoidm"]].ToString();
                                oraTc_OmeObject.Tc_Omevoids = row[dataTable.Columns["tc_omevoids"]].ToString();
                                oraTc_OmeObject.Tc_Omecncl = row[dataTable.Columns["tc_omecncl"]].ToString();
                                oraTc_OmeObject.Tc_Omecnclu = row[dataTable.Columns["tc_omecnclu"]].ToString();
                                oraTc_OmeObject.Tc_Omecncld = (row[dataTable.Columns["tc_omecncld"]]) == DBNull.Value ? "" :
                                    Convert.ToDateTime(row[dataTable.Columns["tc_omecncld"]]).ToString("yyyy-MM-dd");
                                oraTc_OmeObject.Tc_Omecnclt = row[dataTable.Columns["tc_omecnclt"]].ToString();
                                oraTc_OmeObject.Tc_Omecnclm = row[dataTable.Columns["tc_omecnclm"]].ToString();
                                oraTc_OmeObject.Tc_Omecncls = row[dataTable.Columns["tc_omecncls"]].ToString();
                            }
                            catch (Exception ex)
                            {
                                oraResult = "N";
                                Console.WriteLine("Foreach Exception:" + ex.Message);

                            }

                            finally
                            {
                                if (oraResult == "Y")
                                {
                                    SQLServerDataSecuricor dataSecuricor = new SQLServerDataSecuricor();
                                //dataCount = 0;
                                //dataCount = dataSecuricor.SelectOebxRowCounts(oraOebxObject.Tc_oebx01,
                                //    oraOebxObject.Tc_oebx02);

                                //For UPDATE Oebx
                                updateSQLServerTc_OmeObjects.Add(oraTc_OmeObject);

                                }
                            }//End of Getting oebx by each Oeax02
                        }
                        updatedTc_OmeObjects = new List<OraTc_OmeObject>();

                        if (updateSQLServerTc_OmeObjects.Count > 0)
                        {
                            updatedTc_OmeObjects = oracleDBConductor.UpdateTc_OmeDB(updateSQLServerTc_OmeObjects);
                        }

                        dataCount = 0;
                        dataCount = updatedTc_OmeObjects.Count;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Exception:" + ex.Message);
                    throw;
                }
                //goodTc_OmeList = new List<OraTc_OmeObject>();
                //if(listCount >0)
                //{
                    //foreach (OraTc_OmeObject Tc_Omeupd in tc_OmeList)
                    //{
                        //oracleDBConductor = new OracleDBConductor();
                        //goodTc_OmeList.Add(Tc_Omeupd);
                    //}
                //}

                //if (goodTc_OmeList.Count > 0)
                //{
                    //foreach (OraTc_OmeObject tc_OmeObjPostBack in goodTc_OmeList)
                    //{
                        //try
                        //{
                            //postBackResult = oracleDBConductor.ProcessedResultPostBack(tc_OmeObjPostBack.Tc_ome06);
                        //}
                        //catch (Exception ex)
                        //{
                            //Console.WriteLine("Post Back Exception" + ex.Message);
                        //}
                    //}
                //}
        }
    }
}
