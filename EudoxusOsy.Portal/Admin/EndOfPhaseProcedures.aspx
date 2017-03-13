<%@ Page Language="C#" AutoEventWireup="true" Inherits="EudoxusOsy.Portal.Admin.EndOfPhaseProcedures"
    MasterPageFile="~/Admin/Admin.Master" Title="Διαδικασίες λήξης εξαμήνου"
    Codebehind="EndOfPhaseProcedures.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Admin</h1>        
    <div style="text-align:left">
        <table class="dv" border="1">
            <colgroup>
                <col width="400px" />
                <col width="100px" />
            </colgroup>
            <tr>
                <th>
                    Εισαγωγή Διορθωμένων αρχείων
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnInsertCorrectedFiles" OnClick="btnInsertCorrectedFiles_Click" ClientInstanceName="btnInsertCorrectedFiles" 
                        Text="Εισαγωγή"></dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <th>
                    Επεξεργασία διορθωμένων αρχείων και ενημερωση πίνακα Receipt
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnComplementReceipts" OnClick="btnComplementReceipts_Click" ClientInstanceName="btnComplementReceipts" 
                        Text="Επεξεργασία"></dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <th>
                    Αναφορά Σύγκρισης AuditReceiptXML - Receipt
                </th>
                <td>
                    <dx:ASPxButton runat="server" ID="btnCompareReceipts" OnClick="btnCompareReceipts_Click" ClientInstanceName="btnCompareReceipts" 
                        Text="Σύγκριση"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
