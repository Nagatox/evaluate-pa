<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEvaluatePart00.aspx.vb" Inherits="Evaluate.frmEvaluatePart00" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Main - Evaluate Project</title>
    <link rel="stylesheet" type="text/css" href="includes/css/main.css"   /> 
    <link rel="stylesheet" type="text/css" href="includes/css/Scroll.css"   />
    <link rel="stylesheet" type="text/css" href="Evaluate.css"   />
    
    <script type="text/javascript" src="includes/js/jquery-1.4.2.min.js"></script> 
    <script type="text/javascript" src="includes/js/jquery.json-2.2.min.js"></script> 
    
    <script type="text/javascript" src="includes/js/jquery-ui-1.8.4.custom.min.js"></script> 
    <script type="text/javascript" src="includes/js/jquery.corner.js"></script> 
    <script type="text/javascript" src="includes/js/i18n/grid.locale-en.js"></script>
    <script type="text/javascript" src="includes/js/header.js"></script>
    
    <script type="text/javascript" src="includes/js/jquery-1.4.2.min.js"></script> 
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
        
    <script type="text/javascript" src="includes/js/InnosoftCustomLib.js"></script>    
    <style type='text/css'>
        .textshowonly         
        {
            width: 120px;
            font-size: 100%;
        }
        .textshowonly2
        {
            width: 120px;
            font-size: 150%;
            padding-left: 5px;
        }
    </style>
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
                <button id="btnConfirm" class="save" >Confirm</button>
                <button id="btnExport" class="new">Export</button>	            
                <button id="btnPrintImage" class="print">พิมพ์ไปยังไฟล์</button>
             </div>
            <label id='lblConfirmMessage' style='color:Brown;font-weight:bold'></label>	                            
        </div>    
        <div class="grid_8">
            <div class="title rounded"><asp:Label runat="server" ID="lblNameUser"></asp:Label> - รอบการประเมินฯที่ <asp:Label runat="server" ID="lblRoundNo"></asp:Label> / <asp:Label runat="server" ID="lblBudgetYear"></asp:Label> </div>
        </div>
        <div class="clear"></div>
        <!-- ============= input form ============= --> 
        <div class="grid_12"> 
	        <div class="block" id="DIV1">
                <div style='display:none'>
                    <asp:TextBox  runat='server' id='txtUserName'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtResult'></asp:TextBox>

                    <asp:TextBox  runat='server' id='txtRoundNo'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtCampusID'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtFacultyID'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtDepartmentID'></asp:TextBox>

                    <!--<asp:TextBox  runat='server' id='txtLevel'></asp:TextBox>-->
                </div>	        		    
			    <fieldset class="rounded" >
				    <legend class="rounded">ส่วนที่ 1 ข้อมูลทั่วไปเกี่ยวกับพนักงาน</legend>
                    <div>
                        <table>
                            <tr>
                                <td class='headtitle_no'>1.</td>
                                <td class='headtitlelonglong'>ชื่อ-สกุล</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtFullName' disabled="disabled" /></td>
                                <td class='headtitle'>ตำแหน่ง</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtPosition' disabled="disabled" /></td>
                                <td class='headtitle'>เลขที่</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtEmployeeID' disabled="disabled" /></td>
                                <td class='headtitle'>สังกัด</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtDepartmentName' disabled="disabled" /></td>
                            </tr>
                            <tr>
                                <td class='headtitle_no'>2.</td>
                                <td class='headtitle'>เริ่มปฏิบัติงานเมื่อ </td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtStartWorkDate' disabled="disabled" /></td>
                                <td class='headtitle'>รวมระยะเวลาปฏิบัติงาน</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtTotalWorkDay' disabled="disabled" /></td>
                                <td class='headtitle'>ระดับชั้น</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtLevel' disabled="disabled" /></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                            </tr>
                            <tr>
                                <td class='headtitle_no'>3.</td>
                                <td class='headtitle'>สถานะการลาศึกษาต่อ </td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveStudyStatus' disabled="disabled" /></td>
                                <td class='headtitle'>ตั้งแต่วันที่</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveStudyStartDate' disabled="disabled" /></td>
                                <td class='headtitle'>ถึงวันที่</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveStudyEndDate' disabled="disabled" /></td>
                                <td class='headtitle'>กลับเข้าปฏิบัติงานวันที่</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveStudyBackDate' disabled="disabled" /></td>
                            </tr>
                            <tr>
                                <td class='headtitle_no'>4.</td>
                                <td class='headtitle'>เวลาปฏิบัติงานในรอบปี<!--งบประมาณที่ผานมา--> </td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtWorkingDayInYear' disabled="disabled" /></td>
                                <td class='headtitle' colspan='2'><input type='button' id='btnPart01S04' value='โดยมีวันหยุดงาน ดังนี้' /></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                            </tr>
                            <tr class='trPart01S04'>
                                <td class='headtitle_no'></td>
                                <td colspan='8'>
                                    <table id='tblPart01S04'>
                                        <tr class='trPart01S04'>
                                            <td class='headtitle'>ลาป่วย</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveSick' disabled="disabled" /></td>
                                            <td class='headtitle'>ลาป่วยมีใบรับรองแพทย์</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveSickGuarantee' disabled="disabled" /></td>
                                            <td class='headtitle'>ลากิจ</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveCarer' disabled="disabled" /></td>
                                            <td class='headtitle'>มาสาย</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLate' disabled="disabled" /></td>
                                        </tr>
                                        <tr class='trPart01S04'>
                                            <td class='headtitle'>ลาพักผ่อน</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveVacation' disabled="disabled" /></td>
                                            <td class='headtitle'>ลาคลอดบุตร</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveMaternity' disabled="disabled" /></td>
                                            <td class='headtitle'>ลาอุปสมบท</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLeaveOrdination' disabled="disabled" /></td>
                                            <td class='headtitle'>ขาดราชการ</td>
                                            <td class='headvalue'><input type='text' class='textshowonly' id='txtLackAgenturer' disabled="disabled" /></td>
                                        </tr>                                    
                                    </table>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class='headtitle_no'>5.</td>
                                <td class='headtitle'>การเลื่อนขั้นเงินเดือน ปีงบประมาณ</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtPromoted' disabled="disabled" /></td>
                                <td class='headtitle'>% การเลื่อนขั้น</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtPromotedPercent' disabled="disabled" /></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                            </tr>
                            <tr>
                                <td class='headtitle_no'>6.</td>
                                <td class='headtitle'>เคยถูกลงโทษทางวินัย เมื่อ</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtDisciplinaryDate' disabled="disabled" /></td>
                                <td class='headtitle'>ระดับโทษที่ได้รับ</td>
                                <td class='headvalue'><input type='text' class='textshowonly' id='txtDisciplinary' disabled="disabled" /></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                                <td class='headtitle'></td>
                                <td class='headvalue'></td>
                            </tr>
                        </table>
                    </div>				    
				    
				</fieldset>
				<fieldset class="rounded" >
				    <legend class="rounded">ส่วนที่ 2 แบบฟอร์มการประเมินผลการปฏิบัติงาน </legend>
                    <a href='frmEvaluateForm.aspx' id='ahrefPart0201'><input type='button' class='a_button' id='btnPart0201' value='เข้าสู่แบบฟอร์ม 2.1 (การประเมินผลงาน)' /></a>
				    <label id='lblPart0201Message'>ผู้ใช้ยังไม่มีการบันทึก</label>
                    <center>
				    <div id="divMainPart0201" >
                        <table id='tblPart02' style="margin-bottom: 0px; border-bottom:none;">
                            <thead>
                                <tr>
                                    <td rowspan='3' colspan='10'>
                                        <b>สรุปผลการประเมินส่วนที่ 2.1</b>
                                    </td>
                                    <td class='tdFootScore' colspan='7' align='right'>ผมรวมคะแนน AxB (C)</td>
                                    <td class='tdFootScore' align='right'><label class='tdValueX' id='lblSumSuccess'>0</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='hidden' colspan='6' align='right'></td>
                                    <td class='hidden' align='right'><label class='tdValue' id='lblFullPLx'></label></td>
                                    <td class='tdFootScore' colspan='7' align='right'>ผมรวมคะแนน % (C)/4</td>
                                    <td class='tdFootScore' align='right'><label class='tdValueX' id='lblSumPercent'>0.00</label></td>
                                    <td></td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan='10'>สรุประดับชั้นความสามารถ (ความคิดเห็น)</td>
                                    <td colspan='9'>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtSuggestion' name="txtSuggestion" rows='5' disabled ></textarea>
                                        </label>                                
                                        </div>                                    
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <!--<table id='tblSuggestionForm' style="margin-bottom: 0px; border-bottom:none;">
                            <thead>
                                <tr>
                                    <td rowspan='3' colspan='10'>
                                        <b>สรุปผลการประเมินส่วนที่ 2.1</b>
                                    </td>
                                    <td class='tdFootSuccessScore' colspan='7' align='right'>รวมคะแนนระดับความสำเร็จ</td>
                                    <td class='tdFootSuccessScore' align='right'><label class='tdValueX' id='lblSumSuccess'>0.0</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='hidden' colspan='6' align='right'></td>
                                    <td class='hidden' align='right'><label class='tdValue' id='lblFullPLx'></label></td>
                                    <td class='tdFootSuccessScore' colspan='7' align='right'>คะแนนเต็ม</td>
                                    <td class='tdFootSuccessScore' align='right'><label class='tdValueX' id='lblFullSuccess'>0.0</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='hidden' colspan='6' align='right'></td>
                                    <td class='hidden' align='right'><label class='tdValueX' id='lblPercentPLx'></label></td>
                                    <td class='tdFootSuccessScore' colspan='7' align='right'>คิดเป็นร้อยละ</td>
                                    <td class='tdFootSuccessScore' align='right'><label class='tdValueX' id='lblPercentSuccess'>0.00</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFootScore' colspan='2' align='right'>ถ้าคิดเป็นคะแนนเต็ม</td>
                                    <td class='tdFootScore' align='right'><label class='tdValueX' id='lblIFFull'>70</label></td>
                                    <td class='tdFootScore' colspan='6' align='right'></td>
                                    <td class='tdFootScore' align='right'></td>
                                    <td class='tdFootScore' colspan='7' align='right'>คะแนนจากระดับความสำเร็จ</td>
                                    <td class='tdFootScore' align='right'><label class='tdValueX' id='lblIFSuccess'>0.00</label></td>
                                    <td></td>
                                </tr>
                             </thead>
                          </table>
                          <table id='tblSuggestionForm'>
                             <thead>
                                <tr>
                                    <th>ระดับชั้นความสามารถ (PL)</th>
                                    <th>PL1</th>
                                    <th>PL2</th>
                                    <th>PL3</th>
                                    <th>PL4</th>
                                    <th>PL5</th>
                                    <th>สรุประดับชั้นความสามารถ (ความคิดเห็น)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>คาดหวัง</td>
                                    <td><label class='tdValue' id='lblExpectPL1'></label></td>
                                    <td><label class='tdValue' id='lblExpectPL2'></label></td>
                                    <td><label class='tdValue' id='lblExpectPL3'></label></td>
                                    <td><label class='tdValue' id='lblExpectPL4'></label></td>
                                    <td><label class='tdValue' id='lblExpectPL5'></label></td>
                                    <td rowspan='3'>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtSuggestion' name="txtSuggestion" rows='5' disabled ></textarea>
                                        </label>                                
                                        </div>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>ประเมินได้</td>
                                    <td><label class='tdValue' id='lblEvaluatePL1'></label></td>
                                    <td><label class='tdValue' id='lblEvaluatePL2'></label></td>
                                    <td><label class='tdValue' id='lblEvaluatePL3'></label></td>
                                    <td><label class='tdValue' id='lblEvaluatePL4'></label></td>
                                    <td><label class='tdValue' id='lblEvaluatePL5'></label></td>
                                </tr>
                                <tr>
                                    <td colspan='6'></td>
                                </tr>                                                                
                            </tbody>
                        </table>-->
                    </div>
                    </center>
                    <a href='frmEvaluatePart02.aspx' id='ahrefPart0202'><input type='button' class='a_button' id='btnPart0202' value='เข้าสู่แบบฟอร์ม 2.2 (การประเมินคุณลักษณะการปฏิบัติงาน)' /></a>
				    <label id='lblPart0202Message'>ผู้ใช้ยังไม่มีการบันทึก</label>
                    <center>
                    <div id="divMainPart0202">
                        <table id='tblPart02' style="margin-bottom: 0px; border-bottom:none;">
                            <tbody>
                                <tr>
                                    <td  style='text-align:left;width:50%'>
                                        <b>สรุปผลการประเมินส่วนที่ 2.2</b>
                                    </td>
                                    <td colspan='4' class='tdFootScore' style='text-align:left;width:50%'>
                                        <ul style='list-style:none outside none;border: 1px solid black;'>
                                        <li style='float: left;display: block;border: 1px solid black;width:90%;'>
                                        <div class='divSumScore25'>
                                            รวมคะแนนในข้อ 2,3,4 และ 5 = <b><u><label id='lblSumEmpScore25'>0</label></u></b> คะแนน  - ประเมินตนเอง<br />
                                        </div>
                                        รวมคะแนน <b><u><label id='lblSumEmpScore'></label></u></b> คะแนน คิดเป็น <b><u><label id='lblSumEmpScore100'></label> %</u></b> (คะแนนเต็ม 100 คะแนน) - ประเมินตนเอง<br />
                                        </li>
                                        <li style='float: left;display: block;border: 1px solid black;width:90%;'>
                                        <div class='divSumScore25'>
                                            รวมคะแนนในข้อ 2,3,4 และ 5 = <b><u><label id='lblSumScore25'>0</label></u></b> คะแนน - ผู้บังคับบัญชา<br />
                                        </div>
                                        รวมคะแนน <b><u><label id='lblSumScore'></label></u></b> คะแนน คิดเป็น <b><u><label id='lblSumScore100'></label> %</u></b> (คะแนนเต็ม 100 คะแนน) - ผู้บังคับบัญชา<br />
                                        </li>
                                        <li style='float: left;display: block;border: 1px solid black;width:90%;'>
                                        <center>
                                        <input type='checkbox' id='chkPass' disabled /> ผ่าน
                                        <input type='checkbox' id='chkNotPass' disabled /> ไม่ผ่าน
                                        </center>
                                        </li>
                                        </ul>
                                    </td>
                                </tr>
                            </tbody>                           
                        </table>
                    </div>
				    				    
				</fieldset>
				<fieldset class="rounded" >
				    <legend class="rounded">ส่วนที่ 3 แบบฟอร์มการพัฒนาและฝึกอบรม </legend>
				    <a href='frmEvaluatePart03.aspx' id='ahrefPart03'><input type='button' class='a_button' id='btnPart03' value='เข้าสู่แบบฟอร์ม (3) การพัฒนาและฝึกอบรม' /></a>
				    <label id='lblPart03Message'>ผู้ใช้ยังไม่มีการบันทึก</label><br />
				    <div id="divMainPart03" >
				        <table id='tblPart03'>
				            <thead>
				                <tr>
				                    <th>พนักงานกรอก</th>
				                    <th>ผู้บังคับบัญชาให้ความเห็น</th>
				                </tr>
				            </thead>
				            <tbody>
				                <tr>
				                    <td id='tdPart03Employee'></td>
				                    <td id='tdPart03Commander'></td>
				                </tr>
				            </tbody>
				        </table>
				    </div>
				    
				</fieldset>
				<fieldset class="rounded" >
				    <legend class="rounded">ส่วนที่ 4 สรุปผลการประเมิน</legend>
				    <table id='tblPart0401'>
				        <tr>
				            <td colspan='9'><b><u>รวมคะแนนการประเมิน</u></b></td>
				        </tr>
                        <tr>
                            <td class='headtitlex'>ส่วนที่ 2.1.</td>
                            <td class='headtitlelong'>ประเมินผลงานเชิงปริมาณ&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04_0201' value='0' disabled="disabled" /></td>
                            <td class='headtitle'>คิดเป็น&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04_0201Score' value='0.00' disabled="disabled" /></td>
                            <td class='headvalue'>คะแนน (<label id='lblPart04_0201Percent' ></label>%)</td>
                            <td class='headvalue'></td>
                        </tr>
                        <tr class="hidden">
                            <td class='headtitlex'></td>
                            <td class='headtitlelong'>เชิงคุณภาพ&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04_0201X' value='0' disabled="disabled" /></td>
                            <td class='headtitle'>คิดเป็น&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04_0201XScore' value='0.00' disabled="disabled" /></td>
                            <td class='headvalue'>คะแนน (<label id='lblPart04_0201PercentRemain' ></label>%)</td>
                            <td class='headvalue'></td>
                        </tr>                        
                        <tr>
                            <td class='headtitlex'>ส่วนที่ 2.2.</td>
                            <td class='headtitlelong'>ประเมินคุณลักษณะในการปฏิบัติงาน และคุณสมบัติเฉพาะตัว&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04_0202' value='0' disabled="disabled" /></td>
                            <td class='headtitle'>คิดเป็น&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04_0202Score' value='0.00' disabled="disabled" /></td>
                            <td class='headvalue'>คะแนน (<label id='lblPart04_0202Percent' ></label>%)</td>
                            <td class='headvalue'></td>
                        </tr>

                        <tr>
                            <td class='headtitlex'></td>
                            <td class='headtitlelong'></td>
                            <td class='headvalue'></td>
                            <td class='headtitle'><b>รวม</b>&nbsp;&nbsp;</td>
                            <td class='headvalue'><input type='text' class='textshowonly2' id='txtPart04Score' value='0.00' disabled="disabled" /></td>
                            <td class='headvalue'>คะแนน (100%)</td>
                            <td class='headvalue'></td>
                        </tr>
                        <tr>
				            <td colspan='7'>&nbsp;</td>
				        </tr>
				    </table>
				    <table id='tblPart0402'>    		
                        <tr>
                            <td class='headtitlex'><b>สรุป</b></td>
                            <td class='headtitle'></td>
                            <td class='headvalue'><input type='checkbox' id='chkPart04Pass' disabled='disabled' /> ผ่านการประเมิน </td>
                            <td class='hidden'>เห็นสมควรให้ชึ้นเงินเดือน <input type='text' class='textshowonly2' id='txtPart04UpPercent' disabled='disabled' /> %</td>
                            <td class='hidden'>ผลงานระดับ <input type='text' class='textshowonly2' id='txtPart04Level' disabled="disabled" /></td>
                        </tr>
                        <tr>
                            <td class='headtitlex'></td>
                            <td class='headtitle'></td>
                            <td class='headvalue'><input type='checkbox' id='chkPart04NotPass' disabled='disabled' /> ไม่ผ่านการประเมิน</td>
                            <td class='hidden'>ไม่สมควรให้ชึ้นเงินเดือน</td>
                            <td class='hidden' style='text-align:right'></td>
                        </tr>
				    </table>
				    <table id='Table1' style='width:100%'> 
				        <tr>
                            <td class='headtitlex'></td>
				            <td class='headvalue' style='text-align:right'><input type='button' id='btnPart04' value='บันทึก' class='save' /></td>
                        </tr>
				    </table>
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
    </form>
</body>
</html>
<script src="EvaluateConfig.js?v=1" type="text/javascript"></script>
<script src="frmEvaluatePart00.js?v=21" type="text/javascript"></script>
