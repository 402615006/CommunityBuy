<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectDishes.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.selectDishes" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/liststyle.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../js/xmlhelper.js"></script>
    <script src="../js/layer/layer.js"></script>
    <script src="../js/default.js" type="text/javascript"></script>
    <script src="../js/listeditjs.js" type="text/javascript"></script>
    <script src="../js/layerhelper.js"></script>
    <link href="../css/select.css" rel="stylesheet" />
    <style>
        .width182 {
            width:155px;
        }
    </style>
    <script type="text/javascript">
        var valid = '';
        var nameid = '';
        var chooseArr = new Array();
        $(document).ready(function () {
            var index = parent.layer.getFrameIndex(window.name);
            $(".searchbtn").click(function () {
                $("#hidiscombo").val($("#seliscombo").val());
                $("#Button1").click();
            });

            var num = getUrlParam("num");
            $(".selectbtn").click(function () {
                var showobj = document.getElementById('sp_showmes');
                var id = 0;
                var hidval = $("#hid_sel").val();
                var prolabname = $("#txtsel").val();
                var s = document.getElementById('gv_list').getElementsByTagName('input');
                for (var i = 0; i < s.length; i++) {
                    var cname = '';
                    if (s[i].type == 'checkbox' && s[i].checked) {
                        //获取第一列的主键列
                        var keyid = s[i + 1].id;
                        if (s[i + 2].id.indexOf('cname') >= 0) {
                            cname = $('#' + s[i + 2].id).val();
                        }
                        id = $('#' + keyid).val();
                        console.info(id);
                        if (undefined == hidval || hidval.length == 0) {
                            hidval += ",";
                        }
                        if (hidval.indexOf("," + id + ",") < 0) {
                            hidval += id + ",";
                            prolabname += "," + cname;
                        }
                    }
                }

                if (num > 0) {
                    if (hidval.split(',').length > num + 1) {
                        var hint = getCommonInfo('rerefernum');
                        hint = hint.format(num);
                        pcLayerMsg(hint);
                        return false;
                    }
                }
                if (prolabname.length > 0) {
                    if (prolabname.substring(0, 1) == ',') {
                        prolabname = prolabname.substring(1, prolabname.length);
                    }
                }
                //赋值hid;
                $("#hid_sel").val(hidval);
                $("#txtsel").val(prolabname);
            });
            $(".cancel_div").click(function () {
                $("#txtsel").val("");
                $("#hid_sel").val("");
            });
            $(".save_div").click(function () {

                var bid = $("#hid_sel").val();
                nameid = getUrlParam("nameid");
                valid = getUrlParam("valid");
                var resval = bid.substring(1, bid.length - 1);
                if (num > 0) {
                    if (resval.split(',').length > num) {
                        var hint = getCommonInfo('rerefernum');
                        hint = hint.format(num);
                        pcLayerMsg(hint);
                        return;
                    }
                }
                if ($("#txtsel").val().indexOf(",", 0) == 0) {
                    parent.$("#" + nameid).val($("#txtsel").val().substr(1, $("#txtsel").val().length - 1));
                }
                else {
                    parent.$("#" + nameid).val($("#txtsel").val());
                }
                parent.$("#" + valid).val(resval);

                var change = getUrlParam("change");
                if (change.length > 0) {
                    parent.$("#" + change).click();
                }
                parent.layer.close(index);
            });

            if ($("#hidisfirst").val() == "1") {
                var discode = getUrlParam("id");
                var stocode = getUrlParam("stocode");
                if (discode.length > 0) {
                    $.ajax({
                        url: "/ajax/getTableList.ashx",
                        type: "post",
                        data: { "way": "dishes1", "ids": discode, "stocode": stocode },
                        dataType: "json",
                        success: function (data) {
                            var prolabname = "";
                            $(data.data).each(function (i) {
                                prolabname += data.data[i].disname + ",";
                            })
                            $("#txtsel").val(prolabname.substring(0, prolabname.length - 1));
                        }
                    });
                    $("#hid_sel").val(',' + discode + ',');
                }
            }
        })
    </script>
</head>
<body data-pagecode="dishes">
    <form id="form1" data-tbname="dishes" runat="server">
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="Storelist" PageType="Normal" MainMenu="" SubMenu="菜品信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="菜品信息" Operate="List" runat="server" Visible="true"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <table class="btn_tab">
                <tr>
                    <td class="btn_tab_left">已选择</td>
                    <td class="btn_tab_right">
                        <div class="cancel_div">取消</div>
                        <div class="save_div">提交</div>
                    </td>
                </tr>
            </table>
            <div class="selectdiv">
                <textarea id="txtsel" runat="server" readonly="readonly"></textarea>
                <input type="hidden" id="hid_sel" runat="server" />
            </div>
            <div class="rightwhere">
                <ul>
                    <li data-code="distype_where" class="wherename">菜品类型：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="seldistype" Descr="" CssClass="selstyle width182" TextType="Normal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="seldistype_SelectedIndexChanged"></cc1:CDropDownList>
                    </li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="seldistype1" Descr="" CssClass="selstyle width182" TextType="Normal" runat="server"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li data-code="disname_where" class="wherename">菜品名称：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_disname" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="customcode_where" class="wherename">自定义编码：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_customcode" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="iscombo_where" class="wherename">是否套餐：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="seliscombo" Descr="" CssClass="selstyle width182" TextType="Normal" runat="server"></cc1:CDropDownList>
                    </li>
                </ul>
                <ul>
                    <li class="wherename"></li>
                    <li class="wherevale" style="width: 180px;">
                        <div class="searchbtn">查询</div>
                        <div class="selectbtn">选择</div>
                        <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" OnClick="Button1_Click" /></li>
                </ul>
            </div>
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
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
                                <asp:CheckBox ID="CB_Select" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="chooseclass"></HeaderStyle>
                            <ItemStyle CssClass="chooseclass"></ItemStyle>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='discode_list'>菜品编号</span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_stocode" Text='<%# Bind("discode") %>' runat="server" />
                                <asp:HiddenField ID="HD_stocode" Value='<%# Bind("discode") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn SortExpression="disname">
                            <HeaderTemplate><span data-code='disname_list'>菜品名称</span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_cname" Text='<%# Bind("disname") %>' runat="server" />
                                <asp:HiddenField ID="HD_cname" Value='<%# Bind("disname") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="customcode" HeaderText="<span data-code='customcode_list' >自定义编码</span>" SortExpression="customcode" />
                        <%--<asp:BoundColumn DataField="distypename" HeaderText="<span data-code='unit_list' >单位</span>" />--%>
                        <asp:BoundColumn DataField="price" HeaderText="<span data-code='price_list' >售价</span>" />
                        <asp:BoundColumn DataField="costprice" HeaderText="<span data-code='costprice_list' >成本价</span>" />
                        <asp:BoundColumn DataField="iscomboname" HeaderText="<span data-code='iscombo_list' >是否套餐</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="hidiscombo" type="hidden" runat="server" />
                <input id="hidstocode" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <input id="hidisfirst" type="hidden" runat="server" value="0" />
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

