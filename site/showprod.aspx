<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="showprod.aspx.cs" Inherits="showprod" %>

<asp:Content ContentPlaceHolderID="Content_data" runat="server">
    <div style="margin: 20px;">
        <div style="font-size: 150%; margin-bottom: 20px;">
            <asp:Literal runat="server" ID="Literal_nomeProd"></asp:Literal>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div style="text-align: center; max-width: 320px;">
                <div style="position: relative;">
                    <asp:Image runat="server" ID="Image_photo" Style="max-width: 240px;margin-bottom: 6px;" />
                    <asp:Panel ID="Panel_promo" runat="server" Visible="false">
                        <!--span class="saleflag">Promoção</!--span-->
                        <span class="saleflagblack" style="margin-left: 45px;">BLACK DECEMBER</span>
                    </asp:Panel>
                    <blockquote runat="server" id="insta_quote" class="instagram-media" data-instgrm-captioned data-instgrm-permalink="" data-instgrm-version="14" style="background: #FFF; border: 0; border-radius: 3px; box-shadow: 0 0 1px 0 rgba(0,0,0,0.5),0 1px 10px 0 rgba(0,0,0,0.15); margin: 1px; max-width: 240px; min-width: 120px; padding: 0; width: 240px; width: -webkit-calc(80% - 2px); width: calc(80% - 2px);"></blockquote>
                    <script async src="https://instagram.com/static/bundles/es6/EmbedSDK.js/47c7ec92d91e.js"></script>
                </div>
            </div>
        </div>
        <div class="col-md-8" style="padding-left: 5%;">
            <div style="font-size: 75%; color: gray;">
                C&oacute;d.
                <asp:Literal runat="server" ID="Literal_pid"></asp:Literal>
            </div>
            <div>
                <asp:Image runat="server" ID="Image_nota" />
            </div>
            <div>
                <asp:Panel ID="Panel_precoAnt" runat="server" Visible="false">
                    <span style="font-weight: normal; color: brown; text-decoration: line-through; margin-right: 8px;">
                        <asp:Literal ID="Literal_precoAnt" runat="server"></asp:Literal>
                    </span>
                </asp:Panel>
                <span style="font-weight: bold;">
                    <asp:Literal runat="server" ID="Literal_preco"></asp:Literal></span>
                <div class="listaprodparc">
                    <asp:Literal runat="server" ID="Literal_parc"></asp:Literal>
                </div>
                <asp:Panel runat="server" ID="Panel_addCart" Visible="true">
                    <form method="post" action="/site/produtos/addcart.asp">
                        <input type="hidden" name="idprod" value="<%=Page.RouteData.Values["pid"] %>">
                        <input type="hidden" name="qtd" value="1">
                        <input type="submit" class="btn btn-primary btnprod btn-sm" value="Comprar">
                    </form>
                    <div style="font-size: 11px;">O estoque refere-se at&eacute; a &uacute;ltima atualiza&ccedil;&atilde;o do site, estando sujeito a confirma&ccedil;&atilde;o.</div>
                </asp:Panel>
                <asp:Panel runat="server" ID="Panel_waitlist" Visible="true">
                    <a href="aviseme_modal.asp" class="btn btn-sm btn-default btnprod" data-toggle="modal" data-target="#myModal">Avise-me</a>
                    <div id="myModal" class="modal fade">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <!-- Content will be loaded here from "remote.php" file -->
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div style="width: 100%; height: 1px; background-color: gray; margin-top: 10px;"></div>
            </div>
            <div>
                <asp:Literal runat="server" ID="Literal_descricao" Mode="Transform"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
