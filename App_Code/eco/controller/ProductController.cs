using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace eco.controllers
{
    public class ProductController : BasicController
    {
        public ProductController() : base()
        {
        }


        public DataSet getProductInfo(int aPid)
        {
            String sql = "select * from tbProdutos where pkid = @PKId";

            ArrayList paramList = new ArrayList();
            paramList.Add(new SqlParameter("PKId", aPid));
            DataSet ds = getDataSet(sql, "ds", retrieveParamsArray(paramList));
            CloseDb();
            return ds;
        }

        public String getNotaGif(int aPid)
        {
            String sql = @"
                            select isNull(ceiling(avg(cast(rank as float))),11) as nota 
                            from tbcomentario where idproduto = @PKId;
            ";

            ArrayList paramList = new ArrayList();
            paramList.Add(new SqlParameter("PKId", aPid));
            DataSet ds = getDataSet(sql, "ds", retrieveParamsArray(paramList));
            CloseDb();
            int nota = int.Parse(ds.Tables[0].Rows[0]["nota"].ToString());
            String notaStr = "";
            if (nota < 10)
            {
                notaStr = nota + "star.png";
            }
            
            return notaStr;
        }


    }
}
