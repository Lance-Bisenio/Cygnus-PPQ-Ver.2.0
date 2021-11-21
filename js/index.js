// JScript File
var ajaxlogin;
document.onkeydown=checklogin;
//Check Enter Key for Form Submission
function checklogin(objev){
	var theObj = objev ? objev : window.event;
	if (theObj.keyCode == 13){
		loginexec();	
	}
}
function loginexec()
{
    var usrname = document.getElementById("txtuser");
    var passwd = document.getElementById("txtpwd");
    var msgbox = document.getElementById("errmsg");
    if(trim(usrname.value)==""){
        usrname.focus();
        msgbox.innerHTML = "Please provide a username."
        return;
    }else if(usrname.value == usrname.defaultValue){
        return;
    }
    if(trim(passwd.value)==""){
        passwd.focus();
        msgbox.innerHTML = "Please input your password."
        return;
    }
    document.getElementById("divWait").style.visibility = "visible";
    ajaxlogin = GetXmlHttpObject();
    if(ajaxlogin==null){ ajaxerr();return; }
    var url = "index_exec.aspx";
    ajaxlogin.onreadystatechange=loginproc;
	ajaxlogin.open("POST",url);
	ajaxlogin.setRequestHeader("Content-Type","application/x-www-form-urlencoded;charset = UTF-8");
	ajaxlogin.send("usrname="+usrname.value+"&passwd="+passwd.value);
}

function loginproc(){
    var msgbox = document.getElementById("errmsg");
    if(ajaxlogin.readyState=="complete" || ajaxlogin.readyState==4){
        var ajaxreply = ajaxlogin.responseText;
        if(ajaxreply == "ok"){
            servtransfer("evolvemenus.aspx");
            //servtransfer("menu.aspx");
        }else if(ajaxreply == "nok"){
            msgbox.innerHTML = "Username NOT Found or Password INCORRECT!";
            document.getElementById("divWait").style.visibility = "hidden";
        }else{
            msgbox.innerHTML = ajaxreply;
            document.getElementById("divWait").style.visibility = "hidden";
        }
    }
}

function cleanup(){
    var usrname = document.getElementById("txtuser");
    var passwd = document.getElementById("txtpwd");
    usrname.value = "";
    passwd.value = "";
    usrname.focus();
}

function formrule(field)
{
	if (field.defaultValue == field.value) {
		field.value = "";
	}
}
function formrule2(field)
{
	if (field.value == "") {
		field.value = field.defaultValue;
	}
}
function setfocus(elem){
    elem.focus();
}