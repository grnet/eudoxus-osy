<%@ Page Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="PriceVerificationHistory.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.PriceVerificationHistory" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Register TagPrefix="my" TagName="BookPriceChangesGridView" Src="~/UserControls/GridViews/BookPriceChangesGridView.ascx" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">      
    <br/>
    <dx:ASPxButton ID="btnReturn" runat="server" Text="Επιστροφή" Image-Url="~/_img/iconReturn.png" OnClick="btnReturn_Click" CausesValidation="false" />
    <br/>
    
    <h1>Ιστορικό αλλαγών τιμών</h1>
    <div class="br"></div>
    <div>
        <table class="dv">
            <tr>
                <th>
                    Κωδικός Βιβλίου ΚΠΣ
                </th>
                <td>
                    <dx:ASPxTextBox runat="server" ID="txtBookKpsID"></dx:ASPxTextBox>
                </td>
            </tr>
        </table>
    </div>
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
    <br/>   
    <label>Ιστορικό</label>
    <br/>   
    <hr/>
    <my:BookPriceChangesGridView ID="gvBookPriceChange" ClientInstanceName="gvBookPriceChange" runat="server" DataSourceID="odsBookPriceChanges" 
        OnCustomCallback="gvBookPriceChange_OnCustomCallback" OnHtmlRowPrepared="gvBookPriceChange_HtmlRowPrepared">       
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
            gvBookPriceChange.PerformCallback("refresh");
            //doAction('search', '0', "PriceVerificationHistory");
        }
    </script>
</asp:Content>

