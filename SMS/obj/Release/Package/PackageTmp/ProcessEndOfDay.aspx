<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessEndOfDay.aspx.cs" Inherits="SMS.ProcessEndOfDay" %>
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
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>End of Day Processing(Closing)</h4>
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
                    Branch :
                </div>
                <div class="col-sm-8">
                    <div class="input-group">
                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                 
                <div class="col-sm-1">
                    <div id='myHiddenDiv' style="width: 18%; float: left; height: 50px; line-height: 50px; text-align:right;">
                                  <img src='' id='myAnimatedImage' alt="" height="50" /> 
                    </div>
                </div>

                <div class="col-sm-1  text-right">
                      <button id="btnPost" onserverclick="btnPost_Click" type="submit" runat="server" onclick="showDiv();" class="btn btn-success">P O S T&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-check"></span></button>
                      

                </div>

            </div>
        </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Date to Close :
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>

            </div>
        </div>
        <div class="row" style="margin-bottom: 2px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                 
                </div>
                <div class="col-sm-9" style="display:none;">
                      <div class="input-group">
                        <button id="btnPrintX"  onserverclick="btnPrintX_Click" type="submit" runat="server" class="btn btn-default" >PRINT X READING&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                        &nbsp;&nbsp;&nbsp;
                          <button id="btnPrintZ"   onserverclick="btnPrintZ_Click" type="submit" runat="server" class="btn btn-primary" >PRINT Z READING&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                      </div>
                </div>

            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                 Transaction for the day
                <asp:GridView ID="gvSalesForTheDay" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="RecID" OnRowDataBound="gvSalesForTheDay_RowDataBound" >
                    <Columns>
                        <asp:BoundField DataField="RecID" HeaderText="RecID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="vItemID" HeaderText="vItemID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="7%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" ReadOnly="True" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>



                        <asp:BoundField HeaderText="Transaction No" DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="PluCode/ItemDescription" DataField="ItemDescription" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>



                        <asp:TemplateField HeaderText="Qty">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server"
                                    Text='<%#  Eval("Qty","{0:###0;(###0);0}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("Net Amount","{0:n2}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Status" DataField="Status" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            
        </div>
        <br />
         <div class="row">
            <div class="col-sm-12">
                 Transaction with promo or free products
                <asp:GridView ID="gvKit" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ID">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="RecID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SalesDetailedID" HeaderText="TranID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" ItemStyle-Width="7%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Transaction No" DataField="ReceiptNo" ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Promo Code" DataField="PromoCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="PluCode/ItemDescription" DataField="vDESCRIPTION" ReadOnly="True" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Sales">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblSales_qty" runat="server"
                                    Text='<%#  Eval("Sales_qty","{0:###0;(###0);0}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="Free">
                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:Label ID="lblFree_qty" runat="server"
                                    Text='<%#  Eval("Free_qty","{0:###0;(###0);0}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
            
        </div>

    </div>


    <!-- ################################################# START #################################################### -->
    <!-- ################################################# END #################################################### -->
   <%--<script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script> --%>


    <script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>

    <script type="text/javascript">
        function showDiv() {
            document.getElementById('myHiddenDiv').style.display = "";
            setTimeout('document.images["myAnimatedImage"].src = "images/please_wait.gif"', 50);

        }
    </script>

    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
    </script>

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "End of Day Processing",
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

    <div id="messageWarning" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgWarning" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- ################################################# END #################################################### -->
        <!-- ################################################# END #################################################### -->
<script type="text/javascript">
    function ShowNoSalesMsg() {
        $(function () {
            $("#messageNoSales").dialog({
                title: "End of Day Processing",
                width: '335px',
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            $("#messageNoSales").parent().appendTo($("form:first"));
        });
    }
    </script>

    <div id="messageNoSales" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgNoSales" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnProceedsWithProcess" runat="server" Text="YES/Proceed" CssClass="btn btn-warning" OnClick="btnProceedsWithProcess_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

        <!-- ################################################# END #################################################### -->


    <script type="text/javascript">
        function ShowWarningMsgWithPending() {
            $(function () {
                $("#messageWarningWithPending").dialog({
                    title: "End of Day Processing",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/DailySales.aspx") %>';
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

    <!-- ################################################# END #################################################### -->




</asp:Content>
