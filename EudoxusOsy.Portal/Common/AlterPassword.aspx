<%@ Page Title="Αλλαγή Κωδικού Πρόσβασης" MasterPageFile="~/PopUp.Master" Language="C#" AutoEventWireup="true" CodeBehind="AlterPassword.aspx.cs" Inherits="EudoxusOsy.Portal.Common.AlterPassword" %>

<%@ Register TagName="ChangePassword" TagPrefix="my" Src="~/UserControls/GenericControls/ChangePassword.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <my:ChangePassword ID="ucChangePassword" runat="server" />
    
    <br />
    
    <div class="summaryContainer">
        <dx:ASPxValidationSummary  runat="server" ClientInstanceName="validationSummary" RenderMode="BulletedList" ShowErrorsInEditors="true" />
    </div>

    <dx:ASPxLabel ID="lblErrors" runat="server" ForeColor="Red" Text="Ο Παλιός Κωδικός Πρόσβασης δεν είναι έγκυρος" Visible="false" />

    <dx:ASPxButton  runat="server" ClientInstanceName="btnSubmit" ClientVisible="false" Image-Url="~/_img/iconAccept.png" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        function doSubmit() {
            btnSubmit.DoClick();
        }
    </script>

</asp:Content>
