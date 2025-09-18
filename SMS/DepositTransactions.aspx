<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepositTransactions.aspx.cs" Inherits="SMS.DepositTransactions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                <asp:Label ID="Label2" runat="server" Text="Deposit Transactions"></asp:Label>
            </div>
        </div>
        
       

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Branch :
                </div>
                <div class="col-sm-10">
                     <asp:DropDownList ID="ddBranch" runat="server" Width="245px" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged" ></asp:DropDownList>                    
                </div>
            </div>
        </div>

        
        <div class="row" style="margin-bottom: 20px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Status :
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                        <asp:RadioButton ID="rbAll" runat="server" Text="&nbsp;&nbsp; ALL " Checked="true" GroupName="grpDep" AutoPostBack="True" OnCheckedChanged="rbAll_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbPaid" runat="server" Text="&nbsp;&nbsp; FULLY PAID" GroupName="grpDep" AutoPostBack="True" OnCheckedChanged="rbPaid_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbUnpaid" runat="server" Text="&nbsp;&nbsp; UNPAID" GroupName="grpDep" AutoPostBack="True" OnCheckedChanged="rbUnpaid_CheckedChanged" />
        
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="margin-bottom: 20px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                      <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        &nbsp;&nbsp;&nbsp;
                       <button id="btnExcel" onserverclick="btnExcel_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                    </div>
                
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-12" style="margin-left:5px;">
                      <asp:GridView ID="gvDeposits" runat="server" Width="98%" ShowFooter="true" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                           <FooterStyle BackColor="PaleGoldenrod" Font-Bold="true" />
                             <Columns>
                                        
                                        <asp:BoundField HeaderText="Branch" DataField="BrName"  ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Receipt No" DataField="ReceiptNo"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Customer Name" DataField="CustomerName"  ReadOnly="True" ItemStyle-Width="12%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Item Code" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                     <asp:BoundField HeaderText="Plu Code" DataField="vPluCode"  ReadOnly="True" ItemStyle-Width="4%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                       <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:TemplateField  HeaderText="Total Session" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalSession" runat="server" Text='<%# Eval("TotalSession") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Session <br /> Availed"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNoSession" runat="server" Text='<%# Eval("NoSession") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField  HeaderText="Item <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemAmount" runat="server" Text='<%# Eval("ItemAmount","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Deposit <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepAmountt" runat="server" Text='<%# Eval("DepAmount","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Due <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueAmount" runat="server" Text='<%# Eval("DueAmount","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Date Paid" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDatePaid" runat="server" Text='<%# Eval("DatePaid","{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Paid <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaidAmount" runat="server" Text='<%# Eval("PaidAmount","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                      <asp:BoundField HeaderText="Receipt No(Paid)" DataField="PaidReceiptNo"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                </Columns>
                      </asp:GridView>

             </div>
        </div>

    </div>

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
                    title: "Deposit Transaction",
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



</asp:Content>
