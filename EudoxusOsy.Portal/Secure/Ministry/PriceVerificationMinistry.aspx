<%@ Page Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="PriceVerificationMinistry.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.PriceVerificationMinistry" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Register TagPrefix="my" TagName="PriceVerificationGridView" Src="~/UserControls/GridViews/PriceVerificationGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BookPriceVerificationSearchFilters" Src="~/UserControls/SearchFilters/BookPriceVerificationFiltersControl.ascx" %>
<%@ Register TagPrefix="my" TagName="PriceVerificationMinistryExportGrid" Src="~/UserControls/ExportGridViews/PriceVerificationMinistryExportGrid.ascx" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">      
    
    <script type="text/javascript">
        function checkErrors() {
            if (gvBooks.cperrors != null) {
                showAlertBox(gvBooks.cperrors);
                gvBooks.cperrors = null;
            }
        }
    </script>
        <br/>
        <dx:ASPxButton ID="btnReturn" runat="server" Text="Επιστροφή" Image-Url="~/_img/iconReturn.png" OnClick="btnReturn_Click" CausesValidation="false" />
        <br/>
    
        <h1>Λίστα βιβλίων υπό διαδικασία ελέγχου κοστολόγησης</h1>
        <my:BookPriceVerificationSearchFilters ID="ucSearchFilters" runat="server" />
        <div class="filterButtons">
        <table>
            <tr>
                <td>
                <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                            <ClientSideEvents Click="function(s,e) { cmdSearch(); }" />
                        </dx:ASPxButton>                    
                </td>
                <td>
                    <dx:ASPxButton ID="btnExport" runat="server" Text="Εξαγωγή" Image-Url="~/_img/iconExcel.png" OnClick="btnExport_OnClick">                        
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
    <my:PriceVerificationGridView ID="gvBooks" runat="server" ClientInstanceName="gvBooks" 
        DataSourceID="odsBookPriceChangeGrid" OnCustomCallback="gvBooks_OnCustomCallback"
        Mode="Ministry">
        <ClientSideEvents EndCallback="checkErrors" />
       <Columns>
       <dx:GridViewDataTextColumn Settings-AllowSort="False" Name="ComiteeVerification" Caption="Ενέργειες" Width="60px" >
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>                                          
                <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Ξεκλείδωμα Βιβλίου"
                    onclick='<%# string.Format("return doAction(\"unlock\", {0}, \"PriceVerificationMinistry\", \"{1}\");", Eval("BookPriceID"), Eval("TitleView"))%>'
                    visible='<%# ((BookPricesGridV)Container.DataItem).BookPriceID != null && ((BookPricesGridV)Container.DataItem).HasPendingPriceVerification %>'>
                    <img src="/_img/iconUnLock.png" alt="Ξεκλείδωμα Βιβλίου" /></a>
                <a runat="server" href='<%# string.Format("Catalogs.aspx?bID={0}", ((BookPricesGridV)Container.DataItem).BookKpsID) %>' target="_blank" class="img-btn tooltip" title="Προβολή διανομών βιβλίου">
                    <img src="/_img/iconView.png" alt="Προβολή διανομών βιβλίου" /></a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>      
        </Columns>
    </my:PriceVerificationGridView>
    
    <my:PriceVerificationMinistryExportGrid ID="gvBooksExport" Visible="false" runat="server" DataSourceForceStandardPaging="false">
    </my:PriceVerificationMinistryExportGrid>

    <asp:ObjectDataSource ID="odsBookPriceChangeGrid" runat="server" TypeName="EudoxusOsy.Portal.DataSources.BookPricesGrid"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" 
        SortParameterName="sortExpression" 
        OnSelecting="odsBooks_OnSelecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>    
    <br/>   
    <script type="text/javascript">
        function cmdSearch() {
            doAction('search', '0', "PriceVerificationMinistry");
        }
    </script>
</asp:Content>

