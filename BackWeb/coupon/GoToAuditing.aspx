<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoToAuditing.aspx.cs" Inherits="CommunityBuy.BackWeb.GoToAuditing" %>

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
    <script src="/js/default.js"></script>
    <style type="text/css">
        .fixtoph {
            height: 30px;
            line-height: 30px;
        }
    </style>
    <script type="text/javascript">
        function gotoAuditing(type)
        {
            var remark = $("#txt_remark").val();
            if (remark.length==0) {
                pcLayerMsg('请输入审核备注！');
                return;
            }
            $("#hidtype").val(type);
            document.getElementById("btn_save").click();
        }

        function closeparent() {
            pcLayerMsg('已审核成功！');
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }
    </script>
</head>
<body data-pagecode="couponpresent">
    <form id="form1" data-tbname="StockApply" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
                 <cc1:CPathBar ID="pathBar" PageCode="" PageType="Normal" Visible="false" MainMenu="" SubMenu="优惠券赠送" runat="server"></cc1:CPathBar>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td  class="lefttd">审核备注：</td>
                        <td>
                            <textarea id="txt_remark" class="txtstyle" runat="server" style="height: 90px; width: 90%;"></textarea></td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none">
            </div>
            <div class="bottomline">
            </div>
            <div style="width: 100%">
                <table>
                    <tr>
                        <td style="width: 180px;">
                            <input type="hidden" id="hidtype" runat="server" />
                            <input type="hidden" id="hidId" runat="server" />
                        </td>
                        <td colspan="5">
                            <div class="gobackbtn" data-code="back_edit">返回</div>
                            <div class="addbtn" id="okbtn" onclick="gotoAuditing('1');">通过</div>
                            <div class="rejectbtn" id="nobtn" onclick="gotoAuditing('0');">拒绝</div>
                            <asp:Button ID="btn_save" runat="server" Text="Button" Style="display: none" OnClick="btn_save_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="5"><span id="errormessage" runat="server"></span></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
