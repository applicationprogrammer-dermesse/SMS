<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesTarget.aspx.cs" Inherits="SMS.SalesTarget" %>
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


        .ui-dialog {
            position: fixed;
            padding: .1em;
            width: 300px;
            overflow: hidden;
        }
          .thick { border: 3px solid black; }

          .HeaderFreez
            {
            position:relative ;
            top:expression(this.offsetParent.scrollTop);
            z-index: 10;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 20px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>SALES TARGET</h4>
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
                <div class="col-sm-1 text-right">
                    <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">EXPORT&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
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


        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                </div>
                <div class="col-sm-9">
                    <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                </div>

            </div>
        </div>

        <br />
        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-12 text-left">
                    <asp:Label ID="Label3" runat="server" Text="Formula" ForeColor="Blue"></asp:Label>
                    <br />
                    <asp:Label ID="Label1" runat="server"  ForeColor="Maroon" Text="Target Sales To Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  = &nbsp;&nbsp;Daily Target * No Of Days(Date As Of column)"></asp:Label>
                    <br />
                    <asp:Label ID="Label2" runat="server"  ForeColor="Maroon" Text="Target Sales To Date Percentage&nbsp; =&nbsp;&nbsp; Net Sales / Target Sales To Date * 100"></asp:Label>
                    <br />
                    <asp:Label ID="Label4" runat="server"  ForeColor="Maroon" Text="Monthly Target Percentage&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; =&nbsp;&nbsp; Net Sales / Target Sales * 100"></asp:Label>
                </div>

            </div>
        </div>
        <div class="row">
           <%--  <div style="float: left; width: 100%;">
                <table id="Table1" runat="server" cellpadding="1" border="1" style="background-color: #FFFFFF;
                    border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;
                    width: 100%;">
                    <tr style="background-color: #E5E5FE">
                        <th style="width:100px;">
                            Area
                        </th>
                        <th style="width: 150px;">
                            Branch
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 100px;">
                           Target <br /> Sales
                        </th>
                        <th style="width: 10px;">
                        </th>
                    </tr>height: 300px;
                </table>
            </div>--%>
          
            <div style="overflow: auto; width: 100%;  ">
                <br />
                <asp:GridView ID="gvSalesSummary" Width="100%" ShowFooter="true"  runat="server" border="1" CellPadding="4" ForeColor="#333333" GridLines="both"  AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" OnRowDataBound="gvSalesSummary_RowDataBound">
                     <FooterStyle BackColor="PaleGoldenrod" Font-Bold="true" />
                    <Columns>
                        <asp:BoundField HeaderText="AREA" DataField="AREA" ReadOnly="True" ItemStyle-Width="100px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </asp:BoundField>


                        <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True"  ItemStyle-Width="100px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                         
                    

                        <asp:TemplateField HeaderText="Target <br /> Sales" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lbliTarget" runat="server" Text='<%# Eval("iTarget","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Daily Target<br /> Sales" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDailyTarget" runat="server" Text='<%# Eval("Daily Target","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        

                        <asp:TemplateField HeaderText="Gross Sales<br /> for The Day" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGrossSales4TheDay" runat="server" Text='<%# Eval("GrossSales4TheDay","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Discount<br /> for The Day" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDiscSales4TheDay" runat="server" Text='<%# Eval("DiscSales4TheDay","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Gross Sales <br />Product" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGrossSalesProduct" runat="server" Text='<%# Eval("GrossSalesProduct","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Discount <br />Product" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDiscSalesProduct" runat="server" Text='<%# Eval("DiscSalesProduct","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Sales <br />Product" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetSalesProduct" runat="server" Text='<%# Eval("NetSalesProduct","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Gross Sales <br />Service" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGrossSalesService" runat="server" Text='<%# Eval("GrossSalesService","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Discount <br />Service" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDiscSalesService" runat="server" Text='<%# Eval("DiscSalesService","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Net Sales <br />Service" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetSalesService" runat="server" Text='<%# Eval("NetSalesService","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Gift Check" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblGiftCheck" runat="server" Text='<%# Eval("GiftCheck","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="NET SALES" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblTOTALNETSALES" runat="server" Text='<%# Eval("TOTALNETSALES","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="VAT" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblVat" runat="server" Text='<%# Eval("Vat","{0:n4}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField HeaderText="NET OF VAT <br />SALES" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetOfVat" runat="server" Text='<%# Eval("NetOfVat","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="GROSS SALES" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblTOTALGROSSSALES" runat="server" Text='<%# Eval("TOTALGROSSSALES","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                     

                         <asp:TemplateField HeaderText="Date<br /> As Of" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblSalesAsOf" runat="server" Text='<%# Eval("SalesAsOf","{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Target Sales <br />To Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblTargetToDateSalesAsOf" runat="server" Text='<%# Eval("TargetToDateSalesAsOf","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                     

                        <asp:TemplateField HeaderText="Target Sales <br />To Date <br />Percentage" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblPerc" runat="server" Text='<%# Eval("Perc","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>

                            <FooterTemplate>
                                        <asp:Label ID="lblPercToDate" runat="server"></asp:Label>
                             </FooterTemplate>

                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Monthly Target<br />Percentage" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblPercMonth" runat="server" Text='<%# Eval("PercMonth","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>

                            <FooterTemplate>
                                        <asp:Label ID="lblPercMonthF" runat="server"></asp:Label>
                             </FooterTemplate>

                        </asp:TemplateField>

                        <asp:BoundField HeaderText="" DataField="" ReadOnly="True" ItemStyle-Width="30px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                </div>
               <br />
                
                <div style="width:50%; visibility:hidden;">
                    Summary Gross Sales
                    <br />
                    <asp:GridView ID="gvGrossSales" runat="server" AutoGenerateColumns="false" class="table table-bordered table-striped" >
                        <Columns>
                                   <asp:BoundField HeaderText="Type" DataField="ItemType" ReadOnly="True"  ItemStyle-Width="200px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblsAmt" runat="server" Text='<%# Eval("Amt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    Gross Sales Less VAT
                    <br />
                    <asp:GridView ID="gvGrossSalesLessVAT" runat="server" AutoGenerateColumns="false" class="table table-bordered table-striped" >
                        <Columns>
                                   <asp:BoundField HeaderText="Type" DataField="ItemType" ReadOnly="True"  ItemStyle-Width="200px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbls2Amt" runat="server" Text='<%# Eval("Amt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    Discounts
                    <br />
                    <asp:GridView ID="gvDiscounts" runat="server" AutoGenerateColumns="false" class="table table-bordered table-striped" >
                        <Columns>
                                   <asp:BoundField HeaderText="Type" DataField="ItemType" ReadOnly="True"  ItemStyle-Width="200px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbls3Amt" runat="server" Text='<%# Eval("Amt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    Net Of VAT
                    <br />
                    <asp:GridView ID="gvNetOfVAT" runat="server" AutoGenerateColumns="false" class="table table-bordered table-striped" >
                        <Columns>
                                   <asp:BoundField HeaderText="Type" DataField="ItemType" ReadOnly="True"  ItemStyle-Width="200px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbls4Amt" runat="server" Text='<%# Eval("Amt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    VAT
                    <br />
                    <asp:GridView ID="gvVAT" runat="server" AutoGenerateColumns="false" class="table table-bordered table-striped" >
                        <Columns>
                                   <asp:BoundField HeaderText="Type" DataField="ItemType" ReadOnly="True"  ItemStyle-Width="200px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lbls5Amt" runat="server" Text='<%# Eval("Amt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
        </div>
    </div>
    <br />
    <br />
    <br />

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
                    title: "Sales Target",
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










    
<%--<script type = "text/javascript">
    var GridId = "<%=gvSalesSummary.ClientID %>";
    var ScrollHeight = 300;
    window.onload = function () {
        var grid = document.getElementById(GridId);
        var gridWidth = grid.offsetWidth;
        var gridHeight = grid.offsetHeight;
        var headerCellWidths = new Array();
        for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
            headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
        }
        grid.parentNode.appendChild(document.createElement("div"));
        var parentDiv = grid.parentNode;
 
        var table = document.createElement("table");
        for (i = 0; i < grid.attributes.length; i++) {
            if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
            }
        }
        table.style.cssText = grid.style.cssText;
        table.style.width = gridWidth + "px";
        table.appendChild(document.createElement("tbody"));
        table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
        var cells = table.getElementsByTagName("TH");
 
        var gridRow = grid.getElementsByTagName("TR")[0];
        for (var i = 0; i < cells.length; i++) {
            var width;
            if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                width = headerCellWidths[i];
            }
            else {
                width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
            }
            cells[i].style.width = parseInt(width - 3) + "px";
            gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
        }
        parentDiv.removeChild(grid);
 
        var dummyHeader = document.createElement("div");
        dummyHeader.appendChild(table);
        parentDiv.appendChild(dummyHeader);
        var scrollableDiv = document.createElement("div");
        if(parseInt(gridHeight) > ScrollHeight){
            gridWidth = parseInt(gridWidth) + 17;
        }
        scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
        scrollableDiv.appendChild(grid);
        parentDiv.appendChild(scrollableDiv);
    }
    </script>


--%>


     








</asp:Content>
