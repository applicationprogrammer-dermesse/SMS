<%@ Page Language="C#" Title="Print Receipt" AutoEventWireup="true" CodeBehind="OfficialReceipt.aspx.cs" Inherits="SMS.OfficialReceipt" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Receipt</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="crReceipt" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
