<%@ Page Title="Προβολή Στοιχείων Κατάστασης" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ViewCatalogGroupDetails.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.ViewCatalogGroupDetails" %>

<%@ Register TagName="CatalogsGridView" TagPrefix="my" Src="~/UserControls/GridViews/CatalogsGridView.ascx" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <table class="dv" style="width: 100%">
        <tr>
            <th class="header">&raquo; Διανομές που περιέχονται στην Κατάσταση
            </th>
        </tr>
        <tr>
            <td>
                <my:CatalogsGridView ID="gvConnectedCatalogs" runat="server" DataSourceForceStandardPaging="false" HideInstitution="true" />
            </td>
        </tr>
    </table>

</asp:Content>
