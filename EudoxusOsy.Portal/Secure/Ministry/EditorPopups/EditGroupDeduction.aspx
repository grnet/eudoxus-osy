<%@ Page Title="Καθεστώς Φ.Π.Α. Κατάστασης" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="EditGroupDeduction.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.EditGroupDeduction" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <table class="dv" style="width: 600px">
        <colgroup>
            <col style="width: 220px" />
        </colgroup>
        <tr>
            <th class="header" colspan="2">&raquo; Στοιχεία Φ.Π.Α.
            </th>
        </tr>
        <tr>
            <th>Τρέχον Καθεστώς Φ.Π.Α.:
            </th>
            <td>
                <dx:ASPxLabel ID="lblCurrentDeductionType" runat="server" />
            </td>
        </tr>
        <tr id="trCurrentVat" runat="server" visible="false">
            <th>Φ.Π.Α. Κατάστασης:
            </th>
            <td>
                <dx:ASPxLabel ID="lblCurrentVat" runat="server" />
            </td>
        </tr>
        <tr>
            <th>Νέο Καθεστώς Φ.Π.Α.:
            </th>
            <td>
                <dx:ASPxComboBox ID="ddlNewDeductionType" runat="server" ValueType="System.Int32" OnInit="ddlNewDeductionType_Init" Width="250px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { cbpNewVat.PerformCallback(s.GetValue().toString()); }" />
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Νέο Καθεστώς Φ.Π.Α.' είναι υποχρεωτικό" ValidationGroup="vgGroupDeduction" />
                </dx:ASPxComboBox>
            </td>
        </tr>
    </table>

    <dx:ASPxCallbackPanel ID="cbpNewVat" runat="server" ClientInstanceName="cbpNewVat" OnCallback="cbpNewVat_Callback">
        <PanelCollection>
            <dx:PanelContent ID="pcNewVat" runat="server" Visible="false">
                <table class="dv" style="margin-top: 0; border-top: 0; width: 600px">
                    <colgroup>
                        <col style="width: 220px" />
                    </colgroup>
                    <tr>
                        <th style="margin-top: 0; border-top: 0;">Εισάγετε Φ.Π.Α. σε λογιστική μορφή:
                        </th>
                        <td style="margin-top: 0; border-top: 0;">
                            <dx:ASPxSpinEdit ID="txtNewVat" runat="server" DisplayFormatString="F" NumberType="Float" DecimalPlaces="2" Width="100px">
                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Νέο Φ.Π.Α. Κατάστασης' είναι υποχρεωτικό" ValidationGroup="vgGroupDeduction" />
                            </dx:ASPxSpinEdit>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgGroupDeduction" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgGroupDeduction" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
