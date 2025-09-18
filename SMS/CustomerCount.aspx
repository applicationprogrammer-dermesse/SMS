<%@ Page Title="Customer Count" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerCount.aspx.cs" Inherits="SMS.CustomerCount" %>
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
                <asp:Label ID="Label2" runat="server" Text="Customer Count"></asp:Label>
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

        <div class="row" style="margin-bottom: 5px; margin-left:15px;">
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
        </div>
        <div class="row" style="margin-bottom: 15px; ">
            <div class="col-sm-12"  style="margin-left:5px;">
                <div class="col-md-3">
                      <asp:GridView ID="gvCustomerCount" runat="server" Width="90%" DataKeyNames="BrCode" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvCustomerCount_RowCommand" >
                             <Columns>
                                        <asp:BoundField HeaderText="Brc" DataField="BrcODE" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                 
                                       <asp:TemplateField HeaderText="Availed <br/>Service">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkServiceOnly" runat="server" CommandName="ViewListService"  Text='<%#Eval("NewService") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>




                                 <asp:TemplateField HeaderText="Availed <br/>Product Only<br/>(Buying)">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProduct" runat="server" CommandName="ViewListProduct"  Text='<%#Eval("NewProduct") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                   

                                 <asp:BoundField HeaderText="Total Customers<br/>(NEW)" DataField="TotalNewCust" HtmlEncode="false"  ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                            </Columns>
                        </asp:GridView>
            </div>

            <div class="col-md-3">
               <asp:GridView ID="gvActive" runat="server" Width="100%" DataKeyNames="BrCode" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvActive_RowCommand" >
                             <Columns>
                                        <asp:BoundField HeaderText="Brc" DataField="BrCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                               

                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                 
                                    <asp:TemplateField HeaderText="Active <br/>Availed Service">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkActiveS" runat="server" CommandName="ViewListAS"  Text='<%#Eval("ActiveService") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                 <asp:TemplateField HeaderText="Active <br/>Availed Product Only<br/>(Buying)">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkActiveP" runat="server" CommandName="ViewListAP"  Text='<%#Eval("ActiveProduct") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                 
                        </Columns>
                   </asp:GridView>
            </div>

            <div class="col-md-3">
               <asp:GridView ID="gvTransactionRecord" runat="server" Width="100%" DataKeyNames="BrName" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvTransactionRecord_RowCommand" >
                             <Columns>
                                       
                               <asp:BoundField HeaderText="Brc" DataField="BrCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="80px"/>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>





                                 <asp:TemplateField HeaderText="Transaction Records">
                                            <ItemStyle HorizontalAlign="Center" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkTransactionRecords" runat="server" CommandName="ViewTransactionRecords"   Text='<%#Eval("NoRecords") %>'  ForeColor="Blue"/>

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                 
                        </Columns>
                   </asp:GridView>
            </div>

            <div class="col-md-3">
               <asp:GridView ID="gvInactive" runat="server" Width="90%" DataKeyNames="BrCode" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvInactive_RowCommand" >
                             <Columns>
                                        <asp:BoundField HeaderText="Brc" DataField="BrCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                               

                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                 
                                    <asp:TemplateField HeaderText="Inactive <br/>Availed Service">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInactiveS" runat="server" CommandName="ViewListIS"  Text='<%#Eval("InactiveService") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                 <asp:TemplateField HeaderText="Inactive <br/>Availed Product<br/>(Buying)">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInactiveP" runat="server" CommandName="ViewListIP"  Text='<%#Eval("InactiveProduct") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                 
                        </Columns>
                   </asp:GridView>
            </div>
         </div>
        </div>


         <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-12" style="margin-left:45px; margin-bottom: 5px;">
                <div class="input-group">
                <asp:Label ID="lblNote" runat="server"></asp:Label>
                &nbsp;&nbsp;&nbsp;
                    <button id="btnExcelList" onserverclick="btnExcelList_Click" type="submit" runat="server" visible="false" class="btn btn-success">EXPORT LIST&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                    <button id="btnExcelListInactive" onserverclick="btnExcelListInactive_Click" type="submit" runat="server" visible="false" class="btn btn-success">EXPORT LIST&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                    <button id="btnExcelListTRansactionRecord" onserverclick="btnExcelListTRansactionRecord_Click" type="submit" runat="server" visible="false" class="btn btn-success">EXPORT LIST&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                </div>
            </div>
            <div class="col-md-12" style="margin-left:45px;">
                    <%--start--%>
                <asp:GridView ID="gvInactiveList" runat="server" Width="90%" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvInactiveList_RowCommand" >
                           
                             <Columns>

                                        <asp:BoundField HeaderText="Brc" DataField="BrCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="cID" DataField="CustID" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                               <%--   <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>--%>

                                  <asp:TemplateField HeaderText="Customer Name">
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInactiveCN" runat="server" CommandName="ViewListICN"  Text='<%#Eval("CustomerName") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                 <asp:BoundField HeaderText="Item Description" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Last Transaction <br /> Date" HtmlEncode="false" DataField="SalesDate" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Contact No." DataField="CelNo" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                 
                                 </Columns>
                        </asp:GridView>

                    <%--end --%>
                    

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

                                  <asp:BoundField HeaderText="Date" DataField="SalesDate" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Item Type" DataField="ItemType" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Ite mDescription" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Discounts Amt" DataField="DiscountsAmt" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Net Amount" DataField="NetAmount" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                 </Columns>
                        </asp:GridView>


                 <asp:GridView ID="gvTransactionRecordDetails" runat="server" Width="90%" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                           
                             <Columns>

                                        <asp:BoundField HeaderText="CustID" DataField="CustID" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="35%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  

                                  <asp:BoundField HeaderText="Receipt No" DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Customer Type" DataField="PatientStat" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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
