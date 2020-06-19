<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sumcouponEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.sumcouponEdit" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
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
    <script>
        $(document).ready(function () {
            //取消tab事件
            $(".graydiv").off('click');
        });
        function gotocouponpage() {
            $(".graydiv").removeClass("seldiv");
            $("#clb").addClass("seldiv");
            $(".clb").show();
            $(".cla").hide();
            $("#frame").attr('src', 'maincouponList.aspx?id=' + $("#hidId").val() + '&sumcode=' + $("#Hidsumcode").val() + '&stocode=' + $("#hidStocode").val() + '&ctype=' + $("#Hidctype").val());
        }
    </script>
</head>
<body data-pagecode="sumcoupon">
    <form id="form1" data-tbname="sumcoupon" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="sumcouponlist" PageType="Add" MainMenu="" SubMenu="优惠券活动" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="优惠券活动" Operate="Add" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" id="cla">基本信息</div>
                <div data-code="tab2" class="graydiv" id="clb">优惠券信息</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <%-- <td data-code="span_stocode" class="lefttd">所属门店：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_stocode" Descr="所属门店" SelType="Normal" CssClass="selstyle" Style="width: 190px;" runat="server"></cc1:CDropDownList>
                        </td>--%>
                        <td data-code="span_sumcode" class="lefttd">活动编号：</td>
                        <td>
                            <cc1:CTextBox ID="txt_sumcode" data-code="systemcode_placeholder" CssClass="txtstyle" MaxLength="16" IsRequired="false" TextType="Normal" Enabled="false" placeholder="" runat="server"></cc1:CTextBox></td>
                        <td data-code="span_cname" class="lefttd">活动名称：</td>
                        <td>
                            <cc1:CTextBox ID="txt_cname" data-code="cname_placeholder" CssClass="reqtxtstyle" MaxLength="128" Width="200" IsRequired="True" TextType="Normal" placeholder="" onblur="onblurCheck('txt_cname')" runat="server"></cc1:CTextBox></td>
                        <%--<td data-code="span_btime" class="lefttd">活动有效期：</td>
                        <td>
                            <cc1:CTextBox ID="txt_btime" data-code="btime_placeholder" CssClass="reqtxtstyle" IsRequired="True" TextType="Normal" onfocus="ShowShortDate();" placeholder="" runat="server" Width="75"></cc1:CTextBox>&nbsp;-&nbsp;<cc1:CTextBox ID="txt_etime" data-code="etime_placeholder" CssClass="reqtxtstyle" IsRequired="True" TextType="Normal" onfocus="ShowShortDate();" placeholder="" Width="75" runat="server"></cc1:CTextBox>
                        </td>--%>
                    </tr>
                    <tr>
                        <td data-code="span_ctype" class="lefttd">优惠券类型：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_ctype" Descr="优惠券类型" SelType="CouponFirstType" CssClass="selstyle" Width="182" runat="server"></cc1:CDropDownList>&nbsp;
                            <%--<cc1:CDropDownList ID="ddl_secctype" Descr="优惠券二级类型" Width="80" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>--%>
                        </td>
                        <td data-code="span_initype" class="lefttd">发起类型：</td>
                        <td colspan="3">
                            <cc1:CDropDownList ID="ddl_initype" Descr="发起类型" SelType="CouponIniType" CssClass="selstyle" Width="214" runat="server"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_descr" class="lefttd">活动描述：</td>
                        <td colspan="5">
                            <textarea id="txt_descr" rows="6" style="width: 98%;" runat="server"></textarea></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="Hidsumcode" runat="server" />
                            <input type="hidden" id="hidId" runat="server" />
                            <input type="hidden" id="hidStocode" runat="server" />
                            <input type="hidden" id="Hidctype" runat="server" />
                            <input type="hidden" id="Hidsectype" runat="server" />
                        </td>
                        <td colspan="5">
                            <div class="savebtn" data-code="save_edit">保存</div>
                            <div class="gobackbtn" data-code="back_edit">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />
                            <span id="errormessage" runat="server"></span></td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none; height:530px">
                <iframe id="frame" src="" style="width: 100%; height: 500px; border: 0px;"></iframe>
                <div class="gobackbtn" data-code="back_edit">返回</div>
            </div>
            <div class="bottomline"></div>
        </div>
    </form>
</body>
</html>
