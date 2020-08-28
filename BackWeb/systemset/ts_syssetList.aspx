<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ts_syssetList.aspx.cs" Inherits="CommunityBuy.BackWeb.ts_syssetList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
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
    <script src="/js/layui/layui.all.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
</head>
<body data-pagecode="ts_sysset">
    <form id="form1" data-tbname="ts_sysset" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="ts_syssetlist" PageType="List" MainMenu="" SubMenu="系统设置" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="系统设置" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li data-code="key_where" class="wherename">键：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_key" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="val_where" class="wherename">值：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_val" runat="server" /></li>
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
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <input id="cbAll" type="checkbox" onclick="CheckAll()" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("setid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>



                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='key_list'></span></HeaderTemplate>
                            <ItemTemplate><a href="ts_syssetDetail.aspx?setid=<%#Eval("setid").ToString() %>"><%#Eval("key").ToString() %></a></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="val" HeaderText="<span data-code='val_list' >值</span>" />

                        <asp:BoundColumn DataField="descr" HeaderText="<span data-code='descr_list' >描述</span>" />

                        <asp:BoundColumn DataField="ctime" DataFormatString="{0:yyyy-MM-dd }"  HeaderText="<span data-code='ctime_list'  >创建时间</span>" />


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
