
var bResultConfirmed = false;
var bOwner = true;
var bBossConfirmed = false;
var iRevision = 0;

var gRowNum = 3;
var gRowNumArray = [3, 3, 3, 3, 3];
var gRowArray = [1, 2, 3, 4, 5];

var gcontextmenuid = 0;

var g_evaluate_plan = false;

$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    CreateFormBody();
    UpdateCaluate();
    UpdateCaluateResult();

    bOwner = isOwner();
    bResultConfirmed = false;

    //Set2Owner(bOwner, bResultConfirmed);

    var username = $('#txtUserName').val();
    var sresult = '&result=' + iRevision;
    if (iRevision == 0) sresult = "";
    $('#linkForm00').attr('href', 'frmEvaluatePart00.aspx?u=' + username + sresult);
    $('#linkFormPA').attr('href', 'frmFormPA.aspx?u=' + username + sresult);
    $('#linkForm01').attr('href', 'frmEvaluateFormPA.aspx?u=' + username + sresult);
    $('#linkForm02').attr('href', 'frmEvaluatePart02.aspx?u=' + username + sresult);
    $('#linkForm03').attr('href', 'frmEvaluatePart03.aspx?u=' + username + sresult);

    LoadPAHeader(username, iRevision);
    //LoadPADetail(username, iRevision);
    //LoadPADetailResult(username, iRevision);
});

$('.selPALoS').live('change', function() {
    var aid = this.id.split('_');
    var no = aid[1];
    var id = aid[2];
    var w_value = $('#txtPAWeight_' + no + '_' + id).val();
    var value = $('#selPALoS_' + no + '_' + id).val();
    var C = value * w_value;
    $('#lblPAScore_' + no + '_' + id).text(C);
    $('#lblPAPercent_' + no + '_' + id).text((C/4).toFixed(2));
    UpdateCaluateResult();
});

$('.selPALoSBoss').live('change', function() {
    var aid = this.id.split('_');
    var no = aid[1];
    var id = aid[2];
    var w_value = $('#txtPAWeight_' + no + '_' + id).val();
    var value = $('#selPALoSBoss_' + no + '_' + id).val();
    var C = value * w_value;
    $('#lblPAScore_' + no + '_' + id).text(C);
    $('#lblPAPercent_' + no + '_' + id).text((C/4).toFixed(2));
    UpdateCaluateResult();
});

$('#btnConfirmPAResult').click(function() {
    if (confirm('ท่านมีความประสงค์จะส่งและยืนยันข้อมูลของ โดยไม่มีการปรับแก้ไขข้อมูลแล้ว') == true) {
        DoSaveAndConfirm('SaveConfirmPAResult', 'การยืนยัน');

        return false;
    }
    return false;
});

$('#btnSavePAResult').click(function() {

    DoSaveAndConfirm('SavePAResult', 'การบันทึก');

    return false;
});

function DoSaveAndConfirm(service_name, prefix_msg) {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();

    var data = new tblPAHeader();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;

    data.suggest = $('#txtSuggest').val();
    data.sum_score = $('#lblPASumScore').text();
    data.sum_percent = $('#lblPASumPercent').text();

    var count = 0;
    var i;
    for (item_group_no = 1; item_group_no <= 5; item_group_no++) {
        //var index = i + 1;
        for (id = 1; id <= gRowNumArray[item_group_no-1]; id++) {

            var detail = new tblPADetailResult();
            detail.username = username;
            data.evaluate_year = evaluate_year;
            data.round = round;

            detail.item_group_no = item_group_no;
            detail.item_no = id;
            detail.LoS = $('#selPALoS_' + item_group_no + '_' + id).val();
            detail.LoSBoss = $('#selPALoSBoss_' + item_group_no + '_' + id).val();
            detail.comment2 = $('#txtPAComment2_' + item_group_no + '_' + id).val();
        
            data.detail[count++] = detail;
        }
    }
    data.recordCount = count;

    var json = $.toJSON(data);
    //json = json.replace(/&/g, ' ');

    $.ajax({
        url: 'frmPAService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(),  ///
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
                alert(prefix_msg + "สำเร็จ");
                LoadPAHeader(username, 0);
            }
            else {
                alert(prefix_msg + "ล้มเหลว:" + json.ErrMessage);
            }
        }
    });
    return false;
}

$('#btnBossSaveResult').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();

    var data = new tblPAHeader();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;

    data.suggest = $('#txtSuggest').val();
    data.sum_score = $('#lblPASumScore').text();
    data.sum_percent = $('#lblPASumPercent').text();

    var count = 0;
    var i;
    for (item_group_no = 1; item_group_no <= 5; item_group_no++) {
        //var index = i + 1;
        var new_id = 1;
        for (id = 1; id <= gRowNumArray[item_group_no-1]; id++) {

            //if ($('#txtPADetail_' + item_group_no + '_' + id).val() != null) {
            if ($('#txtPAWeight_' + item_group_no + '_' + id).val() !== undefined) {
        
                var detail = new tblPADetail();
                detail.username = username;
                data.evaluate_year = evaluate_year;
                data.round = round;

                detail.item_group_no = item_group_no;
                detail.item_no = new_id++;

                detail.LoSBoss = $('#selPALoSBoss_' + item_group_no + '_' + id).val();  
                detail.comment = $('#txtPAComment_' + item_group_no + '_' + id).val();
            
                data.detail[count++] = detail;
            }
        }
    }
    data.recordCount = count;

    var json = $.toJSON(data);

    $.ajax({
        url: 'frmPAService.aspx?q=BossSaveResult&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
        type: 'POST',
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
            if (!json.isError) {
                alert('บันทึกสำเร็จ');
            }
            else {
                alert('บันทึกล้มเหลว:' + json.ErrMessage);
            }
        }
    });

    return false;
});

$('#btnBossUnlockResult').click(function() {
    if (confirm('ท่านมีความปลดล็อกข้อมูลของผู้ประเมิน') == true) {
        var username = $('#txtUserName').val();
        var evaluate_year = $('#lblBudgetYear').text();
        if (evaluate_year < 2500) { evaluate_year += 543; }
        var round = $('#txtRoundNo').val();
        BossUnlockResult (username, evaluate_year, round);
    }
    return false;
});

function BossUnlockResult (username, evaluate_year, round) {
    $.ajax({
        url: 'frmPAService.aspx?q=BossUnlockResult&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
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
                alert('การปลดล็อคข้อมูลของผู้ประเมินสำเร็จ');
                Set2Owner(bOwner, false, false);
            }
            else {
                alert('การปลดล็อคข้อมูลของผู้ประเมินล้มเหลว:' + json.ErrMessage);
            }
        }
    });
}

$('#btnBossConfirmResult').click(function() {
    if (confirm('ท่านมีความประสงค์จะยืนยันการรับทราบข้อมูลของผู้ประเมิน') == true) {
        var username = $('#txtUserName').val();
        var evaluate_year = $('#lblBudgetYear').text();
        if (evaluate_year < 2500) { evaluate_year += 543; }
        var round = $('#txtRoundNo').val();
        BossConfirmResult (username, evaluate_year, round);
    }
    return false;
});

function BossConfirmResult (username, evaluate_year, round) {
    $.ajax({
        url: 'frmPAService.aspx?q=BossConfirmResult&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
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
                alert('ยืนยันรับทราบข้อมูลของผู้ประเมินสำเร็จ');
                Set2Owner(bOwner, bConfirmed, false);
            }
            else {
                alert('ยืนยันรับทราบข้อมูลของผู้ประเมินล้มเหลว:' + json.ErrMessage);
            }
        }
    });
}

function Set2Owner(Owner, Confirmed, BossConfirmed) {
    //console.log('Set2Owner Result: ' + Owner + ', ' + Confirmed);
    if (Owner & !Confirmed) {
        OwnerEditable(true);
        BossEditable(false);

        btnShow($('#btnSavePAResult'));
        btnShow($('#btnConfirmPAResult'));

        btnHide($("#btnBossSaveResult"));
        btnHide($("#btnBossUnlockResult"));
        btnHide($("#btnBossConfirmResult"));
        
        $('#btnPOPUpdate').hide();
    } else if (Owner & Confirmed) {
        OwnerEditable(false);
        BossEditable(false);

        btnDisable($('#btnSavePAResult'));
        btnDisable($('#btnConfirmPAResult'));

        btnHide($("#btnBossSaveResult"));
        btnHide($("#btnBossUnlockResult"));
        btnHide($("#btnBossConfirmResult"));

        $('#btnPOPUpdate').hide();
    } else if (!Owner & !Confirmed) {
        OwnerEditable(false);
        BossEditable(false);

        btnHide($('#btnSavePAResult'));
        btnHide($('#btnConfirmPAResult'));
        btnHide($('#btnExportResult'));
        btnHide($('#btnPrintImageResult'));

        btnShow($("#btnBossSaveResult"));
        btnDisable($("#btnBossUnlockResult"));
        btnDisable($("#btnBossConfirmResult"));

        $('#btnPOPUpdate').hide();

        $('#lblMessage').text('- ผู้ประเมินยังไม่ได้ยืนยันข้อมูล');
    } else if (!Owner & Confirmed) {
        OwnerEditable(false);
        BossEditable(true);

        btnHide($('#btnSavePAResult'));
        btnHide($('#btnConfirmPAResult'));
        btnHide($('#btnExportResult'));
        btnHide($('#btnPrintImageResult'));

        btnShow($("#btnBossSaveResult"));
        btnShow($('#btnBossUnlockResult'));
        btnShow($('#btnBossConfirmResult'));
        
        $('#btnPOPUpdate').hide();
    }
}

function BossEditable(bEditable) {
    //console.log('BossEditable: ' + bEditable);
    if (bEditable) {
        $('.selPALoSBoss').removeAttr("disabled");
        $('.InputPAComment').removeAttr("disabled");
        $('.InputPASuggest').removeAttr("disabled");
    } else {
        $('.selPALoSBoss').attr('disabled', 'disabled');
        $('.InputPAComment').attr('disabled', 'disabled');
        $('.InputPASuggest').attr('disabled', 'disabled');
    }
}

function OwnerEditable(bEditable) {
    //console.log('OwnerEditable: ' + bEditable);
    if (bEditable) {
        $('.selPALoS').removeAttr("disabled");
        $('.InputPAComment2').removeAttr("disabled");
    } else {
        $('.selPALoS').attr('disabled', 'disabled');
        $('.InputPAComment2').attr('disabled', 'disabled');
    }    
}

