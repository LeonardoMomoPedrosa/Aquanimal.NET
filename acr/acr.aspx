<%@ Page Language="C#" AutoEventWireup="true" CodeFile="acr.aspx.cs" Inherits="acr_acr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aquanimal ACR</title>
    <!-- Latest compiled and minified JavaScript -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
    <style>
        .loading-container {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
        }

        .loading-spinner {
            padding: 15px;
            background: rgba(0, 0, 0, 0.7);
            color: white;
            font-size: 18px;
            border-radius: 8px;
        }
    </style>

</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="sm1" runat="server">
            </asp:ScriptManager>
            <h4>Pedidos com cartão de crédito</h4>
            <a href="https://34.202.215.109:8058/"><- Voltar</a>
            <hr />
            <asp:UpdateProgress ID="upp1" AssociatedUpdatePanelID="up1" runat="server">
                <ProgressTemplate>
                    <div class="loading-container">
                        <div class="loading-spinner">Aguarde Processando...</div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:UpdatePanel runat="server" ID="up1">
                <ContentTemplate>
                    <asp:Label ID="Label_Debug" runat="server"></asp:Label>
                    <asp:Repeater ID="Repeater_orders" runat="server">
                        <ItemTemplate>
                            <div class="row" style='<%# (((int)Eval("parc")) > 1) ? "" : "display:none;" %>'>
                                <div class="col-md-6">
                                    <b>Pedido <%#Eval("PKId")%></b> <%#Eval("nome")%> <%#Eval("cidade")%>/<%#Eval("estado")%> <b>Total R$ <%#Eval("amt")%></b> Parcs: <%#Eval("parc")%> <b>Frete: R$ <%#Eval("frete")%></b>
                                </div>
                                <div class="col-md-6">
                                    <div class="row border">
                                        <div class="col-md-4">
                                            <asp:Button CssClass="btn btn-sm btn-primary" runat="server" Enabled='<%#((String)Eval("REDESTATUS_SHIP")).Length < 2 || ((String)Eval("REDESTATUS_SHIP")).Contains("ERRO")%>' ID="Button_frete" Text="Cobrar Frete" CommandArgument='<%#Eval("PKId")%>' OnClick="cobrarShip" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Label runat="server" ID='LabelResultShip' ForeColor='<%#GetStatusColor(Eval("REDESTATUS_SHIP").ToString())%>'>Ultimo Status Frete: <%#Eval("REDESTATUS_SHIP")%></asp:Label><br />
                                        </div>
                                    </div>
                                    <div class="row border">
                                        <div class="col-md-4">
                                            <asp:Button CssClass="btn btn-sm mt-1 btn-primary" runat="server" Enabled='<%#((String)Eval("REDESTATUS")).Length < 2 || ((String)Eval("REDESTATUS")).Contains("ERRO")%>' ID="Button_charge" Text="Cobrar Pedido" CommandArgument='<%#Eval("PKId")%>' OnClick="cobrar" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Label runat="server" ForeColor='<%#GetStatusColor(Eval("REDESTATUS").ToString())%>' ID='LabelResult'>Ultimo Status Pedido: <%#Eval("REDESTATUS")%></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <hr class="mt-1" />
                            </div>
                            <div class="row" style='<%# (((int)Eval("parc")) == 1) ? "" : "display:none;" %>'>
                                <div class="col-md-6">
                                    <b>Pedido <%#Eval("PKId")%></b> <%#Eval("nome")%> <%#Eval("cidade")%>/<%#Eval("estado")%> <b>Total + Frete R$ <%#(double)Eval("amt") + (double)Eval("frete")%></b> Parcs: <%#Eval("parc")%>
                                </div>
                                <div class="col-md-6">
                                    <div class="row border">
                                        <div class="col-md-4">
                                            <asp:Button CssClass="btn btn-sm mt-1 btn-primary" runat="server" Enabled='<%#((String)Eval("REDESTATUS")).Length < 2 || ((String)Eval("REDESTATUS")).Contains("ERRO")%>' ID="Button2" Text="Cobrar Pedido + Frete" CommandArgument='<%#Eval("PKId")%>' OnClick="cobrarFull" />
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Label runat="server" ForeColor='<%#GetStatusColor(Eval("REDESTATUS").ToString())%>' ID='LabelResultFull'>Ultimo Status Pedido: <%#Eval("REDESTATUS")%></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <hr class="mt-1" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
