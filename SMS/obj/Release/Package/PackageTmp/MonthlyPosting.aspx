<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MonthlyPosting.aspx.cs" Inherits="SMS.MonthlyPosting" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 25px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>MONTHLY POSTING</h4>
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
                                <asp:DropDownList ID="ddBranch" runat="server" Width="245px"  CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Year :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:Label ID="lblYear" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Month :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
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
                                <asp:Button ID="btnProcess" runat="server" class="btn btn-primary" OnClientClick="showDiv();" Text="PROCESS" OnClick="btnProcess_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                 <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12 text-center">
                        <div id='myHiddenDiv' style="width: 18%; float: left; height: 50px; line-height: 50px; text-align:right;">
                                  <img src='' id='myAnimatedImage' alt="" height="50" /> 
                    </div>
                    </div>
                </div>

                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvSummary" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode" OnRowDataBound="gvSummary_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="95px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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
                                            Text='<%#  Eval("BegBal","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty <br />Received">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyReceivedS" runat="server"
                                            Text='<%#  Eval("QtyReceived","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyPRF" runat="server"
                                            Text='<%#  Eval("vQtyPRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unposted PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedPRF" runat="server"
                                            Text='<%#  Eval("UnpostedPRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Sales for the Day">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedQtyS" runat="server" 
                                            Text='<%#  Eval("UnpostedQty","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Sales">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtySales" runat="server"
                                            Text='<%#  Eval("Sales","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Free">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyFree" runat="server"
                                            Text='<%#  Eval("Free","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unposted Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedAdjustment" runat="server"
                                            Text='<%#  Eval("UnpostedAdjustment","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyAdjustmentS" runat="server"
                                            Text='<%#  Eval("Adjustment","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Unposted Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedComplimentary" runat="server"
                                            Text='<%#  Eval("UnpostedComplimentary","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyComplimentaryS" runat="server"
                                            Text='<%#  Eval("Complimentary","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Available Balance">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableBalanceS" runat="server"
                                            Text='<%#  Eval("AvailableBalance","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnPrint" />--%>
                 <%--<asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="onserverclick" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>
    


    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
    </script>

    <script type="text/javascript">
        function showDiv() {
            document.getElementById('myHiddenDiv').style.display = "";
            setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

        }
    </script>




    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Monthly Posting",
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
    function ShowWarningMsgWithPending() {
        $(function () {
            $("#messageWarningWithPending").dialog({
                title: "Monthly Posting",
                width: '335px',
                buttons: {
                    Close: function () {
                        window.location = '<%= ResolveUrl("~/ProcessEndOfDay.aspx") %>';
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageWarningWithPending" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWithPending" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageSuccess").dialog({
                    title: "Monthly Posting",
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

    <div id="messageSuccess" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgSuccess" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
