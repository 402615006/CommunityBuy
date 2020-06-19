<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBBakSetlist.aspx.cs" Inherits="CommunityBuy.BackWeb.DBBakSetlist" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <link href="/css/switch.css" rel="stylesheet" />
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
        .auto-style1 {
            min-width: 100px;
            text-align: right;
            width: 163px;
        }

        .auto-style2 {
            width: 150px;
        }
    </style>
    <script>
        $(document).ready(function () {
            dateschange();
        });

        function dateschange() {
            var vals = $("#txt_bakcycles").val();
            switch (vals) {
                //周
                case '0':
                    $("#ddl_week").css("display", "block");
                    $("#ddl_hour").css("display", "none");
                    $("#starttimes").css("display", "");
                    break;
                    //天
                case '1':
                    $("#ddl_week").css("display", "none");
                    $("#ddl_hour").css("display", "none");
                    $("#starttimes").css("display", "");
                    break;
                    //小时
                case '2':
                    $("#ddl_week").css("display", "none");
                    $("#ddl_hour").css("display", "block");
                    $("#starttimes").css("display", "none");
                    break;
            }
        }

        function sel_cycle() {
            //是否自动执行
            if ($("#txt_isautos").val() == "1") {
                $("#buyacctotalye").addClass("on");
                $("#buyacctotalye").attr('src', '/img/on.png');
                $("#txt_isautos").val("1");
            }
            else {
                $("#buyacctotalye").removeClass("on");
                $("#buyacctotalye").attr('src', '/img/off.png');
                $("#txt_isautos").val("0");
            }

            //备份周期相关选项
            $("#ddl_week").css("display", "none");
            $("#ddl_hour").css("display", "none");
            $("#starttimes").css("display", "none");

            $("#bakcycles").val();// 备份周期
            $("#btime").val(); //开始时间
            //alert($("#bakcycles").val());
            switch ($("#bakcycles").val()) {
                //周
                case '11':
                    $("#txt_bakcycles").val('0');//住下拉框为 周
                    $("#ddl_week").css("display", ""); //周下拉框 显示
                    $("#txt_week").val($("#bakcycles").val());//周下拉框赋值
                    $("#txt_btime").val($("#btime").val());//时间赋值
                    break;
                case '12':
                    $("#txt_bakcycles").val('0');
                    $("#ddl_week").css("display", "");
                    $("#txt_week").val($("#bakcycles").val());
                    $("#txt_btime").val($("#btime").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                case '13':
                    $("#txt_bakcycles").val('0');
                    $("#ddl_week").css("display", "");
                    $("#txt_week").val($("#bakcycles").val());
                    $("#txt_btime").val($("#btime").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                case '14':
                    $("#txt_bakcycles").val('0');
                    $("#ddl_week").css("display", "");
                    $("#txt_week").val($("#bakcycles").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                case '15':
                    $("#txt_bakcycles").val('0');
                    $("#ddl_week").css("display", "");
                    $("#txt_week").val($("#bakcycles").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                case '16':
                    $("#txt_bakcycles").val('0');
                    $("#ddl_week").css("display", "");
                    $("#txt_week").val($("#bakcycles").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                case '17':
                    $("#txt_bakcycles").val('0');
                    $("#ddl_week").css("display", "");
                    $("#txt_week").val($("#bakcycles").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                    //天
                case 'everyday':
                    $("#txt_bakcycles").val('1');
                    $("#txt_btime").css("display", "");
                    $("#txt_btime").val($("#btime").val());
                    $("#txt_btime").val($("#btime").val());
                    break;
                    //小时
                case 'perhour':
                    $("#txt_bakcycles").val('2');
                    $("#txt_hour").css("display", "");
                    $("#txt_hour").val($("#btime").val());
                    break;

            }
        }

        function addClike(obj) {
            if (obj.hasClass("on")) {
                obj.removeClass("on");
                $(obj).attr('src', '/img/off.png');
                $("#txt_isautos").val("0");
            }
            else {
                obj.addClass("on");
                $(obj).attr('src', '/img/on.png');
                $("#txt_isautos").val("1");
            }
        }
    </script>
</head>
<body data-pagecode="DBBakSet">
    <form id="form1" data-tbname="DBBakSet" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="DBBakSetlist" PageType="Normal" MainMenu="" SubMenu="数据备份设置" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="数据备份设置" Operate="Add" runat="server"></cc1:CPageTitle>
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
                        <td data-code="span_bakcycle" class="auto-style1">备份周期：</td>
                        <td>
                            <div style="display: block; float: left; margin-right: 10px;">
                                <cc1:CDropDownList ID="txt_bakcycles" CssClass="wheresel" SelType="Normal" runat="server" Style="width: 182px;" onchange="dateschange();"></cc1:CDropDownList>
                            </div>
                            <%--周--%>
                            <div style="display: none; float: left" id="ddl_week">
                                <cc1:CDropDownList ID="txt_week" runat="server" CssClass="wheresel" SelType="Normal" Style="width: 182px;" IsRequired="True">
                                </cc1:CDropDownList>
                            </div>
                            <%--小时--%>
                            <div style="display: none; float: left;" id="ddl_hour">
                                <cc1:CTextBox ID="txt_hour" data-code="btime_placeholders" onkeyup="onlyNumber(this)" CssClass="reqtxtstyle" MaxLength="8" TextType="Normal" onblur="onblurCheck('txt_btime')" placeholder="" runat="server"></cc1:CTextBox><span data-code="days">天/一次</span>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr id="starttimes">
                        <td class="auto-style1"><span data-code="span_btime">执行开始时间：</span></td>
                        <td><%--IsRequired="True"--%>
                            <cc1:CTextBox ID="txt_btime" onfocus="ShowHM();" runat="server" CssClass="reqtxtstyle" data-code="btime_placeholder" MaxLength="8" onblur="onblurCheck('txt_btime')" placeholder="" TextType="Normal"></cc1:CTextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td data-code="span_isauto" class="auto-style1">是否自动执行：</td>
                        <td>
                            <img src="/img/off.png" id="buyacctotalye" class="btn-recyble" onclick="addClike($(this))" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td data-code="span_durday" class="auto-style1">保留备份数据时长(天)：</td>
                        <td>
                            <cc1:CTextBox ID="txt_durday" data-code="durday_placeholder" CssClass="txtstyle" onkeyup="onlyNumber(this)" MaxLength="4" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_durday')" placeholder="" runat="server"></cc1:CTextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <input type="hidden" id="hidId" runat="server" />
                            <input type="hidden" id="bakcycles" runat="server" /><%--备份周期--%>
                            <input type="hidden" id="btime" runat="server" /><%--开始时间--%>
                            <input type="hidden" id="txt_isautos" runat="server" />
                        </td>
                        <td colspan="2">
                            <div class="savebtn" data-code="save_edit" id="addbtn">保存</div>
                            <div class="savebtn" data-code="backup">立即备份</div>
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
