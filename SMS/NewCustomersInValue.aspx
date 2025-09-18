<%@ Page Title="New Customers In Value" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewCustomersInValue.aspx.cs" Inherits="SMS.NewCustomersInValue" %>

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
                <asp:Label ID="Label2" runat="server" Text="New Customers Report"></asp:Label>
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
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Style="color: Red;"
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
                    
                        <div id='myHiddenDiv' style="text-align: center">
                            <img src='' id='myAnimatedImage' alt="" height="60" />
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="row" style="margin-bottom: 5px; margin-left: 15px;">
            <div class="col-md-4">
                <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="#ff3399" Text="New Customers In Value(Net Amount)"></asp:Label>
            </div>
        </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12" style="margin-left: 15px;">
                <asp:GridView ID="gvCustomerCount" runat="server" Width="70%" DataKeyNames="BrCode" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" OnRowCommand="gvCustomerCount_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="Brc" DataField="BrcODE" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>


                        <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>


                         <asp:BoundField HeaderText="Availed <br/>Service" DataField="NewService" HtmlEncode="false" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                        </asp:BoundField>


                       <%-- <asp:TemplateField HeaderText="Availed <br/>Service">
                            <ItemStyle HorizontalAlign="Right" Width="65px" Height="27px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkServiceOnly" runat="server" CommandName="ViewListService" Text='<%#Eval("NewService") %>' ForeColor="Blue" />

                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:BoundField HeaderText="Availed <br/>Product Only<br/>(Buying)" DataField="NewProduct" HtmlEncode="false" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                         </asp:BoundField>


                      <%--  <asp:TemplateField HeaderText="Availed <br/>Product Only<br/>(Buying)">
                            <ItemStyle HorizontalAlign="Right" Width="65px" Height="27px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkProduct" runat="server" CommandName="ViewListProduct" Text='<%#Eval("NewProduct") %>' ForeColor="Blue" />

                            </ItemTemplate>
                        </asp:TemplateField>--%>


                         <asp:BoundField HeaderText="Total Amount" DataField="TotalNewCust" HtmlEncode="false" ReadOnly="True" ItemStyle-Width="10%" DataFormatString="{0:n2}" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                         </asp:BoundField>

                    <%--    <asp:BoundField HeaderText="Total Amount" DataField="TotalNewCust" HtmlEncode="false" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>--%>
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
                    title: "New Customers In Value",
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
