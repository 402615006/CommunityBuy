<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ts_syssetDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.ts_syssetDetail" %>
<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
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
<body data-pagecode="ts_sysset">
    <form id="form1" data-tbname="ts_sysset" runat="server">
		<div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="ts_syssetlist" PageType="Detail" MainMenu="" SubMenu="系统设置" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle"><cc1:CPageTitle ID="PageTitle" Menu="系统设置" Operate="详情" runat="server"></cc1:CPageTitle></div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>



            <div class="updatediv cla">
				<table>
		
					<tr>
						<td data-code="span_key" class="lefttd">键：</td>
					<td><span id="key" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_val" class="lefttd">值：</td>
					<td><span id="val" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_status" class="lefttd">有效状态（0无效，1有效）：</td>
					<td><span id="status" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_descr" class="lefttd">描述：</td>
					<td><span id="descr" runat="server"></span></td>
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