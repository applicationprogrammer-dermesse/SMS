<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffPerformance.aspx.cs" Inherits="SMS.StaffPerformance" %>

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

        .FonstSize {
            font-size: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 25px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Clinic Staff Performance</h4>
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


        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Branch
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddBranch" runat="server" class="form-control" Style="width: 195px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="true" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged"></asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="RequiredBranch" Enabled="false" runat="server" InitialValue="0"
                            ControlToValidate="ddBranch" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                            ErrorMessage="Please select branch"></asp:RequiredFieldValidator>--%>
                    </div>

                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12" style="margin-bottom: 5px;">
                <div class="col-sm-2 text-right">
                    Doctor/Staff Name :
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                        <asp:DropDownList ID="ddDrs" runat="server" Style="width: 415px;" CssClass="form-control chosen-select" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddDrs_SelectedIndexChanged"></asp:DropDownList>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12" style="margin-bottom: 5px;">
                <div class="col-sm-2 text-right">
                    Option :
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                        <asp:CheckBox ID="ckAll" runat="server" Text="&nbsp;&nbsp; Please check to generate staff/personnel transaction to all branches" AutoPostBack="true" Checked="false" OnCheckedChanged="ckAll_CheckedChanged" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="display:none;">
            <div class="col-sm-12" style="margin-bottom: 5px;">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-10">
                    <div class="input-group">
                        <asp:CheckBox ID="ckConsolidated" runat="server" Enabled="false" Text="&nbsp;&nbsp; Consolidated" Checked="false" AutoPostBack="True" OnCheckedChanged="ckConsolidated_CheckedChanged"/>
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                </div>
                <div class="col-sm-2">
                    <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" onclick="showDiv();"  validationgroup="grpIss" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-toggle-right"></i></button>
                </div>
                <div class="col-sm-2">
                            <div id='myHiddenDiv' style="width: 18%; float: left; height: 50px; line-height: 50px; text-align:right;">
                                  <img src='' id='myAnimatedImage' alt="" height="50" /> 
                              </div>
                        </div>

            </div>
        </div>

        <br />
        <div class="row FonstSize">
            <div class="col-sm-12">
                <asp:GridView ID="gvStaff" Width="100%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" OnRowDataBound="gvStaff_RowDataBound">
                    <Columns>


                        <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>




                        <asp:BoundField HeaderText="Performed By" DataField="PerformedBy" ReadOnly="True" ItemStyle-Width="13%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Position" DataField="Position" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Receipt No." DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>


                        <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="12%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Customer Type" DataField="PatientType" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                          <asp:BoundField HeaderText="Group Name" DataField="GroupName"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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

                        <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Description" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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

                        <asp:BoundField HeaderText="Total Session" DataField="TotalSession" ReadOnly="True" ItemStyle-Width="15px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="No of Session" DataField="NoSession" ReadOnly="True" ItemStyle-Width="15px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>


                        <asp:BoundField HeaderText="Discount Availed" DataField="DiscDescription" ReadOnly="True" ItemStyle-Width="185px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>



                        <asp:TemplateField HeaderText="Discounts Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountsAmt" runat="server" Text='<%# Eval("DiscountsAmt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Perc(%)" DataField="vDiscPerc" ReadOnly="True" ItemStyle-Width="15px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("NetAmount","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                         <asp:TemplateField HeaderText="Gender" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CivilSatus" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblCivilSatus" runat="server" Text='<%# Eval("CivilSatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

        

                               <asp:TemplateField HeaderText="Date Of Birth">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateOfBirth" runat="server"
                                                    Text='<%#  Convert.ToString(Eval("DateOfBirth", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("DateOfBirth", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                  </asp:TemplateField>
                         

                        
                         <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblAge" runat="server" Text='<%# Eval("Age") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                <br />
                <asp:GridView ID="gvConso" Width="100%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" >
                    <Columns>


                        <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>




                        <asp:BoundField HeaderText="Performed By" DataField="PerformedBy" ReadOnly="True" ItemStyle-Width="13%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Position" DataField="Position" ReadOnly="True" ItemStyle-Width="8%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Group Name" DataField="GroupName"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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

                        <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Description" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <%--<asp:TemplateField HeaderText="SRP">
                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" runat="server"
                                    Text='<%#  Eval("vUnitCost","{0:n2}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>


                        <asp:TemplateField HeaderText="Qty">
                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server"
                                    Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>






                        <asp:TemplateField HeaderText="Discounts Amt" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountsAmt" runat="server" Text='<%# Eval("DiscountsAmt","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

             

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("NetAmount","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>





                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
<%--  </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>--%>


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
                    title: "Staff Performance per Branch",
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

    <!-- ################################################# END #################################################### -->

 <script type="text/javascript">
     function showDiv() {
         document.getElementById('myHiddenDiv').style.display = "";
         setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

     }
    </script>
</asp:Content>

