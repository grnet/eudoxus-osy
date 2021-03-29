<%@ Page Title="Στατιστικά Ιδρυμάτων" Language="C#" MasterPageFile="~/Secure/Ministry/Ministry.Master" AutoEventWireup="true" CodeBehind="AcademicsStats.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.AcademicsStats" %>

<%@ Register TagPrefix="my" TagName="InstGridView" Src="~/UserControls/ExportGridViews/InstitutionStatsExportGridView.ascx" %>
<%@ Register TagPrefix="my" TagName="DepGridView" Src="~/UserControls/ExportGridViews/DepartmentsStatsExportGridView.ascx" %>


<asp:Content ContentPlaceHolderID="cphSecureMain" runat="server">    
    <dx:ASPxButton ID="btnReturn" runat="server" Text="Επιστροφή" Image-Url="~/_img/iconReturn.png" OnClick="btnReturn_Click" CausesValidation="false" />
    
<table class="dv">
    <colgroup>
        <col style="width: 90px" />
        <col style="width: 300px" />
        <col style="width: 100px" />
        <col style="width: 150px" />        
    </colgroup>
    <tr>
        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th>Τίτλος Ιδρύματος:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtInistitutionName" runat="server"></dx:ASPxTextBox>
        </td>
        <th>Φάση:
        </th>
        <td>
            <dx:ASPxComboBox ID="dllPhase" ClientInstanceName="dllPhase" runat="server">
                <ClientSideEvents Validation="function(s, e) {
                    if (s.GetSelectedIndex()==0) {
                          e.isValid = false;
                          e.errorText = 'Παρακαλούμε επιλέξτε την φάση πληρωμών για την οποία θέλετε να εμφανίσετε τα στατιστικά.';
                     }}"
                     />

            </dx:ASPxComboBox>
        </td>
    </tr>   
</table>
    
    <div style="padding: 5px 0px 20px;">
        <dx:ASPxButton ID="btnSearch" runat="server" Text="Αναζήτηση" Image-Url="~/_img/iconView.png">
                        <ClientSideEvents Click="function(s,e) { cmdRefresh(); }" />
                    </dx:ASPxButton>
        <dx:ASPxButton ID="btnExportIntitutions" runat="server" Text="Εξαγωγή: Στατιστικών ιδρυμάτων" OnClick="btnExportIntitutions_Click">                        
                    </dx:ASPxButton>
        <dx:ASPxButton ID="btnExportDepartments" runat="server" Text="Εξαγωγή: Στατιστικών τμημάτων" OnClick="btnExportDepartments_Click">
                    </dx:ASPxButton>       
    </div>
    <dx:ASPxGridView ID="gvAcademics" runat="server" DataSourceID="SqlDataSource" OnInit="gvAcademics_OnInit" OnCustomCallback="gvAcademics_OnCustomCallback">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ID" Name="ID" Caption="ID" Width="30px">
                <HeaderStyle HorizontalAlign="Center" />
                <CellStyle HorizontalAlign="Center" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Τίτλος" />
            <dx:GridViewDataTextColumn FieldName="DepartmentCount" Name="DepartmentCount" Caption="Σύνολο Τμημάτων" Width="60px" />
            <dx:GridViewDataTextColumn FieldName="Debt" Caption="Κόστος Ιδρύματος" Width="60px" PropertiesTextEdit-DisplayFormatString="{0:c}">            
            </dx:GridViewDataTextColumn>               
        </Columns>
    </dx:ASPxGridView>    
     
    <asp:SqlDataSource ID="SqlDataSource" runat="server"        
        SelectCommand="SELECT vs.Debt AS Debt, vs.InstName AS Name, DepartmentCount, vs.InstitutionID AS ID
            FROM report.ViewStatisticsPerInstitution vs                        
            WHERE PhaseID = @phaseId AND vs.InstName like @Name
            "
        OnSelecting="SqlDataSource_OnSelecting" CancelSelectOnNullParameter="False">                
         <SelectParameters>
            <asp:Parameter  Name="phaseId"  DbType="Int32" DefaultValue="0" />        
            <asp:Parameter  Name="Name"  DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
            
    <asp:SqlDataSource ID="SqlDataSourceInstitution" runat="server"        
        SelectCommand="SELECT * FROM report.ViewStatisticsPerInstitution WHERE PhaseID = @phaseId"
        OnSelecting="SqlDataSourceInstitution_OnSelecting">                
         <SelectParameters>
            <asp:Parameter  Name="phaseId"  DbType="Int32" DefaultValue="0" />                    
        </SelectParameters>
    </asp:SqlDataSource>
    
    <my:InstGridView ID="gvInstExport" GridClientVisible="false" DataSourceForceStandardPaging="false" 
        ClientInstanceName="gvInstExport" runat="server" OnCustomCallback="gvInstExport_OnCustomCallback" DataSourceID="SqlDataSourceInstitution"
        OnInit="gvInstExport_OnInit">
    </my:InstGridView>
    
    <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server"        
        SelectCommand="SELECT * FROM report.ViewStatisticsPerDepartment WHERE PhaseID = @phaseId"
        OnSelecting="SqlDataSourceDepartment_OnSelecting">                
         <SelectParameters>
            <asp:Parameter  Name="phaseId"  DbType="Int32" DefaultValue="0" />                    
        </SelectParameters>
    </asp:SqlDataSource>
    
    <my:DepGridView ID="gvDepExport" GridClientVisible="false" DataSourceForceStandardPaging="false" 
        ClientInstanceName="gvDepExport" runat="server" OnCustomCallback="gvDepExport_OnCustomCallback" DataSourceID="SqlDataSourceDepartment"
        OnInit="gvDepExport_OnInit">
    </my:DepGridView>
    

    <script type="text/javascript">
        function cmdRefresh() {
            doAction('refresh', '');
        }
    </script>

</asp:Content>
