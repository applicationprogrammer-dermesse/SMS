<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCompliTransaction.aspx.cs" Inherits="SMS.ViewCompliTransaction" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Complimentary Transaction Detail</title>
    <link href="Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="jquery-ui.css" rel="stylesheet" />

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

</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
        <br />
        <br />
        <div class="col-sm-12 text-center">
            Transaction History
        </div>
        <br />
        <br />
         <div class="col-sm-12 text-center">
                                 <asp:GridView ID="gvPrint" runat="server" Width="100%" class="table table-bordered table-hover"  AutoGenerateColumns="false" ShowFooter="True" >
                                     <Columns>
                                         
                                         <asp:TemplateField HeaderText="No"  ItemStyle-Width="5%">   
                                             <ItemTemplate>
                                                     <%# Container.DataItemIndex + 1 %>   
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                         <asp:BoundField DataField="Branch" HeaderText="Branch" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         <asp:BoundField DataField="CompliName" HeaderText="Primary Customer" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         <asp:BoundField DataField="Complimentaryno" HeaderText="No" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         
                                         <asp:TemplateField HeaderText="Date">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                       <asp:Label ID="lblDate" runat="server" Text='<%#  Convert.ToString(Eval("Complimentarydate", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("Complimentarydate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                             </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:BoundField DataField="CompliCustomerName" HeaderText="Customer Name" HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         <asp:BoundField DataField="vPluCode" HeaderText="Plu Code" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>
                                         
                                         <asp:BoundField DataField="vDESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ReadOnly="true">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         

                                         <asp:BoundField DataField="SRP" HeaderText="SRP" DataFormatString="{0:N2}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ReadOnly="true">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                         </asp:BoundField>


                                         <asp:BoundField DataField="vQty" HeaderText="Qty" HtmlEncode="false" ItemStyle-Width="5%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>


                                         <asp:BoundField DataField="CompliAmount" HeaderText="Total" DataFormatString="{0:N2}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ReadOnly="true">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                         </asp:BoundField>

                                         

                                     </Columns>
                                 </asp:GridView>
                             </div>

            </div>

         <!-- jQuery CDN -->
         <%--<script src="jQuery/jquery-1.12.0.min.js"></script>--%>
        
        <script src="jquery-ui.min.js"></script>
        <script src="external/jquery/jquery.js"></script>
         <!-- Bootstrap Js CDN -->
         <script src="Bootstrap/bootstrap.min.js"></script>


    </form>
</body>
</html>
