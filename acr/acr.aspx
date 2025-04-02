<%@ Page Language="C#" AutoEventWireup="true" CodeFile="acr.aspx.cs" Inherits="acr_acr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aquanimal ACR</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" rel="stylesheet" />
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="sm1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel runat="server" ID="up1">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            Pedido 19876
                        </div>
                        <div class="col-md-4">
                            <asp:Button CssClass="btn btn-sm" runat="server" ID="Button_charge" Text="Cobrar" CommandArgument="19876" OnClick="cobrar" />
                        </div>
                        <div class="col-md-4">
                            <asp:Label runat="server" ID="label_19876"></asp:Label>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
