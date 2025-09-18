<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="SMS.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Sales Monitoring System</title>

    <!-- Bootstrap CSS CDN -->
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

        // ENTER KEY CODE WILL WORK
        <%--function tabE(obj, e) {
            var keyCode = (typeof event != 'undefined') ? event.keyCode : e.which;
            if (keyCode == 13) {
                // If focused on password field, submit
                if (obj.id === 'login_password') {
                    document.getElementById('<%= btnLogin.ClientID %>').click();
            return false;
        }

                var ele = document.forms[0].elements;
                for (var i = 0; i < ele.length; i++) {
                    var q = (i == ele.length - 1) ? 0 : i + 1;
                    if (obj == ele[i]) {
                        ele[q].focus();
                        break;
                    }
                }
                return false;
            }
        }--%>


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
<form id="Form1" runat="server"  >
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>    
    <div id="fullscreen_bg" class="fullscreen_bg"/>
    <div class="container-fluid">
                  <div class="row">
                                <div class="col-sm-4" >
                                 <div id="loginbox" style="margin-top:75px; margin-left:0px;" class="mainbox col-sm-12">                    
                                        <div class="panel panel-info" >
                                                <div class="panel-heading">
                                                    <div class="panel-title">LOGIN</div>
                                                </div>     

                                                <div style="padding-top:30px" class="panel-body" >

                                                              <div style="margin-bottom: 25px" class="input-group">
                                                                        <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                                                        <input id="login_username" runat="server" type="text" class="form-control decimalnumbers-only" name="username" placeholder="Emp. No." required="required" onkeydown="return (event.keyCode!=13);" autocomplete="off"  />  
                                                                        <%--<input id="login_username" runat="server" type="text" class="form-control decimalnumbers-only" name="username" placeholder="Emp. No." required="required" onkeydown="return tabE(this, event);" autocomplete="off"  />--%>                                         
                                                                    </div>
                                
                                                               <div style="margin-bottom: 25px" class="input-group">
                                                                        <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                                                        <input id="login_password"  runat="server" type="password" class="form-control" name="password" placeholder="Password" required="required" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled" />
                                                                        <%--<input id="login_password"  runat="server" type="password" class="form-control" name="password" placeholder="Password" required="required" onkeydown="return tabE(this, event);" autocomplete="off" AutoCompleteType="Disabled" />--%>
                                                                        
                                                               </div>
                                                            
                                                                <div style="margin-top:10px;margin-bottom: 35px" class="input-group">
                                                                    <div class="col-sm-12 controls">
                                                                        <button id="btnLogin" onserverclick="btnLogin_Click"  type="submit" runat="server" class="btn btn-success">Login&nbsp;&nbsp;&nbsp;<i class="glyphicon  glyphicon-log-in"></i></button>
                                                                        <a href="ForgotPassword.aspx" style="color:#0066ff;">Forgot password?</a>
                                                                    </div>
                                                                </div>
                                                                
                                                                <div style="margin-top:10px;margin-bottom: 25px;" class="input-group">
                                                                    <div class="col-sm-12 controls">
                                                                        Don't have an account?
                                                                        &nbsp;
                                                                        <a href="Register.aspx" class="btn btn-primary  btn-sm">Register&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-edit"></i></a>
                                                                    </div>
                                                                </div>

                                                </div>
                                            </div>
                               </div>
                     
                                           
                               </div>
                       <div class="col-sm-8" >
                          <%-- <asp:Image ID="Image2" runat="server" ImageUrl="~/images/User_login.png" Height="165px" Width="25%" />--%>
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
    function ShowWarningMsg() {
        $(function () {
            $("#messageWarning").dialog({
                title: "USER LOGIN",
                width: '335px',
                buttons: {
                    Close: function () {
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
