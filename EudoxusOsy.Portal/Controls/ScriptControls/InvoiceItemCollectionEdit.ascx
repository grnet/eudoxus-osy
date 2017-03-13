<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceItemCollectionEdit.ascx.cs" Inherits="EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit" %>

<%@ Register TagName="TipIcon" TagPrefix="my" Src="~/UserControls/GenericControls/TipIcon.ascx" %>

<br />
<fieldset class="fs-records">
    <div style="width: 100%;">
        <div style="margin-top: -5px; margin-bottom: 10px">
            <a id="lnkAddItem" runat="server" class="icon-btn bg-add" href="javascript:void(0);">Προσθήκη Εγγραφής</a>
            <span class="ctrlValArea">
                <asp:CustomValidator runat="server" ID="cvRequired" ErrorMessage="'{0}': Δεν έχετε δηλώσει καμία εγγραφή." Display="Dynamic" ForeColor="Red">Δεν έχετε δηλώσει καμία εγγραφή.</asp:CustomValidator>
            </span>
        </div>
    </div>
    <table width="100%" id="<%= ClientID %>" cellpadding="0" cellspacing="0" class="dv hideUpDownButtons">
        <thead>
            <tr>
                <th style="width: 2%; text-wrap: none;">Α/Α
                </th>
<%--                <th style="width: 5%;">
                    <img id="Img1" alt="" src="~/_img/s.gif" runat="server" style="height: 16px; width: 40px" />
                </th>--%>
                <th style="width: 20%;">Αριθμός Παραστατικού
                </th>
                <th style="width: 2%;">Ημερομηνία
                </th>
                <th style="width: 2%;">Ποσό (χωρίς ΦΠΑ):
                </th>
                <th style="width: 7%;">
                    <img id="Img2" alt="" src="~/_img/s.gif" runat="server" style="height: 16px; width: 40px" />
                </th>
            </tr>
            <tr class="emptyRow" style="display: none;">
                <td colspan="13">Δεν έχετε προσθέσει καμία εγγραφή, μπορείτε να προσθέσετε <a href="javascript:void(0);" id="lnkAddItemFooter" runat="server">πατώντας εδώ</a>.</td>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
    <asp:HiddenField runat="server" ID="hfClientState" />
</fieldset>
<br />

<dx:ASPxPopupControl ID="dxEditPopup" runat="server" CloseAction="CloseButton" HeaderText="Επεξεργασία Εγγραφής" PopupElementID="lnkAddItem" Width="500" Height="200"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="True" Modal="true" ShowFooter="true">
    <FooterTemplate>
        <div style="padding: 8px 12px;">
            <table style="float: right;">
                <tr>
                    <td>
                        <dx:ASPxButton ID="btnSubmit" runat="server" Image-Url="~/_img/iconAdd.png" Text="Καταχώριση & Κλείσιμο" />
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnCancel" runat="server" Image-Url="~/_img/iconCancel.png" Text="Ακύρωση" />
                    </td>
                </tr>
            </table>
            <div style="clear: both;"></div>
        </div>
    </FooterTemplate>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <table style="width: 100%;" class="dv">
                <colgroup>
                    <col style="width: 100px;" />
                    <col style="width: 140px;" />
                    <col style="width: 150px;" />
                    <col style="width: 140px;" />
                    <col style="width: 150px;" />
                    <col style="width: 100px;" />
                </colgroup>
                <tr>
                    <th>Αριθμός Παραστατικού:</th>
                    <td colspan="5">
                        <dx:ASPxTextBox ID="tbDescription" runat="server" Width="100%" MaxLength="200">
                            <ValidationSettings ValidationGroup="vgItems" RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Αριθμός Παραστατικού' είναι υποχρεωτικό" ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Ημερομηνία:</th>
                    <td>
                        <dx:ASPxDateEdit ID="seDate" runat="server" Width="100%" DisplayFormatString="dd/MM/yyyy" >
                             <ValidationSettings ValidationGroup="vgItems" RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ημερομηνία' είναι υποχρεωτικό" ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                        </dx:ASPxDateEdit>
                    </td>
                    <th>Ποσό (χωρίς ΦΠΑ):</th>
                    <td>
                        <dx:ASPxSpinEdit ID="seUnitPrice" runat="server" Width="100%" NumberType="Float" DisplayFormatString="C" MinValue="0" MaxValue="1000000">
                             <ValidationSettings ValidationGroup="vgItems" RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ποσό' είναι υποχρεωτικό" ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                        </dx:ASPxSpinEdit>
                    </td>
                </tr>
            </table>
            <div style="margin-left: 20px;">
                <asp:CustomValidator ID="cvFinancialChecks" runat="server" ValidationGroup="vgItems">&nbsp;</asp:CustomValidator>
                <asp:CustomValidator ID="cvCurrentAmount" runat="server" ValidationGroup="vgItems">&nbsp;</asp:CustomValidator>
                <dx:ASPxValidationSummary ID="vsItems" runat="server" ValidationGroup="vgItems" />
                <asp:ValidationSummary ID="vsItems2" runat="server" ValidationGroup="vgItems" ForeColor="Red" />
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
