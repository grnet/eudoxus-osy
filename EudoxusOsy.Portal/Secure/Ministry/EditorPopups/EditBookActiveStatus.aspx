<%@ Page Title="Επεξεργασία δυνατότητας τιμολόγησης και αποζημίωσης βιβλίου" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="EditBookActiveStatus.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.EditBookActiveStatus" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <table class="dv" style="width: 100%">
        <colgroup>
            <col style="width: 80px" />
        </colgroup>
        <tr>
            <th class="header" colspan="2">&raquo; Επεξεργασία δυνατότητας τιμολόγησης και αποζημίωσης βιβλίου
            </th>
        </tr>
        <tr>
            <th>Τιμολογείται:
            </th>
            <td>
                <dx:ASPxComboBox ID="ddlIsActive" runat="server" ValueType="System.Int32" OnInit="ddlIsActive_Init" Width="150px">
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Τιμολογείται' είναι υποχρεωτικό"  ValidationGroup="vgBook"/>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <th>Σχόλια:
            </th>
            <td>
                <dx:ASPxMemo ID="txtComments" runat="server" Rows="7" />
            </td>
        </tr>
    </table>

    <div class="summaryContainer">
        <dx:ASPxValidationSummary runat="server" ValidationGroup="vgBook" />
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" ValidationGroup="vgBook" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit(s, e) {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
