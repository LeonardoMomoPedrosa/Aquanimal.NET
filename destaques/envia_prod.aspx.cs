using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class destaques_envia_prod : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Files.Count > 0)
        {
            HttpPostedFile uploadedFile = Request.Files[0];

            if (uploadedFile != null && uploadedFile.ContentLength > 0)
            {
                string pkid = Request.QueryString["pkid"];
                string filename = Path.GetFileName(uploadedFile.FileName); 
                string folderPath = Server.MapPath("../site/fotosup");

                // Salvar o arquivo
                string filePath = Path.Combine(folderPath, Path.GetFileName(uploadedFile.FileName));
                uploadedFile.SaveAs(filePath);
                Response.Redirect("insere_foto.asp?pkid=" + pkid + "&foto=" + filename);
            }
            else
            {
                Response.Write("Erro");
            }
        }
    }
}