<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponDelay.aspx.cs" Inherits="CommunityBuy.BackWeb.coupon.CouponDelay" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
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
    <script src="/js/default.js"></script>
    <style type="text/css">
        .fixtoph2 {
            width: 100%;
            height: 10px;
            line-height: 10px;
        }

        .rightcontent {
            padding-bottom: 5px;
            overflow: hidden;
        }

        .cancel_div {
            float: left;
            width: 80px;
            text-align: center;
            border-radius: 5px;
            height: 30px;
            line-height: 30px;
            color: white;
            cursor: pointer;
            background-color: #7A7A7A;
            margin: 10px;
        }

        .lefttd {
            width: 150px;
        }
    </style>
    <script type="text/javascript">
        var index = parent.layer.getFrameIndex(window.name);

        function GetDelayInfo() {
            var Parameters = {
                "GUID": "",
                "USER_ID": "0",
                "mcid": ""
            };

            Parameters.mcid = $("#hidId").val();
            GpAjax('/ajax/membercard/getmemberCard.ashx', getpostParameters('getmaincouponinfo', Parameters), false, function (data) {
                $(data).each(function (i) {
                    var status = data.status;
                    if (status == "0") {
                        $("#lblcouname").html(data.data[i].couname);
                        $("#lblbtime").html(data.data[i].btime);
                        $("#lbletime").html(data.data[i].etime);

                        $("#txt_delaydate").removeAttr("onfocus");
                        $("#txt_delaydate").attr("onfocus", "WdatePicker({dateFmt: 'yyyy-MM-dd',readOnly:true,minDate:'" + data.data[i].etime + "'});");
                    }
                });
            });
        }

        function gotocoupon() {
            parent.layer.close(index);
        }

        function DelayCoupon() {
            var delaytime = $("#txt_delaydate").val();
            if (delaytime != '' && delaytime != null && delaytime != undefined) {
                var Parameters = {
                    "GUID": "",
                    "USER_ID": "0",
                    "mcid": "",
                    "etime": ""
                };

                Parameters.mcid = $("#hidId").val();
                Parameters.etime = delaytime;
                GpAjax('/ajax/membercard/getmemberCard.ashx', getpostParameters('delaycoupon', Parameters), false, function (data) {
                    $(data).each(function (i) {
                        var status = data.status;
                        if (status == "0") {
                            layer.alert(getNameByCode("delay_sucess")
                            , function () {
                                parent.location.href = parent.location.href;
                                parent.layer.close(index);
                            });
                        }
                    });
                });
            }
        }
    </script>
</head>
<body data-pagecode="maincoupon">
    <form id="form1" data-tbname="maincoupon" runat="server">
        <div class="fixtoph2">&nbsp;</div>
        <div class="fixtop" style="display: none;">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="maincouponlist" PageType="List" MainMenu="" SubMenu="优惠活动优惠券信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="优惠活动优惠券信息" Operate="Edit" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="updatediv" id="cla">
                <table>
                    <tr>
                        <td data-code="span_couname" class="lefttd">优惠券名称：</td>
                        <td>
                            <asp:Label ID="lblcouname" Text="" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_btime" class="lefttd">开始时间：</td>
                        <td>
                            <asp:Label ID="lblbtime" Text="" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_etime" class="lefttd">结束时间：</td>
                        <td>
                            <asp:Label ID="lbletime" Text="" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_delaydate" class="lefttd">延期日期：</td>
                        <td>
                            <cc1:CTextBox ID="txt_delaydate" data-code="delay_placeholder" CssClass="txtstyle" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_delaydate')" runat="server"></cc1:CTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <input type="hidden" id="hidId" runat="server" />
                            <div id="savebtn" class="savebtn" runat="server" onclick="DelayCoupon()">保存</div>
                            <div class="cancel_div" onclick="gotocoupon()">返回</div>
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
