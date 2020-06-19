<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePhoneNumRefer.aspx.cs" Inherits="CommunityBuy.BackWeb.ChangePhoneNumRefer" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
    <link href="/css/select.css" rel="stylesheet" />
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="../js/jquery.uploadify/jquery.uploadify.v2.1.4.js" charset="gbk" type="text/javascript"></script>
    <script src="../js/jquery.uploadify/swfobject.js"></script>
    <link href="../js/jquery.uploadify/uploadify.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            var index = parent.layer.getFrameIndex(window.name);
            //加载原来手机号
            $("#oldPhoneNum").val(parent.$("#Ghostsel_PhoneNum").val());

            //保存绑定事件
            $(".save_div").click(function () {
                nameid = getUrlParam("nameid");//获取控件名称
                parent.$("#" + nameid).val($("#newPhoneNum").val());//给父页面控件赋值
                parent.layer.close(index);
            });
        });
        //修改成功
        function Success() {
            layer.msg(getNameByCode("checkSuccess"), { icon: 0 });
        }
        //修改成功
        function Fail() {
            layer.msg(getNameByCode("checkFail"), { icon: 1 });
        }
    </script>
</head>

<body data-pagecode="IncomingMessage" data-noselect="">
    <form id="form1" data-tbname="StockMaterial" runat="server">
        <div class="fixtop" style="display: none">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="StockMateriallist" PageType="Normal" MainMenu="" SubMenu="原料信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="修改手机号" Operate="List" runat="server" Visible="true"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <table class="btn_tab">
                <tr>
                    <td data-code="oidPhoneNum" class="lefttd">原手机号：</td>
                    <td>
                        <cc1:CTextBox ID="oldPhoneNum" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox>
                    </td>
                </tr>
                <tr>
                    <td data-code="newPhoneNum" class="lefttd">新手机号：</td>
                    <td>
                        <cc1:CTextBox ID="newPhoneNum" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" runat="server"></cc1:CTextBox>
                    </td>
                </tr>
                <tr>
                    <td data-code="changeCours" class="lefttd">更改原因：</td>
                    <td>
                        <%-- <cc1:CTextBox ID="CTextBox2" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" Enabled="false" runat="server"></cc1:CTextBox>--%>
                        <textarea></textarea>
                    </td>
                </tr>
                <td data-code="newPhoneNum" class="lefttd">验 证 码：</td>
                <td>
                    <cc1:CTextBox ID="CTextBox1" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" runat="server"></cc1:CTextBox>
                </td>
                <tr>
                    <td></td>
                    <td>
                        <div class="cancel_div">取消</div>
                        <asp:Button ID="Button1" Class="save_div" runat="server" Text="提交" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
