<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectStroe.aspx.cs" Inherits="CommunityBuy.BackWeb.selectStroe" %>

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
    <link href="/css/select.css" rel="stylesheet" />

    <script type="text/javascript">
        var valid = '';
        var nameid = '';

        $(document).ready(function () {
            var index = parent.layer.getFrameIndex(window.name);
            $(".searchbtn").click(function () {
                isfirst = false;
                $("#Button1").click();
            });
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

                            id = $('#' + keyid).val();
                            if (undefined == hidval || hidval.length == 0) {
                                hidval += ",";
                            }
                            if (hidval.indexOf("," + id + ",") < 0) {
                                hidval += id + ",";
                                prolabname += "," + cname;
                            }
                        }
                    }
                }
                var num = getUrlParam("num");
                if (num > 0) {
                    if (hidval.split(',').length - 2 > num) {
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
                var formpage = getUrlParam("formpage");
                if (formpage == "") {
                    parent.$("#" + nameid).val($("#txtsel").val());
                    parent.$("#" + valid).val(bid.substring(1, bid.length - 1));
                } else {
                    chooseArr = [];
                    var dishesarr = {
                        "storecode": '',
                        "storename": ''
                    };

                    if ($("#txtsel").val().substring(0, 1) == ",") {
                        dishesarr.storename = $("#txtsel").val().substring(1, $("#txtsel").val().length);
                    }
                    else {
                        dishesarr.storename = $("#txtsel").val();
                    }
                    dishesarr.storecode = bid.substring(1, bid.length - 1);
                    chooseArr.push(dishesarr);
                    parent.SetSotre(chooseArr);
                }
                parent.layer.close(index);
            });

            if ($("#hidisfirst").val() == "1") {
                var getid = getUrlParam("id");
                if (getid.length > 0) {
                    $.ajax({
                        url: "/ajax/getTableList.ashx",
                        type: "post",
                        data: { "way": "store", "ids": getid },
                        dataType: "json",
                        success: function (data) {
                            var prolabname = "";
                            $(data.storelist).each(function (i) {
                                prolabname += data.storelist[i].cname + ",";
                            })
                            $("#txtsel").val(prolabname.substring(0, prolabname.length - 1));
                        }
                    });
                    $("#hid_sel").val(',' + getid + ',');
                }
            }
        })
    </script>
</head>

<body data-pagecode="Store">
    <form id="form1" data-tbname="Store" runat="server">
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="Storelist" PageType="Normal" MainMenu="" SubMenu="商家门店信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="商家门店信息" Operate="List" runat="server" Visible="true"></cc1:CPageTitle>
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
                <input type="hidden" id="hidbuscode" runat="server" />
            </div>
            <div class="rightwhere">
               <%-- <ul>
                    <li data-code="buscode_where" class="wherename">所属商户：</li>
                    <li class="wherevale">
                        <cc1:CDropDownList ID="ddl_businfo" Descr="所属商户" Width="140" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                    </li>
                    <li></li>
                </ul>--%>
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
                    <li class="wherename"></li>
                    <li class="wherevale" style="width: 200px;">
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
                        <asp:BoundColumn DataField="busname" HeaderText="<span data-code='buscode_list' >所属商户</span>" />
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='stocode_list'></span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_stocode" Text='<%# Bind("stocode") %>' runat="server" />
                                <asp:HiddenField ID="HD_stocode" Value='<%# Bind("stocode") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='cname_list'></span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_cname" Text='<%# Bind("cname") %>' runat="server" />
                                <asp:HiddenField ID="HD_cname" Value='<%# Bind("cname") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <%--<asp:BoundColumn DataField="sname" HeaderText="<span data-code='sname_list' >门店简称</span>" />
                        <asp:BoundColumn DataField="bcode" HeaderText="<span data-code='bcode_list' >门店简码</span>" />
                        <asp:BoundColumn DataField="areaname" HeaderText="<span data-code='provinceid_list' >所在区域</span>" />--%>
                        <asp:BoundColumn DataField="stoprincipal" HeaderText="<span data-code='stoprincipal_list' >负责人</span>" />
                        <asp:BoundColumn DataField="stoprincipaltel" HeaderText="<span data-code='stoprincipaltel_list' >负责人联系电话</span>" />
                        <asp:BoundColumn DataField="tel" HeaderText="<span data-code='tel_list' >门店电话</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidType" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
                <input id="hidisfirst" type="hidden" runat="server" value="0" />
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
