<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showmsgList.aspx.cs" Inherits="CommunityBuy.BackWeb.store.showmsgList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/liststyle.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../js/xmlhelper.js"></script>
    <script src="../js/layer/layer.js"></script>
    <script src="../js/default.js" type="text/javascript"></script>
    <script src="../js/listeditjs.js" type="text/javascript"></script>
    <script src="../js/layerhelper.js"></script>
     <script type="text/javascript">
         function entermob(obj) {
             var phoneNum = $(obj).val().replace(/\s+/g, "");
             if (phoneNum.length > 7) {
                 $(obj).val(phoneNum.substring(0, 3) + " " + phoneNum.substring(3, 7) + " " + phoneNum.substring(7, phoneNum.length));
                 return;
             }
             else if (phoneNum.length > 3) {
                 $(obj).val(phoneNum.substring(0, 3) + " " + phoneNum.substring(3, phoneNum.length));
                 return;
             }
         }
         function freego() {
             $('#Free_btn').hide();
             return true;
         }
    </script>
</head>
<body data-pagecode="showmsg">
    <form id="form1" data-tbname="showmsg" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="showmsg" PageType="List" MainMenu="" SubMenu="短信发送查询" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="短信发送查询" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li class="wherename">用户手机号：</li>
                    <li class="wherevale">
                        <cc1:CTextBox ID="txt_cmobile" data-code="mobile_placeholder" CssClass="txtstyle" MaxLength="13" IsRequired="False" TextType="Mobile" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="#FF3300" Height="40px" onkeyup="entermob(this);" Width="240px"></cc1:CTextBox>
                    </li>
                </ul>
            </div>
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
            <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" />
            <div class="righttable">
                <cc1:CustDataGrid ID="gv_list" runat="server" Width="100%" rules="none" CellPadding="4"
                    EnableViewState="true" AutoGenerateColumns="False" OnSortCommand="gv_list_SortCommand"
                    AllowSorting="True" GridLines="None" CssClass="List_tab" PagerID="GridPager1"
                    SeqNo="0">
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <AlternatingItemStyle BackColor="#EFF3FB" CssClass="List_tab_alter" />
                    <ItemStyle BackColor="White" CssClass="List_tab_tr" />
                    <Columns>
                        <asp:BoundColumn DataField="mobile" HeaderText="手机号码" />
                        <asp:BoundColumn DataField="sendtime" HeaderText="发送时间" />
                        <asp:BoundColumn DataField="smscontent" HeaderText="验证码" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
               
            </div>
        </div>
    </form>
</body>
</html>
