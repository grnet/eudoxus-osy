<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookPricesVGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.BookPricesVGridView" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<dx:ASPxGridView ID="gvPriceChangeV" runat="server" DataSourceForceStandardPaging="true">
    <Columns>               
        <dx:GridViewDataTextColumn FieldName="ID" Caption="ID" Visible="False"  />        
        <dx:GridViewDataTextColumn FieldName="BookPriceChangeID" Caption="BookPriceChangeID" Visible="False"  />        
        <dx:GridViewDataTextColumn FieldName="ChangeYear" Caption="ChangeYear" Visible="False"  />        
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Caption="Κωδικός Βιβλίου"  />        
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Τίτλος Βιβλίου"  />
        <dx:GridViewDataTextColumn Name="CreatedAt" Caption="Ενημέρωση τιμής στο ΠΣ Ευδόξου"/>                                              
        <dx:GridViewDataTextColumn Name="SuggestedPrice" Caption="Προτεινόμενη τιμή Εκδότη" >
            <DataItemTemplate>
                <%#  ((BookPricesV)Container.DataItem).SuggestedPrice.ToString() %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Price" Caption="Εγκεκριμένη τιμή Υπουργείου">
            <DataItemTemplate>
                <%# ((BookPricesV)Container.DataItem).Price == null || 
                        (((BookPricesV)Container.DataItem).Price == 0m && !((BookPricesV)Container.DataItem).PriceChecked)
                        ? string.Empty: ((BookPricesV)Container.DataItem).Price.ToString() %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="PendingCommitteePriceVerification" Caption="Υπό έλεγχο" >                
            <DataItemTemplate>
                <%# ((BookPricesV)Container.DataItem).HasPendingPriceVerification ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="UnexpectedPriceVerified" Caption="Μη αναμενόμενη αλλαγή τιμής">                
            <DataItemTemplate>
                <%# ((Book)Container.DataItem).HasUnexpectedPriceChange ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>