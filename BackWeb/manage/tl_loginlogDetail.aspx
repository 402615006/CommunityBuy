<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tl_loginlogDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.tl_loginlogDetail" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
	<script src="/js/xmlhelper.js"></script>
</head>
<body data-pagecode="tl_loginlog">
    <form id="form1" data-tbname="tl_loginlog" runat="server">
		<div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="tl_loginloglist" PageType="Detail" MainMenu="" SubMenu="系统登录日志" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle"><cc1:CPageTitle ID="PageTitle" Menu="系统登录日志" Operate="详情" runat="server"></cc1:CPageTitle></div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>



            <div class="updatediv cla">
				<table>
					<tr>
						<td data-code="span_buscode" class="lefttd">引用商户表Business的商户编号字段buscode的值：</td>
					<td><span id="buscode" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_strcode" class="lefttd">门店编号：</td>
					<td><span id="strcode" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_userid" class="lefttd">引用系统用户表ts_admins的userid字段值：</td>
					<td><span id="userid" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_cname" class="lefttd">登录姓名：</td>
					<td><span id="cname" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_ip" class="lefttd">登录IP：</td>
					<td><span id="ip" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_logcontent" class="lefttd">日志信息：</td>
					<td><span id="logcontent" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_ctime" class="lefttd">创建时间：</td>
					<td><span id="ctime" runat="server"></span></td>
					</tr>

                    <tr>
                        <td></td>
                        <td>
                            <div class="gobackbtn">返回</div>
                        </td>
                    </tr>
				</table>
            </div>
            <div class="updatediv clb" style="display:none"></div>
        </div>
    </form>
</body>
</html>