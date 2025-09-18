<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SMS.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link href="Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Our Custom CSS -->
    <link href="CSS/cssMaster.css" rel="stylesheet" />
    <link href="jquery-ui.css" rel="stylesheet" />
    <link href="jquery-ui.min.css" rel="stylesheet" />




    <script type="text/javascript">
        function tabE(obj, e) {
            var f = (typeof event != 'undefined') ? window.event : e; // IE : Moz 
            if (e.keyCode == 13) {
                var ele = document.forms[0].elements;
                for (var i = 0; i < ele.length; i++) {
                    var q = (i == ele.length - 1) ? 0 : i + 1; // if last element : if any other 
                    if (obj == ele[i]) { ele[q].focus(); break }
                }
                return false;
            }
        }


        function disableautocompletion(id) {
            var passwordControl = document.getElementById(id);
            passwordControl.setAttribute("autocomplete", "off");
        }

    </script>

 
            
    <style type="text/css">
        .fullscreen_bg {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    
    /*background-size: cover;
    background-position: 50% 50%;
    background-image: url('images/color-splash.jpg');

    background-repeat:repeat;*/
  }
    </style>

</head>
<body>
    <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>    
      <div class="container-fluid"  style="width:98%">
          <br />
          <br />
        <div class="row" style="margin-bottom: 25px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                    <h4>Forgot Password</h4>
                </div>
                <div class="col-md-4 text-center">
                
                </div>
            </div>
        </div>
<hr />
          <br />
          <br />
           <div id="registerbox" style="margin-top: 5px;" class="mainbox col-md-8">
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                <div class="panel-title">Change Password</div>
                            </div>
                            <br />
                            <br />
                            <br />
                                         <div class="row" style="margin-bottom:5px;">
                                                <div class="col-sm-12">
                                                            <div class="col-sm-2 text-right">
                                                                Employee No.
                                                            </div>
                                                            <div class="col-sm-9">
                                                                <div class="input-group">
                                                                     <input type="text" name="EmployeeNumber" style="display:none" value="fake input" />
                                                                    <asp:TextBox ID="txtEmployeeNo" runat="server" class="form-control" value="Emp. No."   Style="width: 105px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                                        ControlToValidate="txtEmployeeNo"  Display="Dynamic" ValidationGroup="grpEmployee" ForeColor="Red"
                                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                                 </div>
                                                            </div>
                                                        </div>
                                            </div>

                                            <div class="row" style="margin-bottom:5px;">
                                                <div class="col-sm-12">
                                                            <div class="col-sm-2 text-right">
                                                                New Password
                                                            </div>
                                                            <div class="col-sm-9">
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtNewPassword" runat="server" class="form-control" Style="width: 125px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled" TextMode="Password" xmlns:asp="#unknown"></asp:TextBox>
                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                        ControlToValidate="txtNewPassword"  Display="Dynamic" ValidationGroup="grpEmployee" ForeColor="Red"
                                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                                 </div>
                                                            </div>
                                                        </div>
                                            </div>

                                       <div class="row" style="margin-bottom:25px;">
                                                <div class="col-sm-12">
                                                            <div class="col-sm-2 text-right">
                                                                Re-type Password
                                                            </div>
                                                            <div class="col-sm-9">
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtRetype" runat="server" class="form-control" Style="width: 125px; margin-bottom: 5px; "  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled" TextMode="Password"></asp:TextBox>
                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                        ControlToValidate="txtRetype"  Display="Dynamic" ValidationGroup="grpEmployee" ForeColor="Red"
                                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                                         ControlToValidate="txtRetype"
                                                                          ForeColor="Red"
                                                                         ControlToCompare="txtNewPassword"
                                                                         ErrorMessage="Password don't match" 
                                                                         ValidationGroup="grpEmployee"
                                                                         ToolTip="Password must be the same" />
                                                                 </div>
                                                            </div>
                                                        </div>
                                            </div>


                                       <div class="row" style="margin-bottom:25px;">
                                                <div class="col-sm-12">
                                                            <div class="col-sm-2 text-right">
                                    
                                                            </div>
                                                            <div class="col-sm-9">
                                                                   <div class="input-group">
                                                                       <asp:Button ID="btnUpdate" runat="server" Text="UPDATE PASSWORD"  ValidationGroup="grpEmployee" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                                                                 </div>
                                                            </div>
                                                        </div>
                                            </div>
                                </div>
                          
                        </div>
       
 </div>
        
       <script src="jquery-ui.min.js"></script>
        <script src="external/jquery/jquery.js"></script>
        <!-- Bootstrap Js CDN -->
        <script src="Bootstrap/bootstrap.min.js"></script>

        <!-- Personal -->
        <script src="JavaScript.js"></script>
        <script src="jquery-ui.js"></script>


         <script type="text/javascript">
             $(".decimalnumbers-only").keypress(function (e) {
                 if (e.which == 46) {
                     if ($(this).val().indexOf('.') != -1) {
                         return false;
                     }
                 }

                 if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
                     return false;
                 }
             });
</script>



        <script type="text/javascript">
            function ShowSuccessMsg() {
                $(function () {
                    $("#messageDiv").dialog({
                        title: "USER CREDENTIAL",
                        width: '335px',
                        modal: true,
                        buttons: {
                            Close: function () {
                                $(this).dialog('close');
                            }
                        },
                        modal: true
                    });
                    $("#messageDiv").parent().appendTo($("form:first"));
                });
            }
        </script>

        <div id="messageDiv" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:Label Text="" ID="lblMsg" runat="server" />
                    <br />

                </ContentTemplate>

            </asp:UpdatePanel>

       </div>

<script type="text/javascript">
    function ShowWarningMsg() {
        $(function () {
            $("#messageWarning").dialog({
                title: "USER CREDENTIAL",
                width: '335px',
                buttons: {
                    Close: function () {
                        window.location = '<%= ResolveUrl("~/LoginPage.aspx") %>';
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageWarning" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWarning" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    
    </form>
</body>
</html>
