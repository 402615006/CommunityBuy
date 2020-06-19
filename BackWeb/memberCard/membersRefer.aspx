<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membersRefer.aspx.cs" Inherits="CommunityBuy.BackWeb.membersRefer" %>

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
    <style type="text/css">
        .headdisplay {
            display: none;
        }
    </style>
    <script type="text/javascript">
        var valid = '';
        var nameid = '';
        $(document).ready(function () {
            var index = parent.layer.getFrameIndex(window.name);
            $(".searchbtn").click(function () {
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
                        }
                        id = $('#' + keyid).val();
                        if (hidval.indexOf("," + id + ",") < 0) {
                            hidval += id + ",";
                            prolabname += "," + cname;
                        }
                    }
                }
                var num = getUrlParam("num");
                if (hidval.split(',').length - 1 > num) {
                    var hint = getCommonInfo('rerefernum');
                    hint = hint.format(num);
                    pcLayerMsg(hint);
                    return false;
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
                var formpage = getUrlParam("formpage");
                if (formpage != undefined && formpage.length > 0)//回调页面方法
                {
                    chooseArr = [];
                    var len = $('#gv_list tbody tr').length;
                    for (var i = 1; i < len; i++) {
                        var flag = $('#gv_list tr:eq(' + i + ') td:eq(0)').find('input:checkbox').is(':checked');
                        if (flag) {
                            var dishesarr = {
                                "memcode": '',
                                "cname": '',
                                "cardcode": '',
                                "creditlinestoname": '',
                                "signcreditstoname": '',
                                "creditlinestocode": '',
                                "signcreditstocode": '',
                                "signcreditmoney": '',
                                "creditlinemoney": ''
                            };
                            dishesarr.memcode = $('#gv_list tr:eq(' + i + ') td:eq(1)').find('span').html();
                            dishesarr.cname = $('#gv_list tr:eq(' + i + ') td:eq(2)').find('span').html();
                            dishesarr.cardcode = $('#gv_list tr:eq(' + i + ') td:eq(3)').find('span').html();
                            dishesarr.creditlinestoname = $('#gv_list tr:eq(' + i + ') td:eq(4)').find('span').html();
                            dishesarr.signcreditstoname = $('#gv_list tr:eq(' + i + ') td:eq(5)').find('span').html();
                            dishesarr.creditlinestocode = $('#gv_list tr:eq(' + i + ') td:eq(4)').find('input:hidden').val();
                            dishesarr.signcreditstocode = $('#gv_list tr:eq(' + i + ') td:eq(5)').find('input:hidden').val();
                            dishesarr.signcreditmoney = $('#gv_list tr:eq(' + i + ') td:eq(6)').find('span').html();
                            dishesarr.creditlinemoney = $('#gv_list tr:eq(' + i + ') td:eq(7)').find('span').html();
                            chooseArr.push(dishesarr);
                        }
                    }
                    switch (formpage) {
                        case "SetBigCustomer":
                            parent.setmembers(chooseArr);
                            break;
                    }
                }
                else {
                    var bid = $("#hid_sel").val();
                    nameid = getUrlParam("nameid");
                    valid = getUrlParam("valid");
                    var isbig = getUrlParam("isbig");

                    parent.$("#" + nameid).val($("#txtsel").val());
                    parent.$("#" + valid).val(bid.substring(0, bid.length - 1));
                    if (isbig == '1') {
                        parent.getPCardCode();
                    }
                }
                parent.layer.close(index);
            });

            if ($("#hidisfirst").val() == "1") {
                var getid = getUrlParam("id");
                if (getid.length > 0) {
                    $.ajax({
                        url: "../ajax/getTableList.ashx",
                        type: "post",
                        data: { "way": "members", "ids": getid },
                        dataType: "json",
                        success: function (data) {
                            var prolabname = "";
                            $(data.memberslist).each(function (i) {
                                prolabname += data.memberslist[i].cname + ",";
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

<body data-pagecode="members" data-intlist="" data-noselect="">
    <form id="form1" data-tbname="members" runat="server">
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memberslist" PageType="Normal" MainMenu="" SubMenu="会员信息表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员信息表" Operate="List" runat="server" Visible="true"></cc1:CPageTitle>
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
                    <li data-code="cname_where" class="wherename">会员名称：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_cname" runat="server" /></li>
                </ul>
                <ul>
                    <li data-code="span_memcode" class="wherename">会员编号：</li>
                    <li class="wherevale">
                        <input type="text" class="wheretxt" id="txt_memcode" runat="server" /></li>
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
                            <HeaderTemplate><span data-code='memcode_list'></span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_stocode" Text='<%# Bind("memcode") %>' runat="server" />
                                <asp:HiddenField ID="HD_stocode" Value='<%# Bind("memcode") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='cname_list'></span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_cname" Text='<%# Bind("cname") %>' runat="server" />
                                <asp:HiddenField ID="HD_cname" Value='<%# Bind("cname") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate><span data-code='cardcode_list'></span></HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_cardcode" Text='<%# Bind("cardcode") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="headdisplay" />
                            <HeaderTemplate><span data-code='creditlinestoname_list'></span></HeaderTemplate>
                            <ItemStyle CssClass="headdisplay" />
                            <ItemTemplate>
                                <asp:Label ID="lb_creditlinestoname" Text='<%# Bind("creditlinestoname") %>' runat="server" />
                                <asp:HiddenField ID="HD_creditlinestocode" Value='<%# Bind("creditlinestocode") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="headdisplay" />
                            <HeaderTemplate><span data-code='signcreditstoname_list'></span></HeaderTemplate>
                            <ItemStyle CssClass="headdisplay" />
                            <ItemTemplate>
                                <asp:Label ID="lb_signcreditstoname" Text='<%# Bind("signcreditstoname") %>' runat="server" />
                                <asp:HiddenField ID="HD_signcreditstocode" Value='<%# Bind("signcreditstocode") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="headdisplay" />
                            <HeaderTemplate><span data-code='signcreditmoney_list'></span></HeaderTemplate>
                            <ItemStyle CssClass="headdisplay" />
                            <ItemTemplate>
                                <asp:Label ID="lb_signcreditmoney" Text='<%# Bind("signcreditmoney") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle CssClass="headdisplay" />
                            <HeaderTemplate><span data-code='creditlinemoney_list'></span></HeaderTemplate>
                            <ItemStyle CssClass="headdisplay" />
                            <ItemTemplate>
                                <asp:Label ID="lb_creditlinemoney" Text='<%# Bind("creditlinemoney") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="mobile" HeaderText="<span data-code='mobile_list' >手机号码</span>" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
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
