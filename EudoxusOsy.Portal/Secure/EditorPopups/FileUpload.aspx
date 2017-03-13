<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.FileUpload" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body style="padding: 0; margin: 0;">
    <form id="form1" runat="server" style="padding: 2px 6px; margin: 0;">
        <div style="float: left; padding-top: 3px; padding-right: 8px;">
            <dx:ASPxUploadControl ID="dxUploadControl" runat="server" Width="280px" OnFileUploadComplete="dxUploadControl_FileUploadComplete" 
                CancelButton-Text="Ακύρωση" ShowProgressPanel="True" BrowseButton-Text="Επιλέξτε..." UploadMode="Auto">
                <AdvancedModeSettings TemporaryFolder="D:\WebApps-Data\EudoxusOsy-App-Uploads\TempUploads" />
                <ValidationSettings MaxFileSizeErrorText="Το μέγεθος του αρχείου υπερβαίνει το μέγιστο επιτρεπόμενο, το οποίο είναι {0}." ShowErrors="false"
                    NotAllowedFileExtensionErrorText="Το αρχείο που επιλέξατε δεν είναι αποδεκτό. Το αρχείο θα πρέπει να έχει κατάληξη .pdf ή .zip" />
            </dx:ASPxUploadControl>
        </div>
        <div style="float: left;">
            <dx:ASPxButton ID="dxBtnUpload" Width="150" Image-Url="~/_img/iconPageAttach.png" runat="server" Text="Επισύναψη αρχείου" AutoPostBack="false" Height="25"></dx:ASPxButton>
        </div>
        <div style="clear: both;"></div>
    </form>
</body>
</html>
