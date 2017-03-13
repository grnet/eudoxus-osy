CREATE
PROCEDURE [report].[spRest_PP] AS

TRUNCATE TABLE report.statisticsPerInstitution_PP

INSERT INTO report.statisticsPerInstitution_PP (institution_kpsid, institution_title, 
		departments_count, totalPrice, phase_id, 
		pricedBooksCount, notPricedBooksCount)
        
SELECT  Institution.ID, Institution.Name, 
	COUNT(DISTINCT(Catalog.DepartmentID)), ROUND(SUM(Catalog.Amount),2), Catalog.PhaseID, 
	COUNT(DISTINCT(Catalog.BookPriceID)) as pricedBooks, COUNT(DISTINCT(Catalog.BookID)) - COUNT(DISTINCT(Catalog.BookPriceID)) as notPricedBooks
FROM Catalog
JOIN report.fnPreviousPhases() AS PP ON PP.phase_id = catalog.PhaseID
JOIN Department ON Catalog.DepartmentID = department.ID
JOIN Institution ON Department.InstitutionID = institution.ID
WHERE Catalog.Status = 1 AND state IN (0,1)          
GROUP BY Institution.ID, Institution.Name, Catalog.PhaseID

TRUNCATE TABLE report.StatisticsPerDepartment_PP

INSERT INTO report.StatisticsPerDepartment_PP (institution_kpsid, institution_title, 
				department_kpsid, library_kpsid, department_title, totalPrice, phase_id, 
				pricedBooksCount, notPricedBooksCount)
SELECT  Institution.ID, Institution.Name, 
				Department.SecretaryKpsID, Department.LibraryKpsID, Department.Name, ROUND(SUM(Catalog.Amount),2), Catalog.PhaseID, 
				COUNT(DISTINCT(Catalog.BookPriceID)) as pricedBooks, COUNT(DISTINCT(Catalog.BookID)) - COUNT(DISTINCT(Catalog.BookPriceID)) as notPricedBooks
FROM Catalog
JOIN report.fnPreviousPhases() AS PP ON PP.phase_id = catalog.PhaseID
JOIN Department ON Catalog.DepartmentID = Department.ID
JOIN Institution ON Department.InstitutionID = Institution.ID
WHERE Catalog.Status = 1 AND state IN (0,1)    
GROUP BY Institution.ID, Institution.Name, Department.SecretaryKpsID, Department.LibraryKpsID, Department.Name, Catalog.PhaseID


UPDATE p SET p.TotalDebt = aa.Amount
FROM Phase p
JOIN (
	SELECT catalog.PhaseID, ROUND((SUM(Amount)),2) AS Amount
	FROM Catalog
	JOIN report.fnPreviousPhases() AS PP ON PP.phase_id = catalog.PhaseID
	WHERE Catalog.Status = 1 AND state IN (0,1)    	
	GROUP BY catalog.PhaseID
) aa ON aa.PhaseID = p.ID



