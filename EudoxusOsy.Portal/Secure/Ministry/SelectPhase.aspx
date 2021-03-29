<%@ Page Title="Επιλογή Περιόδου Πληρωμών" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.master" AutoEventWireup="true" CodeBehind="Ministry.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.SelectPhase" %>

<%@ Register TagName="PhasesGridView" TagPrefix="my" Src="~/UserControls/GridViews/PhasesGridView.ascx" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <script type="text/javascript">
        function onCallbackEnd() {
            
            if (cbSubmitAmount.cperror) {
                showAlertBox("Το ποσό περιόδου που εισάγατε δεν ήταν αρκετό για να καλύψει το όριο αποπληρωμής όλων των εκδοτών.");
                cbSubmitAmount.cperror = null;
            }
            else if (cbSubmitAmount.cperrorcatalogs) {
                showAlertBox("Δεν είναι δυνατή η προσθήκη ποσού. Δεν υπάρχουν Διανομές για την επιλεγμένη περίοδο.");
                cbSubmitAmount.cperrorcatalogs = null;
            }
            else {
                gvPhases.PerformCallback('refresh');
                showAlertBox('Η διαδικασία εισαγωγής του ποσού ολοκληρώθηκε επιτυχώς.');                
            }
            LoadingPanel.Hide();
        }

        function onCallbackEndRemoveExtraSupplierAmount() {
            if (cbRemoveExtraSupplierAmount.cperror) {
                showAlertBox("Η διαγραφή πλεονάζοντος ποσού απέτυχε.");
                cbRemoveExtraSupplierAmount.cperror = null;
            } else if (cbRemoveExtraSupplierAmount.cpnoextraamount) {
                showAlertBox("Δεν υπάρχει πλεονάζον ποσό.");
                cbRemoveExtraSupplierAmount.cpnoextraamount = null;

            } else {
                gvPhases.PerformCallback('refresh');
                showAlertBox('Η διαγραφή πλεονάζοντος ποσού ανάθεσης ολοκληρώθηκε επιτυχώς.');
            }
            LoadingPanel.Hide();
        }

        function onCallbackEndPhaseAmountMinistry() {
            if (cbPhaseAmountMinistry.cperror) {
                showAlertBox("Η ανάθεση ποσού ΥΠΕΠΘ απέτυχε.");
                cbPhaseAmountMinistry.cperror = null;
            } else {
                gvPhases.PerformCallback('refresh');
                showAlertBox('Η ανάθεση ποσού ΥΠΕΠΘ ολοκληρώθηκε επιτυχώς.');                
            }
            LoadingPanel.Hide();
        }

        function DoSubmitAmount(s, e) {
            if (ASPxClientEdit.ValidateGroup("vgAmount")) {
                LoadingPanel.Show();
                cbSubmitAmount.PerformCallback();
            }
        }

        function DoRemoveExtraSupplierAmount(s, e) {
            if (ASPxClientEdit.ValidateGroup("vgSupplierPhase")) {
                LoadingPanel.Show();
                cbRemoveExtraSupplierAmount.PerformCallback();
            }
        }

        function DoSubmitAmountMinistry(s, e) {
            if (ASPxClientEdit.ValidateGroup("vgPhaseAmountMinistry")) {
                LoadingPanel.Show();
                cbPhaseAmountMinistry.PerformCallback();
            }
        }        

        function openVatEditPopup(s, e) {
            showVatDataEditPopup();
        }

        function doCreateCatalogs() {
            showConfirmBox('Δημιουργία Διανομών', 'Είστε σίγουρος/η ότι θέλετε να δημιουργήσετε διανομές για την τρέχουσα περίοδο πληρωμών;',
            function () {
                LoadingPanel.Show();
                btnCreateCatalogsHidden.DoClick();
            });
        }


    </script>
    <my:PhasesGridView ID="gvPhases" ClientInstanceName="gvPhases" runat="server" DataSourceID="odsPhases"
        OnHtmlRowPrepared="gvPhases_HtmlRowPrepared"
        OnCustomDataCallback="gvPhases_CustomDataCallback">
    </my:PhasesGridView>
    <div class="br"></div>
    <table class="dv" width="50%">
        <tr>
            <th class="header" colspan="2">Διαγραφή πλεονάζοντος Ποσού Ανάθεσης</th>
        </tr>        
        <tr>
            <th>Περίοδος Πληρωμών
            </th>
            <td>
                <dx:ASPxComboBox runat="server" ID="ddlSelectSupplierPhase" OnInit="ddlSelectSupplierPhase_Init">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Περίοδος Πληρωμών' είναι υποχρεωτικό."  ValidationGroup="vgSupplierPhase"/>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <dx:ASPxButton runat="server" ID="btnRemoveExtraSupplierAmount" Text="Διαγραφή Ποσού" ValidationGroup="vgSupplierPhase">           
                    <ClientSideEvents Click="DoRemoveExtraSupplierAmount" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <div class="br"></div> 
    <table class="dv" width="50%">
        <tr>
            <th class="header" colspan="2">Ανάθεση ποσού - ΥΠΕΠΘ</th>
        </tr>        
        <tr>
            <th>Περίοδος Πληρωμών
            </th>
            <td>
                <dx:ASPxComboBox runat="server" ID="ddlSelectSupplierPhaseMinistry" OnInit="ddlSelectSupplierPhaseMinistry_Init">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Περίοδος Πληρωμών' είναι υποχρεωτικό."  ValidationGroup="vgPhaseAmountMinistry"/>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <th>Ποσό</th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtPhaseAmountMinistry" Width="100px" MinValue="0" DisplayFormatString="c">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ποσό' είναι υποχρεωτικό."  ValidationGroup="vgPhaseAmountMinistry"/>
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <dx:ASPxButton runat="server" ID="btnPhaseAmountMinistry" Text="Ανάθεση Ποσού" ValidationGroup="vgPhaseAmountMinistry">  
                    <ClientSideEvents Click="DoSubmitAmountMinistry" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <div class="br"></div>
    <table class="dv" width="50%">
        <tr>
            <th class="header" colspan="2">Ανάθεση ποσού - ΠΣ</th>
        </tr>
        <tr>
            <th>Περίοδος Πληρωμών
            </th>
            <td>
                <dx:ASPxComboBox runat="server" ID="ddlSelectPhase" OnInit="ddlSelectPhase_Init">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Περίοδος Πληρωμών' είναι υποχρεωτικό."  ValidationGroup="vgAmount"/>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <th>Ποσό</th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtPhaseAmount" Width="100px" MinValue="0" DisplayFormatString="c">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ποσό' είναι υποχρεωτικό."  ValidationGroup="vgAmount"/>
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>Όριο Αποπληρωμής
            </th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtAmountLimit"  Width="100px" MinValue="0" DisplayFormatString="c">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Όριο αποπληρωμής' είναι υποχρεωτικό." ValidationGroup="vgAmount"/>
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <dx:ASPxButton runat="server" ID="btnSubmitAmount" Text="Ανάθεση ποσού" ValidationGroup="vgAmount">
                    <ClientSideEvents Click="DoSubmitAmount" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>

    <br />
    <dx:ASPxButton runat="server" CssClass="btn-green" ID="btnEditVat" Text="Αλλαγή προκαθορισμένου ΦΠΑ" ClientInstanceName="btnEditVat">
        <ClientSideEvents Click='openVatEditPopup' />
    </dx:ASPxButton>

    <dx:ASPxCallback runat="server" ID="cbSubmitAmount" ClientInstanceName="cbSubmitAmount" OnCallback="cbSubmitAmount_Callback">
        <ClientSideEvents EndCallback="onCallbackEnd" />
    </dx:ASPxCallback>
    
    <dx:ASPxCallback runat="server" ID="cbRemoveExtraSupplierAmount" ClientInstanceName="cbRemoveExtraSupplierAmount" OnCallback="cbRemoveExtraSupplierAmount_Callback">
        <ClientSideEvents EndCallback="onCallbackEndRemoveExtraSupplierAmount" />
    </dx:ASPxCallback>
    
    <dx:ASPxCallback runat="server" ID="cbPhaseAmountMinistry" ClientInstanceName="cbPhaseAmountMinistry" OnCallback="cbPhaseAmountMinistry_Callback">
        <ClientSideEvents EndCallback="onCallbackEndPhaseAmountMinistry" />
    </dx:ASPxCallback>

    <asp:ObjectDataSource ID="odsPhases" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Phases"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPhases_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <div class="filterButtons">
        <dx:ASPxButton runat="server" ID="btnCreateCatalogs" ClientInstanceName="btnCreateCatalogs">
            <ClientSideEvents Click="doCreateCatalogs" />
        </dx:ASPxButton>
        <dx:ASPxButton ClientVisible="false" runat="server" ID="btnCreateCatalogsHidden" ClientInstanceName="btnCreateCatalogsHidden" 
            OnClick="btnCreateCatalogs_Click"></dx:ASPxButton>
    </div>    

    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>

</asp:Content>
