using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class site_produtos_qry_insere_foto2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Files.Count > 0)
        {
            HttpPostedFile uploadedFile = Request.Files["arquivo1"];

            if (uploadedFile != null && uploadedFile.ContentLength > 0)
            {
                string pkid = Request.QueryString["pkid"];
                string filename = Path.GetFileName(uploadedFile.FileName);
                string folderPath = Server.MapPath("../fotosup/produtos");

                // Salvar o arquivo
                string filePath = Path.Combine(folderPath, Path.GetFileName(uploadedFile.FileName));
                uploadedFile.SaveAs(filePath);
                Response.Redirect("qry_altera_foto.asp?pkid=" + pkid + "&nome_foto=" + filename + "&fotog=&foto2=&foto2g=&foto3=&foto3g=");
            } else
            {
                Response.Write("Erro");
            }
        }
    }
}