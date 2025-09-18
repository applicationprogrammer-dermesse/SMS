<%@ Page Title="New Customers Count" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewCustomersCount.aspx.cs" Inherits="SMS.NewCustomersCount" %>
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
                <asp:Label ID="Label2" runat="server" Text="New Customer Count"></asp:Label>
            </div>
        </div>
        

        
  

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Date From :
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                     <asp:TextBox ID="txtDateFrom" runat="server" class="form-control dateCurr" Style="width: 120px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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
        </div>

           <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            To :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">

                                <asp:TextBox ID="txtDate" runat="server" class="form-control dateCurr" Style="width: 120px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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

         <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                      <button id="btnGenerate" onserverclick="btnGenerate_Click" onclick="if(Page_ClientValidate()) showDiv();" type="submit" runat="server" validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        &nbsp;&nbsp;&nbsp;
                           <button id="btnExcel" onserverclick="btnExcel_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                        &nbsp;&nbsp;&nbsp;
                    
                        <div id='myHiddenDiv' style="text-align:center "> 
                                        <img src='' id='myAnimatedImage' alt="" height="60" /> 
                         </div>
                    </div>
                </div>
            </div>
        </div>

        
        <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-12" style="margin-left:45px;">
                      <asp:GridView ID="gvCustomerCount" runat="server" Width="90%" DataKeyNames="BrCode" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvCustomerCount_RowCommand" >
                           
                             <Columns>

                                        <asp:BoundField HeaderText="Brc" DataField="BrcODE" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>


                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                 
                                    <asp:TemplateField HeaderText="Availed <br/>Product and Service">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPandS" runat="server" CommandName="ViewList"  Text='<%#Eval("iBoth") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>


                                 <asp:TemplateField HeaderText="Availed <br/>Product Only">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkProductOnly" runat="server" CommandName="ViewListProduct"  Text='<%#Eval("iProduct") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                         

                                      <asp:TemplateField HeaderText="Availed <br/>Service Only">
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkServiceOnly" runat="server" CommandName="ViewListService"  Text='<%#Eval("iService") %>' ForeColor="Blue"   />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                 <asp:BoundField HeaderText="Total Customers<br/>(NEW)" DataField="cAll" HtmlEncode="false"  ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                            </Columns>
                        </asp:GridView>
            </div>
        </div>


         <div class="row" style="margin-bottom: 15px;">
            <div class="col-md-12" style="margin-left:45px; margin-bottom: 5px;">
                <div class="input-group">
                <asp:Label ID="lblNote" runat="server"></asp:Label>
                &nbsp;&nbsp;&nbsp;
                    <button id="btnExcelList" onserverclick="btnExcelList_Click" type="submit" runat="server" visible="false" class="btn btn-success">EXPORT LIST&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                </div>
            </div>
            <div class="col-md-12" style="margin-left:45px;">
                      <asp:GridView ID="gvCustList" runat="server" Width="90%" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" >
                           
                             <Columns>

                                        <asp:BoundField HeaderText="Brc" DataField="BrcODE" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                         <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Date" DataField="SalesDate" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Item Type" DataField="ItemType" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Ite mDescription" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Discounts Amt" DataField="DiscountsAmt" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                  <asp:BoundField HeaderText="Net Amount" DataField="NetAmount" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:BoundField>
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
                    title: "New Customer Count",
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


     <script type="text/javascript">
         function showDiv() {
             document.getElementById('myHiddenDiv').style.display = "";
             setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

         }
    </script>
    <!-- ################################################# END #################################################### -->

</asp:Content>
