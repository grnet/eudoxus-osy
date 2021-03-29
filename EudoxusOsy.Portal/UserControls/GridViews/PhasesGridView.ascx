<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhasesGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.PhasesGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvPhases" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="ID" Name="ID" Caption="Αριθμός Περιόδου" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="AcademicYearString" Caption="Ακ. Έτος" Width="50px" />
        <dx:GridViewDataTextColumn FieldName="SemesterType" Name="SemesterType" Caption="Εξάμηνο" Width="50px">
            <DataItemTemplate>
                <%# ((enSemesterType)Eval("SemesterType")).GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="StartDate" Name="UpdatedAt" Caption="Ημ/νία Έναρξης" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyyy}" />
        <dx:GridViewDataTextColumn FieldName="EndDate" Name="EndDate" Caption="Ημ/νία Λήξης" PropertiesTextEdit-DisplayFormatString="{0:dd/MM/yyyy}" />
        <dx:GridViewDataTextColumn FieldName="PhaseAmount" Name="PhaseAmount" Caption="Συνολικό ποσό ανάθεσης - ΠΣ" PropertiesTextEdit-DisplayFormatString="{0:c}" />
        <dx:GridViewDataTextColumn FieldName="PhaseAmountMinistry" Name="PhaseAmountMinistry" Caption="Ανάθεση ποσού - ΥΠΕΠΘ" PropertiesTextEdit-DisplayFormatString="{0:c}" />
        <dx:GridViewDataTextColumn FieldName="TotalDebt" Name="TotalDebt" Caption="Οφειλή φάσης" PropertiesTextEdit-DisplayFormatString="{0:c}" />
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gvePhases" runat="server" GridViewID="gvPhases" OnRenderBrick="gvePhases_RenderBrick" />
