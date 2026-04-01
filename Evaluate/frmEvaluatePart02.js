
var bOwner = true;
var iRevision = 0;
var bLevelManager = true;
var data;

$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    $('.normal').autosize();

    bOwner = isOwner();

    if ($('#txtLevelManager').val() == '1') {
        bLevelManager = true;
        data = Data0202B;
        $('.divSumScore25').show();
    } else {
        bLevelManager = false;
        data = Data0202;
        $('.divSumScore25').hide();
    }
    LoadData(data);
    LoadPart0202();
    CalSumScore(bLevelManager);

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

$('#btnPrintImage').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var page = 'EvaluatePart02';
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

$('#btnExport').click(function() {
    //alert ('btnExport');
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    //var round = $('#drpRoundNo').val();
    var round = $('#lblRoundNo').text();
    var revision = iRevision;

    var service_name = 'ExportEvaluateForm';

    var url = 'frmEvaluateService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time();

    window.location = url;

    return false;
});

function CalSumScore(bLevelManager) {
    var m, i, j, ia = 0;
    var SumScore = 0;
    var SumEmpScore = 0;
    var SumScore25 = 0;
    var SumEmpScore25 = 0;
    var bCommandCheck = false;

    for (ia = 2; ia <= 5; ia++) {
        var score = 0;
        score = parseFloat($('#drpScore' + pad(ia, 2)).val());
        if (score >= 0) { SumScore25 += score; }
        score = parseInt($('#drpEmpScore' + pad(ia, 2)).val());
        if (score >= 0) { SumEmpScore25 += score; }
    }
    ia = 0;
    for (m = 0; m < data.M.length; m++) {
        for (i = 0; i < data.M[m].I.length; i++) {
            ia++;
            var score = 0;
            score = parseFloat($('#drpScore' + pad(ia, 2)).val());
            if (score >= 0) { SumScore += score; }
            if (score >= 0) { bCommandCheck = true; }
            score = parseInt($('#drpEmpScore' + pad(ia, 2)).val());
            if (score >= 0) { SumEmpScore += score; }
        }
    }

    $('#lblSumScore').text(SumScore.toFixed(2));
    $('#lblSumScore25').text(SumScore25);

    $('#lblSumEmpScore').text(SumEmpScore);
    $('#lblSumEmpScore25').text(SumEmpScore25);
    
    if (bLevelManager == true) {
        $('#lblSumScore100').text((SumScore * 100 / 135).toFixed(2));
        $('#lblSumEmpScore100').text((SumEmpScore * 100 / 135).toFixed(2));
        
        if (bCommandCheck == false) {
            $('#chkNotPass').removeAttr('checked');
            $('#chkPass').removeAttr('checked');
        }
        else if ((SumScore >= 87.75) && (SumScore25 >= 35)) {
            $('#chkPass').attr('checked', 'checked');
            $('#chkNotPass').removeAttr('checked');
        }
        else {
            $('#chkNotPass').attr('checked', 'checked');
            $('#chkPass').removeAttr('checked');
        }
    } else {
        $('#lblSumScore100').text((SumScore * 100 / 95).toFixed(2));
        $('#lblSumEmpScore100').text((SumEmpScore * 100 / 95).toFixed(2));

        if (bCommandCheck == false) {
            $('#chkNotPass').removeAttr('checked');
            $('#chkPass').removeAttr('checked');
        }
        else if (SumScore >= 57) {
            $('#chkPass').attr('checked', 'checked');
            $('#chkNotPass').removeAttr('checked');
        }
        else {
            $('#chkNotPass').attr('checked', 'checked');
            $('#chkPass').removeAttr('checked');
        }
    }
}

function LoadData(data) {
    var m, i, j, ia = 0;

    var all_tr = '';
    var tr = '';

    var sEmpDisabled = '';
    var sDisabled = 'disabled';
    if (!bOwner) { sDisabled = ''; sEmpDisabled = 'disabled'; }
    
    $('#tblFormPart0202 tbody').empty();
    for (m = 0; m < data.M.length; m++) {

        tr =  "<tr class='trCommanderForm'>" +
                  " <th class='thbFormNo'></th>" +
                  " <th class='thbFormDetail'>หมวดที่ " + (m + 1) + " " + data.M[m].title + "</th>" +
                  " <th class='thbFormScore'></th>" +
                  " <th class='thbFormScoreA'></th>" +
                  " <th class='thbFormScoreB'></th>" +
                  " <th class='thbFormDetailA'></th>" +
                  " </tr>" +
                  " ";

        all_tr += tr;
        //alert(data.M[m].I.length);
        
        for (i = 0; i < data.M[m].I.length; i++) {
            ia++;
            tr = "<tr>" +
                 " <td class='tdmFormNo' rowspan='5'>" + ia + "</td>" +
                 " <td class='tdmFormDetail'><b><u>" + data.M[m].I[i].title + "</u></b> " + data.M[m].I[i].description + "</td>" +
                 " <td class='tdmFormScore'></td>" +
                 " <td class='tdmFormScoreA' rowspan='5'>" +
                 "   <select id='drpEmpScore" + pad(ia, 2) + "' " + sEmpDisabled + " class='class_drp' >" +
                 "";

            //alert(data.M[m].I[i].maxscore);
            tr += " <option value='-1'>ยังไม่ได้ระบุ</option>";
            var l = 0;
            for (l = data.M[m].I[i].maxscore; l >= 0 ; l--) {
                tr += " <option value='" + l + "'>" + l + "</option>";
            }

            tr +=
                 "    </select>" +
                 " </td>" +
                 "";

            tr +=
                 " <td class='tdmFormScoreB' rowspan='5'>" +
                 "  <input type='text' id='drpScore" + pad(ia, 2) + "' " + sDisabled + " class='txtWeight class_drp' />" +
                 " </td>" +
                 "";
            /*tr +=
                 " <td class='tdmFormScoreB' rowspan='5'>" +
                 "   <select id='drpScore" + pad(ia, 2) + "' " + sDisabled + " class='class_drp' >" +
                 "";

             //alert(data.M[m].I[i].maxscore);
             tr += " <option value='-1'>ยังไม่ได้ระบุ</option>";
             for (l = data.M[m].I[i].maxscore; l >= 0; l--) {
                 tr += " <option value='" + l + "'>" + l + "</option>";
             }

             tr +=
                 "    </select>" +
                 " </td>" +
                 "";*/
                 
            tr +=                 
                 " <td class='tdmFormDetail' rowspan='5'>" +
                 "    <div class='inputWrapper'>" +
                 "      <label>" +
                 "          <textarea  class='normal' id='txtSuggestion" + pad(ia,2) + "' name='txtSuggestion" + pad(ia,2) + "' rows='15' " + sDisabled + "></textarea>" +
                 "      </label>" +
                 "    </div>" +
                 " </td>" +
                 "</tr>" +
                 " ";

             all_tr += tr;
            

            for (j = 0; j < data.M[m].I[i].J.length; j++) {

                tr = "<tr>" +
                     " <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- " + data.M[m].I[i].J[j].title + "</td>" +
                     " <td class='tdFormScore'>" + data.M[m].I[i].J[j].score + "</td>" +
                     " <td></td>" +
                     "</tr> " +
                     " ";

                all_tr += tr;
            }            
        }
    }

    $('#tblFormPart0202 tbody').append(all_tr);

    //save old data    
    $('.class_drp').focusin(function() {
        $(this).data('val', $(this).val());
    });

    $('.class_drp').change(function() {
        var max_val = $('#drpEmpScore' + this.id.slice(-2) + ' option').eq(1).val();
        //alert(this.id);
        //alert($(this).data('val'));
        //alert($(this).val() + ' ' + max_val);
        //alert(parseFloat($(this).val()));
        if (!(parseFloat($(this).val()).toString() === $(this).val().toString())) {
            $('#' + this.id).val($(this).data('val'));
        } else if ((parseFloat($(this).val()) < -1) || (parseFloat($(this).val()) > max_val)) {
            $('#' + this.id).val($(this).data('val'));
        } else {
            //$('#' + this.id).val(parseFloat($(this).val()).toFixed(2));
            $('#' + this.id).val(parseFloat($(this).val()).toString());
            CalSumScore(bLevelManager);
        }
        //$('#' + this.id).val('1');
        
    });
}

function isInt(x) {
    return !isNaN(x) && eval(x).toString().length == parseInt(eval(x)).toString().length
}

function isFloat(x) {
    return !isNaN(x) && !isInt(eval(x)) && x.toString().length > 0
}


function LoadPart0202() {
    $('#txtSuggestion01').val('');
    $('#txtSuggestion02').val('');
    $('#txtSuggestion03').val('');
    $('#txtSuggestion04').val('');
    $('#txtSuggestion05').val('');
    $('#txtSuggestion06').val('');
    $('#txtSuggestion07').val('');
    $('#txtSuggestion08').val('');
    $('#txtSuggestion09').val('');
    $('#txtSuggestion10').val('');
    $('#txtSuggestion11').val('');

    //$('#drpScore01').val('');

    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();

    //alert(username);
    //alert(evaluate_year);
    //alert(round);
    $.ajax({
    url: 'frmEvaluateService.aspx?q=LoadPart0202&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=0&sum_only=0&time=' + time(), /// change_yyy ///
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
            //alert(json.detail[0].Suggestion01);
            if (!json.isError) {
                if (json.recordCount > 0) {
                    $('#txtSuggestion01').val(json.detail[0].Suggestion01);
                    $('#txtSuggestion02').val(json.detail[0].Suggestion02);
                    $('#txtSuggestion03').val(json.detail[0].Suggestion03);
                    $('#txtSuggestion04').val(json.detail[0].Suggestion04);
                    $('#txtSuggestion05').val(json.detail[0].Suggestion05);
                    $('#txtSuggestion06').val(json.detail[0].Suggestion06);
                    $('#txtSuggestion07').val(json.detail[0].Suggestion07);
                    $('#txtSuggestion08').val(json.detail[0].Suggestion08);
                    $('#txtSuggestion09').val(json.detail[0].Suggestion09);
                    $('#txtSuggestion10').val(json.detail[0].Suggestion10);
                    $('#txtSuggestion11').val(json.detail[0].Suggestion11);

                    $('#drpScore01').val(json.detail[0].Score01);
                    $('#drpScore02').val(json.detail[0].Score02);
                    $('#drpScore03').val(json.detail[0].Score03);
                    $('#drpScore04').val(json.detail[0].Score04);
                    $('#drpScore05').val(json.detail[0].Score05);
                    $('#drpScore06').val(json.detail[0].Score06);
                    $('#drpScore07').val(json.detail[0].Score07);
                    $('#drpScore08').val(json.detail[0].Score08);
                    $('#drpScore09').val(json.detail[0].Score09);
                    $('#drpScore10').val(json.detail[0].Score10);
                    $('#drpScore11').val(json.detail[0].Score11);

                    $('#drpEmpScore01').val(json.detail[0].EmpScore01);
                    $('#drpEmpScore02').val(json.detail[0].EmpScore02);
                    $('#drpEmpScore03').val(json.detail[0].EmpScore03);
                    $('#drpEmpScore04').val(json.detail[0].EmpScore04);
                    $('#drpEmpScore05').val(json.detail[0].EmpScore05);
                    $('#drpEmpScore06').val(json.detail[0].EmpScore06);
                    $('#drpEmpScore07').val(json.detail[0].EmpScore07);
                    $('#drpEmpScore08').val(json.detail[0].EmpScore08);
                    $('#drpEmpScore09').val(json.detail[0].EmpScore09);
                    $('#drpEmpScore10').val(json.detail[0].EmpScore10);
                    $('#drpEmpScore11').val(json.detail[0].EmpScore11);

                    $('#lblSumScore').val(json.detail[0].SumScore);
                    $('#lblSumScore25').val(json.detail[0].SumScore25);
                    $('#lblSumScore100').val(json.detail[0].SumScore100);

                    CalSumScore(bLevelManager);
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

    var data = new tblPart0202();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;
    data.revision = revision;
    data.editor = $('#lblLoginName').text();

    data.Suggestion01 = $('#txtSuggestion01').val();
    data.Suggestion02 = $('#txtSuggestion02').val();
    data.Suggestion03 = $('#txtSuggestion03').val();
    data.Suggestion04 = $('#txtSuggestion04').val();
    data.Suggestion05 = $('#txtSuggestion05').val();
    data.Suggestion06 = $('#txtSuggestion06').val();
    data.Suggestion07 = $('#txtSuggestion07').val();
    data.Suggestion08 = $('#txtSuggestion08').val();
    if ($('#txtLevelManager').val() == '1') {
        data.Suggestion09 = $('#txtSuggestion09').val();
        data.Suggestion10 = $('#txtSuggestion10').val();
        data.Suggestion11 = $('#txtSuggestion11').val();
    }

    data.Score01 = $('#drpScore01').val();
    data.Score02 = $('#drpScore02').val();
    data.Score03 = $('#drpScore03').val();
    data.Score04 = $('#drpScore04').val();
    data.Score05 = $('#drpScore05').val();
    data.Score06 = $('#drpScore06').val();
    data.Score07 = $('#drpScore07').val();
    data.Score08 = $('#drpScore08').val();
    if ($('#txtLevelManager').val() == '1') {
        data.Score09 = $('#drpScore09').val();
        data.Score10 = $('#drpScore10').val();
        data.Score11 = $('#drpScore11').val();
    }

    data.EmpScore01 = $('#drpEmpScore01').val();
    data.EmpScore02 = $('#drpEmpScore02').val();
    data.EmpScore03 = $('#drpEmpScore03').val();
    data.EmpScore04 = $('#drpEmpScore04').val();
    data.EmpScore05 = $('#drpEmpScore05').val();
    data.EmpScore06 = $('#drpEmpScore06').val();
    data.EmpScore07 = $('#drpEmpScore07').val();
    data.EmpScore08 = $('#drpEmpScore08').val();
    if ($('#txtLevelManager').val() == '1') {
        data.EmpScore09 = $('#drpEmpScore09').val();
        data.EmpScore10 = $('#drpEmpScore10').val();
        data.EmpScore11 = $('#drpEmpScore11').val();
    }

    data.SumScore = $('#lblSumScore').text();
    data.SumScore25 = $('#lblSumScore25').text();
    data.SumScore100 = $('#lblSumScore100').text();

    data.SumEmpScore = $('#lblSumEmpScore').text();
    data.SumEmpScore25 = $('#lblSumEmpScore25').text();
    data.SumEmpScore100 = $('#lblSumEmpScore100').text();

    //data.Result = $("#chkPass").is(':checked') ? 1 : 0;
    data.Result = 0;
    if ($("#chkPass").is(':checked')) {
        data.Result = 1;
    } else if ($("#chkNotPass").is(':checked')) {
        data.Result = -1;
    }

    var json = $.toJSON(data);
    //json = json.replace(/&/g, ' ');

    $.ajax({
        url: 'frmEvaluateService.aspx?q=SavePart0202&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + iRevision + '&time=' + time(),  ///
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
                alert("ล้มเหลว:" + json.ErrMessage);
            }
        }
    });

    return false;
});

function tblPart0202() {
    this.id = 'tblPart0202';
    this.username = '';
    this.evaluate_year = '';
    this.round = '';
    this.revisoin = '';
    this.editor = '';
    this.record_datetime = '';

    this.Score01 = '';
    this.Score02 = '';
    this.Score03 = '';
    this.Score04 = '';
    this.Score05 = '';
    this.Score06 = '';
    this.Score07 = '';
    this.Score07 = '';
    this.Score08 = '';
    this.Score09 = '';
    this.Score10 = '';
    this.Score11 = '';

    this.EmpScore01 = '';
    this.EmpScore02 = '';
    this.EmpScore03 = '';
    this.EmpScore04 = '';
    this.EmpScore05 = '';
    this.EmpScore06 = '';
    this.EmpScore07 = '';
    this.EmpScore07 = '';
    this.EmpScore08 = '';
    this.EmpScore09 = '';
    this.EmpScore10 = '';
    this.EmpScore11 = '';

    this.Suggestion01 = '';
    this.Suggestion02 = '';
    this.Suggestion03 = '';
    this.Suggestion04 = '';
    this.Suggestion05 = '';
    this.Suggestion06 = '';
    this.Suggestion07 = '';
    this.Suggestion08 = '';
    this.Suggestion09 = '';
    this.Suggestion10 = '';
    this.Suggestion11 = '';

    this.SumScore = '';
    this.SumScore25 = '';
    this.SumScore100 = '';
    this.Result = '';

    this.confirmed = '';
    this.confirm_datetime = '';

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

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
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
