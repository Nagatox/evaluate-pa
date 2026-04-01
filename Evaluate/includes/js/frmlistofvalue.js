/*function percentgrid(gname)
{
var gridPercen = 100;
switch (red) {
case "grid_1": gridPercen = 6.333; break;
case "grid_2": gridPercen = 14.666; break;
case "grid_3": gridPercen = 31.333; break;
case "grid_4": gridPercen = 39.666; break;
case "grid_5": gridPercen = 6.333; break;
case "grid_6": gridPercen = 47.5; break;
case "grid_7": gridPercen = 56.333; break;
case "grid_8": gridPercen = 64.333; break;
case "grid_9": gridPercen = 72.666; break;
case "grid_10": gridPercen = 81.333; break;
case "grid_11": gridPercen = 89.666; break;
default: gridPercen = 100;
return (gridPercen);
}
*/
/*
function getWidthInPct(tname,percent,pMax)
{
    screen_res = ($(tname).width())*0.99;
alert(screen_res);
    if (screen_res == 0)
    {
        screen_res = ($(document).width())*0.99 * (pMax/100);
    }
        
    col = parseInt((percent*(screen_res/100)));
    
  //  alert (col);
    return col;
}
*/
function getWidthInPct(percent,screen_res)
{
    col = parseInt((percent*(screen_res/100)));
    return col;
}
function getWidthAll(percent)
{
    screen_res = ($(document).width())*0.99;
   
    col = parseInt((percent*(screen_res/100)));
    return col;
}
function objjqgrid() {
    this.jq_list = "";  // table id
    this.jq_page = "";  // page id
    this.jurl = "";  // page id
    this.jhdiv = "";  // name div for hide popup
    this.jwidth = 500;  // width of pop up
    this.jheight = 150;  // height of pop up
    this.max_row_page = 10;
    this.rowlist = new Array();
    this.colindex = new Array();
    this.colmyname = new Array();
    this.colwidth = new Array();
    this.coltype = new Array();
    this.colhidden = new Array();
    this.colalign = new Array();
    this.coleditable = new Array();
    this.coldataindex = new Array();
    this.coldataform = new Array();
    this.extra = "";  // table id

}

function f_listofvalue(objjq) {
 
    var eval_rowlist = "[";
    for (i = 0; i < objjq.rowlist.length; i++) {
        eval_rowlist = eval_rowlist + objjq.rowlist[i] + ",";
    }
    eval_rowlist = eval_rowlist.substring(0, eval_rowlist.length - 1) + "]";
   // alert(eval_rowlist);

    var eval_return = "";
    for (i = 0; i < objjq.coldataform.length; i++) {
        eval_return = eval_return + "$(\"" + objjq.coldataform[i] + "\").val(myrow." + objjq.coldataindex[i] + ");";
    }

    var eval_colname = "[";
    for (i = 0; i < objjq.colmyname.length; i++) {
        eval_colname = eval_colname + "'" + objjq.colmyname[i] + "',";
    }
    eval_colname = eval_colname.substring(0, eval_colname.length - 1) + "]";

    var eval_colindex = "[";
    var tmpeval = "";
    for (i = 0; i < objjq.colindex.length; i++) {
        tmpeval = "{ name: '" + objjq.colindex[i] + "', index: '" + objjq.colindex[i] + "'";

        if (objjq.coltype[i] != "") // type 
        {
            if (objjq.coltype[i] == "date") {
                tmpeval = tmpeval + ", sorttype: 'date', formatter: 'date', formatoptions: { srcformat: 'd-m-Y', newformat: 'd/m/Y'}";
            }
            else {
                tmpeval = tmpeval + ", sorttype: '" + objjq.coltype[i] + "'";
            }

        }
        
        if (objjq.coleditable[i] == "true")
        {
            tmpeval = tmpeval + ", editable: true ";
        }
        
        tmpeval = tmpeval + ", width: '" + objjq.colwidth[i] + "'";
        tmpeval = tmpeval + ", align: '" + objjq.colalign[i] + "'";

        if (objjq.colhidden[i] != "") // type 
        {
            tmpeval = tmpeval + ", hidden: true";
        }
        eval_colindex = eval_colindex + tmpeval + "},";
    }
    eval_colindex = eval_colindex.substring(0, eval_colindex.length - 1) + "]";

    jQuery(objjq.jq_list).jqGrid ('GridUnload', objjq.jq_page);
    
    eval("\
jQuery(objjq.jq_list).jqGrid(\
{\
    url: objjq.jurl,\
    datatype: \"json\",\
    mtype: 'GET',\
    width: objjq.jwidth, height: objjq.jheight,\
    colNames: " + eval_colname + ",\
    colModel: " + eval_colindex + ",\
    pager: objjq.jq_page,\
    addedrow: \"last\",\
    reloadAfterSubmit: true,\
    scrollrows: true,\
    sortname: 'id',\
    loadonce: true,\
    rowList:" + eval_rowlist + ",\
    rowNum: objjq.max_row_page,\
    onSelectRow: function(ids) {\
        var myrow = jQuery(objjq.jq_list).jqGrid('getRowData', ids);" + eval_return + "\
        if (objjq.extra != \"\") {" + objjq.extra + ";}\
        jQuery(objjq.jq_list).jqGrid('clearGridData').trigger('reloadGrid');\
        $(objjq.jhdiv).dialog('close');\
    },\
    editurl: 'Default.aspx',\
    viewrecords: true\
});");
    jQuery(objjq.jq_list).jqGrid('navGrid', objjq.jq_page, { del: false, add: false, edit: false, search: true },{},{},{},{ closeOnEscape: true, multipleSearch: true, closeAfterSearch: true }, {});
    jQuery(objjq.jq_list).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
};

