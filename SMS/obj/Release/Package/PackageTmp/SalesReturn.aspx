<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesReturn.aspx.cs" Inherits="SMS.SalesReturn" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
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
        .ChkBoxClass input {width:25px; height:25px;}



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 20px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                    <h4>SALES RETURN TRANSACTION</h4>
                </div>
                <div class="col-md-4 text-center">
                
                </div>
            </div>
        </div>
  <!-- ################################################# START #################################################### -->
        <div class="form-group col-md-12">
            <div class="row">
                <div class="col-sm-12">

                    <div class="col-sm-2 text-right">
                        Series No.
                    </div>
                    <div class="col-sm-3">
                        <div class="input-group">
                            <asp:TextBox ID="txtOrderNo" runat="server" class="form-control" ReadOnly="true" Style="width: 185px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <%--<button id="btnRefresh" onserverclick="btnRefresh_Click" type="submit" runat="server" class="btn btn-default"><span class="glyphicon glyphicon-refresh"></span></button>--%>
                        </div>
                    </div>

                    <div class="col-sm-2 text-right">
                        Date
                    </div>
                    <div class="col-sm-3">
                        <div class="input-group">
                            <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" class="form-control" Style="width: 90px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqDate" runat="server"
                                ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDate"
                                ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                ForeColor="Red"
                                ErrorMessage="Invalid date format"
                                ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-sm-2 text-right">
                        <button id="btnPrintPreview" validationgroup="grpPrint"  onserverclick="btnPrintPreview_Click" visible="false" type="submit" runat="server"  class="btn btn-success" >Y E S&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                        <asp:Button ID="Button1" ValidationGroup="grpPrint" runat="server" Text="Button" OnClick="Button1_Click" Visible="false" />
                    </div>

                    
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">

                    <div class="col-sm-2 text-right">
                        OR No.
                    </div>
                    <div class="col-sm-7">
                        <div class="input-group">
                        <asp:TextBox ID="txtManualOR" runat="server" class="form-control decimalnumbers-only" Style="width: 135px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator111" runat="server"
                            ControlToValidate="txtManualOR" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required(put 0 if follow up session only)"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row" style="margin-bottom: 5px; display:none;">
                <div class="col-sm-12">

                    <div class="col-sm-2 text-right">
                        Customer Name
                    </div>
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:DropDownList ID="ddCustomerName" runat="server" Style="width: 395px;" CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                            

                        </div>
                    </div>
                </div>
            </div>
            
         <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Customer Name
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                        <asp:TextBox ID="txtCustomer" CssClass="form-control" Width="395px" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:TextBox ID="txtCustID" CssClass="form-control" Width="95px" ReadOnly="true" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSearchBranch" AccessKey="s" runat="server" CssClass="btn btn-warning"  ValidationGroup="grpSearch" Text="[ALT + S] SEARCH" OnClick="btnSearchBranch_Click" />
                        &nbsp;&nbsp;&nbsp;
                            <%--<asp:Button ID="btnShowSessionBalance" runat="server" class="btn btn-sm btn-primary" ValidationGroup="grpSearch" Text="Show Session Balance" OnClick="btnShowSessionBalance_Click" />--%>


                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server"
                            ControlToValidate="txtCustomer" Display="Dynamic" ValidationGroup="grpSearch" ForeColor="Red"
                            ErrorMessage="Supply customer name"></asp:RequiredFieldValidator>
                    </div>
                </div>

            </div>
        </div>


            

            
   

            <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        Replacement Item
                    </div>
                    <div class="col-sm-10">
                        <div class="input-group">
            
                            <asp:DropDownList ID="ddITemFG" runat="server" Style="width: 705px;"  CssClass="form-control chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddITemFG_SelectedIndexChanged"></asp:DropDownList>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblType" runat="server" class="label label-default" Width="65px" Visible="false" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblNoSession" runat="server" class="label label-default" Width="25px" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblIsKit" runat="server" class="label label-default" Width="15px" Visible="false" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblWithInv" runat="server" class="label label-default" Width="15px" Visible="false" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                       <%--<asp:LinkButton ID="lnkSelect" runat="server" Visible="false" Enabled="false" class="btn btn-default" OnClick="lnkSelect_Click">Select Batch No.</asp:LinkButton>--%>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row" style="margin-bottom: 5px;">
                <div class="col-sm-12">

                    <div class="col-sm-2 text-right">
                        Balance
                    </div>
                    <div class="col-sm-2">
                        <div class="input-group">
                            <asp:TextBox ID="txtAvailable" runat="server" class="form-control" ReadOnly="true" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:TextBox ID="txtItemID" runat="server" class="form-control" Visible="false" ReadOnly="true" Style="width: 55px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqAvail" runat="server" InitialValue="0"
                                ControlToValidate="ddITemFG" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    <div class="col-sm-2 text-right">
                        <div class="input-group">
                          <asp:TextBox ID="TextBox1" runat="server" class="form-control" ReadOnly="true" Text="SRP" Style="width: 55px; text-align: right;"></asp:TextBox>                        
                          <asp:TextBox ID="txtSRP" runat="server" class="form-control decimalnumbers-only" Style="width: 85px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>                        
                        </div>
                    </div>

                    <div class="col-sm-3 text-left">
                        <div class="input-group">
                            <asp:TextBox ID="TextBox2" runat="server" class="form-control" ReadOnly="true" Text="No. of Sessions Availed : " Style="width: auto; text-align: right;"></asp:TextBox>                        
                            <asp:TextBox ID="txtNoSesson" runat="server" class="form-control decimalnumbers-only" Style="width: 50px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>                         
                        </div>
                    </div>

                </div>
            </div>
            

            

            <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        Quantity
                    </div>
                    <div class="col-sm-5">
                        <div class="input-group">
                            <asp:TextBox ID="txtQty" runat="server" class="form-control decimalnumbers-only" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;
                            <asp:RequiredFieldValidator ID="ReqQty" runat="server"
                                ControlToValidate="txtQty" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvQty" runat="server"
                                ErrorMessage="Insufficient balance!" ValidationGroup="grpIss" Display="Dynamic"
                                ControlToCompare="txtAvailable" ControlToValidate="txtQty" Type="Double" Operator="LessThanEqual"
                                ForeColor="Red"></asp:CompareValidator>
                        </div>
                    </div>
                    <div class="col-sm-1">
                    </div>

                </div>
            </div>

          <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        Performed By :
                    </div>
                    <div class="col-sm-10">
                        <div class="input-group">
                            <asp:DropDownList ID="ddDrs" runat="server" Style="width: 415px;"  CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredDoctors" runat="server" InitialValue="0" Enabled="false"
                                ControlToValidate="ddDrs" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required if item is service"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row" style="margin-bottom: 5px;">
                <div class="col-sm-12">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-2">
                        <button id="btnSave" onserverclick="btnSave_Click" type="submit" runat="server" class="btn btn-primary" validationgroup="grpIss">ADD&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-save"></span></button>
                    </div>

                </div>

            </div>



            <%--#######################################START################################--%>
            <div class="row" style="margin-bottom: 20px;">
                <div class="col-sm-12">
                    <div class="panel panel-default ">
                        <div class="panel-heading">Transaction Detail</div>
                        <div class="panel-body">

                            <asp:GridView ID="grdItemGrid" runat="server" ShowFooter="true"
                                CssClass="table table-bordered active active" AutoGenerateColumns="false" OnRowDataBound="grdItemGrid_RowDataBound" OnRowDeleting="grdItemGrid_RowDeleting">

                                <Columns>
                                    <asp:BoundField DataField="vItemID" HeaderText="vItemID" ItemStyle-Width="5" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="vFGCode" HeaderText="vFGCode" ItemStyle-Width="10" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="SRP">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblvUnitCost" runat="server"
                                                Text='<%#Eval("vUnitCost","{0:0.##}")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="vQty" HeaderText="Qty" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TotalSession" HeaderText="Total Session" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="NoSession" HeaderText="No Session" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="sConstant" HeaderText="sConstant" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="DiscDescription" HeaderText="Discount Availed" HtmlEncode="false" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                    <%--<asp:BoundField DataField="DiscountsAmt" HeaderText="DiscountsAmt"  ItemStyle-Width="15" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />--%>

                                    <asp:TemplateField HeaderText="Discounts" FooterStyle-HorizontalAlign="Right">
                                        <FooterStyle HorizontalAlign="Right" ForeColor="Blue"></FooterStyle>
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountsAmt" runat="server"
                                                Text='<%#Eval("DiscountsAmt","{0:N2}")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:BoundField DataField="VatExemption" HeaderText="VatExemption" ItemStyle-Width="10" ItemStyle-HorizontalAlign="Center"/>--%>

                                    <asp:TemplateField HeaderText="VatExemption" FooterStyle-HorizontalAlign="Right" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                        <FooterStyle HorizontalAlign="Right" ForeColor="Blue"></FooterStyle>
                                        <HeaderStyle HorizontalAlign="Center" Width="5px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblVatExemption" runat="server"
                                                Text='<%#Eval("VatExemption","{0:N2}")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="vDiscPerc" HeaderText="Perc(%)" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />

                                    <asp:TemplateField HeaderText="Amount" FooterStyle-HorizontalAlign="Right">
                                        <FooterStyle HorizontalAlign="Right" ForeColor="Blue"></FooterStyle>
                                        <HeaderStyle HorizontalAlign="Center" Width="95px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmount" runat="server"
                                                Text='<%#Eval("NetAmount","{0:N2}")%>'></asp:Label>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAmountDetail" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ItemType" HeaderText="ItemType" ItemStyle-Width="20" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="IsKit" HeaderText="IsKit" ItemStyle-Width="10" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="IsDeposit" HeaderText="IsDeposit" ItemStyle-Width="10" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="PerformedBy" HeaderText="Performed By" HtmlEncode="false" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TransactionID" HeaderText="TransactionID" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="IsDepositPaid" HeaderText="IsDepositPaid" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                    
                                    <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-danger" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-12">
                        <div class="col-sm-4 text-right">
                        </div>
                         <div class="col-sm-4 text-center">
                                <div id='myHiddenDiv' style="width: 18%; float: left; height: 40px; line-height: 40px; text-align: center;">
                                    <img src='' id='myAnimatedImage' alt="" height="40" />
                                </div>
                        </div>
                        <div class="col-sm-4 text-right">
                            <button id="btnSubmit" onserverclick="btnSubmit_Click" onclick="showDiv();" type="submit" runat="server" class="btn btn-success">P O S T&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-ok"></span></button>
                        </div>

                </div>
             </div>
        </div>
    <!-- ################################################# END TRAN DETAI; #################################################### -->


        <%--#######################################END################################--%>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>
            </Triggers>
            <ContentTemplate>

                <div class="form-group col-md-12">
                    <div class="col-sm-6">
                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    Total Amount
                                </div>
                                <div class="col-sm-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtTotalAmount" runat="server" class="form-control decimalnumbers-only" ReadOnly="true" Style="width: 105px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    Amount to be paid
                                </div>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAmtToPaid" runat="server" class="form-control decimalnumbers-only" Style="width: 105px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:TextBox ID="xtlblAmounttobepaid" runat="server" class="form-control decimalnumbers-only" ForeColor="Transparent" BackColor="Transparent"  BorderColor="Transparent" ReadOnly="true" Style="width: 105px; text-align: right;" BorderStyle="None" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11111" runat="server"
                                            ControlToValidate="txtAmtToPaid" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Payment Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    Total Amount Render
                                </div>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtTotalAmtRender" runat="server" class="form-control decimalnumbers-only" ReadOnly="true" Style="width: 115px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                </div>
                                <div class="col-sm-8">
                                    <asp:CompareValidator ID="cvPayment" runat="server"
                                        ErrorMessage="Payment exceed with total amount to paid" ValidationGroup="grpPayment"
                                        ControlToCompare="xtlblAmounttobepaid" ControlToValidate="txtAmtToPaid" Type="Double" Operator="LessThanEqual"
                                        ForeColor="Red"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>





                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    M. O. P.
                                </div>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddPayment" runat="server" AutoPostBack="true" class="form-control" Style="width: 275px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" OnSelectedIndexChanged="ddPayment_SelectedIndexChanged"></asp:DropDownList>
                                        <%--       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                        ControlToValidate="ddPayment" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                        ErrorMessage="Please select M.O.P."></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    Bank
                                </div>
                                <div class="col-sm-5">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddBanks" runat="server" Enabled="false" class="form-control" Style="width: 215px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFBanks" runat="server" InitialValue="0" Enabled="false"
                                            ControlToValidate="ddBanks" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Please select bank"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    <asp:Label ID="lblBatchVoucher" runat="server" Text="Batch #"></asp:Label>
                                </div>
                                <div class="col-sm-5">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtBatch" runat="server" Enabled="false" class="form-control" Style="width: 195px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rbBatch" runat="server" Enabled="false"
                                            ControlToValidate="txtBatch" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    <asp:Label ID="lblRefNo" runat="server" Text="Ref. No."></asp:Label>
                                </div>
                                <div class="col-sm-5">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtReferenceNumber" runat="server" Enabled="false" CssClass="form-control" Style="width: 295px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfReferenceNumber" runat="server" Enabled="false"
                                            ControlToValidate="txtReferenceNumber" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                </div>
                                <div class="col-sm-5">
                                    <asp:Button ID="btnSavePayment" runat="server" Text="ADD PAYMENT" class="btn btn-warning" OnClick="btnSavePayment_Click" ValidationGroup="grpPayment" />
                                </div>
                            </div>
                        </div>

                    </div>

                    <%--############### END #################--%>
                    <%--############### start #################--%>

                    <div class="col-sm-6">
                        <div class="panel panel-default ">
                            <div class="panel-heading">Payment Breakdown</div>
                            <div class="panel-body">
                                <asp:GridView ID="grdPayment" runat="server"
                                    ShowFooter="true"
                                    CssClass="table table-bordered active active" AutoGenerateColumns="false" EmptyDataText="No payment has been found." OnRowDeleting="grdPayment_RowDeleting" OnRowDataBound="grdPayment_RowDataBound">

                                    <Columns>
                                        <asp:BoundField DataField="PaymentMode" HeaderText="Mode of Payment" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                                        <%--<asp:BoundField DataField="TotalAmount" HeaderText="Amount" ItemStyle-Width="90" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />--%>
                                        <asp:TemplateField HeaderText="Amount" FooterStyle-HorizontalAlign="Right">
                                            <FooterStyle HorizontalAlign="Right" ForeColor="Blue"></FooterStyle>
                                            <HeaderStyle HorizontalAlign="Center" Width="95px" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server"
                                                    Text='<%#Eval("TotalAmount")%>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalAmt" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="BankName" HeaderText="Bank/Partner" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" ItemStyle-Width="110" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ReferenceNumber" HeaderText="ReferenceNumber" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />

                                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-danger" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <br />
                        
                        
                    </div>
                    <%--############### END #################--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    


    <br />
    <br />
    <br />
    <br />

    <!-- ################################################# START #################################################### -->
    <!-- ################################################# END #################################################### -->
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

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Sales Transaction",
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
        function ShowSuccessMsg() {
            $(function () {
                $("#messageSuccess").dialog({
                    title: "Posting of Transaction",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            //window.location = '<%= ResolveUrl("~/SalesForTheDay.aspx") %>';
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


    <!-- ################################################# END #################################################### -->

   


    <%--*******************************************************************************--%>

    <script type="text/javascript">
        function ShowConfirmSubmit() {
            $(function () {
                $("#ConfirmSubmit").dialog({
                    title: "Sales Record",
                    width: '425px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
                $("#ConfirmSubmit").parent().appendTo($("form:first"));
            });
        }
    </script>

    <div id="ConfirmSubmit" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblConfirmSubmit" runat="server" />
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Button ID="btnConfirmSubmit" runat="server" Text="YES" class="btn btn-success btn-sm" OnClick="btnConfirmSubmit_Click"    />
    </div>
    <%--"--%>



    <script type="text/javascript">
        function showDiv() {
            document.getElementById('myHiddenDiv').style.display = "";
            setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

        }
    </script>
    <!-- ################################################# END #################################################### -->
    
    











    <%--*******************************************************************************--%>

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



<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        $(".decimalnumbers-only").keypress(function (e) {
            if (e.which == 46) {
                if ($(this).val().indexOf('.') != -1) {
                    return false;
                }
            }

            if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".decimalnumbers-only").keypress(function (e) {
                        if (e.which == 46) {
                            if ($(this).val().indexOf('.') != -1) {
                                return false;
                            }
                        }

                        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                }
            });
        };

    </script>
    <!-- ################################################# END #################################################### -->
 
     <%--.data("placeholder", "Select Framework...").chosen();--%>

    <!-- ################################################# START #################################################### -->

    <%--*******************************************************************************--%>

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
                    heigt: '400px',
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

        
    
        <div id="ShowCustomer" style="display: none;" >
            <div style="overflow:auto; max-height:500px;">
                <asp:GridView ID="gvCustomerList" runat="server"  AutoGenerateColumns="False" OnRowUpdating="gvCustomerList_RowUpdating" >
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


<!-- ################################################# END #################################################### -->

    <%--<script type="text/javascript">
        function ShowPrintPreview() {
            $(function () {
                $("#messagePrintPreview").dialog({
                    title: "Print Receipt",
                    width: '550px',
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

    <div id="messagePrintPreview" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgPrintPreview" runat="server" />
                &nbsp;&nbsp;
                        <br />
                        <br />
                     <div id="dvReport">
                         <div class="row">
                                <div class="col-sm-12">
                                    <div class="col-sm-10 text-left">
  
                                        <iframe id="I1" runat="server" frameborder="0" width="450" height="300" src="SalesReturnReceipt.aspx?SeriesNo="></iframe>
  
                                    </div>
                                    <div class="col-sm-1 text-left">
                                    </div>
                                </div>
                          </div>
                       </div>                       
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>
    
    <!-- ################################################# START #################################################### -->
    


    <%--<iframe id="yourid" src="yourpage.aspx?parentdata=whatever_data_to_be_captured">
</iframe>--%>




    </asp:Content>

