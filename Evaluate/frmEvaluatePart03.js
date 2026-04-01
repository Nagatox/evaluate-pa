
var bOwner = true;
var iRevision = 0;
var gRowNumEmployee01 = 3;
var gRowNumEmployee02 = 3;
var gRowNumEmployee03 = 3;
var gRowNumEmployee04 = 5;
var gRowNumCommander01 = 3;
var gRowNumCommander02 = 5;

$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    /*RebuildForm('Employee', 1, bOwner);
    RebuildForm('Employee', 2, bOwner);
    RebuildForm('Employee', 3, bOwner);
    RebuildForm('Employee', 4, bOwner);

    RebuildForm('Commander', 1, !bOwner);
    RebuildForm('Commander', 2, !bOwner);*/

    bOwner = isOwner();

    LoadPart03();

    $('.normal').autosize();

    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }

    LoadEvaluateHeader4Confirm($('#txtUserName').val(), evaluate_year, $('#lblRoundNo').text(), '0', $('#btnSave'), $('#lblConfirmMessage'));

    var username = $('#txtUserName').val();
    iRevision = $('#txtResult').val();
    var sresult = '&result=' + iRevision;
    if (iRevision != '1') sresult = "";
    $('#linkForm00').attr('href', 'frmEvaluatePart00.aspx?u=' + username + sresult);
    $('#linkFormPA').attr('href', 'frmFormPA.aspx?u=' + username + sresult);
    $('#linkForm01').attr('href', 'frmEvaluateFormPA.aspx?u=' + username + sresult);
    $('#linkForm02').attr('href', 'frmEvaluatePart02.aspx?u=' + username + sresult);
    $('#linkForm03').attr('href', 'frmEvaluatePart03.aspx?u=' + username + sresult);

});

function isOwner() {
    return ($('#lblLoginName').text() == $('#txtUserName').val());
}

$('#btnExport').click(function() {
    //alert ('btnExport');
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    //var round = $('#drpRoundNo').val();
    var round = $('#lblRoundNo').text();
    var revision = $('#txtResult').val();

    var service_name = 'ExportEvaluateForm';

    var url = 'frmEvaluateService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time();

    window.location = url;

    return false;
});

$('#btnPrintImage').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var page = 'EvaluatePart03';
    var result = '';

    $.ajax({
        url: 'frmMainService.aspx?q=SystemCallPhantomJS&page=' + page + '&user_id=' + username + '&budget_year=' + evaluate_year + '&round_no=' + round + '&result=' + result + '&u=' + username + '&time=' + time(),
        //type: 'POST',
        //data: "data=" + json,
        dataType: 'json',
        beforeSend: function() {
            $('#divBackground').show();
            $('#divWaiting').show();
        },
        complete: function() {
            $('#divBackground').hide();
            $('#divWaiting').hide();
        },
        success: function(json) {
            if (!json.isError) {
                if (json.recordCount > 0) {
                    location.href = json.detail[0];
                }
            }
            else {
                alert('Load Fail:SystemCallPhantomJS:' + json.ErrMessage);
            }
            return false;
        }
    });
    return false;
});

function RebuildForm(prefix, id, isOwner) {
    $('#tbl' + prefix + '0' + id + ' tbody').empty();

    var iRowNum = eval('gRowNum' + prefix + '0' + id);
    var i = 1;
    var sDisabled = '';
    if (!isOwner) { sDisabled = 'disabled'; $('#trAddRow' + prefix + '0' + id).hide(); }
    for (i = 1; i <= iRowNum; i++) {
        var tr = "<tr id='tr" + prefix + "0" + id + "_" + i + "'>" +
             "<td class='tdDevFormNo'></td>" +
             "<td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal' id='txtDetail_" + prefix + "_" + id + "_" + i + "' " + sDisabled + "></textarea></div></td>" +
             "<td class='tdDevFormDelete'><button id='btnDelete_" + prefix + "_" + id + "_" + i + "' class='btnDelete' " + sDisabled + ">ลบ</button></td>" +
             "</tr>" +
             "";

        $('#tbl' + prefix + '0' + id + ' tbody').append(tr);

        $('#btnDelete_' + prefix + '_' + id + '_' + i).click(function() {
            var ii = this.id.split('_');
            DelRow(prefix, id, ii[3]);
            return false;
        });
    }
}

function AddRowEmployee(prefix, id) {
    var iRowNum =  eval('gRowNum' + prefix + '0' + id);

    //alert(iRowNum);
    var i = iRowNum + 1;
    var tr = "<tr id='tr" + prefix + "0" + id + "_" + i + "'>" +
             "<td class='tdDevFormNo'></td>" +
             "<td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal' id='txtDetail_" + prefix + "_" + id + "_" + i + "'></textarea></div></td>" +
             "<td class='tdDevFormDelete'><button id='btnDelete_" + prefix + "_" + id + "_" + i + "' class='btnDelete'>ลบ</button></td>" +
             "</tr>" +
             "";

    $('#tbl' + prefix + '0' + id + ' tbody').append(tr);

    eval('gRowNum' + prefix + '0' + id + '++');

    $('#btnDelete_' + prefix + '_' + id + '_' + i).click(function() {
        var ii = this.id.split('_');
        DelRow(prefix, id, ii[3]);
        return false;
    });
    
    return true;
}

function DelRow(prefix, id, no) {
    //alert(prefix);
    //alert(id);
    //alert(no);
    $('#txtDetail_' + prefix + '_' + id + '_' + no).val('');
    $('#tr' + prefix + '0' + id + '_' + no).hide();
}

$('#btnAddRowEmployee01').click(function() { AddRowEmployee('Employee', 1); return false; });
$('#btnAddRowEmployee02').click(function() { AddRowEmployee('Employee', 2); return false; });
$('#btnAddRowEmployee03').click(function() { AddRowEmployee('Employee', 3); return false; });
$('#btnAddRowEmployee04').click(function() { AddRowEmployee('Employee', 4); return false; });

$('#btnAddRowCommander01').click(function() { AddRowEmployee('Commander', 1); return false; });
$('#btnAddRowCommander02').click(function() { AddRowEmployee('Commander', 2); return false; });


function LoadPart03() {
    RebuildForm('Employee', 1, bOwner);
    RebuildForm('Employee', 2, bOwner);
    RebuildForm('Employee', 3, bOwner);
    RebuildForm('Employee', 4, bOwner);

    RebuildForm('Commander', 1, !bOwner);
    RebuildForm('Commander', 2, !bOwner);

    //$('#drpScore01').val('');

    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();

    //alert(username);
    //alert(evaluate_year);
    //alert(round);
    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadPart03&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=0&time=' + time(), /// change_yyy ///
        //type: 'POST',
        //data: "data=" + json,
        dataType: 'json',
        beforeSend: function() {
            $('#divBackground').show();
            $('#divWaiting').show();
        },
        complete: function() {
            $('#divBackground').hide();
            $('#divWaiting').hide();
        },
        success: function(json) {
            //alert(json.recordCount);
            //alert(json.detail[0].Employee01[0]);
            if (!json.isError) {
                if (json.recordCount > 0) {
                    var i;
                    gRowNumEmployee01 = json.detail[0].Employee01.length;
                    RebuildForm("Employee", 1, bOwner);
                    for (i = 0; i < json.detail[0].Employee01.length; i++) 
                        $('#txtDetail_Employee_1_' + (i + 1)).val(json.detail[0].Employee01[i]);

                    gRowNumEmployee02 = json.detail[0].Employee02.length;
                    RebuildForm("Employee", 2, bOwner);
                    for (i = 0; i < json.detail[0].Employee02.length; i++)
                        $('#txtDetail_Employee_2_' + (i + 1)).val(json.detail[0].Employee02[i]);

                    gRowNumEmployee03 = json.detail[0].Employee03.length;
                    RebuildForm("Employee", 3, bOwner);
                    for (i = 0; i < json.detail[0].Employee03.length; i++)
                        $('#txtDetail_Employee_3_' + (i + 1)).val(json.detail[0].Employee03[i]);

                    gRowNumEmployee04 = json.detail[0].Employee04.length;
                    RebuildForm("Employee", 4, bOwner);
                    for (i = 0; i < json.detail[0].Employee04.length; i++)
                        $('#txtDetail_Employee_4_' + (i + 1)).val(json.detail[0].Employee04[i]);

                    gRowNumCommander01 = json.detail[0].Commander01.length;
                    RebuildForm("Commander", 1, !bOwner);
                    for (i = 0; i < json.detail[0].Commander01.length; i++)
                        $('#txtDetail_Commander_1_' + (i + 1)).val(json.detail[0].Commander01[i]);

                    gRowNumCommander02 = json.detail[0].Commander02.length;
                    RebuildForm("Commander", 2, !bOwner);
                    for (i = 0; i < json.detail[0].Commander02.length; i++)
                        $('#txtDetail_Commander_2_' + (i + 1)).val(json.detail[0].Commander02[i]);
                }                    
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

$('#btnSave').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var revision = 0;

    var data = new tblPart03();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;
    data.revision = revision;
    data.editor = $('#lblLoginName').text();

    var i;
    for (i = 1; i <= gRowNumEmployee01; i++) { data.Employee01.push($('#txtDetail_Employee_1_' + i).val()); }
    for (i = 1; i <= gRowNumEmployee02; i++) { data.Employee02.push($('#txtDetail_Employee_2_' + i).val()); }
    for (i = 1; i <= gRowNumEmployee03; i++) { data.Employee03.push($('#txtDetail_Employee_3_' + i).val()); }
    for (i = 1; i <= gRowNumEmployee04; i++) { data.Employee04.push($('#txtDetail_Employee_4_' + i).val()); }

    for (i = 1; i <= gRowNumCommander01; i++) { data.Commander01.push($('#txtDetail_Commander_1_' + i).val()); }
    for (i = 1; i <= gRowNumCommander02; i++) { data.Commander02.push($('#txtDetail_Commander_2_' + i).val()); }
    var json = $.toJSON(data);
    //json = json.replace(/&/g, ' ');

    $.ajax({
        url: 'frmEvaluateService.aspx?q=SavePart03&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + iRevision + '&time=' + time(),  ///
        type: 'POST',
        //data: 'data=' + json,
        data: json,
        dataType: 'json',
        beforeSend: function() {
            $('#divBackground').show();
            $('#divWaiting').show();
        },
        complete: function() {
            $('#divBackground').hide();
            $('#divWaiting').hide();
        },
        success: function(json) {
            $('#divBackground').hide();
            $('#divWaiting').hide();
            if (!json.isError) {
                alert("บันทึกสำเร็จ");
            }
            else {
                alert(prefix_msg + "ล้มเหลว:" + json.ErrMessage);
            }
        }
    });

    return false;
});

jQuery(function($) {
    // Handle "Open Grid" links
    $("a.openGrid").nextAll().hide();
    $("body").on("click", "a.openGrid", function() {
        $(this).nextAll().slideToggle();
        return false;
    });
    // Handle "Open Grid" popups
    $("body").on("click", ".questionTypeGrid .popup label2", function() {
        var el = $(this),
            val = el.find("input").val(),
            answers = el.parents(".inputWrapper").find(".answersGrid label2");
        el.parent().slideUp();
        el.parents(".grid").prev("input").val(el.find("input").val());
        answers.hide();
        answers.filter(":lt(" + val + ")").show();
        answers.find("input:last").change();
    });
    // Handle Answers Grid
    $("body").on("change", ".answersGrid input", function() {
        var el = $(this),
            num = el.parents(".grid").find("label2:visible input:checked").length;
        el.parents(".inputWrapper").find(".numberOfAnswers").val(num || "");
    });
    // Handle TEXTAREA "notEmpty" class
    $("body").on("blur", "textarea", function() {
        var el = $(this);
        el.toggleClass("notEmpty", el.val() !== "");
    });
    // Handle "Add Another Question" button
    var questionTemplate = $(".inputWrapper").clone();
    $("a.addAnotherQuestion").click(function() {
    $(".inputWrapper:last").after(questionTemplate.clone());
});
});

function tblPart03() {
    this.id = 'tblPart03';
    this.username = '';
    this.evaluate_year = '';
    this.round = '';
    this.revisoin = '';
    this.editor = '';
    this.record_datetime = '';

    this.Employee01 = new Array();
    this.Employee02 = new Array();
    this.Employee03 = new Array();
    this.Employee04 = new Array();

    this.Commander01 = new Array();
    this.Commander02 = new Array();

    this.detail = new Array();
    this.recordCount = 0;

    this.isError = false;
    this.ErrMessage = '';
}

function LoadEvaluateHeader4Confirm(username, evaluate_year, round, revision, obj_btn, obj_ret_message) {

    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadEvaluateHeader&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time(), /// change_yyy ///
        //type: 'POST',
        //data: "data=" + json,
        dataType: 'json',
        beforeSend: function() {
            $('#divBackground').show();
            $('#divWaiting').show();
        },
        complete: function() {
            $('#divBackground').hide();
            $('#divWaiting').hide();
        },
        success: function(json) {

            if (!json.isError) {

                if (json.detail[0].confirmed == '1') {

                    if (iRevision != '1') {
                        btnDisable(obj_btn);
                    }
                    else {
                        btnShow(obj_btn);
                    }

                    obj_ret_message.text('ข้อมูลทั้งหมดได้รับการยืนยันแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')')
                }
                else {
                    btnShow(obj_btn);

                    obj_ret_message.text("");
                }
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function btnHide(obj) {
    obj.hide();
}

function btnShow(obj) {
    obj.show();
    obj.removeAttr("disabled");
    obj.attr('aria-disabled', 'false');
    obj.attr('class', 'save ui-button ui-widget ui-state-default ui-corner-all ui-button-text-icon-primary');
}

function btnDisable(obj) {
    obj.attr('disabled', 'disabled');
    obj.attr('aria-disabled', 'true');
    obj.attr('class', 'save ui-button ui-widget ui-state-default ui-corner-all ui-button-disabled ui-state-disabled ui-button-text-icon-primary');
}
