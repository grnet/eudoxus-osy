<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="EditCatalogPopup.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.Ministry.EditorPopups.EditCatalogPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">    
    <div class="br"></div>
    <table>
        <tr runat="server" id="trBookCount" >
            <th>Αριθμός Αντιτύπων: </th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtBookCount" ClientInstanceName="txtBookCount" MinValue="0" NumberType="Integer">                    
                </dx:ASPxSpinEdit>
            </td>
        </tr>        
         <tr runat="server" id="trAmount" Visible="False">
            <th>Κόστος Διανομής: </th>
            <td>
                <dx:ASPxSpinEdit runat="server" ID="txtAmount" ClientInstanceName="txtAmount">                    
                </dx:ASPxSpinEdit>
            </td>
        </tr>
    </table>
    
    <asp:CustomValidator runat="server" ID="cvCatalogEdit" 
        ForeColor="Red" Display="Dynamic" ClientValidationFunction="checkInputData">        
    </asp:CustomValidator>

    <dx:ASPxButton runat="server" ID="btnSubmitHidden" ClientInstanceName="btnSubmitHidden" ClientVisible="false" 
       OnClick="btnSubmitHidden_Click" ValidateInvisibleEditors="false" />

    <script type="text/javascript">
        function doSubmit(s, e) {

            var customValidatorID = '<%= cvCatalogEdit.ClientID %>';                   

            // Get the specific validator element
            var validator = document.getElementById(customValidatorID);

            // Validate chosen validator
            ValidatorValidate(validator);

            if (validator.isvalid) {
                btnSubmitHidden.DoClick();               
            }            
        }

        function checkInputData(s, e) {

            var bookCountValid = false;

            if (typeof txtBookCount !== "undefined" && ASPxClientUtils.IsExists(txtBookCount)) {
                bookCountValid = txtBookCount.GetText().trim() !== "";
            }
                       
            if (typeof txtAmount !== "undefined" && ASPxClientUtils.IsExists(txtAmount))
            {                
                var amountValid = txtAmount.GetText().trim() !== "";

                s.innerText = "Θα πρέπει να συμπληρώσετε το πεδίο.";
                //e.IsValid = (bookCountValid && !amountValid) || (!bookCountValid && amountValid);
                e.IsValid = amountValid;
            } else {                
                s.innerText = "Το πεδίο Αριθμός Αντιτύπων είναι υποχρεωτικό";
                e.IsValid = bookCountValid;
            }
        }
    </script>

</asp:Content>
