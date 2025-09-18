<%@ Page Title="Reprint X and Z Reading" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RePrintXandZ.aspx.cs" Inherits="SMS.RePrintXandZ" %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Reprint X and Z Reading</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>


        <hr />
        <br />

        <div class="row" style="margin-bottom: 8px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    <a href="SalesBook.aspx" class="btn btn-sm btn-default"><i class="glyphicon glyphicon-arrow-left"></i>&nbsp;&nbsp;Back to previous page</a>
                </div>
                 <div class="col-sm-10 text-right">
                     
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Branch :
                </div>
                <div class="col-sm-8">
                    <div class="input-group">
                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Date :
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>

            </div>
        </div>
        <div class="row" style="margin-bottom: 2px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                 
                </div>
                <div class="col-sm-9">
                      <div class="input-group">
                        <button id="btnPreviewX"  onserverclick="btnPreviewX_Click" type="submit" runat="server" class="btn btn-default" >Print Preview(X READING)</button>
                          &nbsp;  &nbsp;
                        <button id="btnPrintX"  onserverclick="btnPrintX_Click" type="submit" visible="false" runat="server" class="btn btn-default" >PRINT X READING&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                        &nbsp;&nbsp;&nbsp;
                          <button id="btnPreviewZ"  onserverclick="btnPreviewZ_Click" type="submit" runat="server" class="btn btn-primary" >Print Preview(Z READING)</button>
                          &nbsp;  &nbsp;
                          <button id="btnPrintZ"   onserverclick="btnPrintZ_Click" type="submit"  visible="false" runat="server" class="btn btn-primary" >PRINT Z READING&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                      </div>
                </div>

            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-sm-12">
                
                <asp:GridView ID="gvSalesDetailed" Width="99%"  runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="ReceiptNo">
                             <Columns>
               

                                        <asp:BoundField HeaderText="Receipt No." DataField="ReceiptNo"  ReadOnly="True" ItemStyle-Width="13%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                      <asp:BoundField HeaderText="OR No" DataField="ORNo"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
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

                         <asp:TemplateField HeaderText="Paymaya" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
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

                        <asp:TemplateField HeaderText="Bank/Fund<br />Transfer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
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

                                           
                                          </Columns>
                        </asp:GridView>
            </div>
            
        </div>
        <br />
         
            
        

    </div>


    <!-- ################################################# START #################################################### -->
    <!-- ################################################# END #################################################### -->
   <%--<script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script> --%>


    <script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>

    <script type="text/javascript">
        function showDiv() {
            document.getElementById('myHiddenDiv').style.display = "";
            setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

        }
    </script>

    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
    </script>

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Reprint X and Z",
                    width: '335px',
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
        <!-- ################################################# END #################################################### -->
<%--<script type="text/javascript">
    function ShowNoSalesMsg() {
        $(function () {
            $("#messageNoSales").dialog({
                title: "Reprint X and Z",
                width: '335px',
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            $("#messageNoSales").parent().appendTo($("form:first"));
        });
    }
    </script>

    <div id="messageNoSales" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgNoSales" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnProceedsWithProcess" runat="server" Text="YES/Proceed" CssClass="btn btn-warning" OnClick="btnProceedsWithProcess_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>

        <!-- ################################################# END #################################################### -->


    <script type="text/javascript">
        function ShowWarningMsgWithPending() {
            $(function () {
                $("#messageWarningWithPending").dialog({
                    title: "End of Day Processing",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/DailySales.aspx") %>';
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageWarningWithPending" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWithPending" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- ################################################# END #################################################### -->




</asp:Content>
