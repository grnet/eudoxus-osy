<%@ Page Title="Στατιστικά εκδοτών" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="SuppliersStats.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.SuppliersStats" %>

<%@ Register TagPrefix="my" TagName="SuppliersSearchFilters" Src="~/UserControls/SearchFilters/SupplierSearchFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersGridView" Src="~/UserControls/GridViews/SuppliersGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SuppliersExportGridView" Src="~/UserControls/ExportGridViews/SuppliersExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BasicStatsView" Src="~/UserControls/MinistryControls/ViewControls/BasicStatsView.ascx" %>


<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <div class="br"></div>
    <div class="reminder">
        <table class="dv">
            <tr>
                <th>Επιλογή Περιόδου Πληρωμών:
                </th>
                <td>
                    <dx:ASPxComboBox runat="server" ID="cmbPhases" OnInit="cmbPhases_Init" AutoPostBack="true" OnValueChanged="cmbPhases_ValueChanged"></dx:ASPxComboBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="br"></div>
    <my:BasicStatsView runat="server" ID="bsView" />
    <div class="br"></div>
    <div style="text-align: left">
        <dx:ASPxButton runat="server" ID="btnExportSuppliersStats" Text="Εξαγωγή στατιστικών εκδοτών" ToolTip="Εξαγωγή στατιστικών εκδοτών" Image-Url="~/_img/iconExcel.png">
            <ClientSideEvents Click="showExportSupplierStats" />
        </dx:ASPxButton>
    </div>
    <div style="text-align: right">
        <a href="AcademicsStats.aspx">Στατιστικά Ιδρυμάτων</a>
    </div>
    <my:SuppliersSearchFilters ID="ucSearchFilters" runat="server" Mode="Stats" />

    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { cmdRefresh(); }" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>

    <dx:ASPxGridView ID="gvSuppliersStats" runat="server" DataSourceID="odsSuppliers"
        DataSourceForceStandardPaging="true" OnCustomCallback="gvSuppliersStats_CustomCallback">
        <Columns>
            <dx:GridViewDataTextColumn Caption="InnerID" FieldName="supplier_id" Visible="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="supplier_kpsid"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Επωνυμία" FieldName="publicFinancialOffice"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΔΟΥ" FieldName="paymentPfo" Settings-AllowSort="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΑΦΜ" FieldName="taxRoll_number" Settings-AllowSort="false"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Υπεύθυνος Επικοινωνίας" FieldName="contact_name"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τύπος Εκδότη" FieldName="supplierType" Settings-AllowSort="false">
                <DataItemTemplate>
                    <%# ((enSupplierType)((SupplierFullStatistics)Container.DataItem).supplierType).GetLabel() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Οφειλή φάσης" FieldName="totalprice" PropertiesTextEdit-DisplayFormatString="{0:c}" >                            
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ανάθεση ποσού φάσης" FieldName="totalpayment" PropertiesTextEdit-DisplayFormatString="{0:c}">                
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ποσό προς ΥΔΕ" FieldName="totaltoyde" PropertiesTextEdit-DisplayFormatString="{0:c}">                
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>

    <asp:ObjectDataSource ID="odsSuppliers" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Suppliers" CacheDuration="600"
        SelectMethod="GetSuppliersStats" SelectCountMethod="CountResult" MaximumRowsParameterName="maximumRows" OnSelecting="odsSuppliers_Selecting"
        StartRowIndexParameterName="startRowIndex"
        EnablePaging="true" SortParameterName="sortExpression">
        <SelectParameters>
            <asp:Parameter Name="supplierKpsID" Type="Int32" />
            <asp:Parameter Name="phaseID" Type="Int32" />
            <asp:Parameter Name="afm" Type="String" />
            <asp:Parameter Name="supplierType" Type="Int32" />
            <asp:Parameter Name="name" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <script type="text/javascript">
        function cmdRefresh() {
            doAction('refresh', '');
        }
    </script>

</asp:Content>
