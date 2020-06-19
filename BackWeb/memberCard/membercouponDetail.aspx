<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membercouponDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.membercouponDetail" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
</head>
<body data-pagecode="members">
    <form id="form1" data-tbname="members" runat="server">
        <div class="fixtop" style="display: none;">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memberslist" PageType="Normal" MainMenu="" SubMenu="会员信息表" runat="server"></cc1:CPathBar>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li data-code="couponusenum_where" class="wherename">是否使用：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_sfsy" CssClass="wheresel" Width="80" SelType="Normal" runat="server" IsSearch="false"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="pstore_where" class="wherename">发放门店：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_store" CssClass="wheresel" Width="150" SelType="Normal" runat="server" IsSearch="false"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="couponsdate_where" class="wherename">开始生效时间：</li>
                    <li class="wherevale">
                        <input type="text" class="datetxt" id="txt_stime" runat="server" onfocus="ShowShortDate();" style="width: 120px;" />
                    </li>
                </ul>
                <ul>
                    <li data-code="couponedate_where" class="wherename">结束过期时间：</li>
                    <li class="wherevale">
                        <input type="text" class="datetxt" id="txt_etime" runat="server" onfocus="ShowShortDate();" style="width: 120px;" />
                    </li>
                </ul>
                <ul>
                    <li data-code="puser_where" class="wherename">发放人：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_ffuser" runat="server" />
                    </li>
                </ul>
                <ul>
                    <li data-code="ptime_where" class="wherename">发放时间：</li>
                    <li class="wherevale" style="width: 300px;">
                        <input type="text" class="datetxt" id="txt_ffstime" runat="server" onfocus="ShowShortDate();" style="width: 120px;" />--<input type="text" class="datetxt" id="txt_ffetime" runat="server" onfocus="ShowShortDate();" style="width: 120px;" />
                    </li>
                </ul>
            </div>
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
            <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" />
            <div class="righttable">
                <cc1:CustDataGrid ID="gv_list" runat="server" Width="100%" rules="none" CellPadding="4"
                    EnableViewState="true" AutoGenerateColumns="False"
                    AllowSorting="True" GridLines="None" CssClass="List_tab" PagerID="GridPager1"
                    SeqNo="0">
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <AlternatingItemStyle BackColor="#EFF3FB" CssClass="List_tab td" />
                    <ItemStyle BackColor="White" CssClass="List_tab td" />
                    <Columns>
                        <asp:BoundColumn DataField="storename" HeaderText="<span data-code='usingrange' >适用门店</span>" />
                        <asp:BoundColumn DataField="couname" HeaderText="<span data-code='couponname' >优惠券名称</span>" />
                        <asp:BoundColumn DataField="checkcode" HeaderText="<span data-code='checkcode_list' >券号</span>" />
                        <asp:BoundColumn DataField="statusname" HeaderText="<span data-code='couponusenum' >是否使用</span>" />
                        <asp:BoundColumn DataField="edate" HeaderText="<span data-code='couponedate' >有效期</span>" />
                        <asp:BoundColumn DataField="prostoname" HeaderText="<span data-code='pstore_list' >发放门店</span>" />
                        <asp:BoundColumn DataField="puser" HeaderText="<span data-code='puser_list' >发放人</span>" />
                        <asp:BoundColumn DataField="ptime" HeaderText="<span data-code='ptime_list' >发放时间</span>" />
                        <asp:BoundColumn DataField="checkstoname" HeaderText="<span data-code='checkstoname_list' >回收门店</span>" />
                        <asp:BoundColumn DataField="checkperson" HeaderText="<span data-code='checkperson_list' >回收人</span>" />
                        <asp:BoundColumn DataField="checktime" HeaderText="<span data-code='checktime_list' >回收时间</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
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
        <div class="bottomline"></div>
    </form>
</body>
</html>
