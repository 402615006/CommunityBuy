<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="repulse.aspx.cs" Inherits="CommunityBuy.BackWeb.repulse" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
</head>
<body data-pagecode="maincoupon">
    <form id="form1" runat="server">
        <cc1:CPathBar ID="pathBar" PageCode="maincoupon" PageType="Normal" MainMenu="" Visible="false" SubMenu="" runat="server"></cc1:CPathBar>
        <table>
            <tr>
                <td></td><td>&nbsp;</td>
            </tr>
            <tr>
                <td class="lefttd" data-code="reject_reason">驳回原因：</td>
                <td>
                    <cc1:ctextbox id="txt_reason" cssclass="reqtxtstyle" maxlength="64" isrequired="False" texttype="Normal" onblur="onblurCheck('txt_reason')" runat="server" placeholder="" Height="60px" TextMode="MultiLine" Width="300px"></cc1:ctextbox><span class="redmess" id="txt_reason_span"></span></td>
            </tr>            
            <tr>
                <td>
                    <input type="hidden" id="hidId" runat="server" /></td>
                <td>
                    <div id="btn_save" data-code="reject_btn" class="savebtn">驳回</div>
                    <asp:Button ID="btnrepulse" runat="server" style="display:none;" Text="" OnClick="btnrepulse_Click" />
                    <span id="errormessage" runat="server"></span>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            //保存按钮
            $("#btn_save").click(function () {
                if ($('#txt_reason').val().length > 64)
                {
                    laytips('#txt_reason', $('#txt_reason').attr('placeholder') + '！');
                    return;
                }
                $("#btnrepulse").trigger("click");
            });
        });
    </script>
</body>
</html>