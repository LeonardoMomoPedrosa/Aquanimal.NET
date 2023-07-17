using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using eco.database;


namespace eco.utils
{
    public class SessionUtils
    {
        public string getDbSession(string sessionId, string code)
        {
            DatabaseHandler dbHandler = new DatabaseHandler();
            SqlDataReader dbResult;
            string returnVal;

            dbResult = dbHandler.queryData("SELECT value FROM tbSession where sessionId='" + sessionId + "' and alias='" + code + "'");

            if (!dbResult.Read())
                returnVal = "invalid";
            else
                returnVal = dbResult.GetString(0);

            dbHandler.Close();

            return returnVal;
        }

        public bool hasDbSession(string sessionId)
        {
            DatabaseHandler dbHandler = new DatabaseHandler();
            SqlDataReader dbResult;
            bool returnVal;

            dbResult = dbHandler.queryData("select * from tbSession where sessionId = '" + sessionId + "'");

            returnVal = dbResult.Read();

            dbHandler.Close();

            return returnVal;
        }
    }
}
