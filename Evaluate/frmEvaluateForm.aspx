<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEvaluateForm.aspx.vb" Inherits="Evaluate.frmEvaluateForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Evaluate Form - Evaluate Project</title>
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
        <div id="header" class="grid_10">
            <img id='imglogo' src="images/EvaluateBanner.png" alt="Evaluate" width="40px" height="50px"/>
            <label id='namesystem'>งานบุคคล</label>
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
                <button id="btnSave" class="save">บันทึก</button>	  
                <button id="btnExport" class="new">Export</button>
                <button id="btnConfirm" class="save">ยืนยัน</button>
                <button id="btnPrintImage" class="save">พิมพ์ไปยังไฟล์</button>          
                <button id="btnOpen" class="open" style="visibility:hidden">Open</button>
                <button id="btnDelete" class="delete" style="visibility:hidden">Delete</button>
                	   
             </div>
             <label id='lblMessage' style='color:Brown;font-weight:bold' ></label>            
             <label id='lblToolMessage' style='color:Brown;font-weight:bold' ></label>
        </div>    
        <div class="grid_8">
            <div class="title rounded"> <asp:Label runat="server" ID="lblNameUser"></asp:Label> - แบบฟอร์มประเมินฯ รอบการประเมินฯที่ <asp:Label runat="server" ID="lblRoundNo"></asp:Label> / <asp:Label runat="server" ID="lblBudgetYear"></asp:Label> </div>
            
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
				    <div id="divHeader">
                        <label class='headform'>แบบประเมินผลการปฏิบัติงานตาม Proficiency Level</label><asp:Label  runat='server' class='hidden' ID='lblFormType'></asp:Label>
                        <asp:Label  runat='server' class='headformtype' ID='Label1'>(</asp:Label><select id='drpResult'><option value='0'>ประเมินตนเอง</option><option value='1'>ผลการประเมินจากผู้บังคับบัญชา</option></select><asp:Label  runat='server' class='headformtype' ID='Label2'>)</asp:Label>
                        <div style='float:right; border: 1px solid black;'>โหลดข้อมูลจากปี 
                            <asp:DropDownList runat="server" ID="drpBudgetYear" DataSourceID="dsBudgetYear" DataTextField="round_year" DataValueField="round_year"></asp:DropDownList>
                            <asp:SqlDataSource ID="dsBudgetYear" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                                SelectCommand="SELECT ltrim(rtrim(str([sround]) + '/' + [syear])) round_year FROM [tblCFG_Year] ORDER BY [syear], [sround]"></asp:SqlDataSource>
                            <button id="btnLoadBackData" class="open">โหลด</button>
                        </div>
                        <br /><br />
                        
                        
                        <table width='100%'>
                            <tr>
                                <td width='10%'></td>
                                <td></td>                             
                                <td class='headtitle'>เลขที่บุคลากร :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblEmployeeID'></asp:Label ></td>
                                <td class='headtitle'>ตำแหน่ง :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'><asp:Label runat="server" id='lblPositionName'></asp:Label ></td>
                                <td class='headtitle'>รอบการประเมินฯ :&nbsp&nbsp&nbsp </td>
                                <td class='headvalue'>
                                    <asp:Label runat="server" id='lblRoundNo2'></asp:Label> / <label id='lblRoundYear'></label>
                                    <!--<asp:DropDownList ID="drpRoundNo" runat="server">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                    </asp:DropDownList> / <label id='lblRoundYear'></label>-->
                                </td>
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
                        <table id='tblEvaluateForm'>
                            <thead>
                                <tr>
                                    <th style='display:nonex' rowspan='3' width='3%' class='thOrder'></th>
                                    <th rowspan='3' width='32%' class='thDetail'>กิจกรรม/งาน</th>
                                    <th rowspan='3' width='10%' class='thDetail'>รายละเอียด</th>
                                    <th rowspan='3' width='5%' class='thWeight'>น้ำหนัก % (A)</th>
                                    <th rowspan='1' width='18%' colspan='6' class='thPLLevel'>ระดับความชำนาญการ (PL)</th>
                                    <th rowspan='3' width='5%' class='thPLScore'>คะแนน PL Cx(A/B)</th>
                                    <th rowspan='2' width='21%' colspan='8' class='thSuccessLevel'>ระดับความสำเร็จ (E)</th>
                                    <th rowspan='3' width='5%' class='thSuccessScore'>คะแนน (D)x(E)</th>
                                    <th rowspan='3' ></th>
                                </tr>
                                <tr>
                                    <th rowspan='2' width='3%' class='thPLLevel'>คาดหวัง (B)</th>
                                    <th rowspan='1' colspan='5' width='15%' class='thPLLevel'>ประเมินได้ (C)</th>
                                </tr>
                                <tr>
                                    <th rowspan='1' width='3%' class='thPLLevel'>PL1</th>
                                    <th rowspan='1' width='3%' class='thPLLevel'>PL2</th>
                                    <th rowspan='1' width='3%' class='thPLLevel'>PL3</th>
                                    <th rowspan='1' width='3%' class='thPLLevel'>PL4</th>
                                    <th rowspan='1' width='3%' class='thPLLevel'>PL5</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>1</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>1.5</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>2</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>2.5</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>3</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>3.5</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>4</th>
                                    <th rowspan='1' width='3%' class='thSuccessLevel'>อื่นๆ</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id='trRowNo_1'>
                                    <td style='display:nonex'><label class='tdValue' id='lblRowNo_1'>1</label></td>
                                    <td>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtDetail_1' name="txtDetail_1"></textarea>
                                        </label>                                
                                        </div>
                                    </td>
                                    <td>
                                        <div class='inputWrapper'>
                                        <label>
                                            <input type='text' id='txtWeight_1' />
                                            <!--<textarea class='normal' id="txtWeight_1" name="txtWeight_1"></textarea>-->
                                        </label>
                                        </div>                                
                                    </td>
                                    
                                    <td>
                                        <select id='selExpect_1'>
                                            <option value='1'>1</option>
                                            <option value='2'>2</option>
                                            <option value='3'>3</option>
                                            <option value='4'>4</option>
                                            <option value='5'>5</option>
                                        </select>
                                        <!--<div class='inputWrapper'>
                                        <label>
                                            <textarea class='normal' name="txtexcept_1"></textarea>
                                        </label>                                
                                        </div>-->
                                    </td>
                                    <td class='tdPLLevel'><input type='radio' id='radPL_1' name='radPL_1' value='1' /></td>
                                    <td class='tdPLLevel'><input type='radio' id='radPL_1' name='radPL_1' value='2' /></td>
                                    <td class='tdPLLevel'><input type='radio' id='radPL_1' name='radPL_1' value='3' /></td>
                                    <td class='tdPLLevel'><input type='radio' id='radPL_1' name='radPL_1' value='4' /></td>
                                    <td class='tdPLLevel'><input type='radio' id='radPL_1' name='radPL_1' value='5' /></td>
                                    <td class='tdPLScore'><label class='tdValue' id='lblPLScore_1'></label></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='1' /></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='2' /></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='3' /></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='4' /></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='5' /></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='6' /></td>
                                    <td class='tdSuccessLevel'><input type='radio' id='radSuccess_1' name='radSuccess_1' value='7' /></td>
                                    <td class='tdSuccessScore'><label class='tdValue' id='lblSuccessScore_1'></label></td>
                                    <td></td>
                                </tr>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td style='display:nonex; text-align:center;'><button id='btnAddRow'>+</button></td>
                                    <td align='right' colspan='2'>รวม</td>
                                    <td align='right'><label class='tdValue' id='lblSumWeight'>0.00</label></td>
                                    <td class='tdFootPLScore' colspan='6' align='right'>รวมคะแนน PL</td>
                                    <td class='tdFootPLScore' align='right'><label class='tdValue' id='lblSumPL'>0.0</label></td>
                                    <td class='tdFootSuccessScore' colspan='8' align='right'>รวมคะแนนระดับความสำเร็จ</td>
                                    <td class='tdFootSuccessScore' align='right'><label class='tdValue' id='lblSumSuccess'>0.0</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style='display:nonex' rowspan='3'></td>
                                    <td class='tdFootPLLevel' rowspan='2' colspan='10' align='center'><label id='lblLevelMessage'></label></td>
                                    <td class='hidden' colspan='6' align='right'></td>
                                    <td class='hidden' align='right'><label class='tdValue' id='lblFullPLx'></label></td>
                                    <td class='tdFootSuccessScore' colspan='8' align='right'>คะแนนเต็ม</td>
                                    <td class='tdFootSuccessScore' align='right'><label class='tdValue' id='lblFullSuccess'>0.0</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='hidden' colspan='6' align='right'></td>
                                    <td class='hidden' align='right'><label class='tdValue' id='lblPercentPLx'></label></td>
                                    <td class='tdFootSuccessScore' colspan='8' align='right'>คิดเป็นร้อยละ</td>
                                    <td class='tdFootSuccessScore' align='right'><label class='tdValue' id='lblPercentSuccess'>0.00</label></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFootScore' colspan='2' align='right'>ถ้าคิดเป็นคะแนนเต็ม</td>
                                    <td class='tdFootScore' align='right'><label class='tdValue' id='lblIFFull'>70</label></td>
                                    <td class='tdFootScore' colspan='6' align='right'></td>
                                    <td class='tdFootScore' align='right'></td>
                                    <td class='tdFootScore' colspan='8' align='right'>คะแนนจากระดับความสำเร็จ</td>
                                    <td class='tdFootScore' align='right'><label class='tdValue' id='lblIFSuccess'>0.00</label></td>
                                    <td></td>
                                </tr>                                
                            </tfoot>                            
                        </table>
                        
                        <table id='tblSuggestionForm'>
                            <thead>
                                <tr>
                                    <th>ระดับความชำนาญการ (PL)</th>
                                    <th>PL1</th>
                                    <th>PL2</th>
                                    <th>PL3</th>
                                    <th>PL4</th>
                                    <th>PL5</th>
                                    <th>สรุประดับความชำนาญการ (ความคิดเห็นจากผู้บังคับบัญชา)</th>
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
                                            <textarea  class='normal' id='txtSuggestion' name="txtSuggestion" rows='5'></textarea>
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
                        </table>
                        <table id='tblNote'>
                            <tr>
                                <td>
                                    <label><u>ผลงานประกอบโดยดูจาก KRA/KPI</u></label>
                                    <ul>
                                        <li>4   = ประสิทธิภาพดีเยี่ยม ควรเป็นแบบอย่าง</li>
                                        <li>3.5 = ประสิทธิภาพดีมาก</li>
                                        <li>3   = ประสิทธิภาพดี</li>
                                        <li>2.5 = ประสิทธิภาพพอใช้ ต้องปรับปรุง</li>
                                        <li>2   = ต้งการปรับปรุงพัฒนา</li>
                                        <li>1.5 = ต้งการปรับปรุงพัฒนาอย่างมาก</li>
                                        <li>1   = ต้งการปรับปรุงพัฒนาอย่างเร่งด่วน</li>
                                    </ul>
                                </td>
                                <td>
                                    <table id='tblNoteRight'>
                                        <tr>
                                            <th colspan='3'>ระดับความสามารถ</th>
                                            <!--<th>สายสนับสนุน</th>-->
                                        </tr>
                                        <tr>
                                            <td>PL1</td>
                                            <td>Basic</td>
                                            <td>รู้และเข้าใจ อธิบาย</td>
                                            <!--<td>จ1 พ1 สว1</td>-->
                                        </tr>
                                        <tr>
                                            <td>PL2</td>
                                            <td>Doing</td>
                                            <td>จัดทำ ปฏิบัติตามกรอบที่กำหนด อธิบาย ตรรวจสอบได้</td>
                                            <!--<td>จ2 พ2 สว2</td>-->
                                        </tr>
                                        <tr>
                                            <td>PL3</td>
                                            <td>Developing</td>
                                            <td>วิเคราห์ ปรับปรุง เสนอแนวทาง แนะนำ แก้ไขปัญหา</td>
                                            <!--<td>จ3 พ3 สว3-4</td>-->
                                        </tr>
                                        <tr>
                                            <td>PL4</td>
                                            <td>Advanced</td>
                                            <td>วางแผน เสนอแนะ ติดดาม พํฒนา ประเมินผล</td>
                                            <!--<td>จ4 พ4 บ1 บ2 สว5</td>-->
                                        </tr>
                                        <tr>
                                            <td>PL5</td>
                                            <td>Expert</td>
                                            <td>กำหนด เปรียบเทียบ คาดการณ์ กระตุ้นผลักดัน เป็นตัวอย่าง</td>
                                            <!--<td>บ3 บ4</td>-->
                                        </tr>
                                    </table>
                                </td>
                            </tr>
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
        <tr><td colspan='3'><center><button id='btnPOPUpdate' class='btnDetail'>OK</button></center></td></tr>
    </table>
    <a class="close-reveal-modal">&#215;</a>
</div>
    </form>
</body>
</html>
<script src="EvaluateConfig.js?v=1" type="text/javascript"></script>
<script src="frmEvaluateForm.js?v=21" type="text/javascript"></script>
