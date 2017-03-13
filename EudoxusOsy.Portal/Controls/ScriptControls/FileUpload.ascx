<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.ascx.cs" Inherits="EudoxusOsy.Portal.Controls.ScriptControls.FileUpload" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div id="<%= ClientID %>">
    <table>
        <tr>
            <td>
                <div id="uploadRow" runat="server">
                    <div style="float: left; padding-top: 5px; padding-right: 8px;">
                        <span style="width: 285px; display: block; white-space: nowrap; text-overflow: ellipsis;">[ Δεν έχει γίνει επισύναψη αρχείου ]</span>
                    </div>
                    <div style="float: left;">
                        <dx:ASPxButton ID="dxBtnUpload" runat="server" Width="150" Image-Url="~/_img/iconPageAttach.png" Text="Προσθήκη αρχείου" AutoPostBack="false"></dx:ASPxButton>
                    </div>
                </div>
                <div id="downloadRow" runat="server">
                    <div style="float: left; padding-top: 5px; padding-right: 8px;">
                        <a href="javascript:void(0);" id="lnkDownload" class="tooltip" style="width: 285px; display: block; white-space: nowrap; text-overflow: ellipsis;" runat="server"></a>
                    </div>
                    <div style="float: left;">
                        <dx:ASPxButton ID="dxBtnDelete" runat="server" Width="150" Image-Url="~/_img/iconPageDelete.png" Text="Διαγραφή αρχείου" AutoPostBack="false"></dx:ASPxButton>
                    </div>
                </div>
                <div style="clear: both;"></div>
            </td>
            <td style="padding-right: 5px; padding-left: 5px;">
                <asp:CustomValidator runat="server" ID="cvFileUpload" ErrorMessage="Το πεδίο είναι υποχρεωτικό" ForeColor="Red" Display="Dynamic">
                    <img src="~/_img/dxError.png" runat="server" alt="Το πεδίο είναι υποχρεωτικό." title="Το πεδίο είναι υποχρεωτικό" />
                </asp:CustomValidator>
            </td>
        </tr>
    </table>
</div>
<iframe runat="server" id="ifrDownload" style="display: none;" src="about:blank" />
<asp:HiddenField runat="server" ID="hfFileID" />


<dx:ASPxPopupControl ID="popupFileUpload" runat="server" AllowDragging="true" AllowResize="false" CloseAction="CloseButton"
    PopupAction="None" PopupHorizontalOffset="-130" PopupHorizontalAlign="RightSides" PopupVerticalAlign="Below" ShowFooter="false"
    Width="500px" Height="50px" HeaderText="Επισύναψη αρχείου" AppearAfter="0" ContentUrl="about:blank">
    <ClientSideEvents CloseButtonClick="function (s, e) { s.SetContentUrl('about:blank'); s.Hide(); }" />
</dx:ASPxPopupControl>
