<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCity.aspx.cs" Inherits="AllCandidate.CreateCity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create City</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Create City</h1>
            <div>
                Name: <asp:TextBox ID="txtCityName" runat="server"></asp:TextBox><br />
                <asp:Button ID="btnCreateCity" runat="server" Text="Create City" OnClick="btnCreateCity_Click" />
            </div>
        </div>
    </form>
</body>
</html>