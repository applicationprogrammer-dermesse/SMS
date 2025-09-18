<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReprintZReading.aspx.cs" Inherits="SMS.ReprintZReading" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reprint Z Reading</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="crReprintZ" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
