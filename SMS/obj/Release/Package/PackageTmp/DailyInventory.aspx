<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyInventory.aspx.cs" Inherits="SMS.DailyInventory" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 25px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>INVENTORY REPORT</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Branch :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">

                                <asp:TextBox ID="txtDate" runat="server" class="form-control datePrev" Style="width: 100px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
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


                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Branch :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddBranch" runat="server" Width="245px" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged"></asp:DropDownList>
<%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" InitialValue="0"
                                    ControlToValidate="ddBranch" Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                    ErrorMessage="Select branch"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <button id="btnGen" onserverclick="btnGen_Click" type="submit" validationgroup="grpIss" runat="server" class="btn btn-primary">GENERATE&nbsp;&nbsp;&nbsp;<i class="fa fa-cog"></i></button>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">E X P O R T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                            </div>
                        </div>
                    </div>
                </div>

                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvSummary" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode">
                            <Columns>
                                <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="95px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>



                                <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION" ReadOnly="True" ItemStyle-Width="395px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Category" DataField="vCATEGORY" ReadOnly="True" ItemStyle-Width="185px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:TemplateField HeaderText="Beg Bal">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyBegBalS" runat="server"
                                            Text='<%#  Eval("vQtyBegBal","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty <br />Received">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyReceivedS" runat="server"
                                            Text='<%#  Eval("vQtyReceived","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyPRF" runat="server"
                                            Text='<%#  Eval("PRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>




                                <asp:TemplateField HeaderText="Sales">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtySales" runat="server"
                                            Text='<%#  Eval("vQtySales","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Free">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyFree" runat="server"
                                            Text='<%#  Eval("vFreeQty","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyAdjustmentS" runat="server"
                                            Text='<%#  Eval("ADJ","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyComplimentaryS" runat="server"
                                            Text='<%#  Eval("COMPLI","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Ending Balance">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableBalanceS" runat="server"
                                            Text='<%#  Eval("EndBal","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrint" />
                <%--<asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="onserverclick" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>


    <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>





    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
    </script>

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
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Inventory Report",
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

</asp:Content>
