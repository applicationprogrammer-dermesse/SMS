<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemMasterList.aspx.cs" Inherits="SMS.ItemMasterList" %>
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
                                &nbsp;&nbsp;&nbsp;
                                <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">EXPORT&nbsp;&nbsp;&nbsp;<i class="fa fa-file-excel-o"></i></button>
                
                            </div>
                        </div>
                    </div>
                </div>


     <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvItem" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode" OnRowDataBound="gvItem_RowDataBound" >
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
                                
                                 <asp:BoundField HeaderText="Group" DataField="GroupName"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Session Group" DataField="SessionGroup"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                 <asp:BoundField HeaderText="Session Type" DataField="SessionType"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
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

                                 <asp:BoundField HeaderText="Status" DataField="Status" ReadOnly="True" ItemStyle-Width="115px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                
                                    
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

</asp:Content>
