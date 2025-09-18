<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupDiscount.aspx.cs" Inherits="SMS.SetupDiscount" %>

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
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Setup Discount</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>


        <br />
        <br />
        <div class="row" style="margin-bottom:15px;">
            <div class="col-sm-12">

                <div class="col-sm-11 text-right">
                    
                </div>
                <div class="col-sm-1 text-right">
                    <div class="input-group">
                        <asp:Button ID="btnPost" runat="server" Text=" P O S T" CssClass="btn btn-sm btn-success" ValidationGroup="grpDiscount" OnClick="btnPost_Click"  />

                    </div>
                </div>
            </div>
        </div>

        <div class="form-group col-md-12">
     <div class="col-sm-8">


        <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    Discount Code : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:TextBox ID="txtDiscountCode" runat="server" MaxLength="8" style="text-transform:uppercase;" Width="115px" CssClass="form-control"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtDiscountCode" Display="Dynamic" ValidationGroup="grpDiscount" ForeColor="Red"
                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    Description : 
                </div>
                <div class="col-sm-9">
                    <div class="input-group">
                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="62" Width="350px" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        ControlToValidate="txtDescription" Display="Dynamic" ValidationGroup="grpDiscount" ForeColor="Red"
                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    Percentage : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:TextBox ID="txtPercentage" runat="server" MaxLength="2" Width="55px" CssClass="form-control decimalnumbers-only"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

       <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    Discount Amount : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:TextBox ID="txtDiscountAmount" runat="server" Text="0"  Width="105px" CssClass="form-control decimalnumbers-only"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
                         
       <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    VAT Exempted : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:RadioButton ID="rbVatNo" Checked="true" runat="server" GroupName="grpVAT" Text="&nbsp;&nbsp;NO" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbVatYes" runat="server" GroupName="grpVAT" Text="&nbsp;&nbsp;YES" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    Discount Type : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:CheckBox ID="ckIsPromo" runat="server" Checked="true" Text="Uncheck if not promo" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
              
                </div>
                <div class="col-sm-5">
                    Covered Date
                </div>
            </div>
        </div>
        

        <div class="row" style="margin-bottom: 5px;">
            <div class="col-sm-12">
                <div class="col-sm-3 text-right">
                     From :
                </div>
                <div class="col-sm-8">
                    <div class="input-group">
                    <asp:TextBox ID="txtDateFrom" runat="server" class="form-control dateCurr" Style="width: 110px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtDateFrom" Display="Dynamic" ValidationGroup="grpDiscount" ForeColor="Red"
                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateFrom"
                        ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                        ForeColor="Red" Display="Dynamic"
                        ErrorMessage="Invalid date format"
                        ValidationGroup="grpDiscount"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-12">
                <div class="col-sm-3 text-right">
                    To :
                </div>
                <div class="col-sm-5">
                    <div class="input-group">

                        <asp:TextBox ID="txtDate" runat="server" class="form-control dateCurr" Style="width: 110px;" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="grpDiscount" ForeColor="Red"
                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDate"
                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                            ForeColor="Red" Display="Dynamic"
                            ErrorMessage="Invalid date format"
                            ValidationGroup="grpDiscount"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Style="color: Red;"
                            ValidationGroup="grpDiscount" Display="Dynamic" ControlToCompare="txtDateFrom"
                            ControlToValidate="txtDate" Type="Date" Operator="GreaterThanEqual"
                            ErrorMessage="Invalid date range"></asp:CompareValidator>

                    </div>
                </div>
            </div>
        </div>
       
        <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    All Items : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:RadioButton ID="rbItemsNo" Checked="true" runat="server" GroupName="grpItems" Text="&nbsp;&nbsp;NO" AutoPostBack="true" OnCheckedChanged="rbItemsNo_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbItemsYes" runat="server" GroupName="grpItems" Text="&nbsp;&nbsp;YES"  AutoPostBack="true" OnCheckedChanged="rbItemsYes_CheckedChanged"/>
                    </div>
                </div>
            </div>
        </div>

      <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
                    Select Item : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Style="width: 495px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

      <div class="row">
            <div class="col-sm-12">

                <div class="col-sm-3 text-right">
      
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:Button ID="btnAddItem" runat="server" Text="ADD ITEM" CssClass="btn btn-sm btn-warning" OnClick="btnAddItem_Click"  />
                    </div>
                </div>
            </div>
        </div>

                        <div class="row">
                                    <div class="col-sm-12">
                                        <asp:GridView ID="grdItem" runat="server" Width="90%" Font-Size="Medium" class="table table-striped table-bordered table-hover" 
                                            AutoGenerateColumns="false" DataKeyNames="vFGCode" OnRowDeleting="grdItem_RowDeleting" >
                                            <Columns>
                        
                                                <asp:BoundField DataField="vFGCode" HeaderText="Item Code" ReadOnly="True">
                                                    <HeaderStyle Width="65px" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>


                                                <asp:BoundField HeaderText="Item Description" DataField="vDESCRIPTION" ReadOnly="True" ItemStyle-Width="305px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>

                                                <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-danger" ItemStyle-Width="40px" />

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

         
      </div>
        <%--end--%>
        
                    <div class="col-sm-4">
                        <%--<div class="panel panel-default ">
                            <div class="panel-heading">List of Branches</div>
                            <div class="panel-body">--%>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:GridView ID="gvBranches" runat="server" Width="99%" Font-Size="Medium" class="table table-striped table-bordered table-hover" 
                                            AutoGenerateColumns="false" DataKeyNames="BrCode" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select Branch">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ckStat" runat="server" Checked="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="BrCode" HeaderText="Branch Code" ReadOnly="True">
                                                    <HeaderStyle Width="65px" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>


                                                <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="145px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            <%--</div>
                        </div>--%>
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


    <!-- ################################################# START #################################################### -->

    <script type="text/javascript">
        $(function () {
            $(".dateCurr").datepicker();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".dateCurr").datepicker();
                }
            });
        };
    </script>
    <!-- ################################################# START #################################################### -->

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Sales Detailed per Branch",
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
