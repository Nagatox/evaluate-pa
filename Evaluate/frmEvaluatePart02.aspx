<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEvaluatePart02.aspx.vb" Inherits="Evaluate.frmEvaluatePart02" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Part 2.2 Evaluation Form - Evaluate Project</title>
    <link rel="stylesheet" type="text/css" href="includes/css/main.css"   /> 
    <link rel="stylesheet" type="text/css" href="includes/css/Scroll.css"   />
    <link rel="stylesheet" type="text/css" href="Evaluate.css"   />
    
    <script type="text/javascript" src="includes/js/jquery-1.10.2.min.js"></script>
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
                <button id="btnPrintImage" class="print">พิมพ์ไปยังไฟล์</button>
                <button id="btnNew" class="new" style="visibility:hidden">New</button>
                <button id="btnOpen" class="open" style="visibility:hidden">Open</button>
                <button id="btnDelete" class="delete" style="visibility:hidden">Delete</button>	   
             </div>
             <label id='lblConfirmMessage' style='color:Brown;font-weight:bold'></label>
        </div>    
        <div class="grid_8">
            <div class="title rounded"><asp:Label runat="server" ID="lblNameUser"></asp:Label> - รอบการประเมินฯที่ <asp:Label runat="server" ID="lblRoundNo"></asp:Label> / <asp:Label runat="server" ID="lblBudgetYear"></asp:Label></div>
        </div>
        <div class="clear"></div>
        <!-- ============= input form ============= --> 
        <div class="grid_12"> 
	        <div class="block" id="DIV1">
	            <div style='display:none'>
	                <asp:TextBox  runat='server' id='txtUserName'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtResult'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtLevelManager'></asp:TextBox>

                    <asp:TextBox  runat='server' id='txtRoundNo'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtCampusID'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtFacultyID'></asp:TextBox>
                    <asp:TextBox  runat='server' id='txtDepartmentID'></asp:TextBox>
                </div>			    
			    <fieldset class="rounded" >
				    <legend class="rounded"> 2.2 แบบประเมินคุณลักษณะการปฏิบัติงานและคุณสมบัติเฉพาะตัว </legend>
                    			    
				    <div id="divCommanderForm">
                        <table id='tblFormPart0202' class='classCommanderForm'>
                            <thead>
                                <tr class='trCommanderForm'>
                                    <th class='thFormNo'></th>
                                    <th class='thFormDetail'>องค์ประกอบและรายละเอียดประกอบการพิจารณา</th>
                                    <th class='thFormScore'>ระดับคะแนน</th>
                                    <th class='thFormScoreA'>คะแนนที่ได้ (พนักงานกรอก)</th>
                                    <th class='thFormScoreB'>คะแนนที่ได้ (ผู้บังคับบัญชา)</th>
                                    <th class='thFormDetailA'>ความสามารถ/ทักษะดีเด่น และที่ปรับปรุงได้อีก</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class='trCommanderForm'>
                                    <th class='thbFormNo'></th>
                                    <th class='thbFormDetail'>หมวดที่ 1 คุณลักษณะในการปฏิบัติงาน</th>
                                    <th class='thbFormScore'></th>
                                    <th class='thbFormScoreA'></th>
                                    <th class='thbFormDetailA'></th>
                                </tr>
                                <tr>
                                    <td class='tdmFormNo' rowspan='5'>1</td>
                                    <td class='tdmFormDetail'><b><u>การพัฒนาอย่างต่อเนื่องและความคิดริเริ่ม</u></b> การนำเอาแนวความคิดหรือแนวปฏิบัติใหม่ฯ มาใช้อย่างรอบคอบ และเป็นระบบเพื่อเพิ่มผลิตภาพของหน่วยงานและการริเริ่มทำงานที่ตนรับผิดชอบด้วยตนเอง</td>
                                    <td class='tdmFormScore'></td>
                                    <td class='tdmFormScoreA' rowspan='5'>
                                        <select id='drpScore01' >
                                            <option value='10' selected>10</option>
                                            <option value='9'>9</option>
                                            <option value='8'>8</option>
                                            <option value='7'>7</option>
                                            <option value='6'>6</option>
                                            <option value='5'>5</option>
                                            <option value='4'>4</option>
                                            <option value='3'>3</option>
                                            <option value='2'>2</option>
                                            <option value='1'>1</option>
                                            <option value='0'>0</option>
                                        </select>
                                    </td>
                                    <td class='tdmFormDetail' rowspan='5'>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtSuggestion01' name="txtSuggestion01" rows='15'></textarea>
                                        </label>                                
                                        </div>                                     
                                    </td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- สามารถพัฒนาการทำงานได้ดีมากและเป็นผู้ริเริ่ม หรือมีส่วนริเริ่มลูงในการปฏิบัติงานทั้งที่เป็นงานเดิมและงานใหม่ ทำให้เกิดผลในการเพิ่มผลิตภาพของสำนักงานได้เป็นอย่างดีมาก</td>
                                    <td class='tdFormScore'>8-10</td>
                                    <td></td>
                                </tr> 
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- สามารถพัฒนาการทำงานได้และมีส่วนริเริ่มและสนับสนุน การปฏิบัติงานเดิมและแนวใหม่ทำให้เกิดผลดีและเพิ่มผลผลิตของสำนักงานได้อย่างดี</td>
                                    <td class='tdFormScore'>6-7</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- สามารถพัฒนาการทำงานได้บางในเรื่อง ยอมรับการปฏิบัติงานในแนวใหม่ได้บ้าง มีส่วนริเริ่มไม่มากนัก และไม่ค่อยสามารถติดหรือปฏิบัติงานในแนวใหม่ได้</td>
                                    <td class='tdFormScore'>4-5</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- ไม่มีความคิดริเริ่ม ไม่สามารถพัฒนางานได้ ไม่อาจดำเนินงานในแนวใหม่ได้ ไม่สามารถร่วมคิด หรือดำเนินงานในแนวใหม่ๆได้</td>
                                    <td class='tdFormScore'>0-3</td>
                                    <td></td>
                                </tr>
                                
                                <!-- -->
                                <tr>
                                    <td class='tdmFormNo' rowspan='5'>2</td>
                                    <td class='tdmFormDetail'><b><u>การแก้ปัญาหา และการตัดสินใจ</u></b> การพยายามแก้ปัญหา โดยมองทั้งในด้านกว้างและด้านลึก มีการตัดสินใจรอบคอบและมีการวางแนวความคิดอย่างมีระบบ แต่ทันการ</td>
                                    <td class='tdmFormScore'></td>
                                    <td class='tdmFormScoreA' rowspan='5'>
                                        <select id='drpScore02' >
                                            <option value='10' selected>10</option>
                                            <option value='9'>9</option>
                                            <option value='8'>8</option>
                                            <option value='7'>7</option>
                                            <option value='6'>6</option>
                                            <option value='5'>5</option>
                                            <option value='4'>4</option>
                                            <option value='3'>3</option>
                                            <option value='2'>2</option>
                                            <option value='1'>1</option>
                                            <option value='0'>0</option>
                                        </select>
                                    </td>
                                    <td class='tdmFormDetail' rowspan='5'>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtSuggestion02' name="txtSuggestion02" rows='12'></textarea>
                                        </label>                                
                                        </div>                                     
                                    </td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- เข้าใจปัญหาได้ลึกซึ้งดีมาก สามารถตัดสินใจเด็ดขาด และมีการวางแนวความคิดอย่างมีระบบทันการ และมีวิจารณญาณที่ดี</td>
                                    <td class='tdFormScore'>8-10</td>
                                    <td></td>
                                </tr> 
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- เข้าใจปัญหาดี ตันสินใจได้ดีโดยส่วนใหญ่ แต่ยังขาดความรอบคอบในการตัดสินใจและการวางแนวความคิดอย่างมีระบบ</td>
                                    <td class='tdFormScore'>6-7</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- ไม่เข้าใจปัญหาดีนัก ตัดสินใจค่อนข้างช้า และการวางแนวความคิดยังไม่ค่อยมีระบบดีนัก</td>
                                    <td class='tdFormScore'>4-5</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- จับประเด็นปัญหาไม่ได้ ไม่อาจไว้ใจให้ตัดสินใจ และไม่มีการวางแผนความคิดอย่างมีระบบ เพิกเฉยไม่ตัดสินใจหรือประเมินข้อเท็จจริงผิด</td>
                                    <td class='tdFormScore'>0-3</td>
                                    <td></td>
                                </tr>
                                
                                <!-- -->
                                <tr class='trCommanderForm'>
                                    <th class='thbFormNo'></th>
                                    <th class='thbFormDetail'>หมวดที่ 2 คุณลักษณะเฉพาะตัว</th>
                                    <th class='thbFormScore'></th>
                                    <th class='thbFormScoreA'></th>
                                    <th class='thbFormDetailA'></th>
                                </tr>
                                <!-- -->
                                <tr>
                                    <td class='tdmFormNo' rowspan='5'>3</td>
                                    <td class='tdmFormDetail'><b><u>ความรู้ความสามารถ</u></b> ความรู้ ความเข้าใจ ความสามารถและความเชี่ยวชาญในงานที่รับผิดชอบ</td>
                                    <td class='tdmFormScore'></td>
                                    <td class='tdmFormScoreA' rowspan='5'>
                                        <select id='drpScore03' >
                                            <option value='15' selected>15</option>
                                            <option value='14'>14</option>
                                            <option value='13'>13</option>
                                            <option value='12'>12</option>
                                            <option value='11'>11</option>
                                            <option value='10'>10</option>
                                            <option value='9'>9</option>
                                            <option value='8'>8</option>
                                            <option value='7'>7</option>
                                            <option value='6'>6</option>
                                            <option value='5'>5</option>
                                            <option value='4'>4</option>
                                            <option value='3'>3</option>
                                            <option value='2'>2</option>
                                            <option value='1'>1</option>
                                            <option value='0'>0</option>
                                        </select>
                                    </td>
                                    <td class='tdmFormDetail' rowspan='5'>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtSuggestion03' name="txtSuggestion03" rows='10'></textarea>
                                        </label>                                
                                        </div>                                     
                                    </td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- รู้ เข้าใจ และสามารถเข้าถึงงานในทุกด้านที่รับผิดชอบได้โดยสมบูรฯ์ และสามารถดัดแปลงเครื่องมือ หรือวิธ๊การทำงานให้เข้ากับภาระงานใหม่ได้</td>
                                    <td class='tdFormScore'>12-15</td>
                                    <td></td>
                                </tr> 
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- มีความรู้ ความสามารถและมีความเข้าใจในงานโดยส่วนใหญ่ มีความชำนาญในงานของตน</td>
                                    <td class='tdFormScore'>8-11</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- ไม่มีความรู้ ความเข้าใจพอสำหรับงานในความรับผิดชอบบางด้านขาดความชำนาญในงานของตน</td>
                                    <td class='tdFormScore'>4-7</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- ต้องพึงผู้อื่นเสมอและไม่สามารถให้อุปกรณ์ได้อย่างถูกต้อง</td>
                                    <td class='tdFormScore'>0-3</td>
                                    <td></td>
                                </tr>
                                
                                <!-- -->
                                <tr>
                                    <td class='tdmFormNo' rowspan='5'>4</td>
                                    <td class='tdmFormDetail'><b><u>การปฏิบัติงานและทัศนคติต่องาน</u></b> การปฏิบัติตามคำแนะนำด้วยความรับผิดชอบ ทัศนคติต่องานและการกำหนดวัตถุประสงค์</td>
                                    <td class='tdmFormScore'></td>
                                    <td class='tdmFormScoreA' rowspan='5'>
                                        <select id='drpScore04' >
                                            <option value='15' selected>15</option>
                                            <option value='14'>14</option>
                                            <option value='13'>13</option>
                                            <option value='12'>12</option>
                                            <option value='11'>11</option>
                                            <option value='10'>10</option>
                                            <option value='9'>9</option>
                                            <option value='8'>8</option>
                                            <option value='7'>7</option>
                                            <option value='6'>6</option>
                                            <option value='5'>5</option>
                                            <option value='4'>4</option>
                                            <option value='3'>3</option>
                                            <option value='2'>2</option>
                                            <option value='1'>1</option>
                                            <option value='0'>0</option>
                                        </select>
                                    </td>
                                    <td class='tdmFormDetail' rowspan='5'>
                                        <div class='inputWrapper'>
                                        <label>
                                            <textarea  class='normal' id='txtSuggestion04' name="txtSuggestion04" rows='10'></textarea>
                                        </label>                                
                                        </div>                                     
                                    </td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- </td>
                                    <td class='tdFormScore'>8-10</td>
                                    <td></td>
                                </tr> 
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- </td>
                                    <td class='tdFormScore'>6-7</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- </td>
                                    <td class='tdFormScore'>4-5</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class='tdFormDetail'>&nbsp;&nbsp;&nbsp;- </td>
                                    <td class='tdFormScore'>0-3</td>
                                    <td></td>
                                </tr>                                
                                                                                             
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td></td>
                                    <td><b>สรุปผลการประเมินส่วนที่ 2.2</b></td>
                                    <td colspan='4' class='tdFootScore'>
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
                            </tfoot>
                        </table>
                    </div>
				</fieldset>
            </div>
                          
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
<script src="EvaluateData.js?v=1" type="text/javascript"></script>
<script src="frmEvaluatePart02.js?v=9" type="text/javascript"></script>
