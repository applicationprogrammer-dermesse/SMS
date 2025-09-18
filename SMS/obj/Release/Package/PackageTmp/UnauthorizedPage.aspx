<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnauthorizedPage.aspx.cs" Inherits="SMS.UnauthorizedPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container-fluid" style="width: 98%">
        <div class="row" style="margin-bottom: 12px; margin-top: 100px;">
            <div class="row">
                <div class="col-md-12 text-center">
                    <div class="jumbotron">
                          <i class="glyphicon glyphicon-remove-sign" aria-hidden="true" style="font-size:96px;color:red"></i>
                          <h1 class="display-4">ACCESS DENIED!</h1>
                          <p class="lead">You are not authorized to view this page.</p>
                          <hr class="my-4" />
                          <%--<p>It uses utility classes for typography and spacing to space content out within the larger container.</p>
                          <a class="btn btn-primary btn-lg" href="#" role="button">Learn more</a>--%>
                     </div>
                </div>
            </div>
        </div>


    <%--<div class="row" style="margin-bottom: 12px;">
            <div class="row">
                <div class="col-md-12 text-center">
                    <h2></h2>
                </div>
            </div>
        </div>--%>



</div>
</asp:Content>

