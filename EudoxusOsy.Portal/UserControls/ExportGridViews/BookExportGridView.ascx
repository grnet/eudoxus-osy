<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookExportGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.ExportGridViews.BookExportGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvBooks" runat="server" DataSourceForceStandardPaging="true">
    <Templates>
        <EmptyDataRow>
            Δεν υπάρχουν βιβλία
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Name="KpsID" Caption="Κωδικός Βιβλίου" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Title" FieldName="Title" Caption="Τίτλος Βιβλίου" />
        <dx:GridViewDataTextColumn Name="Subtitle" FieldName="Subtitle" Caption="Υπότιτλος" />
        <dx:GridViewDataTextColumn Name="PageCount" FieldName="PageCount" Caption="Αριθμός Σελίδων" />
        <dx:GridViewDataTextColumn Name="Author" FieldName="Author" Caption="Συγγραφέας" />
        <dx:GridViewDataTextColumn Name="ISBN" FieldName="ISBN" Caption="ISBN" />
        <dx:GridViewDataTextColumn Name="Publisher" FieldName="Publisher" Caption="Επωνυμία Εκδότη" />
        <dx:GridViewDataTextColumn FieldName="Department" Name="Department" Caption="Τμήμα" />
        <dx:GridViewDataTextColumn Name="BookPrice" FieldName="Price" Caption="Τιμή Κοστολόγησης (€)" />
        <dx:GridViewDataTextColumn Name="PaymentPrice" FieldName="PaymentPrice" Caption="Τιμή Πληρωμής (€)" />
        <dx:GridViewDataTextColumn FieldName="BookCount" Name="BookCount" Caption="Αρ. Αντιτύπων" Width="50px" />
        <dx:GridViewDataTextColumn FieldName="TotalAmount" Name="TotalAmount" Caption="Συνολικό Ποσό (χωρίς Φ.Π.Α.)" Width="50px" />
        <dx:GridViewDataTextColumn FieldName="BookTypeInt" Name="BookType" Caption="Τύπος Βιβλίου" >            
            <DataItemTemplate>
                <%# ((enBookType)Eval("BookType")).GetLabel() %>                
            </DataItemTemplate>    
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveBooks" runat="server" GridViewID="gvBooks" OnRenderBrick="gveBooks_RenderBrick" />
