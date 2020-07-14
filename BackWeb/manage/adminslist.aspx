<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminslist.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.adminslist" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <style type="text/css">
        .wherename {
            width: 80px;
        }
    </style>
</head>
<body data-pagecode="admins">
    <form id="form1" data-tbname="admins" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="adminslist" PageType="List" MainMenu="" SubMenu="" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="" Operate="" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li class="wherename">用户名：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_uname" runat="server" /></li>
                </ul>
                <ul>
                    <li class="wherename" data-code="realname_where">姓名：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_realname" runat="server" /></li>
                </ul>
                <ul>
                    <li class="wherename" data-code="umobile_where">手机号：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_umobile" runat="server" /></li>
                </ul>
                <ul>
                    <li class="wherename" data-code="status_where">状态：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_status" CssClass="wheresel" SelType="Status" IsSearch="true" runat="server" IsNotNull="True"></cc1:CDropDownList></li>
                </ul>
                <ul>
                    <li class="wherename" data-code="rolename_where">所属角色：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_rolename" CssClass="wheresel" runat="server"></cc1:CDropDownList></li>
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
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("userid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="uname" HeaderText="<span>账号</span>"  SortExpression="uname"/>
                        <asp:BoundColumn DataField="realname" HeaderText="<span >姓名</span>"  SortExpression="realname"/>
                        <asp:BoundColumn DataField="remark" HeaderText="<span>备注</span>"  />
                        <asp:BoundColumn DataField="umobile" HeaderText="<span >手机号</span>" />
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <span data-code="StatusList">状态</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("status").ToString()=="1"?"<span>有效</span>":"<span>无效</span>" %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
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
