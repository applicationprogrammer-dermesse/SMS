<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupSalesTarget.aspx.cs" Inherits="SMS.SetupSalesTarget" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
        <div class="row" style="margin-bottom: 20px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Setup Sales Target</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>
        <hr />
        <br />
        
        <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            For the Year :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddYear" runat="server" Width="115px" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddYear_SelectedIndexChanged"></asp:DropDownList>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0" 
                                            ControlToValidate="ddYear"  Display="Dynamic" ValidationGroup="grpSD" ForeColor="Red"
                                            ErrorMessage="Select Year"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 20px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            For the Month :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddMonth" runat="server" Width="205px" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddMonth_SelectedIndexChanged"></asp:DropDownList>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" InitialValue="0" 
                                            ControlToValidate="ddMonth"  Display="Dynamic" ValidationGroup="grpSD" ForeColor="Red"
                                            ErrorMessage="Select Special Discount"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                    <asp:GridView ID="gvTarget" Width="98%" runat="server" ShowFooter="true" DataKeyNames="ID" AutoGenerateColumns="false" OnRowCancelingEdit="gvTarget_RowCancelingEdit" OnRowEditing="gvTarget_RowEditing" OnRowUpdating="gvTarget_RowUpdating" OnRowDataBound="gvTarget_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" FooterStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="BrName" HeaderText="Branch Name" ReadOnly="true">
                                <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="iYear" HeaderText="Year" ReadOnly="true">
                                <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Month" HeaderText="Month" ReadOnly="true">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                                 <asp:TemplateField HeaderText="Target">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbliTarget" runat="server" 
                                                    Text='<%#  Eval("iTarget","{0:#,##0.##;(###0.##);0}")%>'></asp:Label>
                                            </ItemTemplate>
                                      <FooterTemplate>
                                            <asp:Label ID="lbltotalTarget" runat="server"></asp:Label>
                                        </FooterTemplate>
                                            <EditItemTemplate>
                                                 <asp:TextBox ID="txtiTarget" runat="server" class="form-control decimalnumbers-only" Width="125px" Text='<%#Eval("iTarget","{0:###0.##;(###0.##);0}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqQtyiTarget" runat="server" 
                                                    ControlToValidate="txtiTarget"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                       


                                     </asp:TemplateField>

                               <asp:TemplateField HeaderText="No. of Day Closed">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbliDay" runat="server" 
                                                    Text='<%#  Eval("iDay","{0:###0;(###0);0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                 <asp:TextBox ID="txtiDay" runat="server" class="form-control decimalnumbers-only" Width="75px" Text='<%#Eval("iDay","{0:###0;(###0);0}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqQtyiDay" runat="server" 
                                                    ControlToValidate="txtiDay"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                     </asp:TemplateField>


                            

                            

                            <asp:BoundField DataField="EditedBy" HeaderText="Edited By" ReadOnly="true">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="EditedDate" HeaderText="Edited Date" ReadOnly="true">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            
                            <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditGrid" runat="server" CommandName="Edit" Text="EDIT" class="btn btn-sm btn-primary" />

                                            </ItemTemplate>
                                             <EditItemTemplate>
                                                 <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" Width="85px"  ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' Text="Update" class="btn btn-sm btn-success" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'/>
                                                 <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="Cancel" Width="85px" class="btn btn-sm btn-warning" />
                                             </EditItemTemplate>
                                        </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
</div>



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
                    title: "Sales Target",
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
            $(".dateAll").datepicker({
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

