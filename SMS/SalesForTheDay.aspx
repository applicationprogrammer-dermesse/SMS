<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesForTheDay.aspx.cs" Inherits="SMS.SalesForTheDay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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

        #ShowItemBatches {
            padding: .4em 1em .4em 20px;
            text-decoration: none;
            position: relative;
        }

            #ShowItemBatches span.ui-icon {
                margin: 0 5px 0 0;
                position: absolute;
                left: .2em;
                top: 50%;
                margin-top: -8px;
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
    
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Transaction Sales for the Day</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>


        <hr />
        <br />
         <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-2 text-right">
                    Transaction Date : 
                </div>
                <div class="col-sm-3">
                    <div class="input-group">
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-2 text-right">
                    Branch : 
                </div>
                <div class="col-sm-3">
                    <div class="input-group">
                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="margin-top:30px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-left">
                    <asp:Label ID="lblDet" runat="server" Text="Detail" class="form-control"></asp:Label>
                </div>
                <div class="col-sm-1 text-left">
                    <button id="btnPrintDet" onserverclick="btnPrintDet_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
               <asp:GridView ID="gvSalesForTheDay" runat="server" Width="100%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ReceiptNo" OnRowCommand="gvSalesForTheDay_RowCommand" OnRowDataBound="gvSalesForTheDay_RowDataBound"  >
                                    <Columns>
                         <%--    <asp:BoundField HeaderText="Branch" DataField="BrName"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>--%>

                                        <asp:BoundField HeaderText="Receipt No." DataField="ReceiptNo"  ReadOnly="True" ItemStyle-Width="11%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="PPC No" DataField="ORNo"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="AR/OR No" DataField="ARORNo"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Customer Name" DataField="CustomerName"  ReadOnly="True"  ItemStyle-Width="12%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                      

                                   <asp:TemplateField  HeaderText="Discounts Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountsAmt" runat="server" Text='<%# Eval("DiscountsAmt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("Net Amount","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="CASH" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCASHAMT" runat="server" Text='<%# Eval("CASH AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Credit Card" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCreditCardAMT" runat="server" Text='<%# Eval("Credit Card AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="EPS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDebitCardAMT" runat="server" Text='<%# Eval("Debit Card AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PAYMAYA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPaymayaAMT" runat="server" Text='<%# Eval("Paymaya AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Voucher" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherAMT" runat="server" Text='<%# Eval("Voucher AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gift Cheque" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGiftChequeAMT" runat="server" Text='<%# Eval("Gift Cheque AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Bank/Fund<br /> Transfer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblBankFundTransferAMT" runat="server" Text='<%# Eval("BankFundTransfer AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                       <asp:TemplateField HeaderText="Digital<br /> Payment" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDigitalPaymentAMT" runat="server" Text='<%# Eval("DigitalPayment AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                                         <asp:BoundField HeaderText="Status" DataField="Status"  ReadOnly="True"  ItemStyle-Width="3%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                           <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Height="27px" />
                                            <ItemTemplate>
                                                <div class="input-group">
                                                        <asp:LinkButton ID="btnViewDetailed" runat="server" CommandName="View" Text="" ToolTip="View Detail" class="btn btn-sm btn-warning"><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                                       &nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnVoidUnposted" runat="server" CommandName="Void" Text="" ToolTip="Void Transaction" class="btn btn-sm btn-danger"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                         
                                        </asp:TemplateField>
                                          </Columns>
                                </asp:GridView>
            </div>
        </div>
        <%--#########################################start ############################################--%>
        <div class="row" style="margin-top:25px; margin-bottom:1px;">
            <div class="col-sm-12">
                 <div class="col-sm-2 text-left">
                    <asp:Label ID="lblSummary" runat="server" Text="Summary" class="form-control"></asp:Label>
                </div>
                <div class="col-sm-4 text-left">
                    <button id="btnPrintSumm" onserverclick="btnPrintSumm_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:GridView ID="gvSummary" runat="server" Width="100%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false">

                    <Columns>
                        <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>


                        <asp:TemplateField HeaderText="Product<br />Gross Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblProductGrossAmtsumm" runat="server" Text='<%# Eval("Product - Gross Amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Product<br />Discount Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblProductDiscountAmtsumm" runat="server" Text='<%# Eval("Product - DiscountAmt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
            

                        <asp:TemplateField HeaderText="Service<br />Gross Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblServiceGrossAmtsumm" runat="server" Text='<%# Eval("Service - Gross Amt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Service<br />Discount Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblServiceDiscountAmtsumm" runat="server" Text='<%# Eval("Service - DiscountAmt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

               
                        <asp:TemplateField HeaderText="Net Sales" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetSalessumm" runat="server" Text='<%# Eval("Net Sales","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CASH" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCASHAMTsumm" runat="server" Text='<%# Eval("CASH AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Credit Card" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblCreditCardAMTsumm" runat="server" Text='<%# Eval("Credit Card AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="EPS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDebitCardAMTsumm" runat="server" Text='<%# Eval("Debit Card AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PAYMAYA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPaymayaAMTsumm" runat="server" Text='<%# Eval("Paymaya AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Voucher" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherAMTsumm" runat="server" Text='<%# Eval("Voucher AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gift Cheque" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGiftChequeAMTsumm" runat="server" Text='<%# Eval("Gift Cheque AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Bank/Fund Transfer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblBankFundTransferAMTsumm" runat="server" Text='<%# Eval("BankFundTransfer AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Digital<br /> Payment" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDigitalPaymentAMTsumm" runat="server" Text='<%# Eval("DigitalPayment AMT","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <%--################################################end############################################--%>
    </div>



<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowWarningMsg() {
        $(function () {
            $("#messageWarning").dialog({
                title: "Saels for the day",
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



    <!-- ################################################# START #################################################### -->




</asp:Content>
