//Main Javascript Document

//AJAX Http Object
function GetXmlHttpObject()
{
	var xmlHttp=null;
	try
	{
	// Firefox, Opera 8.0+, Safari
		xmlHttp=new XMLHttpRequest();
	}
	catch (e)
	{
		//Internet Explorer
		try
		{
			xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e)
		{
			xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
		}
	}
	return xmlHttp;
}

//Trim Functions
function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");
}
function ltrim(stringToTrim) {
	return stringToTrim.replace(/^\s+/,"");
}
function rtrim(stringToTrim) {
	return stringToTrim.replace(/\s+$/,"");
}

//Redirect to URL Function
function servtransfer(url){
	var servloc = document.location;
	servloc.href = url;
}

function check_email(field,alerttxt)
{
	with (field)
	{
		var apos=value.indexOf("@")
		var dotpos=value.lastIndexOf(".")
		if (apos<1||dotpos-apos<2) 
		{
			alert(alerttxt);
			return false;
		}else{
			return true
		}
	}
}

var dtCh= "/";
var minYear=1900;
var maxYear=2100;

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31;
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30;}
		if (i==2) {this[i] = 29;}
   } 
   return this;
}

function isDate(dtStr){ //check date is in the form mm/dd/yyyy
	var daysInMonth = DaysArray(12);
	var pos1=dtStr.indexOf(dtCh);
	var pos2=dtStr.indexOf(dtCh,pos1+1);
	var strMonth=dtStr.substring(0,pos1);
	var strDay=dtStr.substring(pos1+1,pos2);
	var strYear=dtStr.substring(pos2+1);
	strYr=strYear;
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1);
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1);
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1);
	}
	month=parseInt(strMonth);
	day=parseInt(strDay);
	year=parseInt(strYr);
	if (pos1==-1 || pos2==-1){
		alert("The date format should be : mm/dd/yyyy");
		return false;
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Please enter a valid month");
		return false;
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		alert("Please enter a valid day");
		return false;
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear);
		return false;
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		alert("Please enter a valid date");
		return false;
	}
	return true;
}

function isFloat(s){
	if(trim(s)==""){
		alert("Please supply a number.");
		return false;
	}
	var i;
	var dotpos;
	var dotlast;
	var c;
	dotpos = s.indexOf(".");
	dotlast = s.lastIndexOf(".");
	if(dotpos==-1){ //value is a whole number
		for(i=0;i<s.length;i++){
			c = s.charAt(i);
			if((c < "0") || (c > "9")){
				return false;
			}
		}
	}else{ //contains a decimal point. check if there is another decimal point due to typo error
		if(dotpos!=dotlast){
			return false;	
		}
		//else good decimal format check the digits before and after the decimal point
		for(i=0;i<dotpos;i++){
			c = s.charAt(i);
			if((c < "0") || (c > "9")){
				return false;
			}
		}
		for(i=dotpos+1;i<s.length;i++){
			c = s.charAt(i);
			if((c < "0") || (c > "9")){
				return false;
			}
		}
	}
	return true;
}

function getRadio(r){
	for(var i=0;i<r.length;i++){
		if(r[i].checked){
			return r[i].value;//return the value selected from a radio box
		}
	}
}

function getCheckBox(c){
	var checkarray = "";
	for(var i=0;i < c.length; i++){
		if(c[i].checked){
			checkarray += c[i].value + ",";
		}
	}
	checkarray = checkarray.substr(0,checkarray.length-1);
	return checkarray;
}

function getAge(bday){ //parse a birthdate as string with proper format
	var sdate = new Date(bday);
	var ndate = new Date();
	var month1 = sdate.getMonth() + 1;
	var month2 = ndate.getMonth()+ 1;
	var year1 = sdate.getFullYear();
	var year2 = ndate.getFullYear();
	if((month2 - month1) < 0){
		month2 += 12;
		year2 -= 1;
	}
	var rmonth = month2 - month1;
	var ryear = year2 - year1;
	var age = ryear + " yrs and " + rmonth + " mos";
	return age;
}

function displayAge(bday,txtbox){
	if(trim(bday)==""){
		txtbox.value = "Enter Birth Date";
		return;
	}
	if(!isDate(bday)){
		txtbox.value = "Invalid Date";
	}else{
		txtbox.value = getAge(bday);
	}
	
}