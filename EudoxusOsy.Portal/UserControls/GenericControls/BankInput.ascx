<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BankInput.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GenericControls.BankInput" %>

<table class="dv" style="width: 100%">
    <colgroup>
        <col style="width: 100px" />        
    </colgroup>
    <tr>
        <th class="header" colspan="2">&raquo; Στοιχεία Τράπεζας
        </th>
    </tr>
    <tr>
        <th>Όνομα:
        </th>
        <td>            
            <dx:ASPxTextBox ID="txtName" runat="server" Width="400px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Όνομα' είναι υποχρεωτικό." />
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <th>Είναι Τράπεζα:
        </th>
        <td>
            <dx:ASPxComboBox ID="ddlIsBank" runat="server" ValueType="System.Int32" OnInit="ddlIsBank_Init" Width="150px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο 'Είναι Τράπεζα' είναι υποχρεωτικό" />
            </dx:ASPxComboBox>
        </td>
    </tr>    
</table>