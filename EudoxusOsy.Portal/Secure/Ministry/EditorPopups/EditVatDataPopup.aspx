<%@ Page Title="Αλλαγή Σταθερών Φ.Π.Α." Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="EditVatDataPopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.EditVatDataPopup" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <table class="dv">
        <colgroup>
            <col style="width: 350px" />
        </colgroup>
        <tr>
            <th class="header" colspan="2">&raquo; Σταθερές Φ.Π.Α. (%)
            </th>
        </tr>
        <tr>
            <th>Κανονικός:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtVatHigh" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Κανονικός ΦΠΑ' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Μειωμένος:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtVatMedium" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Μειωμένος ΦΠΑ' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Υπερμειωμένος:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtVatSmall" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Υπερμειωμένος ΦΠΑ' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Κανονικός νήσων πλην Κρήτης:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtVatHighLowered" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Κανονικός νήσων πλην Κρήτης' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Μειωμένος νήσων πλην Κρήτης:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtVatMediumLowered" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Κανονικός νήσων πλην Κρήτης' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Υπερμειωμένος νήσων πλην Κρήτης:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtVatSmallLowered" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Υπερμειωμένος νήσων πλην Κρήτης' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Κρατήσεις υπέρ ΟΓΑ:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtOGA" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Κρατήσεις υπέρ ΟΓΑ' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Κρατήσεις υπέρ Μετοχικού Ταμείου Πολιτικών Υπαλλήλων:
            </th>
            <td>
                <dx:ASPxSpinEdit ID="txtMTPY" runat="server" DisplayFormatString="F" NumberType="Float" MinValue="0" MaxValue="100" DecimalPlaces="2" Width="50px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Κρατήσεις υπέρ Μετοχικού Ταμείου Πολιτικών Υπαλλήλων' είναι υποχρεωτικό" ValidationGroup="vgVatConstants" />
                </dx:ASPxSpinEdit>
            </td>
        </tr>
    </table>

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgVatConstants" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgVatConstants" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
