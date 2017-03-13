CREATE 
PROCEDURE [report].[spExtractDepartmentStats] @phaseId INT, @forCurrent INT  AS
BEGIN

	 IF @forCurrent = 1
		SELECT inst.Name AS InstName, dep.Name AS DepName, priced.Name, Debt, PricedCount, NotPricedCount
		FROM report.ViewStatisticsPerDepartment
		JOIN Department dep ON dep.ID = ViewStatisticsPerDepartment.department_id
		JOIN Institution inst ON inst.ID = dep.InstitutionID
		LEFT JOIN (
			SELECT SUM( times_registered ) as notPricedCount , dep.ID, dep.Name
			FROM report.StatsNotPricedPerDepartment
			JOIN Department dep ON dep.ID = StatsNotPricedPerDepartment.department_id
			WHERE StatsNotPricedPerDepartment.phase_id = @phaseId
			AND dep.IsActive = 1
			GROUP BY dep.ID, dep.Name			
		) as notPriced ON notPriced.ID = dep.ID
		LEFT JOIN (
			SELECT SUM( times_registered ) as pricedCount , dep.ID, dep.Name
			FROM report.StatsPricedPerDepartment
			JOIN Department dep ON dep.ID = StatsPricedPerDepartment.department_id
			WHERE StatsPricedPerDepartment.phase_id = @phaseId
			AND dep.IsActive = 1
			GROUP BY dep.ID, dep.Name			
		) as priced ON priced.ID = dep.ID
		WHERE ViewStatisticsPerDepartment.phase_id = @phaseId
			AND dep.IsActive = 1
			AND inst.IsActive = 1
		ORDER BY ViewStatisticsPerDepartment.debt DESC

	ELSE
		SELECT institution_title AS InstName, department_title AS DepName, totalPrice AS Debt, pricedBooksCount as Pricedcount, notPricedBooksCount as Notpricedcount
		FROM report.StatisticsPerDepartment_PP
		WHERE phase_id = @phaseId

END


