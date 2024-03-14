<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCandidate.aspx.cs" Inherits="AllCandidate.CreateCandidate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Candidate</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Create Candidate</h1>
            <div>
                Name: <asp:TextBox ID="txtName" runat="server"></asp:TextBox><br />
                Date of Birth: <asp:TextBox ID="txtDOB" AutoPostBack="true"  OnTextChanged="TbDobTextChanged"  type="Date" runat="server"></asp:TextBox><br />
                Age: <asp:TextBox ID="txtAge" Enabled="false" runat="server"></asp:TextBox><br />
                City: <asp:DropDownList ID="ddlCity" runat="server"></asp:DropDownList><br />
                <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" />
            </div>
        </div>
    </form>
</body>
</html>