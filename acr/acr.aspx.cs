using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.UI.WebControls;
using System.Net;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;
using System.Drawing;
using eco.utils;

public partial class acr_acr : System.Web.UI.Page
{
    string username = ConfigurationManager.AppSettings["pv"];
    string password = ConfigurationManager.AppSettings["redetoken"];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string tten = ConfigurationManager.AppSettings["testenv"];

            if (Request.UrlReferrer != null)
            {
                if (!Request.UrlReferrer.ToString().Contains("novager.aquanimal.com.br"))
                {
                    Response.End();
                }
            }
            else if (tten != null)
            {
                if (!tten.Equals("1"))
                {
                    Response.End();
                }
            }
            else
            {
                Response.End();
            }

            OrderController orderContol = new OrderController();
            DataSet ds;
            try
            {
                ds = orderContol.getOrders();
            }
            finally
            {
                orderContol.CloseDb();
            }
            Repeater_orders.DataSource = ds;
            Repeater_orders.DataBind();
        }
    }

    protected void cobrarTeste(object sender, EventArgs args)
    {
        Label_Debug.Text = "Teste " + DateTime.Now;
    }

    protected void cobrar(object sender, EventArgs args)
    {
        Button chargeButton = (Button)sender;
        Label label = (Label)chargeButton.Parent.FindControl("LabelResult");
        int orderId = int.Parse(chargeButton.CommandArgument);

        processa("PRODUTO", orderId, ref label, ref chargeButton);
    }

    protected void cobrarFull(object sender, EventArgs args)
    {
        Button chargeButton = (Button)sender;
        Label label = (Label)chargeButton.Parent.FindControl("LabelResultFull");
        int orderId = int.Parse(chargeButton.CommandArgument);

        processa("PRODUTO+FRETE", orderId, ref label, ref chargeButton);
    }

    protected void cobrarShip(object sender, EventArgs args)
    {
        Button chargeButton = (Button)sender;
        Label label = (Label)chargeButton.Parent.FindControl("LabelResultShip");
        int orderId = int.Parse(chargeButton.CommandArgument);

        processa("FRETE", orderId, ref label, ref chargeButton);
    }

    private void processa(String chargeType, int orderId, ref Label label, ref Button button)
    {
        bool freteInd = chargeType.Equals("FRETE");
        bool isFull = chargeType.Equals("PRODUTO+FRETE");

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        OrderController orderController = new OrderController();
        var orderDs = orderController.getOrderInfo(orderId);
        var currStepLbl = freteInd ? "STEP_SHIP" : "STEP";

        var currentStepObj = orderDs.Tables[0].Rows[0][currStepLbl];
        var currentStep = currentStepObj == DBNull.Value ? 1 : (int)currentStepObj;

        var freteLbl = freteInd ? "FRETE" : "";
        HttpResponseMessage response = new HttpResponseMessage();
        AutorizacaoResult res = new AutorizacaoResult();
        string outp = "";
        ///AUTORIZACAO
        try
        {
            if (currentStep == 2)
            {
                res = new AutorizacaoResult();

                if (freteInd)
                {
                    res.amount = int.Parse(((Double)orderDs.Tables[0].Rows[0]["frete"] * 100).ToString());
                    res.tid = orderDs.Tables[0].Rows[0]["TID_SHIP"].ToString();
                    res.authorizationCode = orderDs.Tables[0].Rows[0]["AUTHCODE_SHIP"].ToString();
                }
                else
                {
                    if (isFull)
                    {
                        var amt = (Double)orderDs.Tables[0].Rows[0]["amt"];
                        var amtFrete = (Double)orderDs.Tables[0].Rows[0]["frete"];
                        amt += amtFrete;
                        res.amount = int.Parse((amt * 100).ToString());
                    } else
                    {
                        res.amount = int.Parse(((Double)orderDs.Tables[0].Rows[0]["amt"] * 100).ToString());
                    }
                    res.tid = orderDs.Tables[0].Rows[0]["TID"].ToString();
                    res.authorizationCode = orderDs.Tables[0].Rows[0]["AUTHCODE"].ToString();
                }
            }
            else
            {
                currentStep = 1;
                response = ProcessaAutorizacao(freteInd, isFull, orderId, orderDs);
                outp = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<AutorizacaoResult>(outp);
                }
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                currentStep = 2;
                label.Text = res.authorizationCode + " - " + res.returnMessage;
                if (freteInd)
                {
                    orderController.SalvaTRXShip(orderId,
                                                freteLbl + " AUTORIZADO",
                                                res.returnMessage ?? "",
                                                res.tid,
                                                res.authorizationCode,
                                                res.nsu,
                                                currentStep);
                }
                else
                {
                    orderController.SalvaTRX(orderId,
                            freteLbl + " AUTORIZADO",
                            res.returnMessage ?? "",
                            res.tid,
                            res.authorizationCode,
                            res.nsu,
                            currentStep);
                }

                /// CONFIRMACAO
                try
                {
                    var respConf = ProcessaConfirmacao(res.tid, res.amount + "");
                    string outpc = respConf.Content.ReadAsStringAsync().Result;
                    if (respConf.StatusCode == HttpStatusCode.OK)
                    {
                        ConfirmResult confRes = JsonConvert.DeserializeObject<ConfirmResult>(outpc);
                        label.Text = confRes.authorization.returnCode + " - " + confRes.authorization.returnMessage;
                        if (freteInd)
                        {
                            orderController.SalvaTRXShip(orderId,
                                                        freteLbl + " CONFIRMADO",
                                                        confRes.authorization.returnMessage ?? "",
                                                        confRes.authorization.tid,
                                                        confRes.authorization.brand.authorizationCode,
                                                        res.nsu,
                                                        currentStep);
                            button.Enabled = false;
                        }
                        else
                        {
                            orderController.SalvaTRX(orderId,
                                    freteLbl + " CONFIRMADO",
                                    confRes.authorization.returnMessage ?? "",
                                    confRes.authorization.tid,
                                    confRes.authorization.brand.authorizationCode,
                                    res.nsu,
                                    currentStep);
                            button.Enabled = false;
                        }
                    }
                    else
                    {
                        label.Text = respConf.StatusCode + " - " + respConf.ReasonPhrase + " - " + outpc;
                        if (freteInd)
                        {
                            orderController.SalvaTRXShipSum(orderId,
                                                        freteLbl + " ERRO " + respConf.StatusCode,
                                                        respConf.ReasonPhrase,
                                                        currentStep);
                        }
                        else
                        {
                            orderController.SalvaTRXSum(orderId,
                                                        freteLbl + " ERRO " + respConf.StatusCode,
                                                        respConf.ReasonPhrase,
                                                        currentStep);
                        }
                    }

                }
                catch (Exception exc)
                {
                    label.Text = exc.Message + exc.StackTrace;
                    if (freteInd)
                    {
                        orderController.SalvaTRXShipSum(orderId,
                                        freteLbl + " ERRO Exception",
                                        exc.Message,
                                        currentStep);
                    }
                    else
                    {
                        orderController.SalvaTRXSum(orderId,
                        freteLbl + " ERRO Exception",
                        exc.Message,
                        currentStep);
                    }
                }

            }
            else
            {
                label.Text = response.StatusCode + " - " + response.ReasonPhrase + " - " + outp;
                if (freteInd)
                {
                    orderController.SalvaTRXShipSum(orderId,
                                                freteLbl + " ERRO " + response.StatusCode,
                                                response.ReasonPhrase,
                                                currentStep);
                }
                else
                {
                    orderController.SalvaTRXSum(orderId,
                                                freteLbl + " ERRO " + response.StatusCode,
                                                response.ReasonPhrase,
                                                currentStep);
                }
            }
        }
        catch (Exception ex)
        {
            label.Text = ex.Message + ex.StackTrace;
            if (freteInd)
            {
                orderController.SalvaTRXShipSum(orderId,
                                freteLbl + " ERRO Exception",
                                ex.Message,
                                currentStep);
            }
            else
            {
                orderController.SalvaTRXSum(orderId,
                freteLbl + " ERRO Exception",
                ex.Message,
                currentStep);
            }
        }
    }

    private HttpResponseMessage ProcessaAutorizacao(bool freteInd, bool isFull, int orderId, DataSet orderDs)
    {
        /////////Autorização
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var idAmt = freteInd ? "frete" : "amt";
        var dataRow = orderDs.Tables[0].Rows[0];
        var exp = dataRow["val"].ToString().Split('/');

        var ccc = Desic(dataRow["aa"].ToString());

        var cc = ccc.Split('-');
        var amt = "";

        if (isFull)
        {
            var amtProd = (Double)dataRow["amt"];
            var amtFrete = (Double)dataRow["frete"];
            amtProd += amtFrete;
            amt = (amtProd * 100).ToString();
        } else
        {
            amt = ((Double)dataRow[idAmt] * 100).ToString();
        }

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["rede_endpoint"]);

        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

        request.Headers.Add("Authorization", "Basic " + credentials);
        var autSend = new AutorizacaoSend
        {
            capture = true,
            kind = "credit",
            reference = freteInd ? "S" + orderId : orderId + "",
            amount = amt,//10,00 = 1000
            installments = freteInd ? 1 : int.Parse(dataRow["parc"] + ""),
            cardholderName = dataRow["nome"] + "",
            cardNumber = cc[0],
            expirationMonth = int.Parse(exp[0]),
            expirationYear = int.Parse(exp[1]),
            securityCode = cc[1],
            //softDescriptor = freteInd ? "Frete Aquanimal" : "Venda Aquanimal",
            subscription = false,
            origin = 1,
            distributorAffiliation = 0,
            storageCard = "0",
            transactionCredentials = new TransactionCredentials { credentialId = "01" }
        };

        string jsonString = JsonConvert.SerializeObject(autSend, Formatting.None);
        request.Content = new StringContent(jsonString, null, "application/json");

        var response = client.SendAsync(request).Result;

        return response;
    }

    private string Desic(string txt)
    {
        string u = "https://aquanimal.com.br/acr/sic.ashx?a=274498&d=" + Server.UrlEncode(txt);
        string response = new WebClient().DownloadString(u);
        return response;
    }

    private HttpResponseMessage ProcessaConfirmacao(string tid, string amt)
    {
        /////////Autorização
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, ConfigurationManager.AppSettings["rede_endpoint"] + tid);
        request.Headers.Add("Transaction-Response", "brand-return-opened");

        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

        request.Headers.Add("Authorization", "Basic " + credentials);

        var response = client.SendAsync(request).Result;

        return response;
    }

    public class ProcessaResultInfo
    {
        public int Step { get; set; }
        public HttpResponseMessage responseMessage { get; set; }
    }

    protected Color GetStatusColor(string status)
    {
        var color = Color.Black;
        if (status.ToUpper().Contains("ERRO"))
        {
            color = Color.Red;
        }
        else if (status.ToUpper().Contains("CONFIRMADO"))
        {
            color = Color.Blue;
        }

        return color;
    }

    protected bool IsSuccess(string status)
    {
        var success = false;
        if (status.ToUpper().Contains("ERRO"))
        {
            success = false;
        }
        else if (status.ToUpper().Contains("CONFIRMADO"))
        {
            success = true;
        }

        return success;
    }

    // AutorizacaoSend myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AutorizacaoSend
    {
        public bool capture { get; set; }
        public string kind { get; set; }
        public string reference { get; set; }
        public string amount { get; set; }
        public string affiliation { get; set; }
        public int installments { get; set; }
        public string cardholderName { get; set; }
        public string cardNumber { get; set; }
        public int expirationMonth { get; set; }
        public int expirationYear { get; set; }
        public string securityCode { get; set; }
        public string softDescriptor { get; set; }
        public bool subscription { get; set; }
        public int origin { get; set; }
        public int distributorAffiliation { get; set; }
        public string brandTid { get; set; }
        public string storageCard { get; set; }
        public TransactionCredentials transactionCredentials { get; set; }
    }

    public class TransactionCredentials
    {
        public string credentialId { get; set; }
    }

    public class Link
    {
        public string method { get; set; }
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class AutorizacaoResult
    {
        public string reference { get; set; }
        public string tid { get; set; }
        public string nsu { get; set; }
        public string authorizationCode { get; set; }
        public string brandTid { get; set; }
        public DateTime dateTime { get; set; }
        public int amount { get; set; }
        public int installments { get; set; }
        public string cardBin { get; set; }
        public string last4 { get; set; }
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public List<Link> links { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Authorization
    {
        public DateTime dateTime { get; set; }
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public int affiliation { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string tid { get; set; }
        public string nsu { get; set; }
        public string kind { get; set; }
        public int amount { get; set; }
        public int installments { get; set; }
        public string cardBin { get; set; }
        public string last4 { get; set; }
        public string softDescriptor { get; set; }
        public int origin { get; set; }
        public bool subscription { get; set; }
        public Brand brand { get; set; }
    }

    public class Brand
    {
        public string name { get; set; }
        public string returnMessage { get; set; }
        public string returnCode { get; set; }
        public string brandTid { get; set; }
        public string authorizationCode { get; set; }
    }

    public class ConfirmResult
    {
        public DateTime requestDateTime { get; set; }
        public Authorization authorization { get; set; }
        public List<Link> links { get; set; }
    }






}