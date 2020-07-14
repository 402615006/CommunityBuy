<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreList.aspx.cs" Inherits="CommunityBuy.BackWeb.StoreList" %>

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
    <script src="/js/layer/layer.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
    <script type="text/javascript">
        function gourl(url) {
            ShowOpenpage('详情', url, '100%', '100%', true, true);
        }
    </script>
</head>
<body data-pagecode="Store">
    <form id="form1" data-tbname="Store" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="Storelist" PageType="List" MainMenu="" SubMenu="商家门店信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="商家门店信息" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li data-code="buscode_where" class="wherename">所属商户：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_businfo" Descr="所属商户" Width="140" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="stocode_where" class="wherename">门店编号：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_stocode" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="cname_where" class="wherename">门店名称：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_cname" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="provinceid_where" class="wherename">所在省：</li>
                    <li>
                        <cc1:CDropDownList ID="ddl_provinceid" data-code="status_placeholder" Descr="省" CssClass="wheresel" TextType="Normal" runat="server" OnSelectedIndexChanged="ddl_provinceid_SelectedIndexChanged" AutoPostBack="true">
                        </cc1:CDropDownList>
                    </li>
                    <li>
                        <cc1:CDropDownList ID="ddl_cityid" data-code="status_placeholder" Descr="市" CssClass="wheresel" TextType="Normal" runat="server" OnSelectedIndexChanged="ddl_cityid_SelectedIndexChanged" AutoPostBack="true">
                        </cc1:CDropDownList></li>
                    <li>
                        <cc1:CDropDownList ID="ddl_areaid" data-code="status_placeholder" Descr="区域" CssClass="wheresel" TextType="Normal" runat="server">
                        </cc1:CDropDownList></li>
                </ul>
                <ul>
                    <li data-code="stoprincipal_where" class="wherename">负责人：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_stoprincipal" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="status_where" class="wherename">有效状态：</li>
                    <li class="wherevale">

                        <cc1:CDropDownList ID="ddl_status" data-code="status_placeholder" Descr="状态" CssClass="selstyle" TextType="Normal" runat="server" IsSearch="true" SelType="Status">
                        </cc1:CDropDownList>
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
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <input id="cbAll" type="checkbox" onclick="CheckAll()" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("stoid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="stocode" HeaderText="<span data-code='stocode_list' >门店编号</span>" SortExpression="stocode" />
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='cname_list'></span></HeaderTemplate>
                            <ItemTemplate>
                                <a href='javascript:void(0);' onclick="gourl('StoreDetail.aspx?stoid=<%#Eval("stoid").ToString() %>')"><%#Eval("cname").ToString() %></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="sname" HeaderText="<span data-code='sname_list' >门店简称</span>" />
                        <asp:BoundColumn DataField="bcode" HeaderText="<span data-code='bcode_list' >门店简码</span>" />
                        <asp:BoundColumn DataField="areaname" HeaderText="<span data-code='provinceid_list' >所在区域</span>" />
                        <asp:BoundColumn DataField="stoprincipal" HeaderText="<span data-code='stoprincipal_list' >负责人</span>" />
                        <asp:BoundColumn DataField="stoprincipaltel" HeaderText="<span data-code='stoprincipaltel_list' >负责人联系电话</span>" />
                        <asp:BoundColumn DataField="tel" HeaderText="<span data-code='tel_list' >门店电话</span>" />
                        <asp:BoundColumn DataField="statusname" HeaderText="<span data-code='status_list' >状态</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <div class="pagelist">
                    <webdiyer:AspNetPager ID="anp_top" runat="server" CssClass="paginator" CurrentPageButtonClass="cpb"
                        CustomInfoHTML="<c data-code='PageCount1' ></c>%PageCount%<c data-code='PageCount2' ></c>  %RecordCount%<c data-code='RecordCount' ></c>&nbsp;&nbsp;&nbsp;" AlwaysShow="True" FirstPageText="<c data-code='FirstPage' ></c>"
                        LastPageText="<c data-code='LastPage' ></c>" NextPageText="<c data-code='NextPage' ></c>" PageSize="50" PrevPageText="<c data-code='PrevPage' ></c>" ShowCustomInfoSection="Left"
                        ShowInputBox="Never" CustomInfoTextAlign="Right" LayoutType="Table" OnPageChanging="anp_top_PageChanging"
                        ShowBoxThreshold="300000" ShowNavigationToolTip="True" ShowPrevNext="False">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
