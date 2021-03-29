<%@ Page Title="Στοιχεία Πληρωμής" Language="C#" MasterPageFile="~/Secure/Suppliers/Suppliers.Master" AutoEventWireup="true" CodeBehind="EditFinancialData.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Suppliers.EditFinancialData" %>
<%@ Register TagName="FileUpload" TagPrefix="my" Src="~/Controls/ScriptControls/FileUpload.ascx" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<%@ Register TagName="TipIcon" TagPrefix="my" Src="~/UserControls/GenericControls/TipIcon.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphSecureMain">
<script type="text/javascript">
    function savePaymentInfo(s, e) 
    {                                       
        if(ddlPaymentPfo.GetSelectedItem().value === -1){

            if(ASPxClientEdit.ValidateGroup('vgForeignPfo')){                                    
                e.isValid = true;
                btnSavePaymentPfo.DoClick();
            }
            else{
                e.isValid = false;                                            
            }                                                    
        }
        else if(ASPxClientEdit.ValidateGroup('vgPaymentPfo')){                                    
            e.isValid = true;
            btnSavePaymentPfo.DoClick();            
        }
        else{
            e.isValid = false;            
        }                                                    
    }
    </script>
            
    <table style="width: 100%">
        <tr>
            <td style="vertical-align: top">
                <table class="dv" style="width: 400px;">
                    <tr>
                        <th class="header">&raquo; Στοιχεία Δ.Ο.Υ. Πληρωμών
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <table class="dv" style="width: 100%">
                                <colgroup>
                                    <col style="width: 120px" />
                                </colgroup>
                                <tr>
                                    <th>Δ.Ο.Υ. Πληρωμών:
                                    </th>
                                    <td>
                                        <dx:ASPxComboBox ID="ddlPaymentPfo" runat="server" ClientInstanceName="ddlPaymentPfo" ValueType="System.Int32" OnInit="ddlPaymentPfo_Init">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { cbpForeignPfo.PerformCallback(ddlPaymentPfo.GetValue().toString()); }" />
                                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Δ.Ο.Υ. Πληρωμών' είναι υποχρεωτικό" ValidationGroup="vgPaymentPfo" />
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>

                            <dx:ASPxCallbackPanel ID="cbpForeignPfo" runat="server" ClientInstanceName="cbpForeignPfo" OnCallback="cbpForeignPfo_Callback">
                                <PanelCollection>
                                    <dx:PanelContent ID="pcForeignPfo" runat="server">
                                        <table class="dv" style="margin-top: 0; border-top: 0; width: 100%">
                                            <colgroup>
                                                <col style="width: 120px" />
                                            </colgroup>
                                            <tr>
                                                <th style="margin-top: 0; border-top: 0;">Δ.Ο.Υ. Εξωτερικού:
                                                </th>
                                                <td style="margin-top: 0; border-top: 0;">
                                                    <dx:ASPxTextBox ID="txtForeignPfo" runat="server">
                                                        <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Δ.Ο.Υ. Εξωτερικού' είναι υποχρεωτικό" ValidationGroup="vgForeignPfo" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>

                            <div class="br"></div>

                            <dx:ASPxButton ID="btnSubmitPaymentPfo" ClientInstanceName="btnSubmitPaymentPfo" runat="server" 
                                Text="Αποθήκευση" Image-Url="~/_img/iconSave.png" 
                                ClientEnabled="True">
                                <ClientSideEvents Click="savePaymentInfo">                                    
                                </ClientSideEvents>
                            </dx:ASPxButton>
                            
                            <dx:ASPxButton runat="server" ClientInstanceName="btnSavePaymentPfo" ClientVisible="false" OnClick="btnSavePaymentPfo_Click" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top">
                <table class="dv" style="width: 700px;">
                    <tr>
                        <th class="header">&raquo; Στοιχεία Τραπεζικού Λογαριασμού
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <table class="dv" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                </colgroup>
                                <tr>
                                    <th>Αριθμός Λογαριασμού (σε μορφή IBAN):
                            <my:TipIcon runat="server" Text="Κατά την έκδοση των τίτλων πληρωμής είναι πλέον υποχρεωτική (ΠΟΛ.1116/23-5-2013 απόφαση Υφυπουργού Οικονομικών) η αναγραφή του Τραπεζικού Λογαριασμού του δικαιούχου (σε μορφή IBAN). Συνεπώς, η καταχώριση του Τραπεζικού Λογαριασμού (σε μορφή IBAN) είναι απαραίτητη προϋπόθεση για την έκδοση χρηματικού εντάλματος και την πληρωμή των οφειλών του Υπουργείου προς τους δικαιούχους." />
                                    </th>
                                    <td>
                                        <dx:ASPxTextBox ID="txtIBAN" runat="server">
                                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Αριθμός Λογαριασμού' είναι υποχρεωτικό" ValidationGroup="vgIBAN" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Αποδεικτικό IBAN</th>
                                    <td>
                                        <my:FileUpload FieldName="Δικαιολογητικό" runat="server" ID="ucFileUpload" />
                                    </td>
                                </tr>
                            </table>

                            <div class="br"></div>

                            <dx:ASPxButton ID="btnSaveIBAN" runat="server" Text="Αποθήκευση" Image-Url="~/_img/iconSave.png" OnClick="btnSaveIBAN_Click" ValidationGroup="vgIBAN" />
                            <dx:ASPxLabel ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
