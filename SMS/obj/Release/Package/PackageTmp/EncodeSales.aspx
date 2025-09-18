<%@ Page Title=""  MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EncodeSales.aspx.cs" Inherits="SMS.EncodeSales" %>
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
            
        </div>
  <!-- ################################################# START #################################################### -->
        <div class="form-group col-md-12">
            <div class="row" style="margin-bottom: 5px;">
                <div class="col-sm-12">

                    <div class="col-sm-2 text-right">
                        Receipt No.
                    </div>
                    <div class="col-sm-3">
                        <div class="input-group">
                            <asp:TextBox ID="txtOrderNo" runat="server" class="form-control" ReadOnly="true" Style="width: 185px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <button id="btnRefresh" onserverclick="btnRefresh_Click" type="submit" runat="server" class="btn btn-default"><span class="glyphicon glyphicon-refresh"></span></button>
                        </div>
                    </div>

                    <div class="col-sm-2 text-right">
                        Date
                    </div>
                    <div class="col-sm-3">
                        <div class="input-group">
                            <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" class="form-control" Style="width: 100px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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

                    

                    
                </div>
            </div>

            <div class="row"">
                <div class="col-sm-12">

                    <div class="col-sm-2 text-right">
                        PPC No.
                    </div>
                    <div class="col-sm-7">
                        <div class="input-group">
                        <asp:TextBox ID="txtManualOR" runat="server" Text="" class="form-control decimalnumbers-only" Style="width: 135px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator111" runat="server"
                            ControlToValidate="txtManualOR" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required(put 0 if follow up session only)"></asp:RequiredFieldValidator>
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
                        <asp:TextBox ID="txtCustID" CssClass="form-control" Width="75px" ReadOnly="true" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        
                        <asp:TextBox ID="txtAge" CssClass="form-control" Width="55px" placeholder="Age" ReadOnly="true" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSearchBranch" AccessKey="s" runat="server" CssClass="btn btn-warning"  ValidationGroup="grpSearch" Text="[ALT + S] SEARCH" OnClick="btnSearchBranch_Click" />
                        &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnShowSessionBalance" runat="server" class="btn btn-sm btn-primary" ValidationGroup="grpSearch" Text="Show Session Balance" OnClick="btnShowSessionBalance_Click" />
                             &nbsp;&nbsp;
                            <a href="AddNewCustomer.aspx" class="btn btn-sm btn-default"><i class="glyphicon glyphicon-user"></i>&nbsp;&nbsp;Add New Customer</a>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server"
                            ControlToValidate="txtCustomer" Display="Dynamic" ValidationGroup="grpSearch" ForeColor="Red"
                            ErrorMessage="Supply customer name"></asp:RequiredFieldValidator>
                    </div>
                </div>

            </div>
        </div>


            <div class="row" style="margin-bottom: 5px;">
                <div class="col-sm-12">
                    <div class="col-sm-2 text-right">
                        Customer Type
                    </div>
                    <div class="col-sm-2">
                        <div class="input-group">
                            <asp:DropDownList ID="ddStatus" runat="server" class="form-control" Style="width: 165px;">
                                <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Active" Text="Active Patient"></asp:ListItem>
                                <asp:ListItem Value="Buying" Text="Buying Patient"></asp:ListItem>
                                <asp:ListItem Value="New" Text="New Patient"></asp:ListItem>
                                <asp:ListItem Value="InActive" Text="Inactive Patient"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator113" runat="server" InitialValue="0"
                                ControlToValidate="ddStatus" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Please select"></asp:RequiredFieldValidator>

                        </div>
                    </div>

                    <div class="col-sm-1" style="text-align:right;">
                        
                            Source
                        
                    </div>

                    <div class="col-sm-5"  style="text-align:left;">
                        <div class="input-group">
                            <asp:DropDownList ID="ddSouce" runat="server" class="form-control" Style="width: 255px; text-transform: uppercase;" ></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1555" runat="server" InitialValue="0"
                                ControlToValidate="ddSouce" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Please select"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-3"  style="text-align:left;">
                        <%--<div class="input-group">
                            <asp:DropDownList ID="ddPickTye" runat="server" class="form-control" Enabled="false" Style="width: 125px; text-transform: uppercase;" AutoPostBack="true" OnSelectedIndexChanged="ddSouce_SelectedIndexChanged">
                                 <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                 <asp:ListItem Value="1" Text="Pickup"></asp:ListItem>
                                 <asp:ListItem Value="2" Text="Drop off"></asp:ListItem>
                            </asp:DropDownList>
                             
                        </div>--%>
                    </div>

                </div>
            </div>


            
   

            <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        Item
                    </div>
                    <div class="col-sm-10">
                        <div class="input-group">
            
                            <asp:DropDownList ID="ddITemFG" runat="server" Style="width: 605px;"  CssClass="form-control chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddITemFG_SelectedIndexChanged"></asp:DropDownList>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblType" runat="server" class="label label-default" Width="65px" Visible="false" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblNoSession" runat="server" class="label label-default" Width="25px" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblIsKit" runat="server" class="label label-default" Width="15px" Visible="false" Text=""></asp:Label>
                            &nbsp;&nbsp;
                                        <asp:Label ID="lblWithInv" runat="server" class="label label-default" Width="15px" Visible="false" Text=""></asp:Label>
                            &nbsp;&nbsp;
                             <asp:DropDownList ID="ddConsultationType" runat="server" Style="width: 115px;"  CssClass="form-control chosen-select" ></asp:DropDownList>           
                            <asp:RequiredFieldValidator ID="rvConsultation" runat="server" InitialValue="0"
                                ControlToValidate="ddConsultationType" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required if Consultation"></asp:RequiredFieldValidator>

                            <asp:Label ID="lblConsultationType" runat="server" class="label label-default" Visible="false" Width="15px" Text=""></asp:Label>
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
                    
                    <div class="col-sm-3 text-right">
                        <div class="input-group">
                          <asp:TextBox ID="TextBox1" runat="server" class="form-control" ReadOnly="true" Text="SRP" Style="width: 55px; text-align: right;"></asp:TextBox>                        
                          <asp:TextBox ID="txtSRP" runat="server" class="form-control decimalnumbers-only" Style="width: 85px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>                        
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtSRP" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="col-sm-4 text-left">
                        <div class="input-group">
                            <asp:TextBox ID="TextBox2" runat="server" class="form-control" ReadOnly="true" Text="No. of Sessions Availed : " Style="width: 185px; text-align: right;"></asp:TextBox>                        
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

           <div class="row" >
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        <asp:CheckBox ID="ckDeposit" runat="server" Checked="false" Enabled="false" AutoPostBack="true" Text="Deposit?" OnCheckedChanged="ckDeposit_CheckedChanged" />
                    </div>
                    <div class="col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="TextBoxDeposit" runat="server" class="form-control" ReadOnly="true" Text="Deposit Amt :" Style="width:110px; text-align: right;"></asp:TextBox>                        
                            <asp:TextBox ID="txtAmtDeposit" runat="server"  ReadOnly="true" class="form-control decimalnumbers-only" Style="width: 95px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>                        
                              <asp:RequiredFieldValidator ID="RequiredDeposit" runat="server"
                                ControlToValidate="txtAmtDeposit" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red" Enabled="false"
                                ErrorMessage="Amount to deposit is required for deposit transaction"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-sm-1">
                    </div>

                </div>
            </div>




            <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        Discount :
                    </div>
                    <div class="col-sm-10">
                        <div class="input-group">
                            <asp:DropDownList ID="ddDiscounts" runat="server" Style="width: 395px;"  CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddDiscounts_SelectedIndexChanged"></asp:DropDownList>
                            <asp:Button ID="btnReset" runat="server" Text="RESET" CssClass="btn btn-sm btn-default" OnClick="btnReset_Click" />
                            
                            &nbsp;
                            <asp:Label ID="lblDiscPercentage" runat="server" Text="" Visible="False" ></asp:Label>
                            &nbsp;
                             <asp:Label ID="lblVat" runat="server" Text="" Visible="False" ></asp:Label>
                            &nbsp;
                             <asp:Label ID="lblFlag" runat="server" Text="" Visible="False" ></asp:Label>
                             &nbsp;
                            <asp:Label ID="lblDiscAmt" runat="server" Text="" Visible="False" ></asp:Label>
                           
                        </div>
                    </div>

                </div>
            </div>

              <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        Card Number :
                    </div>
                    <div class="col-sm-2">
                        <div class="input-group">
                          
                           <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control"  placeholder="Card Number" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                
                        </div>
                    </div>
                    <div class="col-sm-3">
                            <asp:RequiredFieldValidator ID="RequiredCardNo" runat="server" Enabled="false"
                                ControlToValidate="txtCardNo" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Card Number is required for this type of discount"></asp:RequiredFieldValidator>
                         </div>

                </div>
            </div>

             <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-2 text-right">
                        GC Code :
                    </div>
                    <div class="col-sm-8">
                        <div class="input-group">
                            <asp:TextBox ID="txtGCcode" runat="server" CssClass="form-control" Width="175" ReadOnly="true"
                                    style="text-transform:uppercase;" MaxLength="14" placeholder="GC Code" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredGCCode" runat="server" Enabled="false"
                                ControlToValidate="txtGCcode" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required if discount is Gift Code Promo"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12" style="margin-bottom: 2px;">
                    <div class="col-sm-2 text-right">
                        Performed/Prescribed By :
                    </div>
                    <div class="col-sm-10">
                        <div class="input-group">
                            <asp:DropDownList ID="ddDrs" runat="server" Style="width: 415px;"  CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                            <asp:Button ID="btnAddEmploee" runat="server" Text="ADD EMPLOYEE" CssClass="btn btn-sm btn-default" OnClick="btnAddEmploee_Click"  />
                            <asp:Label ID="Label9" runat="server" ForeColor="Maroon" Font-Bold="true" Text="For products, select 'Walk-in Customer' if it is a walk-in only."></asp:Label>
                            
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12" style="margin-bottom: 2px;">
                    <div class="col-sm-2 text-right">
                        
                    </div>
                    <div class="col-sm-10">
                        <div class="input-group">
                        <asp:RequiredFieldValidator ID="RequiredDoctors" runat="server" InitialValue="0" Enabled="false"
                                ControlToValidate="ddDrs" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
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
                                    <asp:BoundField DataField="PerformedBy" HeaderText="Performed/Prescribed By" HtmlEncode="false" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
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
                    <div class="col-sm-11">
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
                                <%--ForeColor="Transparent" BackColor="Transparent"  BorderColor="Transparent"--%>
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
                                        <asp:CompareValidator ID="cvPayment" runat="server" Enabled="false"
                                        ErrorMessage="Payment exceed with total amount to paid" ValidationGroup="grpPayment"
                                        ControlToCompare="xtlblAmounttobepaid" ControlToValidate="txtAmtToPaid" Type="Double" Operator="LessThanEqual"
                                        ForeColor="Red"></asp:CompareValidator>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    Change
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtChange" runat="server" class="form-control decimalnumbers-only" ReadOnly="true" Style="width: 105px; text-align: right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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
                                   AR# or OR#
                                </div>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAROR" runat="server" class="form-control" Style="width: 195px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        &nbsp;
                                        &nbsp;
                                        <asp:Label ID="Label7" runat="server" Text="Encode AR # or OR # (format if AR = AR#123456; OR= OR#12345)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="txtAROR" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAROR"
                                            ValidationExpression="^(AR#\w+|OR#\w+)"
                                            ForeColor="Red" Display="Dynamic"
                                            ErrorMessage="The input is invalid. It should start with AR# or OR# in uppercase."
                                            ValidationGroup="grpPayment"></asp:RegularExpressionValidator>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    Bank/Partner
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
                                    <asp:Label ID="Label5" runat="server" Text="Last 4 digit in Card"></asp:Label>
                                </div>
                                <div class="col-sm-5">
                                    <div class="input-group">
                                        <asp:TextBox ID="txt4Digit" runat="server" Enabled="false" MaxLength="4" CssClass="form-control" Style="width: 95px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rftxt4Digit" runat="server" Enabled="false"
                                            ControlToValidate="txt4Digit" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>


                         <div class="row" style="margin-bottom: 5px;">
                            <div class="col-sm-12">
                                <div class="col-sm-3 text-right">
                                    <asp:Label ID="Label6" runat="server" Text="Name on Credit Card"></asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtCCName" runat="server" Enabled="false" CssClass="form-control" Style="width: 205px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rftxtCCName" runat="server" Enabled="false"
                                            ControlToValidate="txtCCName" Display="Dynamic" ValidationGroup="grpPayment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div


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

                    <div class="col-sm-10">
                        <div class="panel panel-default ">
                            <div class="panel-heading">Payment Breakdown</div>
                            <div class="panel-body">
                                <asp:GridView ID="grdPayment" runat="server"
                                    ShowFooter="true"
                                    CssClass="table table-bordered active active" AutoGenerateColumns="false" EmptyDataText="No payment has been found." OnRowDeleting="grdPayment_RowDeleting" OnRowDataBound="grdPayment_RowDataBound">

                                     <Columns>
                                        <asp:BoundField DataField="PaymentMode" HeaderText="Mode of Payment" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                        <%--<asp:BoundField DataField="TotalAmount" HeaderText="Amount" ItemStyle-Width="90" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />--%>
                                        <asp:TemplateField HeaderText="Amount" FooterStyle-HorizontalAlign="Right">
                                            <FooterStyle HorizontalAlign="Right" ForeColor="Blue"></FooterStyle>
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server"
                                                    Text='<%#Eval("TotalAmount")%>'></asp:Label>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalAmt" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="BankName" HeaderText="Bank/Partner" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ReferenceNumber" HeaderText="Reference Number" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CCNumber" HeaderText="CC No.<br />(Last 4digit)" HtmlEncode="false" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CCname" HeaderText="Name on CC" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />

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
    
    <!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridSessionBalance() {
            $(function () {
                $("#ShowSessionBalance").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridSessionBalance() {
            $(function () {
                $("#ShowSessionBalance").dialog({
                    title: "Customer Session Record",
                    //position: ['center', 20],

                    width: '1200px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowSessionBalance").parent().appendTo($("form:first"));
            });
        }
    </script>
    	 <%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>

    <div id="ShowSessionBalance" style="display: none;">
         <br />
         <br />
        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-9 text-right">
                        <div class="input-group">
                        <asp:Label ID="Label1" runat="server" Text="Please check the checkbox and choose on the dropdown list the Doctor/Staff's name if another personnel will perform the service." ForeColor="Maroon"></asp:Label>
                           &nbsp;&nbsp;
                            <asp:CheckBox ID="ckNewDR" runat="server" CssClass="ChkBoxClass" />
                        </div>
                    </div>
                    <div class="col-sm-3  text-right">
                        
                            <asp:DropDownList ID="ddPerformedBy" runat="server" Style="width:275px;"  CssClass="form-control" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>

                    </div>
                </div>
         </div>
            
        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <asp:GridView ID="gvSessionBalance" runat="server" AutoGenerateColumns="False" DataKeyNames="TransactionID" OnRowCommand="gvSessionBalance_RowCommand">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                 
                        <asp:BoundField HeaderText="Transaction ID" DataField="TransactionID" ItemStyle-Width="125px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

<%--                         <asp:TemplateField HeaderText="Transaction ID">
                            <HeaderStyle HorizontalAlign="Center" Width="125px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblTransactionID" runat="server" Text='<%#  Eval("TransactionID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ItemStyle-Width="175px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                
                        <asp:BoundField DataField="BrName" HeaderText="Branch" ReadOnly="True">
                            <HeaderStyle Width="95px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="vFGCode" HeaderText="ItemCode" ReadOnly="True">
                            <HeaderStyle Width="95px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

				        <asp:BoundField HeaderText="Plu Code/Description" DataField="ItemDescription" ItemStyle-Width="225px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Performed By" DataField="PerformedBy" ItemStyle-Width="205px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
				        <asp:BoundField HeaderText="Total Session" DataField="TotalSession" ItemStyle-Width="85px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

				        <asp:BoundField HeaderText="Total Availed" DataField="TotalSessionAvailed" ItemStyle-Width="85px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                     <%--   <asp:BoundField HeaderText="Balance" DataField="Balance" ItemStyle-Width="85px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>--%>

                        <asp:TemplateField HeaderText="Balance">
                            <HeaderStyle HorizontalAlign="Center" Width="55px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:TextBox ID="xtSessionBalance" runat="server" style="text-align:center;" 
                                    Text='<%#  Eval("Balance")%>'
                                    ReadOnly="true" CssClass="form-control" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>


				        <asp:TemplateField HeaderText="Sessions to avail">
                            <HeaderStyle HorizontalAlign="Center" Width="75px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:TextBox ID="xtSessionToAvailed" runat="server" AutoCompleteType="Disabled" CssClass="form-control decimalnumbers-only" ></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="rbSessionToAvailed" runat="server" 
                                        ControlToValidate="xtSessionToAvailed" Display="Dynamic" ValidationGroup="grpSession" ForeColor="Red"
                                        ErrorMessage="Required"></asp:RequiredFieldValidator>

                                <asp:CompareValidator ID="cvQtySession" runat="server"
                                    ErrorMessage="# of sesions s/b less than or equal to balance session of the selected package"
                                  ValidationGroup="grpSession" Display="Dynamic"
                                    ControlToCompare="xtSessionBalance" ControlToValidate="xtSessionToAvailed" Type="Double" Operator="LessThanEqual"
                                    ForeColor="Red"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateField>


                  <%--        <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="55px" />
                                        <ItemStyle HorizontalAlign="Center" Width="55" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckSelected" runat="server"/>
                                        </ItemTemplate>
                            </asp:TemplateField>--%>
                        
                           <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Height="27px" />
                                            <ItemTemplate>
                                                <div class="input-group">
                                                        <asp:LinkButton ID="btnSelectSession" runat="server" CommandName="SelectSession"  Text="SELECT" class="btn btn-sm btn-warning"></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                         
                                 </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <%--*******************OnClientClick="return CloseGridSessionBalance();" ************************************************************--%>















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

                        <asp:BoundField HeaderText="Age" DataField="AgeIntYears"  ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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



<script type="text/javascript">
    function ShowWarningMsgWithPending() {
        $(function () {
            $("#messageWarningWithPending").dialog({
                title: "Monthly Posting",
                width: '335px',
                buttons: {
                    Close: function () {
                        window.location = '<%= ResolveUrl("~/MonthlyPosting.aspx") %>';
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
        });
    }
    </script>

    <div id="messageWarningWithPending" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWithPending" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>





<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridDepositBalance() {
            $(function () {
                $("#ShowDepositBalance").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridDepositBalance() {
            $(function () {
                $("#ShowDepositBalance").dialog({
                    title: "Customer Deposit Record",
                    //position: ['center', 20],

                    width: '720px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowDepositBalance").parent().appendTo($("form:first"));
            });
        }
    </script>
    	

    <div id="ShowDepositBalance" style="display: none;">
        <br />
        <br />
        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px; margin-top:5px;">
                    <div class="col-sm-4 text-right">
                        Branch Code 
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxBrCode" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>

        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Branch Name
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxBrName" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>

        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Transaction No.
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxReceiptNo" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>


                                      
        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Sales Date
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxSalesDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>


      <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Customer ID 
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxCustID" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>
	               
        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Customer Name
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxCustomerName" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>

        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Itemcode
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxFGCode" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>

        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Item Description
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxItemDescription" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>
                                      
        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Total Session
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxTotalSession" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>
                              
        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Session Availed
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxSessionAvailed" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>
                                      
         <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Amount
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxItemAmount" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>


        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Deposit Amount
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxDepAmount" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>


        <div class="row text-right">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Balance
                    </div>
                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblxDueAmount" runat="server" Text=""></asp:Label>
                    </div>
                </div>
         </div>

        <br />
         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Session to Avail
                    </div>
                    <div class="col-sm-6 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtxSession" runat="server" class="form-control decimalnumbers-only" Text="1" Style="width: 50px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvXsession" runat="server"
                                ControlToValidate="txtxSession" Display="Dynamic" ValidationGroup="grpX" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Performed By :
                    </div>
                    <div class="col-sm-6 text-left"">
                        <div class="input-group">
                            <asp:DropDownList ID="ddxDoctor" runat="server" Style="width: 305px;"  CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rvXDr" runat="server" InitialValue="0"
                                ControlToValidate="ddxDoctor" Display="Dynamic" ValidationGroup="grpX" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


        

         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
                        Discount :
                    </div>
                    <div class="col-sm-6 text-left"">
                        <div class="input-group">
                            <asp:DropDownList ID="ddxDiscount" runat="server" Style="width: 305px;"  CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>

        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-4 text-right">
        
                    </div>
                    <div class="col-sm-6">
                        <div class="input-group">
                            <asp:Button ID="btnXpaybalance" runat="server" Text="PAY BALANCE" class="btn btn-primary" ValidationGroup="grpX" OnClick="btnXpaybalance_Click"/>
                        </div>
                    </div>

                </div>
            </div>


    </div>

<!-- ################################################# END #################################################### -->    

    <script type="text/javascript">
        function ShowWarningMsgX() {
            $(function () {
                $("#messageWarningX").dialog({
                    title: "Sales Deposit Transaction",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });

                $("#messageWarningX").parent().appendTo($("form:first"));
            });
        }
    </script>

    <div id="messageWarningX" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel3x" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWarningX" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>





<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridAddEmployee() {
            $(function () {
                $("#ShowAddEmployee").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridAddEmployee() {
            $(function () {
                $("#ShowAddEmployee").dialog({
                    title: "Add New Employee",
                    //position: ['center', 20],

                    width: '720px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowAddEmployee").parent().appendTo($("form:first"));
            });
        }
    </script>
    	

    <div id="ShowAddEmployee" style="display: none;">
        <br />
	    <br />
         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
                        Employee No.
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtEEmployeeNo" runat="server" class="form-control decimalnumbers-only" MaxLength="6" Text="" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvEmployee" runat="server"
                                ControlToValidate="txtEEmployeeNo" Display="Dynamic" ValidationGroup="grpE" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
                        Surname :
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtEEmployeeSurname" runat="server" class="form-control decimalnumbers-only" Text="" Style="width: 275px; text-transform:uppercase;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvEmployee1" runat="server"
                                ControlToValidate="txtEEmployeeSurname" Display="Dynamic" ValidationGroup="grpE" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

			<div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
                        First Name :
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtEEmployeeFirstNaame" runat="server" class="form-control decimalnumbers-only" Text="" Style="width: 275px; text-transform:uppercase;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvEmployee2" runat="server"
                                ControlToValidate="txtEEmployeeFirstNaame" Display="Dynamic" ValidationGroup="grpE" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 15px;">
                    <div class="col-sm-3 text-right">
                        Position :
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:DropDownList ID="ddEPosition" runat="server" Style="width: 275px;"  CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rvEpos" runat="server" InitialValue="0"
                                ControlToValidate="ddEPosition" Display="Dynamic" ValidationGroup="grpE" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

         

        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
        
                    </div>
                    <div class="col-sm-8">
                        <div class="input-group">
                            <asp:Button ID="btnEaddEmployee" runat="server" Text="S A V E" class="btn btn-primary" ValidationGroup="grpE" OnClick="btnEaddEmployee_Click"/>
                        </div>
                    </div>

                </div>
            </div>


    </div>

<!-- ################################################# END #################################################### -->    



<%-- <script type="text/javascript">
     document.attachEvent('onkeyup', KeysShortcut);

     // Now we need to implement the KeysShortcut
     function KeysShortcut() {

         if (event.keyCode == 113) {
             document.getElementById('<%= btnSearchBranch.ClientID %>').click();

             }

         }
  </script>--%>


<!-- ################################################# END #################################################### -->


    <script type="text/javascript">
        function ShowSuccessMsgPosting() {
            $(function () {
                $("#messageSuccessPosting").dialog({
                    title: "Posting of Transaction",
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

    <div id="messageSuccessPosting" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgSuccessPosting" runat="server" />
                <br />
                <br />
                <asp:Label Text="Receipt No." ID="Label3" runat="server" />
                &nbsp;&nbsp;
                <asp:Label Text="" ID="lblSeriesNo" runat="server" />
                
                <div class="input-group">
                    <asp:Label Text="PRINT RECEIPT?" ID="Label2" runat="server" />
                    &nbsp;&nbsp;
                    <button id="btnClosePrint"  onserverclick="btnClosePrint_Click" type="submit" runat="server" onclick="javascript:scroll(0,0);"  class="btn btn-warning" >N O&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-remove-sign"></span></button>
                    &nbsp;&nbsp;
                    <button id="btnPrintPreview"  onserverclick="btnPrintPreview_Click" type="submit" runat="server"  class="btn btn-success" >Y E S&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                 </div>
                <br />
                <asp:Label Text="Please don't print(Not yet registered)." ID="Label4" runat="server" />
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrintPreview" />
                <asp:PostBackTrigger ControlID="btnClosePrint" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- ################################################# START #################################################### -->


    <script type="text/javascript">
        function ShowWarningMsg2() {
            $(function () {
                $("#messageWarning2").dialog({
                    title: "Follow up Session",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            //$(this).dialog('close');
                            $('#<%= btnShowSessionBalance.ClientID %>').click();
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageWarning2" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel32" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWarning2" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>




<script type="text/javascript">
    function ShowWarningMsg3() {
        $(function () {
            $("#messageWarning3").dialog({
                title: "Sales Transaction",
                width: '335px',
                buttons: {
                    Close: function () {
                        window.location = '<%= ResolveUrl("~/LoginPage.aspx") %>';
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageWarning3" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel33" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWarning3" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>





    </asp:Content>


