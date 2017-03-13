<%@ Page Title="Ταμείο" Language="C#" MasterPageFile="~/Secure/Suppliers/Suppliers.Master" AutoEventWireup="true" CodeBehind="EditFinancialData.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Suppliers.EditFinancialData" %>

<%@ Register TagName="TipIcon" TagPrefix="my" Src="~/UserControls/GenericControls/TipIcon.ascx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphSecureMain" runat="server">
    <script language="javascript" type="text/jscript">
        $(function () {
            var otherCountryValue = <%= OtherCountryOffice.ID %>;
            var selectedValue = txtOtherOfficeDescription.GetValue();
            var tdOtherOfficeDescriptionID = '<%= tdOtherOfficeDescription.ClientID %>';


            if(selectedValue != otherCountryValue)
            {
                txtOtherOfficeDescription.SetVisible(false);
                $('#'+tdOtherOfficeDescriptionID).hide();
            }
        });
        
        function callbackEnd(s, e) {
            Imis.Lib.notify('Τα στοιχεία αποθηκεύτηκαν επιτυχώς.');
        }

        function doClick(s,e)
        {
            cbUpdateFinancialData.PerformCallback(cmbSelectOffice.GetValue() + ':' + txtInsertIBAN.GetValue());
        }

        function OfficeSelected(s, e) {
            var selectedValue = s.GetValue();
            var otherCountryValue = <%= OtherCountryOffice.ID %>;
            var tdOtherOfficeDescriptionID = '<%= tdOtherOfficeDescription.ClientID %>';

            if(selectedValue == otherCountryValue)
            {
                txtOtherOfficeDescription.SetVisible(true);
                $('#'+tdOtherOfficeDescriptionID).show();
            }
            else
            {
                txtOtherOfficeDescription.SetVisible(false);
                txtOtherOfficeDescription.SetValue(null);
                $('#'+tdOtherOfficeDescriptionID).hide();
            }
        }
    </script>
    <div>
        <table class="dv">
            <tr>
                <th>Δ.Ο.Υ. Πληρωμών:</th>
                <td>
                    <dx:ASPxComboBox runat="server" ID="cmbSelectOffice" ClientInstanceName="cmbSelectOffice" TextField="Name" ValueField="ID" ValueType="System.Int32">
                        <ClientSideEvents ValueChanged="OfficeSelected" />
                        <Columns>
                            <dx:ListBoxColumn FieldName="ID" Visible="false" />
                            <dx:ListBoxColumn FieldName="Name" Visible="true" />
                        </Columns>
                    </dx:ASPxComboBox>
                </td>
                <td id="tdOtherOfficeDescription" runat="server">
                    <dx:ASPxTextBox runat="server" Width="300px" ID="txtOtherOfficeDescription" ClientVisible="false" ClientInstanceName="txtOtherOfficeDescription">
                    </dx:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <th>Τραπεζικός Λογαριασμός (σε μορφή IBAN):
                    <my:TipIcon runat="server" Text="Κατά την έκδοση των τίτλων πληρωμής είναι πλέον υποχρεωτική (ΠΟΛ.1116/23-5-2013 απόφαση Υφυπουργού Οικονομικών) η αναγραφή του Τραπεζικού Λογαριασμού του δικαιούχου (σε μορφή IBAN). Συνεπώς, η καταχώριση του Τραπεζικού Λογαριασμού (σε μορφή IBAN) είναι απαραίτητη προϋπόθεση για την έκδοση χρηματικού εντάλματος και την πληρωμή των οφειλών του Υπουργείου προς τους δικαιούχους." />
                </th>
                <td>
                    <dx:ASPxTextBox runat="server" ID="txtInsertIBAN" ClientInstanceName="txtInsertIBAN" Width="400px"></dx:ASPxTextBox>
                </td>
            </tr>
        </table>
    </div>

    <div class="br"></div>
    <div class="br"></div>

    <dx:ASPxLabel ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" />

    <dx:ASPxButton runat="server" ID="btnSubmit" Text="Αποθήκευση" ClientInstanceName="btnSubmit" ClientVisible="true" Image-Url="~/_img/iconSave.png">
        <ClientSideEvents Click="doClick" />
    </dx:ASPxButton>

    <dx:ASPxCallback runat="server" ID="cbUpdateFinancialData" ClientInstanceName="cbUpdateFinancialData" OnCallback="cbUpdateFinancialData_Callback">
        <ClientSideEvents EndCallback="callbackEnd" />
    </dx:ASPxCallback>
</asp:Content>
