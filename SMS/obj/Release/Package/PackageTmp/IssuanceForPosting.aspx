<%@ Page Title=""  MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IssuanceForPosting.aspx.cs" Inherits="SMS.IssuanceForPosting" %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Issuance for Posting</h4>
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
                    Branch
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddBranch" runat="server"  class="form-control" Style="width: 195px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged" ></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0"
                                            ControlToValidate="ddBranch"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Select branch"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Issue Slip No.
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddIssueNo" runat="server"  class="form-control" Style="width: 215px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddIssueNo_SelectedIndexChanged" ></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                            ControlToValidate="ddIssueNo"  Display="Dynamic" ValidationGroup="grpIssuance" ForeColor="Red"
                                            ErrorMessage="Select Issue Slip No"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-2">
                    <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">EXPORT&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                </div>
        
            </div>
        </div>


          <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-2">
                    <button id="btnSubmit"  onserverclick="btnSubmit_Click"  type="submit" runat="server"  ValidationGroup="grpIssuance" class="btn btn-primary"  onclick="showDiv();">P O S T&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-ok"></span></button>
                </div>
                <div class="col-sm-1">
                    <div id='myHiddenDiv' style="width: 18%; float: left; height: 50px; line-height: 50px; text-align:left;">
                                  <img src='' id='myAnimatedImage' alt="" height="50" /> 
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-12">
               <asp:GridView ID="gvIssuance" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ID" OnRowDeleting="gvIssuance_RowDeleting" OnRowCancelingEdit="gvIssuance_RowCancelingEdit" OnRowEditing="gvIssuance_RowEditing" OnRowUpdating="gvIssuance_RowUpdating" OnRowDataBound="gvIssuance_RowDataBound" >
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

                                         <asp:BoundField DataField="vItemID" HeaderText="vItemID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        
                                        <asp:BoundField HeaderText="Branch" DataField="BrName"  ReadOnly="True" ItemStyle-Width="145px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Issue Slip No" DataField="IssuanceNo"  ReadOnly="True" ItemStyle-Width="180px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                    
                                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode"  ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION"  ReadOnly="True"  ItemStyle-Width="355px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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
                                                 <%--<asp:RequiredFieldValidator ID="ReqBatchNoDR" runat="server" 
                                                    ControlToValidate="txtBatchNoDR"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
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
                                                 <%--<asp:RequiredFieldValidator ID="ReqvDateExpiry" runat="server" 
                                                    ControlToValidate="txtDateExpiryDR"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
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
                                                  <asp:Label ID="lblQtyOrig" runat="server" Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>
                                                 <asp:TextBox ID="txtQtyIssuance" runat="server" class="form-control decimalnumbers-only" Width="75px" Text='<%#Eval("vQty","{0:###0;(###0);0}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqQtyDR" runat="server" 
                                                    ControlToValidate="txtQtyIssuance"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                     </asp:TemplateField>

                                        
                                        <asp:TemplateField HeaderText="Issue Slip No.(Hard Copy)">
                                            <HeaderStyle HorizontalAlign="Center" Width="165px" />
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

                                                                              


                                        <asp:TemplateField HeaderText="DATE DELIVERED">
                                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssuanceDate" runat="server" 
                                                    Text='<%#  Eval("IssuanceDate","{0:MM/dd/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:TextBox ID="txtIssuanceDateDR" runat="server" class="dateTxtYear form-control" Width="105px" Text='<%#Eval("IssuanceDate","{0:MM/dd/yyyy}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqvDateIssuance" runat="server" 
                                                    ControlToValidate="txtIssuanceDateDR"  Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                     </asp:TemplateField>

                                        

                                        <asp:BoundField DataField="IsPIS" HeaderText="Src" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    

                                        <asp:BoundField DataField="SourceRecord" HeaderText="SR" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="5px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
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



    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "ISSUANCE",
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


</asp:Content>
