<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmEvaluatePart03.aspx.vb" Inherits="Evaluate.frmEvaluatePart03" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Part 3 Evaluation Form - Evaluate Project</title>
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
            <div class="title rounded"> <asp:Label runat="server" ID="lblNameUser"></asp:Label> - แบบฟอร์มการพัฒนาและฝึกอบรม รอบการประเมินฯที่ <asp:Label runat="server" ID="lblRoundNo"></asp:Label> / <asp:Label runat="server" ID="lblBudgetYear"></asp:Label> </div>
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
                </div>			    
			    <fieldset class="rounded" >
				    <legend class="rounded"> ส่วนที่ 3 การพัฒนาและฝึกอบรม </legend>
                    			    
				    <div id="divDevFormEmployee">
                        <table class='classDevFormX' >
                            <thead>
                                <tr class='trDevForm'>
                                    <th class='thDevFormHead'>พนักงานกรอก</th>
                                    <th class='thDevFormHeadSpace'></th>
                                    <th class='thDevFormCHead'>ผู้บังคับบัญชาให้ความเห็น</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td id='tdEmployee'>
                                        <table class='classDevForm' id='tblEmployee01'>
                                            <thead>
                                                <tr class='trDevForm' >
                                                    <td class='tdmDevFormNo'>1.</td>
                                                    <td class='tdmDevFormDetail'>ท่านขาดความรู้ ความชำนาญ ทักษะในด้านใดบ้าง</td>
                                                    <td class='tdmDevFormDelete'></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='btnDelete_" + i + "' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button4' class='btnDelete'>ลบ</button></td>
                                                </tr>
                                            </tbody>
                                            <tfoot>
                                                <tr id='trAddRowEmployee01'>
                                                    <td class='tfDevFormAdd'><button id='btnAddRowEmployee01'>+</button></td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                        <table class='classDevForm' id='tblEmployee02'>
                                            <thead>
                                                <tr class='trDevForm' >
                                                    <td class='tdmDevFormNo'>2.</td>
                                                    <td class='tdmDevFormDetail'>ท่านคิดว่าจะสามารถเพิ่มความรู้ ความชำนาญ ทักษะดังกล่าวได้โดยวิธีใด้บ้าง</td>
                                                    <td class='tdmDevFormDelete'></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button1' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button2' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                            </tbody>
                                            <tfoot>
                                                <tr id='trAddRowEmployee02'>
                                                    <td class='tfDevFormAdd'><button id='btnAddRowEmployee02'>+</button></td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                        <table class='classDevForm' id='tblEmployee03'>
                                            <thead>
                                                <tr class='trDevForm' >
                                                    <td class='tdmDevFormNo'>3.</td>
                                                    <td class='tdmDevFormDetail'>ในช่วง 6 เดือนที่ผ่านมา ท่านได้เข้ารับการอบรมอะไรบ้าง</td>
                                                    <td class='tdmDevFormDelete'></td>
                                                </tr>
                                            </thead>                                        
                                            <tbody>
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button8' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button9' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                            </tbody>
                                            <tfoot>
                                                <tr id='trAddRowEmployee03'>
                                                    <td class='tfDevFormAdd'><button id='btnAddRowEmployee03'>+</button></td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                        <table class='classDevForm' id='tblEmployee04'>
                                            <thead>
                                                <tr class='trDevForm' >
                                                    <td class='tdmDevFormNo'>4.</td>
                                                    <td class='tdmDevFormDetail'>ท่านสนใจการฝึกอบรม หรือต้องการเรียนรู้เรื่องใดบ้างที่จะช่วยให้มีความสามารถปฏิบัติงานที่ได้รับมอบหมายในปัจจุบันได้ดียิ่งขึ้น (เรียงลำดับความสำคัญ 1-5)</td>
                                                    <td class='tdmDevFormDelete'></td>
                                                </tr>
                                            </thead>                                        
                                            <tbody>
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button11' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button12' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                            </tbody>
                                            <tfoot>
                                                <tr id='trAddRowEmployee04'>
                                                    <td class='tfDevFormAdd'><button id='btnAddRowEmployee04'>+</button></td>
                                                </tr>
                                            </tfoot>
                                        </table>                                          
                                    </td>
                                    <td></td>
                                    <td id='tdCommander'>
                                        <table class='classDevForm' id='tblCommander01'>
                                            <thead>
                                                <tr class='trDevForm' >
                                                    <td class='tdmDevFormCNo'>1.</td>
                                                    <td class='tdmDevFormCDetail'>ท่านคิดว่าผู้ใต้บังคับบัญชายังขาดความรู้ ความชำนาญ ทักษะในเรื่องใดบ้าง</td>
                                                    <td class='tdmDevFormCDelete'></td>
                                                </tr>
                                            </thead>                                        
                                            <tbody>
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button5' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button6' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                            </tbody>
                                            <tfoot>
                                                <tr id='trAddRowCommander01'>
                                                    <td class='tfDevFormAdd'><button id='btnAddRowCommander01'>+</button></td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                        <table class='classDevForm' id='tblCommander02'>
                                            <thead>
                                                <tr class='trDevForm' >
                                                    <td class='tdmDevFormCNo'>2.</td>
                                                    <td class='tdmDevFormCDetail'>ท่านคิดว่าผู้ใต้บังคับบัญชาควรจะอบรม หรือต้องการความรู้เรื่องใดบ้างที่จะช่วยให้มีความสามารถปฏิบัติงานที่ได้รับมอบหมายในปัจจุบันได้ดียิ่งขึ้น (เรียงลำดับความสำคัญ 1-5)</td>
                                                    <td class='tdmDevFormCDelete'></td>
                                                </tr>
                                            </thead>                                        
                                            <tbody>
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button14' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                                <tr>
                                                    <td class='tdDevFormNo'></td>
                                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                                    <td class='tdDevFormDelete'><button id='Button15' class='btnDelete'>ลบ</button></td>
                                                </tr>                                                               
                                            </tbody>
                                            <tfoot>
                                                <tr id='trAddRowCommander02'>
                                                    <td class='tfDevFormAdd'><button id='btnAddRowCommander02'>+</button></td>
                                                </tr>
                                            </tfoot>
                                        </table>                                         
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        
<!--                                    
                        <table>
                            <thead>
                                <tr class='trDevForm'>
                                    <td class='tdmDevFormNo'>1.</td>
                                    <td class='tdmDevFormDetail'>ท่านขาดความรู้ ความชำนาญ ทักษะในด้านใดบ้าง</td>
                                    <td class='tdmDevFormDelete'></td>
                                </tr>
                            </thead>                                        
                            <tbody>
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='btnDelete_" + i + "' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button4' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td style='display:nonex; text-align:center;'><button id='btnAddRow'>+</button></td>
                                </tr>
                            </tfoot>                                        
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                            </table>
                            

                            <tbody>
                                <tr class='trDevForm'>
                                    <td class='tdmDevFormNo'>2.</td>
                                    <td class='tdmDevFormDetail'>ท่านคิดว่าจะสามารถเพิ่มความรู้ ความชำนาญ ทักษะดังกล่าวได้โดยวิธีใด้บ้าง</td>
                                    <td class='tdmDevFormDelete'></td>
                                </tr>
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button5' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button6' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                            </tbody>
                            <tbody>
                                <tr class='trDevForm'>
                                    <td class='tdmDevFormNo'>3.</td>
                                    <td class='tdmDevFormDetail'>ในช่วง 6 เดือนที่ผ่านมา ท่านได้เข้ารับการอบรมอะไรบ้าง</td>
                                    <td class='tdmDevFormDelete'></td>
                                </tr>
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button7' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button8' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                            </tbody>
                            <tbody>
                                <tr class='trDevForm'>
                                    <td class='tdmDevFormNo'>4.</td>
                                    <td class='tdmDevFormDetail'>ท่านสนใจการฝึกอบรม หรือต้องการเรียนรู้เรื่องใดบ้างที่จะช่วยให้มีความสามารถปฏิบัติงานที่ได้รับมอบหมายในปัจจุบันได้ดียิ่งขึ้น (เรียงลำดับความสำคัญ 1-5)</td>
                                    <td class='tdmDevFormDelete'></td>
                                </tr>
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button15' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button16' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                            </tbody>
                        </table>
                    </div>
                    <div id="divDevFormCommander">
                        
                        <table class='classDevForm' id='tblDevFormCommander' >
                            <thead>
                                <tr class='trDevForm'>
                                    <th class='thDevFormHead' colspan='3'>ผู้บังคับบัญชาให้ความเห็น</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class='trDevForm'>
                                    <td class='tdmDevFormNo'>1.</td>
                                    <td class='tdmDevFormDetail'>ท่านคิดว่าผู้ใต้บังคับบัญชายังขาดความรู้ ความชำนาญ ทักษะในเรื่องใดบ้าง</td>
                                    <td class='tdmDevFormDelete'></td>
                                </tr>
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button9' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button10' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                            </tbody>
                            <tbody>
                                <tr class='trDevForm'>
                                    <td class='tdmDevFormNo'>2.</td>
                                    <td class='tdmDevFormDetail'>ท่านคิดว่าผู้ใต้บังคับบัญชาควรจะอบรม หรือต้องการความรู้เรื่องใดบ้างที่จะช่วยให้มีความสามารถปฏิบัติงานที่ได้รับมอบหมายในปัจจุบันได้ดียิ่งขึ้น (เรียงลำดับความสำคัญ 1-5)</td>
                                    <td class='tdmDevFormDelete'></td>
                                </tr>
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button11' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                                <tr>
                                    <td class='tdDevFormNo'></td>
                                    <td class='tdDevFormDetail'><div  class='inputWrapper'><textarea class='normal'></textarea></div></td>
                                    <td class='tdDevFormDelete'><button id='Button12' class='btnDelete'>ลบ</button></td>
                                </tr>                                                               
                            </tbody>
                        </table>-->
                    </div>
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
<script src="frmEvaluatePart03.js?v=9" type="text/javascript"></script>
