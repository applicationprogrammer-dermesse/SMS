<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InventoryReports.aspx.cs" Inherits="SMS.InventoryReports" %>
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
        <div class="row" style="margin-bottom: 25px;">
            <div class="row">
                <div class="col-md-4 text-center">
                </div>
                <div class="col-sm-3 text-center" style="background-color: #f2f2f2; color: maroon; border-radius: 15px;">
                    <h4>INVENTORY REPORT</h4>
                </div>
                <div class="col-md-4 text-center">
                </div>
            </div>
        </div>
      <%--  <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            All Item per Branch :
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <asp:DropDownList ID="ddBranch" runat="server" Width="245px" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged"></asp:DropDownList>
                                <%--           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" InitialValue="0" 
                                            ControlToValidate="ddBranch"  Display="Dynamic" ValidationGroup="grpIss" ForeColor="Red"
                                            ErrorMessage="Select branch"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                                    
                                   <asp:Label ID="lblRemarks" runat="server" Text="Per Item Per Branch"></asp:Label>
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddITemFG" runat="server" class="chosen-select" Style="width: 695px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" OnSelectedIndexChanged="ddITemFG_SelectedIndexChanged" AutoPostBack="True"  ></asp:DropDownList>
                                </div>
                         
                    </div>
                </div>

             <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                      
                                </div>
                                <div class="col-sm-9">
                                        <asp:RadioButton ID="rbSummary" runat="server" Checked="true" GroupName="rpt" Text="&nbsp;&nbsp;&nbsp;Summary" AutoPostBack="True" OnCheckedChanged="rbSummary_CheckedChanged" />
                                </div>
                         
                    </div>
                </div>

              <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                      
                                </div>
                                <div class="col-sm-9">
                                    <asp:RadioButton ID="rbDetailed" runat="server"  GroupName="rpt" Text="&nbsp;&nbsp;&nbsp;Detailed(ALL)" AutoPostBack="True" OnCheckedChanged="rbDetailed_CheckedChanged" />
                                 </div>
                         
                    </div>
                </div>

          <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                      
                                </div>
                                <div class="col-sm-9">
                                    <asp:RadioButton ID="rbMoreThanOne" runat="server"  GroupName="rpt" Text="&nbsp;&nbsp;&nbsp;Detailed(More than 1 year expiration)" AutoPostBack="True"  />
                                 </div>
                         
                    </div>
                </div>

          <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                      
                                </div>
                                <div class="col-sm-9">
                                    <asp:RadioButton ID="rbLessThanOne" runat="server"  GroupName="rpt" Text="&nbsp;&nbsp;&nbsp;Detailed(Less than 1 year expiration)" AutoPostBack="True"  />
                                 </div>
                         
                    </div>
                </div>

          <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                      
                                </div>
                                <div class="col-sm-9">
                                    <asp:RadioButton ID="rbLessThanSix" runat="server"  GroupName="rpt" Text="&nbsp;&nbsp;&nbsp;Detailed(Less than 6 months expiration)" AutoPostBack="True"  />
                                    
                                 </div>
                         
                    </div>
                </div>

          <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                               <div class="col-sm-2 text-right">
                      
                                </div>
                                <div class="col-sm-9">
                                        <asp:RadioButton ID="rbExpired" runat="server"  GroupName="rpt" Text="&nbsp;&nbsp;&nbsp;Detailed(Expired)" AutoPostBack="True"  />
                                 </div>
                         
                    </div>
                </div>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                        </div>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <button id="btnGenerate" onserverclick="btnGenerate_Click" type="submit" runat="server" class="btn btn-primary">GENERATE</button>
                                &nbsp;&nbsp;&nbsp;
                                <button id="btnPrint" onserverclick="btnPrint_Click" type="submit" runat="server" class="btn btn-success">E X P O R T</button>
                            </div>
                        </div>
                    </div>
                </div>

                <br />
                 <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                    </div>
                 </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:GridView ID="gvSummary" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode" OnRowDataBound="gvSummary_RowDataBound">
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

                                <asp:BoundField HeaderText="Category" DataField="vCATEGORY" ReadOnly="True" ItemStyle-Width="185px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:TemplateField HeaderText="Beg Bal">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyBegBalS" runat="server"
                                            Text='<%#  Eval("BegBal","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty <br />Received">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyReceivedS" runat="server"
                                            Text='<%#  Eval("QtyReceived","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyPRF" runat="server"
                                            Text='<%#  Eval("vQtyPRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unposted PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedPRF" runat="server"
                                            Text='<%#  Eval("UnpostedPRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Sales for the Day">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedQtyS" runat="server" 
                                            Text='<%#  Eval("UnpostedQty","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Sales">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtySales" runat="server"
                                            Text='<%#  Eval("Sales","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Free">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyFree" runat="server"
                                            Text='<%#  Eval("Free","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unposted Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedAdjustment" runat="server"
                                            Text='<%#  Eval("UnpostedAdjustment","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyAdjustmentS" runat="server"
                                            Text='<%#  Eval("Adjustment","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Unposted Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedComplimentary" runat="server"
                                            Text='<%#  Eval("UnpostedComplimentary","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyComplimentaryS" runat="server"
                                            Text='<%#  Eval("Complimentary","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Available Balance">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableBalanceS" runat="server"
                                            Text='<%#  Eval("AvailableBalance","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="col-sm-12">
                        <asp:GridView ID="gvSummAllBranchPerItem" Width="99%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover" DataKeyNames="vFGCode">
                            <Columns>
                                <asp:BoundField HeaderText="Branch" DataField="BrName" ReadOnly="True" ItemStyle-Width="195px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

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

                                <asp:BoundField HeaderText="Category" DataField="vCATEGORY" ReadOnly="True" ItemStyle-Width="205px" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:TemplateField HeaderText="Beg Bal">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyBegBalS" runat="server"
                                            Text='<%#  Eval("BegBal","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty <br />Received">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyReceivedS" runat="server"
                                            Text='<%#  Eval("QtyReceived","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyPRF" runat="server"
                                            Text='<%#  Eval("vQtyPRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unposted PRF">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedPRF" runat="server"
                                            Text='<%#  Eval("UnpostedPRF","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Sales for the Day">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedQtyS" runat="server" 
                                            Text='<%#  Eval("UnpostedQty","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Sales">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtySales" runat="server"
                                            Text='<%#  Eval("Sales","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Free">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyFree" runat="server"
                                            Text='<%#  Eval("Free","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unposted Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedAdjustment" runat="server"
                                            Text='<%#  Eval("UnpostedAdjustment","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Adjustment">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyAdjustmentS" runat="server"
                                            Text='<%#  Eval("Adjustment","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Unposted Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnpostedComplimentary" runat="server"
                                            Text='<%#  Eval("UnpostedComplimentary","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Complimentary">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyComplimentaryS" runat="server"
                                            Text='<%#  Eval("Complimentary","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Available Balance">
                                    <HeaderStyle HorizontalAlign="Center" Width="85px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableBalanceS" runat="server"
                                            Text='<%#  Eval("AvailableBalance","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="col-sm-12">
                         <asp:GridView ID="gvDetailed" Width="100%" runat="server" AutoGenerateColumns="false" class="table table-striped table-bordered table-hover">
                            <Columns>
                                <asp:BoundField HeaderText="Branch Name" DataField="BrCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="PLU Code" DataField="vPluCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                                <asp:BoundField HeaderText="Itemcode" DataField="vFGCode" ReadOnly="True" ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>


                                <asp:BoundField HeaderText="Description" DataField="vDESCRIPTION" ReadOnly="True" ItemStyle-Width="13%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                
                        
                                <%--<asp:TemplateField HeaderText="Date Received">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                               <asp:Label ID="lblvRecDate" runat="server" Text='<%#  Convert.ToString(Eval("vRecDate", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vRecDate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                     </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Batch No">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchNo" runat="server"
                                            Text='<%#  Eval("vBatchNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Date Expiry">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                               <asp:Label ID="lblvDateExpiry" runat="server" Text='<%#  Convert.ToString(Eval("vDateExpiry", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vDateExpiry", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                     </ItemTemplate>
                                </asp:TemplateField>


                              

                              
                              
                            
                                <asp:TemplateField HeaderText="Available <br />Balance">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalance" runat="server"
                                            Text='<%#  Eval("Available Balance","{0:###0;(###0);0}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                              

                               <asp:TemplateField HeaderText="Remarks">
                                    <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server"
                                            Text='<%#  Eval("Remarks")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                               

                            </Columns>
                        </asp:GridView>
                    </div>


                </div>
            
    </div>


    <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>
    


    <script>
        $(".readonly2").on('keydown paste', function (e) {
            e.preventDefault();
        });
    </script>

    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Inventory Report",
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
