<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="couponcheck.aspx.cs" Inherits="CommunityBuy.BackWeb.couponcheck" %>

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
    <script src="/js/MY97DATE/WdatePicker.js"></script>
    <script src="/js/datehelper.js"></script>
    <style type="text/css">
        .rounds {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            cursor: pointer;
            float: left;
            background-color: #eae4e4;
        }

        .roundson {
            background-color: #3f4461;
        }

        .rounds-span {
            line-height: 30px;
            margin-left: 4px;
            margin-right: 10px;
            float: left;
        }

        .lefttd {
            width: 100px;
            text-align: right;
        }

        #tbcoupon .trtoday td {
            background-color: #ccb607;
        }

        #tbcoupon {
            width: 100%;
            text-align: center;
            border: 1px solid #ccc;
            border-collapse: collapse;
            font-size: 16px;
        }

            #tbcoupon tbody td {
                height: 25px;
                border: 1px solid #ccc;
                background-color: #E1E1E1;
                font-size: 16px;
            }

            #tbcoupon tbody tr td {
                height: 25px;
                border: 1px solid #ccc;
                text-align: center;
                background-color: #E1E1E1;
                font-size: 16px;
            }
        .auto-style1 {
            width: 90px;
            text-align: right;
            height: 50px;
        }
        .auto-style3 {
            width: 539px;
        }
    </style>
    <script type="text/javascript">
        function entermob(obj) {
            var Num = $(obj).val().replace(/\s+/g, "");
            if (Num.length > 12) {
                $(obj).val(Num.substring(0, 4) + " " + Num.substring(4, 8) + " " + Num.substring(8, 12) + " " + Num.substring(12, Num.length));
                return;
            }
            else if (Num.length > 8) {
                $(obj).val(Num.substring(0, 4) + " " + Num.substring(4, 8) + " " + Num.substring(8, Num.length));
                return;
            }
            else if (Num.length > 4) {
                $(obj).val(Num.substring(0, 4) + " " + Num.substring(4, Num.length));
                return;
            }
        }
        function freego() {
            $('#Free_btn').hide();
            return true;
        }
    </script>
</head>
<body data-pagecode="couponcheck">
    <form id="form1" data-tbname="couponcheck" runat="server">
        <div style="line-height: 30px; height: 30px;">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="couponcheck" PageType="Normal" MainMenu="" SubMenu="优惠券消券" runat="server"></cc1:CPathBar>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
            </div>
            <div class="updatediv cla">
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="Updatepanel1" runat="server" UpdateMode="Always">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="readCard" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td data-code="span_coupon" class="auto-style1" style="line-height: 50px; ">券码：</td>
                                <td colspan="2">
                                    <cc1:CTextBox ID="txt_coupon" data-code="coupon_placeholder" CssClass="txtstyle" MaxLength="19" IsRequired="False" TextType="Normal" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="#FF3300" Height="40px" onkeyup="entermob(this);" Width="282px"></cc1:CTextBox>&nbsp;<asp:Button ID="readCard" Style="width: 120px; color: #ffffff; height: 44px; background-color: #3f4461; font-size: 16px;" runat="server" data-code="readcard" Text="查询资料" OnClick="readCard_Click" BorderStyle="None" Font-Size="Medium" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Free_btn" Style="width: 160px; color: #ffffff; height: 44px; background-color: #FF3300; font-size: 20px; font-weight: bold;" runat="server" data-code="free_btn" Text="优惠券消券" OnClick="Save_btn_Click" BorderStyle="None" OnClientClick="return freego();" Visible="False" />

                                    <span id="errormessage" style="color: #05640d; font-size: 20px;" runat="server"></span>
                                </td>
                            </tr>
                             <tr>
                                <td></td>
                                <td class="auto-style3">
                                </td>
                                <td id="tb_freeinfo" style="width:480px;" rowspan="4" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td data-code="span_scid" class="lefttd">优惠券信息：</td>
                                <td valign="top" class="auto-style3">
                                    <asp:Label ID="lbshowname" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td id="couponsinfo" style="font-size:16px;font-weight:bold; vertical-align:top;" runat="server" class="auto-style3"></td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="hidden" id="hidcoupons" runat="server" />
                                    <input type="hidden" id="hidecardcode" runat="server" />
                                    <input type="hidden" id="hidmemcode" runat="server" />
                                    <input type="hidden" id="hidshownum" runat="server" />
                                    <input type="hidden" id="hidisfree" runat="server" />
                                    <input type="hidden" id="Hidpcode" runat="server" />
                                    <input type="hidden" id="hidfreenum" runat="server" />
                                    <input type="hidden" id="hidnum" runat="server" />
                                    <input type="hidden" id="hidId" runat="server" />
                                </td>
                                <td class="auto-style3"></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="updatediv clb" style="display: none"></div>
            <div class="bottomline"></div>
        </div>
    </form>
</body>
</html>
