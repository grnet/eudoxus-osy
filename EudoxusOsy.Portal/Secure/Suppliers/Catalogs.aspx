<%@ Page Title="Ασύνδετες Διανομές" Language="C#" MasterPageFile="~/Secure/Suppliers/Suppliers.master" AutoEventWireup="true" CodeBehind="Catalogs.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Suppliers.Catalogs" %>

<%@ Register TagName="CatalogsGridView" TagPrefix="my" Src="~/UserControls/GridViews/CatalogsGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">
    <dx:ASPxButton runat="server" ClientInstanceName="btnGroupCatalogs" ID="btnGroupCatalogs" Text="Ομαδοποίηση διανομών ανά Ίδρυμα" Image-Url="~/_img/iconEdit.png" OnClick="btnGroupCatalogs_Click">
    </dx:ASPxButton>

    <dx:ASPxButton runat="server" ClientInstanceName="btnUngroupCatalogs" ID="btnUngroupCatalogs" Text="Αποσύνδεση Διανομών" Image-Url="~/_img/iconDelete.png" OnClick="btnUngroupCatalogs_Click">
    </dx:ASPxButton>
    <br />
    <br />
    <my:CatalogsGridView ID="gvCatalogs" runat="server" DataSourceID="odsCatalogs"
        OnHtmlRowPrepared="gvCatalogs_HtmlRowPrepared"
        OnCustomDataCallback="gvCatalogs_CustomDataCallback" DataSourceForceStandardPaging="true">
        <Columns>
            <dx:GridViewDataTextColumn Name="Actions" Caption="Δημιουργία Καστάστασης" Width="50px" VisibleIndex="8">
                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                <CellStyle HorizontalAlign="Center" />
                <DataItemTemplate>
                    <a runat="server" class="img-btn tooltip" title="Δημιουργία Κατάστασης"
                        href="javascript:void(0);"                      
                        onclick='<%# string.Format("createGroup({0});", Eval("ID")) %>'>
                        <img src="/_img/iconAddNewItem.png" alt="Δημιουργία Κατάστασης" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </my:CatalogsGridView>

    <asp:ObjectDataSource ID="odsCatalogs" runat="server" TypeName="EudoxusOsy.Portal.DataSources.Catalogs"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsCatalogs_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <div style="display: none">
        <asp:Button ID="btnCreateGroup" runat="server" OnClick="btnCreateGroup_Click" />
        <input type="hidden" id="hfCatalogID" runat="server" />
    </div>

    <script type="text/javascript">
        function createGroup(catalogID) {
            $('#<%= hfCatalogID.ClientID %>').val(catalogID);
            <%= ClientScript.GetPostBackEventReference(btnCreateGroup, null) %>
        }
    </script>

</asp:Content>
