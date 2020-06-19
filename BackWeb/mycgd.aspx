<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mycgd.aspx.cs" Inherits="CommunityBuy.BackWeb.mycgd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    user：<asp:TextBox ID="txt_user" runat="server" MaxLength="32" Width="236px"></asp:TextBox>
        <br />
        <br />
        key： <asp:TextBox ID="txt_p" runat="server" MaxLength="32" Width="235px"></asp:TextBox>
        <br />
        <br />
    text：<asp:TextBox ID="IT_text" runat="server" Height="200px" Width="800px"></asp:TextBox>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_goto" runat="server" Font-Size="Large" Height="46px" OnClick="btn_goto_Click" Text="GOTO-Test" Width="196px" />
        <asp:Label ID="lb_Mes" runat="server" Font-Size="X-Large" ForeColor="Red" Text="Label"></asp:Label>
        <br />
    </div>
    </form>
</body>
</html>
