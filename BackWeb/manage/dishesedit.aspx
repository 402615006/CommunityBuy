<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dishesedit.aspx.cs" Inherits="CommunityBuy.BackWeb.dishesedit" %>

<%@ Register Assembly="Sam.WebControl" Namespace="Sam.WebControl" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出品管理详情</title>
    <link href="../js/layer/skin/layer.css" rel="stylesheet" />
    <link href="../js/dishes/css/list.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/editstyle.css" rel="stylesheet" />
  <%--  <link href="../js/dishes/css/edit.css" rel="stylesheet" />--%>
    <link href="../js/dishes/css/common.css" rel="stylesheet" />
    <link href="../js/dishes/css/normalize.css" rel="stylesheet" />
    <script src="../js/dishes/jquery.min.js"></script>
    <link href="../js/jquery.uploadify/uploadify.css" rel="stylesheet" />
    <script src="../js/jquery.uploadify/jquery.uploadify.v2.1.4.js" charset="gbk" type="text/javascript"></script>
    <script src="../js/jquery.uploadify/swfobject.js"></script>
    <link href="../js/jquery.uploadify/uploadify.css" rel="stylesheet" />
    <script src="../js/dishes/common.js"></script>

    <script src="../js/dishes/pagehelper.js"></script>
    <script src="../js/layerhelper.js"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>
    <script src="../js/dishes/SystemEnum.js"></script>

    <script src="/js/layer/layer.js"></script>
    <script src="../js/dishes/dishesedit.js"></script>
    <script src="../js/dishes/dishescommon.js"></script>
    <script src="../js/dishes/pinyin_dict_notone.js"></script>
    <script src="../js/dishes/pinyin_dict_withtone.js"></script>
    <script src="../js/dishes/pinyinUtil.js"></script>
    <style>
        .dishes-info li {
            float: left;
            width: 230px;
            margin-right: 10px;
            margin-bottom: 20px;
        }

        td {
            width: 250px;
            text-align: right;
        }
    </style>
    <script>
        //返回上一页
        function backform() {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }
        $(function () {
            getPinyin();

            document.getElementById('disname').addEventListener('input', getPinyin);
        });

        function getPinyin() {
            var value = $('#disname').val();
            var result = '';
            if (value) {
                result = pinyinUtil.getFirstLetter(value, false);
            }
            var html = result;
            if (result instanceof Array) {
                html = '<ol>';
                result.forEach(function (val) {
                    html += '<li>' + val + '</li>';
                });
                html += '</ol>';
            }
            $("#quickcode").val(html);
        }
    </script>
</head>
<body data-pagecode="dishes">
    <form id="form1" data-tbname="dishes" runat="server">
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="disheslist" PageType="Edit" MainMenu="出品管理" SubMenu="出品管理" runat="server"></cc1:CPathBar>
            </div>
             <div style="display:block;color:red;"></div>
            <div style="display:block;height:35px;line-height:35px;margin-left:30px;">
                 <cc1:CPageTitle ID="PageTitle" Menu="出品管理" Operate="Edit" runat="server">
                     <a href="javascript:void(0);" onclick="backform()">返回</a>&nbsp;
                </cc1:CPageTitle>
            </div>
        </div>
        <div id="btns" class="title">
            <button id="save" type="button" class="savebtn1" data-code="" onclick="saveclick();" style="float: right; margin-right: 300px;margin-top:50px;">保存</button>
            <%-- <button id="saveadd" type="button" class="saveadd" onclick="saveaddclick();" data-code="">保存并新增</button>--%>
        </div>
        <div class="rightcontent">

            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div id="tab_info" data-code="tab1" class="graydiv" title="cla">菜品信息</div>
                <div id="tab_zf" data-code="tab2" class="graydiv" title="clb">做法/加价</div>
                <div id="tab_dcp" data-code="tab3" class="graydiv" style="display: none;" title="clc">多菜谱</div>
                <div id="tab_pl" data-code="tab4" class="graydiv seldiv" title="cld">配料</div>
            </div>
            <div class="updatediv cla" style="height: 1000px; overflow-y: auto; display: none;">
                <table>
                    <tr>
                        <td>
                            <label for="opeid" data-code="span_opeid">菜品编号</label></td>
                        <td style="text-align: left;">
                            <input id="opeid" type="text" value="系统自动生成" class="txtstyle" maxlength="16" data-code="opeid_placeholder" readonly="readonly" style="background: #bfbcbc;"></td>
                        <td>
                            <label for="stocode">门店:</label></td>
                        <td style="text-align: left;">
                            <select id="stocode" class="selstyle" onchange="stoSelect()" disabled>
                            </select></td>
                        <td>
                            <label for="disname" data-code="span_disname">菜品名称</label></td>
                        <td style="text-align: left;">
                            <input id="disname" type="text" data-code="disname_placeholder" class="reqtxtstyle" maxlength="32" data-notempty="true" disabled /></td>
                    </tr>

                    <tr>
                        <td>
                            <label for="" data-code="span_disothername">其他名称</label></td>
                        <td style="text-align: left;">
                            <input id="disothername" maxlength="128" type="text" value="" class="txtstyle" data-code="disothername_placeholder" disabled></td>
                        <td>
                            <label for="" data-code="span_distypecode">菜品类别</label></td>
                        <td style="text-align: left;">
                            <input id="distypecode" type="text" value="" data-notempty="true" class="reqtxtstyle" data-code="distypecode_placeholder" disabled onfocus="ChooseDisheType();">
                            <input type="hidden" id="hiddistypecode" /></td>
                        <td>
                            <label for="" data-code="span_quickcode">速查码</label></td>
                        <td style="text-align: left;">
                            <input id="quickcode" data-code="quickcode_placeholder" maxlength="32" class="reqtxtstyle" type="text" disabled value="" data-notempty="true"></td>
                    </tr>

                    <tr>
                        <td>
                            <label for="" data-code="span_customcode">自定义编号</label></td>
                        <td style="text-align: left;">
                            <input id="customcode" data-code="customcode_placeholder" maxlength="8" class="txtstyle" type="text" disabled value=""></td>
                        <td>
                            <label for="" data-code="span_unit">单位</label></td>
                        <td style="text-align: left;">
                            <input id="selunit" type="text" data-code="unit_placeholder" maxlength="8" class="reqtxtstyle" disabled data-notempty="true" /></td>
                        <td>
                            <label for="" data-code="span_price">售价</label></td>
                        <td style="text-align: left;">
                            <input id="price" maxlength="18" type="text" value="" class="reqtxtstyle" data-notempty="true" disabled data-reg="Decimal">
                            <div id="multiprice" class="radio"></div>
                            <span class="more-menu" data-code="span_multiprice">多菜谱</span></td>
                    </tr>

                    <tr>
                        <td>
                            <label for="" data-code="Menu_list">菜谱</label></td>
                        <td style="text-align: left;">
                            <select id="sel_meal" class="txtstyle"></select></td>
                        <td>
                            <label for="" data-code="span_memberprice">会员价</label></td>
                        <td style="text-align: left;">
                            <input id="memberprice" maxlength="18" type="text" value="" class="reqtxtstyle" data-notempty="true" disabled data-reg="Decimal"></td>
                        <td>
                            <label for="" data-code="span_costprice">成本价</label></td>
                        <td style="text-align: left;">
                            <input id="costprice" maxlength="18" class="reqtxtstyle" type="text" value="" data-notempty="true" disabled data-reg="Decimal">
                            <div id="costByingRedient" class="radio" disabled></div>
                            <span class="batching-scheme" data-code="span_costByingRedient">配料方案</span></td>
                    </tr>

                    <tr>
                        <td>
                            <label for="" data-code="span_pushmoney">提成</label></td>
                        <td style="text-align: left;">
                            <input id="pushmoney" maxlength="18" type="text" value="" class="reqtxtstyle" disabled data-notempty="true" data-reg="Decimal"></td>
                        <td>
                            <label for="" data-code="span_extcode">外部码</label></td>
                        <td style="text-align: left;">
                            <input id="extcode" data-code="extcode_placeholder" class="txtstyle" disabled maxlength="64" type="text" value=""></td>
                        <td>
                            <label for="" data-code="span_fincode">财务类别</label></td>
                        <td style="text-align: left;">
                            <select id="selfincode" class="txtstyle" disabled>
                            </select></td>
                    </tr>
                    <!--<li>
                            <label for="" data-code="span_matclscode">物料类别</label>
                            <select id="selmatclscode">
                            </select>
                        </li>-->

                    <tr>
                        <td>
                            <label for="" data-code="span_dcode">出品部门</label></td>
                        <td style="text-align: left;">
                            <select id="seldcode" class="selstyle" style="background-color: none;" disabled onchange="BindEmployeeInfo(2);">
                                <!--onchange="BindDepartmentInfo();"-->
                            </select></td>
                        <td>
                            <label for="" data-code="span_kitcode">制作厨房</label></td>
                        <td style="text-align: left;">
                            <select id="selkitcode" class="selstyle" style="background-color: none;" disabled>
                            </select></td>
                        <td>
                            <label for="" data-code="span_ecode">制作厨师</label></td>
                        <td style="text-align: left;">
                            <select id="sel_ecode" class="txtstyle" style="background-color: none;" disabled>
                                <option value="">--无--</option>
                            </select></td>
                    </tr>

                    <tr>
                        <td>
                            <label for="" data-code="span_maketime">制作时长</label></td>
                        <td style="text-align: left;">
                            <input id="maketime" type="text" maxlength="2" value="" class="txtstyle" data-code="maketime_placeholder" disabled data-reg="Int"></td>
                        <td>
                            <label for="" data-code="span_qrcode">菜品二维码</label></td>
                        <td style="text-align: left;">
                            <input id="qrcode" type="text" data-code="qrcode_placeholder" class="txtstyle" disabled maxlength="256" value=""></td>
                        <td>
                            <label for="" data-code="span_matcode">所属原料</label></td>
                        <td style="text-align: left;">
                            <input id="matcode" type="text" data-code="matcode_placeholder" class="txtstyle" disabled maxlength="16" value="" onfocus="chooseStockMaterialrefer();">
                            <input type="hidden" id="hidmatcode" /></td>
                    </tr>
                    <tr>
                        <td>
                            <label for="" data-code="span_warcode">所属仓库</label></td>
                        <td style="text-align: left;">
                            <select id="sel_warcode" class="selstyle" disabled></select></td>
                        <td>
                            <label data-code="span_uploadify" style="vertical-align: top">图片上传</label></td>
                        <td style="text-align: left;">
                            <img id="upimg" alt="" style="width: 100px; height: 70px; border: 1px solid #ccc;" />
                            <input id="uploadify" type="file" style="vertical-align: middle;">
                            <br />
                            <span style="margin-left: 10px;" data-code="span_uploadifytips">支持400*280的PNG、JPG图片，&lt; 2M.</span>
                            <a id="showuploadsrc"></a>
                            <input type="hidden" id="uploadsrc" /></td>
                        <td>
                            <label>描述</label></td>
                        <td style="text-align: left;">
                            <textarea id="remark" style="width: 94%; height: 94px; float: right;" disabled></textarea></td>
                    </tr>

                    <tr>
                        <td>
                            <label for="" data-code="span_warcode">条只方案</label></td>
                        <td colspan="6" style="text-align: left;">
                            <div id="entity" class="radio" style="float: left; line-height: 30px; vertical-align: bottom; margin-top: 10px;" disabled></div>
                            <label for="" data-code="span_entity" style="float: left; line-height: 35px; margin-left: 4px;">条/只方案</label>
                            <div id="div_entity" style="float: left; display: none; line-height: 30px;">
                                <label for="" data-code="span_entitydefcount">默认条/只</label>
                                <input id="entitydefcount" class="txtstyle" type="text" value="" data-reg="Int">
                                <label for="" data-code="span_entityprice">条/只单价</label>
                                <input id="entityprice" class="txtstyle" type="text" value="" data-reg="Decimal">
                                <!--data-notempty="true"-->
                            </div>
                        </td>
                    </tr>

                    <tr id="chooseList" class="full-row">
                        <td>
                            <label for="" data-code="span_warcode">其他</label></td>
                        <td colspan="6" style="text-align: left; padding-top: 10px;">
                            <button type="button" id="iscanmodifyprice" class="other-choose" data-code="span_iscanmodifyprice" disabled>可变价</button>
                            <button type="button" id="isneedweigh" class="other-choose" data-code="span_isneedweigh" disabled>需称重</button>
                            <button type="button" id="isneedmethod" class="other-choose" data-code="span_isneedmethod" disabled>做法必选</button>
                            <button type="button" id="iscaninventory" class="other-choose" data-code="span_iscaninventory" disabled>烟酒(可入库)</button>
                            <button type="button" id="iscancustom" class="other-choose" data-code="span_iscancustom" disabled>可自定义</button>
                            <button type="button" id="isallowmemberprice" class="other-choose" data-code="span_isallowmemberprice" disabled>允许会员价</button>
                            <button type="button" id="isattachcalculate" class="other-choose" data-code="span_isattachcalculate" disabled>参与附加费计算</button>
                            <button type="button" id="isclipcoupons" class="other-choose" data-code="span_isclipcoupons" disabled>支持使用消费券</button>
                            <button type="button" id="iscandeposit" class="other-choose" data-code="span_iscandeposit" disabled>可寄存</button>
                            <button type="button" id="isnonoperating" class="other-choose" data-code="span_isnonoperating" disabled>营业外收入</button></td>
                    </tr>
                </table>

            </div>
            <div class="updatediv clb" style="display: none;">
                <input type="hidden" id="hidDishesMethods" />
                <div style="line-height: 60px; vertical-align: middle; text-align: center;">做法加价方案适用于产品的加价销售，通过配置做法加价方案，可实现加价方式的成本控制。</div>
                <table id="DishesMethods" style="width: 700px;" class="practices-content">
                    <thead>
                        <tr>
                            <th>序号</th>
                            <th style="width: 200px;">做法</th>
                            <th style="width: 150px;">加价方式</th>
                            <th style="width: 200px; text-align: left;">加价金额/百分比</th>
                            <th>
                                <%-- <button id="practicesAdd" type="button" class="showbtn" onclick="DishesMethodsAdd();"><i></i><span>添加</span></button>--%></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div class="updatediv clc" style="display: none;">
                <input type="hidden" id="hidDishesMeal" />
                <table id="DishesMeal" style="width: 700px;" class="practices-content">
                    <thead>
                        <tr>
                            <th style="text-align: center;">序号</th>
                            <th style="width: 200px; text-align: center;">所属菜谱</th>
                            <th style="width: 60px; text-align: center;">默认</th>
                            <th style="width: 150px; text-align: center;">售价</th>
                            <th style="width: 150px; text-align: center;">会员价</th>
                            <th style="text-align: center;">
                                <%--<button id="Button1" type="button" class="showbtn" onclick="DishesMealAdd();"><i></i><span data-code="add_button">添加</span></button>--%></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div class="updatediv cld">
                <input type="hidden" id="hidDishesMate" />
                <table id="DishesMate" style="width: 100%;" class="practices-content">
                    <thead>
                        <tr>
                            <th style="width: 160px;">原料编号</th>
                            <th style="width: 160px;">原料</th>
                            <th style="width: 120px;">原料分类</th>
                            <th style="width: 80px;">单位</th>
                            <th style="width: 80px;">净料数量</th>
                            <th style="width: 80px;">净料率</th>
                            <th style="width: 80px;">毛料数量</th>
                            <th style="width: 80px;">物料属性</th>
                            <th style="width: 80px;">最近单价</th>
                            <th style="width: 80px;">金额</th>
                            <th style="width: 80px;">备注</th>
                            <th>
                                <button id="Button2" type="button" class="showbtn" onclick="DishesMateAdd();"><i></i><span data-code="add_button">添加</span></button></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
             <input id="stocodes" type="hidden" runat="server" />
    </form>
</body>
</html>
