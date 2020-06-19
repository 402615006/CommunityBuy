<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminsdetail.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.adminsdetail" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js"></script>
    <script src="/js/listeditjs.js"></script>
    <script src="/js/xmlhelper.js"></script>
</head>
<body data-pagecode="admins">
    <form id="form1" data-tbname="admins" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="adminslist" PageType="Detail" MainMenu="系统管理" SubMenu="用户管理" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle"><cc1:CPageTitle ID="PageTitle" Menu="用户管理" Operate="详情" runat="server"></cc1:CPageTitle></div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td data-code="span_uname" class="lefttd">用户名：</td>
                        <td>
                            <span id="uname" runat="server"></span>
                        </td>
                        <td data-code="span_pwd" class="lefttd">密码：</td>
                        <td>
                            <span id="pwd" runat="server"></span>
                        </td>
                        <td data-code="span_cname" class="lefttd">真实姓名：</td>
                        <td>
                            <span id="realname" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_empcode" class="lefttd">所属员工：</td>
                        <td>
                            <span id="empcode" runat="server"></span>
                        </td>
                        <td data-code="span_umobile" class="lefttd">手机号：</td>
                        <td>
                            <span id="umobile" runat="server"></span>
                        </td>
                        <td data-code="span_role" class="lefttd">所属角色：</td>
                        <td>
                            <span id="rolname" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_descr" class="lefttd">备注：</td>
                        <td>
                            <span id="descr" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="3">
                            <div class="gobackbtn">返回</div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>