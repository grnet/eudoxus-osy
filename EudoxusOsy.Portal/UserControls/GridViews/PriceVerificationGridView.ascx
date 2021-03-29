<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceVerificationGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.PriceVerificationGridView" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<dx:ASPxGridView ID="gvPriceVerification" runat="server" DataSourceForceStandardPaging="true">
    <Columns>                       
        <dx:GridViewDataTextColumn FieldName="BookPriceID" Caption="BookPriceID" Visible="False"  />        
        <dx:GridViewDataTextColumn FieldName="BookID" Caption="BookID" Visible="False"  />        
        <dx:GridViewDataTextColumn FieldName="BookKpsID" Caption="Κωδικός Βιβλίου"  />        
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Τίτλος Βιβλίου"  />
        <dx:GridViewDataTextColumn FieldName="CreatedAt" Caption="Ενημέρωση τιμής στο ΠΣ Ευδόξου" />                                                   
        <dx:GridViewDataTextColumn FieldName="SuggestedPrice" Caption="Προτεινόμενη τιμή Εκδότη" />                    
        <dx:GridViewDataTextColumn FieldName="Price" Caption="Εγκεκριμένη τιμή Υπουργείου"  />                    
        <dx:GridViewDataTextColumn Name="PendingCommitteePriceVerification" Caption="Υπό έλεγχο"  >                                    
            <DataItemTemplate>
                <%# ((BookPricesGridV)Container.DataItem).HasPendingPriceVerification ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="UnexpectedPriceVerified" Caption="Μη αναμενόμενη αλλαγή τιμής"  >                                    
            <DataItemTemplate>
                <%# ((BookPricesGridV)Container.DataItem).HasUnexpectedPriceChange ? "ΝΑΙ" : "ΟΧΙ" %>
            </DataItemTemplate>                                  
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ChangeYear" Caption="Έτος"  />    
    </Columns>
</dx:ASPxGridView>


<dx:ASPxGridViewExporter ID="gvePriceVerification" runat="server" GridViewID="gvPriceVerification" OnRenderBrick="gvePriceVerification_OnRenderBrick" />
<!--<dx:GridViewDataTextColumn FieldName="" Caption="Φάση (περίοδος διανομής) όπου γίνεται ο έλεγχος του Υπουργείου"  />-->