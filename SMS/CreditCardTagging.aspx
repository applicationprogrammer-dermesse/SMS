<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreditCardTagging.aspx.cs" Inherits="SMS.CreditCardTagging" %>
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

        .theGrid {
            width: 98%;
            height: 220px;
            overflow-x: auto;
            white-space: nowrap;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid" style="width: 98%">
                <div class="row" style="margin-bottom: 25px;">
                    <div class="row">
                        <div class="col-md-4 text-center">
                        </div>
                        <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                            <h4>CREDIT CARD TAGGING</h4>
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
                                <asp:TextBox ID="txtDateFrom" runat="server" class="datePrev form-control" Style="width: 105px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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
                            <button id="btnPrint" visible="false" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">EXPORT&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>

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
                                <asp:TextBox ID="txtDateTo" runat="server" class="datePrev form-control" Style="width: 105px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateTo"
                                    ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                    ForeColor="Red" Display="Dynamic"
                                    ErrorMessage="Invalid date format"
                                    ValidationGroup="grpIss"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" Style="color: Red;"
                                    ValidationGroup="grpIss" Display="Dynamic" ControlToCompare="txtDateFrom"
                                    ControlToValidate="txtDateTo" Type="Date" Operator="GreaterThanEqual"
                                    ErrorMessage="Invalid date range"></asp:CompareValidator>
                            </div>
                        </div>

                    </div>
                </div>


                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Branch
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddBranch" runat="server" AutoPostBack="true" class="form-control" Style="width: 195px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0"
                                    ControlToValidate="ddBranch" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                    ErrorMessage="Please select branch"></asp:RequiredFieldValidator>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Option 
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:RadioButton ID="rbAll" runat="server" Checked="true" AutoPostBack="true" GroupName="grpRB" Text="&nbsp;&nbsp;&nbsp;ALL" OnCheckedChanged="rbAll_CheckedChanged" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbCC" runat="server" GroupName="grpRB" AutoPostBack="true" Text="&nbsp;&nbsp;&nbsp;Credit Cards" OnCheckedChanged="rbCC_CheckedChanged" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbPaymaya" runat="server" GroupName="grpRB" AutoPostBack="true" Text="&nbsp;&nbsp;&nbsp;Paymaya" OnCheckedChanged="rbPaymaya_CheckedChanged" />
                            </div>
                        </div>

                    </div>
                </div>

                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Bank
                        </div>
                        <div class="col-sm-5">
                            <div class="input-group">
                                <asp:DropDownList ID="ddBanks" runat="server" Enabled="false" class="form-control" Style="width: 215px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddBanks_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 2px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                        </div>
                        <div class="col-sm-7">
                            <div class="input-group">
                            </div>
                        </div>
                        <div class="col-sm-2 text-right">
                            <div class="input-group">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="txtTaggingControlNo" Display="Dynamic" ValidationGroup="grpTagged" ForeColor="Red"
                                    ErrorMessage="Please supply"></asp:RequiredFieldValidator>
                                Check No. : &nbsp; &nbsp; &nbsp;
                            </div>
                        </div>
                        <div class="col-sm-1 text-right">
                        </div>

                    </div>
                </div>



                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                        </div>
                        <div class="col-sm-7">
                            <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        </div>
                        <div class="col-sm-2 text-right">
                            <div class="input-group">
                                <asp:TextBox ID="txtTaggingControlNo" CssClass="form-control" Width="155px" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1 text-right">
                            
                            <asp:Button ID="btnPostTagged" runat="server" Text="P O S T" validationgroup="grpTagged" class="btn btn-warning" OnClick="btnPostTagged_Click"/>
                            <button id="btnPost" onserverclick="btnPost_Click" type="submit" runat="server" validationgroup="grpTagged" visible="false" class="btn btn-warning">P O S T&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                        </div>

                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvCC" Width="99%" runat="server" AutoGenerateColumns="false" DataKeyNames="RecID"
                            ShowFooter="true"
                            class="table table-striped table-bordered table-hover">
                            <Columns>

                                <asp:BoundField DataField="RecID" HeaderText="Rec No." ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol">
                                    <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Transaction No" DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:BoundField HeaderText="OR No" DataField="ORno" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:BoundField HeaderText="Payment Type" DataField="PaymentMode" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Bank/Partner" DataField="BankName" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Batch No" DataField="BatchNumber" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Approval No." DataField="ReferenceNumber" ReadOnly="True" FooterStyle-Font-Bold="true" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="TotalAmount" DataField="TotalAmount" DataFormatString="{0:n2}" FooterStyle-Font-Bold="true" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:BoundField>

                                <%--        <asp:TemplateField HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblTotalAmount" runat="server"
                                    Text='<%#  Eval("TotalAmount","{0:n2}")%>'></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                                        <asp:Label ID="lblTotalAmt" runat="server"></asp:Label>
                                        </FooterTemplate>
                        </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Select">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="4%" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckStat" OnCheckedChanged="ckStat_CheckedChanged" runat="server" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


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
                    title: "Credit Card",
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

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".datePrev").datepicker({
                        maxDate: new Date, minDate: new Date(2007, 6, 12)
                    });
                }
            });
        };
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

    <!-- ################################################# END #################################################### -->
</asp:Content>
