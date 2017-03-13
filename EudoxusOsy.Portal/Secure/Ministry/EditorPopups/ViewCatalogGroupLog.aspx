<%@ Page Title="Ιστορικό Μεταβολών" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.ViewCatalogGroupLog" CodeBehind="ViewCatalogGroupLog.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <dx:ASPxGridView ID="gvCatalogGroupLogs" runat="server" DataSourceID="odsCatalogGroupLogs" DataSourceForceStandardPaging="true">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="OfficeSlipNumber" Caption="Αριθμός υπηρεσιακού σημειώματος">
                <HeaderStyle Wrap="True" HorizontalAlign="Center" />
                <DataItemTemplate>
                    <%# ((CatalogGroupLog)Container.DataItem).OfficeSlipNumber != null ? ((CatalogGroupLog)Container.DataItem).OfficeSlipNumber.ToString().Substring(4) + "/" + ((CatalogGroupLog)Container.DataItem).OfficeSlipNumber.ToString().Substring(0,4) : string.Empty  %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OfficeSlipDate" Caption="Ημερομηνία υπηρεσιακού σημειώματος">
                <HeaderStyle Wrap="True" HorizontalAlign="Center" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Comments" Caption="Αιτιολογία μεταβολής">
                <HeaderStyle Wrap="True" HorizontalAlign="Center" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="NewState" Caption="Νέο Στάδιο κατάστασης">
                <DataItemTemplate>
                    <%# ((CatalogGroupLog)Container.DataItem).NewState.GetLabel() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Amount" Caption="Ποσό πληρωμής" PropertiesTextEdit-DisplayFormatString="c" />
            <dx:GridViewDataTextColumn FieldName="CreatedAt" Caption="Ημ/νία Μεταβολής" />
            <dx:GridViewDataTextColumn FieldName="CreatedBy" Caption="Υπεύθυνος Μεταβολής">
                <HeaderStyle Wrap="True" HorizontalAlign="Center" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PdfNotes" Caption="Σχόλια pdf">
                <CellStyle Wrap="True" HorizontalAlign="Center"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Λήψη PDF" Name="PdfLink">
                <CellStyle Wrap="True" HorizontalAlign="Center"></CellStyle>
                <DataItemTemplate>
                    <a runat="server" href='<%# string.Format("~/Secure/GenerateCatalogPDFFromCatalogLog.ashx?id={0}",((CatalogGroupLog)Container.DataItem).ID) %>'
                        visible='<%# ((CatalogGroupLog)Container.DataItem).Comments == enCatalogGroupLogAction.PDFPrint.GetLabel() %>'><img src="../../../_img/iconPdf.gif" alt="pdfFile" /></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
    <asp:ObjectDataSource ID="odsCatalogGroupLogs" runat="server" TypeName="EudoxusOsy.Portal.DataSources.CatalogGroupLogs"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsCatalogGroupLogs_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
