<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memcardconsumptionLists.aspx.cs" Inherits="CommunityBuy.BackWeb.memcardconsumptionLists" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
</head>
<body data-pagecode="memcardconsumption">
    <form id="form1" data-tbname="memcardconsumption" runat="server">
        <div class="fixtoph" style="height: 20px;">&nbsp;</div>
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memcardconsumptionlist" PageType="Normal" MainMenu="" SubMenu="会员卡消费表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员卡消费表" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <span data-code="cardid_where" style="margin-left: 20px;">会员卡号：</span>
                <cc1:CDropDownList ID="cardcode" SelType="Normal" CssClass="selstyle" runat="server" Style="width: 200px;"></cc1:CDropDownList>&nbsp;&nbsp;
                <%--<cc1:CDropDownList ID="paytype" SelType="Normal" CssClass="selstyle" runat="server" Style="width: 100px;"></cc1:CDropDownList>--%>
                <asp:Button ID="Button1" runat="server" Text="搜索" Style="margin-left: 10px; width: 80px; height: 35px; background-color: #3E4460; color: white" OnClick="Button1_Click" />
            </div>
            <div class="hintmess" id="sp_showmes" runat="server" style="display: none">&nbsp;</div>
            <div style="display: none">
                <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" />
            </div>
            <div class="righttable">
                <cc1:CustDataGrid ID="gv_list" runat="server" Width="100%" rules="none" CellPadding="4"
                    EnableViewState="true" AutoGenerateColumns="False" OnSortCommand="gv_list_SortCommand"
                    AllowSorting="True" GridLines="None" CssClass="List_tab" PagerID="GridPager1"
                    SeqNo="0">
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <AlternatingItemStyle BackColor="#EFF3FB" CssClass="List_tab_alter" />
                    <ItemStyle BackColor="White" CssClass="List_tab_tr" />
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <input id="cbAll" type="checkbox" onclick="CheckAll()" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("consumid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="memcode" HeaderText="<span data-code='cardid_list' >会员卡号</span>" />

                        <asp:BoundColumn DataField="income" HeaderText="<span data-code='income_list' >收入</span>" />

                        <asp:BoundColumn DataField="expend" HeaderText="<span data-code='expend_list' >支出</span>" />

                        <asp:BoundColumn DataField="balancetotal" HeaderText="<span data-code='balancetotal_list' >可用余额</span>" />

                        <asp:BoundColumn DataField="ctime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="<span data-code='ctime_list' >创建时间</span>" />

                        <asp:BoundColumn DataField="remark" HeaderText="<span data-code='remark_list' >备注</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <div class="pagelist">
                    <webdiyer:AspNetPager ID="anp_top" runat="server" CssClass="paginator" CurrentPageButtonClass="cpb"
                        CustomInfoHTML="<c data-code='PageCount1' ></c>%PageCount%<c data-code='PageCount2' ></c>  %RecordCount%<c data-code='RecordCount' ></c>&nbsp;&nbsp;&nbsp;" AlwaysShow="True" FirstPageText="<c data-code='FirstPage' ></c>"
                        LastPageText="<c data-code='LastPage' ></c>" NextPageText="<c data-code='NextPage' ></c>" PageSize="10" PrevPageText="<c data-code='PrevPage' ></c>" ShowCustomInfoSection="Left"
                        ShowInputBox="Never" CustomInfoTextAlign="Right" LayoutType="Table" OnPageChanging="anp_top_PageChanging"
                        ShowBoxThreshold="300000" ShowNavigationToolTip="True" ShowPrevNext="False">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
