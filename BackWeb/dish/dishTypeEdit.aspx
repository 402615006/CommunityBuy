<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dishTypeEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.systemset.dishTypeEdit" %>

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
    <script src="/js/layui/layui.all.js"></script>
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
<body data-pagecode="dishType">
    <form id="form1" data-tbname="dishType" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="dishType" PageType="Add" MainMenu="" SubMenu="商品类型" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="商品类型" Operate="Add" runat="server"></cc1:CPageTitle>
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
                        <td class="lefttd">一级分类：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_pdicid" data-code="pdicid_placeholder" Descr="父节点" CssClass="selstyle" TextType="Normal" runat="server">
                            </cc1:CDropDownList>
                            <span class="redmess" id="txt_pdicid_span"></span>
                        </td>
                    </tr>
                    <tr>
                        <td  class="lefttd">分类名称：</td>
                        <td>
                            <cc1:CTextBox ID="txt_dicname" data-code="dicname_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_dicname')" placeholder="" runat="server"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td class="lefttd">排序号：</td>
                        <td>
                            <cc1:CTextBox ID="txt_orderno" data-code="orderno_placeholder" CssClass="txtstyle" MaxLength="4" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_orderno')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_orderno_span"></span></td>
                    </tr>
                    <tr>
                        <td class="lefttd">状态：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_status"  CssClass="selstyle" TextType="Normal" runat="server" SelType="Status">
                            </cc1:CDropDownList><span class="redmess" id="txt_status_span"></span>						</td>
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
