<%@ Control Inherits="EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo" CodeBehind="MultiSelectCombo.ascx.cs" Language="C#" AutoEventWireup="true" %>

<div id="<%= ClientID %>">
    <dx:ASPxDropDownEdit ID="ddxDropDownEdit" runat="server" Width="100%" ClientEnabled="true">
        <DropDownWindowStyle BackColor="#EDEDED" />
        <DropDownWindowTemplate>
            <dx:ASPxListBox ID="ddlListBox" runat="server" ValueType="System.Int32" SelectionMode="CheckColumn" ClientEnabled="true" Width="100%">
                <Border BorderStyle="None" />
                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
            </dx:ASPxListBox>
        </DropDownWindowTemplate>
        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" EnableCustomValidation="true" />
    </dx:ASPxDropDownEdit>
    <asp:HiddenField ID="hfClientState" runat="server" />
</div>
