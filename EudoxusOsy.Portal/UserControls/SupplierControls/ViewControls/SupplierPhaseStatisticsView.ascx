<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierPhaseStatisticsView.ascx.cs" Inherits="EudoxusOsy.Portal.UserControls.SupplierControls.ViewControls.SupplierPhaseStatisticsView" %>

<%@ Register TagName="TipIcon" TagPrefix="my" Src="~/UserControls/GenericControls/TipIcon.ascx" %>

<table runat="server" id="tableStatistics" class="dv">
    <colgroup>
        <col style="width: 115px" />
        <col style="width: 170px" />
        <col style="width: 200px" />
        <col style="width: 200px" />
    </colgroup>
    <tr>
        <th style="text-align: right;">Οφειλή φάσης
            <my:TipIcon runat="server" Text="Tο ποσό αποζημίωσης του εκδότη για το σύνολο των διανομών της τρέχουσας περιόδου πληρωμών" />
        </th>
        <th style="text-align: right;">Ανάθεση ποσού φάσης
            <my:TipIcon runat="server" Text="Το συνολικό ποσό που έχει μέχρι στιγμής τεθεί στη διάθεση του εκδότη από το Υπουργείο για την αποπληρωμή των οφειλών που προκύπτουν από διανομές στην επιλεγμένη φάση." />
        </th>
        <th style="text-align: right;">Διαθέσιμο ποσό (χωρίς ΦΠΑ)
            <my:TipIcon runat="server" Text="Το συνολικό ποσό που απομένει προς 'Επιλογή για πληρωμή' από τον εκδότη για την τρέχουσα περίοδο πληρωμών. Προκύπτει ως η διαφορά της 'ανάθεσης ποσού φάσης' από το άθροισμα του κόστους των καταστάσεων (της επιλεγμένης φάσης) οι οποίες βρίσκονται σε κατάσταση 'Επιλεχθείσα για πληρωμή' ή μεταγενέστερη." />
        </th>
        <th style="text-align: right;">Εισπραχθέν ποσό (χωρίς ΦΠΑ)
            <my:TipIcon runat="server" Text="Το άθροισμα του κόστους των καταστάσεων οι οποίες βρίσκονται σε κατάσταση 'Σταλμένη προς ΥΔΕ' (στην επιλεγμένη φάση)" />
        </th>
    </tr>
    <tr>
        <td style="text-align: right;"><span id="spanOwedAmount" runat="server"></span></td>
        <td style="text-align: right;"><span id="spanAllocatedAmount" runat="server"></span></td>
        <td style="text-align: right;"><span id="spanRemainingAmount" runat="server"></span></td>
        <td style="text-align: right;"><span id="spanPaidAmount" runat="server"></span></td>
    </tr>
</table>
