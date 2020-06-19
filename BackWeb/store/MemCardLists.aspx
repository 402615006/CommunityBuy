<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemCardLists.aspx.cs" Inherits="CommunityBuy.BackWeb.MemCardLists" %>

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
<body data-pagecode="MemCard">
    <form id="form1" data-tbname="MemCard" runat="server">
        <div class="fixtoph" style="height: 20px;">&nbsp;</div>
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="MemCardlist" PageType="List" MainMenu="" SubMenu="会员卡信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员卡信息" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
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
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("cardid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="cardCode" HeaderText="<span data-code='cardCode_list' >会员卡卡号</span>" />

                        <asp:BoundColumn DataField="mctname" HeaderText="<span data-code='cracode_list' >会员卡等级</span>" />

                        <asp:BoundColumn DataField="levelname" HeaderText="<span data-code='ctype_list' >会员卡类型</span>" />

                        <asp:BoundColumn DataField="validate" HeaderText="<span data-code='validate_list' >卡有效期</span>" />

                         <asp:BoundColumn DataField="accountPay" HeaderText="<span data-code='totalbalance_list' >可用余额</span>" />

                         <asp:BoundColumn DataField="score" HeaderText="<span data-code='invoice_list' >可用积分</span>" />

                  <%--      <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='totalbalance_list'>可用余额</span></HeaderTemplate>
                            <ItemTemplate><a href="#" onclick="showmemcardconsumptionList('<%#Eval("cardcode").ToString() %>')"><%#Eval("accountPay").ToString() %></a></ItemTemplate>
                        </asp:TemplateColumn>--%>
                    <%--    <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='invoice_list'>可用积分</span></HeaderTemplate>
                            <ItemTemplate><a href="#" onclick="showmemintegralList('<%#Eval("cardcode").ToString() %>')"><%#Eval("score").ToString() %></a></ItemTemplate>
                        </asp:TemplateColumn>--%>

                        <asp:BoundColumn DataField="statusname" HeaderText="<span data-code='status_list' >状态</span>" />


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
