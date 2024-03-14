<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCandidates.aspx.cs" Inherits="AllCandidate.ListCandidates" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Candidates</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>List Candidates</h1>
            <asp:Button ID="btnCreateCandidate" runat="server" Text="Create Candidate" 
                  OnClick="btnCreateCandidate_Click" />
            <br />
                <br />
            <asp:GridView ID="gvCandidates" runat="server"   DataKeyNames="Sr_No" AutoGenerateColumns="False" OnRowEditing="gvCandidates_RowEditing" OnRowDeleting="gvCandidates_RowDeleting" >
                <Columns>
                    <asp:BoundField DataField="Sr_No" HeaderText="ID" />
                    <asp:BoundField DataField="Candidate_Name" HeaderText="Name" />
                    <asp:BoundField DataField="DOB" HeaderText="Date of Birth" />
                    <asp:BoundField DataField="Age" HeaderText="Age" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:ButtonField ButtonType="Button" Text="Edit" CommandName="Edit" />
                    <asp:ButtonField ButtonType="Button" Text="Delete" CommandName="Delete" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>