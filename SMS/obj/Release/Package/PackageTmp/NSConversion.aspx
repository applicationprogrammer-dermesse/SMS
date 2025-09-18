<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NSConversion.aspx.cs" Inherits="SMS.NSConversion" %>
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
        text-align:center;
        vertical-align: middle;
         background-color:#f2f2f2;
              font-size:12px;
    }
     table tr {
        vertical-align: middle;
        font-size:12px;
    }
     .hiddencol { display: none; }

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
.ui-dialog { position: fixed; padding: .1em; width: 300px; overflow: hidden; }

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
     <div class="container-fluid"  style="width:98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                    <h4>Nuderm Supreme Conversion</h4>
                </div>
                <div class="col-md-4 text-center">
                
                </div>
            </div>
        </div>

          
         <hr />
         <br />
 

           <div class="row">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Adjustment No.
                                </div>
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAdjustmentNo" runat="server" class="form-control" ReadOnly="true" Style="width: 175px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            ControlToValidate="txtAdjustmentNo"  Display="Dynamic" ValidationGroup="grpAdjustment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                                    <div class="col-sm-1 text-right">
                                            <button id="btnSubmit"  onserverclick="btnSubmit_Click" type="submit" runat="server"  class="btn btn-success" >P O S T&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-ok"></span></button>
                                    </div>
                            </div>
                </div>
         

            <div class="row">
                           <div class="col-sm-12" style="margin-bottom: 5px;">
                                <div class="col-sm-2 text-right">
                                    Item Description
                                </div>
                                <div class="col-sm-9" >
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Style="width: 595px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddITemFG_SelectedIndexChanged" ></asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkSelect" Visible="false" Enabled="false" runat="server" class="btn btn-default" OnClick="lnkSelect_Click">Select Batch No.</asp:LinkButton>
                                    </div>
                                </div>

                             </div>
                                 

                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 15px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Balance

                                    
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAvailable" runat="server"  class="form-control"  ReadOnly="true" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:TextBox ID="txtItemID" runat="server" BackColor="Transparent" ForeColor="Transparent" BorderStyle="None"  class="form-control" ReadOnly="true" Style="width: 55px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ReqAvail" runat="server"
                                                ControlToValidate="txtAvailable" Display="Dynamic" ValidationGroup="grpAdjustment" ForeColor="Red"
                                                ErrorMessage="Please select item"></asp:RequiredFieldValidator>

                                    </div>
                                </div>


                             </div>
                </div>


           <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Quantity To Adjust
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtQty" runat="server"  class="form-control" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        &nbsp;&nbsp;  
                                        <asp:Label ID="Label1" runat="server" ForeColor="Silver" Text="NOTE: Put a negative sign (-) if adjustment is deduction of item"></asp:Label>
                                        <asp:RequiredFieldValidator ID="ReqQty" runat="server" 
                                            ControlToValidate="txtQty"  Display="Dynamic" ValidationGroup="grpAdjustment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ControlToValidate="txtQty"
                                            ValidationGroup="grpAdjustment"
                                            ErrorMessage="Only numeric character allowed here!"
                                            ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                            ForeColor="Red"
                                            Display="Dynamic"></asp:RegularExpressionValidator>

                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 15px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Remarks
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtReason" runat="server" Text="Package Convertion"   class="form-control"  Style="width: 385px;" MaxLength="249" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ControlToValidate="txtReason"  Display="Dynamic" ValidationGroup="grpAdjustment" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        

                                    </div>
                                </div>


                             </div>
                </div>

        <div class="row" style="margin-bottom: 15px;">
                            <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                </div>
                                <div class="col-sm-2">
                                    <button id="btnSave"  onserverclick="btnSave_Click" type="submit" runat="server"  class="btn btn-primary" validationgroup="grpAdjustment">SAVE&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-save"></span></button>
                                </div>
                             
                            </div>

             </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-left:5px;">
                                <asp:GridView ID="gvAdjustment" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ID" OnRowDeleting="gvAdjustment_RowDeleting" >
                                    <Columns>

                                        <asp:BoundField DataField="ID" HeaderText="RecID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                         <asp:BoundField DataField="vItemID" HeaderText="vItemID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                    
                                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="95px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True"  ItemStyle-Width="395px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                      <asp:TemplateField HeaderText="Balance">
                                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblvQtyBalance" runat="server" 
                                                    Text='<%#  Eval("vQtyBalance","{0:###0;(###0);0}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                  <%--      <asp:TemplateField HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" 
                                                    Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                   <asp:TemplateField  HeaderText="Qty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("vQty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:BoundField HeaderText="Reason of Adjustment" DataField="Remarks"  ReadOnly="True"  ItemStyle-Width="125px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                           <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                
                                                <asp:LinkButton ID="btnDeleteUnposted" runat="server" CommandName="Delete" Width="85px" Text="DELETE" class="btn btn-sm btn-danger" />
                                            </ItemTemplate>
                                         
                                        </asp:TemplateField>
                                          </Columns>
                                </asp:GridView>
                            </div>
             </div>

    </div>  


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
                    title: "NS Conversion",
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
    <%-- OnRowUpdating="gvItemBatch_RowUpdating" style="font-size: 12px;" OnRowDataBound="gvItemBatch_RowDataBound"--%>

    <script type="text/javascript">
        function CloseGridItemBatches() {
            $(function () {
                $("#ShowItemBatches").dialog('close');
            });
        }
</script>



    <script type="text/javascript">
        function ShowGridItemBatches() {
            $(function () {
                $("#ShowItemBatches").dialog({
                    title: "Orders",
                    //position: ['center', 20],

                    width: '750px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                //$("#ShowItemBatches").parent().appendTo($("form:first"));
            });
        }
    </script>


    <div id="ShowItemBatches" style="display:none;">  
            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
           <%-- <ContentTemplate>  --%>  
                <asp:Label Text="" ID="lblShowItemBatchesHeader" runat="server" />
                 <br />
                <asp:Label Text="" ID="lblUnpostedRecNum" Visible="false" runat="server" />
                <asp:GridView ID="gvItemBatch" runat="server" AutoGenerateColumns="False" OnRowUpdating="gvItemBatch_RowUpdating" OnSelectedIndexChanged="gvItemBatch_SelectedIndexChanged" OnRowDataBound="gvItemBatch_RowDataBound">
                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                         <Columns>
                             <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Width="175px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelectItemBatch" runat="server" class="btn btn-success btn-sm" CommandName="Update" Text="SELECT" ForeColor="white" ValidationGroup="grpPICKQ" OnClientClick="return   CloseGridItemBatches();" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                              

                                <asp:BoundField HeaderText="BATCH #" DataField="vBatchNo" ItemStyle-Width="195px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="DATE EXPIRY">
                                            <HeaderStyle HorizontalAlign="Center" Width="75px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbDateExpiryIssue" runat="server"
                                                    Text='<%#  Convert.ToString(Eval("vDateExpiry", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vDateExpiry", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                  </asp:TemplateField>

                                <asp:BoundField HeaderText="Available Balance" DataField="Available Balance" ItemStyle-Width="85px" DataFormatString="{0:###0.####;-###0.####;0}">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>



                             <asp:BoundField HeaderText="Unposted Qty" DataField="Unposted Qty" ItemStyle-Width="85px" DataFormatString="{0:###0.####;-###0.####;0}">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                             <%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                <asp:BoundField DataField="vItemID" HeaderText="vItemID" ReadOnly="True" >
                                    <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>



                             </Columns>
                    </asp:GridView>
             <%--   </ContentTemplate>
                </asp:UpdatePanel>--%>
    </div>









     <%--*******************************************************************************--%>

    <script type="text/javascript">
        function ShowConfirmSubmit() {
            $(function () {
                $("#ConfirmSubmit").dialog({
                    title: "NS Conversion",
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
        <asp:Button ID="btnConfirmSubmit" runat="server" Text="YES"  class="btn btn-success btn-sm"  OnClick="btnConfirmSubmit_Click" />
    </div>
    <%--"--%>

    <%--*******************************************************************************--%>


</asp:Content>
