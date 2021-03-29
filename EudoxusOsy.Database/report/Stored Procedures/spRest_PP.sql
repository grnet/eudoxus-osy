
--EXEC [report].[spRest_PP]
CREATE
PROCEDURE [report].[spRest_PP] AS
BEGIN

TRUNCATE TABLE report.ViewStatisticsPerInstitution_PP

INSERT INTO report.ViewStatisticsPerInstitution_PP([InstitutionID],[Debt],[PhaseId],[InstName],[PricedCount],[NotPricedCount],[DepartmentCount])        
SELECT  Institution.ID, ROUND(SUM(Catalog.Amount),2), Catalog.PhaseID, Institution.Name, 	 
	COUNT(DISTINCT(Catalog.BookPriceID)) as pricedBooks, COUNT(DISTINCT(Catalog.BookID)) - COUNT(DISTINCT(Catalog.BookPriceID)) as notPricedBooks,
	COUNT(DISTINCT(Catalog.DepartmentID))
FROM Catalog
JOIN report.fnPreviousPhases() AS PP ON PP.phase_id = catalog.PhaseID
JOIN Department ON Catalog.DepartmentID = department.ID
JOIN Institution ON Department.InstitutionID = institution.ID
WHERE Catalog.Status = 1 AND state IN (0,1)          
GROUP BY Institution.ID, Institution.Name, Catalog.PhaseID

TRUNCATE TABLE report.ViewStatisticsPerDepartment_PP

INSERT INTO report.ViewStatisticsPerDepartment_PP([DepartmentID],[Debt],[PhaseID],[InstName],[DepName],[PricedCount],[NotPricedCount])
SELECT  DepartmentID, ROUND(SUM(Catalog.Amount),2), Catalog.PhaseID, Institution.Name, Department.Name,  	
	COUNT(DISTINCT(Catalog.BookPriceID)) as pricedBooks, COUNT(DISTINCT(Catalog.BookID)) - COUNT(DISTINCT(Catalog.BookPriceID)) as notPricedBooks
FROM Catalog
JOIN report.fnPreviousPhases() AS PP ON PP.phase_id = catalog.PhaseID
JOIN Department ON Catalog.DepartmentID = Department.ID
JOIN Institution ON Department.InstitutionID = Institution.ID
WHERE Catalog.Status = 1 AND state IN (0,1)    
GROUP BY Institution.ID, Institution.Name,DepartmentID, Department.Name, Catalog.PhaseID

UPDATE p SET p.TotalDebt = aa.Amount
FROM Phase p
JOIN (
	SELECT catalog.PhaseID, ROUND((SUM(Amount)),2) AS Amount
	FROM Catalog
	JOIN report.fnPreviousPhases() AS PP ON PP.phase_id = catalog.PhaseID
	WHERE Catalog.Status = 1 AND state IN (0,1)    	
	GROUP BY catalog.PhaseID
) aa ON aa.PhaseID = p.ID

END


--SELECT * 
--FROM report.ViewStatisticsPerInstitution_PP