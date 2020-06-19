<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ts_syssetEdit.aspx.cs" Inherits="CommunityBuy.BackWeb.ts_syssetEdit" %>

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
</head>
<body data-pagecode="ts_sysset">
    <form id="form1" data-tbname="ts_sysset" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="ts_syssetlist" PageType="Add" MainMenu="" SubMenu="系统设置" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="系统设置" Operate="Add" runat="server"></cc1:CPageTitle>
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

                        <td data-code="span_key" class="lefttd">键：</td>
                        <td>
                            <cc1:CTextBox ID="txt_key" data-code="key_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_key')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_key_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_val" class="lefttd">值：</td>
                        <td>
                            <cc1:CTextBox ID="txt_val" data-code="val_placeholder" CssClass="txtstyle" MaxLength="16" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_val')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_val_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_status" class="lefttd">状态：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_status" data-code="status_placeholder" Descr="父节点" CssClass="selstyle" TextType="Normal" runat="server" SelType="Status">
                            </cc1:CDropDownList><span class="redmess" id="txt_status_span"></span>							</td>
                    </tr>
                    <tr>
                        <td data-code="span_descr" class="lefttd">描述：</td>
                        <td>
                            <%--       <textarea id="txt_descr" data-code="descr_placeholder" cssclass="txtstyle" maxlength="128" style="height: '100px' Width:'500px'" 			
                            --%>
                            <textarea data-code="descr_placeholder" class="txtstyle" maxlength="128" style="height: 50px; width: 500px" id="txt_descr" isrequired="False" texttype="Normal" textmode="MultiLine" onblur="onblurCheck('txt_descr')" placeholder="" runat="server"></textarea><span class="redmess" id="txt_descr_span"></span>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_explain" class="lefttd">说明：</td>
                        <td>
                            <textarea data-code="explain_placeholder" class="txtstyle" maxlength="128" style="height: 100px; width: 500px" id="txt_explain" isrequired="False" texttype="Normal" textmode="MultiLine" onblur="onblurCheck('txt_explain')" placeholder="" runat="server"></textarea><span class="redmess" id="txt_explain_span"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidId" runat="server" /></td>
                        <td>
                            <div class="savebtn">保存</div>
                            <div class="gobackbtn">返回</div>
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
