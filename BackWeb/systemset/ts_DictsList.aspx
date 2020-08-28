<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ts_DictsList.aspx.cs" Inherits="CommunityBuy.BackWeb.systemset.ts_DictsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/layui/layui.all.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <link href="../js/zTree/css/demo.css" rel="stylesheet" />
    <link href="../js/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <script src="../js/zTree/js/jquery.ztree.core.js"></script>
    <script>
        var setting = {
            data: {


            },
            callback: {
                //beforeClick: beforeClick,
                onClick: onClick
            }
        };
        function onClick(event, treeId, treeNode) {
            $('#hidtreid').val(treeNode.id);

            GpAjaxGet('/ajax/system/S_dict.ashx', 'type=dictsigle&id=' + treeNode.id, true, settreeinfo);
        }

        function settreeinfo(data) {
            //alert(data['dictlist']);
            //console.log(data[0].orderno);
            //$("parentName").html(data["dictlist"][0].orderno);

            $('#parentName').text(data[0].pname);
            $('#diccode').text(data[0].diccode);
            $('#dicname').text(data[0].dicname);
            $('#orderno').text(data[0].orderno);
            $('#status').text(data[0].status == 0 ? "无效" : "有效");
            $('#remark').text(data[0].remark);
        }

        //ajax再封装,purl:地址,param:参数json,isasync:是否异步,funback回调方法，requesttype请求手机头标识
        //function GpAjax(purl, action, param, isasync, funback, requesttype) {


        $(function () {

            GpAjaxGet('/ajax/system/S_dict.ashx', 'type=dict', true, settreeData);

        });

        function settreeData(data) {
            //console.log("ok");
            $.fn.zTree.init($("#treeDemo"), setting, data);
        }

    </script>
    <style>
        .showdeptinfo {
            padding-top: 15px;
            margin-left: auto;
            line-height: 20px;
            width: 550px;
            height: 550px;
            border: 1px solid #eeeeee;
            background-color: #eeeeee;
        }

            .showdeptinfo div {
                margin-top: 30px;
                margin-left: 50px;
                text-align: left;
                border-bottom: 0.5px solid #000000;
                margin-right: 100px;
            }
    </style>
</head>

<body data-pagecode="ts_Dicts">
    <form id="form1" data-tbname="ts_Dicts" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="ts_Dictslist" PageType="List" MainMenu="" SubMenu="系统字典信息" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="系统字典信息" Operate="List" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="zTreeDemoBackground" style="height: 612px; background-color: #eee; float: left; overflow-x: hidden; overflow-y: auto;">
                <ul id="treeDemo" class="ztree"></ul>
                <input type="hidden" id="hidtreid" runat="server" />
            </div>

            <div style="float: left">

                <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>
                <uc2:ToolBar runat="server" ID="ToolBar1" OnToolBarClick="ToolBar1_Click" />
                <div class="showdeptinfo">
                    <div>
                        <span data-code="parentName_list"></span><span id="parentName"></span>
                    </div>

                    <div>
                        <span data-code="diccode_list"></span><span id="diccode"></span>
                    </div>


                    <div>
                        <span data-code="dicname_list"></span><span id="dicname"></span>


                    </div>

                    <div>
                        <span data-code="orderno_list"></span><span id="orderno"></span>
                    </div>

                    <div>
                        <span data-code="status_list"></span><span id="status"></span>
                    </div>

                    <div>
                        <span data-code="remark_list"></span><span id="remark"></span>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
