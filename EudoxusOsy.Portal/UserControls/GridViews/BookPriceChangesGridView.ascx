<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookPriceChangesGridView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.GridViews.BookPriceChangesGridView" %>
<%@ Import Namespace="EudoxusOsy.BusinessModel" %>

<dx:ASPxGridView ID="gvBookPriceChanges" runat="server" DataSourceForceStandardPaging="true">
    <Columns>                       
        <dx:GridViewDataTextColumn Name="BookKpsID" Caption="Kps ID" Settings-AllowSort ="False">        
         <DataItemTemplate>
                <%# ((BookPriceChange)Container.DataItem).Book.BookKpsID %>
            </DataItemTemplate>      
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Title" Caption="Τίτλος" Settings-AllowSort ="False" >
         <DataItemTemplate>
                <%# ((BookPriceChange)Container.DataItem).Book.Title %>
            </DataItemTemplate>      
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SuggestedPrice" Caption="Τιμή Εκδότη"  />
        <dx:GridViewDataTextColumn FieldName="Price" Caption="Τιμή Υπουργείου"  />
        <dx:GridViewDataTextColumn FieldName="DecisionNumber" Caption="Αριθμός απόφασης"  />
        <dx:GridViewDataTextColumn FieldName="PriceComments" Caption="Σχόλια"  />        
        <dx:GridViewDataTextColumn FieldName="PriceChecked" Caption="Ελέγχθηκε"  >                     
            <DataItemTemplate>                
                <%# ((BookPriceChange)Container.DataItem).PriceChecked ? "ΝΑΙ" : "ΟΧΙ"   %>
            </DataItemTemplate>      
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Approved" Caption="Έγκριση" Settings-AllowSort ="False">        
            <DataItemTemplate>                
                <%# ((BookPriceChange)Container.DataItem).IsApproved ? "ΝΑΙ" : "ΟΧΙ"   %>
            </DataItemTemplate>      
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="CreatedAt" Caption="Δημιουργήθηκε"  />                
    </Columns>
</dx:ASPxGridView>

