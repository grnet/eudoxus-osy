<%@ Page Title="Αποστολή προς ΥΔΕ" MasterPageFile="~/PopUp.Master" CodeBehind="SendToYDE.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.SendToYDE" Language="C#" AutoEventWireup="true" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <table class="dv" style="width: 100%">
        <colgroup>
            <col style="width: 130px" />
        </colgroup>
        <tr>
            <th class="header" colspan="2">&raquo; Στοιχεία Αποστολής προς ΥΔΕ
            </th>
        </tr>
        <tr>
            <th>Ημ/νία Αποστολής:
            </th>
            <td>
                <dx:ASPxDateEdit ID="txtSentDate" runat="server" Width="100px">
                    <ValidationSettings ValidateOnLeave="true" RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Ημ/νία Αποστολής' είναι υποχρεωτικό" ValidationGroup="vgSentDate" ErrorDisplayMode="ImageWithTooltip" />
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <th>Σχόλια:
            </th>
            <td>
                <dx:ASPxMemo ID="txtSentComments" runat="server" Rows="7" />
            </td>
        </tr>
    </table>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" CausesValidation="true" OnClick="btnSubmit_Click" ValidationGroup="vgSentDate" />

    <script type="text/javascript">
        var clickYDECounter = 0;

        function doSubmit(s, e) {
            ASPxClientEdit.ValidateGroup("vgSentDate");
            if (ASPxClientEdit.AreEditorsValid() && clickYDECounter ==0) {
                clickYDECounter = 1;
                btnSubmit.DoClick();
            }
            else
            {
                return false;
            }
        }
    </script>

</asp:Content>
