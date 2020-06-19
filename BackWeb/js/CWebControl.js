function OnKeyPressCheck(txtBox, etype) {
    var val = txtBox.value;
    var k = event.keyCode;
    //alert(String.fromCharCode(k)+" "+k);
    switch (etype) {
        case "Float":
            if ((k == 46) || (k == 45) || (k <= 57 && k >= 48)) {
                if (k == 46) {
                    if (val.indexOf('.') >= 0 || val.length == 0) {
                        return false;
                    }
                }
                else if (k == 45) {
                    if (val.indexOf('-') >= 0 || val.length > 0) {
                        return false;
                    }
                    else {
                        if (String.fromCharCode(k) == '') {
                            return false;
                        }
                        return true;
                    }
                }
            }
            else {
                return false;
            }
            break;
        case "Float1"://正Float
            if ((k == 46) || (k <= 57 && k >= 48)) {
                if (k == 46) {
                    if (val.indexOf('.') >= 0 || val.length == 0) {
                        return false;
                    }
                    else {
                        if (String.fromCharCode(k) == '') {
                            return false;
                        }
                        return true;
                    }
                }
            }
            else {
                return false;
            }
            break;
        //整型格式
        case "Decimal1":
        case "Decimal":
            if ((k == 46) || (k <= 57 && k >= 48)) {
                if (k == 46) {
                    if (val.indexOf('.') >= 0 || val.length == 0) {
                        return false;
                    }
                    else {
                        if (String.fromCharCode(k) == '') {
                            return false;
                        }
                        return true;
                    }
                }
            }
            else
                return false;
            break;
        case "Int":
        case "Mobile":
            if (k <= 57 && k >= 48) {
                //if (k == 48 && val.length == 0) {
                //    return false;
                //}
                //else {

                //}
                return true;
            }
            else {
                return false;
            }
            break;
        case "Tel":
            if ((k == 45) || k <= 57 && k >= 48) {
                if (k == 45 && val.length == 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
            break;
        case "ChineseID":
            if ((k <= 57 && k >= 48) || (k >= 65 && k <= 90)) {
                if ((k >= 65 && k <= 90) && val.length != 18) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
            break; 
    }
}

function FormDataValidationCheck() {
    var Flag = true;
    for (var f = 0; f < document.forms.length; f++) {
        var form = document.forms[f];
        //遍历指定form表单所有元素
        for (var i = 0; i < form.length; i++) {
            var element = form[i];
            if (element.type == "text") {
                if (!onblurCheck(element.id)) {
                    return false;
                }
            }
        }
    }
    return Flag;
}

function CheckIsNull(ControlName, isalert) {
    var obj = document.getElementById(ControlName);
    if (trimStr(obj.value).length == 0) {
        return false;
    }
    return true;
}

function trimStr(str) { return str.replace(/(^\s*)|(\s*$)/g, ""); }

//根据类型获得相应的正则表达式
function CheckFormat(Mes, ControlName, Type, Flag) {
    var Reg;
    switch (Type) {
        //用户 密码格式(不包含*,&,<,>,,",',空格)
        case 'User':
            //Reg="^([^*&<>\"\'\\s]){6,10}$";："^[a-zA-Z]\w{5,17}$
            Reg = /^([a-zA-Z\u4e00-\u9fa5]{1}[a-zA-Z\u4e00-\u9fa50-9]{2,15})$/;
            break;
        case 'Pwd':
            //Reg = /^[a-zA-Z]\w{5,10}$/;
            Reg = /^[0-9a-zA-Z_]{6,16}$/;
            break;
            //邮箱格式
        case 'Email':
            //Reg=/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
            Reg = /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/
            break;
            //联系电话
        case 'Mobile':
            Reg = /^1[3456789]\d{9}$/;
            break;
        case 'Tel':
            Reg = /^(((1[0-9]{1}[0-9]{1}))+\d{8})$/;
            break;
            //整型
        case 'Int':
            Reg = "^([0-9]){1,}$";
            break;
            //数字+,
        case 'IntD':
            Reg = "^([0-9,]){1,}$";
            break;
            //浮点型
        case 'float':
            Reg = "^(-?[0-9]{1,})(.[0-9]{1,4})?$";
            break;
            //浮点型
        case 'Float':
            Reg = "^(-?[0-9]{1,})(.[0-9]{1,10})?$";
            break;
            //浮点型
        case 'Float1':
            Reg = "^([0-9]{1,})(.[0-9]{1,10})?$";
            break;

            //金额
        case 'Decimal':
            Reg = "^(-?[0-9]{1,})(.[0-9]{1,2})?$";
            //Reg = "^(-?[1-9][0-9]{1,15}\.[0-9]{2}|0)$";
            break;
        case 'Decimal1':
            Reg = "^[0-9]*[1-9][0-9]*$";
            break;
            //中文汉字
        case 'Chinese':
            Reg = "^[\u4e00-\u9fa5]{2,10}$";
            break;
            //中文汉字,字母，空格
        case 'CES':
            Reg = "^[\u4e00-\u9fa5A-Za-z\\s]*$";
            break;
            //英文字母
        case 'english':
            Reg = "^[A-Z]*$";
            break;
            //英文字母及空格
        case 'englishO':
            Reg = "^[A-Z\\s]*$";
            break;
            //身份证号
        case 'chineseID':
            Reg = "^([0-9]{15}|[0-9]{17}[xX0-9]{1})$";
            break;
            //日期（格式为：19yy-mm-dd）
        case 'Date':
            Reg = /^((19)|(20))(\d{2})(-)(\d{1,2})(-)(\d{1,2})$/;
            break;
            //相对路径100
        case 'UrlInfo':
            Reg = "^[a-zA-Z\/\.]{1,100}$";
            break;
        default:
            break;

    }
    return CheckRegExp(Mes, ControlName, Reg, Flag);
}

//正则表达式验证
function CheckRegExp(Mes, ControlName, Reg, Flag) {
    var str = document.getElementById(ControlName).value;
    if (str.length > 0) {
        var re = new RegExp(Reg);
        var r = str.search(re);

        if (r == -1) {
            return false;
        }
        else
            return true;
    }
    return true;
}
function EmptyPage() {
    for (var f = 0; f < document.forms.length; f++) {
        var form = document.forms[f];
        //alert(form.name);
        //遍历指定form表单所有元素
        for (var i = 0; i < form.length; i++) {
            var element = form[i];
            switch (element.type) {
                case "text":
                case "password":
                case "textarea":
                    element.value = '';
                    break;
                case "select-one":
                case "select-multiple":
                    element.selectedIndex = 0;
                    break;
                case "checkbox":
                    element.checked = false;
                    break;
            }
        }
    }
}

function SetTextCSS(id, type) {
    var obj = document.getElementById(id);
    if (obj == null)
        return;
    var isrequired = false;
    if (obj.getAttribute("IsRequired") == "true") {
        isrequired = true;
    }
    var classname = '';
    switch (type) {
        case 1:
            if (isrequired) {
                classname = "reqtxtstyle";
            }
            else {
                classname = "txtstyle";
            }
            break;
        case 2:
            if (isrequired) {
                classname = "reqTextRequired";
            }
            else {
                classname = "Requiredselect";
            }
            break;
        case 3:
            classname = "errortext";
            break;
    }
    document.getElementById(id).className = classname;
}

//onblur事件
function onblurCheck(id) {
    var obj = document.getElementById(id);
    if (obj != null) {
        if (obj.getAttribute("IsRequired") != undefined) {
            var placeholder = document.getElementById(id).getAttribute("placeholder");
            if (!CheckIsNull(id, true) && obj.getAttribute("IsRequired") == "true") {
                if (placeholder != undefined) {
                    laytips('#' + id, placeholder + '！');
                }
                return false;
            }
            if (CheckIsNull(id, true)) {
                var tetxtype = obj.getAttribute("TextType");
                if (tetxtype != undefined) {
                    if (!CheckFormat('', id, tetxtype, true)) {
                        if (placeholder != undefined) {
                            laytips('#' + id, placeholder + '！');
                        }
                        return false;
                    }
                }
            }
        }
    }
    return true;
}


//初始化页面TextBox
function initializetext() {
    for (var f = 0; f < document.forms.length; f++) {
        var form = document.forms[f];
        //遍历指定form表单所有元素
        for (var i = 0; i < form.length; i++) {
            var element = form[i];
            switch (element.type) {
                case "text":
                case "password":
                case "textarea":
                    try {
                        if (element.value.length > 0) {
                            break;
                        }
                        var formdescr = element.FormDescr;
                    }
                    catch (e) {
                        continue;
                    }
                    break;
            }
        }
    }
}