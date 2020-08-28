<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membersList.aspx.cs" Inherits="CommunityBuy.BackWeb.membersList" %>

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
    <script>
        $(function () {
            getprovice();
        })
    </script>
    <style type="text/css">
        .wherename {
            width:80px;
        }
    </style>
</head>
<body data-pagecode="members">
    <form id="form1" data-tbname="members" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memberslist" PageType="List" MainMenu="" SubMenu="会员信息表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员信息表" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li class="wherename">会员编号：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_memcode" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="cname_where" class="wherename">姓名：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_cname" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="mobile_where" class="wherename">手机号码：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_mobile" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="IDNO_where" class="wherename">证件号码：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_IDNO" runat="server" /></li>
                </ul>
            </div>
            <div class="hintmess" id="sp_showmes" runat="server">请输入条件进行查询&nbsp;</div>
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
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("memcode") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="memcode" HeaderText="<span data-code='memcode_list' >会员编号</span>" SortExpression="memcode" />
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='cname_list'></span></HeaderTemplate>
                            <ItemTemplate><a href="membersDetail.aspx?id=<%#Eval("memcode").ToString() %>"><%#Eval("cname").ToString() %></a></ItemTemplate>
                        </asp:TemplateColumn>
                        <%--<asp:BoundColumn DataField="bigcustomername" HeaderText="<span data-code='bigcustomer_list' >性别</span>" />--%>
                        <asp:BoundColumn DataField="sexname" HeaderText="<span data-code='sex_list' >性别</span>" />
                        <asp:BoundColumn DataField="mobile" HeaderText="<span data-code='mobile_list' >手机号码</span>" />
                        <asp:BoundColumn DataField="idtypeName" HeaderText="<span data-code='idtype_list' >证件类型</span>" />
                        <asp:BoundColumn DataField="IDNO" HeaderText="<span data-code='IDNO_list' >证件号码</span>" />
                        <asp:BoundColumn DataField="areaname" HeaderText="<span data-code='areaid_list' >所属区域</span>" />
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='totalcard'>卡数量</span></HeaderTemplate>
                            <ItemTemplate><a href="membersDetail.aspx?id=<%#Eval("memcode").ToString() %>"></a></ItemTemplate>
                        </asp:TemplateColumn>
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