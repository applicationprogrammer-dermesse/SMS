<%@ Page Title="Edit Item" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="setupItem.aspx.cs" Inherits="SMS.setupItem" %>
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
                <div class="row" style="margin-bottom: 12px;">
                    <div class="row">
                        <div class="col-md-4 text-center">
                        </div>
                        <div class="col-sm-3 text-center" style="background-color:#f2f2f2; color:maroon; border-radius: 15px;">
                            <h4>ITEM MASTER LIST</h4>
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
                                    ControlToValidate="ddType" Display="Dynamic" ValidationGroup="grpItem" ForeColor="Red"
                                    ErrorMessage="Please select type"></asp:RequiredFieldValidator>
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
                                <asp:Button ID="btnGen" runat="server" Text="GENERATE" CssClass="btn btn-primary"  ValidationGroup="grpItem" OnClick="btnGen_Click" />
                
                            </div>
                        </div>
                    </div>
                </div>


     <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvItem" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode" OnRowCancelingEdit="gvItem_RowCancelingEdit" OnRowEditing="gvItem_RowEditing" OnRowUpdating="gvItem_RowUpdating" OnRowDataBound="gvItem_RowDataBound" >
                            <Columns>
                                <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="95px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Plu Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                 <asp:TemplateField HeaderText="Item Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblvDESCRIPTION" runat="server" 
                                                    Text='<%#  Eval("vDESCRIPTION")%>'></asp:Label>
                                            </ItemTemplate>
                                      
                                            <EditItemTemplate>
                                                 <asp:TextBox ID="txtvDESCRIPTION" runat="server" class="form-control" Width="325px" Text='<%#Eval("vDESCRIPTION") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqvDESCRIPTION" runat="server" 
                                                    ControlToValidate="txtvDESCRIPTION"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Service Group">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupName" runat="server" 
                                                    Text='<%#  Eval("GroupName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtGroupName" runat="server" class="form-control" Width="205px" Text='<%#Eval("GroupName") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqGroupName" runat="server" InitialValue="0" 
                                                    ControlToValidate="txtGroupName"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>



                                 <asp:TemplateField HeaderText="Category">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" 
                                                    Text='<%#  Eval("vCATEGORY")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddCategory" runat="server" class="form-control"  Style="width: 215px;"></asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="ReqCategory" runat="server" InitialValue="0" 
                                                    ControlToValidate="ddCategory"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>


                                

                                
                                

                                <asp:TemplateField HeaderText="Item Type">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemType" runat="server" 
                                                    Text='<%#  Eval("ItemType")%>'></asp:Label>
                                            </ItemTemplate>
                                      
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddItemType" runat="server" class="form-control"  Style="width: 125px;">
                                                </asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="ReqITemType" runat="server" InitialValue="0" 
                                                    ControlToValidate="ddItemType"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="No of Session">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoSession" runat="server"
                                            Text='<%#  Eval("NoSession","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                         <EditItemTemplate>
                                                 <asp:TextBox ID="txtiNoSession" runat="server" class="form-control decimalnumbers-only" Width="125px" Text='<%#Eval("NoSession","{0:###0;(###0);0}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqQtyiNoSession" runat="server" 
                                                    ControlToValidate="txtiNoSession"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SRP">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblvUnitCostS" runat="server"
                                            Text='<%#  Eval("vUnitCost","{0:N2}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                                 <asp:TextBox ID="txtivUnitCost" runat="server" class="form-control decimalnumbers-only" Width="125px" Text='<%#Eval("vUnitCost","{0:N2}") %>' AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);" onkeydown="return (event.keyCode!=13);" autocomplete="off" BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqQtyivUnitCost" runat="server" 
                                                    ControlToValidate="txtivUnitCost"  Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                </asp:TemplateField>

                               <%--  <asp:BoundField HeaderText="Status" DataField="Status" ReadOnly="True" ItemStyle-Width="85px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>--%>
                            
                                 <asp:TemplateField HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" Width="125px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblvStatus" runat="server"
                                            Text='<%#  Eval("Status")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddStatus" runat="server" class="form-control"  Width="125px">
                                            <%--<asp:ListItem Value="1" Text="Active" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>--%>
                                        </asp:DropDownList>    
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

<!-- ################################################# START #################################################### -->
    <script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>
<!-- ################################################# START #################################################### -->


        <!-- ################################################# START #################################################### -->
 
    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "ITEM MASTER LIST",
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
