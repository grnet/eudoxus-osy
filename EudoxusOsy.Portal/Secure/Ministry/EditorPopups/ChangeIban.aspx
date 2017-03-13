<%@ Page Title="Αλλαγή IBAN" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ChangeIban" CodeBehind="ChangeIban.aspx.cs" Language="C#" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <table class="dv" style="width: 100%">
        <colgroup>
            <col style="width: 50px" />
        </colgroup>
        <tr>
            <th colspan="2" class="header">&raquo; Αλλαγή IBAN
            </th>
        </tr>
        <tr>
            <th>IBAN:
            </th>
            <td>
                <dx:ASPxTextBox ID="txtIBAN" runat="server">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'IBAN' είναι υποχρεωτικό" ValidationGroup="vgIBAN" />
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgIBAN" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgIBAN" OnClick="btnSubmit_Click" />

    <asp:PlaceHolder ID="phErrors" runat="server" Visible="false">
        <br />
        <dx:ASPxLabel ID="lblErrors" runat="server" ForeColor="Red" />
    </asp:PlaceHolder>

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
