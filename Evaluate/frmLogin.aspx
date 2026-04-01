<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmLogin.aspx.vb" Inherits="Evaluate.frmLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Evaluate</title>
    <link rel="stylesheet" type="text/css" href="includes/css/main.css"  media="screen" /> 
    <link rel="stylesheet" type="text/css" href="Evaluate.css"  media="screen" />

    <script type="text/javascript" src="includes/js/jquery-1.10.2.min.js"></script>
    
    <script type="text/javascript" src="includes/js/jquery-migrate-1.2.1.min.js"></script>
    
    <script type="text/javascript" src="includes/js/jquery-ui-1.8.4.custom.min.js"></script> 
    <script type="text/javascript" src="includes/js/jquery.corner.js"></script> 
    <script type="text/javascript" src="includes/js/i18n/grid.locale-en.js"></script>
    <script type="text/javascript" src="includes/js/header.js"></script>
</head>
<body>
    <div class="container_12">
    <!--  =============== Start Header ===============   -->    	
        <div id="header" class="grid_12">
            <img id='imglogo' src="images/EvaluateBanner.png" alt="Evaluate" width="40px" height="50px"/>
            <label id='namesystem'>งานบุคคล</label>
            <!--<img src="images/EvaluateBanner.png" alt="Evaluate" width="100%" height="70px"/>-->
        </div>    
        <!--<div id="login" class="grid_2 rounded">Admin | <a href="#">logout</a></div>-->
        <!--  =============== Start Main Menu ===============   -->
        <!-- #include FILE="menulogin.inc" -->  
        <div class="clear"></div>
   
        <div class="grid_9">
            <div class="title rounded"> ประกาศ </div>
            <fieldset class="rounded" id="FIELDSET1" >
            <br />
            <br /><b>6/6/2568</b> เพิ่ม URL <b>https://evaluate-eng.kmutt.ac.th</b> เพื่อรองรับ HTTPS (จากเดิม http://evaluate.eng... เพิ่ม https://evaluate-eng...)
            <br />
            <br /><b>7/4/2568</b> เปลี่ยนแบบฟอร์มการกรอกข้อมูล 2.1 เป็น แบบข้อตกลง PA (Performance Agreement) ซึ่งต้องประเมินผล 6 เดือน และ 12 เดือน
            <br />
            <br /><b>7/9/2558</b> เปลี่ยนการกรอกข้อมูล ของผู้บังคับบัญชาในหัวข้อ 2.2 เป็นระบุเลขทศนิยม 2 ตำแหน่ง
            <br />
            <br /><b>28/9/2558</b> ระบบ พิมพ์รายงาน (ปุ่ม <button id="btnPrintImage" class="save">พิมพ์ไปยังไฟล์</button>) ผู้ใช้สามารถพิมพ์หน้าจอออกเป็นรูปภาพ PNG เพื่อใช้เป็นเอกสารแนบต่อไป
            <br />
            <br /><b>28/1/2557</b> ระบบ Export สามารถใช้งานได้แล้ว (ผู้ใช้สามารถ Export แบบฟอร์มออกมาให้อยู่ในรูปแบบ Excel ไฟล์ เพื่อนำได้ใช้ในระบบงานอื่น หรือสั่งพิมพ์ได้)
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br /><b><u>หมายเหตุ</u></b>
            <br />แจ้งปัญหาการใช้งานที่ 
            <br />คุณ ออมสิน แก่งหลวง สำนักงานคณบดีคณะวิศวกรรมศาสตร์
            <br />Email: Aomsyn.kaen@kmutt.ac.th 
            <br />Tel: 02-470-9022
            <br /><asp:Label runat="server" ID="lblVersion"></asp:Label>
            </fieldset> 
        </div>
        <div class="grid_3">
            <div class="title rounded">Login </div>
            <fieldset class="rounded" >
            <form id="Form1" runat="server">
                <table width="100%" align="center">
                    <tr>
                        <td>ชื่อผู้ใช้ :</td>
                        <td><asp:TextBox runat="server" ID="txtUserName" >
                        </asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>รหัสผ่าน :</td>
                        <td><asp:TextBox runat="server" ID="txtPassword" TextMode="Password" >
                        </asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>รอบ/ปีงบประมาณ :</td>
                        <td><asp:DropDownList runat="server" ID="drpBudgetYear" DataSourceID="dsBudgetYear" DataTextField="round_year" DataValueField="round_year">
                        </asp:DropDownList><asp:SqlDataSource ID="dsBudgetYear" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
                            SelectCommand="SELECT ltrim(rtrim(str([sround]) + '/' + [syear])) round_year FROM [tblCFG_Year] ORDER BY [syear], [sround]"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr height="10">
                        <td></td>
                        <td></td>
                    </tr>                    
                    <tr>
                        <td></td>
                        <td><asp:Button runat="server" ID="btnSubmit" Text="Login" /></td>
                    </tr>
                </table>
                <asp:Label runat="server" ID="lblMessage" CssClass="message_error" Width="100%"></asp:Label>
            </form>
            </fieldset>
        </div>
        <div class="clear"></div>
        <!-- ============= input form ============= -->        
        <!--<div class="grid_12"> 
	        <div class="block" id="forms">		    
			    <fieldset class="rounded" >
				    <legend class="rounded"> Login </legend>
				</fieldset>
            </div>
        </div>-->
        <div class="clear"></div>        
        <!--  =============== Start Footer ===============   -->
		<div class="grid_12">
			<div class="rounded" id="footer">
				<p>&copy; 2557 <strong>คณะวิศวกรรมศาสตร์ </strong> </p>
			</div>
		</div>
		<div class="clear"></div>        
    </div>
</body>
</html>
<script src="frmLogin.js?v=4" type="text/javascript"></script>
