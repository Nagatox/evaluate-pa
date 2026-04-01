<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmSuggestionForm.aspx.vb" Inherits="Evaluate.frmSuggestionForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Suggestion - Evaluate Project</title>
    <link rel="stylesheet" type="text/css" href="includes/css/main.css"  media="screen" /> 
    <link rel="stylesheet" type="text/css" href="includes/css/Scroll.css"  media="screen" />
    <link rel="stylesheet" type="text/css" href="Evaluate.css"  media="screen" />
    
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
</head>
<body>
    <div id='divBackground' class="UpdateProgressBackground"></div>
    <div id='divWaiting' class="UpdateProgressContent">Please Wait ...</div>

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
                <button id="btnSave" class="save" style="visibility:hidden">Save</button>	            
                <button id="btnNew" class="new" style="visibility:hidden">New</button>
                <button id="btnOpen" class="open" style="visibility:hidden">Open</button>
                <button id="btnDelete" class="delete" style="visibility:hidden">Delete</button>	   
             </div>            
        </div>    
        <div class="grid_8">
            <div class="title rounded"> สรุประดับชั้นความสามารถ ปีการประเมินฯ <asp:Label runat="server" ID="lblBudgetYear"></asp:Label> </div>
        </div>
        <div class="clear"></div>
        <!-- ============= input form ============= --> 
        <div class="grid_12"> 
	        <div class="block" id="DIV1">		    
			    <fieldset class="rounded" >
				    <legend class="rounded"> สรุประดับชั้นความสามารถ</legend>
				    <center>
				    <div id="Div2" class="tableContainerx">

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
</body>
</html>

<script src="frmSuggestionForm.js" type="text/javascript"></script>