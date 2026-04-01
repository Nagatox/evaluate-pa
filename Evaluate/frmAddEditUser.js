$(document).ready(function() {

    SetMenuAdmin($('#lblGroupName').text());

    f_openform();

    $('#divBackground').hide();
    $('#divWaiting').hide();

});


var n_jqgrid = "tblUser"; // table             /// change_xxx ///
var t_jqgrid = "#" + n_jqgrid; // table     /// change_xxx ///
var p_jqgrid = "#div" + n_jqgrid; // div page   /// change_xxx ///
var u_jqgrid = "frmAddEditUser.aspx?q=5"; // URL     /// change_xxx ///
var frmg = "#FrmGrid_" + n_jqgrid + " ";    /// change_xxx ///

var max_row_page = 10000;  /// change_xxx ///

jQuery(t_jqgrid).jqGrid(
{
    // url: u_jqgrid ,
    datatype: "local",
    mtype: 'GET',
    width: 900, height: 500,  /// change_xxx ///
    forceFit: true,
    colNames: ['ลำดับที่', 'user_id', 
               'เลขที่บุคลากร', 'ชื่อบัญชี',
               'ชื่อ', 'นามสกุล',
               'position_id', 'ตำแหน่ง', 
               'level_id', 'ระดับ',
               'group_id', 'กลุ่มผู้ใช้',
               'campus_id', 'พื้นที่การศึกษา',
               'faculty_id', 'คณะ',
               'department_id', 'ภาควิชา',
               'สถานะ', 'หมายเหตุ'], /// change_xxx ///
    colModel: [
        { name: 'id', index: 'id', sorttype: "int", width: 80, editable: false, hidden: false, align: 'center' },
        { name: 'user_id', index: 'user_id', editable: true, hidden: true },
        { name: 'employee_id', index: 'employee_id', editable: true, hidden: false },  /// change_xxx ///
        { name: 'username', index: 'username', editable: true, hidden: false },  /// change_xxx ///
        {name: 'firstname', index: 'firstname', editable: true, hidden: false },  /// change_xxx ///
        {name: 'lastname', index: 'lastname', editable: true, hidden: false },  /// change_xxx ///
        {name: 'position_id', index: 'position_id', editable: true, hidden: true },
        { name: 'position', index: 'position', editable: true, hidden: false, edittype: "select", editoptions: {
            value: "1",
            dataInit: "กรุณาเลือก"
            }
        },
        { name: 'level_id', index: 'level_id', editable: true, hidden: true },
        { name: 'level', index: 'level', editable: true, hidden: false, edittype: "select", editoptions: {
            value: "1",
            dataInit: "กรุณาเลือก"
        }
        },

        {name: 'group_id', index: 'group_id', editable: true, hidden: true },
        { name: 'group', index: 'group', editable: true, hidden: false, edittype: "select", editoptions: {
            value: "1",
            dataInit: "กรุณาเลือก"
        }
        },  /// change_xxx ///
        {name: 'campus_id', index: 'campus_id', editable: true, hidden: true },
        { name: 'campus', index: 'campus', editable: true, hidden: false, edittype: "select", editoptions: {
            value: "1",
            dataInit: "กรุณาเลือก"
        }
        },  /// change_xxx ///
        {name: 'faculty_id', index: 'faculty_id', editable: true, hidden: true },
        { name: 'faculty', index: 'faculty', editable: true, hidden: false, edittype: "select", editoptions: {
            value: "1",
            dataInit: "กรุณาเลือก"
        }
        },  /// change_xxx ///
        {name: 'department_id', index: 'department_id', editable: true, hidden: true },
        { name: 'department', index: 'department', editable: true, hidden: false, edittype: "select", editoptions: {
            value: "1",
            dataInit: "กรุณาเลือก"
        }
        },  /// change_xxx ///
        {name: 'active', index: 'active', width: 50, editable: true, hidden: false, align: 'center', formatter: 'checkbox', edittype: "checkbox", stype: 'checkbox', editoptions: { value: "1:0"} },  /// change_xxx ///
        {name: 'status', index: 'status', editable: true, hidden: false },  /// change_xxx ///
	],
    pager: p_jqgrid,
    addedrow: "last",
    reloadAfterSubmit: true,
    scrollrows: true,
    sortname: 'id',  /// change_xxx /// 
    loadonce: true, // disabled paging and all sorting done locally
    rowNum: max_row_page, //  grouping:true, groupingView : { groupField : ['lotno'] }, // if you want groupping 
    editurl: 'Default.aspx?q=dummy', // this is dummy existing url
    onSelectRow: function(ids) { },
    viewrecords: true
});


//// for add edit dele
jQuery(t_jqgrid).jqGrid('navGrid', p_jqgrid, {},
{
    //jQuery(t_jqgrid).jqGrid('setGridParam', {page:'1',sortname:'tax',sortorder:'asc'}).trigger('reloadGrid');
    editCaption: "Edit the record", //////////editttttttttttttttttt
    url: 'frmAddEditUser.aspx?q=UpdateUser',
    addedrow: "last",
    bSubmit: "Submit",
    closeAfterEdit: "true",
    bCancel: "Cancel",
    width: 620,  // Pop up Width
    processData: "Processing...",
    msg: {
        required: "Field is required",
        number: "Please enter valid number!",
        minValue: "value must be greater than or equal to ",
        maxValue: "value must be less than or equal to"
    },
    beforeShowForm: function(formid) {
        var group_id = $("#group_id", formid).val();
        var position_id = $("#position_id", formid).val();
        var level_id = $("#level_id", formid).val();
        var campus_id = $("#campus_id", formid).val();
        var faculty_id = $("#faculty_id", formid).val();
        var department_id = $("#department_id", formid).val();

        getDrpData(position_id, level_id, group_id, campus_id, faculty_id, department_id);
        ResetFormInput();  /// change_xxx ///
        AddMarkRequireField();

        return [true, "dd"];
    },
    beforeSubmit: function(response, postdata) {
        //$('#divBackground').show();
        //$('#divWaiting').show();

        return [true, "dd"];
    },
    afterSubmit: function(response, postdata) {
        //$('#divBackground').hide();
        //$('#divWaiting').hide();

        return [true, "dd", postdata.id];
    },
    afterComplete: function(response, postdata, formid) {

        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "position_id", $('#position option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "position", $('#position option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "level_id", $('#level option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "level", $('#level option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "group_id", $('#group option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "group", $('#group option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "campus_id", $('#campus option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "campus", $('#campus option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "faculty_id", $('#faculty option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "faculty", $('#faculty option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "department_id", $('#department option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "department", $('#department option:selected').text());
    },
    afterclickPgButtons: function(whichbutton, formid, rowid) {
    }
},
{
    addCaption: "Add Value",
    url: 'frmAddEditUser.aspx?q=AddUser',
    width: 620,
    closeAfterAdd: "true",
    reloadAfterSubmit: false,
    addedrow: "last",
    beforeShowForm: function(formid) {
        getDrpData(1, 1, 4, 1, 7, 1);
        ResetFormInput(); /// change_xxx ///
        AddMarkRequireField();

        return [true, "dd"];
    },
    afterSubmit: function(response, postdata) {


        //return [true, '', new_user_id];
        var myrow = jQuery(t_jqgrid).jqGrid('getGridParam', 'data');
        return [true, "", myrow.length + 1];
    },
    afterComplete: function(response, postdata, formid) {
        //alert(response.responseText);
        //var new_user_id = $.parseJSON(response.responseText).user_id;
        //alert(new_user_id);
        //jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "user_id", new_user_id);

        //jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "position_id", $('#position option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "position_id", $('#position option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "position", $('#position option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "level_id", $('#level option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "level", $('#level option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "group_id", $('#group option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "group", $('#group option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "campus_id", $('#campus option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "campus", $('#campus option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "faculty_id", $('#faculty option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "faculty", $('#faculty option:selected').text());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "department_id", $('#department option:selected').val());
        jQuery(t_jqgrid).jqGrid('setCell', postdata.id, "department", $('#department option:selected').text());
    },
    addedrow: "last"
},
{	//delete option
    url: 'frmAddEditUser.aspx?q=DeleteUser',
    /*delData: { user_id: function() {
    //var rowData = jQuery(this).jqGrid('getRowData', rowid);
    return 99;
    }
    },*/
    onclickSubmit: function(options, rowid) {
        var rowData = jQuery(t_jqgrid).jqGrid('getRowData', rowid);
        return { username: rowData.username };

    },
    afterSubmit: function(response, postdata) {
        return [true, "dd", postdata.id];
    },
    afterComplete: function(response, postdata, formid) {
        //f_sortdatagrid(1, t_jqgrid, colmydata); // 0 order all
        RearrageGrid(t_jqgrid);
    }

},
{ closeOnEscape: true, multipleSearch: true, closeAfterSearch: true }, {} //search options
);

function LoadtoGridDetail(objjson)  // Load To Grid
{
    objtblUser = objjson; /// change_yyy ///

    jQuery(t_jqgrid).jqGrid('clearGridData').trigger('reloadGrid');
    jQuery(t_jqgrid).jqGrid('setGridParam', { data: objtblUser.detail, datatype: 'local' }).trigger('reloadGrid');  // change_xxx objca101.rmlot

}
/// change_yyy ///

function ChkRequiredInputBeforSave2Grid(postdata) {
    var objchk = true;

    objchk = checkNulltext($(frmg + '#username'), $(frmg + '#username').val()) && objchk;
    objchk = checkNulltext($(frmg + '#first_name'), $(frmg + '#first_name').val()) && objchk;
    objchk = checkNulltext($(frmg + '#last_name'), $(frmg + '#last_name').val()) && objchk;
    objchk = checkNullDrp($('#groupname'), $('#groupname').val()) && objchk;
    objchk = checkNullDrp($('#campus'), $('#campus').val()) && objchk;
    objchk = checkNullDrp($('#faculty'), $('#faculty').val()) && objchk;
    objchk = checkNullDrp($('#department'), $('#department').val()) && objchk;
    //    objchk = checkNulltext($(frmg + '#status'), $(frmg + '#status').val()) && objchk;

    return objchk;
}


function ChkSizeInputBeforSave2Grid(postdata) {
    var objchk = true;

    return objchk;
}

function ChkDuplicateKeyBeforSave2Grid(postdata, mode) {
    var all_data = jQuery(t_jqgrid).jqGrid('getGridParam', 'data');
    for (i = 0; i < all_data.length; i++) {
        if (all_data[i].id != postdata.id) {

            if (all_data[i].username == postdata.username) {
                //           if ((all_data[i].staff_name == postdata.staff_name) && (all_data[i].project_name == postdata.project_name)) {
                changetxtcolor(frmg + '#username', false);
                // changetxtcolor(frmg + '#project_name', false); 
                return false;
            }
        }
    }
}

function ResetFormInput() {
    $('#editmodtblUser').width(700); /// Set textbox width

    changetxtcolor(frmg + "#username", true); /// change_xxx
    $(frmg + "#username").width(200); /// Set textbox width

    changetxtcolor(frmg + "#first_name", true); /// change_xxx
    $(frmg + "#first_name").width(400); /// Set textbox width

    changetxtcolor(frmg + "#last_name", true); /// change_xxx
    $(frmg + "#last_name").width(400); /// Set textbox width

    changetxtcolor(frmg + "#status", true); /// change_xxx
    $(frmg + "#status").width(400); /// Set textbox width
}

function getDrpData(position_id, level_id, group_id, campus_id, faculty_id, department_id) {
    loadTodropdown('#position', "frmMainService.aspx?q=GetPositionList2DRP&time=" + time());
    $('#position').val(position_id);
    loadTodropdown('#level', "frmMainService.aspx?q=GetLevelList2DRP&time=" + time());
    $('#level').val(level_id);
    loadTodropdown('#group', "frmMainService.aspx?q=GetGroupList2DRP&time=" + time());
    $('#group').val(group_id);
    loadTodropdown('#campus', "frmMainService.aspx?q=GetCampusList2DRP&time=" + time());
    $('#campus').val(campus_id);
    loadTodropdown('#faculty', "frmMainService.aspx?q=GetFacultyList2DRP&campus_id=" + $('#campus').val() + "&time=" + time());
    $('#faculty').val(faculty_id);
    loadTodropdown('#department', "frmMainService.aspx?q=GetDepartmentList2DRP&campus_id=" + $('#campus').val() + "&faculty_id=" + $('#faculty').val() + "&time=" + time());
    $('#department').val(department_id);

    $('#campus').change(function() {
    loadTodropdown('#faculty', "frmMainService.aspx?q=GetFacultyList2DRP&campus_id=" + $('#campus').val() + "&time=" + time());
    loadTodropdown('#department', "frmMainService.aspx?q=GetDepartmentList2DRP&campus_id=" + $('#campus').val() + "&faculty_id=" + $('#faculty').val() + "&time=" + time());
    });

    $('#faculty').change(function() {
    loadTodropdown('#department', "frmMainService.aspx?q=GetDepartmentList2DRP&campus_id=" + $('#campus').val() + "&faculty_id=" + $('#faculty').val() + "&time=" + time());
    });
}
function AddMarkRequireField() {
    //$('#tr_end_date').name ('*');
}

function tblUser_dataHead() {
    this.id = '';
    this.detail = new Array();
    this.recordCount = '';
    this.isError = false;
    this.ErrMessage = '';
}

function tblUser_data() {
    this.id = '';
    this.user_id = '';
    this.username = '';
    this.first_name = '';
    this.last_name = '';
    this.group_id = '';
    this.groupname = '';
    this.campus_id = '';
    this.campus = '';
    this.faculty_id = '';
    this.faculty = '';
    this.department_id = '';
    this.department = '';
    this.active = '';
    this.status = '';
}
var objtblUser = new tblUser_dataHead(); /// change_yyy ///


function f_openform() {

    var pageData = new tblUser_dataHead(); /// change_yyy ///
    var objtblUserDetail = new tblUser_data(); /// change_yyy ///
    pageData.detail = objtblUserDetail;

    pageData.id = 'frmAddEditUser'; /// change_yyy ///
    var json = $.toJSON(pageData);
    $.ajax({
        url: 'frmAddEditUser.aspx?q=GetUserList&time=' + time(), /// change_yyy ///
        type: 'POST',
        data: 'data=' + json,
        dataType: 'json',
        success: function(json) {
            if (!json.isError) {
                LoadtoGridDetail(json);
            }
            else {
                alert('Load Fail');
            }
        }
    });
}

function checkNullDrp(objname, objval) {

    if (trim(objval) < 0) {
        changetxtcolor(objname, false);
        return false;
    }
    else {
        changetxtcolor(objname, true);
        return true;
    }
}






