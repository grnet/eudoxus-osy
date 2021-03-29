<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="SelectPhasePopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.SelectPhasePopup" %>

<%@ Register TagPrefix="my" TagName="CommitmentsExportGridView" Src="~/UserControls/ExportGridViews/CommitmentsRegistryExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="NoLogisticBooksExportGridView" Src="~/UserControls/ExportGridViews/SuppliersNoLogisticBooksExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="CoAuthorExportGridView" Src="~/UserControls/ExportGridViews/CoAuthorExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="SupplierStatsExportGridView" Src="~/UserControls/ExportGridViews/SuppliersStatsExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="CatalogsReportExportGridView" Src="~/UserControls/ExportGridViews/CatalogsReportExportGridView.ascx" %>



<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
    </script>
    <div class="br"></div>
    <div runat="server" id="divSelectPhase">
        <table>
            <tr>
                <th>
                    <asp:Label ID="lblPhase" runat="server">Εισάγετε τη φάση πληρωμών*:</asp:Label>
                </th>
                <td>
                    <dx:ASPxSpinEdit runat="server" ID="txtPhaseID">
                        <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Φάση Πληρωμών' είναι υποχρεωτικό"></ValidationSettings>
                    </dx:ASPxSpinEdit>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblAllPhases" runat="server">(*) 0 για όλες τις φάσεις πληρωμών</asp:Label>
    </div>
    <div runat="server" id="divSelectYear">
        <table class="dv">
            <tr>
                <th>
                     Επιλέξτε το ακαδημαϊκό έτος*:
                </th>
                <td>
                    <dx:ASPxComboBox runat="server" Width="200px" ID="ddlSelectYear" ClientInstanceName="ddlSelectYear" OnInit="ddlSelectYear_Init">
                    </dx:ASPxComboBox>
<%--                    <dx:ASPxSpinEdit runat="server" ID="spinYear">
                        <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Έτος' είναι υποχρεωτικό"></ValidationSettings>
                    </dx:ASPxSpinEdit>--%>
                </td>
            </tr>
        </table>
        <span>(*) [χωρίς επιλογή] για όλα τα ακαδημαϊκά έτη</span>
    </div>
    <div class="br"></div>
    <div runat="server" id="divMoveToPhase">
        <table>
            <tr>
                <th>Μεταφορά στη φάση πληρωμών:
                </th>
                <td>
                    <dx:ASPxSpinEdit runat="server" ID="txtMoveToPhaseID">
                        <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Φάση Πληρωμών' είναι υποχρεωτικό"></ValidationSettings>
                    </dx:ASPxSpinEdit>
                </td>
            </tr>
        </table>
    </div>

    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" ValidationGroup="vgFormErrors" OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />
    <my:CommitmentsExportGridView ID="gveExportCommitments" GridClientVisible="false" DataSourceForceStandardPaging="false" ClientInstanceName="gveExportCommitments" runat="server">
    </my:CommitmentsExportGridView>
    <my:NoLogisticBooksExportGridView ID="gveNoLogisticBooks" GridClientVisible="false" DataSourceForceStandardPaging="false" ClientInstanceName="gveNoLogisticBooks" runat="server">
    </my:NoLogisticBooksExportGridView>
    <my:CoAuthorExportGridView runat="server" ID="ucCoAuthorExport" DataSourceForceStandardPaging="false" Visible="false"></my:CoAuthorExportGridView>
    <my:SupplierStatsExportGridView runat="server" ID="ucSupplierStats" DataSourceForceStandardPaging="false" Visible="false"></my:SupplierStatsExportGridView>
    <my:CatalogsReportExportGridView runat="server" ID="ucCatalogsReport" DataSourceForceStandardPaging="false" Visible="false"></my:CatalogsReportExportGridView>


    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmitHidden.DoClick();
        }
    </script>

</asp:Content>
