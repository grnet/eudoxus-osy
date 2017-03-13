CREATE TABLE report.StatsPricedPerDepartment (		
		book_id INT NOT NULL,
		times_registered INT NULL,
		book_title NVARCHAR(500), 
		book_kpsid INT, 
		department_id INT, 
		phase_id INT NOT NULL
	) 