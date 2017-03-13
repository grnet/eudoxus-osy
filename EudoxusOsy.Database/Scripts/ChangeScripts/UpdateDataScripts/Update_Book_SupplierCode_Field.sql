Update Book 
SET SupplierCode = bp.PublisherID
from BookPublishers bp
inner join book b on b.BookKpsID = bp.bookID