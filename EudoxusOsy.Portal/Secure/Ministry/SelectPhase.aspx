<%@ Page Title="Επιλογή Περιόδου Πληρωμών" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.master" AutoEventWireup="true" CodeBehind="Ministry.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.SelectPhase" %>

<%@ Register TagName="PhasesGridView" TagPrefix="my" Src="~/UserControls/GridViews/PhasesGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <script type="text/javascript">
        function onCallbackEnd() {
            LoadingPanel.Hide();
            if (cbSubmitAmount.cperror) {
                showAlertBox("Τα ποσό περιόδου που εισάγατε δεν ήταν αρκετό για να καλύψει το όριο αποπληρωμής όλων των εκδοτών.");
                cbSubmitAmount.cperror = null;
            }
            else {
                Imis.Lib.notify('Η διαδικασία εισαγωγής του ποσού ολοκληρώθηκε επιτυχώς.');
            }
        }

        function DoSubmitAmount(s, e) {
            LoadingPanel.Show();            
            cbSubmitAmount.PerformCallback();
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
    <my:PhasesGridView ID="gvPhases" runat="server" DataSourceID="odsPhases"
        OnHtmlRowPrepared="gvPhases_HtmlRowPrepared"
        OnCustomDataCallback="gvPhases_CustomDataCallback">
        <%--        <Columns>
            <dx:GridViewDataTextColumn Name="Actions" Caption=" " Width="50px" VisibleIndex="0">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <input runat="server" type="checkbox" onclick='<%# string.Format("return doAction(\"selectPhase\", {0}, \"SelectPhase\");", Eval("ID"))%>'
                        checked='<%# IsSelected((Phase)Container.DataItem) %>' />
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>--%>
    </my:PhasesGridView>
    <div class="br"></div>
    <table class="dv" width="50%">
        <tr>
            <th class="header" colspan="2">Εισαγωγή ποσού για περίοδο πληρωμών</th>
        </tr>
        <tr>
            <th>Επιλογή Περιόδου Πληρωμών
            </th>
            <td>
                <dx:ASPxComboBox runat="server" ID="ddlSelectPhase" OnInit="ddlSelectPhase_Init"></dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <th>Ποσό που θέλετε να εισάγετε</th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtPhaseAmount" Width="100px"></dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <th>Όριο Αποπληρωμής
            </th>
            <td>
                <dx:ASPxTextBox runat="server" ID="txtAmountLimit" Width="100px"></dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <dx:ASPxButton runat="server" ID="btnSubmitAmount" Text="Εισαγωγή Ποσού">
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
