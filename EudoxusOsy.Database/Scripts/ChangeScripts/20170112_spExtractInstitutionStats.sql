CREATE 
PROCEDURE report.spExtractInstitutionStats @phaseId INT, @forCurrent INT  AS
BEGIN
	IF @forCurrent = 1
		SELECT inst.Name AS InstName, Debt, PricedCount, NotPricedCount
		FROM report.ViewStatisticsPerInstitution
		JOIN Institution inst ON inst.ID = ViewStatisticsPerInstitution.institution_id
		LEFT JOIN (
			SELECT SUM( times_registered ) as notPricedCount , inst.ID AS InstitutionID, inst.Name AS institution_title
			FROM report.StatsNotPricedPerDepartment
			JOIN Department dep ON dep.ID = StatsNotPricedPerDepartment.department_id
			JOIN Institution inst ON inst.ID = dep.InstitutionID
			WHERE StatsNotPricedPerDepartment.phase_id = @phaseId	
				AND dep.IsActive = 1
				AND inst.IsActive = 1
			GROUP BY inst.ID, inst.Name			
		) notPriced ON notPriced.InstitutionID = ViewStatisticsPerInstitution.institution_id
		LEFT JOIN (
			SELECT SUM( times_registered ) as pricedCount , inst.ID AS InstitutionID, inst.Name AS institution_title
			FROM report.StatsPricedPerDepartment
			JOIN Department dep ON dep.ID = StatsPricedPerDepartment.department_id
			JOIN Institution inst ON inst.ID = dep.InstitutionID
			WHERE StatsPricedPerDepartment.phase_id = @phaseId			
				AND dep.IsActive = 1
				AND inst.IsActive = 1
			GROUP BY inst.ID, inst.Name			
		) as priced ON priced.InstitutionID = notPriced.InstitutionID

		WHERE  ViewStatisticsPerInstitution.phase_id = @phaseId
		ORDER BY debt DESC

	ELSE
		SELECT institution_title AS InstName, totalPrice AS Debt, pricedBooksCount as Pricedcount, notPricedBooksCount as Notpricedcount
		FROM report.StatisticsPerInstitution_PP
		WHERE phase_id = @phaseId


END
GO

--EXEC report.spExtractInstitutionStats 9, 1