<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Complimentary.aspx.cs" Inherits="SMS.Complimentary" %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <div class="container-fluid"  style="width:98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                    <h4>Complimentary</h4>
                </div>
                <div class="col-md-4 text-center">
                
                </div>
            </div>
        </div>
         <div class="row">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Complimentary No.
                                </div>
                                <div class="col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtComplimentaryNo" runat="server" class="form-control" ReadOnly="true" Style="width: 195px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            ControlToValidate="txtComplimentaryNo"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     </div>
                                </div>
                                    <div class="col-sm-1 text-right">
                                        <button id="btnSubmit"  onserverclick="btnSubmit_Click" type="submit" runat="server"  class="btn btn-success" >SUBMIT&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-ok"></span></button>
                                    </div>
                            </div>
                </div>
          <div class="row">
                           <div class="col-sm-12" style="margin-bottom: 5px;">
                                <div class="col-sm-2 text-right">
                                    Type
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddCompliType" runat="server" class="form-control" Style="width: 295px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="Please select" 
                                            ControlToValidate="ddCompliType"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                             </div>
                </div>

          <div class="row">
                            <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Date
                                </div>
                                <div class="col-sm-10">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDateReceived" runat="server" class="datePrev form-control" Style="width: 105px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ReqDate" runat="server" 
                                            ControlToValidate="txtDateReceived"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateReceived"
                                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                            ForeColor="Red"
                                            ErrorMessage="Invalid date format"
                                            ValidationGroup="grpComplimentary"></asp:RegularExpressionValidator>
                                     </div>
                                </div>
                            </div>
                </div>

        <div class="row">
                           <div class="col-sm-12" style="margin-bottom: 5px;">
                                <div class="col-sm-2 text-right">
                                    Item Description
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Style="width: 695px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddITemFG_SelectedIndexChanged" ></asp:DropDownList>
                                    </div>
                                </div>
                             </div>
                </div>

            <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">

                <div class="col-sm-2 text-right">
                    Balance
                </div>
                <div class="col-sm-8">
                    <div class="input-group">
                        <asp:TextBox ID="txtAvailable" runat="server" class="form-control" ReadOnly="true" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:TextBox ID="txtItemID" runat="server"  class="form-control" ReadOnly="true" Style="width: 55px; display:none;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ReqAvail" runat="server"
                            ControlToValidate="txtAvailable" Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                            ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                    </div>
                </div>
                
        </div>
      </div>

         <div class="row">
                           <div class="col-sm-12" style="margin-bottom: 5px;">

                <div class="col-sm-2 text-right">
                   SRP
                </div>
                <div class="col-sm-8">
                    <div class="input-group">
                        <asp:TextBox ID="txtSRP" runat="server" class="form-control  decimalnumbers-only" Style="width: 125px; text-align:right;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                </div>
                
        </div>

           <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Quantity
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtQty" runat="server"  class="form-control decimalnumbers-only" Style="width: 85px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ReqQty" runat="server" 
                                            ControlToValidate="txtQty"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvQty" runat="server"
                                            ErrorMessage="Qty s/b less than or equal to available balance!" ValidationGroup="grpComplimentary" Display="Dynamic"
                                            ControlToCompare="txtAvailable" ControlToValidate="txtQty" Type="Double" Operator="LessThanEqual"
                                            ForeColor="Red"></asp:CompareValidator>
                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Customer Name/Remarks
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtRemarks" runat="server" MaxLength="348"  class="form-control" Style="width: 485px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator101" runat="server" 
                                            ControlToValidate="txtRemarks"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>


                             </div>
                </div>



        <div class="row" style="margin-bottom: 15px;">
                            <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                </div>
                                <div class="col-sm-10">
                                                <button id="btnSave" onserverclick="btnSave_Click" type="submit" runat="server"  class="btn btn-primary" validationgroup="grpComplimentary">SAVE&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-save"></span></button>
                                                <%--  <button id="btnClearItem" type="submit" runat="server"  class="btn btn-default" formnovalidate="formnovalidate">CLEAR ENTRY&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-refresh"></span></button>--%>
                                                <%--<button id="btnPrint"  type="submit" runat="server" class="btn btn-danger" visible="false"  validationgroup="grpComplimentary">P R I N T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>--%>
                                                &nbsp;&nbsp;&nbsp; <%--onserverclick="btnSubmit_Click"--%>
                                                
                                            </div>
                                    
                                </div>

             </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-left:5px;">
                                <asp:GridView ID="gvUnposted" runat="server" Width="99%" Font-Size="Medium" DataKeyNames="ID" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false"  OnRowCancelingEdit="gvUnposted_RowCancelingEdit" OnRowDeleting="gvUnposted_RowDeleting" OnRowEditing="gvUnposted_RowEditing" OnRowUpdating="gvUnposted_RowUpdating" >
                                    <Columns>

                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                      <asp:BoundField HeaderText="Complimentary No." DataField="Complimentaryno"  ReadOnly="True" ItemStyle-Width="125px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>



                                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True"  ItemStyle-Width="395px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                             

                                  <%--   <asp:TemplateField HeaderText="Batch No.">
                                            <HeaderStyle HorizontalAlign="Center" Width="145px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblBatchNoDR" runat="server" 
                                                    Text='<%#  Eval("vBatchNo")%>'></asp:Label>
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:TextBox ID="txtBatchNoDR" runat="server" class="form-control" Width="135px" Text='<%#Eval("vBatchNo") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqBatchNoDR" runat="server" 
                                                    ControlToValidate="txtBatchNoDR"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                     </asp:TemplateField>

                                        <asp:TemplateField HeaderText="DATE EXPIRY">
                                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblvDateExpiry" runat="server" 
                                                    Text='<%#  Eval("vDateExpiry","{0:MM/dd/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:TextBox ID="txtDateExpiryDR" runat="server" class="dateTxtYear form-control" Width="105px" Text='<%#Eval("vDateExpiry","{0:MM/dd/yyyy}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqvDateExpiry" runat="server" 
                                                    ControlToValidate="txtDateExpiryDR"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                     </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" 
                                                    Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                  
                                                 <asp:TextBox ID="txtQtyDR" runat="server" class="form-control decimalnumbers-only" Width="75px" Text='<%#Eval("vQty","{0:###0;(###0);0}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqQtyDR" runat="server" 
                                                    ControlToValidate="txtQtyDR"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                 <asp:Label ID="lblQtyOrig" runat="server" Visible="false"
                                                    Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>

                                        </EditItemTemplate>
                                     </asp:TemplateField>

                                          <asp:TemplateField HeaderText="SRP">
                                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSRP" runat="server" 
                                                    Text='<%#  Eval("SRP","{0:N2}")%>'></asp:Label>
                                            </ItemTemplate>
                                             
                                     </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Remarks">
                                            <HeaderStyle HorizontalAlign="Center" Width="285px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" 
                                                    Text='<%#  Eval("Remarks")%>'></asp:Label>
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:TextBox ID="txtRemarks" runat="server" class="form-control" Width="215px" Text='<%#Eval("Remarks") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: left;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqRemarks" runat="server" 
                                                    ControlToValidate="txtRemarks"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                     </asp:TemplateField>

                                        <asp:BoundField HeaderText="Type" DataField="CompliType"  ReadOnly="True"  ItemStyle-Width="175px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                         <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="65px" Height="27px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditGrid" runat="server" CommandName="Edit" Text="EDIT" class="btn btn-sm btn-primary" />

                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" Width="85px"  ValidationGroup="grpDR" Text="Update" class="btn btn-sm btn-success" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'/>
                                                 <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="Cancel" Width="85px" class="btn btn-sm btn-warning" />
                                                 <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Width="85px" Text="DELETE" class="btn btn-sm btn-danger" />
                                             </EditItemTemplate>
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
                    title: "Complimentary",
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

    <script type="text/javascript">
        $(function () {
            $('.dateTxtYear').datepicker({
                changeMonth: true,
                changeYear: true
            });
        });



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



    








     <%--*******************************************************************************--%>

    <script type="text/javascript">
        function ShowConfirmSubmit() {
            $(function () {
                $("#ConfirmSubmit").dialog({
                    title: "Complimentary",
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
        <asp:Button ID="btnConfirmSubmit" runat="server" Text="YES"  class="btn btn-success btn-sm"  OnClick="btnConfirmSubmit_Click" />
    </div>


    <%--*******************************************************************************--%>


</asp:Content>
