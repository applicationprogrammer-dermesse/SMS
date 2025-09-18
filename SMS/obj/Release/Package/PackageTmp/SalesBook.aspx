<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesBook.aspx.cs" Inherits="SMS.SalesBook" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 20px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>SALES BOOK</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>
        <hr />
        <br />

        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    SMS Last Transaction Date(Posted)
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                        <asp:Label ID="lblSMSTransactionDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Last Process Date(Sales Book)
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                        <asp:Label ID="lblLastProcessDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                        <asp:Button ID="btnProcess" runat="server" Text="Process Sales Book" CssClass="btn btn-primary btn-sm" OnClick="btnProcess_Click" />
                    </div>
                </div>
            </div>
        </div>
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
                        <asp:TextBox ID="txtDateFrom" runat="server" class="datePrev form-control" Style="width: 110px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateFrom"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpIss"></asp:RegularExpressionValidator>

                    </div>
                </div>
                

            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    TO
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtDateTo" runat="server" class="datePrev form-control" Style="width: 110px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateTo"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server"  Style="color: Red;"
                                            ValidationGroup="grpIss" Display="Dynamic" ControlToCompare="txtDateFrom"
                                            ControlToValidate="txtDateTo" Type="Date" Operator="GreaterThanEqual"
                                            ErrorMessage="Invalid date range"></asp:CompareValidator>

                    </div>
                </div>

            </div>
        </div>


        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Branch
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                    <asp:DropDownList ID="ddBranch" runat="server" class="form-control" Style="width: 195px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0"
                            ControlToValidate="ddBranch" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Please select branch"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Option
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:RadioButton ID="rbASC" runat="server" GroupName="grpOpt" Text="&nbsp;&nbsp; In Ascending Order" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbDESC" runat="server" GroupName="grpOpt" Checked="true" Text="&nbsp;&nbsp; In Descending Order" />
                    </div>
                </div>

            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" validationgroup="grpIss" class="btn btn-info">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        &nbsp;&nbsp;&nbsp;    
                        <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">EXPORT&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                    </div>
                </div>

            </div>
        </div>

        <br />
        <div class="row">
            <div style="overflow: auto; width: 100%; height: 400px; ">
                <asp:GridView ID="gvSalesBook" Width="98%" Font-Size="Small" runat="server" DataKeyNames="RecID" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" OnRowDeleting="gvSalesBook_RowDeleting" OnRowCommand="gvSalesBook_RowCommand" OnRowDataBound="gvSalesBook_RowDataBound">
                    <Columns>

                        <asp:BoundField HeaderText="ID" DataField="RecID" ReadOnly="True" ItemStyle-Width="3%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                         
                        <asp:BoundField HeaderText="Date" DataField="SalesDate" ReadOnly="True" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Previous<br />Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPrevGT_amt" runat="server" Text='<%# Eval("PrevGT_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Product<br />Gross Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblProduct_amt" runat="server" Text='<%# Eval("Product_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service<br />Gross Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblService_amt" runat="server" Text='<%# Eval("Service_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Product<br />Discount Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblProdDisc_amt" runat="server" Text='<%# Eval("ProdDisc_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
           
                        <asp:TemplateField HeaderText="Service<br />Discount Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblServDisc_amt" runat="server" Text='<%# Eval("ServDisc_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        

                        <asp:TemplateField HeaderText="Net Sales" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetSales_amt" runat="server" Text='<%# Eval("NetSales_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CASH" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCash_amt" runat="server" Text='<%# Eval("Cash_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Credit Card" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCCard_amt" runat="server" Text='<%# Eval("CCard_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Debit Card" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDebit_amt" runat="server" Text='<%# Eval("Debit_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Bank <br />Fund <br />Transfer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblBankTransfer_amt" runat="server" Text='<%# Eval("BankTransfer_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Voucher" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVoucher_amt" runat="server" Text='<%# Eval("Voucher_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Paymaya" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPaymaya_amt" runat="server" Text='<%# Eval("Paymaya_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Digital Payment" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDigital_amt" runat="server" Text='<%# Eval("Digital_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Gift Cheque" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGiftCheque_amt" runat="server" Text='<%# Eval("GiftCheque_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCurrGT_amt" runat="server" Text='<%# Eval("CurrGT_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="VAT <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVat_amt" runat="server" Text='<%# Eval("Vat_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="VAT Exempt <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVATExempt_amt" runat="server" Text='<%# Eval("VATExempt_amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Vatable <br />Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVatableAmt" runat="server" Text='<%# Eval("VatableAmt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:BoundField HeaderText="Last Receipt No" DataField="LastReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                                     <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Height="27px" />
                                            <ItemTemplate>
                                                <div class="input-group">
                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="" ToolTip="Delete" class="btn btn-sm btn-danger"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                        <asp:LinkButton ID="btnRePrint" runat="server" CommandName="RePrint" Text="" ToolTip="Reprint x and z" class="btn btn-sm btn-default"><span class="glyphicon glyphicon-print"></span></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                     </asp:TemplateField>

                    </Columns>
                </asp:GridView>

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
                    title: "Sales Book",
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

        <!-- ################################################# END #################################################### -->


<script type="text/javascript">
    function ShowConfirmMsg() {
        $(function () {
            $("#ConfirmDiv").dialog({
                title: "Delete Sales Book Record",
                width: '425px',
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            $("#ConfirmDiv").parent().appendTo($("form:first"));
        });
    }
</script>

<div id="ConfirmDiv" style="display:none;">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgConfirm" runat="server" />
                &nbsp;&nbsp;
                <asp:Label Text="" ID="lblDate" runat="server" />
                <br />
                <asp:Label Text="" ID="lblConfirmNote" runat="server" />
            </ContentTemplate>
         </asp:UpdatePanel>
                    <asp:Button ID="btnYes" runat="server" Text="YES"  class="btn btn-primary"   OnClick="btnYes_Click" />
</div>
</asp:Content>
