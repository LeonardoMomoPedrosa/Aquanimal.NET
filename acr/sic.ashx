<%@ WebHandler Language="C#" Class="Sic" %>

using System;
using System.Web;
using eco.utils;
using System.Configuration;

public class Sic : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string x = "agyyu78aduoqnjppapdhjkfo28gzai7t";
        context.Response.ContentType = "text/plain";
        var refh = context.Request.UserHostName;
        bool isTestEv = ConfigurationManager.AppSettings["testenv"] != null
                            && ConfigurationManager.AppSettings["testenv"].Equals("1");
        if (refh.Contains("127.0.0.1") || isTestEv)
        {
            String action = context.Request["a"];
            String data = context.Request["d"];
            String result = "";
            switch (action)
            {
                case "1":
                    result = EcoUtils.Sic(data, x);
                    break;
                case "274498":
                    result = EcoUtils.Asic(data, x);
                    break;

            }
            context.Response.Write(result);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}