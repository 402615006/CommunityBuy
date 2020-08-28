<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membersDetail.aspx.cs" Inherits="CommunityBuy.BackWeb.membersDetail" %>

<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/liststyle.css" rel="stylesheet" />
    <link href="/css/editstyle.css" rel="stylesheet" />
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/js/CWebControl.js" type="text/javascript"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="/js/layui/layui.all.js"></script>
    <script src="/js/layerhelper.js"></script>
    <script src="/js/xmlhelper.js"></script>
    <style type="text/css">
        #MenuList {
            width: 100%;
            margin-left: 50px;
        }

        .simg {
            width: 20px;
            height: 20px;
            vertical-align: middle;
            margin-left: 5px;
            margin-right: 5px;
            cursor: pointer;
        }

        .delete {
            height: 20px;
            width: 20px;
            line-height: 20px;
        }

        .lefttd {
            width: 100px;
            text-align: right;
        }
    </style>
    <script>
        $(document).ready(function () {
            if ($('#hidbigcustomer').val() == "1") {
                $('#cb_bigcustomer').attr('src', '/img/on.png');
            }
        });

        function showmembercoupon(cardcode, memcode, sumcode, mccode, status, num) {
            if (num == 0) {
                return;
            }
            var linkstr = 'membercouponDetail.aspx?cardcode=' + cardcode + '&memcode=' + memcode + '&status=' + status + '&sumcode=' + sumcode + '&mccode=' + mccode;
            var title = getNameByCode('goto_coupon');
            showpage(title, linkstr);
        }

        function showpage(title, linkstr) {
            var index = layui.layer.open({
                title: title,
                type: 2,
                area: ['100%', '100%'],
                fix: true, //不固定
                maxmin: false,
                content: linkstr
            });
            layui.layer.full(index);
        }
    </script>
</head>
<body data-pagecode="members">
    <form id="form1" data-tbname="members" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="memberslist" PageType="Normal" MainMenu="" SubMenu="会员信息表" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="会员信息表" Operate="Add" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div data-code="tab1" class="graydiv seldiv" title="cla">基本信息</div>
                <div class="graydiv" data-code="cardinfo" id="clb" title="clb">会员卡信息</div>
                <div data-code="tabcoupon" class="graydiv" id="p_clc" title="clc">优惠券信息</div>
            </div>
            <div class="updatediv cla">
                <table>
                    <tr>
                        <td data-code="span_source" class="lefttd">会员来源：</td>
                        <td>
                            <cc1:CDropDownList ID="txt_source" CssClass="selstyle" IsRequired="False" SelType="Normal" runat="server"></cc1:CDropDownList>
                        </td>
                        <td data-code="span_wxaccount" class="lefttd">微信账户：</td>
                        <td>
                            <cc1:CTextBox ID="txt_wxaccount" CssClass="txtstyle" MaxLength="20" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_wxaccount')" placeholder="" runat="server"></cc1:CTextBox></td>
                    </tr>
                    <tr style="display: none;">
                        <td data-code="span_strcode" class="lefttd">所属门店：</td>
                        <td>
                            <input type="hidden" id="hidstore" runat="server" />
                            <cc1:CTextBox ID="txt_stocode" data-code="strcode_placeholder" CssClass="txtstyle" MaxLength="8" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_strcode')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_strcode_span"></span>
                            <img src="/img/search.png" onclick="selectStore()" class="simg" style="display: none" />
                        </td>
                        <td data-code="span_bigcustomer" class="lefttd">是否大客户：</td>
                        <td>
                            <img src="/img/off.png" id="cb_bigcustomer" class="btn-recyble" runat="server" />
                            <input type="hidden" id="hidbigcustomer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_cname" class="lefttd">姓名：</td>
                        <td>
                            <cc1:CTextBox ID="txt_cname" data-code="cname_placeholder" CssClass="txtstyle" MaxLength="32" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_cname')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_cname_span"></span>						</td>
                        <td data-code="span_sex" class="lefttd">性别：</td>
                        <td>
                            <cc1:CDropDownList ID="txt_sex" CssClass="selstyle" IsRequired="False" SelType="Sex" runat="server"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_mobile" class="lefttd">手机号码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_mobile" data-code="mobile_placeholder" CssClass="reqtxtstyle" MaxLength="12" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_mobile')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_mobile_span"></span>						</td>
                        <td data-code="span_birthday" class="lefttd">生日：</td>
                        <td>
                            <cc1:CTextBox ID="txt_birthday" data-code="birthday_placeholder" CssClass="txtstyle" MaxLength="4" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_birthday')" onfocus="ShowShortDate();" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_birthday_span"></span>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_idtype" class="lefttd">证件类型：</td>
                        <td>
                            <cc1:CDropDownList ID="ddl_idtype" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                        </td>
                        <td data-code="span_IDNO" class="lefttd">证件号码：</td>
                        <td>
                            <cc1:CTextBox ID="txt_IDNO" data-code="IDNO_placeholder" CssClass="reqtxtstyle" MaxLength="20" IsRequired="true" TextType="Normal" onblur="onblurCheck('txt_IDNO')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_IDNO_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_email" class="lefttd">邮箱地址：</td>
                        <td>
                            <cc1:CTextBox ID="txt_email" CssClass="txtstyle" MaxLength="128" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_email')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_email_span"></span>						</td>
                        <td data-code="span_tel" class="lefttd">电话：</td>
                        <td>
                            <cc1:CTextBox ID="txt_tel" CssClass="txtstyle" MaxLength="64" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_tel')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_tel_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_provinceid" class="lefttd">所属区域：</td>
                        <td colspan="3">
                            <cc1:CDropDownList ID="ddl_provinceid" CssClass="selstyle" IsRequired="False" SelType="Normal" runat="server" OnSelectedIndexChanged="ddl_provinceid_SelectedIndexChanged" AutoPostBack="true"></cc1:CDropDownList>
                            <cc1:CDropDownList ID="ddl_cityid" CssClass="selstyle" IsRequired="False" SelType="Normal" runat="server" OnSelectedIndexChanged="ddl_cityid_SelectedIndexChanged" AutoPostBack="true"></cc1:CDropDownList>
                            <cc1:CDropDownList ID="ddl_areaid" Descr="所属区域" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_photo" class="lefttd">照片：</td>
                        <td>
                            <div>
                                <div style="padding-top: 10px; float: left">
                                    <img runat="server" id="txt_photo" width="200" height="200" style="float: left" />
                                </div>
                            </div>
                        </td>
                        <td data-code="span_signature" class="lefttd">电子签名：</td>
                        <td>
                            <div style="padding-top: 10px; float: left">
                                <img runat="server" id="txt_signature" width="200" height="200" style="float: left" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td data-code="span_address" class="lefttd">会员地址：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_address" data-code="address_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_address')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_address_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_hobby" class="lefttd">特殊爱好：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_hobby" data-code="hobby_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_hobby')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_hobby_span"></span>						</td>
                    </tr>
                    <tr>
                        <td data-code="span_remark" class="lefttd">备注：</td>
                        <td colspan="3">
                            <cc1:CTextBox ID="txt_remark" data-code="remark_placeholder" CssClass="txtstyle" MaxLength="128" Width="80%" Height="150px" IsRequired="False" TextType="Normal" onblur="onblurCheck('txt_remark')" placeholder="" runat="server"></cc1:CTextBox><span class="redmess" id="txt_remark_span"></span>						</td>
                    </tr>
                    <tr>
                        <td>
                            <input type="hidden" id="hidprovince" runat="server" />
                            <input type="hidden" id="hidcity" runat="server" />
                            <input type="hidden" id="hidarea" runat="server" />
                            <input type="hidden" id="hidId" runat="server" />
                        </td>
                        <td>
                            <div class="backbtn">返回</div>
                            <cc1:CButton ID="Save_btn" runat="server" Style="display: none" />
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="updatediv clb" style="display: none">
                <iframe id="frame" src="<%=url%>" style="width: 100%; height: 500px; border: 0px;"></iframe>
            </div>
            <div class="updatediv clc" style="display: none" id="clc">
                <div class="righttable" style="margin-top: 30px">
                    <cc1:CustDataGrid ID="gv_list" runat="server" Width="100%" rules="none" CellPadding="4"
                        EnableViewState="true" AutoGenerateColumns="False"
                        AllowSorting="True" GridLines="None" CssClass="List_tab" PagerID="GridPager1"
                        SeqNo="0">
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <AlternatingItemStyle BackColor="#EFF3FB" CssClass="List_tab td" />
                        <ItemStyle BackColor="White" CssClass="List_tab td" />
                        <Columns>
                            <asp:BoundColumn DataField="cardcode" HeaderText="<span data-code='cardCode_list' >会员卡号</span>" />
                            <asp:BoundColumn DataField="waysname" HeaderText="<span data-code='waysname_list' >适用渠道</span>" />
                            <asp:BoundColumn DataField="storename" HeaderText="<span data-code='usingrange' >使用门店</span>" />
                            <asp:BoundColumn DataField="distypename" HeaderText="<span data-code='usetype' >使用大类</span>" />
                            <asp:BoundColumn DataField="couname" HeaderText="<span data-code='couponname' >优惠券名称</span>" />
                            <asp:TemplateColumn>
                                <HeaderTemplate><span data-code='couponnum'>数量</span></HeaderTemplate>
                                <ItemTemplate><a href="#" onclick="showmembercoupon('<%#Eval("cardcode").ToString() %>','<%#Eval("memcode").ToString() %>','<%#Eval("sumcode").ToString() %>','<%#Eval("mccode").ToString() %>','',<%#Eval("total").ToString() %>)"><%#Eval("total").ToString() %></a></ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate><span data-code='couponnotuse'>未使用</span></HeaderTemplate>
                                <ItemTemplate><a href="#" onclick="showmembercoupon('<%#Eval("cardcode").ToString() %>','<%#Eval("memcode").ToString() %>','<%#Eval("sumcode").ToString() %>','<%#Eval("mccode").ToString() %>','0',<%#Eval("notusecoupon").ToString() %>)"><%#Eval("notusecoupon").ToString() %></a></ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate><span data-code='couponusenum'>已使用</span></HeaderTemplate>
                                <ItemTemplate><a href="#" onclick="showmembercoupon('<%#Eval("cardcode").ToString() %>',<%#Eval("memcode").ToString() %>','<%#Eval("sumcode").ToString() %>','<%#Eval("mccode").ToString() %>','1',<%#Eval("usecoupon").ToString() %>)"><%#Eval("usecoupon").ToString() %></a></ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate><span data-code='couponcancel'>作废</span></HeaderTemplate>
                                <ItemTemplate><a href="#" onclick="showmembercoupon('<%#Eval("cardcode").ToString() %>','<%#Eval("memcode").ToString() %>','<%#Eval("sumcode").ToString() %>','<%#Eval("mccode").ToString() %>','2',<%#Eval("cancelcoupon").ToString() %>)"><%#Eval("cancelcoupon").ToString() %></a></ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </cc1:CustDataGrid>
                </div>
            </div>
            <div class="bottomline"></div>
        </div>
    </form>
</body>
</html>
