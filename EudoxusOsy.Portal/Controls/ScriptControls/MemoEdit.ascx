<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemoEdit.ascx.cs" Inherits="EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit" %>

<div id="<%= ClientID %>">
    <table id="tbMemo" runat="server" style="table-layout: fixed; overflow: hidden;">
        <tr>
            <td style="width: 100%;">
                <dx:ASPxMemo runat="server" ID="memoArea" Width="100%" Height="100%" AutoResizeWithContainer="false" >
                    <ValidationSettings RequiredField-IsRequired="false" RequiredField-ErrorText="Το πεδίο είναι υποχρεωτικό" ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                </dx:ASPxMemo>
            </td>
        </tr>
        <tr runat="server" id="counterRow" style="height: 20px;">
            <td style="padding-top: 6px">
                <span id="spCharCounter" runat="server"></span>
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hfClientState" runat="server" />
