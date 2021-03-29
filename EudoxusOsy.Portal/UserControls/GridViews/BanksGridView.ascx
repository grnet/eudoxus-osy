<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BanksGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.BanksGridView" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>
<%@ Import Namespace="EudoxusOsy.Portal" %>

<dx:ASPxGridView ID="gvBanks" runat="server" DataSourceForceStandardPaging="true">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="ID" Caption="ID" Width="20px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Όνομα" />
        <dx:GridViewDataTextColumn FieldName="IsBank" Name="IsBank" Caption="Είναι Τράπεζα" Width="70px">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
            <DataItemTemplate>
                <%# ((bool)Eval("IsBank")) ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<dx:ASPxGridViewExporter ID="gveBanks" runat="server" GridViewID="gvBanks" OnRenderBrick="gveBanks_RenderBrick" />
