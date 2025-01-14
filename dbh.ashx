<%@ WebHandler Language="C#" Class="dbh" %>

using System;
using System.Web;
using StackExchange.Redis;

public class dbh : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
       
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}