<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Deliveries.aspx.cs" Inherits="SMS.Deliveries" %>
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 20px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>DELIVERIES</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>


       <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                   Branch :
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                    </div>
                </div>
           
            </div>
        </div>
                
        <div class="row"  style="margin-bottom:5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                   Source :
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:Label ID="lblSource" runat="server" Text="DERMPHARMA, INC."></asp:Label>
                    </div>
                </div>
           
            </div>
        </div>

         <div class="row"  style="margin-bottom:5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Delivery Receipt No. :
                </div>
                <div class="col-sm-4">
                    <div class="input-group">
                        <asp:TextBox ID="txtDelReceiptNo" runat="server" class="form-control" Style="width: 155px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                        <button id="btnGet" onserverclick="btnGet_Click" type="submit"  ValidationGroup="grpDel" runat="server" class="btn btn-primary" onclick="showDiv();">DOWNLOAD&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-download"></span></button>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="txtDelReceiptNo" Display="Dynamic" ValidationGroup="grpDel" ForeColor="Red"
                            ErrorMessage="Please supply DR number."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div id='myHiddenDiv' style="width: 18%; float: left; height: 50px; line-height: 50px; text-align:left;">
                                  <img src='' id='myAnimatedImage' alt="" height="50" /> 
                    </div>
                </div>
                <div class="col-sm-5  text-right">
                      <button id="btnPost" onserverclick="btnPost_Click" type="submit" runat="server" ValidationGroup="grpPost" onclick="showDiv();" class="btn btn-success">P O S T&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-check"></span></button>
                </div>

            </div>
        </div>
    
         <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Date Received
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">

                                <asp:TextBox ID="txtDate" runat="server" class="form-control dateCurr" Style="width: 110px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                &nbsp;&nbsp;
                                 <asp:RequiredFieldValidator ID="ReqDate" runat="server"
                                    ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="grpPost" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDate"
                                    ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                    ForeColor="Red"
                                    ErrorMessage="Invalid date format"
                                    ValidationGroup="grpPost"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                </div>
        <div class="row" style="margin-bottom:15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
        
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                      <button id="btnClear" onserverclick="btnClear_Click" type="submit" runat="server" class="btn btn-warning">C L E A R&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-trash"></span></button>
                    </div>
                </div>
           
            </div>
        </div>

     <div class="row"  style="margin-bottom:15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">

                </div>
                <div class="col-sm-9">
                    <asp:Label ID="lblNote" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                </div>

            </div>
        </div>   
    <div class="row">
            <div class="col-sm-12">
               <asp:GridView ID="gvDeliveries" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ID" OnRowDataBound="gvDeliveries_RowDataBound">
                                    <Columns>
                                       <asp:TemplateField HeaderText="Select Item">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckStat" runat="server" Checked="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ID" HeaderText="RecID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        
                                          

                                        <asp:BoundField DataField="BrCode" HeaderText="BrCode" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        
                                        <asp:BoundField DataField="DR_Number" HeaderText="DR No." ReadOnly="True">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                         <asp:BoundField DataField="MRFNo" HeaderText="MRF No." ReadOnly="True">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>


                                        
                                        <asp:BoundField HeaderText="Plu Code" DataField="vPluCode"  ReadOnly="True" ItemStyle-Width="105px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                              
                    
                                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True"  ItemStyle-Width="375px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                      

                                   <asp:TemplateField  HeaderText="Qty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("vQty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:BoundField HeaderText="Batch No" DataField="vBatchNo"  ReadOnly="True"  ItemStyle-Width="125px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                     <asp:BoundField HeaderText="Date Expiry" DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}"  ReadOnly="True"  ItemStyle-Width="105px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                          </Columns>
                                </asp:GridView>
            </div>
        </div>
    </div>








    <!-- ################################################# START #################################################### -->
    <!-- ################################################# END #################################################### -->

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
                    title: "Deliveries",
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

</asp:Content>
