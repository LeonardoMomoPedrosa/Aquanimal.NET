using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.IO;

public partial class trabalhe_conosco : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        if (!fileUpload.HasFile || Path.GetExtension(fileUpload.FileName).ToLower() != ".pdf")
        {
            lblStatus.Text = "Por favor, envie um arquivo PDF.";
            lblStatus.ForeColor = System.Drawing.Color.Red;
            return;
        }

        try
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string mensagem = txtMensagem.Text.Trim();

            // Configurar o e-mail
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("seu@email.com");
            mail.To.Add("gerente@email.com");
            mail.Subject = "Novo Currículo Recebido";
            mail.Body = String.Format("Nome: {0}\nEmail: {1}\nMensagem:\n{2}", nome, email, mensagem);

            // Anexar o currículo
            mail.Attachments.Add(new Attachment(fileUpload.PostedFile.InputStream, fileUpload.FileName, "application/pdf"));

            // Enviar o e-mail
            SmtpClient smtp = new SmtpClient("smtp.seuservidor.com", 587);
            smtp.Credentials = new NetworkCredential("seu@email.com", "sua_senha");
            smtp.EnableSsl = true;
            smtp.Send(mail);

            lblStatus.Text = "Currículo enviado com sucesso!";
            lblStatus.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Erro ao enviar: " + ex.Message;
            lblStatus.ForeColor = System.Drawing.Color.Red;
        }
    }
}