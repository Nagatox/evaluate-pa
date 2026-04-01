
$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    $('#divBackground').hide();
    $('#divWaiting').hide();

    if ($('#lblGroupName').text() == 'Department User') {
        //window.location = "frmEvaluateForm.aspx"
        window.location = "frmEvaluatePart00.aspx"
        return;
    }

    LoadSummaryDataPAbyLoginName();
});

function LoadSummaryDataPAbyLoginName() {
    var loginname = $('#lblLoginName').text();
    var round = $('#txtRoundNo').val();
    var evaluate_year = $('#lblBudgetYear').text();

    if ($('#lblGroupName').text() == 'Faculty User') {
        department_id = '0';
    }

    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadSummaryDataPAbyLoginName&loginname=' + loginname + '&round=' + round + '&evaluate_year=' + evaluate_year + '&time=' + time(), /// change_yyy ///
        //type: 'POST',
        //data: { username: username },
        dataType: 'json',
        success: function(json) {
            //alert(json.recordCount);
            if (!json.isError) {
                $('#tableEmployeeList tbody').empty();
                var i;

                for (i = 0; i < json.recordCount; i++) {
                    var str_confirm = '';
                    if (json.detail[i].confirmed == '1') {
                        str_confirm = '<b>ผู้ใช้ยืนยันข้อมูลแล้ว เมื่อวันที่ ' + json.detail[i].confirm_datetime + '</b>';
                    }

                    var row = "<tr>" +
                          "     <td rowspan='1'>" + json.detail[i].department_name + "</td>" +
                          "     <td rowspan='1'><a href='frmEvaluatePart00.aspx?u=" + json.detail[i].username + "&result=1'>" + FormatEmployeeID(json.detail[i].employee_id) + "</a></td>" +
                          "     <td rowspan='1'>" + json.detail[i].firstname + " " + json.detail[i].lastname + "</td>" +
                          "     <td>" + ((json.detail[i].confirmed == '1') ? "<label class='lblConfirmed'>ยืนยัน</label><br \>" + json.detail[i].confirm_datetime : "ยังไม่ยืนยัน") + "</td>" +
                          "     <td>" + ((json.detail[i].boss_confirmed == '1') ? "<label class='lblConfirmed'>ยืนยันรับทราบ</label><br \>" + json.detail[i].boss_confirm_datetime : "ยังไม่ยืนยันรับทราบ") + "</td>" +
                          "     <td>" + ((json.detail[i].result_confirmed == '1') ? "<label class='lblConfirmed'>ยืนยัน</label><br \>" + json.detail[i].result_confirm_datetime : "ยังไม่ยืนยัน") + "</td>" +
                          "     <td>" + ((json.detail[i].boss_result_confirmed == '1') ? "<label class='lblConfirmed'>ยืนยันรับทราบ</label><br \>" + json.detail[i].boss_result_confirm_datetime : "ยังไม่ยืนยันรับทราบ") + "</td>" +
                          "     <td>" + parseFloat(json.detail[i].sum_percent).toFixed(2) + "</td>" +
                          "     <td>" + parseFloat(json.detail[i].sum_score_100).toFixed(2) + "</td>" +
                          "     <td>" + parseFloat(json.detail[i].total_score).toFixed(2) + "</td>" +
                          "     <td>" + parseFloat(json.detail[i].force_mean).toFixed(2) + "</td>" +
                          "     <td>" + ({ 1: 'ผ่าน', [-1]: 'ไม่ผ่าน' }[json.detail[i].result] || '') + "</td>" +
                          "     <td rowspan='1'>" +
                          "         <div class='inputWrapper'>" +
                          "         <label>" +
                          "             <textarea  class='normal' id='txtSuggestion' name='txtSuggestion' rows='5' disabled>" + json.detail[i].suggest + "</textarea>" +
                          "         </label>" +
                          "         </div>" +
                          "     </td>" +
                          " </tr>";

                    $('#tableEmployeeList tbody').append(row);
                }
                /*if (json.recordCount > 5) {
                for (i = 5; i < json.recordCount; i++) {
                CreateOneRow(++gRowNum);
                }
                }
                for (i = 0; i < json.recordCount; i++) {
                //alert (json.detail[i].activate_detail);
                var index = i + 1;
                $('#txtDetail_' + index).val(json.detail[i].activate_detail);
                $('#txtWeight_' + index).val(json.detail[i].weight);
                $('#selExpect_' + index).val(json.detail[i].excepted_value);
                $('input:radio[name=radPL_' + index + '][value=' + json.detail[i].evaluate_value + ']').attr('checked', true);
                $('input:radio[name=radSuccess_' + index + '][value=' + json.detail[i].success_level + ']').attr('checked', true);
                }

                ResetEvent();
                UpdateCaluate();
                $('.normal').autosize();
                SettoOwner(isOwner());*/
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function LoadSummaryDatabyLoginName() {
    var loginname = $('#lblLoginName').text();
    var round = $('#txtRoundNo').val();
    var evaluate_year = $('#lblBudgetYear').text();

    if ($('#lblGroupName').text() == 'Faculty User') {
        department_id = '0';
    }

    $.ajax({
        url: 'frmEvaluateService.aspx?q=LoadSummaryDatabyLoginName&loginname=' + loginname + '&round=' + round + '&evaluate_year=' + evaluate_year + '&time=' + time(), /// change_yyy ///
        //type: 'POST',
        //data: { username: username },
        dataType: 'json',
        success: function(json) {
            //alert(json.recordCount);
            if (!json.isError) {
                $('#tableEmployeeList tbody').empty();
                var i;

                for (i = 0; i < json.recordCount; i++) {
                    var str_confirm = '';
                    if (json.detail[i].confirmed == '1') {
                        str_confirm = '<b>ผู้ใช้ยืนยันข้อมูลแล้ว เมื่อวันที่ ' + json.detail[i].confirm_datetime + '</b>';
                    }                
                    var row = "<tr>" +
                          "     <td rowspan='3'>" + json.detail[i].department_name + "</td>" +
                          "     <td rowspan='3'><a href='frmEvaluatePart00.aspx?u=" + json.detail[i].username + "&result=1'>" + FormatEmployeeID(json.detail[i].employee_id) + "</a></td>" +
                          "     <td rowspan='3'>" + json.detail[i].firstname + " " + json.detail[i].lastname + "</td>" +
                          "     <td>คาดหวัง</td>" +
                          "     <td><label class='tdValue' id='lblExpectPL1'>" + json.detail[i].sum_excepted_PL1 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblExpectPL2'>" + json.detail[i].sum_excepted_PL2 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblExpectPL3'>" + json.detail[i].sum_excepted_PL3 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblExpectPL4'>" + json.detail[i].sum_excepted_PL4 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblExpectPL5'>" + json.detail[i].sum_excepted_PL5 + "</label></td>" +
                          "     <td rowspan='3'>" +
                          "         <div class='inputWrapper'>" +
                          "         <label>" +
                          "             <textarea  class='normal' id='txtSuggestion' name='txtSuggestion' rows='5' disabled>" + json.detail[i].commander_recomment + "</textarea>" +
                          "         </label>" +
                          "         </div>" +
                          "     </td>" +
                          " </tr>" +
                          " <tr>" +
                          "     <td>ประเมินได้</td>" +
                          "     <td><label class='tdValue' id='lblEvaluatePL1'>" + json.detail[i].sum_evaluate_PL1 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblEvaluatePL2'>" + json.detail[i].sum_evaluate_PL2 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblEvaluatePL3'>" + json.detail[i].sum_evaluate_PL3 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblEvaluatePL4'>" + json.detail[i].sum_evaluate_PL4 + "</label></td>" +
                          "     <td><label class='tdValue' id='lblEvaluatePL5'>" + json.detail[i].sum_evaluate_PL5 + "</label></td>" +
                          " </tr>" +
                          " <tr>" +
                          "     <td colspan='6'>" + str_confirm + "</td>" +
                          " </tr>";

                    $('#tableEmployeeList tbody').append(row);
                }
                /*if (json.recordCount > 5) {
                for (i = 5; i < json.recordCount; i++) {
                CreateOneRow(++gRowNum);
                }
                }
                for (i = 0; i < json.recordCount; i++) {
                //alert (json.detail[i].activate_detail);
                var index = i + 1;
                $('#txtDetail_' + index).val(json.detail[i].activate_detail);
                $('#txtWeight_' + index).val(json.detail[i].weight);
                $('#selExpect_' + index).val(json.detail[i].excepted_value);
                $('input:radio[name=radPL_' + index + '][value=' + json.detail[i].evaluate_value + ']').attr('checked', true);
                $('input:radio[name=radSuccess_' + index + '][value=' + json.detail[i].success_level + ']').attr('checked', true);
                }

                ResetEvent();
                UpdateCaluate();
                $('.normal').autosize();
                SettoOwner(isOwner());*/
            }
            else {
                alert('Load Fail:' + json.ErrMessage);
            }
        }
    });
}

function FormatEmployeeID(employee_id) {
    return employee_id.substring(4, 7) + '/' + employee_id.substring(0, 4);
}