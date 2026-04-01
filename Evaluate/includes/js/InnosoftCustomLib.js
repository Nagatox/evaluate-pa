function SetPremission(obj, fname, action) {
    $.ajax({
        type: "GET",
        async: false,
        url: "frmMain.aspx?q=CheckPermission&form_name=" + fname + "&action=" + action + "&time=" + time(),
        dataType: 'json',
        success: function(json) {
            if (!json.isError) {
                if (!json.allow) obj.hide();
            } else {
                obj.hide();
            }
        }
    });
}

function SetMenuAdmin(group_name) {
    if (group_name.trim() == 'Power User') {
        $('.menuAdmin').css('visibility', 'visible');
    }
}

function SetFacultyPremission(obj, faculty_id, action) {
    $.ajax({
        type: "GET",
        async: false,
        url: "frmMain.aspx?q=CheckFacultyPermission&faculty_id=" + faculty_id + "&action=" + action + "&time=" + time(),
        dataType: 'json',
        success: function(json) {
            if (!json.isError) {
                if (!json.allow) obj.hide();
            } else {
                obj.hide();
            }
        }
    });
}

function checkFormRequired(formId) {
    var isError = false;
    $('#' + formId + ' [required="required"]').each(
		function(index, obj) {
		    if ($(obj).val() == '') {
		        isError = true;
		        changetxtcolor(obj, false);
		        // $(obj).addClass('error');
		    }
		    else {
		        changetxtcolor(obj, true);
		        // $(obj).removeClass('error');
		    }
		}
	);
    return isError;
}
function checkFormNumber(formId) {
    var isError = false;
    $('#' + formId + ' [rtype="decimal"]').each(
		function(index, obj) {
            if (frmcheckNumber(obj, 4) == false) {
		        isError = true;
		    }
		}
	);
	
	if (isError == false)
	{
	     $('#' + formId + ' [rtype="integer"]').each(
		    function(index, obj) {
                if (frmcheckNumber(obj, 1) == false) {
		            isError = true;
		        }
		    }
	    );
	}
	
	if (isError == false)
	{
	     $('#' + formId + ' [rtype="p_decimal"]').each(
		    function(index, obj) {
                if (frmcheckNumber(obj, 3) == false) {
		            isError = true;
		        }
		    }
	    );
	}
	
	
    return isError;
}

function checkFormTime(formId) {
    var isError = false;
    $('#' + formId + ' [rtype="time"]').each(
		function(index, obj) {
		    if (frmcheckTime(obj) == false) {
		        isError = true;
		    }
		}
	);
    return isError;
}

function checkFormDate(formId) {
    var isError = false;
    $('#' + formId + ' [type="date"]').each(
		function(index, obj) {
		    if (frmcheckDate(obj) == false) {
		        isError = true;
		    }
		}
	);
    return isError;
}
function checkMoreDate(eleForm,eleUntil) {
    // Second
    var isError = false;
    var diffTime = getTimeBetween(eleForm.value, eleUntil.value, "00:00:00", "00:00:00");

    if (diffTime < 0) {
        changetxtcolor(eleForm, false);
        changetxtcolor(eleUntil, false);
		isError =  true;
	}
	else {
	    changetxtcolor(eleForm, true);
	    changetxtcolor(eleUntil, true);
	}
	return isError
}
function checkMoreTime(eleForm, eleUntil) {
    // Second
    var isError = false;
    var diffTime = getTimeBetween("01/01/2553", "01/01/2553", eleForm.value, eleUntil.value);

    if (diffTime < 0) {
        changetxtcolor(eleForm, false);
        changetxtcolor(eleUntil, false);
        isError = true;
    }
    else {
        changetxtcolor(eleForm, true);
        changetxtcolor(eleUntil, true);
    }
    return isError
}
function checkMoreDateTime(eleForm, eleUntil,eleTimeForm, eleTimeUntil) {
    // Second

    var isError = false;
    
    var diffTime = getTimeBetween($(eleForm).val(), $(eleUntil).val(), $(eleTimeForm).val(), $(eleTimeUntil).val());
    if (diffTime < 0) {
        changetxtcolor(eleForm, false);
        changetxtcolor(eleUntil, false);
        changetxtcolor(eleTimeForm, false);
        changetxtcolor(eleTimeUntil, false);
        isError = true;
    }
    else {
        changetxtcolor(eleForm, true);
        changetxtcolor(eleUntil, true);
        changetxtcolor(eleTimeForm, true);
        changetxtcolor(eleTimeUntil, true);
    }
    return isError
}
function time() {
    return Math.floor(new Date().getTime() / 1000);
}
function trim(text) {
    return text.replace(/^\s*/, "").replace(/\s*$/, "");
}

function objchkrequire() {
    this.chknull = false;
    this.msgerr = "";
}
function checkNulltext(objname, objval) {

    if (trim(objval) == "") {
        changetxtcolor(objname, false);
        return false;
    }
    else {
        changetxtcolor(objname, true);
        return true;
    }
}
function checkInlineNulltext(objval) {

    if (trim(objval) == "") {
       // changetxtcolor(objname, false);
        return false;
    }
    else {
       // changetxtcolor(objname, true);
        return true;
    }
}
function changetxtcolor(objname, objval) {
    if (objval == true) {
        $(objname).css('background-color', 'white');
    }
    else {
        $(objname).css('background-color', '#ffcccc');
    }
}
function changetxtcolorDiable(objname) {
    $(objname).attr("disabled", true); 
    $(objname).css('background-color','#fafacc');
}
function frmcheckTime(element) {
    var iserror = checkTime(element.value);
    if (iserror == false) {
        changetxtcolor(element, false);
        return false;
    }
    else {
        changetxtcolor(element, true);
        return true;
    }
}
function frmcheckNumber(element, mode) {
    var iserror = checknumber(element.value, mode);
    if (iserror == "ERROR") {
        changetxtcolor(element, false);
        return false;
    }
    else {
        changetxtcolor(element, true);
        return true;
    }
}
function frmcheckDate(element) {
    var iserror = checkDate(element.value);
    if (iserror == false) {
        changetxtcolor(element, false);
        return false;
    }
    else {
        changetxtcolor(element, true);
        return true;
    }
}
function jqchecknumber(frm, postdata, name, mode) {
    var chknum = "";
    eval("chknum = checknumber(postdata." + name + "," + mode + ");");
    if (chknum == "ERROR") {
        changetxtcolor(frm, false);
        //$(frm).focus();
        return false;
    }
    else {
        changetxtcolor(frm, true);
        eval("postdata." + name + " = chknum ;");
        return true;
    }
    return (testresult)
}
function jqcheckDate(frm, postdata, name) {
    var chknum = "";
    eval("chknum = checkDate(postdata." + name + ");");
    if (chknum == false) {
        changetxtcolor(frm, false);
        //$(frm).focus();
        return false;
    }
    else {
        changetxtcolor(frm, true);
        return true;
    }
    return (testresult)
}
function jqcheckTime(frm, postdata, name) {
    var chknum = "";
    eval("chknum = checkTime(postdata." + name + ");");
    if (chknum == false) {
        changetxtcolor(frm, false);
        //$(frm).focus();
        return false;
    }
    else {
        changetxtcolor(frm, true);
        return true;
    }
    return (testresult)
}
function jqcheckMore(frm, postdata, name,vmore,vless) {
    var chknum = "";

    eval("chknum = checkMore(postdata." + name + ",vmore,vless);");
    if (chknum == false) {
        changetxtcolor(frm, false);
        //$(frm).focus();
        return false;
    }
    else {
        changetxtcolor(frm, true);
        return true;
    }
    return (testresult)
}

function checkMore(value,vmore,vless) {
    var testresult = true;
    if (value > vmore )
    {
      testresult = false;
    }  
    if (value < vless)
    {
      testresult = false;
    }   
    return (testresult)
}
/*
function jqcheckTime(frm, postdata, name) {
    var chknum = "";
    eval("chknum = checknumber(postdata." + name + "," + mode + ");");
    if (chknum == "ERROR") {
        changetxtcolor(frm, false);
        //$(frm).focus();
        return false;
    }
    else {
        changetxtcolor(frm, true);
        eval("postdata." + name + " = chknum ;");
        return true;
    }
    return (testresult)
}*/
function checknumber(value, mode) {
    // 1  N decimal, N minus
    // 2  N decimal, Y minus
    // 3  Y decimal, N minus
    // 4  Y decimal, Y minus 

    // type 0 is value , 1 object
    //var anum=/(^\d+$)|(^\d+\.\d+$)/
    //var anum=/(^[-]?\d+$)|(^[-]?\d+\.\d+$)/;
    value = trim(value);

    var anum = "";
    switch (mode) {
        //  case 4: anum = /(^\d+$)/; break; // 100
        case 1: anum = /(^\d+$)/; break; // 100
        case 2: anum = /(^[-]?\d+$)/; break; // -100
        case 3: anum = /(^\d+$)|(^\d+\.\d+$)/; break; // 100.215
        case 4: anum = /(^[-]?\d+$)|(^[-]?\d+\.\d+$)/; break; // -100.215
        default: anum = /(^\d+$)|(^\d+\.\d+$)/;
    }

    if (anum.test(value))
        testresult = parseFloat(value);
    else {
        //alert("Please input a valid number!")
        testresult = "ERROR";  // it will return 0 it false
    }
    return (testresult)
}
function checkTime(value) {
    value = trim(value);
   // var anum = /^([0-9]|[01][0-9]|2[0-3])+\.[0-5][0-9]$/;
   // var anum = /^([0-9]|[01][0-9]|2[0-3]).[0-5][0-9]$/;
    var anum = /^((2[0-3])|[01][0-9])+\.[0-5][0-9]$/;
    if (anum.test(value))
        testresult = true;
    else {
        testresult = false;
    }
    return (testresult)
}
// for force In form 
function chkformnumber(element, mode) {
    if (!isNumeric(element.value, mode)) {
         element.value = element.value.substring(0,element.value.length-1);
       // element.value = "";
    }
}
function isNumeric(strString, mode) {
    var strValidChars = "0123456789";
    var strChar;
    var blnResult = true;
    switch (mode) {
        case 1: strValidChars = "0123456789"; // 100
        case 2: strValidChars = "-0123456789"; // -100
        case 3: strValidChars = "0123456789."; // 100.215
        case 4: strValidChars = "-0123456789."; // -100.215
        default: anum = /(^\d+$)|(^\d+\.\d+$)/;
    }
    if (strString.length == 0) {
        return false;
    }
    for (i = 0; i < strString.length && blnResult == true; i++) {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1) {
            blnResult = false;
        }
    }
    return blnResult;
}
function checkInlineNumber(element, mode) {
    var iserror = checknumber(element, mode);
    if (iserror == "ERROR") {
       /// changetxtcolor(element, false);
        return false;
    }
    else {
      //  changetxtcolor(element, true);
        return true;
    }
}

function decodeType(mode) 
{
    var vtype = 1;
    switch (mode) {

        case "integer": vtype = 1; break; // 100
        case "decimal": vtype = 4 ; break; // -100
        default: vtype = 1;
    }
    return vtype;
}

var dtCh = "/";
var minYear = 1900;
var maxYear = 2100;
//var minYearA = 1900;
//var maxYearA = 2100;
var minYearA = 1900 + 543;
var maxYearA = 2100 + 543;
function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}
function stripCharsInBag(s, bag) // check format
{
    var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}
function daysInFebruary(year) // 29 02 
{
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}
function DaysArray(n)  // 30 31
{
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}
function checkDate(dtStr) // main check
{   dtStr = trim(dtStr);
    var daysInMonth = DaysArray(12)
    var pos1 = dtStr.indexOf(dtCh)
    var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
    var strDay = dtStr.substring(0, pos1)
    var strMonth = dtStr.substring(pos1 + 1, pos2)
    var strYear = dtStr.substring(pos2 + 1)
    strYr = strYear
    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)  // CUT 0
    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)  // CUT 0
    for (var i = 1; i <= 3; i++) {
        if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1) // CUT 0
    }
    day = parseFloat(strDay);
    month = parseFloat(strMonth);
    year = parseFloat(strYr);
    year = year - 543;
    if (pos1 == -1 || pos2 == -1) {
        alert("The date format should be : dd/mm/yyyy")
        return false
    }
    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
        alert("Please enter a valid day")
        return false
    }
    if (strMonth.length < 1 || month < 1 || month > 12) {
        alert("Please enter a valid month")
        return false
    }
    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
        alert("Please enter a valid 4 digit year between " + minYearA + " and " + maxYearA)
        return false
    }
    if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
        alert("Please enter a valid date")
        return false
    }
    return true
}
function showNowDate() 
{
    var currentTime = new Date();
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear()+543;
    var dNow = (day + "/" + month + "/" + year);
    return dNow
}
function conFormatTime(sTime) // 20.01 To 20:01:00
{
    var convertTime = sTime.replace(/\./g, ":")+":00";
    return convertTime;
}
function seperateDate(sDate) {
    var dtch = "/";
    var pos1 = sDate.indexOf(dtCh);
    var pos2 = sDate.indexOf(dtCh, pos1 + 1);
    var strDay = sDate.substring(0, pos1);
    var strMonth = parseFloat(sDate.substring(pos1 + 1, pos2)) - 1;
    var strYear = sDate.substring(pos2 + 1) - 543;
    var monthArrayShort = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec');
    var strDate = strDay + " " + monthArrayShort[strMonth] + " " + strYear;
    return strDate;
}
function getTimeBetween(from, until, timefrom, timeuntil) {
    
    var past = from == " " ? new Date() : new Date(seperateDate(from) + " " + conFormatTime(timefrom));
    var future = until == " " ? new Date() : new Date(seperateDate(until) + " " + conFormatTime(timeuntil));
    /*
    if (past >= future) {
        var tmp = past;
        past = future;
        future = tmp;
    }*/
    var between = (future - past) / (1000);
    return between;
    /*
    var between = [ future.getFullYear() - past.getFullYear(),
    future.getMonth() - past.getMonth(),
    future.getDate() - past.getDate(),
    future.getHours() - past.getHours(),
    future.getMinutes() - past.getMinutes(),
    future.getSeconds() - past.getSeconds()];
    alert ((future-past)/(1000));

	if (between[2] < 0)
    {
    between[1] -- ;
    var ynum = future.getFullYear();
    var mlengths =[31, (ynum %4 == 0 && ynum%100 != 0 || ynum%400 ==0) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31] ;
    var mnum = future.getMonth() - 1;
    if(mnum < 0) { mnum += 12; }
    between[2] += mlengths[mnum] ;
    }
    if (between[1] < 0)
    {
    between[0]--;
    between[1] += 12;
    }
    if (between[5]< 0)
    {
    between[4]--;
    between[5] += 60;
    }
    if (between[4]< 0)
    {
    between[3]--;
    between[4] += 60;
    }
    if (between[3]< 0)
    {
    between[2]--;
    between[3] += 24;
    }
    */
}
function getTimeBetweenFormat(from, until, timefrom, timeuntil) {
  var difftime = getTimeBetween(from, until, timefrom, timeuntil);
  var vhr = Math.floor(difftime/(60*60));
  var vmin = Math.floor((difftime - (vhr * (60*60)))/60); 
  if (vmin < 10)
  {
  vmin = "0" + vmin;
  }
  var dtime = vhr + "." + vmin;
  return dtime;
}

function loadTodropdown(obj, url) {
  $(obj+ ' >option').remove();
            $.ajax({
                type: "GET",
                  async: false,
                url: url,
                success: function(html){
                $(obj).html(html);
                }
            });
            
   //$("#yourdropdownid option:selected").text();
   /* $('#txtQuestion >option').remove();
             var myOptions = {
                val1 : 'text1',
                val2 : 'text2'
            };
            $.each(myOptions, function(val, text) {
                $('#txtQuestion').append(
                    $('<option></option>').val(val).html(text)
                );
            });*/
}


function checkInlineRequired(tname,round,lastsel,modeinline,colName,colReq,colType) {
    var objchk = true;
    var tmp = true;
    var obname = "";
    var obval = "";
     for (var i = 0; i < round; i++)
     {
         if (colReq[i] == "y")
         {
            obname = tname + " #"+lastsel+ "_" + colName[i];
            if (modeinline == 1)
            {
             obval = $(obname).val();
            }
            else
            {
              var vf = jQuery(tname).jqGrid('getGridParam', 'data');  
              eval("obval = vf[lastsel-1]." + colName[i] + ";");  
            }
            tmp = checkInlineNulltext(obval); /// change_xxx
            objchk = objchk && tmp;
            if (tmp == false)
            {
                 if (modeinline == "1")  //zxy
                    { //zxy
                        $(tname).jqGrid('restoreRow',lastsel);
                        $(tname).jqGrid('setSelection', lastsel);
                        $(tname).jqGrid('editRow',lastsel, true, '', '', '', '', chkInlineAll,'', '');
                        $(obname).focus();
                   } //zxy
                    else //zxy
                    { //zxy
                        $(tname).jqGrid('saveRow',lastsel); //zxy
                    } //zxy
                    
                    i = round;
            }
         }
     }
    return objchk;
}
function checkInlineRequiredType(tname,round,lastsel,modeinline,colName,colReq,colType) {
    var objchk = true;
    var tmp = true;
    var obname = "";
    var obval = "";
    
     for (var i = 0; i < round; i++)
     {
            obname = tname + " #"+lastsel+ "_" + colName[i];
            if (modeinline == 1)
            {
             obval = $(obname).val();
            }
            else
            {
              var vf = jQuery(tname).jqGrid('getGridParam', 'data');  
              eval("obval = vf[lastsel-1]." + colName[i] + ";");  
            }
            if ((colmy2Type[i] != "") && (obval != ""))
            {
                tmp = checkInlineNumber(obval,decodeType(colType[i]));
                objchk = objchk && tmp;
                if (tmp == false)
                {

                    if (modeinline == "1") //zxy
                    { //zxy
                        $(tname).jqGrid('restoreRow',lastsel);
                        $(tname).jqGrid('setSelection', lastsel);
                        $(tname).jqGrid('editRow',lastsel, true, '', '', '', '', chkInlineAll,'', '');
                        $(obname).focus();
                   } //zxy
                    else //zxy
                    { //zxy
                        $(tname).jqGrid('saveRow',lastsel); //zxy
                    } //zxy
                    
                    i = round;
                    

                }
           }
     }
    return objchk;
}

function checkInlineRequiredTime(tname,round,lastsel,modeinline,colName,colReq,colType) {
    var objchk = true;
    var tmp = true;
    var obname = "";
    var obval = "";
	for (var i = 0; i < round; i++)
        {
            obname = tname + " #"+lastsel+ "_" + colName[i];
            if (modeinline == 1)
            {
             obval = $(obname).val();
            }
            else
            {
              var vf = jQuery(tname).jqGrid('getGridParam', 'data');  
              eval("obval = vf[lastsel-1]." + colName[i] + ";");  
            }

            tmp = checkTime(obval); // checkInlineNumber(obval,decodeType(colType[i]));
            objchk = objchk && tmp;
            if (tmp == false)
            {
           
                if (modeinline == "1")  //zxy
                    { //zxy
                        $(tname).jqGrid('restoreRow',lastsel);
                        $(tname).jqGrid('setSelection', lastsel);
                        $(tname).jqGrid('editRow',lastsel, true, '', '', '', '', chkInlineAll,'', '');
                        $(obname).focus();
                   } //zxy
                    else //zxy
                    { //zxy
                        $(tname).jqGrid('saveRow',lastsel); //zxy
                    } //zxy
                    
                    i = round;
            }

        }          

    return objchk;
}

function setshowRound(tname,maxR,curR,colInline) 
{
        for (var i = 0; i < maxR; i++) 
         {
            $(tname).hideCol(colInline[i]);      //  $(t_2jqgrid).setGridWidth(550,false);
         }
         var chkRound =  curR;
    
         for (var i = 0; i < chkRound; i++) 
         { 
             $(tname).showCol(colInline[i]);   //   $(t_2jqgrid).jqGrid('setColProp',colmy2data[i],{width:xx}); 
         }
 }/*
 function isArray(obj) {
return (obj.constructor.toString().indexOf("Array") != -1);
}
*/
function isArray(testObject) {  
var  tmp = (testObject && !(testObject.propertyIsEnumerable('length')) && typeof testObject === 'object' && typeof testObject.length === 'number');
if (tmp == undefined)
{
tmp = false;
}
    return tmp;
}
function chkNewArray(data,ids) 
{
    if (isArray(data[ids]) == false) // is array	 
	{
	   data[ids] = new Array();
	}
}
function addTimeRound(val, interval) {

    var pos1 = val.indexOf(".");
    var pos2 = val.indexOf(".", pos1 + 1);
    
    var hr1 = val.substring(0, pos1);
    var mn1 = val.substring(pos1 + 1);
    
    mn1 = parseFloat(mn1) + parseFloat(interval);
    if (mn1>=60) 
    { 
      mn1 = -(60 - mn1); 
      hr1 = parseFloat(hr1)+1; 
    }
   
    if (hr1>=24) 
    {
     hr1 = -(24 - hr1); 
    }

    mn1 = parseFloat(mn1);
    hr1 = parseFloat(hr1);
    if (mn1 < 10)
    {
       mn1 = "0" + mn1;    
    }
    if (hr1 < 10)
    {
       hr1 = "0" + hr1;    
    }
    var  result = hr1 + "." + mn1;
   // alert (result);
	return result;
}

    
function roundNumber(num, dec) {
	var result = Math.round(num*Math.pow(10,dec))/Math.pow(10,dec);
	return result;
}
average = function(a){
    var r = {mean: 0, variance: 0, deviation: 0}, t = a.length;
    for(var m, s = 0, l = t; l--; s += a[l]);
    for(m = r.mean = s / t, l = t, s = 0; l--; s += Math.pow(a[l] - m, 2));
    return r.deviation = Math.sqrt(r.variance = s / t), r;
}
/*
var x = average([2, 3, 4]);

document.write(
    "average([2, 3, 4]).mean = ", x.mean, "<br />",
    "average([2, 3, 4]).deviation = ", x.deviation, "<br />",
    "average([2, 3, 4]).variance = ", x.variance
);
*/

function isNumberOrEmpty(element, mode) {
    if (trim(element.val()) == "") return true;

    var iserror = checknumber(element.val(), mode);
    if (iserror == "ERROR") {
        changetxtcolor(element, false);
        return false;
    }
    else {
        changetxtcolor(element, true);
        return true;
    }
}

function isDateOrEmpth(element) {
    if (trim(element.val()) == "") return true;
    
    var iserror = checkDate(element.val());
    if (iserror == false) {
        changetxtcolor(element, false);
        return false;
    }
    else {
        changetxtcolor(element, true);
        return true;
    }
}

function isOverSize(element, limit) {
    if (element.val().length > limit) {
        changetxtcolor(element, false);
        return false;
    }
    else {
        changetxtcolor(element, true);
        return true;
    }
}