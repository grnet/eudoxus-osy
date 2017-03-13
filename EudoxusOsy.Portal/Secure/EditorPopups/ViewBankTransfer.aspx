<%@ Page Title="Εκχώρηση σε Τράπεζα" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.ViewBankTransfer" CodeBehind="ViewBankTransfer.aspx.cs" Language="C#" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <table class="dv" style="width: 100%">
        <colgroup>
            <col style="width: 50px" />
        </colgroup>
        <tr>
            <th class="header" colspan="2">&raquo; Στοιχεία Εκχώρησης
            </th>
        </tr>
        <tr>
            <th>Τράπεζα:
            </th>
            <td>
                <dx:ASPxLabel ID="lblBank" runat="server" />
            </td>
        </tr>
    </table>
    
</asp:Content>
