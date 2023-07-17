using System;
using System.Data.SqlClient;

namespace eco.database
{
    public class DatabaseHandler
    {
        private SqlConnection sqlConn;
        private SqlCommand sqlComm;
        private SqlDataReader sqlResultDR;
        private SqlDataAdapter sqlAdapt;

        private string connectionString;

        public DatabaseHandler()
        {
            // connectionString = New String("Data Source=sql.braslink.com;User ID=ecoaalbr;Password=8h9ga7;Database=ecoanimal")
            // connectionString = New String("Data Source=mssql03-mia.braslink.com;User ID=ecoaalbr;Password=8h9ga7;Database=ecoanimal")
            connectionString = "Data Source=mssql2k.braslink.com;User ID=ecoaalbr;Password=8h9ga7;Database=ecoanimal";
        }

        public DatabaseHandler(ref SqlConnection pSqlConn)
        {
            sqlConn = pSqlConn;
        }

        public SqlConnection getConnection()
        {
            return sqlConn;
        }

        public void Close()
        {
            if (sqlConn.State == System.Data.ConnectionState.Open)
                sqlConn.Close();
        }

        public void executeNonQuery(string sqlStatement)
        {
            sqlConn = new SqlConnection(connectionString);
            sqlComm = new SqlCommand(sqlStatement, sqlConn);

            sqlComm.CommandType = System.Data.CommandType.Text;

            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                    sqlConn.Open();

                sqlComm.ExecuteNonQuery();

                sqlConn.Close();
            }
            catch (Exception oErr)
            {
                throw oErr;
            }
        }

        public SqlDataReader queryData(string query)
        {
            sqlConn = new SqlConnection(connectionString);
            SqlDataReader sqlDataR;

            sqlComm = new SqlCommand(query, sqlConn);

            sqlComm.CommandType = System.Data.CommandType.Text;

            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                    sqlConn.Open();

                sqlDataR = sqlComm.ExecuteReader();
            }
            catch (Exception oErr)
            {
                throw oErr;
            }

            sqlResultDR = sqlDataR;

            return sqlResultDR;
        }

        public System.Data.DataSet queryDataSet(string query, string name)
        {
            sqlConn = new SqlConnection(connectionString);
            System.Data.DataSet dtSet = new System.Data.DataSet();

            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                    sqlConn.Open();

                sqlAdapt = new SqlDataAdapter(query, sqlConn);

                sqlAdapt.Fill(dtSet, name);
            }
            catch (Exception oErr)
            {
                throw oErr;
            }

            return dtSet;
        }
    }
}
