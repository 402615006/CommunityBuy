<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false"  CodeBehind="advertedit.aspx.cs" Inherits="CommunityBuy.BackWeb.advertedit" %>

<%@ Register Assembly="CommunityBuy.WebControl" Namespace="CommunityBuy.WebControl" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出品管理详情</title>
    <link href="../js/dishes/css/list.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/editstyle.css" rel="stylesheet" />
    <link href="../js/dishes/css/common.css" rel="stylesheet" />
    <link href="../js/dishes/css/normalize.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.2.min.js"></script>
    <script src="/js/layui/layui.all.js"></script>
    <script src="../js/layerhelper.js"></script>
    <script src="/js/listeditjs.js" type="text/javascript"></script>

    <%--<script src="../js/dishes/dishesedit.js"></script>--%>
    <script src="../js/dishes/pinyin_dict_notone.js"></script>
    <script src="../js/dishes/pinyin_dict_withtone.js"></script>
    <script src="../js/dishes/pinyinUtil.js"></script>
    <style>
        .dishes-info li {
            float: left;k
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
        var deleteIndex = new Array();
        //返回上一页
        function backform() {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }
        $(function () {
            getPinyin();

            //初始化文件上传
            layui.use('upload', function () {
                var upload = layui.upload;

                //执行上传
                var uploadInst = upload.render({
                    elem: '#upload' //绑定元素
                    , url: '/ajax/UploadFile.ashx'//上传接口
                    , method: 'POST'
                    // , accept: 'file'
                    // , size: 50
                    , multiple: true
                    , data: { filetype: 'adverimage', pwd: 'combuy' }
                    , bindAction: '#btn_uploadlogo'
                    , auto: false
                    , before: function (obj) {
                        layui.layer.load();
                    }
                    , choose: function (obj) {
                        //将每次选择的文件追加到文件队列
                        var files = obj.pushFile();

                        //预读本地文件，如果是多文件，则会遍历。(不支持ie8/9)
                        obj.preview(function (index, file, result) {
                            //console.log(index); //得到文件索引
                            //console.log(file); //得到文件对象
                            //console.log(result); //得到文件base64编码，比如图片
                            //obj.resetFile(index, file, '123.jpg'); //重命名文件名，layui 2.3.0 开始新增
                            //这里还可以做一些 append 文件列表 DOM 的操作
                            //obj.upload(index, file); //对上传失败的单个文件重新上传，一般在某个事件中使用
                            //delete files[index]; //删除列表中对应的文件，一般在某个事件中使用

                            var imgHtml = '<img  imgindex="' + index + '"  width="200" height="200" style="float: left;margin-left:6px;" src="' + result + '" onclick="deleteimage(this,' + index+')" />';
                            $('#tdImg').append(imgHtml);
                          
                        });
                    }
                    , before: function (obj) {
                        var files = obj.pushFile();
                        for (var i = 0; i < deleteIndex.length; i++) {
                            var fileindex = deleteIndex[i] + '-0';
                            delete files[fileindex];
                        }
                    }
                    , allDone: function (res) {//上传完毕回调
                        layui.layer.msg('图片上传成功');
                        layui.layer.closeAll('loading');
                    }
                    ,done: function (res) {
                        if (res.name) {
                            var images = $('#hidimages').val();
                            images += res.name + ",";
                            $('#hidimages').val(images);
                        } else {
                            layui.layer.msg('图片上传失败');
                        }
                    }

                    , error: function () {//请求异常回调
                        layui.layer.closeAll('loading');
                        layui.layer.msg('网络异常，请稍后重试！');
                    }
                });
            });

            //绑定后台数据
            $('#tdImg').html($('#HidImagesHtml').val());
        });

        function deleteimage(img, index) {
            deleteIndex.push(index);
            $(img).remove();
            var images = $('#hidimages').val().replace(index, '');
            $('#hidimages').val(images);
            alert(images);
        }

        function getPinyin() {
            var value = $('#txt_disname').val();
            var result = '';
            if (value) {
                result = pinyinUtil.getFirstLetter(value, false);
            }
            $("#txt_quickcode").val(result);
        }


    </script>
</head>
<body data-pagecode="advert">
    <form id="form1" data-tbname="advert" runat="server">
        <asp:HiddenField id="hidid" runat="server" />
        <div class="fixtoph">&nbsp;</div>
        <div class="fixtop">
            <div class="currentpath">
                <cc1:CPathBar ID="pathBar" PageCode="disheslist" PageType="Edit" MainMenu="广告管理" SubMenu="广告管理" runat="server"></cc1:CPathBar>
            </div>
            <div class="redline"></div>
            <div class="righttitle">
                <cc1:CPageTitle ID="PageTitle" Menu="广告管理" Operate="编辑" runat="server"></cc1:CPageTitle>
            </div>
        </div>
        <div class="rightcontent">
            <div class="height25 gray">&nbsp;</div>
            <div class="labelledlist">
                <div id="tab_info" data-code="tab1" class="graydiv" title="cla">广告信息</div>
            </div>
            <div class="updatediv cla" style="height: 1000px; overflow-y: auto;">
                <table>
                    <tr>
                          <td><label>类型</label></td>
                        <td  style="text-align: left;">
                                 <cc1:CDropDownList ID="ddl_sel_dishetypetwo" Descr="类型" Width="140" SelType="Normal" CssClass="selstyle" runat="server"></cc1:CDropDownList>
                        </td>
                         <td>
                            <label for="disname">标题</label></td>
                        <td style="text-align: left;">
                            <input id="txt_title" runat="server" type="text" onchange="getPinyin()" data-code="disname_placeholder" class="reqtxtstyle" maxlength="32" data-notempty="true" /></td>
                       <td>
                            <label for="" data-code="span_quickcode">排序</label></td>
                        <td style="text-align: left;">
                            <input id="txt_sort" runat="server" data-code="quickcode_placeholder" maxlength="32" style="width:90%;" class="txtstyle" type="text" value="" data-notempty="true"/></td>
                    </tr>
                    <tr>
                          <td><label>描述</label></td>
                        <td  style="text-align: left;" colspan="5">
                                 <input id="txtDes" runat="server" maxlength="256" style="width:90%;" type="text" value="" class="txtstyle"/>
                        </td>
                        </tr>
                    <tr>
                        <td>
                            <label for="" data-code="span_disothername">连接地址</label></td>
                        <td style="text-align: left;" colspan="5">
                            <input id="txt_url" runat="server" maxlength="256" type="text" value="" class="txtstyle" /></td>
     
                    </tr>
                    <tr>
                        <td><label data-code="span_uploadify" style="vertical-align: top">图片上传</label></td>
                         <td id="tdImg" colspan="3">
                        </td>
                       <td>
                             <button type="button" class="layui-btn" id="upload"></button>
                        </td>
                        <td>
                              <input class="addbtn" type="button" id="btn_uploadlogo" value="上传"/>
                         </td>
                    </tr>
                    <tr>
                        <td>
                             <cc1:CButton ID="Save_btn" runat="server"  OnClick="Save_btn_Click" Text="保存" IsFormValidation="false"/>
                        </td>
                        <td colspan="5">
                            <span id="errormessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <input id="stocodes" type="hidden" runat="server" />
        <input id="hidimages" type="hidden" runat="server" />
        <asp:HiddenField ID="HidImagesHtml" ClientIDMode="Inherit" runat="server" />
    </form>
</body>
</html>
