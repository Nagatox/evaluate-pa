
var bConfirmed = false;
var bOwner = true;
var bBossConfirmed = false;
var iRevision = 0;

var gRowNum = 3;
var gRowNumArray = [3, 3, 3, 3, 3];
var gRowArray = [1, 2, 3, 4, 5];

var gcontextmenuid = 0;

var g_evaluate_plan = true;

$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    CreateFormBody();
    UpdateCaluate();

    bOwner = isOwner();
    bConfirmed = false;

    //Set2Owner(bOwner, bConfirmed);

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
});

$('#btnConfirmPA').click(function() {
    if (confirm('ท่านมีความประสงค์จะส่งและยืนยันข้อมูลของ โดยไม่มีการปรับแก้ไขข้อมูลแล้ว') == true) {
        if (DoSaveAndConfirm('SaveConfirmPAForm', 'การยืนยัน')) {
            // lock screen
            var username = $('#txtUserName').val();
            //LoadEvaluateHeaderR0(username);
            //LoadEvaluateHeader(username, iRevision);
            //LoadEvaluateDetail(username, iRevision);
        }

        return false;
    }
    return false;
});

$('#btnSavePA').click(function() {

    DoSaveAndConfirm('SavePAForm', 'การบันทึก');

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
    //data.suggest = $('#txtSuggest').val();

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
                detail.evaluate_year = evaluate_year;
                detail.round = round;

                detail.item_group_no = item_group_no;
                detail.item_no = new_id++;
                detail.detail = $('#txtPADetail_' + item_group_no + '_' + id).val();
                detail.detail2 = $('#txtPADetail2_' + item_group_no + '_' + id).val();
                detail.weight = $('#txtPAWeight_' + item_group_no + '_' + id).val();
                detail.month_08 = ($('#chkPAMonth_08_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_09 = ($('#chkPAMonth_09_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_10 = ($('#chkPAMonth_10_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_11 = ($('#chkPAMonth_11_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_12 = ($('#chkPAMonth_12_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_01 = ($('#chkPAMonth_01_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_02 = ($('#chkPAMonth_02_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_03 = ($('#chkPAMonth_03_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_04 = ($('#chkPAMonth_04_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_05 = ($('#chkPAMonth_05_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_06 = ($('#chkPAMonth_06_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.month_07 = ($('#chkPAMonth_07_' + item_group_no + '_' + id).is(':checked')) ? 1 : 0;
                detail.KPI = $('#txtPAKPI_' + item_group_no + '_' + id).val();
                detail.STG = $('#txtPASTG_' + item_group_no + '_' + id).val();
                detail.comment = $('#txtPAComment_' + item_group_no + '_' + id).val();
            
                data.detail[count++] = detail;
            }
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

$('#btnBossSave').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();
    
    var data = new tblPAHeader();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.suggest = $('#txtSuggest').val();
    
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

                detail.comment = $('#txtPAComment_' + item_group_no + '_' + id).val();
            
                data.detail[count++] = detail;
            }
        }
    }
    data.recordCount = count;

    var json = $.toJSON(data);

    $.ajax({
        url: 'frmPAService.aspx?q=BossSave&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
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

$('#btnBossUnlock').click(function() {
    if (confirm('ท่านมีความปลดล็อกข้อมูลของผู้ประเมิน') == true) {
        var username = $('#txtUserName').val();
        var evaluate_year = $('#lblBudgetYear').text();
        if (evaluate_year < 2500) { evaluate_year += 543; }
        var round = $('#txtRoundNo').val();
        BossUnlock (username, evaluate_year, round);
    }
    return false;
});

function BossUnlock (username, evaluate_year, round) {
    $.ajax({
        url: 'frmPAService.aspx?q=BossUnlock&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
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
                LoadPAHeader(username, 1);
            }
            else {
                alert('การปลดล็อคข้อมูลของผู้ประเมินล้มเหลว:' + json.ErrMessage);
            }
        }
    });
}

$('#btnBossConfirm').click(function() {
    if (confirm('ท่านมีความประสงค์จะยืนยันการรับทราบข้อมูลของผู้ประเมิน') == true) {
        var username = $('#txtUserName').val();
        var evaluate_year = $('#lblBudgetYear').text();
        if (evaluate_year < 2500) { evaluate_year += 543; }
        var round = $('#txtRoundNo').val();

        BossConfirm (username, evaluate_year, round);
    }
    return false;
});

function BossConfirm (username, evaluate_year, round) {
    $.ajax({
        url: 'frmPAService.aspx?q=BossConfirm&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(), /// change_yyy ///
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
                bBossConfirmed = true;
                Set2Owner(bOwner, bConfirmed, bBossConfirmed);
                LoadPAHeader(username, 1);
            }
            else {
                alert('ยืนยันรับทราบข้อมูลของผู้ประเมินล้มเหลว:' + json.ErrMessage);
            }
        }
    });
}

$('.btnAddRow').click(function() {
    var id = this.id.split('_')[1];
    CreateOneRow('#tbodyG_' + id, id, ++gRowNumArray[id-1]);
    ResetEvent();

    $('.normal').autosize();

    UpdateCaluate();

    return false;
});

$('.btnPADelete').live('click', function() {
    //alert(this.id);
    var aid = this.id.split('_');
    var no = aid[1];
    var id = aid[2];
    //alert(id);

    DeleteRow(no, id);

    return false;
});

function DeleteRow(no, id) {

    console.log('#trPARow_' + no + '_' + id);
    $('#trPARow_' + no + '_' + id).remove();

    //gRowNumArray[no-1]--;
    //gRowArray.remove(gRowArray.indexOf(parseInt(id)));
    //console.log(gRowArray);

    UpdateCaluate();

    return false;
}

function Set2Owner(Owner, Confirmed, BossConfirmed) {

    if (Owner & !Confirmed) {
        OwnerEditable(true);
        BossEditable(false);

        btnShow($('#btnSavePA'));
        btnShow($('#btnConfirmPA'));
        //btnDisable($('#btnExport')); 
        btnHide($('#btnExport'));
        btnDisable($('#btnPrintImage'));

        $("#btnBossSave").hide();
        $("#btnBossUnlock").hide();
        $("#btnBossConfirm").hide();
        
    } else if (Owner & Confirmed) {
        OwnerEditable(false);
        BossEditable(false);

        btnDisable($('#btnSavePA'));
        btnDisable($('#btnConfirmPA'));
        //btnShow($('#btnExport'));
        btnHide($('#btnExport'));
        btnShow($('#btnPrintImage'));

        $("#btnBossSave").hide();
        $("#btnBossUnlock").hide();
        $("#btnBossConfirm").hide();

        $('#btnPOPUpdate').hide();
    } else if (!Owner & !Confirmed) {
        OwnerEditable(false);
        BossEditable(false);

        $('#btnSavePA').hide();
        $('#btnConfirmPA').hide();
        btnHide($('#btnExport'));
        btnHide($('#btnPrintImage'));        

        btnShow($("#btnBossSave"));
        btnDisable($("#btnBossUnlock"));
        btnDisable($("#btnBossConfirm"));

        $('#btnPOPUpdate').hide();

        $('#lblMessage').text('- ผู้ประเมินยังไม่ได้ยืนยันข้อมูล');
    } else if (!Owner & Confirmed & !BossConfirmed) {
        OwnerEditable(false);
        BossEditable(true);

        $('#btnSavePA').hide();
        $('#btnConfirmPA').hide();
        btnHide($('#btnExport'));
        btnHide($('#btnPrintImage'));        

        btnShow($("#btnBossSave"));
        btnShow($('#btnBossUnlock'));
        btnShow($('#btnBossConfirm'));  

        $('#btnPOPUpdate').hide();
    } else if (!Owner & Confirmed & BossConfirmed) {
        OwnerEditable(false);
        BossEditable(true);

        $('#btnSavePA').hide();
        $('#btnConfirmPA').hide();
        btnHide($('#btnExport'));
        btnHide($('#btnPrintImage'));        

        btnShow($("#btnBossSave"));
        btnDisable($('#btnBossUnlock'));
        btnDisable($('#btnBossConfirm'));        

        $('#btnPOPUpdate').hide();
    }
}

function BossEditable(bEditable) {
    console.log('BossEditable: ' + bEditable);
    if (bEditable) {
        $('.InputPAComment').removeAttr("disabled");
        $('.InputPASuggest').removeAttr("disabled");
    } else {
        $('.InputPAComment').attr('disabled', 'disabled');
        $('.InputPASuggest').attr('disabled', 'disabled');
    }
}

function OwnerEditable(bEditable) {
    if (bEditable) {
        $('.InputPADetail').removeAttr("disabled");
        $('.InputPAWeight').removeAttr("disabled");
        $('.InputPAMonth').removeAttr("disabled");
        $('.InputPAKPI').removeAttr("disabled");
        $('.InputPASTG').removeAttr("disabled");

        $('.btnPADelete').removeAttr("disabled");
        $('.btnAddRow').removeAttr("disabled");
    } else {
        $('.InputPADetail').attr('disabled', 'disabled');
        $('.InputPAWeight').attr('disabled', 'disabled');
        $('.InputPAMonth').attr('disabled', 'disabled');
        $('.InputPAKPI').attr('disabled', 'disabled');
        $('.InputPASTG').attr('disabled', 'disabled');
        
        $('.btnPADelete').attr('disabled', 'disabled');
        $('.btnAddRow').attr('disabled', 'disabled');
    }    
}


