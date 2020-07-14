<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rolefunctionedit.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.rolefunctionedit" %>

<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script type="text/javascript" src="/js/jquery-1.7.2.min.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/RolmasChoice.js" type="text/javascript"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".savebtn").click(function () {
                if (PageCheck() == true) {
                    document.getElementById("Save_btn").click();
                }
            });
            $(".gobackbtn").click(function () {
                var _iframe = window.parent.document;
                var _btn = window.parent.document.getElementById("ToolBar1_LinkRefresh");
                if ($(_iframe).find(".layui-layer-title").attr("move") != undefined) {
                    if (_btn != undefined) {
                        _btn.click();
                    }
                    //parent.location.reload();
                    parent.layer.closeAll("iframe");
                }
                //location.href = "rolefunctionlist.aspx";
            });
            //selstore();
        });

        //默认参数
        var para = {
            "actionname": "savedata"
        };

        function PageCheck() {
            var flag = true;
            GetFunIdStr();
            return flag;
        }

        function hidebutton() {
            $(".savebtn").hide();
        }
    </script>
    <style type="text/css">
        #MenuList {
            width: 100%;
            /*margin-left: 50px;*/
        }

        .lefttd {
            width: 130px;
        }
    </style>
</head>
<body data-pagecode="rolefunction">
    <form id="form1" data-tbname="rolefunction" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="rolefunctionlist" PageType="Normal" MainMenu="系统管理" SubMenu="角色管理" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="角色管理" Operate="新建" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div class="graydiv seldiv" title="cla">基本信息</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <%--<td data-code="span_scope" class="lefttd">权限范围：</td>
                        <td style="width: 350px;">
                            <input type="hidden" id="hidstore" runat="server" />
                            <cc1:CDropDownList ID="ddl_scope" data-code="scope_placeholder" CssClass="selstyle" Width="60" IsRequired="True" SelType="AccScope" onchange="selstore();" runat="server"></cc1:CDropDownList>
                            <cc1:CTextBox ID="txt_stocode" data-code="stocode_placeholder" CssClass="txtstyle" IsRequired="false" Width="160" TextType="Normal" onfocus="" runat="server" placeholder="请选择门店" onclick="selectStore()"></cc1:CTextBox>
                            <img id="img_select" src="/img/search.png" onclick="selectStore()" class="simg" />
                        </td>--%>
                        <td data-code="span_name" class="lefttd">角色名称：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="rol_name" runat="server" placeholder="请输入角色名称" CssClass="reqtxtstyle" MaxLength="64" IsRequired="true" TextType="Normal" onblur="onblurCheck('rol_name')"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td data-code="span_status" class="lefttd">角色状态：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_status" data-code="status_placeholder" CssClass="selstyle" Width="60" IsRequired="True" SelType="Status" runat="server"></cc1:CDropDownList>
                        </td>
                        <td data-code="span_descr" class="lefttd">角色描述：</td>
                        <td>
                            <cc1:CTextBox ID="rol_descr" runat="server" data-code="descr_placeholder" placeholder="" CssClass="txtstyle" MaxLength="128" IsRequired="False" TextType="Normal" Width="300" FormDescr=""></cc1:CTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div id="MenuList" runat="server">
                            </div>
                            <input id="HidfunIdStr" name="HidfunIdStr" runat="server" type="hidden" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td colspan="3">
                            <div class="savebtn">
                                保存
                            </div>
                            <div class="gobackbtn">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" IsFormValidation="false" />
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
