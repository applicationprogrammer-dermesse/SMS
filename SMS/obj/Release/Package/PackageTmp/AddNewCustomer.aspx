<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewCustomer.aspx.cs" Inherits="SMS.AddNewCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<%--<link rel="stylesheet" href="docsupport/prism.css" />
  <link rel="stylesheet" href="chosen.css" />--%>
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
     table th {
        text-align:center;
        vertical-align: middle;
         background-color:#f2f2f2;
              font-size:12px;
    }
     table tr {
        vertical-align: middle;
        font-size:12px;
    }
     .hiddencol { display: none; }

.ui-dialog { position: fixed; padding: .1em; width: 300px; overflow: hidden; }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
  <div class="container-fluid"  style="width:98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                    <h4>Add New Customer Form</h4>
                </div>
                <div class="col-md-4 text-center">
                
                </div>
            </div>
        </div>

          
         <hr />
      <div class="row" style="margin-bottom:2px; margin-top:3px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">

                                </div>
                                 <div class="col-sm-2">
                                    <div class="input-group">
                                        Last Name
                                     </div>
                                </div>
                                <div class="col-sm-2">
                                    <div class="input-group">
                                        First Name
                                     </div>
                                </div>

                           <div class="col-sm-2">
                                    <div class="input-group">
                                        Middle Name
                                     </div>
                                </div>


                            </div>
      </div>

      <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Customer Name :
                                </div>
                                 <div class="col-sm-2">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtLastName" runat="server"  class="form-control" MaxLength="149" Style="width: 175px; text-transform:uppercase;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                    
                                     </div>
                                </div>
                                <div class="col-sm-2">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtFirstName" runat="server" class="form-control" MaxLength="149" Style="width: 175px; text-transform:uppercase;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                      
                                     </div>
                                </div>

                           <div class="col-sm-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtMiddleName" runat="server" class="form-control" MaxLength="149" Style="width: 175px; text-transform:uppercase;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnBuyungOnly" runat="server" Text="SAVE (Buying Patient Only)" CssClass="btn-success btn-sm" OnClick="btnBuyungOnly_Click" ValidationGroup="grpCustBuyOnly"/>                                   
                                     </div>
                                </div>


                            </div>
                </div>

      <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">

                                </div>
                                 <div class="col-sm-2">
                                    <div class="input-group">
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                            ControlToValidate="txtLastName"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                                <div class="col-sm-2">
                                    <div class="input-group">
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                            ControlToValidate="txtFirstName"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>

                           <div class="col-sm-2">
                                    <div class="input-group">
                                          <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                            ControlToValidate="txtMiddleName"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                                     </div>
                                </div>


                            </div>
      </div>

              <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Region/Province :
                                </div>
                                 <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddRegion" runat="server" class="form-control" Width="285px" AutoPostBack="True" OnSelectedIndexChanged="ddRegion_SelectedIndexChanged"></asp:DropDownList>
                                        &nbsp;
                                        <asp:Button ID="btnAddRegion" runat="server" Text="ADD REGION/PROVINCE" CssClass="btn btn-default" OnClick="btnAddRegion_Click" />
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" InitialValue="0" 
                                            ControlToValidate="ddRegion"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                        </div>
                </div>

           <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    City :
                                </div>
                                 <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddCity" runat="server" class="form-control" Width="305px"></asp:DropDownList>
                                        &nbsp;
                                        <asp:Button ID="btnAddCity" runat="server" Text="ADD CITY" Enabled="false" CssClass="btn btn-default" OnClick="btnAddCity_Click" />
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0" 
                                            ControlToValidate="ddCity"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                        </div>
                </div>

          <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Stree/House Address :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                       <asp:TextBox ID="txtHouseAddress" runat="server" class="form-control" MaxLength="450" Style="width: 505px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                            ControlToValidate="txtHouseAddress"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                                     </div>
                                </div>
                        </div>
                </div>

         <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Gender :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddGender" runat="server" class="form-control" Width="235px">
                                            <asp:ListItem Value="0" Text="Please select gender" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Male"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Female"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Gay"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Lesbian"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Not Sure"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" InitialValue="0" 
                                            ControlToValidate="ddGender"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0" 
                                            ControlToValidate="ddGender"  Display="Dynamic" ValidationGroup="grpCustBuyOnly" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                        </div>
                </div>

      <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Civil Satus :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddCivilSatus" runat="server" class="form-control" Width="205px">
                                             <asp:ListItem Value="0" Text="Please select status" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Single"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Married"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Separated"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Widow"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" InitialValue="0" 
                                            ControlToValidate="ddCivilSatus"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                        </div>
                </div>

      
          <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                   Date Of Birth :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                       <asp:TextBox ID="txtDateOfBirth" runat="server" class="form-control dateTxtYear" Style="width: 105px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                            ControlToValidate="txtDateOfBirth"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                        </div>
                </div>

      <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                   Contat No. 1 :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                       <asp:TextBox ID="txtContatNo1" runat="server" class="form-control" MaxLength="20" Style="width: 155px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                                            ControlToValidate="txtContatNo1"  Display="Dynamic" ValidationGroup="grpCust" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                        </div>
                </div>

      <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                   Contat No. 2 :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                       <asp:TextBox ID="txtContatNo2" runat="server" class="form-control" MaxLength="20" Style="width: 155px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                     </div>
                                </div>
                        </div>
                </div>

      <div class="row" style="margin-bottom:15px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                   Emai Address :
                                </div>
                                 <div class="col-sm-9">
                                    <div class="input-group">
                                       <asp:TextBox ID="txtEmailAddress" runat="server" class="form-control" MaxLength="100" Style="width: 285px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                     </div>
                                </div>
                        </div>
                </div>

      <div class="row" style="margin-bottom:5px;">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">

                                </div>
                                 <div class="col-sm-2">
                                    <div class="input-group">
                                        <asp:Button ID="btnSave" runat="server" Text="SAVE" ValidationGroup="grpCust" CssClass="btn-primary btn-sm" OnClick="btnSave_Click"/>
                                     </div>
                                </div>

                                 <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:Button ID="btnUpdate" runat="server" Text="UPDATE" ValidationGroup="grpCust" Enabled="false" CssClass="btn-success btn-sm" OnClick="btnUpdate_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Label ID="lblNote" runat="server" Text="Click update button to update records" Visible="false"></asp:Label>
                                     </div>
                                </div>
                        </div>
                </div>

      <br />
      <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Search Customer To Update
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtCustomer" CssClass="form-control" Width="405px" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:TextBox ID="txtCustID" CssClass="form-control" Width="95px" ReadOnly="true" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSearchBranch" runat="server" CssClass="btn btn-warning"  ValidationGroup="grpSearch" Text="SEARCH" OnClick="btnSearchBranch_Click" />
                        &nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server"
                            ControlToValidate="txtCustomer" Display="Dynamic" ValidationGroup="grpSearch" ForeColor="Red"
                            ErrorMessage="Supply Customer Name"></asp:RequiredFieldValidator>
                    </div>
                </div>

            
        </div>


 </div>


        <%--*******************************************************************************--%>
    <script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.dateTxtYear').datepicker({
                //minDate: 0,
                changeMonth: true,
                changeYear: true,
                yearRange: "-100:+0",
            });
        });


        </script>


     <!-- ################################################# END #################################################### -->


    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageSuccess").dialog({
                    title: "New Customer",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/EncodeSales.aspx") %>';
                            $(this).dialog('close');

                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageSuccess" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgSuccess" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- ################################################# START #################################################### -->
 
    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Customer Name",
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







<!-- ################################################# END #################################################### -->




<script type="text/javascript">
    function CloseGridCustomer() {
        $(function () {
            $("#ShowCustomer").dialog('close');
        });
    }
    </script>



    <script type="text/javascript">
        function ShowGridCustomer() {
            $(function () {
                $("#ShowCustomer").dialog({
                    title: "Customer",
                    //position: ['center', 20],

                    width: '600px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowCustomer").parent().appendTo($("form:first"));
            });
        }
    </script>


    <div id="ShowCustomer" class="box-content" style="display: none;">
         <div style="overflow:auto; max-height:500px;">
                <asp:GridView ID="gvCustomerList" runat="server" AutoGenerateColumns="False" OnRowUpdating="gvCustomerList_RowUpdating" >
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                 
                        <asp:BoundField HeaderText="Branch" DataField="BrCode" ItemStyle-Width="105px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Customer ID" DataField="CustID" ItemStyle-Width="105px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ItemStyle-Width="275px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                
                        

                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Center" Width="175px" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSelect" runat="server" class="btn btn-success btn-sm" CommandName="Update" Text="SELECT" ForeColor="white"  OnClientClick="return   CloseGridCustomer();" />
                    </ItemTemplate>
                </asp:TemplateField>

                        
                    </Columns>
                </asp:GridView>
         </div>
    </div>

    <%--*******************end************************************************************--%>


    </div>


<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridAddRegion() {
            $(function () {
                $("#ShowAddRegion").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridAddRegion() {
            $(function () {
                $("#ShowAddRegion").dialog({
                    title: "Add New Region",
                    //position: ['center', 20],

                    width: '720px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowAddRegion").parent().appendTo($("form:first"));
            });
        }
    </script>
    	

    <div id="ShowAddRegion" style="display: none;">
        <br />
	    <br />
         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
                        Region Name
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtERegionName" runat="server" class="form-control" MaxLength="240" Text="" Style="width: 325px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvRegion" runat="server"
                                ControlToValidate="txtERegionName" Display="Dynamic" ValidationGroup="grpE" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


         

        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
        
                    </div>
                    <div class="col-sm-8">
                        <div class="input-group">
                            <asp:Button ID="btnEaddRegion" runat="server" Text="S A V E" class="btn btn-primary" ValidationGroup="grpE" OnClick="btnEaddRegion_Click"/>
                        </div>
                    </div>

                </div>
            </div>


    </div>

<!-- ################################################# END #################################################### -->    

<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridAddCity() {
            $(function () {
                $("#ShowAddCity").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridAddCity() {
            $(function () {
                $("#ShowAddCity").dialog({
                    title: "Add New City",
                    //position: ['center', 20],

                    width: '720px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowAddCity").parent().appendTo($("form:first"));
            });
        }
    </script>
    	

    <div id="ShowAddCity" style="display: none;">
        <br />
	    <br />
         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
                        City Name
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtECityName" runat="server" class="form-control" MaxLength="240" Text="" Style="width: 325px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvCity" runat="server"
                                ControlToValidate="txtECityName" Display="Dynamic" ValidationGroup="grpC" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


         

        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
        
                    </div>
                    <div class="col-sm-8">
                        <div class="input-group">
                            <asp:Button ID="btnEaddCity" runat="server" Text="S A V E" class="btn btn-primary" ValidationGroup="grpC" OnClick="btnEaddCity_Click"/>
                        </div>
                    </div>

                </div>
            </div>


    </div>

<!-- ################################################# END #################################################### -->    




</asp:Content>
