<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminsedit.aspx.cs" Inherits="CommunityBuy.BackWeb.manage.adminsedit" %>

<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script type="text/javascript" src="/js/jquery-1.7.2.min.js"></script>
    <script src="/js/CWebControl.js"></script>
    <script src="/js/listeditjs.js"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <script src="/js/default.js"></script>
    <style type="text/css">
        .simg {
            width: 20px;
            height: 20px;
            vertical-align: middle;
            margin-left: 5px;
            margin-right: 5px;
            cursor: pointer;
        }
    </style>
    <script type="text/javascript">
        function selstore() {
            if ($('#ddl_scope').val() == '2') {
                $('#txt_stocode').hide();
                $('#img_select').hide();
                $("#lblstoname").show();
            }
            else {
                $('#txt_stocode').show();
                $('#img_select').show();
                $("#lblstoname").hide();
            }
        }

        function selectRole() {
            var title = '选择角色';
            ShowReferPage('hidroleid', 'txt_role', title, 'selectrole.aspx', 1000, '85%', '80%');
        }

        $(document).ready(function () {

        });
    </script>
</head>
<body data-pagecode="admins">
    <form id="form1" data-tbname="admins" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="adminslist" PageType="Add" MainMenu="系统管理" SubMenu="用户管理" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="用户管理" Operate="新建" runat="server"></cc1:CPageTitle>
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
                        <td data-code="span_role" class="lefttd">所属角色：</td>
                        <td>
                            <cc1:CTextBox ID="txt_role" CssClass="txtstyle" IsRequired="true" TextType="Normal" onfocus="selectRole();" Width="500" runat="server" placeholder="请选择角色"></cc1:CTextBox><img src="/img/search.png" onclick="selectRole();" class="simg" /><input type="hidden" id="hidroleid" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_uname" class="lefttd">用户名：</td>
                        <td>
                            <cc1:CTextBox ID="txt_uname" data-code="name_placeholder" CssClass="reqtxtstyle" MaxLength="64" IsRequired="true" TextType="User" onblur="onblurCheck('txt_uname')" runat="server"></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td data-code="span_pwd" class="lefttd">密码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_pwd" data-code="pwd_placeholder" CssClass="reqtxtstyle" MaxLength="64" IsRequired="true" TextType="Pwd" onblur="onblurCheck('txt_pwd')" runat="server" placeholder="6-16位数字字母组合" TextMode="SingleLine"></cc1:CTextBox></td>
                    </tr>
                    <%--<tr>
                        <td data-code="span_cname" class="lefttd">真实姓名：</td>
                        <td>
                            <cc1:CTextBox ID="txt_cname" data-code="cname_placeholder" CssClass="reqtxtstyle" MaxLength="64" IsRequired="false" TextType="Normal" onblur="onblurCheck('txt_cname')" Enabled="false" runat="server" placeholder="2-16位字符"></cc1:CTextBox></td>
                    </tr>--%>
<%--                    <tr>
                        <td data-code="span_empcode" class="lefttd">所属员工：</td>
                        <td>
                            <input type="hidden" id="hid_empcode" runat="server" />
                            <cc1:CTextBox ID="txt_empcode" data-code="empcode_placeholder" CssClass="txtstyle" MaxLength="16" IsRequired="true" TextType="Normal" onfocus="showemppage();" runat="server" placeholder="请选择员工"></cc1:CTextBox><img src="/img/search.png" onclick="showemppage();" class="simg" /><input type="hidden" id="hidempcode" runat="server" /></td>

                    </tr>--%>
  <%--                  <tr>
                        <td data-code="span_scope" class="lefttd">权限范围：</td>
                        <td>
                            <input type="hidden" id="hidstore" runat="server" />
                            <cc1:CDropDownList ID="ddl_scope" data-code="scope_placeholder" CssClass="selstyle" Width="60" IsRequired="True" SelType="AccScope" onchange="selstore();" runat="server"></cc1:CDropDownList>
                            <cc1:CTextBox ID="txt_stocode" data-code="stocode_placeholder" CssClass="txtstyle" IsRequired="false" Width="160" TextType="Normal" onfocus="" runat="server" placeholder="请选择门店" onclick="selectStore()"></cc1:CTextBox>
                            <img id="img_select" src="/img/search.png" onclick="selectStore()" class="simg" />
                            <asp:Label ID="lblstoname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td data-code="span_status" class="lefttd">状态：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_status" CssClass="wheresel" SelType="Status" IsSearch="false" runat="server" IsNotNull="True"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_descr" class="lefttd">备注：</td>
                        <td colspan="5">
                            <cc1:CTextBox ID="txt_descr" data-code="descr_placeholder" placeholder="描述最长128个字符" CssClass="txtstyle" MaxLength="128" IsRequired="false" TextType="Normal" runat="server" Width="500" FormDescr=""></cc1:CTextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td colspan="5">
                            <div data-code="save_edit" class="savebtn">保存</div>
                            <div data-code="back_edit" class="gobackbtn">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" OnClick="Save_btn_Click" />&nbsp;&nbsp; <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <script type="text/javascript">
            function adminedit(chooseArr) {
                if (chooseArr != undefined) {
                    $("#hidempcode").val(chooseArr[0].empcode);
                    $("#txt_empcode").val(chooseArr[0].empname);
                    $("#lblstoname").html(chooseArr[0].stoname);
                }
            }
        </script>
    </form>
</body>
</html>
