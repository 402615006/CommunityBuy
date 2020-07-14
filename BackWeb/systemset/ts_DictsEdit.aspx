<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ts_DictsEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.systemset.ts_DictsEdit" %>

<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <style>
        .lefttd {
            width: 160px;
        }
    </style>
</head>
<body data-pagecode="ts_Dicts">
    <form id="form1" data-tbname="ts_Dicts" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="ts_Dictslist" PageType="Add" MainMenu="" SubMenu="系统字典信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="系统字典信息" Operate="Add" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td data-code="span_pdicid" class="lefttd">父节点：</td>
                        <td>

                            <cc1:CDropDownList ID="ddl_pdicid" data-code="pdicid_placeholder" Descr="父节点" CssClass="selstyle" TextType="Normal" runat="server">
                            </cc1:CDropDownList>

                            <span class="redmess" id="txt_pdicid_span"></span>
                        </td>

                        <td data-code="span_lng" class="lefttd">语言代码：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_lng" data-code="lng_placeholder" Descr="角色" CssClass="selstyle" TextType="Normal" runat="server">
                            </cc1:CDropDownList><span class="redmess" id="txt_lng_span"></span>
                        </td>
                    </tr>
                    <tr>

                        <td data-code="span_diccode" class="lefttd">字典编号：</td>
                        <td>
                            <cc1:CTextBox ID="txt_diccode" data-code="systemcode_placeholder" CssClass="txtstyle" MaxLength="16" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_diccode')" Enabled="false" runat="server"></cc1:CTextBox></td>

                        <td data-code="span_dicname" class="lefttd">字典名称：</td>
                        <td>
                            <cc1:CTextBox ID="txt_dicname" data-code="dicname_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_dicname')" placeholder="" runat="server"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td data-code="span_orderno" class="lefttd">排序号：</td>
                        <td>
                            <cc1:CTextBox ID="txt_orderno" data-code="orderno_placeholder" CssClass="txtstyle" MaxLength="4" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_orderno')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_orderno_span"></span>						</td>
                        <td data-code="span_status" class="lefttd">状态：</td>
                        <td>


                            <cc1:CDropDownList ID="ddl_status" data-code="status_placeholder" Descr="父节点" CssClass="selstyle" TextType="Normal" runat="server" SelType="Status">
                            </cc1:CDropDownList><span class="redmess" id="txt_status_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_remark" class="lefttd">备注：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_remark" data-code="remark_placeholder" CssClass="txtstyle" MaxLength="128" Height="80" Width="80%" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_remark')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_remark_span"></span>						</td>

                    </tr>

                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td>
                            <div id="savepage" runat="server" class="savebtn">保存</div>
                            <div class="gobackbtn">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none"></div>
            <div class="bottomline"></div>
        </div>
    </form>
</body>
</html>
