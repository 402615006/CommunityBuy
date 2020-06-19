<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CommunityBuy.BackWeb.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title data-code="PageTitle"></title>
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/xmlhelper.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckCode() {
            var img = document.getElementById("imgcode");
            img.src = "/CheckCode/validateNum.aspx?id=user_" + Math.random();
        }
        function KeyEnter() {
            if (event.keyCode == 13) {
                $("#CheckLogin").click();
            }
        }
        $(function () {
            $(".btn01").click(function () {
                $("#CheckLogin").click();
            })
        })
        function linecolor(classid) {
            $("." + classid).css("border", "1px solid #3385FF");
        }
        function moveline(classid) {
            $("." + classid).css("border", "1px solid #999");
        }
    </script>
    <style type="text/css">
        * {
            margin: 0px;
            padding: 0px;
            font-size: 13px;
            list-style: none;
            border: 0px;
            font-family: "微软雅黑";
        }

        .content {
            background-image: url(/img/log_back.png);
        }

        .logtitle {
            font-size: 40px;
            color: #3E4560;
            font-weight: bold;
            height: 33px;
            line-height: 40px;
            width: 500px;
            margin: 0px auto;
            margin-top: 50px;
        }

        .icoR {
            position: absolute;
            top: 35px;
            font-size: 14px;
        }

        .logcont {
            background-color: #3E4560;
            width: 100%;
            height: 500px;
        }

        .cont01 {
            width: 500px;
            height: 500px;
            margin: auto;
        }

        .conthead {
            height: 45px;
            line-height: 45px;
            font-size: 18px;
            color: #fff;
            font-weight: bold;
            padding-top: 30px;
        }

        .loginbox {
            width: 460px;
            height: 200px;
            margin: 5px;
            border-radius: 10px;
            background-color: #fff;
            border: #3f4461 5px solid;
            padding-top: 30px;
        }

            .loginbox div {
                width: 315px;
                margin: 12px auto;
            }

        .username, .password {
            position: relative;
            height: 35px;
            line-height: 35px;
            margin-top: 10px;
            border: 1px solid #999;
            border-radius: 5px;
        }

        .user-icon {
            padding: 15px;
            background: url(/img/name.png) no-repeat center;
        }

        .password-icon {
            padding: 15px;
            background: url(/img/pwd.png) no-repeat center;
        }

        #UserName, #UserPwd {
            width: 260px;
            height: 30px;
            line-height: 30px;
            padding-left: 5px;
            padding-right: 5px;
        }

        #UserCode {
            width: 200px;
            height: 35px;
            line-height: 35px;
            border: 1px solid #999;
            border-radius: 5px;
            padding-left: 5px;
            padding-right: 5px;
            margin-right: 15px;
        }

        .loginfoot {
            width: 100%;
            text-align: center;
            color: #fff;
            height: 100px;
            line-height: 100px;
        }

        .loginbtn {
            width: 100%;
            min-width: 500px;
            height: 50px;
            line-height: 50px;
            position: absolute;
            top: 380px;
            left: 0px;
        }

        .btn01 {
            height: 40px;
            width: 180px;
            text-align: center;
            margin: 0 auto;
            line-height: 40px;
            background-image: url(/img/Login_button.png);
            color: #fff;
            border-radius: 5px;
            cursor: pointer;
            font-size: 18px;
        }
    </style>
</head>
<body data-pagecode="login" class="content">
    <form id="form1" runat="server">
        <div data-code="Common_Company" class="logtitle">新疆万众<span class="icoR">®</span></div>
        <div class="logcont">
            <div class="cont01">
                <p data-code="SystemName" class="conthead">新疆万众连锁后台管理系统登录</p>
                <div class="loginbox">
                    <div class="username" onmousemove="linecolor('username')" onmouseout="moveline('username')">
                        <span class="user-icon">&nbsp;</span>
                        <input type="text" name="username" id="UserName" data-code="username_placeholder" placeholder="请输入您的用户名" autocomplete="on" onkeyup="value=value.replace(/[\W]/g,'')" runat="server" />
                    </div>
                    <div class="password" onmousemove="linecolor('password')" onmouseout="moveline('password')">
                        <span class="password-icon">&nbsp;</span>
                        <input type="password" name="password" id="UserPwd" data-code="UserPwd_placeholder" placeholder="请输入您的密码" runat="server" />
                    </div>
                    <div class="checkcode">
                        <input type="text" id="UserCode" data-code="UserCode_placeholder" class="UserCode" onmousemove="linecolor('UserCode')" onmouseout="moveline('UserCode')" runat="server" placeholder="请输入验证码" onkeypress="KeyEnter()" maxlength="4" />
                        <img style="vertical-align: middle; cursor: pointer;" id="imgcode" src="/CheckCode/validateNum.aspx?id=user" data-code="imgcode_title" title="点击刷新" onclick="CheckCode()" width="75" height="24" alt="" />
                    </div>
                    <div><span id="Message" runat="server"></span></div>
                </div>
                <div class="loginbtn">
                    <div data-code="ButtonLogin" class="btn01">登录</div>
                    <asp:Button ID="CheckLogin" Style="display: none;" runat="server" Text="登录" OnClick="CheckLogin_Click" />
                </div>
                <p data-code="Common_Copyright" class="loginfoot">copyright@2018 新疆万众 版权所有</p>
                <select id="language" runat="server">
                    <option value="zh-cn">中文简体</option>
                    <option value="zh-tw">中文繁体</option>
                    <option value="ja-jp">日本語</option>
                    <option value="ko-kr">한국어</option>
                    <option value="en-us">english</option>
                </select>
                <input id="btnset" data-code="buttonSet" type="button" value="设置" onclick="setlanguage();" />&nbsp; <a style="color: #fff;" href="install_lodop32.exe">下载打印组件</a>
            </div>
        </div>
    </form>
</body>
</html>
