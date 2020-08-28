<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dishTypeDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.systemset.dishTypeDetail" %>
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
<body data-pagecode="ts_Dicts">
    <form id="form1" data-tbname="ts_Dicts" runat="server">
		<div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="ts_Dictslist" PageType="Detail" MainMenu="" SubMenu="系统字典信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle"><cc1:CPageTitle ID="PageTitle" Menu="系统字典信息" Operate="详情" runat="server"></cc1:CPageTitle></div>
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
						<td data-code="span_dictype" class="lefttd">类别：</td>
					<td><span id="dictype" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_lng" class="lefttd">语言代码：</td>
					<td><span id="lng" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_pdicid" class="lefttd">父ID：</td>
					<td><span id="pdicid" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_diccode" class="lefttd">字典编号：</td>
					<td><span id="diccode" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_dicname" class="lefttd">字典名称：</td>
					<td><span id="dicname" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_dicvalue" class="lefttd">字典值：</td>
					<td><span id="dicvalue" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_orderno" class="lefttd">排序号：</td>
					<td><span id="orderno" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_remark" class="lefttd">备注：</td>
					<td><span id="remark" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_status" class="lefttd">有效状态（0无效，1有效）：</td>
					<td><span id="status" runat="server"></span></td>
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