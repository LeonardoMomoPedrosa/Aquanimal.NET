using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

public partial class acr_acr : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void cobrar(object sender, EventArgs args)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        Button chargeButton = (Button)sender;
        Label label = (Label)chargeButton.Parent.FindControl("label_19876");

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
            reference = "OS124",
            amount = "1000",//10,00 = 1000
            installments = 1,
            cardholderName = "John Snow",
            cardNumber = "5448280000000007",
            expirationMonth = 1,
            expirationYear = 2028,
            securityCode = "123",
            softDescriptor = "Venda Aquanimal",
            subscription = false,
            origin = 1,
            distributorAffiliation = 31097197,
            storageCard = "0",
            transactionCredentials = new TransactionCredentials { credentialId = "01" }
        };

        string jsonString = JsonConvert.SerializeObject(autSend, Formatting.None);
        request.Content = new StringContent(jsonString, null, "application/json");

        try
        {
            var response = client.SendAsync(request).Result;
            string outp = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                AutorizacaoResult res = JsonConvert.DeserializeObject<AutorizacaoResult>(outp);
                label.Text = res.authorizationCode + " - " + res.returnMessage;
            } else
            {
                label.Text = response.StatusCode + " - " + response.ReasonPhrase + " - " + outp;
            }
        }
        catch (Exception ex)
        {
            label.Text = ex.Message + ex.StackTrace;
        }
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