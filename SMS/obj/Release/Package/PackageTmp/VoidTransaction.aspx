<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VoidTransaction.aspx.cs" Inherits="SMS.VoidTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 15px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Void Transaction</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 8px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    <a href="SalesForTheDay.aspx" class="btn btn-sm btn-default">Back to Previous Page</a>
                </div>
                 <div class="col-sm-9 text-left">
                     <asp:Label ID="Label1" runat="server" ForeColor="Maroon" Text="Are you sure you want to void this transaction?"></asp:Label>
                     &nbsp;&nbsp;
                     <asp:Button ID="btnVoid" runat="server" Text="YES" CssClass="btn btn-sm btn-danger" OnClick="btnVoid_Click" />
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Series No. :
                </div>
                <div class="col-sm-9 text-left">
                    <asp:Label ID="lblSeriesNo" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Customer Name :
                </div>
                <div class="col-sm-9 text-left">
                    <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Patient Status :
                </div>
                <div class="col-sm-9 text-left">
                    <asp:Label ID="lblPatientStatus" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>

        
         <div class="row">
            <div class="col-sm-12">
                <div class="col-sm-2 text-left">
                    Item Availed
                </div>
                 <div class="col-sm-9 text-left">
        
                </div>
            </div>
        </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:GridView ID="gvViewTransaction" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                <Columns>


                    <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Description" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="385px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>



                    <asp:TemplateField HeaderText="SRP">
                        <HeaderStyle HorizontalAlign="Center" Width="85px" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblPrice" runat="server"
                                Text='<%#  Eval("vUnitCost","{0:n2}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Qty">
                        <HeaderStyle HorizontalAlign="Center" Width="85px" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblQty" runat="server"
                                Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="No of Session" DataField="NoSession" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ItemStyle-Width="15px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Discount Availed" DataField="DiscDescription" ReadOnly="True" ItemStyle-Width="185px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="% Disc" DataField="vDiscPerc" ReadOnly="True" DataFormatString="{0:###0;(###0);0}" ItemStyle-Width="65px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Disc Amount">
                        <HeaderStyle HorizontalAlign="Center" Width="85px" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblDiscAmount" runat="server"
                                Text='<%#  Eval("DiscountsAmt","{0:n2}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Amount">
                        <HeaderStyle HorizontalAlign="Center" Width="85px" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server"
                                Text='<%#  Eval("NetAmount","{0:n2}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:BoundField DataField="ItemType" HeaderText="ItemType" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                        <HeaderStyle Width="5px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
        <br />
    <div class="row">
            <div class="col-sm-12">
                <div class="col-sm-2 text-left">
                    Payment Detail
                </div>
                 <div class="col-sm-9 text-left">
        
                </div>
            </div>
        </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:GridView ID="gvPaymentDetail" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                <Columns>


                    <asp:BoundField HeaderText="Mode of Payment" DataField="PaymentMode" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Amount">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblPriceAmt" runat="server"
                                Text='<%#  Eval("TotalAmount","{0:n2}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="Bank Name" DataField="BankName" ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>


                     <asp:BoundField HeaderText="Batch Number" DataField="BatchNumber" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                     <asp:BoundField HeaderText="Reference Number" DataField="ReferenceNumber" ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    


                    
                </Columns>
            </asp:GridView>
        </div>
    </div>

</div>

    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageSuccess").dialog({
                    title: "Voiding of Transaction",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/SalesForTheDay.aspx") %>';
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

</asp:Content>
