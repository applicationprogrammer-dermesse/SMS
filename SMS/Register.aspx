<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SMS.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Register Page</title>


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
            background-repeat: repeat;*/
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="fullscreen_bg" class="fullscreen_bg" />
        <div class="container-fluid">
            <br />
          
            <div class="row">

                <div class="col-sm-8">
                    <div id="registerbox" style="margin-top: 10px;" class="mainbox col-md-10">
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                <div class="panel-title">Register an Account</div>
                                <%--<div style="float:right; font-size: 80%; position: relative; top:-10px"><a href="#">Forgot password?</a></div>--%>
                            </div>

                            <div class="form-group">
                                <div class="col-md-12 control">
                                    <div style="border-top: 1px solid#888; padding-top: 15px; font-size: 100%">
                                        Don't have an account! Sign Up Here
                                    </div>
                                </div>
                            </div>

                            <div style="padding-top: 30px" class="panel-body">
                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-address-book-o"></i></span>
                                        
                                        <input id="inpEmpNo" runat="server" type="text" class="form-control decimalnumbers-only" name="EmployeeNumber" autocomplete="off" placeholder="Emp. No." maxlength="5" style="width: 105px;" required="required" xmlns:asp="#unknown"/>
                                    </div>

                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                        
                                        <input id="inpRegPass" runat="server" type="password" value="" class="form-control" name="password" placeholder="password" style="width: 155px;" autocomplete="off" required="required" xmlns:asp="#unknown"/>
                                    </div>

                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-address-card-o"></i></span>
                                        <input id="inpfullname" runat="server" type="text" class="form-control" name="Full name" value="" placeholder="Full name" style="width: 365px; text-transform: capitalize;" required="required" />
                                    </div>

                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-envelope"></i></span>
                                        <input id="inpEmail" runat="server" type="text" class="form-control" name="email" value="" placeholder="email address" style="width: 205px;" required="required" />
                                        <select id="selemail" runat="server" style="width: 255px" class="form-control">
                                            <option value="@dermclinic.com.ph">@dermclinic.com.ph</option>
                                            <option value="@dermpharma.com.ph">@dermpharma.com.ph</option>

                                            
                                        </select>
                                    </div>

                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-child"></i></span>
                                        <select id="selLevel" runat="server" style="width: 255px" class="form-control" required="required" oninvalid="this.setCustomValidity('Please select position')" oninput="setCustomValidity('')">
                                            <option value="">Please Select Position Level</option>
                                            <option value="1">Rank & File</option>
                                            <option value="2">Supervisor</option>
                                            <option value="3">Manager</option>
                                        </select>
                                    </div>

                                    <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-bank"></i></span>
                                        <select id="selCompany" runat="server" class="form-control" style="width: 305px;" required="required" oninvalid="this.setCustomValidity('Please select company')" oninput="setCustomValidity('')" >
                                            <option value="DCI">Dermclinic, Inc.</option>
                                            <option value="DPI">Dermpharma, Inc.</option>
                                        </select>
                                    </div>

                                 <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-bank"></i></span>
                                        <%--<asp:DropDownList ID="ddBranch" runat="server" Width="245px" CssClass="form-control"></asp:DropDownList>--%>
                                          <select id="selBranch" runat="server" class="form-control" style="width: 355px;" required="required" oninvalid="this.setCustomValidity('Please select')" oninput="setCustomValidity('')">
                                          </select>
                                    </div>


                               <%--     <div style="margin-bottom: 25px" class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-asl-interpreting"></i></span>
                                        <select id="selDepartment" runat="server" class="form-control" style="width: 355px;" required oninvalid="this.setCustomValidity('Please select department')" oninput="setCustomValidity('')">
                                        </select>
                                    </div>--%>

                                    <div style="margin-bottom: 20px" class="input-group">
                                        <p style="color: #00c0ef !important; font-size: smaller;">All fields are required</p>
                                    </div>

                                    <div style="margin-bottom: 15px" class="input-group">
                                         <%--<asp:Button ID="btnReg" runat="server" Text="REGISTER" class="btn btn-primary" OnClick="btnReg_Click" />--%>
                                         <button id="btnReg" onserverclick="btnReg_Click"  type="submit" runat="server" class="btn btn-danger">REGISTER&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-user"></i></button>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Click to go to Login Page&nbsp;<i class="glyphicon glyphicon-arrow-right"></i>
                                         <a href="LoginPage.aspx" class="btn btn-success btn-sm">LOG IN&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-log-in"></i></a>
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
    </form>

</body>
</html>
