using System;
using System.Data.SqlClient;
using System.Data;

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
            connectionString = @"Data Source=OPUS3\SQLEXPRESS;User ID=sa;Password=Qgmfl123!;Database=ecoanimal";
            // connectionString = New String("Data Source=mssql03-mia.braslink.com;User ID=ecoaalbr;Password=8h9ga7;Database=ecoanimal")
           // connectionString = "Data Source=aquanimal2db.cu9zlyfmg2ii.us-east-1.rds.amazonaws.com;User ID=andbuser;Password=Qgqkxp789;Database=aquanimadb";
            //PROVIDER=SQLOLEDB;DATA SOURCE=aquanimaldb.cu9zlyfmg2ii.us-east-1.rds.amazonaws.com,1433;UID=andbuser;PWD=Qgqkxp789;DATABASE=aquanimadb
        }

        public DatabaseHandler(SqlConnection pSqlConn)
        {
            sqlConn = pSqlConn;
        }

        public SqlConnection getConnection()
        {
            return sqlConn;
        }

        public void Close()
        {
            if (sqlConn != null
                && sqlConn.State == System.Data.ConnectionState.Open)
            {
                sqlConn.Close();
            }
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

        public DataSet queryDataSet(string query, string name)
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

        public DataSet retrieveDataSet(String query, String label, SqlConnection sqlConn)
        {
            sqlConn = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConn);

            DataSet ds = new DataSet();
            da.Fill(ds, label);
            return ds;
        }

        public DataSet retrieveDataSet(String query, String label, SqlParameter[] param, SqlConnection sqlConn)
        {
            sqlConn = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConn;

            if (param != null)
                command.Parameters.AddRange(param);

            command.CommandText = query;

            SqlDataAdapter da = new SqlDataAdapter(command);

            DataSet ds = new DataSet();
            da.Fill(ds, label);
            return ds;
        }
    }
}
