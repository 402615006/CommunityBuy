<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="welcomePage.aspx.cs" Inherits="CommunityBuy.BackWeb.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/xmlhelper.js"></script>
    <style>
        .main {
            margin-top: 30px;
            margin-left: auto;
            line-height: 20px;
            width: 100%;
        }

            .main div {
                margin-top: 30px;
                margin-left: 5%;
                text-align: left;
            }
    </style>
</head>
<body data-pagecode="welcome" class="content">
    <form id="form1" runat="server">
        <div class="main">
            <div>
                <span data-code="Machinename"></span>
                <span>
                    <%=machinename %>
                </span>
            </div>

            <div>
                <span data-code="UserHostAddress"></span>
                <span>
                    <%=userHostAddress %>
                </span>
            </div>

            <div>
                <span data-code="Browser"></span>
                <span>
                    <%=browser %>
                </span>

            </div>
        </div>
    </form>
</body>
</html>
