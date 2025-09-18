<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesPerSKU.aspx.cs" Inherits="SMS.SalesPerSKU" %>
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
                <asp:Label ID="Label2" runat="server" Text="Sales Per SKU"></asp:Label>
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

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Date From :
                </div>
                <div class="col-sm-10">
                     <asp:TextBox ID="txtDateFrom" runat="server" class="form-control dateCurr" Style="width: 110px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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

           <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            To :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">

                                <asp:TextBox ID="txtDate" runat="server" class="form-control dateCurr" Style="width: 110px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Type :
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                     <asp:DropDownList ID="ddType" runat="server" Width="145px" CssClass="form-control">
                         <asp:ListItem Value="0" Text="Please select"></asp:ListItem>
                         <asp:ListItem Value="Product" Text="Product"></asp:ListItem>
                         <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                     </asp:DropDownList>  
                      <asp:RequiredFieldValidator ID="ReqV2" runat="server" InitialValue="0"
                            ControlToValidate="ddType" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>                  
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                                    
                                   <asp:CheckBox ID="ckPerItem" runat="server" Checked="false" Text="&nbsp;&nbsp;&nbsp; Per Item" AutoPostBack="True" OnCheckedChanged="ckPerItem_CheckedChanged" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Enabled="false" Style="width: 695px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                                </div>
                         
                    </div>
                </div>


        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                      <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        &nbsp;&nbsp;&nbsp;
                           <button id="btnExcel" onserverclick="btnExcel_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                    </div>
                <div class="col-sm-3">
                       <div id='myHiddenDiv' style="text-align:center "> 
                                        <img src='' id='myAnimatedImage' alt="" height="70px" /> 
                         </div>
                 </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-12" style="margin-left:5px;">
                      <asp:GridView ID="gvPerSku" runat="server" Width="98%" ShowFooter="true" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                           <FooterStyle BackColor="PaleGoldenrod" Font-Bold="true" />
                             <Columns>
                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="4%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Group Name" DataField="GroupName"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Item Type" DataField="ItemType"  ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Category" DataField="vCATEGORY"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Session Group" DataField="SessionGroup"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Session Type" DataField="SessionType"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                 

                                 <asp:BoundField HeaderText="Plu Code" DataField="vPluCode"  ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                 <asp:BoundField HeaderText="Item Code" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                       <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                     <asp:BoundField HeaderText="Performed/Prescribed By" DataField="PerformedBy"  ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Receipt No" DataField="ReceiptNo"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:TemplateField  HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Gross <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossAmt" runat="server" Text='<%# Eval("GrossAmt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Discount  <br />Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountsAmt" runat="server" Text='<%# Eval("DiscAmt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Net  <br />Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("NetAmt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                      </asp:GridView>
                <br />
           
                <asp:GridView ID="gvSalesPerItem" runat="server" Width="98%" ShowFooter="true" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                           <FooterStyle BackColor="PaleGoldenrod" Font-Bold="true" />
                             <Columns>
                                        
                                        <asp:BoundField HeaderText="Branch Name" DataField="BrName"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Plu Code" DataField="vPluCode"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                 <asp:BoundField HeaderText="Item Code" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                       <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                      <asp:BoundField HeaderText="Performed/Prescribed By" DataField="PerformedBy"  ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Receipt No" DataField="ReceiptNo"  ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                            

                                 


                                 <asp:TemplateField  HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Gross <br /> Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossAmt" runat="server" Text='<%# Eval("GrossAmt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField  HeaderText="Discount  <br />Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountsAmt" runat="server" Text='<%# Eval("DiscAmt","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Net  <br />Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                       <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("NetAmt","{0:n2}") %>'></asp:Label>
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
                    title: "Sales Detailed per Branch",
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
