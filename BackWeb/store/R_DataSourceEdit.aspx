<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R_DataSourceEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.R_DataSourceEdit" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <script src="/js/base-loading.js" type="text/javascript"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnCal").click(function () {
                var stocode = $("#ddl_stocode").val();
                if (!stocode) {
                    layer.alert("请选择门店", { icon: 0 });
                    return;
                }
                var startdate = $("#txt_startdate").val();
                if (!startdate) {
                    layer.alert("请选择执行开始日期", { icon: 0 });
                    return;
                }
                var enddate = $("#txt_enddate").val();
                if (!enddate) {
                    layer.alert("请选择执行结束日期", { icon: 0 });
                    return;
                }
                dataloading();
                $("#Save_btn").click();
            });
        });
    </script>
</head>
<body data-pagecode="R_DataSource">
    <form id="form1" data-tbname="R_DataSource" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="R_DataSourcelist" PageType="Normal" MainMenu="" SubMenu="报表数据源" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="报表数据源" Operate="Normal" runat="server"></cc1:CPageTitle>
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
                        <td data-code="span_stocode" class="lefttd">门店：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_stocode" SelType="Normal" runat="server" CssClass="selstyle"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_date" class="lefttd">选定日期：</td>
                        <td>
                            <input type="text" readonly="readonly" class="datetxt" id="txt_startdate" onfocus="ShowShortDate('maxDate:\'%y-%M-%d\'');" runat="server" style="height: 25px;" />&nbsp;至&nbsp;
                            <input type="text" readonly="readonly" class="datetxt" id="txt_enddate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(\'txt_startdate\')}',maxDate:'#F{$dp.$D(\'txt_startdate\',{d:31});}'});" runat="server" style="height: 25px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td colspan="5">
                            <div class="savebtn1" id="btnCal">执行计算</div>
                            <div class="gobackbtn" data-code="back_edit">返回</div>
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
