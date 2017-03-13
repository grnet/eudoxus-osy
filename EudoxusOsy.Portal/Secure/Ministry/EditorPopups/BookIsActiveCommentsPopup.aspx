<%@ Page Title="Ενεργοποίηση / Απενεργοποίηση Βιβλίου" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.BookIsActiveCommentsPopup" CodeBehind="BookIsActiveCommentsPopup.aspx.cs" Language="C#" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <div>
        <table>
            <tr>
                <td>Σχόλια:
                </td>
                <td>
                    <dx:ASPxTextBox runat="server" ID="txtComments" />
                </td>
            </tr>
        </table>
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>
</asp:Content>
