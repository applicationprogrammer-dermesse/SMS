<%@ Page Title="Add Item to Discount" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddItemToDiscount.aspx.cs" Inherits="SMS.AddItemToDiscount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />
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

.ui-dialog { position: fixed; padding: .1em; width: 300px; overflow: hidden; }

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 20px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>Add Item to Discount</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>

    <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">

                <div class="col-sm-2 text-right">
                    Select Discount : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:DropDownList ID="ddDiscount" runat="server" class="form control chosen-select" Style="width: 495px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" AutoPostBack="True" OnSelectedIndexChanged="ddDiscount_SelectedIndexChanged" ></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                    ControlToValidate="ddDiscount" Display="Dynamic" ValidationGroup="grpDisc" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

      

    <div class="row" style="margin-bottom:15px;">
            <div class="col-sm-12">

                <div class="col-sm-2 text-right">
                    Select Item : 
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:DropDownList ID="ddITemFG" runat="server" class="form control chosen-select" Style="width: 495px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" InitialValue="0"
                                    ControlToValidate="ddITemFG" Display="Dynamic" ValidationGroup="grpDisc" ForeColor="Red"
                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin-bottom:15px;">
            <div class="col-sm-12">

                <div class="col-sm-2 text-right">
                    
                </div>
                <div class="col-sm-5">
                    <div class="input-group">
                        <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-info" OnClick="btnAdd_Click" ValidationGroup="grpDisc" />
                    </div>
                </div>
            </div>
        </div>


        <div class="row" style="margin-bottom:5px;">
            <div class="col-sm-12">
                <asp:GridView ID="gvDisc" runat="server" DataKeyNames="DiscountDetailID" AutoGenerateColumns="false" Width="100%" OnRowDeleting="gvDisc_RowDeleting">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                 
                        <asp:BoundField HeaderText="ID" DataField="DiscountDetailID" ItemStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="BrCode" DataField="BrCode" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Discount Code" DataField="sConstant" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Description" DataField="sDescription" ItemStyle-Width="50%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                
                        

                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="server" class="btn btn-success btn-sm" CommandName="Delete" Text="DELETE" ForeColor="white"  OnClientClick="return   CloseGridCustomer();" />
                                    </ItemTemplate>
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

</asp:Content>
