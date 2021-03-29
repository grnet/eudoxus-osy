<%@ Page Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="BusinessLogikErrorsKP.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.BusinessLogikErrorsKP" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">      
    <br/>
    <dx:ASPxButton ID="btnReturn" runat="server" Text="Επιστροφή" Image-Url="~/_img/iconReturn.png" OnClick="btnReturn_Click" CausesValidation="false" />
    <br/>
    
    <h1>Καταγραφή λογικών σφαλμάτων</h1>
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
            <tr>
                <th>
                    Ημ/νία Καταγραφής σφάλματος
                </th>
                <td>
                    <dx:ASPxDateEdit runat="server" ID="dateErrorDate"></dx:ASPxDateEdit>
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
            </tr>
        </table>
    </div>
    <br/>   
    <label>Καταγραφή λογικών σφαλμάτων</label>
    <br/>   
    <hr/>

    <dx:ASPxGridView runat="server" ID="gvBusinessLogicErrors" ClientInstanceName="gvBusinessLogicErrors" DataSourceID="odsBusinessLogicErrorsKP" DataSourceForceStandardPaging="true">
        <Columns>
            <dx:GridViewDataTextColumn Name="EntityID" FieldName="EntityID" Caption="Κωδικός ΚΠΣ Βιβλίου"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="CreatedAt" FieldName="CreatedAt" Caption="Ημ/νία Καταγραφής"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="Description" FieldName="Description" Caption="Περιγραφή Σφάλματος"></dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>

    <asp:ObjectDataSource ID="odsBusinessLogicErrorsKP" runat="server" TypeName="EudoxusOsy.Portal.DataSources.BusinessLogicErrors"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" 
        SortParameterName="sortExpression" 
        OnSelecting="odsBusinessLogicErrorsKP_OnSelecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>    
    
    <script type="text/javascript">
        function cmdSearch() {
            gvBusinessLogicErrors.PerformCallback("refresh");
            //doAction('search', '0', "PriceVerificationHistory");
        }
    </script>
</asp:Content>

