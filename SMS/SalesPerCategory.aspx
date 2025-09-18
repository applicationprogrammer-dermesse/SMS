<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesPerCategory.aspx.cs" Inherits="SMS.SalesPerCategory" %>
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


        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="container-fluid" style="width: 100%">


             <div class="row" style="margin-bottom: 5px; margin-top:15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Type
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddType" runat="server" class="form-control"  Style="width: 165px;">
                                    <asp:ListItem Text="Please Select" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Service" Value="Service"></asp:ListItem>
                                    <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" InitialValue="0"
                                    ControlToValidate="ddType" Display="Dynamic" ValidationGroup="grpItem" ForeColor="Red"
                                    ErrorMessage="Please select Type"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

         <div class="row" style="margin-bottom: 5px; margin-top:15px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Option
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="DDoPTION" runat="server" class="form-control"  Style="width: 165px;" AutoPostBack="true" OnSelectedIndexChanged="DDoPTION_SelectedIndexChanged">
                                    <asp:ListItem Text="Please Select" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="MTD" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="YTD" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                    ControlToValidate="DDoPTION" Display="Dynamic" ValidationGroup="grpItem" ForeColor="Red"
                                    ErrorMessage="Please select option"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>

           <div class="row" style="margin-bottom: 5px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Month
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddMonth" runat="server" class="form-control"  Style="width: 165px;" AutoPostBack="true" OnSelectedIndexChanged="ddMonth_SelectedIndexChanged">
                                    <asp:ListItem Text="Please Select" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" InitialValue="0"
                                    ControlToValidate="ddMonth" Display="Dynamic" ValidationGroup="grpItem" ForeColor="Red"
                                    ErrorMessage="Please select Month"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
         
         <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12">
                        <div class="col-sm-2 text-right">
                            Year
                        </div>
                        <div class="col-sm-9">
                            <div class="input-group">
                                <asp:DropDownList ID="ddYear" runat="server" class="form-control"  Style="width: 165px;" AutoPostBack="true" OnSelectedIndexChanged="ddYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
         <%--onserverclick="btnPrint_Click"--%>
         <div class="row" style="margin-bottom: 5px; margin-top:15px;">
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

            <div class="row" style="margin-bottom: 10px;">
                    <div class="col-sm-12 text-center">
                        
                         <asp:GridView ID="gvSalesPerSubCategory" runat="server" AutoGenerateColumns="false" OnDataBound="gvSalesPerSubCategory_DataBound" OnRowCreated="gvSalesPerSubCategory_RowCreated">
                                 <Columns>
                                        <%--<asp:BoundField DataField = "SessionGroup" HeaderText = "SessionGroup" ItemStyle-Width = "260" />--%>
                                          
                                      <%--<asp:BoundField HeaderText="Session Group" DataField="SessionGroup"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>--%>

                                      <asp:TemplateField HeaderText="Session Group">
                                    <HeaderStyle HorizontalAlign="Center" Width="265px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSessionGroup" runat="server"
                                            Text='<%#  Eval("SessionGroup")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                        <asp:BoundField DataField = "SessionType" HeaderText = "SessionType" ItemStyle-Width = "270" >
                                             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                         <asp:BoundField DataField = "QtyPM" HeaderText = "Quantity<br />(Previous Year(Month))" HtmlEncode="false" ItemStyle-Width = "130" DataFormatString = "{0:N0}" ItemStyle-HorizontalAlign = "Center" >
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                         <asp:BoundField DataField = "NetAmtPM" HeaderText = "Net Sales<br />(Previous Year(Month))" HtmlEncode="false" ItemStyle-Width = "145" DataFormatString = "{0:N2}" ItemStyle-HorizontalAlign = "Right">
                                             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            
                                        </asp:BoundField>

                                         <asp:BoundField DataField = "PMPercent" HeaderText = "%" HtmlEncode="false" ItemStyle-Width = "80" DataFormatString = "{0:N2}" ItemStyle-HorizontalAlign = "Center" >
                                             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                        
                                         <asp:BoundField DataField = "QtyCM" HeaderText = "Quantity<br />(Current Month)" HtmlEncode="false" ItemStyle-Width = "130" DataFormatString = "{0:N0}" ItemStyle-HorizontalAlign = "Center" >
                                             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                         
                                         <asp:BoundField DataField = "NetAmtCM" HeaderText = "Net Sales<br />(Current Month)" HtmlEncode="false" ItemStyle-Width = "145" DataFormatString = "{0:N2}" ItemStyle-HorizontalAlign = "Right" >
                                             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            
                                        </asp:BoundField>
                                         
                                     <asp:BoundField DataField = "CMPercent" HeaderText = "%" HtmlEncode="false" ItemStyle-Width = "80" DataFormatString = "{0:N2}" ItemStyle-HorizontalAlign = "Center" >
                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                    </Columns>
                            </asp:GridView>
                    </div>
            </div>

     </div>




    
<!-- ################################################# START #################################################### -->
 
    <script type="text/javascript">
        function ShowWarningMsg() {
            $(function () {
                $("#messageWarning").dialog({
                    title: "Sales per sub category",
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
