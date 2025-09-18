<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewItem.aspx.cs" Inherits="SMS.AddNewItem" %>

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
        .fullscreen_bg {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
    </style>

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
                <br />
                <br />
           <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Last Plu Code Assign
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:Label ID="lblLastPlucode" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Plu Code
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtPluCode" runat="server" class="form-control"  Style="width: 100px;" MaxLength="6"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="txtPluCode" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                 <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Itemcode
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtItemcode" runat="server" class="form-control"  Style="width: 125px; text-transform:uppercase;" MaxLength="10"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtItemcode" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Description
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtDescription" runat="server" class="form-control"  Style="width: 685px;" MaxLength="250"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtDescription" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Short Name
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtShortName" runat="server" class="form-control"  Style="width: 375px;" MaxLength="31"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="txtShortName" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
          
            <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Price
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtPrice" runat="server" class="form-control decimalnumbers-only"  Style="width: 125px;  text-align: right;" MaxLength="15"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                    ControlToValidate="txtPrice" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Type
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddType" runat="server" class="form-control"  Style="width: 165px;" AutoPostBack="true" OnSelectedIndexChanged="ddType_SelectedIndexChanged" >
                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                    <asp:ListItem Text="Service" Value="Service"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" InitialValue="0"
                                    ControlToValidate="ddType" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>


         <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Group Name
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddGroupName" runat="server" class="form-control"  Style="width: 315px;" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFielddGroupName" runat="server" InitialValue="0"
                                    ControlToValidate="ddGroupName" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Category
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddCategory" runat="server" class="form-control"  Style="width: 315px;" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" InitialValue="0"
                                    ControlToValidate="ddCategory" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Session Group
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddSessionGroup" runat="server" class="form-control"  Style="width: 315px;" AutoPostBack="True" OnSelectedIndexChanged="ddSessionGroup_SelectedIndexChanged" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredddSessionGroup" runat="server" InitialValue="0"
                                    ControlToValidate="ddSessionGroup" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required if Service"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Session Type
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddSessionType" runat="server" class="form-control"  Style="width: 315px;" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredSessionType" runat="server" InitialValue="0"
                                    ControlToValidate="ddSessionType" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required if Service"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                
           <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            # of Session
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtNoSession" runat="server" ReadOnly="true" class="form-control decimalnumbers-only"  Style="width: 85px;" MaxLength="2"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RVSession" runat="server" Enabled="false"
                                    ControlToValidate="txtNoSession" Display="Dynamic" ValidationGroup="grpAddItem" ForeColor="Red"
                                    ErrorMessage="Required if Service"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

            <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            With Inventory
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:CheckBox ID="ckWithInv" runat="server" Text="&nbsp;&nbsp;&nbsp; YES " />
                            </div>
                        </div>
                    </div>
                </div>
                
           <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
           
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:Button ID="btnSave" runat="server" Text="S A V E" CssClass="btn btn-primary"  ValidationGroup="grpAddItem" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                    </div>
        </div>
          <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvNewItem" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode" OnRowDeleting="gvNewItem_RowDeleting" OnSelectedIndexChanged="gvNewItem_SelectedIndexChanged">
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

                                <asp:BoundField HeaderText="Category" DataField="vCATEGORY" ReadOnly="True" ItemStyle-Width="285px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Item Type" DataField="ItemType" ReadOnly="True" ItemStyle-Width="185px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:TemplateField HeaderText="No of Session">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoSession" runat="server"
                                            Text='<%#  Eval("NoSession","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SRP">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblvUnitCostS" runat="server"
                                            Text='<%#  Eval("vUnitCost","{0:N2}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Height="27px" />
                                            <ItemTemplate>
                                                <div class="input-group">
                                                        <asp:LinkButton ID="btnDeleteNewItem" runat="server" CommandName="Delete" Text="" ToolTip="Delete newly added item" class="btn btn-sm btn-danger"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                    </asp:TemplateField>

                            </Columns>
                         </asp:GridView>
                    </div>
           </div>

    </div>


<!-- ################################################# START #################################################### -->
    <script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>
<!-- ################################################# START #################################################### -->

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

        <!-- ################################################# START #################################################### -->
 
    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "NEW ITEM",
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
<!-- ################################################# END #################################################### -->
</asp:Content>
