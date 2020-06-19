<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DishesFeedRateList.aspx.cs" Inherits="CommunityBuy.BackWeb.DishesFeedRateList" %>

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
    <script type="text/javascript">
        function gourl(o) {
            ShowOpenpage('详情', 'DishesMateInfo.aspx?type=info&discode=' + $(o).attr("name")+'&stocode='+$(o).attr("stocode"), '90%', '100%', true, true);
        }
    </script>
</head>
<body data-pagecode="DishesFeedRate">
    <form id="form1" data-tbname="DishesFeedRate" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="DishesFeedRatelist" PageType="List" MainMenu="出品管理" SubMenu="菜品净料率" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li data-code="stocode_where" class="wherename">部门：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_stocode" Descr="部门" Width="140" SelType="Normal" CssClass="selstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_stocode_SelectedIndexChanged"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="Menu_where" class="wherename">仓库：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_Menu_list" Descr="仓库" Width="140" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="search_where" class="wherename">菜品检索：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" runat="server" data-code="search_placeholder" id="txt_disname" style="width: 180px;" />
                    </li>
                </ul>

            </div>
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
            <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" />
            <div class="righttable">
                <cc1:CustDataGrid ID="gv_list" runat="server" Width="100%" rules="none" CellPadding="4"
                    EnableViewState="true" AutoGenerateColumns="False" OnSortCommand="gv_list_SortCommand"
                    AllowSorting="True" GridLines="None" CssClass="List_tab" PagerID="GridPager1"
                    SeqNo="0" OnItemCreated="gv_list_ItemCreated" OnPreRender="gv_list_PreRender">
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <AlternatingItemStyle BackColor="#EFF3FB" CssClass="List_tab_alter" />
                    <ItemStyle BackColor="White" CssClass="List_tab_tr" />
                    <Columns>
                         <asp:TemplateColumn HeaderStyle-Width="180">
                            <HeaderTemplate>菜品名称</HeaderTemplate>
                            <ItemTemplate>
                                <a href='javascript:void(0);' id='info' name='<%# Eval("discode") %>' stocode='<%# Eval("stocode") %>' onclick="gourl(this)" style="color:dodgerblue;"><%#Eval("disname").ToString() %></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="disname" HeaderText="菜品名称" HeaderStyle-Width="180" />
                        <asp:BoundColumn DataField="disnuitname" HeaderText="单位" HeaderStyle-Width="180"/> 
                        <asp:BoundColumn DataField="warname" HeaderText="仓库" HeaderStyle-Width="180"/>
                        <asp:BoundColumn DataField="matname" HeaderText="物料名称" HeaderStyle-Width="180"/>
                        <asp:BoundColumn DataField="spec" HeaderText="规格" HeaderStyle-Width="180"/>
                        <asp:BoundColumn DataField="unitname" HeaderText="基本单位" HeaderStyle-Width="180"/>
                        <asp:BoundColumn DataField="jlnum" HeaderText="用量" HeaderStyle-Width="180"/>
                        <asp:BoundColumn DataField="jlvs" HeaderText="净料率" HeaderStyle-Width="180"/>
                        <asp:BoundColumn DataField="mlnum" HeaderText="用量" HeaderStyle-Width="180"/>
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <div class="pagelist">
                    <webdiyer:AspNetPager ID="anp_top" runat="server" CssClass="paginator" CurrentPageButtonClass="cpb"
                        CustomInfoHTML="<c data-code='PageCount1' >共</c>%PageCount%<c data-code='PageCount2' >页</c>  %RecordCount%<c data-code='RecordCount' >条记录</c>&nbsp;&nbsp;&nbsp;" AlwaysShow="True" FirstPageText="<c data-code='FirstPage' ></c>"
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
