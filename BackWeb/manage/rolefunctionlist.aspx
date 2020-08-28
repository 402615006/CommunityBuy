<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rolefunctionlist.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.rolefunctionlist" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/RolmasChoice.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/layui/layui.all.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
</head>
<body data-pagecode="rolefunction">
    <form id="form1" data-tbname="rolefunction" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="rolefunctionlist" PageType="List" MainMenu="" SubMenu="" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="" Operate="" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li class="wherename" data-code="cname_where">角色名称：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="username" runat="server" /></li>
                </ul>
                <ul>
                    <li class="wherename" data-code="status_where">状态：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="sel_status" CssClass="wheresel" IsSearch="true" Width="80" SelType="Status" runat="server"></cc1:CDropDownList></li>
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
                    <AlternatingItemStyle BackColor="White" CssClass="List_tab_alter" />
                    <ItemStyle BackColor="#EFF3FB" CssClass="List_tab_tr" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="">
                            <HeaderTemplate>
                                <input id="cbAll" type="checkbox" onclick="CheckAll()" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%#Bind("roleid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="roleid" HeaderText="<span data-code='' >角色ID</span>" />
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <span data-code='cname_list'>角色名称</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a href='javascript:void(0);' onclick="openWindow('/manage/rolefunctionedit.aspx?id=<%#Eval("roleid") %>&type=detail')"><%#Eval("cname") %></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="status" HeaderText="<span data-code='status_list' >状态</span>" />
                        <asp:BoundColumn DataField="descr" HeaderText="<span data-code='descr_list' >描述</span>" />
                        <asp:BoundColumn DataField="ctime" HeaderText="<span data-code='ctime_list' ></span>" DataFormatString="{0:d}" SortExpression="rol_crt_time" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <div class="pagelist">
                    <webdiyer:AspNetPager ID="anp_top" runat="server" CssClass="paginator" CurrentPageButtonClass="cpb"
                        CustomInfoHTML="<c data-code='PageCount1' ></c>%PageCount%<c data-code='PageCount2' ></c>  %RecordCount%<c data-code='RecordCount' ></c>&nbsp;&nbsp;&nbsp;" AlwaysShow="True" FirstPageText="<c data-code='FirstPage' ></c>"
                        LastPageText="<c data-code='LastPage' ></c>" NextPageText="<c data-code='NextPage' ></c>" PageSize="30" PrevPageText="<c data-code='PrevPage' ></c>" ShowCustomInfoSection="Left"
                        ShowInputBox="Never" CustomInfoTextAlign="Right" LayoutType="Table" OnPageChanging="anp_top_PageChanging"
                        ShowBoxThreshold="300000" ShowNavigationToolTip="True" ShowPrevNext="False">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
