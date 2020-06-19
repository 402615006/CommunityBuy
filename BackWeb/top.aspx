<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="CommunityBuy.BackWeb.top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .head {
            background-image: url(/images/top_bg.jpg);
            background-repeat: repeat-x;
            padding-right: 10px;
            padding-left: 10px;
        }

        .head_time {
            background-image: url(/images/top_time.png);
            background-repeat: no-repeat;
            text-align: center;
            padding-top: 4px;
            font-size: 14px;
        }

        .head_quite {
            color: #FFF;
            padding-right: 10px;
        }

            .head_quite a {
                color: #f0f731;
                text-decoration: underline;
            }

                .head_quite a:hover {
                    color: #F00;
                    text-decoration: underline;
                }
    </style>
    <script src="/js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function nowTime(type) {
            var Y, M, D, W, H, I, S;
            function fillZero(v) {
                if (v < 10) { v = '0' + v; }
                return v;
            }
            (function () {
                var d = new Date();
                var Week = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
                Y = d.getFullYear();
                M = fillZero(d.getMonth() + 1);
                D = fillZero(d.getDate());
                W = Week[d.getDay()];
                H = fillZero(d.getHours());
                I = fillZero(d.getMinutes());
                S = fillZero(d.getSeconds());
                if (type && type == 12) {
                    if (H <= 12) {
                        H = '' + H;
                    } else if (H > 12 && H < 24) {
                        H = '' + fillZero(H);
                    } else if (H == 24) {
                        H = '00';
                    }
                }
                $("#time").html(Y + '年' + M + '月' + D + '日 ' + '&nbsp;' + W + '&nbsp;' + H + ':' + I + ':' + S);
                setTimeout(arguments.callee, 1000);
            })();
        }
        $(function () {
            var time = nowTime(12);
            $("#time").text(time);
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="98" align="left" valign="top" class="head">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="height: 90px; width: 650px;">
                                <img src="/images/logo11.png" height="90" alt="" />
                            </td>
                            <td align="right" valign="top">
                                <table width="250" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="50" valign="top" class="head_time"><span id="time"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="head_quite">
                                            <span id="AdminName" runat="server"></span>，您好！&nbsp;<a href="javascript:top.location.href='/front/index.aspx'">网站前台</a>&nbsp;<a href="logout.aspx" target="_parent">退出</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
