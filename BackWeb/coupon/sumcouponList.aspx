<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sumcouponList.aspx.cs" Inherits="CommunityBuy.BackWeb.sumcouponList" %>

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
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
</head>
<body data-pagecode="sumcoupon">
    <form id="form1" data-tbname="sumcoupon" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="sumcouponlist" PageType="List" MainMenu="" SubMenu="优惠券活动" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="优惠券活动" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <%--<ul>
                    <li data-code="stocode_where" class="wherename">所属门店：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_stocode" CssClass="wheresel" SelType="Normal" runat="server"></cc1:CDropDownList></li>
                </ul>--%>
                <ul>
                    <li data-code="cname_where" class="wherename">活动名称：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_cname" runat="server" /></li>
                </ul>
                <%--<ul>
                    <li data-code="btime_where" class="wherename">活动有效期：</li>
                    <li style="width: 200px;">
                        <input type="text" class="datetxt" id="txt_btime" onfocus="ShowShortDate();" runat="server" />&nbsp;-&nbsp;<input type="text" class="datetxt" id="txt_etime" onfocus="ShowShortDate();" runat="server" /></li>
                </ul>--%>
                <ul>
                    <li data-code="ctype_where" class="wherename">优惠券类型：</li>
                    <li class="wherevale" style="width: 200px;">
                        <cc1:CDropDownList ID="ddl_ctype" Width="80" IsSearch="true" SelType="CouponFirstType" runat="server"></cc1:CDropDownList></li>
                </ul>
                <ul>
                    <li data-code="initype_where" class="wherename">发起类型：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_initype" CssClass="wheresel" SelType="CouponIniType" IsSearch="true" runat="server"></cc1:CDropDownList></li>
                </ul>
                <ul>
                    <li data-code="status_where" class="wherename">状态：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_zt" CssClass="wheresel" Width="80" SelType="Normal" IsSearch="false" runat="server"></cc1:CDropDownList></li>
                </ul>
                <ul>
                    <li data-code="audstatus_where" class="wherename">审核状态：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_audstatus" CssClass="wheresel" Width="80" SelType="AuditStatus" IsSearch="true" runat="server"></cc1:CDropDownList></li>
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
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("sumid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="cname" HeaderText="<span data-code='cname_list' >活动名称</span>">
                            <ItemTemplate>
                                <a href="macouDetail.aspx?id=<%#Eval("sumid") %>&sumcode=<%#Eval("sumcode") %>&stocode=<%#Eval("stocode") %>&ctype=<%#Eval("ctype") %>"><%#Eval("cname") %></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ctypename" HeaderText="<span data-code='ctype_list' >优惠券类型</span>" />
                        <asp:BoundColumn DataField="initypename" HeaderText="<span data-code='initype_list' >发起类型</span>" />
                        <asp:BoundColumn DataField="statusname" HeaderText="<span data-code='status_list' >状态</span>" />
                        <asp:BoundColumn DataField="audusername" HeaderText="<span data-code='auduser_list' >审核人</span>" />
                        <asp:BoundColumn DataField="audstatusname" HeaderText="<span data-code='audstatus_list' >审核状态</span>" />
                        <asp:BoundColumn DataField="cusername" HeaderText="<span data-code='cuser_list' >创建人</span>" />
                        <asp:BoundColumn DataField="ctime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="<span data-code='ctime_list' >创建时间</span>" />
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
