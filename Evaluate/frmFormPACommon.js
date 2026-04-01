

$('#btnPrintImage').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();
    var page = 'FormPA';
    var result = ($('#lblFormType').text()==' (ประเมินตนเอง)')?'':'1';

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


function tblPAHeader() {
    this.id = 'tblPAHeader';
    this.username = '';
    this.year = '';

    this.user_entrydate = '';
    this.boss = '';
    this.boss_entrydate = '';
    this.boss2 = '';
    this.boss2_entrydate = '';

    this.suggest = '';

    this.detail = new Array();
    this.recordCount = 0;
    
    this.isError = false;
    this.ErrMessage = '';
}

function tblPADetail() {
    this.id = 'tblPADetail';
    this.username = '';
    this.year = '';
    this.item_group_no = '';
    this.item_no = '';

    this.detail = '';
    this.weight = '';
    this.month_08 = '';
    this.month_09 = '';
    this.month_10 = '';
    this.month_11 = '';
    this.month_12 = '';
    this.month_01 = '';
    this.month_02 = '';
    this.month_03 = '';
    this.month_04 = '';
    this.month_05 = '';
    this.month_06 = '';
    this.month_07 = '';
    this.KPI = '';
    this.STG = '';
    this.comment = '';

    this.revision = '';
    this.record_datetime = '';
    this.editor = '';
}

function tblPADetailResult() {
    this.id = 'tblPADetailResult';
    this.username = '';
    this.year = '';
    this.item_group_no = '';
    this.item_no = '';

    this.LoS = '';
    this.LoSBoss = '';
    this.comment2 = '';

    this.revision = '';
    this.record_datetime = '';
    this.editor = '';
}

$('#btnExport').click(function() {
    //alert ('btnExport');
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    //var round = $('#drpRoundNo').val();
    var round = 0;
    var revision = iRevision;

    var service_name = 'ExportPAForm';

    var url = 'frmPAService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time();

    /*if (username == $('#lblUserName').val()) {
        DoSaveAndConfirm('SaveEvaluate', url);
    } else {
        window.location = url;
    }*/

    window.location = url;

    return false;
});


function LoadPAHeader(username, revision) {

    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();
    LoadPAHeader01(username, revision, evaluate_year, round);
}

function LoadPAHeader01(username, revision, evaluate_year, round) {
    //alert(username);
    //var json = $.toJSON(pageData);
    //var json = { username: username, year: '2557', round: '2' };

    $('#txtSuggestion').val('');

    //var year = $('#lblBudgetYear').text();
    ////var round = $('#drpRoundNo').val();
    //var round = $('#lblRoundNo').text();
    $.ajax({
        url: 'frmPAService.aspx?q=LoadPAHeader&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
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
                //$('#user_entrydate').text(json.detail[0].user_entrydate);
                //$('#boss').text(json.detail[0].boss);
                //$('#boss_entrydate').text(json.detail[0].boss_entrydate);
                //$('#boss2').text(json.detail[0].boss2);
                //$('#boss2_entrydate').text(json.detail[0].boss2_entrydate);

                var username = json.detail[0].username;

                $('#txtSuggest').val(json.detail[0].suggest);
                $('#txtConfirmed').val(json.detail[0].confirmed);
                $('#txtConfirmDatetime').val(json.detail[0].confirm_datetime);

                console.log('g_evaluate_plan:' + g_evaluate_plan);
                bConfirmed = json.detail[0].confirmed;
                bResultConfirmed = (json.detail[0].result_confirmed)? json.detail[0].result_confirmed : '0';
                bBossConfirmed = (json.detail[0].boss_confirmed)? json.detail[0].boss_confirmed : '0';
                bBossResultConfirmed = (json.detail[0].boss_result_confirmed)? json.detail[0].boss_result_confirmed : '0';
                console.log('bConfirmed:' + bConfirmed + ' bResultConfirmed:' + bResultConfirmed + ' bBossConfirmed:' + bBossConfirmed + ' bBossResultConfirmed:' + bBossResultConfirmed);
                console.log('images/pa_status_' + bBossResultConfirmed + '' + bBossConfirmed + '' + bResultConfirmed + '' + bConfirmed + '.png');
                $('#imgStatus').attr('src', 'images/pa_status_' + bBossResultConfirmed + '' + bBossConfirmed + '' + bResultConfirmed + '' + bConfirmed + '.png');

                $('#lblMessage').text("");
                $('#lblMessage2').text("");

                if (g_evaluate_plan) {
                    Set2Owner(bOwner, bConfirmed=='1', bBossConfirmed=='1');

                    LoadPADetail(username, iRevision);
                    
                    if (json.detail[0].confirmed == '1') {
                        $('#lblMessage').text('- ผู้ประเมินได้ยืนยันข้อมูลแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')');
                    }
                    if (json.detail[0].boss_confirmed == '1') {
                        $('#lblMessage2').text('- ผู้บังคับบัญชาได้ยืนยันรับทราบแล้ว (เมื่อวันที่ ' + json.detail[0].boss_confirm_datetime + ')');
                    }
    
                } else {
                    if (json.detail[0].boss_confirmed == '1') {
                        Set2Owner(bOwner, bResultConfirmed=='1', bBossConfirmed=='1');

                        LoadPADetail(username, iRevision);
                        LoadPADetailResult(username, iRevision);

                        if (json.detail[0].result_confirmed == '1') {
                            $('#lblMessage').text('- ผู้ประเมินได้ยืนยันข้อมูลแล้ว (เมื่อวันที่ ' + json.detail[0].result_confirm_datetime + ')');
                        }
                        if (json.detail[0].boss_result_confirmed == '1') {
                            $('#lblMessage2').text('- ผู้บังคับบัญชาได้ยืนยันรับทราบแล้ว (เมื่อวันที่ ' + json.detail[0].boss_result_confirm_datetime + ')');
                        }
                    } else {
                        /*$('#lblMessage').text('- ผู้บังคับบัญชายังไม่ยืนยันรับทราบแผนการดำเนินงาน ไม่สามารถดำเนินการในเมนูนี้ได้');

                        OwnerEditable(false);
                        BossEditable(false);

                        btnDisable($('#btnSavePAResult'));
                        btnDisable($('#btnConfirmPAResult'));
                        
                        btnDisable($('#btnExportResult'));
                        btnDisable($('#btnPrintImageResult'));

                        btnHide($("#btnBossSaveResult"));
                        btnHide($("#btnBossUnlockResult"));
                        btnHide($("#btnBossConfirmResult"));*/

                        alert ('ผู้บังคับบัญชายังไม่ยืนยันรับทราบแผนการดำเนินงาน\n\n   ไม่สามารถดำเนินการในเมนูนี้ได้');
                        var username = $('#txtUserName').val();
                        window.location = 'frmFormPA.aspx?u=' + username;
                    }
                }
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function LoadPADetail(username, revision) {
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();

    LoadPADetail01(username, revision, evaluate_year, round);
}

function LoadPADetail01(username, revision, evaluate_year, round) {
    //alert(username);
    //var json = $.toJSON(pageData);

    //gRowNumArray = [3, 3, 3, 3, 3];
    CreateFormBody();

    $.ajax({
        url: 'frmPAService.aspx?q=LoadPADetail&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
        //type: 'POST',
        //data: { username: username },
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
                gRowNumArray = [3, 3, 3, 3, 3];
                for (i = 0; i < json.recordCount; i++) {
                    //alert (json.detail[i].activate_detail);
                    var item_group_no = json.detail[i].item_group_no;
                    var item_no = json.detail[i].item_no;
                    console.log('item_group_no:' + item_group_no + ' item_no:' + item_no + ' gRowNumArray:' + gRowNumArray[item_group_no-1] + ' json.detail[i].detail:' + json.detail[i].detail);   

                    if (json.detail[i].item_no > gRowNum) { CreateOneRow('#tbodyG_' + item_group_no, item_group_no, ++gRowNumArray[item_group_no-1]); }

                    $('#txtPADetail_' + item_group_no + '_' + item_no).val(json.detail[i].detail);
                    $('#txtPADetail2_' + item_group_no + '_' + item_no).val(json.detail[i].detail2);
                    $('#txtPAWeight_' + item_group_no + '_' + item_no).val(json.detail[i].weight);
                    if (json.detail[i].month_01 == 1) { $('#chkPAMonth_01_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_02 == 1) { $('#chkPAMonth_02_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_03 == 1) { $('#chkPAMonth_03_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_04 == 1) { $('#chkPAMonth_04_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_05 == 1) { $('#chkPAMonth_05_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_06 == 1) { $('#chkPAMonth_06_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_07 == 1) { $('#chkPAMonth_07_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_08 == 1) { $('#chkPAMonth_08_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_09 == 1) { $('#chkPAMonth_09_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_10 == 1) { $('#chkPAMonth_10_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_11 == 1) { $('#chkPAMonth_11_' + item_group_no + '_' + item_no).attr('checked', true); }
                    if (json.detail[i].month_12 == 1) { $('#chkPAMonth_12_' + item_group_no + '_' + item_no).attr('checked', true); }
                    $('#txtPAKPI_' + item_group_no+ '_' + item_no).val(json.detail[i].KPI);
                    $('#txtPASTG_' + item_group_no + '_' + item_no).val(json.detail[i].STG);
                    $('#txtPAComment_' + item_group_no + '_' + item_no).val(json.detail[i].comment);

                    $("#selPALoS_" + item_group_no + "_" + item_no).change();
                    $("#selPALoSBoss_" + item_group_no + "_" + item_no).change();
                }

                ResetEvent();
                UpdateCaluate();
                //UpdateCaluateResult();
                $('.normal').autosize();
                if (g_evaluate_plan) {
                    Set2Owner(bOwner, bConfirmed=='1', bBossConfirmed=='1');
                } else {
                    Set2Owner(bOwner, bResultConfirmed=='1', bBossConfirmed=='1');
                }
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
            //UpdateCaluate();
            //UpdateCaluateResult();
        }
    });
}

function LoadPADetailResult(username, revision) {
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();

    LoadPADetailResult01(username, revision, evaluate_year, round);
}

function LoadPADetailResult01(username, revision, evaluate_year, round) {
    //alert(username);
    //var json = $.toJSON(pageData);

    gRowNumArray = [3, 3, 3, 3, 3];
    CreateFormBody();

    $.ajax({
        url: 'frmPAService.aspx?q=LoadPADetailResult&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
        //type: 'POST',
        //data: { username: username },
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
                for (i = 0; i < json.recordCount; i++) {
                    //alert (json.detail[i].activate_detail);
                    var item_group_no = json.detail[i].item_group_no;
                    var item_no = json.detail[i].item_no;
                    //if (json.detail[i].item_no > gRowNum) { CreateOneRow('#tbodyG_' + item_group_no, item_group_no, ++gRowNumArray[item_group_no-1]); }

                    $('#selPALoS_' + item_group_no + '_' + item_no).val(json.detail[i].LoS);
                    $('#selPALoSBoss_' + item_group_no + '_' + item_no).val(json.detail[i].LoSBoss);
                    $('#txtPAComment2_' + item_group_no + '_' + item_no).val(json.detail[i].comment2);

                    $("#selPALoS_" + item_group_no + "_" + item_no).change();
                    $("#selPALoSBoss_" + item_group_no + "_" + item_no).change();
                }

                ResetEvent();
                //UpdateCaluate();
                UpdateCaluateResult();
                $('.normal').autosize();

                Set2Owner(bOwner, bResultConfirmed=='1');
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
            //UpdateCaluate();
            //UpdateCaluateResult();
        }
    });
}

function ResetEvent() {
    $("input").unbind();
    $('input').bind('change', function() { BeforeUpdateCaluate(this.id); });
    //$("select").unbind();
    //$('select').bind('change', function() { BeforeUpdateCaluate(this.id); });
    //$('#drpRoundNo').bind('change', function() { drpRoundNo_change(); });

    //$('#drpResult').bind('change', function() { drpResult_change(); });
}

$('.btnDetail').live('click', function() {
    //console.log(this.id);
    var aid = this.id.split('_');
    var no = aid[1];
    var id = aid[2];
    //console.log(no + ' ' + id);
    console.log($('#txtPADetail2_' + no + '_' + id).val());


    $('#lblPOPNo').text(no + '.' + id);
    $('#lblPOPName').text($('#txtPADetail_' + no + '_' + id).val());
    $('txtPOPDetail').css( 'height', '500px');
    $('txtPOPDetail').val('');
    $('#txtPOPDetail').val($('#txtPADetail2_' + no + '_' + id).val());

    //$('.normal').autosize();

    $('#EvaluateDetail_divModel').reveal();

    return false;
});

$('#btnPOPUpdate').click(function() {
    var aid = $('#lblPOPNo').text().split('.');
    var no = aid[0];
    var id = aid[1];
    console.log(no + ' ' + id);    
    
    $('#txtPADetail2_' + no + '_' + id).val($('#txtPOPDetail').val());
    
    $('#EvaluateDetail_divModel').trigger('reveal:close');
    return false;
});

$('#btnPOPClose').click(function() {
    $('#EvaluateDetail_divModel').trigger('reveal:close');
    return false;
});

function BeforeUpdateCaluate(id) {
    var name = id.split('_')[0];
    var idx = id.split('_')[1];
    //alert(name);
    switch (name) {
        case 'txtPAWeight':
        case 'txtWeight':
            UpdateCaluate();
            break;
        case 'radSuccess':
            var e = $('input[name=radSuccess_' + idx + ']:checked').val();
            if (e != null) {
                e = parseInt(e);
                e = 0.5 * (e + 1);
                $('#txtSuccess_' + idx).val(e);
            }
            //alert(e);
            UpdateCaluate();
            break;
        case 'txtSuccess':
            var e = $('#txtSuccess_' + idx).val();
            e = (e / 0.5) - 1;
            if ((e == 1) || (e == 2) || (e == 3) || (e == 4) || (e == 5) || (e == 6) || (e == 7)) {
                $('input:radio[name=radSuccess_' + idx + '][value=' + e + ']').attr('checked', true);
            } else {
                var e = $('input[name=radSuccess_' + idx + ']:checked').val();
                $('input:radio[name=radSuccess_' + idx + '][value=' + e + ']').attr('checked', false);
            }
            UpdateCaluate();
            break;
    }
    //UpdateCaluate();
}

function CreateFormBody() {
    //return false;
    $('#tbodyG_1').empty();
    $('#tbodyG_2').empty();
    $('#tbodyG_3').empty();
    $('#tbodyG_4').empty();
    $('#tbodyG_5').empty();
    gRowArray = [];
    var i;
    for (i = 1; i <= gRowNum; i++) {
        CreateOneRow ('#tbodyG_1', 1, i);
        CreateOneRow ('#tbodyG_2', 2, i);
        CreateOneRow ('#tbodyG_3', 3, i);
        CreateOneRow ('#tbodyG_4', 4, i);
        CreateOneRow ('#tbodyG_5', 5, i);
    }
    if (($('#txtFacultyID').val() == '07') && ($('#txtDepartmentID').val() == '01')) {
        $('#trRowNo_5').show();
        $('#tbodyG_5').show();
        $('#trPRRowNo_5').show();
    } else {
        $('#trRowNo_5').hide();
        $('#tbodyG_5').hide();
        $('#trPRRowNo_5').hide();
    }
    ResetEvent();

    //$('.normal').autosize();
}
function CreateOneRow(tbody, no, i) {
    CreateOneRow2P(tbody, no, i, i, g_evaluate_plan);
}

function CreateOneRow2P(tbody, no, i, order, plan) {

    if (plan) {
        string_disable = '';
    } else {
        string_disable = 'disabled';
    }
    var row = "<tr class='trPARowData' id='trPARow_" + no + "_" + i + "'>" +
                "<td class='tdPANo'><label id='lblPANo_" + no + "_" + i + "'>" + no + "." + i + "</label></td>" +
                "<td class='tdPADetail'>" +
                    "<div class='inputWrapper'>" +
                    "<label>" +
                        "<textarea class='InputPADetail normal'  id='txtPADetail_" + no + "_" + i + "' name='txtPADetail_" + no + "_" + i + "' " + string_disable + "></textarea>" +
                    "</label>" +
                    "</div>" +
                "    <div class='hidden'>" +
                "        <textarea  class='InputPADetail normal' id='txtPADetail2_" + no + "_" + + i + "' name='txtPADetail2_" + no + "_" + + i + "'></textarea>" +
                "    </div>" +                       
                "</td>" +
                "<td class='tdPADetail'><button id='btnDetail_" + no + "_" + i + "' class='btnDetail'>รายละเอียด</button></td>" +
                "<td class='tdPAWeight'>" +
                    "<div class='inputWrapper'>" +
                    "<label>" +
                        "<input class='InputPAWeight' type='text' id='txtPAWeight_" + no + "_" + i + "' name='txtPAWeight_" + no + "_" + i + "' " + string_disable + "/>" +
                    "</label>" +
                    "</div>" +
                "</td>";
        if (plan) {
        row +=
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_08_" + no + "_" + i + "' name='chkPAMonth_08_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_09_" + no + "_" + i + "' name='chkPAMonth_09_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_10_" + no + "_" + i + "' name='chkPAMonth_10_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_11_" + no + "_" + i + "' name='chkPAMonth_11_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_12_" + no + "_" + i + "' name='chkPAMonth_12_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_01_" + no + "_" + i + "' name='chkPAMonth_01_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_02_" + no + "_" + i + "' name='chkPAMonth_02_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_03_" + no + "_" + i + "' name='chkPAMonth_03_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_04_" + no + "_" + i + "' name='chkPAMonth_04_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_05_" + no + "_" + i + "' name='chkPAMonth_05_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_06_" + no + "_" + i + "' name='chkPAMonth_06_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAMonth'><input class='InputPAMonth' type='checkbox' id='chkPAMonth_07_" + no + "_" + i + "' name='chkPAMonth_07_" + no + "_" + i + "' value='1' /></td>" +
                "<td class='tdPAKPI'>" +
                    "<div class='inputWrapper'>" +
                    "<label>" +
                        "<textarea  class='InputPAKPI normal' id='txtPAKPI_" + no + "_" + i + "' name='txtPAKPI_" + no + "_" + i + "'></textarea>" +
                    "</label>" +
                    "</div>" +
                "</td>";
        } else {
        row +=
                "<td class='tdPALoS'>" +
                    "<select class='selPALoS' id='selPALoS_" + no + "_" + i + "'>" +
                        "<option value='0.00'>-</option>";
            for (var los = 4.0; los >= 0.0; los -= 0.1) {
                los = los.toFixed(1);
                row += "<option value='" + los + "'>" + los + "</option>";
            }

        row +=  "</select>" +
                "</td>" +
                "<td class='tdPALoS'>" +
                    "<select class='selPALoSBoss' id='selPALoSBoss_" + no + "_" + i + "'>" +
                        "<option value='0.00'>-</option>";
            for (var los = 4.0; los >= 0.0; los -= 0.1) {
                los = los.toFixed(1);
                row += "<option value='" + los + "'>" + los + "</option>";
            }
        row +=  "</select>" +
                "</td>" +
                "<td class='tdPAScore'><label class='thPAScore' id='lblPAScore_" + no + "_" + i + "' name='lblPAScore_" + no + "_" + i + "' >0<label></label></td>" +
                "<td class='tdPAPercent'><label class='thPAPercent' id='lblPAPercent_" + no + "_" + i + "' name='lblPAPercent_" + no + "_" + i + "' >0<label></label></td>" +
                "<td class='tdPAComment'>" +
                "<div class='inputWrapper'>" +
                    "<label>" +
                        "<textarea class='InputPAComment2 normal' id='txtPAComment2_" + no + "_" + i + "' name='txtPAComment2_" + no + "_" + i + "'></textarea>" +
                    "</label>" +                                
                "</div>" +
                "</td>";
        }
        row +=
                "<td class='tdPASTG'>" +
                    "<div class='inputWrapper'>" +
                    "<label>" +
                        "<input class='InputPASTG' type='text' id='txtPASTG_" + no + "_" + i + "' name='txtPASTG_" + no + "_" + i + "' " + string_disable + " />" +
                    "</label>" +
                    "</div>" +
                "</td>";
        if (plan) {
                row += "<td class='tdPADelete'><button id='btnPADelete_" + no + "_" + i + "' class='btnPADelete'>ลบ</button></td>";
        }
        row +=
                "<td class='tdPAComment'>" +
                    "<div class='inputWrapper'>" +
                    "<label>" +
                        "<textarea class='InputPAComment normal' id='txtPAComment_" + no + "_" + i + "' name='txtPAComment_" + no + "_" + i + "'></textarea>" +
                    "</label>" +
                    "</div>" +
                "</td>" +
            "</tr>";
    var iorder = gRowArray.indexOf(parseInt(order));
    //console.log(i + '  ' + order + ' ' + iorder);
    if (i == order) {
        $(tbody).append(row);
        //gRowArray.insert(i - 1, i);
    } else {
        $(tbody + ' tr:nth-child(' + (iorder+1) + ')').before(row);
        //gRowArray.insert(iorder, i);
    }
    
    //console.log(gRowArray);
}

function UpdateCaluate() {
    //alert(this.id);
    var sum_no = [ 0, 0, 0, 0, 0 ];
    var total = 0;
    for (no = 1; no <= 5; no++) {
        var run = 1;
        for (id = 1; id <= gRowNumArray[no-1]; id++) {
            //$('#txtPAWeight_' + no + '_' + id).val(id);
            var value = $('#txtPAWeight_' + no + '_' + id).val();
            if (value !== undefined) { $('#lblPANo_' + no + '_' + id).text(no + '.' + run++); }
            sum_no[no-1] += (parseInt(value) || 0);
            //console.log('no:' + no + ' id:' + id + ' val:' + value + ' sum:' + sum_no[no-1]);
        }
        $('#lblPASumWeight_' + no).text(sum_no[no-1]);
        total += sum_no[no-1];
    }
    $('#lblPASumWeight').text(total);
    if (total == 100) {
        $('#lblPASumWeight').css('color', 'green');
    } else {    
        $('#lblPASumWeight').css('color', 'red');
    }

    return false;
}

function UpdateCaluateResult() {
    //alert(this.id);
    var use_owner = true;
    for (item_group_no = 1; item_group_no <= 5; item_group_no++) {
        for (id = 1; id <= gRowNumArray[item_group_no-1]; id++) {
            if ($('#selPALoSBoss_' + item_group_no + '_' + id).val() != '0') {
                use_owner = false;
            }
        }
    }

    var sum1_no = [ 0, 0, 0, 0, 0 ];
    var sum2_no = [ 0, 0, 0, 0, 0 ];
    var total1 = 0;
    var total2 = 0;
    for (no = 1; no <= 5; no++) {
        for (id = 1; id <= gRowNumArray[no-1]; id++) {
            var w_value = $('#txtPAWeight_' + no + '_' + id).val();
            var value = $('#selPALoS_' + no + '_' + id).val();
            if (!use_owner) { value = $('#selPALoSBoss_' + no + '_' + id).val(); }
            var C = value * w_value;
            $('#lblPAScore_' + no + '_' + id).text(C);
            $('#lblPAPercent_' + no + '_' + id).text((C/4).toFixed(2));

            sum1_no[no-1] += (parseInt(C) || 0);
            sum2_no[no-1] += (parseFloat(C/4) || 0);
        }
        total1 += sum1_no[no-1];
        total2 += sum2_no[no-1];
    }

    $('#lblPASumScore').text(total1);
    $('#lblPASumPercent').text(total2.toFixed(2));

    return false;
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

function txtEnable(obj) {
    obj.removeAttr("disabled");
}

function txtDisable(obj) {
    obj.attr('disabled', 'disabled');
}

function isOwner() {
    return ($('#lblLoginName').text() == $('#txtUserName').val());
}

Array.prototype.insert = function(index) {
    index = Math.min(index, this.length);
    arguments.length > 1
        && this.splice.apply(this, [index, 0].concat([].pop.call(arguments)))
        && this.insert.apply(this, arguments);
    return this;
};

Array.prototype.remove = function(from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

/* Format input to display number 2 floating point Ex 1,000.00 */
function formatNumber (num) {
    if (num === '') return '';
    num = parseFloat(num); 
    return num.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}

/* Check n is integer */
function isInt(n){
    return Number(n) === n && n % 1 === 0;
}

/* Check n is floating point */
function isFloat(n){
    return Number(n) === n && n % 1 !== 0;
}

/* Pattern for check input is Date format DD/MM/YYYY */
var dateRegex = /^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-.\/])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$/;
//var numberRegex = /^-?\d+\.?\d*$/;

/* Pattern for check input is Money format 1,000.00 */
var numberRegex = /^-?\d*\.?\d*$/