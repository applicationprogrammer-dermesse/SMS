<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransactionQuery.aspx.cs" Inherits="SMS.TransactionQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />
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

       /*#ShowCustomer {
  overflow: scroll;
}*/

.box-content { max-height: 200px; overflow: scroll; }



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 25px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Transaction Query</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>
        <hr />
        <br />
        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                </div>
                <div class="col-sm-9">
                    Covered Date
                </div>
                <div class="col-sm-1 text-right">
                </div>

            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    FROM 
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtDateFrom" runat="server" class="datePrev form-control" Style="width: 105px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateFrom"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Style="color: Red;"
                            ValidationGroup="grpIss" Display="Dynamic" ControlToCompare="txtDateFrom"
                            ControlToValidate="txtDateTo" Type="Date" Operator="GreaterThanEqual"
                            ErrorMessage="Invalid date range"></asp:CompareValidator>

                    </div>
                </div>
                <div class="col-sm-1 text-right">
                    <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" visible="false" runat="server" class="btn btn-success">EXPORT&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                </div>

            </div>
        </div>

        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    TO
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtDateTo" runat="server" class="datePrev form-control" Style="width: 105px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateTo"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                    </div>
                </div>

            </div>
        </div>

    <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Search Customer Record
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtCustomer" CssClass="form-control" Width="405px" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:TextBox ID="txtCustID" CssClass="form-control" Width="95px" ReadOnly="true" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSearchBranch" runat="server" CssClass="btn btn-warning"  ValidationGroup="grpSearch" Text="SEARCH" OnClick="btnSearchBranch_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" validationgroup="grpSearch" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server"
                            ControlToValidate="txtCustomer" Display="Dynamic" ValidationGroup="grpSearch" ForeColor="Red"
                            ErrorMessage="Supply Customer Name"></asp:RequiredFieldValidator>
                    </div>
                </div>

            </div>
        </div>

        <div class="col-sm-12" style="margin-bottom: 5px;">
                                <div class="col-sm-2 text-right">
                                    Search Item Availment
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Style="width: 650px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <button id="btnItem" onserverclick="btnItem_Click" type="submit" runat="server" validationgroup="grpItem" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0"
                                                ControlToValidate="ddITemFG" Display="Dynamic" ValidationGroup="grpItem" ForeColor="Red"
                                                ErrorMessage="Select item"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
         </div>



        <%--<div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Customer Name
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddCustomerName" runat="server" class="form-control chosen-select" Style="width: 695px;"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" InitialValue="0"
                            ControlToValidate="ddCustomerName" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Select customer"></asp:RequiredFieldValidator>
                    </div>
                </div>

            </div>
        </div>--%>

        
        <br />
        <div class="row">
            <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                    <asp:GridView ID="gvCustomer" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover">
                        <Columns>
                            <asp:BoundField HeaderText="Receipt No." DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Date" DataField="Date" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                                <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                                <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                                <asp:BoundField HeaderText="Item Description" DataField="vDESCRIPTION" ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="SRP">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblvUnitCost" runat="server"
                                        Text='<%#  Eval("vUnitCost","{0:N2}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Qty">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblvQty" runat="server"
                                        Text='<%#  Eval("vQty","{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField HeaderText="Discount" DataField="DiscDescription" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Discounts Amt">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountsAmt" runat="server"
                                        Text='<%#  Eval("DiscountsAmt","{0:n2}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Net Amt">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmount" runat="server"
                                        Text='<%#  Eval("NetAmount","{0:n2}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:GridView ID="gvItem" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover">
                        <Columns>
                            <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Date" DataField="Date" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                                <asp:BoundField HeaderText="Contact No" DataField="ContactNo" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Email Address" DataField="EmailAddress" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>


                                <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                                <asp:BoundField HeaderText="Item Description" DataField="vDESCRIPTION" ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            
                            <asp:TemplateField HeaderText="Qty">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lblvQty" runat="server"
                                        Text='<%#  Eval("vQty","{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>


    <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>





    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
    </script>

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Transaction Query",
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
    <!-- ################################################# START #################################################### -->

    <script type="text/javascript">
        $(function () {
            $(".datePrev").datepicker({
                maxDate: new Date, minDate: new Date(2007, 6, 12)
            });
        });

        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //if (prm != null) {
        //    prm.add_endRequest(function (sender, e) {
        //        if (sender._postBackSettings.panelsToUpdate != null) {
        //            $(".datePrev").datepicker({
        //                maxDate: new Date, minDate: new Date(2007, 6, 12)
        //            });
        //         }
        //    });
        //};
    </script>

    <script type="text/javascript">
        $(function () {
            $(".dateAll").datepicker({
            });
        });

        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //if (prm != null) {
        //    prm.add_endRequest(function (sender, e) {
        //        if (sender._postBackSettings.panelsToUpdate != null) {
        //            $(".datePrev").datepicker({
        //                maxDate: new Date, minDate: new Date(2007, 6, 12)
        //            });
        //         }
        //    });
        //};
    </script>

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

    <%--*******************OnClientClick="return CloseGridSessionBalance();" ************************************************************--%>




</asp:Content>
