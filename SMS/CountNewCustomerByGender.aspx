<%@ Page Title="New Customer By Gender" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountNewCustomerByGender.aspx.cs" Inherits="SMS.CountNewCustomerByGender" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="docsupport/prism.css" />
  <link rel="stylesheet" href="chosen.css" />

    <style type="text/css">
    .alignCenter {
    text-align: center !important;
}
            table th {
            text-align: center;
            vertical-align: middle;
            background-color: #f2f2f2;
            font-size: 12px;
        }

        table tr {
            vertical-align: middle;
            font-size: 12px;
        }

        .hiddencol {
            display: none;
        }


        .ui-dialog {
            position: fixed;
            padding: .1em;
            width: 300px;
            overflow: hidden;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="container">

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12 text-center">
                <asp:Label ID="Label2" runat="server" Text="Customer Count by Gender"></asp:Label>
            </div>
        </div>
        

        
  

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Date From :
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                     <asp:TextBox ID="txtDateFrom" runat="server" class="form-control dateCurr" Style="width: 120px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateFrom"
                                    ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                    ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Invalid date format"
                                    ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                     </div>
                </div>
            </div>
        </div>

           <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            To :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">

                                <asp:TextBox ID="txtDate" runat="server" class="form-control dateCurr" Style="width: 120px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDate"
                                    ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                    ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Invalid date format"
                                    ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"  Style="color: Red;"
                                            ValidationGroup="grpIss" Display="Dynamic" ControlToCompare="txtDateFrom"
                                            ControlToValidate="txtDate" Type="Date" Operator="GreaterThanEqual"
                                            ErrorMessage="Invalid date range"></asp:CompareValidator>

                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Customer Type
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddCustomer" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Text="New Customers Only" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="All Customers"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                        </div>
                    </div>
                </div>

         <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                      <button id="btnGenerate" onserverclick="btnGenerate_Click" onclick="if(Page_ClientValidate()) showDiv();" type="submit" runat="server" validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        &nbsp;&nbsp;&nbsp;
                           <button id="btnExcel" onserverclick="btnExcel_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                        &nbsp;&nbsp;&nbsp;
                    
                        <div id='myHiddenDiv' style="text-align:center "> 
                                        <img src='' id='myAnimatedImage' alt="" height="60" /> 
                         </div>
                    </div>
                </div>
            </div>
        </div>

    <%--    <div class="row" style="margin-bottom: 5px; margin-left:15px;">
            <div class="col-md-3">
                <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="#ff3399" Text="New Customers Count"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="Label4" runat="server" Font-Bold="true" ForeColor="#ff3399" Text="Active Customers Count"></asp:Label>
            </div>

            <div class="col-md-3">
                <asp:Label ID="Label5" runat="server" Font-Bold="true" ForeColor="#ff3399" Text="Transaction Record Count"></asp:Label>
            </div>

            <div class="col-md-3">
                <asp:Label ID="Label3" runat="server" Font-Bold="true" ForeColor="#ff3399" Text="Inactive Customers Count"></asp:Label>
            </div>
        </div>--%>

         

        <div class="row" style="margin-bottom: 15px; ">
            <div class="col-sm-12"  style="margin-left:50px;">
                <div class="col-md-12">
                      <asp:GridView ID="gvCustomerCount" runat="server" Width="70%" DataKeyNames="BrCode" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvCustomerCount_RowCommand1" >
                             <Columns>
                                        <asp:BoundField HeaderText="Brc" DataField="BrcODE" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="40%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                 
                                       <asp:TemplateField HeaderText="Male">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkMaleOnly" runat="server" CommandName="ViewMaleOnly"  Text='<%#Eval("NewMale") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>




                                 

                                    <asp:TemplateField HeaderText="Female">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFemaleOnly" runat="server" CommandName="ViewFemaleOnly"  Text='<%#Eval("NewFemale") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                 <asp:TemplateField HeaderText="No Gender">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkNoGender" runat="server" CommandName="ViewNoGender"  Text='<%#Eval("NoGender") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Total Customers<br/>(By Gender)">
                                            <ItemStyle HorizontalAlign="Center" Width="15%" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkAllGender" runat="server" CommandName="ViewAllGender"  Text='<%#Eval("TotalNewCust") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                             <%--    <asp:BoundField HeaderText="Total Customers<br/>(By Gender)" DataField="TotalNewCust"  HtmlEncode="false"  ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>--%>
                            </Columns>
                        </asp:GridView>
            </div>

            
         </div>
        </div>

         <div class="col-md-12" style="margin-left:45px; margin-bottom: 5px;">
                <div class="input-group">
                    <asp:Label ID="lblNote" runat="server"></asp:Label>
                </div>
            </div>

          <div class="row" style="margin-bottom: 15px; ">
                <div class="col-md-12">
                        
                        <asp:GridView ID="gvCustList" runat="server" Width="90%" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                           
                             <Columns>

                                        <asp:BoundField HeaderText="Brc" DataField="BrCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                    <asp:BoundField HeaderText="Gender" DataField="Gender" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                       <asp:BoundField HeaderText="Civil Satus" DataField="CivilSatus" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>




                                  <asp:BoundField HeaderText="Date Of Birth" DataField="DateOfBirth" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                               

                                  <asp:BoundField HeaderText="Age" DataField="Age" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  
                                 </Columns>
                        </asp:GridView>

                </div>
         </div>

         
    </div>

   <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>
    


<script src="external/jquery/jquery.js"></script>
<script src="jquery-ui.js"></script>

    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
</script>


    <!-- ################################################# START #################################################### -->

    <script type="text/javascript">
        $(function () {
            $(".dateCurr").datepicker();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".dateCurr").datepicker();
                }
            });
        };
    </script>
    <!-- ################################################# START #################################################### -->

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Customer Count",
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


     <script type="text/javascript">
         function showDiv() {
             document.getElementById('myHiddenDiv').style.display = "";
             setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

         }
    </script>
    <!-- ################################################# END #################################################### -->



    
<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridAddEmployee() {
            $(function () {
                $("#ShowAddEmployee").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridCustomerTransaction() {
            $(function () {
                $("#ShowCustomerTransaction").dialog({
                    title: "Customer Transaction",
                    //position: ['center', 20],

                    width: '720px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowCustomerTransaction").parent().appendTo($("form:first"));
            });
        }
    </script>
    	

    <div id="ShowCustomerTransaction" style="display: none;">
        <br />
        <asp:Label ID="lblCustName" runat="server" Text="Label"></asp:Label>
	    <br />
         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px; text-align: center; margin-top:15px;">
                    <asp:GridView ID="gvHistory" runat="server" Width="100%" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                             <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="SalesDate" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                         <asp:BoundField HeaderText="Receipt No" DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Item Description" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="40%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                 <asp:BoundField HeaderText="Net Amount" DataField="NetAmount" ReadOnly="True"  DataFormatString="{0:N2}" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                 </Columns>
                    </asp:GridView>
                </div>
         </div>

        


    </div>

<!-- ################################################# END #################################################### -->  

</asp:Content>
