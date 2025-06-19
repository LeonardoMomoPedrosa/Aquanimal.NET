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
                                isnull(c.REDESTATUS,'') as REDESTATUS,
		                        c.REDESTATUSDESC,
                                c.TID,
                                c.STEP,
                                c.AUTHCODE,
		                        c.AUTHCODE_SHIP,
		                        isnull(c.REDESTATUS_SHIP,'') as REDESTATUS_SHIP,
		                        c.REDESTATUSDESC_SHIP,
                                c.TID_SHIP,
                                c.STEP_SHIP,
                                c.frete,
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
                        select c.PKId,
		                        c.data,
								cc.aa,
		                        c.frete,
		                        c.parc,
		                        round(c.parc*c.parcVal,2,1) as amt,
		                        c.AUTHCODE,
		                        isnull(c.REDESTATUS,'') as REDESTATUS,
		                        c.REDESTATUSDESC,
                                c.TID,
                                c.STEP,
		                        c.AUTHCODE_SHIP,
		                        isnull(c.REDESTATUS_SHIP,'') as REDESTATUS_SHIP,
		                        c.REDESTATUSDESC_SHIP,
                                c.TID_SHIP,
                                c.STEP_SHIP,
		                        u.nome,
		                        u.cidade,
		                        u.estado
                        from tbcompra c 
						left join sysalloc cc on cc.pkid = c.idcc
                        join tbusuarios u on u.id = c.PKIdUsuario
                        where c.metodoPagto='C' 
                        and c.status='G'
						and len(cc.aa) > 5
                        order by c.data desc
                    ";
        DataSet ds;
        ArrayList paramList = new ArrayList();
        ds = getDataSet(sql, "ds", retrieveParamsArray(paramList));

        return ds;
    }

    public void SalvaTRX(int PKId,
                                    string REDESTATUS,
                                    string REDESTATUSDESC,
                                    string TID,
                                    string AUTHCODE,
                                    string NSU,
                                    int STEP)
    {
        String sql = @"
                        update tbcompra 
                        set REDESTATUS=@redestatus,
                            REDESTATUSDESC=@redestatusdesc,
                            TID=@tid,
                            AUTHCODE=@authcode,
                            STEP=@step,
                            NSU=@nsu
                        where PKId = @pkid;
                        insert into tbRedeHistory values (@pkid,getdate(),'V',@redestatus,@redestatusdesc,@tid,@authcode,@step)
                    ";

        ArrayList paramList = new ArrayList();
        paramList.Add(new SqlParameter("pkid", PKId));
        paramList.Add(new SqlParameter("redestatus", REDESTATUS));
        paramList.Add(new SqlParameter("redestatusdesc", REDESTATUSDESC));
        paramList.Add(new SqlParameter("tid", TID));
        paramList.Add(new SqlParameter("authcode", AUTHCODE));
        paramList.Add(new SqlParameter("step", STEP));
        paramList.Add(new SqlParameter("nsu", NSU));
        getDataSet(sql, "ds", retrieveParamsArray(paramList));

    }

    public void SalvaTRXSum(int PKId,
                                string REDESTATUS,
                                string REDESTATUSDESC,
                                int STEP)
    {
        String sql = @"
                        update tbcompra 
                        set REDESTATUS=@redestatus,
                            REDESTATUSDESC=@redestatusdesc,
                            STEP=@step
                        where PKId = @pkid;
                        insert into tbRedeHistory values (@pkid,getdate(),'V',@redestatus,@redestatusdesc,'','',@step)
                    ";

        ArrayList paramList = new ArrayList();
        paramList.Add(new SqlParameter("pkid", PKId));
        paramList.Add(new SqlParameter("redestatus", REDESTATUS));
        paramList.Add(new SqlParameter("redestatusdesc", REDESTATUSDESC));
        paramList.Add(new SqlParameter("step", STEP));
        getDataSet(sql, "ds", retrieveParamsArray(paramList));

    }

    public void SalvaTRXShip(int PKId,
                                string REDESTATUS,
                                string REDESTATUSDESC,
                                string TID,
                                string AUTHCODE,
                                string NSU,
                                int STEP)
    {
        String sql = @"
                        update tbcompra 
                        set REDESTATUS_SHIP=@redestatus,
                            REDESTATUSDESC_SHIP=@redestatusdesc,
                            TID_SHIP=@tid,
                            AUTHCODE_SHIP=@authcode,
                            STEP_SHIP=@step,
                            NSU_SHIP=@nsu
                        where PKId = @pkid;
                        insert into tbRedeHistory values (@pkid,getdate(),'S',@redestatus,@redestatusdesc,@tid,@authcode,@step)
                    ";

        ArrayList paramList = new ArrayList();
        paramList.Add(new SqlParameter("pkid", PKId));
        paramList.Add(new SqlParameter("redestatus", REDESTATUS));
        paramList.Add(new SqlParameter("redestatusdesc", REDESTATUSDESC));
        paramList.Add(new SqlParameter("tid", TID));
        paramList.Add(new SqlParameter("authcode", AUTHCODE));
        paramList.Add(new SqlParameter("step", STEP));
        paramList.Add(new SqlParameter("nsu", NSU));
        getDataSet(sql, "ds", retrieveParamsArray(paramList));

    }

    public void SalvaTRXShipSum(int PKId,
                            string REDESTATUS,
                            string REDESTATUSDESC,
                            int STEP)
    {
        String sql = @"
                        update tbcompra 
                        set REDESTATUS_SHIP=@redestatus,
                            REDESTATUSDESC_SHIP=@redestatusdesc,
                            STEP_SHIP=@step
                        where PKId = @pkid;
                        insert into tbRedeHistory values (@pkid,getdate(),'S',@redestatus,@redestatusdesc,'','',@step)
                    ";

        ArrayList paramList = new ArrayList();
        paramList.Add(new SqlParameter("pkid", PKId));
        paramList.Add(new SqlParameter("redestatus", REDESTATUS));
        paramList.Add(new SqlParameter("redestatusdesc", REDESTATUSDESC));
        paramList.Add(new SqlParameter("step", STEP));
        getDataSet(sql, "ds", retrieveParamsArray(paramList));

    }
}