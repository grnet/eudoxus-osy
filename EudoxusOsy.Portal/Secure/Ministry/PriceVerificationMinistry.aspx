<%@ Page Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="PriceVerificationMinistry.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.PriceVerificationMinistry" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Register TagPrefix="my" TagName="PriceVerificationGridView" Src="~/UserControls/GridViews/PriceVerificationGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BookPriceChangesGridView" Src="~/UserControls/GridViews/BookPriceChangesGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="BookSearchFilters" Src="~/UserControls/SearchFilters/BookSearchFiltersControl.ascx" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">      
    <br/>
    <dx:ASPxButton ID="btnReturn" runat="server" Text="Επιστροφή" Image-Url="~/_img/iconReturn.png" OnClick="btnReturn_Click" CausesValidation="false" />
    <br/>
    
    <h1>Έγκριση τιμών Υπουργείου</h1>
    <my:BookSearchFilters ID="ucSearchFilters" runat="server" />
    <div class="filterButtons">
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { cmdSearch(); }" />
                    </dx:ASPxButton>                    
                </td>
            </tr>
        </table>
    </div>
    <my:PriceVerificationGridView ID="gvBooks" runat="server" ClientInstanceName="gvBooks" 
        DataSourceID="odsBooks" OnCustomCallback="gvBooks_OnCustomCallback"
        Mode="Ministry">
       <Columns>
       <dx:GridViewDataTextColumn Settings-AllowSort="False" Name="ComiteeVerification" Caption="Ενέργειες" Width="60px" >
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>                                          
                <a runat="server" href="javascript:void(0);" class="img-btn tooltip" title="Ξεκλείδωμα Βιβλίου"
                    onclick='<%# string.Format("return doAction(\"unlock\", {0}, \"PriceVerificationMinistry\", \"{1}\");", Eval("ID"), Eval("title"))%>'
                    visible='<%# ((Book)Container.DataItem).PendingCommitteePriceVerification.Value %>'>
                    <img src="/_img/iconUnLock.png" alt="Ξεκλείδωμα Βιβλίου" /></a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>      
        </Columns>
    </my:PriceVerificationGridView>

    <asp:ObjectDataSource ID="odsBooks" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Books"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" 
        SortParameterName="sortExpression" 
        OnSelecting="odsBooks_OnSelecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>    
    <br/>   
    <label>Ιστορικό</label>
    <br/>   
    <hr/>
    <my:BookPriceChangesGridView ID="gvBookPriceChange" runat="server" DataSourceID="odsBookPriceChanges" OnCustomCallback="gvBookPriceChange_OnCustomCallback">       
    </my:BookPriceChangesGridView>

    <asp:ObjectDataSource ID="odsBookPriceChanges" runat="server" TypeName="EudoxusOsy.Portal.DataSources.BookPriceChanges"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" 
        SortParameterName="sortExpression" 
        OnSelecting="odsBookPriceChanges_OnSelecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>    
    
    <script type="text/javascript">
        function cmdSearch() {
            doAction('search', '0', "PriceVerificationMinistry");
        }
    </script>
</asp:Content>

