<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dishesList.aspx.cs" Inherits="CommunityBuy.BackWeb.dishesList" %>

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
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
    <script type="text/javascript">
        function gourl(o) {
            ShowOpenpage('详情', 'dishesedit.aspx?type=info&discode=' + $(o).attr("id") + '&stocode=' + $("#ddl_stocode").val(), '90%', '100%', true, true);
        }
    </script>
    <style>
        /* 回收站 */
        .recyble {
            color: #747474;
            font-size: 18px;
            margin-bottom: 18px;
        }

        .btn-recyble {
            width: 25px;
            height: 25px;
        }

        .recyble .btn-recyble.on {
            background: url('/images/on.png') no-repeat 0 0;
        }

        #ToolBar1_ToolBarMenu {
            height: auto;
            min-height: 40px;
        }

        .righttable1 {
            width: 98%;
            margin: 0 auto;
            position: relative;
            padding-bottom: 40px;
            min-height: 260px;
            z-index: 9;
        }
    </style>
</head>
<body data-pagecode="dishes">
    <form id="form1" data-tbname="dishes" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="disheslist" PageType="List" MainMenu="出品管理" SubMenu="出品管理" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="出品管理" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">

                <ul>
                    <li data-code="" class="wherename">菜品分类：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_selDisheType" Descr="菜品大分类" Width="140" SelType="Normal" CssClass="selstyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_selDisheType_SelectedIndexChanged"></cc1:CDropDownList>
                    </li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_sel_dishetypetwo" Descr="菜品小分类" Width="140" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="" class="wherename">状态：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_status" Descr="状态" Width="140" SelType="Normal" CssClass="selstyle" runat="server">
                            <asp:ListItem Value="1">有效</asp:ListItem>
                            <asp:ListItem Value="0">无效</asp:ListItem>
                            <asp:ListItem Value="-1" Selected>全部</asp:ListItem>
                        </cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="" class="wherename">菜品检索：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" runat="server" data-code="search_placeholder" id="txt_disname" style="width: 180px;" />
                    </li>
                </ul>
 
            </div>
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
            <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" style="height: auto; min-height: 80px;" />
            <div class="righttable1">
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
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("discode") %>'  runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="discode" HeaderText="菜品编号" />
                        <asp:TemplateColumn>
                            <HeaderTemplate>菜品名称</HeaderTemplate>
                            <ItemTemplate>
                                <a href='javascript:void(0);' id='<%# Eval("discode") %>' stocode='<%# Eval("stocode") %>' onclick="gourl(this)" style="color: dodgerblue;"><%#Eval("disname").ToString() %></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="TypeName" HeaderText="菜品类别" />
                        <asp:BoundColumn DataField="unit" HeaderText="单位" />
                        <asp:BoundColumn DataField="costprice" HeaderText="成本价" />
                        <asp:BoundColumn DataField="price" HeaderText="售价" />
                        <asp:BoundColumn DataField="statusname" HeaderText="状态" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidWhere1" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="discodes" type="hidden" runat="server" />
                 <input id="stocode" type="hidden" runat="server" />
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
