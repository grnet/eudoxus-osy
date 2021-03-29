<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ExportTransferPerIssuer.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ExportTransferPerIssuer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">   
    <div class="br"></div>
    <table>
        <tr>
            <th>Ημερομηνία αποστολής προς ΥΔΕ: </th>
            <td>
                <dx:ASPxDateEdit runat="server" ID="dateSentAt">
                    <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="vgExport" RequiredField-ErrorText="Το πεδίο 'Ημερομηνία αποστολής προς ΥΔΕ' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr runat="server" id="trSupplier">
            <th>ID εκδότη</th>
            <td>
                <dx:ASPxSpinEdit  runat="server" ID="txtSupplierKpsID" >
                    <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="vgExport" 
                        RequiredField-ErrorText="Το πεδίο 'ID εκδότη' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxSpinEdit>
            </td>
        </tr>        
    </table>

    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" ValidationGroup="vgFormErrors" OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />

    

    <script type="text/javascript">
        function doSubmit(s, e) {
            if (ASPxClientEdit.ValidateGroup("vgExport")) {
                btnSubmitHidden.DoClick();
            }
        }
    </script>

</asp:Content>