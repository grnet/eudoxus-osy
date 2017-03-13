<%@ Page Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.Secure.Ministry.BookKpsServices"
    MasterPageFile="~/Secure/Ministry/Ministry.Master" Title="Services Βιβλίων ΚΠΣ"
    CodeBehind="BookKpsServices.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSecureMain" runat="server">
    <h1>Admin</h1>
    <div style="text-align: left">
        <table class="dv" border="1">
            <colgroup>
                <col width="400px" />
                <col width="100px" />
            </colgroup>
            <tr>
                <th>Λήψη μεταβολών βιβλίων
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnGetModifiedBooks" OnClick="btnGetModifiedBooks_Click" ClientInstanceName="btnGetModifiedBooks"
                        Text="Λήψη μεταβολών βιβλίων">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <th>Λήψη νέων βιβλίων
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnGetNewBooks" OnClick="btnGetNewBooks_Click" ClientInstanceName="btnGetNewBooks"
                        Text="Λήψη νέων βιβλίων">
                    </dx:ASPxButton>
                </td>
            </tr>
<%--            <tr>
                <th>Αρχικοποίηση νέων βιβλίων - ΠΡΟΣΟΧΗ !!! θέτει όλα τα βιβλία ως συγχρονισμένα
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnInitKpsNewBooks" OnClick="btnInitKpsNewBooks_Click" ClientInstanceName="btnGetNewBooks"
                        Text="Αρχικοποίηση νέων βιβλίων">
                    </dx:ASPxButton>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <dx:ASPxTextBox runat="server" ID="txtUpdateBookStatusID"></dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxButton runat="server" ID="btnUpdateBookStatus" OnClick="btnUpdateBookStatus_Click" ClientInstanceName="btnUpdateBookStatus"
                        Text="Ενημέρωση κατάστασης βιβλίου ΚΠΣ">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel runat="server" ID="upResults" UpdateMode="Always">
            <ContentTemplate>   
                <asp:TextBox Width="500px" runat="server" TextMode="MultiLine" ID="txtResults" Rows="5">
                </asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
