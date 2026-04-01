<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEvaluateFormPA.aspx.vb" Inherits="Evaluate.frmEvaluateFormPA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Evaluate PA Form - Evaluate Project</title>
    <link rel="stylesheet" type="text/css" href="includes/css/main.css"   /> 
    <link rel="stylesheet" type="text/css" href="includes/css/Scroll.css"   />
    <link rel="stylesheet" type="text/css" href="Evaluate.css"   />
    
    <script type="text/javascript" src="includes/js/jquery-1.10.2.min.js"></script>
    <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>

	<!--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>-->
    <script type="text/javascript" src="includes/js/jquery-migrate-1.2.1.min.js"></script>
    <script type="text/javascript" src="includes/js/jquery.json-2.2.min.js"></script>
    
    <script type="text/javascript" src="includes/js/jquery-ui-1.8.4.custom.min.js"></script> 
    <script type="text/javascript" src="includes/js/jquery.corner.js"></script> 
    <script type="text/javascript" src="includes/js/i18n/grid.locale-en.js"></script>
    <script type="text/javascript" src="includes/js/header.js"></script>
    
    <!--<script type="text/javascript" src="includes/js/jquery-1.4.2.min.js"></script>-->
    <script type="text/javascript" src="includes/js/jquery-ui-1.8.4.custom.min.js"></script> 
    <script type="text/javascript" src="includes/js/jquery.ui.datepicker-th.js"></script> 
    <script type="text/javascript" src="includes/js/jquery-ui-1.8.1.offset.datepicker.min.js"></script>
    <script type="text/javascript" src="includes/js/jquery.corner.js"></script> 
    <script type="text/javascript" src="includes/js/jquery-fluid16.js"></script>

    <script type="text/javascript" src="includes/js/i18n/grid.locale-en.js"></script>
    <script type="text/javascript" src="includes/js/jquery.jqGrid.js"></script>

    <script type="text/javascript" src="includes/js/jqDnR.js"></script>
    <script type="text/javascript" src="includes/js/jquery.json-2.2.min.js"></script>

    <script type="text/javascript" src="includes/js/InnosoftCustomLib.js"></script>

    <script type="text/javascript" src="includes/js/frmlistofvalue.js"></script>
    <script type="text/javascript" src="includes/js/header.js"></script>

    <link rel="stylesheet" href="includes/css/reveal.css" />
    <script type="text/javascript" src="includes/js/jquery.reveal.js"></script>

    <script type="text/javascript" src="includes/js/jquery.contextmenu.js"></script>
    <link rel="stylesheet" type="text/css" href="includes/css/contextmenu.css"   />    
        
    <script type="text/javascript" src="includes/js/InnosoftCustomLib.js"></script> 
    <script type="text/javascript" src="includes/js/jquery.autosize.js"></script> 
		<script>
		    $(function() {
		        $('.normal').autosize();
		    });
		</script>    
</head>
<body>
    <div id='divBackground' class="UpdateProgressBackground"></div>
    <div id='divWaiting' class="UpdateProgressContent">Please Wait ...</div>
    <form id="form1" runat="server">
    <div class="container_12">
    <!--  =============== Start Header ===============   -->    	
         <div id="header" class="grid_2">
            <img id='imglogo' src="images/EvaluateBanner.png" alt="Evaluate" width="40px" height="50px"/>
            <label id='namesystem'>งานบุคคล</label>
        </div>
        <div id="div_stauts" class="grid_8" style="text-align: center;">
            <img id='imgStatus' src="images/pa_status.png" alt="Evaluate" height="50px" style="width:60%;height:30%"/>
        </div>
        <div id="login" class="grid_2 rounded">
            <b><asp:Label runat="server" ID="lblLoginName"></asp:Label></b><br />
            กลุ่มผู้ใช้ : <asp:Label runat="server" ID="lblGroupName"></asp:Label><br />
            หน่วยงาน : <asp:Label runat="server" ID="lblOrgName"></asp:Label><br />
        </div>
        <!--  =============== Start Main Menu ===============   -->
        <!-- #include FILE="menu.inc" -->  
        <div class="clear"></div>
        <div class="grid_4">
            <div class="toolbar">
                <button id="btnBossSaveResult" class="save" disabled>บันทึก</button>
                <button id="btnBossConfirmResult" class="check" disabled>ยืนยัน รับทราบ</button>
                <button id="btnBossUnlockResult" class="unlocked" disabled>ปลดล็อค ให้กลับไปแก้ไข</button>
                <button id="btnSavePAResult" class="save" disabled>บันทึก</button>	  
                <button id="btnConfirmPAResult" class="confirm" disabled>ยืนยัน</button>
                <button id="btnPrintImageResult" class="print"disabled>พิมพ์ไปยังไฟล์</button>          
             </div>
             <label id='lblMessage' style='color:Brown;font-weight:bold' ></label><br />
             <label id='lblMessage2' style='color:Brown;font-weight:bold' ></label>            
             
             <label id='lblToolMessage2' style='color:Brown;font-weight:bold' ></label>
        </div>    
        <div class="grid_8">
            <div class="title rounded"> <asp:Label runat="server" ID="lblNameUser"></asp:Label> - รายงานผลการปฏิบัติงาน สำหรับพนักงานสายวิชาชีพอื่น </div>
            
        </div>
        <div class="clear"></div>
        <!-- ============= input form ============= --> 
        <div class="grid_12"> 
	        <div class="block" id="DIV1">
                <div style='display:none'>
                    <asp:TextBox  runat='server' id='txtUserName'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtConfirmed'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtConfirmDatetime'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtResult'></asp:TextBox>

                    <asp:TextBox  runat='server' id='txtRoundNo'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtCampusID'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtFacultyID'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtDepartmentID'></asp:TextBox>
                </div>	        		    
			    <fieldset class="rounded" >
				    <legend class="rounded"> แบบฟอร์มประเมินฯ</legend>
				    <center>
				    <div id="divHeader" style="background-color: lightblue;">
                        <label class='headform'>รายงานผลการปฏิบัติงาน สำหรับพนักงานสายวิชาชีพอื่น</label><asp:Label  runat='server' class='hidden' ID='lblFormType'></asp:Label>
                        <!--<asp:Label  runat='server' class='headformtype' ID='Label1'>(</asp:Label><select id='drpResult'><option value='0'>ประเมินตนเอง</option><option value='1'>ผลการประเมินจากผู้บังคับบัญชา</option></select><asp:Label  runat='server' class='headformtype' ID='Label2'>)</asp:Label>
                        <div style='float:right; border: 1px solid black;'>โหลดข้อมูลจากปี 
                            <asp:DropDownList runat="server" ID="drpBudgetYear" DataSourceID="dsBudgetYear" DataTextField="round_year" DataValueField="round_year"></asp:DropDownList>
                            <asp:SqlDataSource ID="dsBudgetYear" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                SelectCommand="SELECT ltrim(rtrim(str([sround]) + '/' + [syear])) round_year FROM [tblCFG_Year] ORDER BY [syear], [sround]"></asp:SqlDataSource>
                            <button id="btnLoadBackData" class="open">โหลด</button>
                        </div>-->
                        <br />
                        <label class='headform'>แผนปฏิบัติงาน รอบการประเมินฯที่ <asp:Label runat="server" ID="lblRoundNo"></asp:Label> / <asp:Label runat="server" ID="lblBudgetYear"></asp:Label></label>
                        <br />
                        
                        
                        <table width='100%'>
                            <tr>
                                <td width='10%'></td>
                                <td></td>                             
                                <td class='headtitle'>เลขที่บุคลากร :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblEmployeeID'></asp:Label ></td>
                                <td class='headtitle'>ตำแหน่ง :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblPositionName'></asp:Label ></td>
                                
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>                               
                                <td class='headtitle'>ชื่อ-สกุล :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblFullName'></asp:Label ></td>
                                <td class='headtitle'>ระดับ :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblLevelNo'></asp:Label ></td>                                 
                                <td class='headtitle'>หน่วยงาน :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblDepartmentName'></asp:Label ></td>                                
                            </tr>
                        </table>
                    </div>
                    <div id="divEvaluate">
                        <table id='tblEvaluateFormPA'>
                            <thead>
                                <tr>
                                    <th style='display:nonex' rowspan='3' width='3%' class='thPANo'></th>
                                    <th rowspan='3' width='20%' class='thPADetail'>เป้าหมาย/โครงการ/แผนงาน</th>
                                    <th rowspan='3' width='10%' class='thPADetail'>รายละเอียด</th>
                                    <th rowspan='3' width='4%' class='thPAWeight'>ค่าน้ำหนัก (A)</th>
                                    <th rowspan='3' colspan='2' width='10%' class='thPALoS'>ระดับความสำเร็จ <br>(Level of Success) <br>1 - 4<br>(B)</th>
                                    <th rowspan='3' width='4%' class='thPAScore'>คะแนน <br> A x B <br>(C)</th>
                                    <th rowspan='3' width='4%' class='thPAPercent'>ผลรวม <br>คะแนน % <br>(C) / 4</th>
                                    <th rowspan='3' width='16%' class='thPAComment2'>ความคิดเห็นเพิ่มเติม/ข้อเสนอแนะ<br> ปัญหาอุปสรรค <br>(Comment)</th>
                                    <th rowspan='3' width='4%' class='thPASTG'>รหัสกลยุทธ์ ใส่เฉพาะข้อ 2-3</th>
                                    <th rowspan='3' width='16%' class='thPAComment'>ความคิดเห็นของผู้บังคับบัญชา (Comment)</th>
                                </tr>
                            </thead>
                            <tr class="trPARowTitle" id='trRowNo_1'>
                                <td colspan="3" class='tdPATitle'><label id='lblRowNo_1'>1. งานปฏิบัติการ (Routine Operation)</label></td>
                                <td class='tdPASumWeight'><label class='tdPAValue' id='lblPASumWeight_1'>0</label></td>
                                <td colspan="1" class='tdPASumWeight'><label>ผู้ประเมิน</label></td>
                                <td colspan="1" class='tdPASumWeight'><label>ผู้บังคับบัญชา</label></td>
                                <td colspan="15"></td>
                            </tr>
                            <tbody id="tbodyG_1">
                                
                            </tbody>
                            <tr class="trPARowTitle" id='trRowNo_2'>
                                <td colspan="3" class='tdPATitle'><label id='lblRowNo_2'>2. งานบริหาร (Mangement)</label></td>
                                <td class='tdPASumWeight'><label class='tdPAValue' id='lblPASumWeight_2'>0</label></td>
                                <td colspan="1"></td>
                                <td colspan="1"></td>
                                <td colspan="15"></td>
                            </tr>
                            <tbody id="tbodyG_2"></tbody>
                            <tr class="trPARowTitle" id='trRowNo_3'>
                                <td colspan="3" class='tdPATitle'><label id='lblRowNo_3'>3. งานที่ทำแบบข้ามสายงาน (Cross Function Team)</label></td>
                                <td class='tdPASumWeight'><label class='tdPAValue' id='lblPASumWeight_3'>0</label></td>
                                <td colspan="1"></td>
                                <td colspan="1"></td>
                                <td colspan="15"></td>
                            </tr>
                            <tbody id="tbodyG_3"></tbody>
                            <tr class="trPARowTitle" id='trRowNo_4'>
                                <td colspan="3" class='tdPATitle'><label id='lblRowNo_4'>4. งานที่พัฒนาตนเอง (Self Development)</label></td>
                                <td class='tdPASumWeight'><label class='tdPAValue' id='lblPASumWeight_4'>0</label></td>
                                <td colspan="1"></td>
                                <td colspan="1"></td>
                                <td colspan="15"></td>
                            </tr>
                            <tbody id="tbodyG_4"></tbody>
                            <tr class="trPARowTitle" id='trRowNo_5'>
                                <td colspan="3" class='tdPATitle'><label id='lblRowNo_5'>5. งาน R2R (สำหรับสำนักงานคณะบดี)</label></td>
                                <td class='tdPASumWeight'><label class='tdPAValue' id='lblPASumWeight_5'>0</label></td>
                                <td colspan="1"></td>
                                <td colspan="1"></td>
                                <td colspan="15"></td>
                            </tr>
                            <tbody id="tbodyG_5"></tbody>
                            <tfoot>
                                
                                <tr class="trPARowFoot">
                                    <td colspan="3" class="tdPAFoot">จำนวนรวมทั้งหมดค่าน้ำหนักผลงาน (100)</td>
                                    <td class='tdPASumWeight'><label id='lblPASumWeight'>100</label></td>
                                    <td></td>
                                    <td></td>
                                    <td class='tdPASumScore'><label id='lblPASumScore'>0</label></td>
                                    <td class='tdPASumPercent'><label id='lblPASumPercent'>0.00</label></td>
                                    <td class='tdPASumComment'colspan="4">คะแนน (คะแนนเต็ม 100 คะแนน)</td>
                                </tr>
                                <tr class="trPARowTitleSuggest"><td colspan='10' class='tdPATitle'>Suggestion</td></tr>
                                <tr class="trPARowSuggest">
                                    <td class="tdPASuggest" colspan="20">
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea class='InputPASuggest normal' id='txtSuggest' name="txtSuggest"></textarea>
                                        </label>                                
                                        </div>
                                    </td>
                                </tr>
                            </tfoot>                            
                        </table>
                    </div>
				    </center>
				</fieldset>
            </div>
        </div>
        <div class="clear"></div>       
        
        <div class="grid_12">
        </div>        
        <div class="clear"></div> 
        <div class="grid_12"> 
        </div>
        <div class="clear"></div>       
        <!--  =============== Start Footer ===============   -->
		<div class="grid_12">
			<div class="rounded" id="footer">
				<p>&copy; 2557 <strong>คณะวิศวกรรมศาสตร์ </strong> </p>
			</div>
		</div>
		<div class="clear">
            &nbsp;</div>        
    </div>
    
<div id="EvaluateDetail_divModel" class="reveal-modal">
    <table id='tblEvaluatePopup'>
        <tr class='hidden'><td><label id='lblPOPNo'></label></td></tr>
        <tr>
            <td colspan='3'><b>กิจกรรม/งาน : </b><label id='lblPOPName'></label></td>
        </tr>
        <tr><td colspan='3' height='20px'></td></tr>
        <tr><td colspan='3'><b>รายละเอียด :</b></td></tr>
        <tr>
            <td colspan='3'>
                <div class='inputWrapper'>
                    <textarea  class='normal' id='txtPOPDetail' name='txtPOPDetail'  rows="15" cols="80"></textarea>
                </div>
            </td>
        </tr>
        <tr><td colspan='3' height='20px'></td></tr>
        <tr><td colspan='3'><center><button id='btnPOPUpdate' class='btnDetail'>Update</button> <button id='btnPOPClose' class='btnDetail'>Close</button></center></td></tr>
    </table>
    <a class="close-reveal-modal">&#215;</a>
</div>
    </form>
</body>
</html>
<script src="EvaluateConfig.js?v=1" type="text/javascript"></script>
<script src="frmFormPACommon.js?v=3" type="text/javascript"></script>
<script src="frmEvaluateFormPA.js?v=2" type="text/javascript"></script>
