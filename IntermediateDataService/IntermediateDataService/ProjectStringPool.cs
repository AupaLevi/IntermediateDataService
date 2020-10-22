using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class ProjectStringPool
    {
        //Oracle Side
        private string oracleConnectionString;
        private string selectTc_omeDataSQL;
        private string selectOmeDataSQL;
        private string insSQLServerOmeSQL;
        private string updateTc_OmeStatus;
        //SQLServer Side
        private string sqlServerEi_omeDataCount;
        private string insSQLServerEi_omeSQL;
        private string selectEi_omeDataSQL;
        private string deleteEi_omeDataSQL;

        //private string updSQLServerCccxSQL;

        public string getOracleConnectionString(string host, string port, string sid, string user, string pass)
        {
            this.oracleConnectionString = String.Format(
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})" +
                "(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};",
                host, port, sid, user, pass);
            return this.oracleConnectionString;
        }

        public string getSelectTc_OmeDataSQL()
        {
            this.selectTc_omeDataSQL =
                " SELECT * FROM tc_ome_file " ; 
       
            return this.selectTc_omeDataSQL;
        }

        public string getSelectOmeDataSQL()
        {
            this.selectOmeDataSQL =
               " select * from ome_file " +
               " where year (ome02) = 2020 " +
               " order by ome02 desc " ;

            return this.selectOmeDataSQL;
        }

        public string getSelectEi_OmeDataSQL()
        {
            this.selectEi_omeDataSQL =
                " select * from tc_ome_file , ome_file " +
                " where tc_ome01 = ome01 and tc_ome06 = '1' ";



            return this.selectEi_omeDataSQL;
        }

        public string getDeleteOmeDataSQL()
        {
            this.deleteEi_omeDataSQL = "DELETE FROM ei_ome_file " +
                                     " WHERE ei_ome02 = @val01 " +
                                     "   AND ei_ome04 = @val02 ";
            return this.deleteEi_omeDataSQL;
        }

        public string getsqlServerEi_OmeDataCount()
        {

            this.sqlServerEi_omeDataCount =
                "SELECT COUNT(ei_ome01) FROM ei_ome_file " +
                " WHERE ei_ome01 = ?  " +
                "   AND ei_ome08 = ? " ;
               
            return this.sqlServerEi_omeDataCount;
        }

        public string getInsSQLServerOmeSQL()
        {
            this.insSQLServerOmeSQL =
                 " INSERT INTO tc_ome_file (" +
                 " tc_ome01 ,tc_ome02 ,tc_ome03 ,tc_ome04 ,tc_ome05 ,tc_ome06 , " +
                 " tc_omevoid ,tc_omevoidu ,tc_omevoidd ,tc_omevoidt ,tc_omevoidm  , " +
                 " tc_omecncl ,tc_omecnclu ,tc_omecncld ,tc_omecnclt ,tc_omecnclm  " +
                 " ) VALUES ( " +
                 " @val01 ,@val02 ,@val03 ,@val04 ,@val05 ,@val06 , " +
                 " @val07 ,@val08 ,@val09 ,@val010 ,@val011 ,@val012 , " +
                 " @val013 ,@val014  , " +
                 " @val015 ,@val016    " +
                 ")";

            return this.insSQLServerOmeSQL;
        }

        public string getInsSQLServerEi_OmeSQL()
        {
            this.insSQLServerEi_omeSQL =
                " INSERT INTO Ei_ome_file (" +
                " ei_ome01 ,ei_ome02 ,ei_ome04 ,ei_ome05 ,ei_ome06 ,ei_ome07 ,ei_ome08 ,ei_ome09 ,ei_ome10 ,ei_ome11 ," +
                " ei_ome12 ,ei_ome13  " +
                ")VALUES(" +
                "@val01 ,@val02 ,@val03 ,@val04 ,@val05 ,@val06 ,@val07 ,@val08 ,@val09 ,@val010 ,@val011 ,@val012  " +
                 ")";
            return this.insSQLServerEi_omeSQL;
        }

        public string getUpdateTc_OmeStatus()
        {
            this.updateTc_OmeStatus =
                " update tc_ome_file set TC_OME06 = '2'" +
                " where TC_OME06 = '1'" ;
            return this.updateTc_OmeStatus;
        }

    }
}
