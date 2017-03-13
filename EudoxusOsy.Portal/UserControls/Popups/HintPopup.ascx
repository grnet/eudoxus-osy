<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HintPopup.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.Popups.HintPopup" %>

<dx:ASPxPopupControl ID="dxHintPopup" runat="server" ClientInstanceName="dxHintPopup" ShowFooter="false" ShowHeader="false" Modal="false" Width="500" Height="20"
    PopupHorizontalAlign="OutsideRight" PopupVerticalAlign="TopSides" AllowDragging="false" CloseAction="CloseButton" PopupAction="None" 
    EnableHotTrack="false" PopupHorizontalOffset="5">
    <ContentStyle BackColor="#EEEEEE" ForeColor="#000000">
        <Paddings PaddingBottom="8px" PaddingLeft="10px" PaddingTop="8px" />
        <Border BorderWidth="0px" />
    </ContentStyle>
    <ContentCollection>
        <dx:PopupControlContentControl>
            <asp:Panel ID="popupPanel" runat="server" CssClass="info">
                <span id="dxHintPopupValue"></span>
            </asp:Panel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
