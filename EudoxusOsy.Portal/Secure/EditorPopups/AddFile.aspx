<%@ Page Title="Προσθήκη Αρχείου" MasterPageFile="~/PopUp.Master" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.AddFile" CodeBehind="AddFile.aspx.cs" Language="C#" AutoEventWireup="true" %>

<%@ Register TagName="FileUpload" TagPrefix="my" Src="~/Controls/ScriptControls/FileUpload.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">    

    <div>
        <table class="gv">
            <tr>
                <th>Φάση Πληρωμών: </th>
                <td>
                    <%--<dx:ASPxSpinEdit runat="server" ID="spinPhaseID"></dx:ASPxSpinEdit>--%>
                    <dx:ASPxComboBox runat="server" ID="ddlSelectPhase" OnInit="ddlSelectPhase_Init"></dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <th>Επιλεγμένο Αρχείο: </th>
                <td>
                    <my:FileUpload FieldName="Δικαιολογητικό" runat="server" ID="ucFileUpload" />
                </td>
            </tr>
        </table>
    </div>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
