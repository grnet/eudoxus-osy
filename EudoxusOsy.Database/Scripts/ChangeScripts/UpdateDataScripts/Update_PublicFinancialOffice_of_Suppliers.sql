ALTER TABLE Supplier 
ADD PfoID int null,
PaymentPfoID int null

GO

Update Supplier
Set PfoID = pfo.ID
from Supplier s
inner join PublicFinancialOffice pfo on REPLACE(pfo.Name,'''','') = REPLACE(s.pfo,'΄','')

GO

Update Supplier
Set PaymentPfoID = pfo.ID
from Supplier s
inner join PublicFinancialOffice pfo on pfo.Name = s.PaymentPfo

GO

Update Supplier 
SET pfo = 
	CASE WHEN pfo = 'Ν. ΗΡΑΚΛΕΙΟΥ' THEN 'ΗΡΑΚΛΕΙΟΥ ΑΤΤΙΚΗΣ' 
		WHEN pfo = 'ΑΘΗΝΩΝ Δ΄ (Δ΄,Η΄)'  THEN 'ΑΘΗΝΩΝ Δ'''
		WHEN pfo = 'Ν. ΣΜΥΡΝΗΣ' THEN 'ΝΕΑΣ ΣΜΥΡΝΗΣ'
		WHEN pfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ Β΄ (Β΄, Γ΄)' THEN 'ΘΕΣΣΑΛΟΝΙΚΗΣ Β'''
		WHEN pfo = 'ΑΘΗΝΩΝ Α΄(Α΄, Β΄, Γ΄)' THEN 'ΑΘΗΝΩΝ Α'''
		WHEN pfo = 'ΑΘΗΝΩΝ Φ.Α.Ε.' THEN 'ΑΘΗΝΩΝ ΦΑΕΕ'
		WHEN pfo = 'ΑΘΗΝΩΝ Ε΄(Ε΄,Θ΄)' THEN 'ΑΘΗΝΩΝ Ε'''
		WHEN pfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ ΤΟΥΜΠΑΣ' THEN 'ΤΟΥΜΠΑΣ'
		WHEN pfo = 'ΠΑΛ. ΦΑΛΗΡΟΥ' THEN 'ΠΑΛΑΙΟΥ ΦΑΛΗΡΟΥ'
		WHEN pfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ ΑΜΠΕΛΟΚΗΠΩΝ' THEN 'ΑΜΠΕΛΟΚΗΠΩΝ'
		WHEN pfo = 'ΚΟΖΑΝΗΣ' THEN 'ΚΟΖΑΝΗ'
		WHEN pfo = 'Ν. ΙΩΝΙΑΣ' THEN 'ΝΕΑΣ ΙΩΝΙΑΣ'
		WHEN pfo = 'ΑΓΙΟΥ ΔΗΜΗΤΡΙΟΥ' THEN 'ΑΓ. ΔΗΜΗΤΡΙΟΥ'
		WHEN pfo = 'ΑΘΗΝΩΝ ΣΤ΄ (ΣΤ΄,Ζ΄)' THEN 'ΑΘΗΝΩΝ ΣΤ'''
		WHEN pfo = 'ΠΕΙΡΑΙΑ Α΄ (Α΄,Β΄)' THEN 'ΠΕΙΡΑΙΑ Α'''
		WHEN pfo = 'ΙΩΝΙΑΣ ΘΕΣ/ΝΙΚΗΣ' THEN 'Ν.ΙΩΝΙΑΣ ΘΕΣ/ΚΗΣ'
		WHEN pfo = 'ΕΞΩΤΕΡΙΚΟΥ' THEN 'ΚΑΤΟΙΚΩΝ ΕΞΩΤΕΡΙΚΟΥ'
		WHEN pfo = 'Ν. ΦΙΛΑΔΕΛΦΕΙΑΣ' THEN 'ΝΕΑΣ ΦΙΛΑΔΕΛΦΕΙΑΣ'
		WHEN pfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ ΝΕΑΠΟΛΗΣ' THEN 'ΝΕΑΠΟΛΗΣ ΘΕΣ/ΝΙΚΗΣ'
		WHEN pfo = 'ΧΑΪΔΑΡΙΟΥ' THEN 'ΧΑΙΔΑΡΙΟΥ'
		WHEN pfo = 'ΑΓΙΟΥ ΑΘΑΝΑΣΙΟΥ' THEN 'ΑΓ. ΑΘΑΝΑΣΙΟΥ'
		WHEN pfo = 'ΛΙΜΕΝΟΣ ΧΕΡΣΟΝΗΣΟΥ' THEN 'ΛΙΜΕΝΑ ΧΕΡΣΟΝΗΣΟΥ'
		WHEN pfo = 'ΦΑΕ ΑΘΗΝΩΝ' THEN 'ΑΘΗΝΩΝ ΦΑΕΕ'
		WHEN pfo = 'Φ.Α.Ε. ΑΘΗΝΩΝ' THEN 'ΑΘΗΝΩΝ ΦΑΕΕ'
		WHEN pfo = 'ΣΤ Αθήνας' THEN 'ΑΘΗΝΩΝ ΣΤ'''
		WHEN pfo = 'Ν. ΙΩΝΙΑΣ ΜΑΓΝΗΣΙΑΣ' THEN 'ΙΩΝΙΑΣ ΜΑΓΝΗΣΙΑΣ'
ELSE pfo END

GO


Update Supplier 
SET PaymentPfo = 
	CASE WHEN PaymentPfo = 'Ν. ΗΡΑΚΛΕΙΟΥ' THEN 'ΗΡΑΚΛΕΙΟΥ ΑΤΤΙΚΗΣ' 
		WHEN PaymentPfo = 'ΑΘΗΝΩΝ Δ΄ (Δ΄,Η΄)'  THEN 'ΑΘΗΝΩΝ Δ'''
		WHEN PaymentPfo = 'Ν. ΣΜΥΡΝΗΣ' THEN 'ΝΕΑΣ ΣΜΥΡΝΗΣ'
		WHEN PaymentPfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ Β΄ (Β΄, Γ΄)' THEN 'ΘΕΣΣΑΛΟΝΙΚΗΣ Β'''
		WHEN PaymentPfo = 'ΑΘΗΝΩΝ Α΄(Α΄, Β΄, Γ΄)' THEN 'ΑΘΗΝΩΝ Α'''
		WHEN PaymentPfo = 'ΑΘΗΝΩΝ Φ.Α.Ε.' THEN 'ΑΘΗΝΩΝ ΦΑΕΕ'
		WHEN PaymentPfo = 'ΑΘΗΝΩΝ Ε΄(Ε΄,Θ΄)' THEN 'ΑΘΗΝΩΝ Ε'''
		WHEN PaymentPfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ ΤΟΥΜΠΑΣ' THEN 'ΤΟΥΜΠΑΣ'
		WHEN PaymentPfo = 'ΠΑΛ. ΦΑΛΗΡΟΥ' THEN 'ΠΑΛΑΙΟΥ ΦΑΛΗΡΟΥ'
		WHEN PaymentPfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ ΑΜΠΕΛΟΚΗΠΩΝ' THEN 'ΑΜΠΕΛΟΚΗΠΩΝ'
		WHEN PaymentPfo = 'ΚΟΖΑΝΗΣ' THEN 'ΚΟΖΑΝΗ'
		WHEN PaymentPfo = 'Ν. ΙΩΝΙΑΣ' THEN 'ΝΕΑΣ ΙΩΝΙΑΣ'
		WHEN PaymentPfo = 'ΑΓΙΟΥ ΔΗΜΗΤΡΙΟΥ' THEN 'ΑΓ. ΔΗΜΗΤΡΙΟΥ'
		WHEN PaymentPfo = 'ΑΘΗΝΩΝ ΣΤ΄ (ΣΤ΄,Ζ΄)' THEN 'ΑΘΗΝΩΝ ΣΤ'''
		WHEN PaymentPfo = 'ΠΕΙΡΑΙΑ Α΄ (Α΄,Β΄)' THEN 'ΠΕΙΡΑΙΑ Α'''
		WHEN PaymentPfo = 'ΙΩΝΙΑΣ ΘΕΣ/ΝΙΚΗΣ' THEN 'Ν.ΙΩΝΙΑΣ ΘΕΣ/ΚΗΣ'
		WHEN PaymentPfo = 'ΕΞΩΤΕΡΙΚΟΥ' THEN 'ΚΑΤΟΙΚΩΝ ΕΞΩΤΕΡΙΚΟΥ'
		WHEN PaymentPfo = 'Ν. ΦΙΛΑΔΕΛΦΕΙΑΣ' THEN 'ΝΕΑΣ ΦΙΛΑΔΕΛΦΕΙΑΣ'
		WHEN PaymentPfo = 'ΘΕΣΣΑΛΟΝΙΚΗΣ ΝΕΑΠΟΛΗΣ' THEN 'ΝΕΑΠΟΛΗΣ ΘΕΣ/ΝΙΚΗΣ'
		WHEN PaymentPfo = 'ΧΑΪΔΑΡΙΟΥ' THEN 'ΧΑΙΔΑΡΙΟΥ'
		WHEN PaymentPfo = 'ΑΓΙΟΥ ΑΘΑΝΑΣΙΟΥ' THEN 'ΑΓ. ΑΘΑΝΑΣΙΟΥ'
		WHEN PaymentPfo = 'ΛΙΜΕΝΟΣ ΧΕΡΣΟΝΗΣΟΥ' THEN 'ΛΙΜΕΝΑ ΧΕΡΣΟΝΗΣΟΥ'
		WHEN PaymentPfo = 'ΦΑΕ ΑΘΗΝΩΝ' THEN 'ΑΘΗΝΩΝ ΦΑΕΕ'
		WHEN PaymentPfo = 'Φ.Α.Ε. ΑΘΗΝΩΝ' THEN 'ΑΘΗΝΩΝ ΦΑΕΕ'
		WHEN PaymentPfo = 'ΣΤ Αθήνας' THEN 'ΑΘΗΝΩΝ ΣΤ'''
		WHEN PaymentPfo = 'Ν. ΙΩΝΙΑΣ ΜΑΓΝΗΣΙΑΣ' THEN 'ΙΩΝΙΑΣ ΜΑΓΝΗΣΙΑΣ'
ELSE PaymentPfo END


GO


Update Supplier
Set PfoID = pfo.ID
from Supplier s
inner join PublicFinancialOffice pfo on REPLACE(pfo.Name,'''','') = REPLACE(s.pfo,'''','')

GO

Update Supplier
Set PaymentPfoID = pfo.ID
from Supplier s
inner join PublicFinancialOffice pfo on REPLACE(pfo.Name,'''','') = REPLACE(s.PaymentPfo,'΄','')

GO

Update Supplier
Set PaymentPfoID = pfo.ID
from Supplier s
inner join PublicFinancialOffice pfo on REPLACE(pfo.Name,'''','') = REPLACE(s.PaymentPfo,'''','')

GO



SET IDENTITY_INSERT PublicFinancialOffice ON
GO

Insert into PublicFinancialOffice (ID, GgpsID, Name)
Values (-1,-1, 'Χώρας Εξωτερικού')
GO


SET IDENTITY_INSERT PublicFinancialOffice OFF
GO

Update Supplier SET PfoID = -1 where PfoID is null and pfo is not null
GO

Update Supplier SET PaymentPfoID = -1 where PaymentPfoID is null and paymentpfo is not null
GO