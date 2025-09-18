<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SMS.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    
    table th {
        text-align:center;
        font-size:12px;
        height:50px;
        font-weight:normal;
        font-size:small;
        color:darkblue;
        /*background-color:#e2dfdf;*/
        
    }
     table tr {
        font-size:11px;
        
    }
     .hiddencol { display: none; }


     .row {
  display: flex; /* equal height of the children */
}

.col {
  flex: 1; /* additionally, equal width */
  padding: 1em;
}

#Q {
    border-radius: 25px;
    padding: 20px;
    /*background-color:gray;*/
    /*width: 400px;*/
    height: 150px;
}
#O {
    border-radius: 25px;
    padding: 20px;
    /*background-color:gray;*/
    /*width: 400px;*/
    height: 150px;
}

#S {
    border-radius: 25px;
    padding: 20px;
    /*background-color:gray;*/
    /*width: 400px;*/
    height: 150px;
}

#W {
    border-radius: 25px;
    padding: 20px;
    /*background-color:gray;*/
    /*width: 400px;*/
    height: 150px;
}

#R {
    border-radius: 25px;
    padding: 20px;
    /*background-color:gray;*/
    /*width: 400px;*/
    height: 150px;
}

#Z {
    border-radius: 25px;
    padding: 20px;
    /*background-color:gray;*/
    /*width: 400px;*/
    height: 150px;
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
<div class="container-fluid">
    <div class="row" style="margin-bottom:10px;">

        <!-- ./col -->
                       
                <!-- ./col -->

               <div id="Q" class="col-lg-4 col-xs-6" >
                        <a class="btn btn-lg btn-primary col-lg-10" style="height:115px; color:black; background-color:tan"  href="SalesForTheDay.aspx">
                            <i class=""></i> 
                            <asp:Label ID="lblSalesfortheday" runat="server" Text=""></asp:Label>
                            <br />
                            Transaction for the day
                        </a>

               </div>
                
          <div id="O" class="col-lg-4 col-xs-6">
           
                                       <a class="btn btn-lg btn-default col-lg-10" style="height:115px" href="EncodeSales.aspx">
                                            <i class="fa fa-cogs fa-2x pull-left fa-3x"></i> 
                                            
                                            
                                            Transaction Date
                                           <br />
                                           <br />
                                           <asp:Label ID="lblTransactionDate" runat="server" Text="0"></asp:Label>
                                        </a>
                        
                 </div>

                <div id="R" class="col-lg-4 col-xs-6">
                       <a class="btn btn-lg btn-success col-lg-10" style="height:115px" href="IssuanceForPosting.aspx">
                          <i class="fa fa-truck fa-2x pull-left fa-3x"" aria-hidden="true"></i>
                           <asp:Label ID="lblIssuance" runat="server" Text="0"></asp:Label>
                           <br />
                           Unposted
                           <br />
                           Issuance
                        </a>
                  </div>
      
             

     </div>

      <div class="row" style="margin-bottom:10px;">  
               <div class="col-sm-12">
                        
                       <div id="W" class="col-lg-4 col-xs-6">
                                   <a class="btn btn-lg btn-warning col-lg-10" style="height:115px; background-color:powderblue; color:black;" href="ComplimentaryListForPosting.aspx">
                                      <i class="fa fa-gift fa-2x pull-left fa-3x"" aria-hidden="true"></i>
                                       <asp:Label ID="lblComplimentaryno" runat="server" Text=""></asp:Label>
                                        <br />
                                      Complimentary <br />for HO Posting
                                    </a>
                              </div>

                        
                                
                        
                         
                                 <div id="S" class="col-lg-4 col-xs-6">
                                       <a class="btn btn-lg btn-danger col-lg-10" style="height:115px" href="PRFListForPosting.aspx">
                                          <i class="fa fa-exchange fa-2x pull-left fa-3x"" aria-hidden="true"></i>
                                           <asp:Label ID="lblPRFno" runat="server" Text="0"></asp:Label>
                                           <br />
                                           PRF/Stock Transfer for Posting
                                           <br />
                                        </a>
                                  </div>

                              <div id="Z" class="col-lg-4 col-xs-6">
                                   <a class="btn btn-lg btn-warning col-lg-10" style="height:115px; background-color:#ffc6b3; color:black;" href="AdjustmentListForPosting.aspx">
                                      <i class="fa fa-edit fa-2x pull-left fa-3x"" aria-hidden="true"></i>
                                       <asp:Label ID="lblAdjustment" runat="server" Text=""></asp:Label>
                                        <br />
                                      Adjustment <br />for HO Posting
                                    </a>
                              </div>

                        
          
               </div>     

    </div>
       

</div>


     <script type="text/javascript">
         function filter2(phrase, _id) {
             var words = phrase.value.toLowerCase().split(" ");
             var table = document.getElementById(_id);
             var ele;
             for (var r = 1; r < table.rows.length; r++) {
                 ele = table.rows[r].innerHTML.replace(/<[^>]+>/g, "");
                 var displayStyle = 'none';
                 for (var i = 0; i < words.length; i++) {
                     if (ele.toLowerCase().indexOf(words[i]) >= 0)
                         displayStyle = '';
                     else {
                         displayStyle = 'none';
                         break;
                     }
                 }
                 table.rows[r].style.display = displayStyle;
             }
         }
</script>

</asp:Content>
