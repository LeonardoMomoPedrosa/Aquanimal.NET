using eco.controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrderController
/// </summary>
public class OrderController : BasicController
{
    public OrderController() : base()
    {
    }

    public DataSet getOrderInfo(int pkid)
    {
        String sql = @"
                        SELECT s.aa,
                                s.PKId,
                                s.idUser,
                                s.val,
                                s.nome,
                                c.parc,
                                c.parc * c.parcVal as amt
                            FROM sysalloc s
                            JOIN tbCompra c on c.idCC = s.PKid
                            WHERE c.PKId = @pkid
                    ";
        DataSet ds;
        ArrayList paramList = new ArrayList();
        paramList.Add(new SqlParameter("pkid", pkid));
        ds = getDataSet(sql, "ds", retrieveParamsArray(paramList));

        return ds;
    }

    public DataSet getOrders()
    {
        String sql = @"
                        select * from tbcompra where metodoPagto='C' and status='A' and AUTHCODE is null
                    ";
        DataSet ds;
        ArrayList paramList = new ArrayList();
        ds = getDataSet(sql, "ds", retrieveParamsArray(paramList));

        return ds;
    }
}