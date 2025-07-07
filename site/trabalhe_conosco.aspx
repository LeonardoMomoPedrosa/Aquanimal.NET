<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="trabalhe_conosco.aspx.cs" Inherits="trabalhe_conosco" %>

<asp:Content ContentPlaceHolderID="Content_data" runat="server">
    <script>
        function updateFileName() {
            var input = document.getElementById("<%= fileUpload.ClientID %>");
            var fileName = input.files[0] ? input.files[0].name : "Nenhum arquivo selecionado";
            document.getElementById("file-name").textContent = fileName;
        }
    </script>
    <style>
        .custom-file-upload {
            margin-top: 2px;
            width: 400px;
            display: inline-block;
            padding: 10px 20px;
            cursor: pointer;
            background-color: white;
            color: #fff;
            border-radius: 5px;
            font-family: sans-serif;
            font-size: 14px;
        }

            .custom-file-upload:hover {
                background-color: #0056b3;
            }

        #fileUpload {
            display: none;
        }

        #file-name {
            margin-left: 10px;
            font-style: italic;
        }
    </style>
    <div style="margin: 20px;">
        <div style="font-size: 150%; margin-bottom: 20px;">
            Trabalhe Conosco
        </div>
    </div>
    <div class="row" style="margin: 18px;">
        <div class="col-md-12">
            Se você gosta do mundo da Aquarofilia e busca de uma oportunidade de trabalho, essa é sua oportunidade!
        <form name="form" style="padding: 10px;" runat="server">
            <div>
                <label for="name-3434">Nome</label>
                <asp:TextBox ID="txtNome" placeholder="Digite seu nome" runat="server" class="form-control form-control-sm" Style="width: 400px;"></asp:TextBox>
            </div>
            <div class="u-form-email u-form-group u-label-none">
                <label for="email-3434" class="u-label">Email</label>
                <asp:TextBox ID="txtEmail" placeholder="Digite seu Email" runat="server" class="form-control form-control-sm" Style="width: 400px;"></asp:TextBox>
            </div>
            <div class="u-form-group u-form-message u-label-none">
                <label for="message-3434" class="u-label">Mensagem</label>
                <asp:TextBox TextMode="MultiLine" Columns="50" Rows="4" ID="txtMensagem" placeholder="Escreva seu interesse em trabalhar conosco" runat="server" class="form-control form-control-sm" Style="width: 400px;"></asp:TextBox>
                <!--textarea placeholder="Escreva seu interesse em trabalhar conosco" rows="4" cols="50" id="txtMensagem" name="message" class="form-control form-control-sm" style="width: 400px;"></!--textarea-->
            </div>
            <div class="u-align-left u-form-group u-form-submit u-label-none">
                <label class="custom-file-upload">
                    <asp:FileUpload ID="fileUpload" runat="server" onchange="updateFileName()" />
                    Selecionar currículo (PDF)
                </label>
                <br />
                <asp:Button ID="btnEnviar" runat="server" CssClass="btn btn-sm" Text="Enviar" OnClick="btnEnviar_Click" />
            </div>
            <asp:Label ID="lblStatus" runat="server" ForeColor="Green" />
        </form>
        </div>
    </div>
</asp:Content>
