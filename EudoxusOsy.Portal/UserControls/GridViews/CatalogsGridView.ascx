<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogsGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.CatalogsGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvCatalogs" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="Book.BookKpsID" Name="BookKpsID" Caption="Κωδικός Βιβλίου" Width="30px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# Eval("Book.BookKpsID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Book.Title" Name="BookTitle" Caption="Τίτλος Βιβλίου">
            <DataItemTemplate>
                <%# Eval("Book.Title") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="BookAuthor" FieldName="Book.Author" Caption="Συγγραφέας">
            <DataItemTemplate>
                <%# Eval("Book.Author") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Department.Institution.Name" Name="InstitutionName" Caption="Ίδρυμα">
            <DataItemTemplate>
                <%# CacheManager.Institutions.Get((int)Eval("Department.InstitutionID")).Name %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Department.Name" Name="DepartmentName" Caption="Τμήμα/Βιβλιοθήκη">
            <DataItemTemplate>
                <%# Eval("Department.Name") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="IsBookActive" Name="IsBookActive" Caption="Δυνατότητα τιμολόγησης και αποζημίωσης" Width="50px">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# ((bool)Eval("IsBookActive")) ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="BookCount" Name="BookCount" Caption="Αριθμός Αντιτύπων" Width="50px">
            <HeaderStyle HorizontalAlign="Right" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Amount" Name="Amount" Caption="Χρηματικό Ποσό (χωρίς Φ.Π.Α.)" Width="80px" PropertiesTextEdit-DisplayFormatString="C">
            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
            <CellStyle HorizontalAlign="Right" />
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveCatalogs" runat="server" GridViewID="gvCatalogs" OnRenderBrick="gveCatalogs_RenderBrick" />
