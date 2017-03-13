<%@ Page Title="Προβολή Παραστατικών" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ManageFiles.aspx.cs" Inherits="EudoxusOsy.Portal.Secure.EditorPopups.ManageFiles" %>

<%@ Register TagName="FileSelfPublisherGridView" TagPrefix="my" Src="~/UserControls/GridViews/FileSelfPublisherGridView.ascx" %>

<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="gridViewTopButtons">
                <dx:ASPxButton ID="btnAddFile" runat="server" Text="Προσθήκη Αρχείου" Image-Url="~/_img/iconAddNewItem.png" />
            </div>

            <table class="dv">
                <tr>
                    <th>
                        Επιλέξτε περίοδο πληρωμών
                    </th>
                    <td>
                        <dx:ASPxComboBox runat="server" ID="cmbPhaseID" OnInit="cmbPhaseID_Init"></dx:ASPxComboBox>
                    </td>
                    <td>
                        <dx:ASPxButton runat="server" ID="btnSearch" OnClick="btnSearch_Click" CssClass="img-btn" Image-Url="~/_img/iconView.png" Text="Αναζήτηση"></dx:ASPxButton>
                    </td>

                </tr>
            </table>
            <div class="br"></div>
            <my:FileSelfPublisherGridView ID="gvFiles" runat="server" DataSourceID="odsFiles" OnCustomCallback="gvFiles_CustomCallback">
                <Columns>
                    <dx:GridViewDataTextColumn Name="Actions" Caption="Ενέργειες" Width="70px" VisibleIndex="5">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" href="javascript:void(0);" class="tooltip" title="Διαγραφή Αρχείου"
                                onclick='<%# string.Format("deleteFile({0});", Eval("ID")) %>'>
                                <img src="/_img/iconDelete.png" alt="Διαγραφή Αρχείου" /></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </my:FileSelfPublisherGridView>

            <div style="display: none">
                <asp:Button ID="btnDeleteFile" runat="server" OnClick="btnDeleteFile_Click" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" />
                <input type="hidden" id="hfFileID" runat="server" />
            </div>

            <asp:ObjectDataSource ID="odsFiles" runat="server" TypeName="EudoxusOsy.Portal.DataSources.FileSelfPublishers"
                SelectMethod="FindInfoWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true"  OnSelecting="odsFiles_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="criteria" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function deleteFile(fileID) {
            var doDelete = function () {
                $('#<%= hfFileID.ClientID %>').val(fileID);
                <%= ClientScript.GetPostBackEventReference(btnDeleteFile, null) %>
            };

            showConfirmBox('Διαγραφή Αρχείου', 'Είστε σίγουροι ότι θέλετε να διαγράψετε το αρχείο;', doDelete);
        }

        function cmdRefresh() {
            <%= ClientScript.GetPostBackEventReference(btnRefresh, null) %>
        }
    </script>
</asp:Content>
