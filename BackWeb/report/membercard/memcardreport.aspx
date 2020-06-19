<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memcardreport.aspx.cs" Inherits="CommunityBuy.BackWeb.report.membercard.memcardreport" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <link href="/css/stockreport.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <script src="/js/CWebControl.js"></script>
    <script src="/js/card.js"></script>
    <script src="/js/base-loading.js"></script>
    <script src="/js/tablefreeze.js"></script>
    <style type="text/css">
        .wherename {
            width: 80px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            getcardtype();
        });

        function selectStoInfo() {
            var title = getNameByCode('storerefer');
            ShowReferPage('hidstocode', 'txt_stoname', title, '/manage/selectStroe.aspx', 100, '80%', '80%');
        }

        function tbfreeze() {
            freeTable('reportList', 250);
        }

        function setselecttype() {
            $("#hidctypename").val($("#sel_ctype").find("option:selected").text());
        }
        function setselectlevel() {
            $("#hidlevelname").val($("#sel_cracode").find("option:selected").text());
        }
    </script>
</head>
<body data-pagecode="memcardreport" style="overflow: hidden;">
    <form id="form1" data-tbname="memcardreport" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memcardreport" PageType="List" MainMenu="" SubMenu="会员卡报表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员卡报表" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="rightwhere">
                <ul>
                    <li data-code="stime_where" class="wherename">开始时间：</li>
                    <li class="wherevale" style="width: 300px;">
                        <input type="text" class="datetxt" id="txt_btime" runat="server" onfocus="ShowLongDate('startDate:\'%y-%M-%d 07:00:00\'');" style="width: 140px;" /></li>
                </ul>
                <ul>
                    <li data-code="etime_where" class="wherename">结束时间：</li>
                    <li class="wherevale" style="width: 300px;">
                        <input type="text" class="datetxt" id="txt_etime" runat="server" onfocus="ShowLongDate('startDate:\'%y-%M-%d 07:00:00\'');" style="width: 140px;" /></li>
                </ul>
                <ul>
                    <li data-code="stoname_where" class="wherename">门店：</li>
                    <li class="wherevale" style="width: 300px;">
                        <input type="text" class="wheretxt" id="txt_stoname" runat="server" onfocus="selectStoInfo()" />
                    </li>
                </ul>
                <ul>
                    <li data-code="ctype_where" class="wherename">会员卡类型：</li>
                    <li class="wherevale" style="width: 300px;">
                        <select id="sel_ctype" cssclass="wheresel" seltype="Normal" runat="server" onchange="getcardlevel();setselecttype();" style="min-width: 120px; height: 30px">
                        </select>
                        <input type="hidden" runat="server" id="hidctype" />
                    </li>
                </ul>
                <ul>
                    <li data-code="cracode_where" class="wherename">会员卡等级：</li>
                    <li class="wherevale" style="width: 300px;">
                        <input type="hidden" runat="server" id="hidcracode" />
                        <select id="sel_cracode" cssclass="wheresel" seltype="Normal" runat="server" style="height: 30px; min-width: 120px" onchange="setlevel();setselectlevel();">
                        </select>
                    </li>
                </ul>
            </div>
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
            <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" />
            <div id="lstData" class="righttable" style="width: 100%; overflow: auto;" runat="server">
            </div>
            <input type="hidden" id="hidstocode" runat="server" />
            <input type="hidden" runat="server" id="hidlevelname" />
            <input type="hidden" runat="server" id="hidctypename" />
        </div>
    </form>
</body>
</html>
