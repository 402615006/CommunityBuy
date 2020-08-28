<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dishTypeList.aspx.cs" Inherits="CommunityBuy.BackWeb.dish.dishTypeList" %>

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
                key: { name: "name",isParent:"isParent" },
                simpleData: {idKey:"id",pIdKey:"pid"}
            },
            callback: {
                onClick: onClick
            }
        };
        function onClick(event, treeId, treeNode) {
            $('#diccode').text(treeNode.id);
            $('#txt_dicname').val(treeNode.name);
            $('#hidpkcode').val(treeNode.id);
            $('#txt_sort').val(treeNode.sort);
        }

        $(function () {
            GpAjaxGet('/ajax/dishes/WSDisheType.ashx', 'actionname=getlist&parameters={"GUID":"", "USER_ID":"", "pageSize":9999, "currentPage":1, "filter":"", "order":""}', true, settreeData);
        });

        function settreeData(data) {
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

<body data-pagecode="dishType">
    <form id="form1" data-tbname="dishType" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="dishType" PageType="List" MainMenu="" SubMenu="商品类别" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="商品类别" Operate="List" runat="server"></cc1:CPageTitle>
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
 <%--                   <div>
                        <span>父级分类：</span><span id="parentName"></span>
                    </div>--%>

                    <div>
                        <span>编号：</span><span id="diccode"></span>
                    </div>
                    <div>
                        <span>名称：<cc1:CTextBox ID="txt_dicname" ClientIDMode="Inherit" data-code="dicname_placeholder" MaxLength="32" IsRequired="False" TextType="Normal"  placeholder="" runat="server"></cc1:CTextBox></span>
                    </div>
                    <asp:HiddenField ID="hidpkcode" runat="server" ClientIDMode="Inherit" />
                    <div>
                        <span>排序号：<cc1:CTextBox ID="txt_sort" ClientIDMode="Inherit" data-code="dicname_placeholder" MaxLength="32" IsRequired="False" TextType="Normal"  placeholder="" runat="server"></cc1:CTextBox></span>
                    </div>
                    <div>
                        <span>状态：</span><span id="status"></span>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
