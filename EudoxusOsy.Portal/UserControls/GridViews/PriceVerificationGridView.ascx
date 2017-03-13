<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceVerificationGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.PriceVerificationGridView" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<dx:ASPxGridView ID="gvPriceVerification" runat="server" DataSourceForceStandardPaging="true">
    <Columns>               
        <dx:GridViewDataTextColumn FieldName="ID" Caption="ID" Visible="False"  />        
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Caption="BookKpsID"  />        
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title"  />
        <dx:GridViewDataTextColumn Name="CreatedAt" Caption="Πότε ενημερώθηκε το ΟΣΥ ότι το Βιβλίο πήρε Τιμή Υπουργείου" Settings-AllowSort ="False">   
            <DataItemTemplate>
                <%# ((Book)Container.DataItem).LastPriceChange == null ? string.Empty: ((Book)Container.DataItem).LastPriceChange.CreatedAt.ToShortDateString() %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="SuggestedPrice" Caption="Τιμή Εκδότη" Settings-AllowSort ="False">
            <DataItemTemplate>
                <%#  ((Book)Container.DataItem).LastPriceChange == null ? string.Empty: ((Book)Container.DataItem).LastPriceChange.SuggestedPrice.ToString() %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Price" Caption="Τιμή Υπουργείου"  Settings-AllowSort ="False">
            <DataItemTemplate>
                <%# ((Book)Container.DataItem).LastPriceChange == null ? string.Empty: ((Book)Container.DataItem).LastPriceChange.Price.ToString() %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="PendingCommitteePriceVerification" Caption="Υπό έλεγχο"  Settings-AllowSort ="False">                
            <DataItemTemplate>
                <%# ((Book)Container.DataItem).HasPendingPriceVerification ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="UnexpectedPriceVerified" Caption="Υπό έλεγχο"  Settings-AllowSort ="False">                
            <DataItemTemplate>
                <%# ((Book)Container.DataItem).HasUnexpectedPriceChange ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>

<!--<dx:GridViewDataTextColumn FieldName="" Caption="Φάση (περίοδος διανομής) όπου γίνεται ο έλεγχος του Υπουργείου"  />-->