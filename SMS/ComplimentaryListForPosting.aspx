<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComplimentaryListForPosting.aspx.cs" Inherits="SMS.ComplimentaryListForPosting" %>
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
                    <h4>Complimentary for Posting</h4>
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
                                            ControlToValidate="ddBranch"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Select branch"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    Complimentary No.
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:DropDownList ID="ddComplimentaryno" runat="server"  class="form-control" Style="width: 195px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddComplimentaryno_SelectedIndexChanged" ></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                            ControlToValidate="ddComplimentaryno"  Display="Dynamic" ValidationGroup="grpComplimentary" ForeColor="Red"
                                            ErrorMessage="Select Complimentary No"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

          <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-9">
                    <button id="btnSubmit"  onserverclick="btnSubmit_Click"  type="submit" runat="server"  ValidationGroup="grpComplimentary" class="btn btn-success" >P O S T&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-ok"></span></button>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-12">
               <asp:GridView ID="gvComplimentary" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ID" OnRowDeleting="gvComplimentary_RowDeleting" OnRowCancelingEdit="gvComplimentary_RowCancelingEdit" OnRowEditing="gvComplimentary_RowEditing" OnRowUpdating="gvComplimentary_RowUpdating" OnRowDataBound="gvComplimentary_RowDataBound" >
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

                                        
                                        <asp:BoundField HeaderText="Branch" DataField="BrName"  ReadOnly="True" ItemStyle-Width="105px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="ComplimentaryNo" DataField="ComplimentaryNo"  ReadOnly="True" ItemStyle-Width="135px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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

                                      

<%--                                   <asp:TemplateField  HeaderText="Qty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("vQty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:BoundField HeaderText="Reason of return" DataField="Remarks"  ReadOnly="True"  ItemStyle-Width="125px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>--%>

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
                                                    ControlToValidate="txtQtyDR" Display="Dynamic" ValidationGroup="grpDR" ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblQtyOrig" runat="server" Visible="false"
                                                    Text='<%#  Eval("vQty","{0:###0;(###0);0}")%>'></asp:Label>

                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <HeaderStyle HorizontalAlign="Center" Width="245px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server"
                                                    Text='<%#  Eval("Remarks")%>'></asp:Label>
                                            </ItemTemplate>
                                    
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
                                                 <%--<asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" Width="85px"  ValidationGroup="grpDR" Text="Update" class="btn btn-sm btn-success" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'/>--%>
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



</asp:Content>
