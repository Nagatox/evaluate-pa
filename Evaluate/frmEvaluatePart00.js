
var bOwner = true;
var iRevision = 0;
var giffull = 70;

var giffull0201 = 70;
var giffull0201remain = 0;
var giffull0202 = 30;

$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    $('#lblPart0201Percent').text(giffull0201);
    $('#lblPart0201PercentRemain').text(giffull0201remain);
    $('#lblPart0202Percent').text(giffull0202);

    $('#txtPart04_0201X').val('0');
    $('#txtPart04_0201XScore').val('0.00');

    $('#lblConfirmMessage').text('');
    //$('#btnConfirm').removeAttr('disabled');
    btnShow($('#btnConfirm'));

    $('#divMainPart0201').show();
    $('#divMainPart0202').show();
    $('#divMainPart03').show();
    
    btnHide($('#btnExport'));

    bOwner = isOwner();

    if (!bOwner) {
        $('#btnConfirm').hide();
        $('#btnPart04').show();
        $('#chkPart04Pass').removeAttr('disabled');
        $('#chkPart04NotPass').removeAttr('disabled');
        $('#txtPart04_0201X').removeAttr('disabled');
        //$('#txtPart04UpPercent').removeAttr('disabled');
        //$('#txtPart04Level').removeAttr('disabled');
    } else {
        $('#btnConfirm').show();
        $('#btnPart04').hide();
        $('#chkPart04Pass').attr('disabled', 'disabled');
        $('#chkPart04NotPass').attr('disabled', 'disabled');
        $('#txtPart04_0201X').attr('disabled', 'disabled');
    }

    UpdatePart04();

    LoadEvaluateMain();

    var username = $('#txtUserName').val();
    var iRevision = $('#txtResult').val();
    var sresult = '&result=' + iRevision;
    if (iRevision != '1') sresult = "";
    $('#linkForm00').attr('href', 'frmEvaluatePart00.aspx?u=' + username + sresult);
    $('#linkFormPA').attr('href', 'frmFormPA.aspx?u=' + username + sresult);
    $('#linkForm01').attr('href', 'frmEvaluateFormPA.aspx?u=' + username + sresult);
    $('#linkForm02').attr('href', 'frmEvaluatePart02.aspx?u=' + username + sresult);
    $('#linkForm03').attr('href', 'frmEvaluatePart03.aspx?u=' + username + sresult);

    $('#ahrefPart0201').attr('href', 'frmEvaluateFormPA.aspx?u=' + username + sresult);
    $('#ahrefPart0202').attr('href', 'frmEvaluatePart02.aspx?u=' + username + sresult);
    $('#ahrefPart03').attr('href', 'frmEvaluatePart03.aspx?u=' + username + sresult);    

});

function isOwner() {
    return ($('#lblLoginName').text() == $('#txtUserName').val());
}

$('#btnPart01S04').click(function() {
    if ($('#btnPart01S04').val() == 'โดยมีวันหยุดงาน ดังนี้') {
        $('#btnPart01S04').val('ซ่อน');
        $('.trPart01S04').show();
    } else {
    $('#btnPart01S04').val('โดยมีวันหยุดงาน ดังนี้');
        $('.trPart01S04').hide();
    }
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

$('#btnPrintImage').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var page = 'EvaluatePart00';
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

function LoadEvaluateMain() {
    //$('#drpScore01').val('');

    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();

    //alert(username);
    //alert(evaluate_year);
    //alert(round);
    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadEvaluateMain&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=0&time=' + time(), /// change_yyy ///
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
                    $('#txtFullName').val(json.detail[0].firstname + ' ' + json.detail[0].lastname);
                    $('#txtPosition').val(json.detail[0].position);
                    $('#txtEmployeeID').val(json.detail[0].employee_id);
                    $('#txtDepartmentName').val(json.detail[0].department_name);

                    $('#txtStartWorkDate').val(json.detail[0].start_work_date);
                    if (json.detail[0].start_work_date.trim().length > 0) {
                        var pattern = /(\d{2})\/(\d{2})\/(\d{4})/;
                        var d1 = new Date(json.detail[0].start_work_date.replace(pattern, '$3-$2-$1'));
                        var d2 = new Date();
                        //var y = DateDiff.inYears(d1, d2);
                        //var m = DateDiff.inMonths(d1, d2) - (y * 12);
                        //$('#txtTotalWorkDay').val((y + 543) + 'ปี ' + m + 'เดือน');
                        var diff = new Date(d2.getTime() - d1.getTime());
                        var y = (diff.getUTCFullYear() - 1970);
                        var m = diff.getUTCMonth();
                        $('#txtTotalWorkDay').val((y + 543) + 'ปี ' + m + 'เดือน');
                    }

                    $('#txtLeaveStudyStatus').val((json.detail[0].leave_study_status == '1' ? 'ลาศึกษาต่อ' : 'ไม่ได้ลาศึกษาต่อ'));
                    $('#txtLeaveStudyStartDate').val(json.detail[0].leave_study_start_date);
                    $('#txtLeaveStudyEndDate').val(json.detail[0].leave_study_end_date);
                    $('#txtLeaveStudyBackDate').val(json.detail[0].leave_study_end_date);

                    $('#txtPromoted').val(json.detail[0].promoted);
                    $('#txtPromotedPercent').val(json.detail[0].promoted_percent);
                    $('#txtDisciplinaryDate').val(json.detail[0].disciplinary_date);
                    $('#txtDisciplinary').val(json.detail[0].disciplinary);

                    $('#txtLeaveSick').val(json.detail[0].leave_sick == '0.00' ? 0 : json.detail[0].leave_sick);
                    $('#txtLeaveSickGuarantee').val(json.detail[0].leave_sick_guarantee == '0.00' ? 0 : json.detail[0].leave_sick_guarantee);
                    $('#txtLeaveCarer').val(json.detail[0].leave_carer == '0.00' ? 0 : json.detail[0].leave_carer);
                    $('#txtLate').val(json.detail[0].late == '0.00' ? 0 : json.detail[0].late);

                    $('#txtLeaveVacation').val(json.detail[0].leave_vacation == '0.00' ? 0 : json.detail[0].leave_vacation);
                    $('#txtLeaveMaternity').val(json.detail[0].leave_maternity == '0.00' ? 0 : json.detail[0].leave_maternity);
                    $('#txtLeaveOrdination').val(json.detail[0].leave_ordination == '0.00' ? 0 : json.detail[0].leave_ordination);
                    $('#txtLackAgenturer').val(json.detail[0].lack_agenturer == '0.00' ? 0 : json.detail[0].lack_agenturer);

                    //$('#txtWorkingDayInYear').val(' -' + (json.detail[0].leave_sum == '0.00' ? 0 : json.detail[0].leave_sum));
                    $('#txtWorkingDayInYear').val('');

                    $('#txtLevel').val(json.detail[0].level);

                    var i;
                    for (i = 0; i < ConfigLevel.array.length; i++) {
                        if (ConfigLevel.array[i].level == $('#txtLevel').val()) {
                            giffull0201 = ConfigLevel.array[i].percent;
                            giffull0201remain = giffull - giffull0201;

                            //$('#txtPart04_0201X').val('0');
                        }
                    }
                    $('#lblPart04_0201Percent').text(giffull0201);
                    $('#lblPart04_0201PercentRemain').text(giffull0201remain);
                    $('#lblPart04_0202Percent').text(giffull0202);
                    UpdateQuality ();

                    /*if (json.detail[0].part0201status != '0') {
                        //$('#divMainPart0201').show();
                        LoadEvaluatePart0201(username, evaluate_year, round, "");*/
                    if (json.detail[0].part0201pastatus != '0') {
                            LoadEvaluatePart0201pa(username, evaluate_year, round, "");
                    } else {
                        btnDisable($('#btnConfirm'));
                        //$('#btnConfirm').attr('disabled', 'disabled');
                        $('#lblConfirmMessage').text('ยังไม่สามารถยืนยันได้ เนื่องจากยังบันทึกแบบฟอร์มไม่ครบ');
                    }
                    if (json.detail[0].part0202status != '0') {
                        //$('#divMainPart0202').show();
                        LoadEvaluatePart0202(username, evaluate_year, round, 0);
                    } else {
                        //$('#btnConfirm').attr('disabled', 'disabled');
                        btnDisable($('#btnConfirm'));
                        $('#lblConfirmMessage').text('ยังไม่สามารถยืนยันได้ เนื่องจากยังบันทึกแบบฟอร์มไม่ครบ');
                    }
                    if (json.detail[0].part03status != '0') {
                        //$('#divMainPart03').show();
                        LoadEvaluatePart03(username, evaluate_year, round, 0);
                    } else {
                        //$('#btnConfirm').attr('disabled', 'disabled');
                        btnDisable($('#btnConfirm'));
                        $('#lblConfirmMessage').text('ยังไม่สามารถยืนยันได้ เนื่องจากยังบันทึกแบบฟอร์มไม่ครบ');
                    }

                    LoadEvaluatePart04(username, evaluate_year, round, 0);

                    if (json.detail[0].confirmed == '1') {
                        //$('#btnConfirm').attr('disabled', 'disabled');
                        btnDisable($('#btnConfirm'));
                        $('#lblConfirmMessage').text('ผู้ใช้ยืนยันข้อมูลแล้ว ณวันที ' + json.detail[0].confirm_datetime);
                    }
                }
            }
            else {
                alert('Load Fail:LoadEvaluateMain:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluatePart0201pa(username, evaluate_year, round, revision) {
    //alert(username);
    //var json = $.toJSON(pageData);
    //var json = { username: username, evaluate_year: '2557', round: '2' };

    $('#txtSuggestion').val('');

    //var evaluate_year = $('#lblBudgetYear').text();
    //var round = $('#drpRoundNo').val();
    //var round = $('#lblRoundNo').text();
    $.ajax({
        url: 'frmPAService.aspx?q=LoadPAHeader&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time(), /// change_yyy ///
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
            if (!json.isError) {
                $('#txtSuggestion').val(json.detail[0].suggest);
                $('#txtConfirmed').val(json.detail[0].confirmed);
                $('#txtConfirmDatetime').val(json.detail[0].confirm_datetime);

                $('#lblSumSuccess').text(json.detail[0].sum_score);
                $('#lblSumPercent').text(parseFloat(json.detail[0].sum_percent).toFixed(2));

                $('#lblPart0201Message').text('บันทึกล่าสุดเมื่อ ' + json.detail[0].confirm_datetime);

                // Update Score in Part 4
                $('#txtPart04_0201').val(json.detail[0].sum_percent);
                $('#txtPart04_0201Score').val((parseFloat(json.detail[0].sum_percent)*70)/100);

                $('#txtPart04Score').val(parseFloat($('#txtPart04_0201Score').val()) + parseFloat($('#txtPart04_0202Score').val()) + parseFloat($('#txtPart04_0201XScore').val()));

                bConfirmed = json.detail[0].confirmed;
                bResultConfirmed = (json.detail[0].result_confirmed)? json.detail[0].result_confirmed : '0';
                bBossConfirmed = (json.detail[0].boss_confirmed)? json.detail[0].boss_confirmed : '0';
                bBossResultConfirmed = (json.detail[0].boss_result_confirmed)? json.detail[0].boss_result_confirmed : '0';
                console.log('bConfirmed:' + bConfirmed + ' bResultConfirmed:' + bResultConfirmed + ' bBossConfirmed:' + bBossConfirmed + ' bBossResultConfirmed:' + bBossResultConfirmed);
                console.log('images/pa_status_' + bBossResultConfirmed + '' + bBossConfirmed + '' + bResultConfirmed + '' + bConfirmed + '.png');
                $('#imgStatus').attr('src', 'images/pa_status_' + bBossResultConfirmed + '' + bBossConfirmed + '' + bResultConfirmed + '' + bConfirmed + '.png');


                /*if (json.detail[0].confirmed == '1') {
                bConfirmed = true;
                //$('#lblMessage').text('ข้อมูลทั้งหมดได้รับการยืนยันแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')')
                }
                else {
                bConfirmed = false;

                    $('#lblMessage').text("");
                }*/
            }
            else {
                alert('Load Fail:LoadEvaluatePart0201pa:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluatePart0201(username, evaluate_year, round, revision) {
    //alert(username);
    //var json = $.toJSON(pageData);
    //var json = { username: username, evaluate_year: '2557', round: '2' };

    $('#txtSuggestion').val('');

    //var evaluate_year = $('#lblBudgetYear').text();
    //var round = $('#drpRoundNo').val();
    //var round = $('#lblRoundNo').text();
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
            //alert(json.recordCount);
            if (!json.isError) {
                $('#txtSuggestion').val(json.detail[0].commander_recomment);
                $('#txtConfirmed').val(json.detail[0].confirmed);
                $('#txtConfirmDatetime').val(json.detail[0].confirm_datetime);

                $('#lblExpectPL1').text(json.detail[0].sum_excepted_PL1);
                $('#lblExpectPL2').text(json.detail[0].sum_excepted_PL2);
                $('#lblExpectPL3').text(json.detail[0].sum_excepted_PL3);
                $('#lblExpectPL4').text(json.detail[0].sum_excepted_PL4);
                $('#lblExpectPL5').text(json.detail[0].sum_excepted_PL5);

                $('#lblEvaluatePL1').text(json.detail[0].sum_evaluate_PL1);
                $('#lblEvaluatePL2').text(json.detail[0].sum_evaluate_PL2);
                $('#lblEvaluatePL3').text(json.detail[0].sum_evaluate_PL3);
                $('#lblEvaluatePL4').text(json.detail[0].sum_evaluate_PL4);
                $('#lblEvaluatePL5').text(json.detail[0].sum_evaluate_PL5);

                //$('#lblSumWeight').text(json.detail[0].SumWeight);
                //$('#lblSumPL').text(json.detail[0].SumPL);
                $('#lblSumSuccess').text(json.detail[0].SumSuccess);
                $('#lblFullSuccess').text(json.detail[0].FullSuccess);
                $('#lblPercentSuccess').text(json.detail[0].PercentSuccess);
                $('#lblIFFull').text(json.detail[0].IFFull);
                $('#lblIFSuccess').text(json.detail[0].IFSuccess);

                $('#lblPart0201Message').text('บันทึกล่าสุดเมื่อ ' + json.detail[0].record_datetime);

                // Update Score in Part 4
                $('#txtPart04_0201').val(json.detail[0].PercentSuccess);
                $('#txtPart04_0201Score').val(json.detail[0].IFSuccess);

                $('#txtPart04Score').val(parseFloat($('#txtPart04_0201Score').val()) + parseFloat($('#txtPart04_0202Score').val()) + parseFloat($('#txtPart04_0201XScore').val()));


                /*if (json.detail[0].confirmed == '1') {
                bConfirmed = true;
                //$('#lblMessage').text('ข้อมูลทั้งหมดได้รับการยืนยันแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')')
                }
                else {
                bConfirmed = false;

                    $('#lblMessage').text("");
                }*/
            }
            else {
                alert('Load Fail:LoadEvaluatePart0201:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluatePart0202(username, evaluate_year, round, revision) {
    //alert(username);
    //var json = $.toJSON(pageData);
    //var json = { username: username, evaluate_year: '2557', round: '2' };

    //$('#txtSuggestion').val('');

    //var evaluate_year = $('#lblBudgetYear').text();
    //var round = $('#drpRoundNo').val();
    //var round = $('#lblRoundNo').text();
    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadPart0202&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&sum_only=1&time=' + time(), /// change_yyy ///
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
            if (!json.isError) {
                $('#lblSumScore').text(json.detail[0].SumScore);
                $('#lblSumScore25').text(json.detail[0].SumScore25);
                $('#lblSumScore100').text(json.detail[0].SumScore100);

                $('#lblSumEmpScore').text(json.detail[0].SumEmpScore);
                $('#lblSumEmpScore25').text(json.detail[0].SumEmpScore25);
                $('#lblSumEmpScore100').text(json.detail[0].SumEmpScore100);

                if (json.detail[0].Result.trim() == '1') {
                    $('#chkPass').attr('checked', 'checked');
                    $('#chkNotPass').removeAttr('checked');
                }
                else if (json.detail[0].Result.trim() == '-1') {
                    $('#chkNotPass').attr('checked', 'checked');
                    $('#chkPass').removeAttr('checked');
                } else {
                    $('#chkPass').removeAttr('checked');
                    $('#chkNotPass').removeAttr('checked');
                }

                if (json.detail[0].LevelManager == '1') {
                    $('.divSumScore25').show();
                } else {
                    $('.divSumScore25').hide();
                }

                $('#lblPart0202Message').text('บันทึกล่าสุดเมื่อ ' + json.detail[0].record_datetime);

                // Update Score in Part 4
                //if (revision == 0) {
                if (json.detail[0].SumScore100 == '0') {
                    $('#txtPart04_0202').val(json.detail[0].SumEmpScore100);
                    $('#txtPart04_0202Score').val((parseFloat(json.detail[0].SumEmpScore100) * 30) / 100);

                    $('#txtPart04Score').val(parseFloat($('#txtPart04_0201Score').val()) + parseFloat($('#txtPart04_0202Score').val()) + parseFloat($('#txtPart04_0201XScore').val()));
                } else {
                    $('#txtPart04_0202').val(json.detail[0].SumScore100);
                    $('#txtPart04_0202Score').val((parseFloat(json.detail[0].SumScore100) * 30) / 100);

                    $('#txtPart04Score').val(parseFloat($('#txtPart04_0201Score').val()) + parseFloat($('#txtPart04_0202Score').val()) + parseFloat($('#txtPart04_0201XScore').val()));
                }

                /*if (json.detail[0].confirmed == '1') {
                bConfirmed = true;
                //$('#lblMessage').text('ข้อมูลทั้งหมดได้รับการยืนยันแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')')
                }
                else {
                bConfirmed = false;

                    $('#lblMessage').text("");
                }*/
            }
            else {
                alert('Load Fail:LoadEvaluatePart0202:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluatePart03(username, evaluate_year, round, revision) {

    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadPart03&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time(), /// change_yyy ///
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
            if (!json.isError) {
                var i;
                $('#tdPart03Employee').empty();
                $('#tdPart03Commander').empty();

                $('#lblPart03Message').text('บันทึกล่าสุดเมื่อ ' + json.detail[0].record_datetime);

                //$('#tdPart03Employee').append("<table style='width:100%'></table>");
                var detail = '';
                detail += ('1. ท่านขาดความรู้ ความชำนาญ ทักษะในด้านใดบ้าง' + '<br />');
                for (i = 0; i < json.detail[0].Employee01.length; i++)
                    if (json.detail[0].Employee01[i].trim().length > 0)
                        detail += ('&nbsp;&nbsp;&nbsp;- ' + json.detail[0].Employee01[i] + '<br />');

                detail += ('2. ท่านคิดว่าจะสามารถเพิ่มความรู้ ความชำนาญ ทักษะดังกล่าวได้โดยวิธีใด้บ้าง' + '<br />');
                for (i = 0; i < json.detail[0].Employee02.length; i++)
                    if (json.detail[0].Employee02[i].trim().length > 0)
                        detail += ('&nbsp;&nbsp;&nbsp;- ' + json.detail[0].Employee02[i] + '<br />');

                detail += ('3. ในช่วง 6 เดือนที่ผ่านมา ท่านได้เข้ารับการอบรมอะไรบ้าง' + '<br />');
                for (i = 0; i < json.detail[0].Employee03.length; i++)
                    if (json.detail[0].Employee03[i].trim().length > 0)
                        detail += ('&nbsp;&nbsp;&nbsp;- ' + json.detail[0].Employee03[i] + '<br />');

                detail += ('4. ท่านสนใจการฝึกอบรม หรือต้องการเรียนรู้เรื่องใดบ้างที่จะช่วยให้มีความสามารถปฏิบัติงานที่ได้รับมอบหมายในปัจจุบันได้ดียิ่งขึ้น (เรียงลำดับความสำคัญ 1-5)' + '<br />');
                for (i = 0; i < json.detail[0].Employee04.length; i++)
                    if (json.detail[0].Employee04[i].trim().length > 0)
                        detail += ('&nbsp;&nbsp;&nbsp;- ' + json.detail[0].Employee04[i] + '<br />');

                $('#tdPart03Employee').append(detail);
                detail = '';

                detail += ('1. ท่านคิดว่าผู้ใต้บังคับบัญชายังขาดความรู้ ความชำนาญ ทักษะในเรื่องใดบ้าง' + '<br />');
                for (i = 0; i < json.detail[0].Commander01.length; i++)
                    detail += ('&nbsp;&nbsp;&nbsp;- ' + json.detail[0].Commander01[i] + '<br />');

                detail += ('2. ท่านคิดว่าผู้ใต้บังคับบัญชาควรจะอบรม หรือต้องการความรู้เรื่องใดบ้างที่จะช่วยให้มีความสามารถปฏิบัติงานที่ได้รับมอบหมายในปัจจุบันได้ดียิ่งขึ้น (เรียงลำดับความสำคัญ 1-5)' + '<br />');
                for (i = 0; i < json.detail[0].Commander02.length; i++)
                    detail += ('&nbsp;&nbsp;&nbsp;- ' + json.detail[0].Commander02[i] + '<br />');

                $('#tdPart03Commander').append(detail);
            }
            else {
                alert('Load Fail:LoadEvaluatePart03:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluatePart04(username, evaluate_year, round, revision) {

    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadPart04&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time(), /// change_yyy ///
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
            if (!json.isError) {
                $('#txtPart04Level').val(json.detail[0].Level);
                $('#txtPart04UpPercent').val(json.detail[0].UpPercent);
                $('#txtPart04_0201X').val(json.detail[0].Quality);
                UpdateQuality(); 

                $('#chkPart04Pass').removeAttr('checked');
                $('#chkPart04NotPass').removeAttr('checked');

                if (json.detail[0].Result == '1') {
                    $('#chkPart04Pass').attr('checked', 'checked');
                } else if (json.detail[0].Result == '-1') {
                    $('#chkPart04NotPass').attr('checked', 'checked');
                }

                UpdatePart04();
                
                $('#txtPart04Score').val(parseFloat($('#txtPart04_0201Score').val()) + parseFloat($('#txtPart04_0202Score').val()) + parseFloat($('#txtPart04_0201XScore').val()));
            }
            else {
                alert('Load Fail:LoadEvaluatePart04:' + json.ErrMessage);
            }
        }
    });
}

function tblEvaluateHead() {
    this.id = 'tblEvaluateHead';
    this.username = '';
    this.evaluate_year = '';
    this.round = '';
    this.revisoin = '';

    this.sum_excepted_PL1 = '';
    this.sum_excepted_PL2 = '';
    this.sum_excepted_PL3 = '';
    this.sum_excepted_PL4 = '';
    this.sum_excepted_PL5 = '';

    this.sum_evaluate_PL1 = '';
    this.sum_evaluate_PL2 = '';
    this.sum_evaluate_PL3 = '';
    this.sum_evaluate_PL4 = '';
    this.sum_evaluate_PL5 = '';

    this.commander_recomment = '';

    this.detail = new Array();
    this.recordCount = 0;

    this.isError = false;
    this.ErrMessage = '';
}

function tblPart04() {
    this.id = 'tblPart04';
    this.username = '';
    this.evaluate_year = '';
    this.round = '';
    this.revisoin = '';
    this.editor = '';
    this.record_datetime = '';

    this.Result = '';
    this.UpPercent = '';
    this.Level = '';
    this.Quality = '';
    
    this.detail = new Array();
    this.recordCount = 0;

    this.isError = false;
    this.ErrMessage = '';
}

$('#btnPart04').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var revision = 0;

    var data = new tblPart04();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;
    data.revision = revision;
    data.editor = $('#lblLoginName').text();

    data.Result = $('#chkPart04Pass').is(':checked') ? 1 : 0;
    if (data.Result == 0) {
        data.Result = $('#chkPart04NotPass').is(':checked') ? -1 : 0;
    }
    data.UpPercent = $('#txtPart04UpPercent').val();
    data.Level = $('#txtPart04Level').val();
    data.Quality = $('#txtPart04_0201X').val();

    var json = $.toJSON(data);
    //json = json.replace(/&/g, ' ');

    $.ajax({
        url: 'frmEvaluateService.aspx?q=SavePart04&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + iRevision + '&time=' + time(),  ///
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

$('#chkPart04Pass').click(function() {
    //alert($('#chkPart04Pass').is(':checked'));
    UpdatePart04();
});

$('#chkPart04NotPass').click(function() {
    if ($('#chkPart04NotPass').is(':checked')) {
        $('#chkPart04Pass').removeAttr('checked');
    }

    UpdatePart04();
});

$('#txtPart04_0201X').change(function() {
    UpdateQuality();
    $('#txtPart04Score').val(parseFloat($('#txtPart04_0201Score').val()) + parseFloat($('#txtPart04_0202Score').val()) + parseFloat($('#txtPart04_0201XScore').val()));
});

function UpdateQuality() {
    var txtPart04_0201X = ($('#txtPart04_0201X').val() > 100) ? 100 : $('#txtPart04_0201X').val();
    var quality_score = $('#lblPart04_0201PercentRemain').text() * txtPart04_0201X / 100;
    $('#txtPart04_0201XScore').val(quality_score.toFixed(2));
}

function UpdatePart04() {
    if (!bOwner) {
        if ($('#chkPart04Pass').is(':checked')) {
            $('#chkPart04NotPass').removeAttr('checked');

            $('#txtPart04UpPercent').removeAttr('disabled');
            $('#txtPart04Level').removeAttr('disabled');
        }
        else {
            $('#txtPart04UpPercent').attr('disabled', 'disabled');
            $('#txtPart04Level').attr('disabled', 'disabled');
        }
    }
}

function tblPart00() {
    this.id = 'tblPart00';
    this.username = '';
    this.evaluate_year = '';
    this.round = '';
    this.revisoin = '';
    this.editor = '';
    this.record_datetime = '';

    this.confirmed = '';
    this.confirm_datetime = '';

    this.detail = new Array();
    this.recordCount = 0;

    this.isError = false;
    this.ErrMessage = '';
}

$('#btnConfirm').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var revision = 0;

    var data = new tblPart00();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;
    data.revision = revision;
    data.editor = $('#lblLoginName').text();

    data.confirmed = '1';

    var json = $.toJSON(data);
    //json = json.replace(/&/g, ' ');

    $.ajax({
        url: 'frmEvaluateService.aspx?q=SaveConfirm&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + iRevision + '&time=' + time(),  ///
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
            //alert(json.isError);
            $('#divBackground').hide();
            $('#divWaiting').hide();
            if (!json.isError) {
                alert("Confirm Completed");
                //$('#btnConfirm').attr('disabled', 'disabled');
                btnDisable($('#btnConfirm'));
                $('#lblConfirmMessage').text('ผู้ใช้ยืนยันข้อมูลแล้ว ณวันที ' + json.RetMessage);
            }
            else {
                alert("Confirm ล้มเหลว:" + json.ErrMessage);
            }
        }
    });

    return false;
});


var DateDiff = {

    inDays: function(d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000));
    },

    inWeeks: function(d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
    },

    inMonths: function(d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();

        return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
    },

    inYears: function(d1, d2) {
        return d2.getFullYear() - d1.getFullYear();
    }
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