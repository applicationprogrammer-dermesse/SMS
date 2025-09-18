<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Issuance.aspx.cs" Inherits="SMS.Issuance" %>
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
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
     <div class="container-fluid"  style="width:98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                    <h4>Issuance</h4>
                </div>
                <div class="col-md-4 text-center">
                
                </div>
            </div>
        </div>
         <div class="row">
                    <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                    Issuance No.
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtIssuanceNo" runat="server" class="form-control" ReadOnly="true" Style="width: 185px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <button id="btnRefreshIssNo" onserverclick="btnRefreshIssNo_Click"  type="submit" runat="server" class="btn btn-default"><span class="glyphicon glyphicon-refresh"></span></button>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            ControlToValidate="txtIssuanceNo"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     <%--<asp:TextBox ID="TextTest"  runat="server" class="form-control" Style="width: 185px; margin-bottom: 5px;"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>--%>
                                     </div>
                                </div>
                                    <div class="col-sm-3 text-right">
                                        <button id="btnPrintPreview"  onserverclick="btnPrintPreview_Click" type="submit" runat="server"  class="btn btn-default" >PRINT PREVIEW&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-print"></span></button>
                                        &nbsp;
                                        <button id="btnSubmit"  onserverclick="btnSubmit_Click" type="submit" runat="server"  class="btn btn-success" >SUBMIT&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-ok"></span></button>
                                    </div>
                            </div>
                </div>

         <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Branch
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddBranch" runat="server"  class="form-control" Style="width: 195px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                            ControlToValidate="ddBranch"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Select branch"></asp:RequiredFieldValidator>
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
                                            ControlToValidate="txtDateReceived"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateReceived"
                                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                            ForeColor="Red" Display="Dynamic"
                                            ErrorMessage="Invalid date format"
                                            ValidationGroup="grpIssuance"></asp:RegularExpressionValidator>
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
                                        <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Style="width: 650px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')"></asp:DropDownList>
                                         &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkSelectBatch" runat="server" ForeColor="Blue" Font-Underline="true" OnClick="lnkSelectBatch_Click">Select Batch</asp:LinkButton>
                                         &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkBatchPIS" runat="server" ForeColor="Maroon" Font-Underline="true" OnClick="lnkBatchPIS_Click">Get Batch(PIS)</asp:LinkButton>                                        
                                         &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkBtachPRF" runat="server" ForeColor="Maroon" Font-Underline="true" OnClick="lnkBtachPRF_Click" >Get Batch(PRF)</asp:LinkButton>                                        
                                    </div>
                                </div>
                             </div>
                </div>

         <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Option
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:CheckBox ID="ckOption" runat="server" Checked="true" AutoPostBack="true" Text="&nbsp;&nbsp;&nbsp; Unchecked to show not clinic supplies item" OnCheckedChanged="ckOption_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="ckOption2" runat="server" Checked="false" AutoPostBack="true" Text="&nbsp;&nbsp;&nbsp; Check if item not in PIS or PRF" OnCheckedChanged="ckOption2_CheckedChanged"  />
                    </div>
                </div>
            </div>
        </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Balance
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtBalance" runat="server" ReadOnly="true" class="form-control decimalnumbers-only" Style="width: 95px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                     <asp:TextBox ID="txtID" runat="server" Visible="false"  ReadOnly="true" class="form-control decimalnumbers-only" Style="width: 65px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="requiredBalance" runat="server" 
                                            ControlToValidate="txtBalance"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Required(Please click Select Batch)"></asp:RequiredFieldValidator>
                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Source
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                        <asp:Label ID="lblSource" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Batch No.
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtBatchNo" runat="server" ReadOnly="true" class="form-control" Style="width: 195px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                            ControlToValidate="txtBatchNo"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage=" Please Supply Batch No."></asp:RequiredFieldValidator>

                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Expiration Date
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtExpirationDate" runat="server" ReadOnly="true" class="form-control dateTxtYear" Style="width: 105px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                            ControlToValidate="txtExpirationDate"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage=" Please Supply Expiration Date"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RelarExpressionValidator1" runat="server" ControlToValidate="txtExpirationDate"
                                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                            ForeColor="Red" Display="Dynamic"
                                            ErrorMessage="Invalid date format"
                                            ValidationGroup="grpIssuance"></asp:RegularExpressionValidator>
                                    </div>
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
                                            ControlToValidate="txtQty"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                         <asp:CompareValidator ID="cvQty" runat="server"
                                        ErrorMessage="Insufficient balance!" ValidationGroup="grpIssuance" Display="Dynamic"
                                        ControlToCompare="txtBalance" ControlToValidate="txtQty" Type="Double" Operator="LessThanEqual"
                                        ForeColor="Red"></asp:CompareValidator>
                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row">
                            <div class="col-sm-12" style="margin-bottom: 5px;">
                                            
                                <div class="col-sm-2 text-right">
                                   Hard Copy Issue Slip No.
                                </div>
                                <div class="col-sm-10" >
                                    <div class="input-group">
                                     <asp:TextBox ID="txtReason" runat="server" MaxLength="148"  class="form-control" Style="width: 145px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator101" runat="server" 
                                            ControlToValidate="txtReason"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Required(Put 0 if no issue slip no.)"></asp:RequiredFieldValidator>
                                    </div>
                                </div>


                             </div>
                </div>

         <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Delivered By
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddDeliveredBy" runat="server"  class="form-control" Style="width: 275px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAddEmployee" runat="server" Text="ADD EMPLOYEE" CssClass="btn btn-default" OnClick="btnAddEmployee_Click" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" InitialValue="0"
                                            ControlToValidate="ddDeliveredBy"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Please Select Delivered By"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="margin-bottom: 15px;">
                            <div class="col-sm-12">
                                <div class="col-sm-2 text-right">
                                </div>
                                <div class="col-sm-10">
                                                <button id="btnSave" onserverclick="btnSave_Click" type="submit" runat="server"  class="btn btn-primary" validationgroup="grpIssuance">SAVE&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-save"></span></button>
                                                <%--  <button id="btnClearItem" type="submit" runat="server"  class="btn btn-default" formnovalidate="formnovalidate">CLEAR ENTRY&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-refresh"></span></button>--%>
                                                <%--<button id="btnPrint"  type="submit" runat="server" class="btn btn-danger" visible="false"  validationgroup="grpIssuance">P R I N T&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>--%>
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

                                      <asp:BoundField HeaderText="Issuance No." DataField="IssuanceNo"  ReadOnly="True" ItemStyle-Width="125px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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

                             

                                 <asp:TemplateField HeaderText="Batch No.">
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
                                     </asp:TemplateField>

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
                                            <%--     <asp:Label ID="lblQtyOrig" runat="server"   Visible="false"
                                                    Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>--%>

                                        </EditItemTemplate>
                                     </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderText="Reason">
                                            <HeaderStyle HorizontalAlign="Center" Width="285px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblReasonOfReturn" runat="server" 
                                                    Text='<%#  Eval("ReasonOfReturn")%>'></asp:Label>
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:TextBox ID="txtReasonOfReturn" runat="server" class="form-control" Width="215px" Text='<%#Eval("ReasonOfReturn") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: left;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqReasonOfReturn" runat="server" 
                                                    ControlToValidate="txtReasonOfReturn"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                     </asp:TemplateField>--%>

                                   

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
                    title: "Issuance",
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
        $(".datePrev").datepicker();
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
                    title: "Submit Issuance",
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
      <script type="text/javascript">
          function CloseGridItemBatch() {
              $(function () {
                  $("#ShowItemBatch").dialog('close');
              });
          }
    </script>

 <!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function ShowGridItemBatch() {
            $(function () {
                $("#ShowItemBatch").dialog({
                    title: "Item Batch",
                    //position: ['center', 20],

                    width: '1000px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowItemBatch").parent().appendTo($("form:first"));
            });
        }
    </script>

    <div id="ShowItemBatch" style="display: none;">
      <%--  <asp:UpdatePanel ID="UpdatePanel20" runat="server">
         <ContentTemplate> --%> 
                <asp:GridView ID="gvItemBalance" runat="server" AutoGenerateColumns="False" OnRowUpdating="gvItemBalance_RowUpdating" >
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
		
    
                        <asp:BoundField HeaderText="Rec No." DataField="Sup_RecNum" ItemStyle-Width="95px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Item Code" DataField="Sup_ItemCode" ItemStyle-Width="155px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                
                        <asp:BoundField DataField="Sup_ItemDesc" HeaderText="Item Description" ReadOnly="True">
                            <HeaderStyle Width="295px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Sup_BatchNo" HeaderText="Batch No" ReadOnly="True">
                            <HeaderStyle Width="195px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                      

                        <asp:BoundField DataField="Sup_DateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                            <HeaderStyle Width="115px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="True">
                            <HeaderStyle Width="195px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Center" Width="175px" />
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="lnkSelectID" runat="server" class="btn btn-success btn-sm" CommandName="Select" Text="SELECT" ForeColor="white" ValidationGroup="grpSession" OnClientClick="return  CloseGridItemBatch();" />--%>
                        <asp:Button ID="btnSelectID" runat="server" Text="SELECT BATCH" CssClass="btn btn-primary" CommandName="Update" OnClientClick="return  CloseGridItemBatch();" />
                    </ItemTemplate>
                </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
              <%--</ContentTemplate>
        </asp:UpdatePanel>--%>

    
    </div>
  <!-- ################################################# END #################################################### -->


<!-- ################################################# START #################################################### -->
    <%--*******************************************************************************--%>
      <script type="text/javascript">
          function CloseGridPRFBatch() {
              $(function () {
                  $("#ShowPRFBatch").dialog('close');
              });
          }
    </script>

    <script type="text/javascript">
        function ShowGridPRFBatch() {
            $(function () {
                $("#ShowPRFBatch").dialog({
                    title: "PRF Batch",
                    //position: ['center', 20],

                    width: '1000px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowPRFBatch").parent().appendTo($("form:first"));
            });
        }
    </script>

    <div id="ShowPRFBatch" style="display: none;">
        <div style="overflow:auto; max-height:400px;">
                <asp:GridView ID="gvPRFBatch" runat="server" AutoGenerateColumns="False" OnRowUpdating="gvPRFBatch_RowUpdating" >
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
		
    
                        <asp:BoundField HeaderText="Rec No." DataField="ID" ItemStyle-Width="95px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                           <asp:BoundField HeaderText="PRF No" DataField="PRFno" ItemStyle-Width="255px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Item Code" DataField="vFGCode" ItemStyle-Width="155px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                
                   <%--     <asp:BoundField DataField="Sup_ItemDesc" HeaderText="Item Description" ReadOnly="True">
                            <HeaderStyle Width="295px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>--%>

                        <asp:BoundField DataField="vBatchNo" HeaderText="Batch No" ReadOnly="True">
                            <HeaderStyle Width="195px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                      

                        <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                            <HeaderStyle Width="115px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                       <asp:BoundField DataField="ReasonOfReturn" HeaderText="Reason Of Return" ReadOnly="True">
                            <HeaderStyle Width="395px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="vQty" HeaderText="Qty" ReadOnly="True">
                            <HeaderStyle Width="125px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Center" Width="175px" />
                    <ItemTemplate>
                        <asp:Button ID="btnSelectPRFID" runat="server" Text="SELECT BATCH" CssClass="btn btn-primary" CommandName="Update" OnClientClick="return  CloseGridPRFBatch();" />
                    </ItemTemplate>
                </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
        </div>
    
    </div>
  <!-- ################################################# END #################################################### -->

<!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        function CloseGridAddEmployee() {
            $(function () {
                $("#ShowAddEmployee").dialog('close');
            });
        }
    </script>



    <script type="text/javascript">
        function ShowGridAddEmployee() {
            $(function () {
                $("#ShowAddEmployee").dialog({
                    title: "Add New Employee",
                    //position: ['center', 20],

                    width: '720px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowAddEmployee").parent().appendTo($("form:first"));
            });
        }
    </script>
    	

    <div id="ShowAddEmployee" style="display: none;">
        <br />
	    <br />
         <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
                        Employee Name
                    </div>
                    <div class="col-sm-8 text-left"">
                        <div class="input-group">
                            <asp:TextBox ID="txtEEmployeeName" runat="server" class="form-control" MaxLength="150" Text="" Style="width: 285px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvEmployee" runat="server"
                                ControlToValidate="txtEEmployeeName" Display="Dynamic" ValidationGroup="grpE" ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


         

        <div class="row">
                <div class="col-sm-12" style="margin-bottom: 5px;">
                    <div class="col-sm-3 text-right">
        
                    </div>
                    <div class="col-sm-8">
                        <div class="input-group">
                            <asp:Button ID="btnEaddEmployee" runat="server" Text="S A V E" class="btn btn-primary" ValidationGroup="grpE" OnClick="btnEaddEmployee_Click"/>
                        </div>
                    </div>

                </div>
            </div>


    </div>

<!-- ################################################# END #################################################### -->    



</asp:Content>
