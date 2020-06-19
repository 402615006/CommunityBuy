<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dishesInportMate.aspx.cs" Inherits="CommunityBuy.BackWeb.dishesInportMate" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>
<%@ Register Src="/UserControls/ToolBar.ascx" TagPrefix="uc2" TagName="ToolBar" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
     <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/xmlhelper.js"></script>
    <script src="/js/layer/layer.js"></script>
    <script src="/js/default.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layerhelper.js"></script>
    <script type="text/javascript">
        function showerr() {
            $("#errpanl").css("display", "block");
        }
        function Button1Click1() {
            $("#Button1").click();
        }
        function Button2Click1() {
            $("#Button2").click();
        }
         //返回上一页
        function backform() {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }
    </script>
    <style>
        /* 回收站 */
        .recyble {
            color: #747474;
            font-size: 18px;
            margin-bottom: 18px;
        }

        .btn-recyble {
            width: 25px;
            height: 25px;
        }

        .recyble .btn-recyble.on {
            background: url('/images/on.png') no-repeat 0 0;
        }
    </style>
</head>
<body data-pagecode="dishes">
    <form id="form1" data-tbname="dishes" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="disheslist" PageType="List" MainMenu="出品管理" SubMenu="成本卡导入" runat="server"></cc1:CPathBar>
            </div>
            <div style="display:block;color:red;"></div>
            <div style="display:block;height:35px;line-height:35px;margin-left:30px;">
                <cc1:CPageTitle ID="PageTitle" Menu="出品管理" Operate="成本卡导入" runat="server">
                     <a href="javascript:void(0);" onclick="backform()">返回</a>&nbsp;
                </cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
           
            <div class="hintmess" id="sp_showmes" runat="server">&nbsp;</div>

            <div class="righttable">
                <table style="margin-left: 30px;">
                    <tr style="height:80px;">
                        <td>选择门店</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_stocode" Descr="部门" Width="140" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr style="height:80px;">
                        <td>选择文件</td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            
                        </td>
                    </tr>
                    <tr style="height:80px;">
                        <td colspan="2" style="text-align: center">
                            <div class="savebtn1" onclick="Button1Click1()">导入成本卡</div>
                            <asp:Button ID="Button1" runat="server" CssClass="operlinkbtn" Text="导入成本卡" OnClick="Button1_Click1" style="display:none;" />
                        </td>
                    </tr>
                </table>
                 <input id="hiddate" type="hidden" runat="server" />
                <div id="errpanl" style="display:none;margin-top:30px;width:700px;height:100%;text-align:center;">
                    <div>
                        <h4>以下成本卡信息中的单位或原料未匹配到,无法导入。</h4>
                    </div>
                    <div id="errmate" runat="server" style="margin-top:20px; display:block;width:700px;overflow-wrap:break-word;text-align:center;overflow-y:auto;font-size:20px;">

                    </div>
                   <%-- <div style="display: block;width: 100%;padding-left: 45%;">
                        <div class="savebtn1" onclick="Button2Click1()">继续导入</div>
                        <asp:Button ID="Button2" runat="server" Text="继续导入" CssClass="operlinkbtn" OnClick="Button2_Click" style="display:none;"/>
                    </div>--%>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
