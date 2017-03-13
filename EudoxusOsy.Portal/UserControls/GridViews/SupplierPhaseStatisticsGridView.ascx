<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierPhaseStatisticsGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.SupplierPhaseStatisticsGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvSupplierPhaseStatistics" runat="server">
    <SettingsPager Mode="ShowAllRecords" />
    <Columns>
        <dx:GridViewDataTextColumn FieldName="Phase.ID" Name="PhaseID" Caption="Αριθμός Περιόδου" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Phase.AcademicYearString" Caption="Ακ. Έτος" Width="50px" />
        <dx:GridViewDataTextColumn FieldName="Phase.SemesterType" Name="SemesterType" Caption="Εξάμηνο" Width="50px">
            <DataItemTemplate>
                <%# ((enSemesterType)Eval("Phase.SemesterType")).GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Phase.StartDate" Name="UpdatedAt" Caption="Ημ/νία Έναρξης" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyyy}" />
        <dx:GridViewDataTextColumn FieldName="Phase.EndDate" Name="EndDate" Caption="Ημ/νία Λήξης" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyyy}" />
        <dx:GridViewDataTextColumn FieldName="OwedAmount" Name="OwedAmount" Caption="Οφειλή φάσης" PropertiesTextEdit-DisplayFormatString="{0:c}" />
        <dx:GridViewDataTextColumn FieldName="AllocatedAmount" Name="AllocatedAmount" Caption="Ανάθεση ποσού φάσης" PropertiesTextEdit-DisplayFormatString="{0:c}" />
        <dx:GridViewDataTextColumn FieldName="RemainingAmount" Name="RemainingAmount" Caption="Διαθέσιμο ποσό (χωρίς ΦΠΑ)" PropertiesTextEdit-DisplayFormatString="{0:c}" />
        <dx:GridViewDataTextColumn FieldName="PaidAmount" Name="PaidAmount" Caption="Εισπραχθέν ποσό (χωρίς ΦΠΑ)" PropertiesTextEdit-DisplayFormatString="{0:c}" />
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveSupplierPhaseStatistics" runat="server" GridViewID="gvSupplierPhaseStatistics" OnRenderBrick="gveSupplierPhaseStatistics_RenderBrick" />
