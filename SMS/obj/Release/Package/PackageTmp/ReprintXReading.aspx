<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReprintXReading.aspx.cs" Inherits="SMS.ReprintXReading" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reprint X Reading</title>

    
<%--<script type="text/javascript">
    function Print() {
        var dvReport = document.getElementById("dvReport");
        var frame1 = dvReport.getElementsByTagName("iframe")[0];
        if (navigator.appName.indexOf("Internet Explorer") != -1) {
            frame1.name = frame1.id;
            window.frames[frame1.id].focus();
            window.frames[frame1.id].print();
        }
        else {
            var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
            frameDoc.print();
        }
    }
</script>--%>


</head>
<body>
    <form id="form1" runat="server">
        <input type="button" id="btnPrint" value="Print" onclick="Print()" />
        <script type="text/javascript">
            function Print() {
                var dvReport = document.getElementById("dvReport");
                var frame1 = dvReport.getElementsByTagName("iframe")[0];
                if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                    frame1.name = frame1.id;
                    window.frames[frame1.id].focus();
                    window.frames[frame1.id].print();
                }
                else {
                    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                    frameDoc.print();
                }
            }
    </script>

        <br />
        <div id="dvReport">
         <%--<CR:CrystalReportViewer ID="crReprintX" runat="server" AutoDataBind="true" />--%>
            <CR:CrystalReportViewer ID="crReprintX" runat="server" AutoDataBind="true"
                 ToolPanelView="None" EnableDatabaseLogonPrompt="false"  />
        </div>
        <br />
        <br />
        
        <br />
    </form>
</body>
</html>
