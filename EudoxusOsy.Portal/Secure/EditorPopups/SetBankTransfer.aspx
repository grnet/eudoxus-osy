<%@ Page Title="Εκχώρηση σε Τράπεζα" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.SetBankTransfer" CodeBehind="SetBankTransfer.aspx.cs" Language="C#" AutoEventWireup="true" %>

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
                <dx:ASPxComboBox ID="ddlBank" runat="server" ValueType="System.Int32" OnInit="ddlBank_Init">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Τράπεζα' είναι υποχρεωτικό." ValidationGroup="vgBankTransfer" />
                </dx:ASPxComboBox>
            </td>
        </tr>
    </table>

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgBankTransfer" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgBankTransfer" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
