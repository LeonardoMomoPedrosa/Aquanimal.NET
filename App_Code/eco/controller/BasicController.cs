using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using eco.database;

namespace eco.controllers
{
    public class BasicController
    {
        protected DatabaseHandler dbHandler;

        public BasicController()
        {
            dbHandler = new DatabaseHandler();
        }

        public BasicController(SqlConnection sqlConn)
        {
            dbHandler = new DatabaseHandler(sqlConn);
        }

        public void CloseDb()
        {
            dbHandler.Close();
        }

        protected SqlParameter[] retrieveParamsArray(ArrayList aList)
        {
            return (SqlParameter[])aList.ToArray(typeof(SqlParameter));
        }

        protected DataSet getDataSet(String query, String label, SqlParameter[] param)
        {
            DataSet ds = null;
            DatabaseHandler db = new DatabaseHandler();
            ds = db.retrieveDataSet(query, label, param, dbHandler.getConnection());
            return ds;
        }

    }
}
