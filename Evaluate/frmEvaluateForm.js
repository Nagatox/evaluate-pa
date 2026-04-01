
var bConfirmed = false;
var bOwner = true;
var iRevision = 0;
var giffull = 70;
var gRowNum = 5;
var gRowArray = [1, 2, 3, 4, 5];
var gbEditable = true;

var gcontextmenuid = 0;

$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    /*console.log(gRowArray);
    gRowArray.insert(2, 6);
    console.log(gRowArray);
    gRowArray.insert(6, 7);
    console.log(gRowArray);
    gRowArray.insert(6, 8);
    console.log(gRowArray);
    gRowArray.insert(0, 9);
    console.log(gRowArray);*/

    //    $('#lblEmployeeID').text('059/2554');
    //    $('#lblFullName').text('เอกชัย วิวรรธนาภิรักษ์');
    //    $('#lblPositionName').text('นักคอมพิวเตอร์');
    //    $('#lblRoundNo').text('1/2');
    //    $('#lblLevelNo').text('ช');
    //    $('#lblDepartmentName').text('วิศวกรรมคอมพิวเตอร์');

    bOwner = isOwner();
    
    //if ($('#lblGroupName').text() == 'Head of Department') {
    if (!bOwner) {
        $('#txtSuggestion').removeAttr("disabled");
    } else {
        $('#txtSuggestion').attr('disabled', 'disabled');
    }

    UpdateLevelMessage();

    CreateFormBody();
    UpdateCaluate();

    $('#lblRoundYear').text($('#lblBudgetYear').text());

    // $('#lblLevelNo').text() set value from vb code
    var i;
    for (i = 0; i < ConfigLevel.array.length; i++) {
        if (ConfigLevel.array[i].level == $('#lblLevelNo').text()) {
            giffull = ConfigLevel.array[i].percent;
        }
    }

    bConfirmed = false;
    if ($('#txtResult').val() != '1') {
        iRevision = 0;

        //$('#btnSave').show();
        //$('#btnConfirm').show();

        //$('#drpRoundNo').removeAttr("disabled");
    }
    else {
        iRevision = 1;
        //$('#btnSave').hide();
        //$('#btnConfirm').hide();

        //$('#drpRoundNo').attr('disabled', 'disabled');
    }

    $('#drpResult').val(iRevision);

    Set2Owner(bOwner, bConfirmed, iRevision);

    var username = $('#txtUserName').val();
    var sresult = '&result=' + iRevision;
    if (iRevision == 0) sresult = "";
    $('#linkForm00').attr('href', 'frmEvaluatePart00.aspx?u=' + username + sresult);
    $('#linkForm01').attr('href', 'frmEvaluateForm.aspx?u=' + username + sresult);
    $('#linkForm02').attr('href', 'frmEvaluatePart02.aspx?u=' + username + sresult);
    $('#linkForm03').attr('href', 'frmEvaluatePart03.aspx?u=' + username + sresult);

    LoadEvaluateHeaderR0(username);
    LoadEvaluateHeader(username, iRevision);
    LoadEvaluateDetail(username, iRevision);

    CreateContextMenu();
});

function CreateContextMenu() {
    //disable_menu = ["More", "Insert"];
    var option = { width: 150, items: [
                            { text: "แทรก", icon: "images/plus.gif", alias: "Insert", action: menuAction },
                            { text: "ลบ", icon: "images/minus.gif", alias: "Delete", action: menuAction },
                            { type: "splitLine" },
                            { text: "อื่นๆ", icon: "sample-css/wi0009-16.gif", alias: "More", type: "group", width: 170, items: [
	                            { text: "Group Three", icon: "sample-css/wi0054-16.gif", alias: "2-2", type: "group", width: 190, items: [
		                            { text: "Group3 Item One", icon: "sample-css/wi0062-16.gif", alias: "3-1", action: menuAction },
		                            { text: "Group3 Item Tow", icon: "sample-css/wi0063-16.gif", alias: "3-2", action: menuAction }
	                            ]
	                            },
	                            { text: "Group Two Item1", icon: "sample-css/wi0096-16.gif", alias: "2-1", action: menuAction },
	                            { text: "Group Two Item1", icon: "sample-css/wi0111-16.gif", alias: "2-3", action: menuAction },
	                            { text: "Group Two Item1", icon: "sample-css/wi0122-16.gif", alias: "2-4", action: menuAction }
                            ]
                            },
                            ], onShow: applyrule,
        onContextMenu: BeforeContextMenu
    };
    function menuAction() {
        //alert(this.data.alias);
        //alert(gcontextmenuid);
        var aid = gcontextmenuid.split('_');
        var id = aid[1];
        //alert(id);

        if (this.data.alias == 'Insert') {
            InsertRow(id);
        }
        else if (this.data.alias == 'Delete') {
            DeleteRow(id);
        }
    }
    function applyrule(menu) {
        var disable_menu = ["More"];
        if (!gbEditable) {
            disable_menu = ["More", "Insert", "Delete"];
        }
        //if (this.id == "อื่นๆ") {
            menu.applyrule({ name: "More",
                disable: true,
                items: disable_menu
            });
        /*}
        else {
            menu.applyrule({ name: "all",
                disable: true,
                items: []
            });
        }*/
    }
    function BeforeContextMenu() {
        //alert('1234');
        //return this.id != "target3";
        gcontextmenuid = this.id;
        return true;
    }
    $(".contextmenu").contextmenu(option);
}

$('#btnPrintImage').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    var page = 'EvaluateForm';
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

// don't have event to this function because unbind select
$("#drpResult").change(function() {
    //alert('1234');
    //window.location = "frmEvaluateForm.aspx?result=" + $('#drpResult').val();
    //return true;
});


$('#btnLoadBackData').click(function() {
    //alert($('#drpBudgetYear').val());

    var year_round = $('#drpBudgetYear').val();
    var aa = year_round.split('/');
    var year = aa[1];
    var round = aa[0];

    var username = $('#txtUserName').val();
    LoadEvaluateHeader01(username, iRevision, year, round);
    LoadEvaluateDetail01(username, iRevision, year, round);

    return false;
});

function drpResult_change() {
    if (iRevision == 0) {
        window.location = "frmEvaluateForm.aspx?u=" + $('#txtUserName').val() + "&result=1";
    }
    else {
        window.location = "frmEvaluateForm.aspx?u=" + $('#txtUserName').val();
    }
};

// don't have event to this function because unbind select
/*$("#drpRoundNo").change(function() {
    drpRoundNo_change();
});*/

function drpRoundNo_change() 
{
    var username = $('#txtUserName').val();
    LoadEvaluateHeaderR0(username);
    LoadEvaluateHeader(username, iRevision);
    LoadEvaluateDetail(username, iRevision);

};

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

    this.SumWeight = '';
    this.SumPL = '';
    this.SumSuccess = '';
    this.FullSuccess = '';
    this.PercentSuccess = '';
    this.IFFull = '';
    this.IFSuccess = '';
    
    this.commander_recomment = '';

    this.detail = new Array();
    this.recordCount = 0;
    
    this.isError = false;
    this.ErrMessage = '';
}

function tblEvaluateDetail() {
    this.id = 'tblEvaluateDetail';
    this.username = '';
    this.evaluate_year = '';
    this.round = '';
    this.activate_no = '';

    this.activate_name = '';
    this.activate_detail = '';
    this.weight = '';
    this.excepted_value = '';
    this.evaluate_value = '';
    this.pl_score = '';
    this.success_level = '';
    this.success_score = '';
    this.total_score = '';

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
    var round = $('#lblRoundNo').text();
    var revision = iRevision;

    var service_name = 'ExportEvaluateForm';

    var url = 'frmEvaluateService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time();

    /*if (username == $('#lblUserName').val()) {
        DoSaveAndConfirm('SaveEvaluate', url);
    } else {
        window.location = url;
    }*/

    window.location = url;


    //open (url)

    /*$.ajax({
    url: 'frmEvaluateService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(),  ///
    type: 'POST',
    //data: 'data=' + json,
    //data: json,
    dataType: 'html',
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
    //alert(json.toString ());
    if (!json.isError) {
    alert("สำเร็จ");
    }
    else {
    alert("ล้มเหลว:" + json.ErrMessage);
    }
    }
    });*/
    return false;
});

$('#btnConfirm').click(function() {
    if (confirm('ท่านมีความประสงค์จะส่งและยืนยันข้อมูลของ โดยไม่มีการปรับแก้ไขข้อมูลแล้ว') == true) {
        if (DoSaveAndConfirm('SaveConfirmEvaluate', 'การยืนยัน')) {
            // lock screen
            var username = $('#txtUserName').val();
            LoadEvaluateHeaderR0(username);
            LoadEvaluateHeader(username, iRevision);
            LoadEvaluateDetail(username, iRevision);
        }

        return false;
    }
    return false;
});

$('#btnSave').click(function() {

    DoSaveAndConfirm('SaveEvaluate', 'การบันทึก');

    return false;
});

function DoSaveAndConfirm(service_name, prefix_msg) {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    //var round = $('#drpRoundNo').val();
    var round = $('#lblRoundNo').text();
    var revision = iRevision;

    var data = new tblEvaluateHead();

    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;
    data.revision = revision;
    data.commander_recomment = $('#txtSuggestion').val();

    data.SumWeight = $('#lblSumWeight').text();
    data.SumPL = $('#lblSumPL').text();
    data.SumSuccess = $('#lblSumSuccess').text();
    data.FullSuccess = $('#lblFullSuccess').text();
    data.PercentSuccess = $('#lblPercentSuccess').text();
    data.IFFull = $('#lblIFFull').text();
    data.IFSuccess = $('#lblIFSuccess').text();

    data.sum_excepted_PL1 = $('#lblExpectPL1').text();
    data.sum_excepted_PL2 = $('#lblExpectPL2').text();
    data.sum_excepted_PL3 = $('#lblExpectPL3').text();
    data.sum_excepted_PL4 = $('#lblExpectPL4').text();
    data.sum_excepted_PL5 = $('#lblExpectPL5').text();

    data.sum_evaluate_PL1 = $('#lblEvaluatePL1').text();
    data.sum_evaluate_PL2 = $('#lblEvaluatePL2').text();
    data.sum_evaluate_PL3 = $('#lblEvaluatePL3').text();
    data.sum_evaluate_PL4 = $('#lblEvaluatePL4').text();
    data.sum_evaluate_PL5 = $('#lblEvaluatePL5').text();

    var count = 0;
    var i;
    for (i = 0; i < gRowArray.length; i++) {
        //var index = i + 1;
        var index = gRowArray[i];
        if ($('#trRowNo_' + index).html() != "") {

            var detail = new tblEvaluateDetail();
            detail.username = username;
            detail.evaluate_year = evaluate_year;
            detail.round = round;
            detail.revision = revision;

            detail.activate_no = count + 1;
            detail.activate_name = $('#txtName_' + index).val();
            detail.activate_detail = $('#txtDetail_' + index).val();

            detail.weight = $('#txtWeight_' + index).val();
            detail.excepted_value = $('#selExpect_' + index).val();
            detail.evaluate_value = $('input:radio[name=radPL_' + index + ']:checked').val();
            detail.pl_score = $('#lblPLScore_' + index).text();
            detail.success_level = $('input:radio[name=radSuccess_' + index + ']:checked').val();
            detail.success_score = $('#txtSuccess_' + index).val();
            detail.total_score = $('#lblSuccessScore_' + index).text();

            data.detail[count++] = detail;
        }
    }
    data.recordCount = count;

    var json = $.toJSON(data);
    //json = json.replace(/&/g, ' ');

    $.ajax({
        url: 'frmEvaluateService.aspx?q=' + service_name + '&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + iRevision + '&time=' + time(),  ///
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
                if (prefix_msg.length > 20) {

                    //window.open(prefix_msg, '_blank');
                    window.location = prefix_msg;
                }
                else
                    alert(prefix_msg + "สำเร็จ");
            }
            else {
                alert(prefix_msg + "ล้มเหลว:" + json.ErrMessage);
            }
        }
    });
    return true;
}

function LoadEvaluateHeader(username, revision) {

    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();
    LoadEvaluateHeader01(username, revision, evaluate_year, round);
}

function LoadEvaluateHeader01(username, revision, evaluate_year, round) {
    //alert(username);
    //var json = $.toJSON(pageData);
    //var json = { username: username, evaluate_year: '2557', round: '2' };

    $('#txtSuggestion').val('');

    //var evaluate_year = $('#lblBudgetYear').text();
    ////var round = $('#drpRoundNo').val();
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
                $('#lblSumWeight').text(json.detail[0].SumWeight);
                $('#lblSumPL').text(json.detail[0].SumPL);
                $('#lblSumSuccess').text(json.detail[0].SumSuccess);
                $('#lblFullSuccess').text(json.detail[0].FullSuccess);
                $('#lblPercentSuccess').text(json.detail[0].PercentSuccess);
                $('#lblIFFull').text(json.detail[0].IFFull);
                $('#lblIFSuccess').text(json.detail[0].IFSuccess);

                $('#txtSuggestion').val(json.detail[0].commander_recomment);
                $('#txtConfirmed').val(json.detail[0].confirmed);
                $('#txtConfirmDatetime').val(json.detail[0].confirm_datetime);

                /*if (json.detail[0].confirmed == '1') {

                    bConfirmed = true;

                    $('#lblMessage').text('ข้อมูลทั้งหมดได้รับการยืนยันแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')')
                }
                else {
                bConfirmed = false;

                    $('#lblMessage').text("");
                }*/
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluateHeaderR0(username) {
    //alert(username);
    //var json = $.toJSON(pageData);
    //var json = { username: username, evaluate_year: '2557', round: '2' };

    $('#txtSuggestion').val('');

    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    //var round = $('#drpRoundNo').val();
    var round = $('#lblRoundNo').text();
    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadEvaluateHeader&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=0&time=' + time(), /// change_yyy ///
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

                if (json.detail[0].confirmed == '1') {

                    bConfirmed = true;

                    $('#lblMessage').text('ข้อมูลทั้งหมดได้รับการยืนยันแล้ว (เมื่อวันที่ ' + json.detail[0].confirm_datetime + ')')
                }
                else {
                bConfirmed = false;

                    $('#lblMessage').text("");
                }
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluateDetailx(username, revision) {
    //alert(username);
    //var json = $.toJSON(pageData);

    gRowNum = 5;
    CreateFormBody();
    
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    //var round = $('#drpRoundNo').val();
    var round = $('#lblRoundNo').text();
    $.ajax({
    url: 'frmEvaluateService.aspx?q=LoadEvaluateDetail&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time(), /// change_yyy ///
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
                if (json.recordCount > 5) {
                    for (i = 5; i < json.recordCount; i++) {
                        CreateOneRow(++gRowNum);
                    }
                }
                for (i = 0; i < json.recordCount; i++) {
                    //alert (json.detail[i].activate_detail);
                    var index = i + 1;
                    $('#txtName_' + index).val(json.detail[i].activate_name);
                    $('#txtDetail_' + index).val(json.detail[i].activate_detail);
                    $('#txtWeight_' + index).val(json.detail[i].weight);
                    $('#selExpect_' + index).val(json.detail[i].excepted_value);
                    $('input:radio[name=radPL_' + index + '][value=' + json.detail[i].evaluate_value + ']').attr('checked', true);
                    //$('input:radio[name=radSuccess_' + index + '][value=' + json.detail[i].success_level + ']').attr('checked', true);
                    if (json.detail[i].success_level != '0') {
                        $('input:radio[name=radSuccess_' + index + '][value=' + json.detail[i].success_level + ']').attr('checked', true);
                        var e = parseInt(json.detail[i].success_level);
                        e = 0.5 * (e + 1);
                        $('#txtSuccess_' + index).val(e);
                    } else {
                        if (json.detail[i].success_score != '0.00') { $('#txtSuccess_' + index).val(json.detail[i].success_score); }
                    }
                }

                ResetEvent();
                UpdateCaluate();
                $('.normal').autosize();
                Set2Owner(bOwner, bConfirmed, iRevision);
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function LoadEvaluateDetail(username, revision) {
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#lblRoundNo').text();

    LoadEvaluateDetail01(username, revision, evaluate_year, round);
}

function LoadEvaluateDetail01(username, revision, evaluate_year, round) {
    //alert(username);
    //var json = $.toJSON(pageData);

    gRowNum = 5;
    CreateFormBody();

    //var evaluate_year = $('#lblBudgetYear').text();
    ////var round = $('#drpRoundNo').val();
    //var round = $('#lblRoundNo').text();
    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadEvaluateDetail&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&revision=' + revision + '&time=' + time(), /// change_yyy ///
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
                if (json.recordCount > 5) {
                    for (i = 5; i < json.recordCount; i++) {
                        CreateOneRow(++gRowNum);
                    }
                }
                for (i = 0; i < json.recordCount; i++) {
                    //alert (json.detail[i].activate_detail);
                    var index = i + 1;
                    $('#txtName_' + index).val(json.detail[i].activate_name);
                    $('#txtDetail_' + index).val(json.detail[i].activate_detail);
                    $('#txtWeight_' + index).val(json.detail[i].weight);
                    $('#selExpect_' + index).val(json.detail[i].excepted_value);
                    $('input:radio[name=radPL_' + index + '][value=' + json.detail[i].evaluate_value + ']').attr('checked', true);
                    //$('input:radio[name=radSuccess_' + index + '][value=' + json.detail[i].success_level + ']').attr('checked', true);
                    if (json.detail[i].success_level != '0') {
                        $('input:radio[name=radSuccess_' + index + '][value=' + json.detail[i].success_level + ']').attr('checked', true);
                        var e = parseInt(json.detail[i].success_level);
                        e = 0.5 * (e + 1);
                        $('#txtSuccess_' + index).val(e);
                    } else {
                        if (json.detail[i].success_score != '0.00') { $('#txtSuccess_' + index).val(json.detail[i].success_score); }
                    }

                }

                ResetEvent();
                UpdateCaluate();
                $('.normal').autosize();
                Set2Owner(bOwner, bConfirmed, iRevision);
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
            CreateContextMenu();
            UpdateCaluate();
        }
    });
}

function ResetEvent() {
    $("input").unbind();
    $('input').bind('change', function() { BeforeUpdateCaluate(this.id); });
    $("select").unbind();
    $('select').bind('change', function() { BeforeUpdateCaluate(this.id); });
    $('#drpRoundNo').bind('change', function() { drpRoundNo_change(); });

    $('#drpResult').bind('change', function() { drpResult_change(); });
}

function BeforeUpdateCaluate(id) {
    var name = id.split('_')[0];
    var idx = id.split('_')[1];
    //alert(name);
    switch (name) {
        case 'txtWeight':
        case 'radPL':
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


$('#btnAddRow').click(function() {
    CreateOneRow(++gRowNum);
    ResetEvent();

    $('.normal').autosize();

    UpdateCaluate();

    return false;
});

$('.btnDelete').live('click', function() {
    //alert(this.id);
    var aid = this.id.split('_');
    var id = aid[1];
    //alert(id);

    DeleteRow(id);

    return false;
});

function DeleteRow(id) {

    $('#trRowNo_' + id).remove();

    gRowArray.remove(gRowArray.indexOf(parseInt(id)));
    console.log(gRowArray);

    UpdateCaluate();

    CreateContextMenu();
    
    return true;
}

$('.btnInsert').live('click', function() {
    //alert(this.id);
    var aid = this.id.split('_');
    var id = aid[1];
    //alert(id);

    InsertRow (id);

    return false;
});

function InsertRow (id) {

    CreateOneRow2P(++gRowNum, id);

    ResetEvent();
    UpdateCaluate();

    $('.normal').autosize();
    Set2Owner(bOwner, bConfirmed, iRevision);

    CreateContextMenu();
    
    return true;
}

$('.btnDetail').live('click', function() {
    //alert(this.id);
    var aid = this.id.split('_');
    var index = aid[1];
    //alert(index);

    $('#lblPOPNo').text(index);
    $('#lblPOPName').text($('#txtName_' + index).val());
    $('txtPOPDetail').css( 'height', '500px');
    $('txtPOPDetail').val('');
    $('#txtPOPDetail').val($('#txtDetail_' + index).val());

    //$('.normal').autosize();

    $('#EvaluateDetail_divModel').reveal();

    return false;
});

$('#btnPOPUpdate').click(function() {
    var index = $('#lblPOPNo').text();
    
    $('#txtDetail_' + index).val($('#txtPOPDetail').val());
    
    $('#EvaluateDetail_divModel').trigger('reveal:close');
    return false;
});

function CreateFormBody() {
    $('#tblEvaluateForm tbody').empty();
    gRowArray = [];
    var i;
    for (i = 1; i <= gRowNum; i++) {
        CreateOneRow (i);
    }
    ResetEvent();

    //$('.normal').autosize();
}
function CreateOneRow(i) {
    CreateOneRow2P(i, i);
}

function CreateOneRow2P(i, order) {

    var row = "<tr id='trRowNo_" + i + "' >" +
                "<td style='display:nonex' id='contextmenu_" + i + "'  class='contextmenu'><label class='tdValue' id='lblRowNo_" + i + "'></label>" +
                "<!--<button id='btnInsert_" + i + "' class='btnInsert'>I</button>--></td>" +
                "<td>" +
                "    <div class='inputWrapper'>" +
                "    <label>" +
                "        <textarea  class='normal' id='txtName_" + i + "' name='txtName_" + i + "'></textarea>" +
                "    </label>" +
                "    </div>" +
                "    <div class='hidden'>" +
                "        <textarea  class='normal' id='txtDetail_" + i + "' name='txtDetail_" + i + "'></textarea>" +
                "    </div>" +                
                "</td>" +
                "<td><button id='btnDetail_" + i + "' class='btnDetail'>รายละเอียด</button></td>" +
                "<td>" +
                "    <div class='inputWrapper'>" +
                "    <label>" +
                "        <input class='txtWeight' type='text' id='txtWeight_" + i + "' /> " +
                "    </label>" +
                "    </div>" +
                "</td>" +
                "<td>" +
                    "<select id='selExpect_" + i + "'>" +
                    "    <option value='1'>1</option>" +
                    "    <option value='2'>2</option>" +
                    "    <option value='3'>3</option>" +
                    "    <option value='4'>4</option>" +
                    "    <option value='5'>5</option>" +
                    "</select>" +
                "</td>" +
                "<td class='tdPLLevel'><input type='radio' id='radPL_" + i + "' name='radPL_" + i + "' value='1' /></td>" +
                "<td class='tdPLLevel'><input type='radio' id='radPL_" + i + "' name='radPL_" + i + "' value='2' /></td>" +
                "<td class='tdPLLevel'><input type='radio' id='radPL_" + i + "' name='radPL_" + i + "' value='3' /></td>" +
                "<td class='tdPLLevel'><input type='radio' id='radPL_" + i + "' name='radPL_" + i + "' value='4' /></td>" +
                "<td class='tdPLLevel'><input type='radio' id='radPL_" + i + "' name='radPL_" + i + "' value='5' /></td>" +
                "<td class='tdPLScore'><label class='tdValue' id='lblPLScore_" + i + "'></label></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='1' /></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='2' /></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='3' /></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='4' /></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='5' /></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='6' /></td>" +
                "<td class='tdSuccessLevel'><input type='radio' id='radSuccess_" + i + "' name='radSuccess_" + i + "' value='7' /></td>" +
                "<td class='tdSuccessLevel'>" +
                "    <div class='inputWrapper'>" +
                "    <label>" +
                "        <input class='txtWeight' type='text' id='txtSuccess_" + i + "' /> " +
                "    </label>" +
                "    </div>" +
                "</td>" +                
                "<td class='tdSuccessScore'><label class='tdValue' id='lblSuccessScore_" + i + "'></label></td>" +
                "<td><button id='btnDelete_" + i + "' class='btnDelete'>ลบ</button></td>" +
            "</tr>";
    var iorder = gRowArray.indexOf(parseInt(order));
    console.log(i + '  ' + order + ' ' + iorder);
    if (i == order) {
        $('#tblEvaluateForm tbody').append(row);
        gRowArray.insert(i - 1, i);
    } else {
        $('#tblEvaluateForm tbody tr:nth-child(' + (iorder+1) + ')').before(row);
        gRowArray.insert(iorder, i);
    }
    
    console.log(gRowArray);
}

function UpdateCaluate() {
    //alert(this.id);
    var iffull = giffull;  //70;
    var sa = 0;
    var sd = 0;
    var sf = 0;

    var expect_pl1 = 0;
    var expect_pl2 = 0;
    var expect_pl3 = 0;
    var expect_pl4 = 0;
    var expect_pl5 = 0;

    var evaluate_pl1 = 0;
    var evaluate_pl2 = 0;
    var evaluate_pl3 = 0;
    var evaluate_pl4 = 0;
    var evaluate_pl5 = 0;

    var i;
    //for (i = 1; i <= gRowNum; i++) {
    for (i = 0; i <= gRowArray.length; i++) {
        var index = gRowArray[i];
        var a = parseFloat($('#txtWeight_' + index).val());
        var b = parseFloat($('#selExpect_' + index).val());
        var c = parseFloat($('input[name=radPL_' + index + ']:checked').val());
        var d = parseFloat(a / b) * parseFloat(c);
        if (isNaN(d)) {
            $('#lblPLScore_' + index).text('');
            d = 0;
        }
        else $('#lblPLScore_' + index).text(d.toFixed(2));

        /*var e = $('input[name=radSuccess_' + index + ']:checked').val();
        
        if (e == null) { continue; }
        e = parseInt(e);
        e = 0.5 * (e + 1);*/
        e = $('#txtSuccess_' + index).val();
        var f = d * e;
        if (isNaN(f)) {
            $('#lblSuccessScore_' + index).text('');
        }
        else $('#lblSuccessScore_' + index).text(f.toFixed(2));

        //if ((!isNaN(a)) & (a != '')) { sa += parseFloat(a); }
        if ((!isNaN(d)) & (d != '')) { sd += parseFloat(d); }
        if ((!isNaN(f)) & (f != '')) { sf += parseFloat(f); }

        if ((!isNaN(a)) & (a != '')) {
            sa += parseFloat(a);
            switch (b) {
                case '1':
                case 1:
                    expect_pl1 += (a); 
                    break;
                case '2':
                case 2:
                    expect_pl2 += (a); 
                    break;
                case '3':
                case 3:
                    expect_pl3 += (a); 
                    break;
                case '4':
                case 4:
                    expect_pl4 += (a); 
                    break;
                case '5':
                case 5:
                    expect_pl5 += (a); 
                    break;
            }

            switch (c) {
                case '1':
                case 1:
                    evaluate_pl1 += (a); 
                    break;
                case '2':
                case 2:
                    evaluate_pl2 += (a); 
                    break;
                case '3':
                case 3:
                    evaluate_pl3 += (a); 
                    break;
                case '4':
                case 4:
                    evaluate_pl4 += (a); 
                    break;
                case '5':
                case 5:
                    evaluate_pl5 += (a); 
                    break;
            }             
        }
    }

    $('#lblSumWeight').text(sa.toFixed(2));
    $('#lblFullPL').text(sa.toFixed(3));
    $('#lblFullSuccess').text((sd * 4).toFixed(3));

    $('#lblSumPL').text(sd.toFixed(1));
    $('#lblSumSuccess').text(sf.toFixed(2));

    //$('#lblPercentPL').text((sd / sa * 100).toFixed(1));
    //$('#lblPercentSuccess').text((sf / (sa * 4) * 100).toFixed(2));
    var pPL = sd / sa * 100;
    if (isNaN(pPL)) {
        $('#lblPercentPL').text('');
    }
    else $('#lblPercentPL').text(pPL.toFixed(2));

    var pSuccess = sf / (sd * 4) * 100;
    if (isNaN(pSuccess)) {
        $('#lblPercentSuccess').text('');
    }
    else $('#lblPercentSuccess').text(pSuccess.toFixed(2));
    
    $('#lblIFFull').text(iffull);
    //$('#lblIFPL').text(((sd / sa * 100) * iffull / 100).toFixed(1));
    //$('#lblIFSuccess').text(((sf / (sa * 4) * 100) * iffull / 100).toFixed(2));
    var ifPL = pPL * iffull / 100;
    if (isNaN(ifPL)) {
        $('#lblIFPL').text('');
    }
    else $('#lblIFPL').text(ifPL.toFixed(1));

    var ifSuccess = pSuccess * iffull / 100;
    if (isNaN(ifSuccess)) {
        $('#lblIFSuccess').text('');
    }
    else $('#lblIFSuccess').text(ifSuccess.toFixed(2));

    $('#lblExpectPL1').text(expect_pl1);
    $('#lblExpectPL2').text(expect_pl2);
    $('#lblExpectPL3').text(expect_pl3);
    $('#lblExpectPL4').text(expect_pl4);
    $('#lblExpectPL5').text(expect_pl5);

    $('#lblEvaluatePL1').text(evaluate_pl1);
    $('#lblEvaluatePL2').text(evaluate_pl2);
    $('#lblEvaluatePL3').text(evaluate_pl3);
    $('#lblEvaluatePL4').text(evaluate_pl4);
    $('#lblEvaluatePL5').text(evaluate_pl5);

    //$('#lblSumWeight').text();
    if (sa > 100) {
        $('#lblSumWeight').css('color', 'Red');
        $('#lblSumWeight').css('font-weight', 'bold');
        $("#lblSumWeight").attr('title', 'ต้องเท่ากับ 100%');
    }
    else if (sa < 100) {
        $('#lblSumWeight').css('color', 'Red');
        $('#lblSumWeight').css('font-weight', 'bold');
        $("#lblSumWeight").attr('title', 'ต้องเท่ากับ 100%');
    }
    else {
        $('#lblSumWeight').css('color', 'Green');
        $('#lblSumWeight').css('font-weight', 'bold');
        $("#lblSumWeight").attr('title', '');
    }
}

function Set2Owner(Owner, Confirmed, Revision) {

    if (Owner & !Confirmed & (Revision == 0)) { btnShow($("#btnSave")); btnShow($("#btnConfirm")); Editable(true); $("#lblToolMessage").text(''); }
    if (Owner & Confirmed & (Revision == 0)) { btnDisable($("#btnSave")); btnDisable($("#btnConfirm")); Editable(false); $("#lblToolMessage").text(''); }
    if (Owner & (Revision == 1)) { btnHide($("#btnSave")); btnHide($("#btnConfirm")); Editable(false); $("#lblToolMessage").text(''); }
    if (!Owner & (Revision == 0)) { btnHide($("#btnSave")); btnHide($("#btnConfirm")); Editable(false); $("#lblToolMessage").text(''); }
    if (!Owner & !Confirmed & (Revision == 1)) { btnDisable($("#btnSave")); btnHide($("#btnConfirm")); Editable(false); $("#lblToolMessage").text("ไม่สามารถบันทึกได้ เนื่องจากผู้ใช้ยังไม่ได้ยืนยันข้อมูล"); }
    if (!Owner & Confirmed & (Revision == 1)) { btnShow($("#btnSave")); btnHide($("#btnConfirm")); Editable(true); $("#lblToolMessage").text(''); }

    btnHide($("#btnConfirm"));
}

function Editable(bEditable) {
    var i;
    gbEditable = bEditable;
    if (bEditable) {
        for (i = 1; i <= gRowNum; i++) {
            $('#txtName_' + i).removeAttr("disabled");
            $('#txtWeight_' + i).removeAttr("disabled");
            $('#selExpect_' + i).removeAttr("disabled");
            $('#btnDelete_' + i).removeAttr("disabled");
        }
        $(':radio').removeAttr("disabled");
        $('#btnAddRow').removeAttr("disabled");
        $('#btnPOPUpdate').removeAttr("disabled");
    } else {
        for (i = 1; i <= gRowNum; i++) {
            $('#txtName_' + i).attr('disabled', 'disabled');
            $('#txtWeight_' + i).attr('disabled', 'disabled');
            $('#selExpect_' + i).attr('disabled', 'disabled');
            $('#btnDelete_' + i).attr('disabled', 'disabled');
        }
        $(':radio').attr('disabled', 'disabled');
        $('#btnAddRow').attr('disabled', 'disabled');
        $('#btnPOPUpdate').attr('disabled', 'disabled');
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

function UpdateLevelMessage() {
    switch ($('#lblLevelNo').text()) {
        case 'จ1':
        case 'จ2':
        case 'พ1':
        case 'พ2':
        case 'สว1':
        case 'สว2':
            $('#lblLevelMessage').text('จ1-2/พ1-2/สว1-2 ระดับชั้นความสามารถที่คาดหวัง 2 ขึ้นไป');
            break;
        case 'จ3':
        case 'พ3':
        case 'สว3':
            $('#lblLevelMessage').text('จ3/พ3/สว3 ระดับชั้นความสามารถที่คาดหวัง 3 ขึ้นไป จะต้องไม่น้อยกว่า 30% ');
            break;
        case 'จ4':
        case 'พ4':
        case 'สว4':
            $('#lblLevelMessage').text('จ4/พ4/สว4 ระดับชั้นความสามารถที่คาดหวัง 3 ขึ้นไป จะต้องไม่น้อยกว่า 40% ');
            break;
        case 'บ1':
        case 'สว5':
        case 'ช':
            $('#lblLevelMessage').text('บ1/สว5/ช ระดับชั้นความสามารถที่คาดหวัง 3 และ 4 (ไม่น้อยกว่า 5%) รวมกันจะต้องไม่น้อยกว่า 50%');
            break;
        case 'บ2':
            $('#lblLevelMessage').text('บ2 ระดับชั้นความสามารถที่คาดหวัง 3 และ 4 (ไม่น้อยกว่า 10%) รวมกันจะต้องไม่น้อยกว่า 60%');
            break;
        case 'บ3':
            $('#lblLevelMessage').text('บ3 ระดับชั้นความสามารถที่คาดหวัง 3 และ 4 (ไม่น้อยกว่า15%) รวมกันจะต้องไม่น้อยกว่า 60%');
            break;
        case 'บ4':
            $('#lblLevelMessage').text('บ4 ระดับชั้นความสามารถที่คาดหวัง 3 และ 4 (ไม่น้อยกว่า20%) รวมกันจะต้องไม่น้อยกว่า 60% และ คาดหวัง 5 ไม่น้อยกว่า 5%');
            break;

    }
}

function isOwner() {
    return ($('#lblLoginName').text() == $('#txtUserName').val());
}

/*jQuery(function($) {
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
});*/

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