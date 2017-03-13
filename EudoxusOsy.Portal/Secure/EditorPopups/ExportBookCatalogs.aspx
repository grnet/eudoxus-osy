<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ExportBookCatalogs.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.ExportBookCatalogs" %>

<%@ Register TagPrefix="my" TagName="BooksGroupsExportGridView" Src="~/UserControls/ExportGridViews/BookGroupsExportGridView.ascx" %>


<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>
<%@ Import Namespace="EudoxusOsy.Portal.Secure.Ministry" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
    </script>
    <div class="br"></div>
    <table>
        <tr>
            <th>
                Εισάγετε το ID του βιβλίου:
            </th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtKpsID">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'ΚΠΣ ID Βιβλίου' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxSpinEdit>
            </td>
        </tr>
        <tr>
            <th>
                Εισάγετε τη φάση πληρωμών*:
            </th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtPhaseID">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Περίοδος' είναι υποχρεωτικό"></ValidationSettings>
                </dx:ASPxSpinEdit>
            </td>
        </tr>
    </table>
    <span>(*) 0 για όλες τις φάσεις πληρωμών</span>
    <div class="br"></div>

    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" ValidationGroup="vgFormErrors" OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />
    <my:BooksGroupsExportGridView ID="gvExportBookCatalogs" GridClientVisible="false" DataSourceForceStandardPaging="false" ClientInstanceName="gvExportBookCatalogs" runat="server" >
        <ClientSideEvents EndCallback="onExportBookGroupsEndCallback" />
    </my:BooksGroupsExportGridView>

    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmitHidden.DoClick();
        }
    </script>

</asp:Content>
