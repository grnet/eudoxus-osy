<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookGroupsExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.BookGroupsExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvBooks" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν υπάρχουν βιβλία
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn Name="BookID" FieldName="BookID" Caption="ID Βιβλίου">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="Title" FieldName="Title" Caption="Τίτλος Βιβλίου">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="SupplierCode" FieldName="SupplierCode" Caption="ID Διαθέτη">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="Publisher" FieldName="Publisher" Caption="Επωνυμία Εκδότη">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="BookPrice" FieldName="BookPrice" Caption="Τιμή Βιβλίου (€)">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="GroupID" FieldName="GroupID" Caption="ID κατάστασης">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn FieldName="Institution" Name="Institution" Caption="Ίδρυμα">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Department" Name="Department" Caption="Τμήμα/Βιβλιοθήκη">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookCount" Name="BookCount" Caption="Αρ. Αντιτύπων" Width="50px" />
        <dx:GridViewDataTextColumn Name="PaymentPrice" FieldName="PaymentPrice" Caption="Τιμή Πληρωμής (€)">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="GroupState" FieldName="GroupState" Caption="Στάδιο Κατάστασης">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="SentAt" FieldName="SentAt" Caption="Ημερομηνία προς ΥΔΕ">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Year" FieldName="Year" Caption="Έτος">
        </dx:GridViewDataTextColumn> 
        <dx:GridViewDataTextColumn Name="PhaseID" FieldName="PhaseID" Caption="Φάση πληρωμών">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveBooks" runat="server" GridViewID="gvBooks" OnRenderBrick="gveBooks_RenderBrick" />