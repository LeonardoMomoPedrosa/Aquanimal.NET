using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.AspNet.FriendlyUrls;
using eco.controllers;
using eco.database;
using eco.utils;
using System.Data;

public partial class showprod : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IList<string> segments = Request.GetFriendlyUrlSegments();

        String pid = Page.RouteData.Values["pid"].ToString();
        String name = Page.RouteData.Values["name"].ToString();

        int result = 0;

        if (!int.TryParse(pid, out result))
        {
            Response.Redirect("/site");
        }

        ProductController prodControl = new ProductController();

        DataSet dsProd = null;
        String notaGif = null;

        try
        {
            dsProd = prodControl.getProductInfo(int.Parse(pid));
            notaGif = prodControl.getNotaGif(int.Parse(pid));
        }
        finally
        {
            prodControl.CloseDb();
        }


        int idSubtipo = int.Parse(dsProd.Tables[0].Rows[0]["id_subtipo"].ToString());
        String nomeProd = (String)dsProd.Tables[0].Rows[0]["nome_new"];
        String photo = (String)dsProd.Tables[0].Rows[0]["nome_foto"];
        String promo = dsProd.Tables[0].Rows[0]["promocao"].ToString();
        Double precoAnt = (Double)dsProd.Tables[0].Rows[0]["precoant"];
        Double preco = (Double)dsProd.Tables[0].Rows[0]["preco"];
        Double lucro = (Double)dsProd.Tables[0].Rows[0]["lucro"];
        int minParcJuros = int.Parse(dsProd.Tables[0].Rows[0]["minParcJuros"].ToString());
        bool estoque = (bool)dsProd.Tables[0].Rows[0]["estoque"];
        String descricao = (String)dsProd.Tables[0].Rows[0]["descricao"];
        String metaName = (String)dsProd.Tables[0].Rows[0]["meta_name"];
        String metaKeys = (String)dsProd.Tables[0].Rows[0]["meta_keys"];

        /*if (EcoUtils.isWet(idSubtipo))
        {
            nomeProd = nomeProd.Split('-')[1];
        }*/

        Literal_nomeProd.Text = nomeProd;


        if (photo == null || photo.Length < 3)
        {
            photo = "/site/fotosup/produtos/semfoto.png";
        }
        else
        {
            photo = "/site/fotosup/produtos/" + photo;
        }
        Image_photo.ImageUrl = photo;
        
        if (promo.Contains("1"))
        {
            Panel_promo.Visible = true;
        }

        Image_nota.ImageUrl = notaGif;

        Literal_pid.Text = pid;
        

        if (precoAnt > 0)
        {
            precoAnt = precoAnt * (1 + lucro/100);
            Panel_precoAnt.Visible = true;
            Literal_precoAnt.Text = "R$ " + string.Format("{0:N2}", precoAnt);
        }

        Double precoFinal = preco * (lucro / 100 + 1);
        Literal_preco.Text = "R$ " + string.Format("{0:N2}", precoFinal);

        int minParc = 0;
        int parcMin = 0;

        if (precoFinal > 10)
        {
            if (minParcJuros < 4)
            {
                minParc = 3;
            } else
            {
                minParc = minParcJuros;
            }

            parcMin = (int)(precoFinal / 50);

            if (parcMin < minParc)
            {
                minParc = parcMin;
            }

            if (minParc > 1)
            {
                Literal_parc.Text = minParc+"x de R$ "+ string.Format("{0:N2}", precoFinal/minParc)+" sem juros.";
            }
        }

        Panel_addCart.Visible = estoque;
        Panel_waitlist.Visible = !estoque;
        Literal_descricao.Text = descricao.Replace("\n", "<br/>");

        String metaStr = "";
        metaStr = @"Peixes de alta linhagem e garantia! Na Aquanimal você encontra " + metaName + ". Aguardamos sua visita!";
        Page.MetaDescription = metaStr;
        Page.MetaKeywords = metaKeys;
        Page.Title = metaName + " | Aquanimal";
    }
}
