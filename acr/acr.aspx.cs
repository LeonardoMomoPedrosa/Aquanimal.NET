using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;

public partial class acr_acr : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
    protected void cobrar(object sender, EventArgs args)
    {
        Button chargeButton = (Button)sender;
        Label label = (Label)chargeButton.Parent.FindControl("LabelResult");
        int orderId = int.Parse(chargeButton.CommandArgument);

        processa(true, orderId, ref label);
    }

    protected void cobrarShip(object sender, EventArgs args)
    {
        Button chargeButton = (Button)sender;
        Label label = (Label)chargeButton.Parent.FindControl("LabelResult");
        int orderId = int.Parse(chargeButton.CommandArgument);

        processa(true, orderId, ref label);
    }

    private void processa(bool freteInd, int orderId, ref Label label)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        OrderController orderController = new OrderController();

        var freteLbl = freteInd ? "FRETE" : "";

        ///AUTORIZACAO
        try
        {
            var response = processaAutorizacao(freteInd, orderId, ref orderController);
            string outp = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                AutorizacaoResult res = JsonConvert.DeserializeObject<AutorizacaoResult>(outp);
                label.Text = res.authorizationCode + " - " + res.returnMessage;
                if (freteInd)
                {
                    orderController.SalvaTRXShip(orderId,
                                                freteLbl + " AUTORIZADO",
                                                res.returnMessage,
                                                res.tid,
                                                res.authorizationCode,
                                                1);
                }
                else
                {
                    orderController.SalvaTRX(orderId,
                            freteLbl + " AUTORIZADO",
                            res.returnMessage,
                            res.tid,
                            res.authorizationCode,
                            1);
                }
            }
            else
            {
                label.Text = response.StatusCode + " - " + response.ReasonPhrase + " - " + outp;
                if (freteInd)
                {
                    orderController.SalvaTRXShip(orderId,
                                                freteLbl + " ERRO " + response.StatusCode,
                                                response.ReasonPhrase,
                                                "",
                                                "",
                                                1);
                }
                else
                {
                    orderController.SalvaTRX(orderId,
                                                freteLbl + " ERRO " + response.StatusCode,
                                                response.ReasonPhrase,
                                                "",
                                                "",
                                                1);
                }
            }
        }
        catch (Exception ex)
        {
            label.Text = ex.Message + ex.StackTrace;
            if (freteInd)
            {
                orderController.SalvaTRXShip(orderId,
                                freteLbl + " ERRO Exception",
                                ex.Message,
                                "",
                                "",
                                1);
            }
            else
            {
                orderController.SalvaTRX(orderId,
                freteLbl + " ERRO Exception",
                ex.Message,
                "",
                "",
                1);
            }
        }

        //CONFIRMACAO

    }

    private HttpResponseMessage processaAutorizacao(bool freteInd, int orderId, ref OrderController orderController)
    {
        /////////Autorização
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        DataSet orderDs;

        try
        {
            orderDs = orderController.getOrderInfo(orderId);
        }
        finally
        {
            orderController.CloseDb();
        }

        var idAmt = freteInd ? "frete" : "amt";
        var dataRow = orderDs.Tables[0].Rows[0];
        var exp = dataRow["val"].ToString().Split('/');
        var cc = dataRow["aa"].ToString().Split('-');
        var amt = ((Double)dataRow[idAmt] * 100).ToString();

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://sandbox-erede.useredecloud.com.br/v1/transactions");

        string username = "31097197";
        string password = "7cbcb495e0bf4347a7c3bbb5256cdc9e";
        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

        request.Headers.Add("Authorization", "Basic " + credentials);
        var autSend = new AutorizacaoSend
        {
            capture = false,
            kind = "credit",
            reference = freteInd ? "S" + orderId : orderId + "",
            amount = amt,//10,00 = 1000
            installments = freteInd ? 1 : int.Parse(dataRow["parc"] + ""),
            cardholderName = dataRow["nome"] + "",
            cardNumber = cc[0],
            expirationMonth = int.Parse(exp[0]),
            expirationYear = int.Parse(exp[1]),
            securityCode = cc[1],
            softDescriptor = freteInd ? "Frete Aquanimal" : "Venda Aquanimal",
            subscription = false,
            origin = 1,
            distributorAffiliation = 31097197,
            storageCard = "0",
            transactionCredentials = new TransactionCredentials { credentialId = "01" }
        };

        string jsonString = JsonConvert.SerializeObject(autSend, Formatting.None);
        request.Content = new StringContent(jsonString, null, "application/json");

        var response = client.SendAsync(request).Result;

        return response;
    }

    public class ProcessaResultInfo
    {
        public int Step { get; set; }
        public HttpResponseMessage responseMessage { get; set; }
    }

    // AutorizacaoSend myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AutorizacaoSend
    {
        public bool capture { get; set; }
        public string kind { get; set; }
        public string reference { get; set; }
        public string amount { get; set; }
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




}