<%@ Page Title="Employee Setup" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="setupEmployee.aspx.cs" Inherits="SMS.setupEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        table th {

            text-align: center;
            vertical-align: middle;
            background-color: #f2f2f2;
            font-size: 15px;

            
            
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
                    <h4>Setup Employee</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>
        <hr />
        <br />

       <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                    <asp:GridView ID="gvEmployee" Width="98%" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" OnRowCancelingEdit="gvEmployee_RowCancelingEdit" OnRowEditing="gvEmployee_RowEditing" OnRowUpdating="gvEmployee_RowUpdating" OnRowDataBound="gvEmployee_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                          <asp:TemplateField HeaderText="Employee No">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpNo" runat="server" 
                                                    Text='<%#  Eval("EmpNo")%>'></asp:Label>
                                            </ItemTemplate>
                                      
                                            <EditItemTemplate>
                                                 <asp:TextBox ID="txtEmpNo" runat="server" class="form-control  decimalnumbers-only" Width="125px" Text='<%#Eval("EmpNo") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqEmpNo" runat="server" 
                                                    ControlToValidate="txtEmpNo"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>



                                 <asp:TemplateField HeaderText="Employee Name">
                                            <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" 
                                                    Text='<%#  Eval("EmployeeName")%>'></asp:Label>
                                            </ItemTemplate>
                                      
                                            <EditItemTemplate>
                                                 <asp:TextBox ID="txtEmployeeName" runat="server" class="form-control" Width="325px" Text='<%#Eval("EmployeeName") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                <asp:Label ID="lblPerformedBy" runat="server" Text='<%#Eval("EmployeeName") %>' Visible="false"></asp:Label>
                                                 <asp:RequiredFieldValidator ID="ReqEmployeeName" runat="server" 
                                                    ControlToValidate="txtEmployeeName"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>

                              <asp:TemplateField HeaderText="Position">
                                            <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPosition" runat="server" 
                                                    Text='<%#  Eval("Position")%>'></asp:Label>
                                            </ItemTemplate>
                                      
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddPosition" runat="server" class="form-control" BackColor="#ffcc99" ></asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="ReqPosition" runat="server" InitialValue="0" 
                                                    ControlToValidate="ddPosition"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>


                            

                            
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
                    title: "Employee",
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
