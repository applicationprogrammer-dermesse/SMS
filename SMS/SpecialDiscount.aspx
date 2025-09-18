<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SpecialDiscount.aspx.cs" Inherits="SMS.SpecialDiscount" %>
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


        .ui-dialog {
            position: fixed;
            padding: .1em;
            width: 300px;
            overflow: hidden;
        }
          .thick { border: 3px solid black; }

          .HeaderFreez
            {
            position:relative ;
            top:expression(this.offsetParent.scrollTop);
            z-index: 10;
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
                    <h4>Special Discount</h4>
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
                            All Item per Branch :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddBranch" runat="server" Width="245px" CssClass="form-control"></asp:DropDownList>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0" 
                                            ControlToValidate="ddBranch"  Display="Dynamic" ValidationGroup="grpSD" ForeColor="Red"
                                            ErrorMessage="Select branch"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Special Discount :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddSpecialDiscount" runat="server" Width="295px" CssClass="form-control"></asp:DropDownList>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" InitialValue="0" 
                                            ControlToValidate="ddSpecialDiscount"  Display="Dynamic" ValidationGroup="grpSD" ForeColor="Red"
                                            ErrorMessage="Select Special Discount"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                </div>
                <div class="col-sm-9">
                    Covered Date
                </div>
                <div class="col-sm-1 text-right">
                </div>

            </div>
        </div>

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    FROM 
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtDateFrom" runat="server" class="dateAll form-control" Style="width: 110px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="grpSD" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateFrom"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpSD"></asp:RegularExpressionValidator>

                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                    TO
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtDateTo" runat="server" class="dateAll form-control" Style="width: 110px; margin-bottom: 5px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtDateTo" Display="Dynamic" ValidationGroup="grpSD" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateTo"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpSD"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server"  Style="color: Red;"
                                            ValidationGroup="grpSD" Display="Dynamic" ControlToCompare="txtDateFrom"
                                            ControlToValidate="txtDateTo" Type="Date" Operator="GreaterThanEqual"
                                            ErrorMessage="Invalid date range"></asp:CompareValidator>

                    </div>
                </div>

            </div>
       </div>  
      
        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-right">
                </div>
                <div class="col-sm-10">
                    <asp:Button ID="btnActibate" runat="server" Text="Activate" CssClass="btn btn-sm btn-primary"  ValidationGroup="grpSD" OnClick="btnActibate_Click"/>
                </div>
            </div>
        </div>
    
     <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-2 text-left">
                    Active Special  Discount
                </div>
                <div class="col-sm-10">
        
                </div>
        </div>
      </div>

        <div class="row" style="margin-bottom: 15px;">
            <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                    <asp:GridView ID="gvDiscount" Width="98%" runat="server" DataKeyNames="DiscID" AutoGenerateColumns="false" OnRowDeleting="gvDiscount_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="DiscID" HeaderText="DiscID" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="BrName" HeaderText="Branch Name" ReadOnly="true">
                                <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="sConstant" HeaderText="Discount Code" ReadOnly="true">
                                <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="sDescription" HeaderText="Discount Description" ReadOnly="true">
                                <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="iPercent" HeaderText="Percent" ReadOnly="true">
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="iValidFrom_dt" HeaderText="Date From" ReadOnly="true">
                                <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="iValidUntil_dt" HeaderText="Date To" ReadOnly="true">
                                <HeaderStyle Width="7%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="dtLastUpdate_dt" HeaderText="Date Update" ReadOnly="true">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="updateBy" HeaderText="Update By" ReadOnly="true">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:CommandField ShowDeleteButton="True" ItemStyle-Width="5%" HeaderStyle-Width="5%" ButtonType="Button" DeleteText="De-Activate" ControlStyle-CssClass="btn btn-sm btn-danger" />

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
                    title: "Special Discount",
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

</asp:Content>
