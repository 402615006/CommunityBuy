<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sumcouponDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.sumcouponDetail" %>
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
<body data-pagecode="sumcoupon">
    <form id="form1" data-tbname="sumcoupon" runat="server">
		<div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="sumcouponlist" PageType="Detail" MainMenu="" SubMenu="优惠券活动" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle"><cc1:CPageTitle ID="PageTitle" Menu="优惠券活动" Operate="详情" runat="server"></cc1:CPageTitle></div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>
            <div class="updatediv cla">
				<table>
					<tr>
						<td data-code="span_sumcode" class="lefttd">活动编号：</td>
					<td><span id="sumcode" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_buscode" class="lefttd">商户编号：</td>
					<td><span id="buscode" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_stocode" class="lefttd">所属门店：</td>
					<td><span id="stocode" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_cname" class="lefttd">活动名称：</td>
					<td><span id="cname" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_btime" class="lefttd">活动有效期(起)：</td>
					<td><span id="btime" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_etime" class="lefttd">活动有效期(终)：</td>
					<td><span id="etime" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_ctype" class="lefttd">优惠券一级类型：</td>
					<td><span id="ctype" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_secctype" class="lefttd">优惠券二级类型：</td>
					<td><span id="secctype" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_initype" class="lefttd">发起类型：</td>
					<td><span id="initype" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_status" class="lefttd">状态：</td>
					<td><span id="status" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_descr" class="lefttd">活动描述：</td>
					<td><span id="descr" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_auduser" class="lefttd">审核人：</td>
					<td><span id="auduser" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_audremark" class="lefttd">审核备注：</td>
					<td><span id="audremark" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_audstatus" class="lefttd">审核状态：</td>
					<td><span id="audstatus" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_cuser" class="lefttd">创建人：</td>
					<td><span id="cuser" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_ctime" class="lefttd">创建时间：</td>
					<td><span id="ctime" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_uuser" class="lefttd">最后更新人标识：</td>
					<td><span id="uuser" runat="server"></span></td>
					</tr>
					<tr>
						<td data-code="span_utime" class="lefttd">更新时间：</td>
					<td><span id="utime" runat="server"></span></td>
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