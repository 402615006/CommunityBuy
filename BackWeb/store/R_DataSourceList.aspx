<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R_DataSourceList.aspx.cs" Inherits="CommunityBuy.BackWeb.R_DataSourceList" %>

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
    <script type="text/javascript">
        $(document).ready(function () {
            initLoadtype();
        });

        function initLoadtype() {
            var dtype = $("#ddl_dtype").val();
            var Paremeters = { "dtype": "" };
            if (dtype) {
                Paremeters.dtype = dtype;
            }
            GpAjax('/ajax/StockSupp/WSR_DataSource.ashx', getpostParameters('getprojecttype', Paremeters), true,
                function (data) {
                    if (data.data) {
                        var selHtml = [];
                        selHtml.push("<option value=''>--全部--</option>");
                        for (var i = 0; i < data.data.length; i++) {
                            var datalst = data.data[i];
                            if (datalst.datacode == $("#HidProject").val()) {
                                selHtml.push("<option selected='true' value='" + datalst.datacode + "'>" + datalst.dataname + "</option>");
                            } else {
                                selHtml.push("<option value='" + datalst.datacode + "'>" + datalst.dataname + "</option>");
                            }
                        }
                        $("#ddl_source").html(selHtml.join(""));
                        $("#HidProject").val($("#ddl_source").val());
                    }
                })
        }

        //
        function setsourceValue() {
            $("#HidProject").val($("#ddl_source").val());
        }
    </script>
</head>
<body data-pagecode="R_DataSource">
    <form id="form1" data-tbname="R_DataSource" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="R_DataSourcelist" PageType="List" MainMenu="" SubMenu="报表数据源" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="报表数据源" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li data-code="type_where" class="wherename">类型</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_dtype" IsSearch="true" SelType="DataType" CssClass="selstyle" runat="server" onchange="initLoadtype();" Style="width: 150px;"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="source_where" class="wherename">数据项目</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_source" SelType="Normal" runat="server" CssClass="selstyle" onchange="setsourceValue();" Style="width: 150px;"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="seltime_where" class="wherename">选择日期</li>
                    <li class="wherevale" style="width: 300px;">
                        <input type="text" class="datetxt" id="txt_startdate" runat="server" onfocus="ShowShortMaxDate();" style="width: 120px;" />--
                        <input type="text" class="datetxt" id="txt_enddate" runat="server" onfocus="ShowShortMaxDate();" style="width: 120px;" />
                    </li>
                </ul>
                <ul>
                    <li data-code="stocode_where" class="wherename">门店</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_stocode" SelType="Normal" runat="server" CssClass="selstyle"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="dishes_where" class="wherename">菜品</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_dishes" runat="server" />
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
                                <asp:CheckBox ID="CB_Select" runat="server" /><asp:HiddenField ID="HD_Key" Value='<%# Bind("dsid") %>'
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="dtypename" HeaderText="<span data-code='dtype_list' >类型</span>" />
                        <asp:BoundColumn DataField="datenames" HeaderText="<span data-code='itemcode_list' >数据项目</span>" />
                        <asp:BoundColumn DataField="stoname" HeaderText="<span data-code='stocode_list' >门店</span>" />
                        <asp:BoundColumn DataField="dday" DataFormatString="{0:yyyy}" HeaderText="<span data-code='year_list' >年</span>" />
                        <asp:BoundColumn DataField="dday" DataFormatString="{0:MM}" HeaderText="<span data-code='month_list' >月</span>" />
                        <asp:BoundColumn DataField="dday" DataFormatString="{0:yyyy-MM-dd}" HeaderText="<span data-code='dday_list' >日期</span>" />
                        <asp:BoundColumn DataField="disname" HeaderText="<span data-code='discode_list' >菜品</span>" />
                        <asp:BoundColumn DataField="disnum" HeaderText="<span data-code='disnum_list' >数量</span>" />
                        <asp:BoundColumn DataField="disaccount" HeaderText="<span data-code='disaccount_list' >金额(元)</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <input id="HidProject" type="hidden" runat="server" />
                <div class="pagelist">
                    <webdiyer:AspNetPager ID="anp_top" runat="server" CssClass="paginator" CurrentPageButtonClass="cpb"
                        CustomInfoHTML="<c data-code='PageCount1' ></c>%PageCount%<c data-code='PageCount2' ></c>  %RecordCount%<c data-code='RecordCount' ></c>&nbsp;&nbsp;&nbsp;" AlwaysShow="True" FirstPageText="<c data-code='FirstPage' ></c>"
                        LastPageText="<c data-code='LastPage' ></c>" NextPageText="<c data-code='NextPage' ></c>" PageSize="20" PrevPageText="<c data-code='PrevPage' ></c>" ShowCustomInfoSection="Left"
                        ShowInputBox="Never" CustomInfoTextAlign="Right" LayoutType="Table" OnPageChanging="anp_top_PageChanging"
                        ShowBoxThreshold="300000" ShowNavigationToolTip="True" ShowPrevNext="False">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
