<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectrole.aspx.cs" Inherits="CommunityBuy.BackWeb.selectrole" %>

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
    <link href="/css/select.css" rel="stylesheet" />
    <script type="text/javascript">
        var valid = '';
        var nameid = '';
        $(document).ready(function () {
            var index = parent.layer.getFrameIndex(window.name);
            $(".searchbtn").click(function () {
                $("#Button1").click();
            });
            $(".cancel_div").click(function () {
                $("#txtsel").val("");
                $("#hid_sel").val("");
            });
            $(".save_div").click(function () {
                var bid = $("#hid_sel").val();
                nameid = getUrlParam("nameid");
                valid = getUrlParam("valid");
                if ($("#txtsel").val() != "") {
                    parent.$("#" + nameid).val($("#txtsel").val());
                }
                if (bid != "") {
                    parent.$("#" + valid).val(bid.substring(1, bid.length - 1));
                }
                parent.layer.close(index);
            });
        })
    </script>
</head>

<body data-pagecode="rolefunction">
    <form id="form1" data-tbname="rolefunction" runat="server">
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="rolefunctionlist" PageType="Normal" MainMenu="" SubMenu="角色信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="角色信息" Operate="List" runat="server" Visible="true"></cc1:CPageTitle>
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
                    <li class="wherename">角色名称：</li>
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
                        <asp:TemplateColumn>
                            <HeaderTemplate>角色ID</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_stocode" Text='<%# Bind("roleid") %>' runat="server" />
                                <asp:HiddenField ID="HD_roleid" Value='<%# Bind("roleid") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>角色名称</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lb_cname" Text='<%# Bind("cname") %>' runat="server" />
                                <asp:HiddenField ID="HD_cname" Value='<%# Bind("cname") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="descr" HeaderText="角色描述" />
                    </Columns>
                </cc1:CustDataGrid>
                <input id="HidWhere" runat="server" type="hidden" />
                <input id="HidOrder" type="hidden" runat="server" />
                <input id="HidSortExpression" type="hidden" runat="server" />
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
