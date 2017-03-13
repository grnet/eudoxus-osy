CREATE
PROCEDURE [dbo].[spReceipt] 
	@registrationKpsId bigint, 
	@received DATETIME2(7), 
	@bookKpsId bigint, 
	@secretariatKpsId bigint,
	@sentByKps DATETIME2(7), 
	@reason int, 
	@auditReceiptID int,
	@ret INT OUTPUT
AS

BEGIN --Declare return codes
 DECLARE @SUCCESS INT = 0 --0 SUCCESS
 DECLARE @INVALID_REASON INT = 1 --1 Invalid reason
 DECLARE @ZERO_ROWS INT = 2 --2 Zero rows inserted (missing book id or department id)
 DECLARE @REGISTRATION_KPS_ZERO INT = 3 --3 registrationKpsId is zero
 DECLARE @REPEAT_OF_REASON INT = 4 --4 repeat of reason between different states
 DECLARE @PROCEDURE_FAILED INT = 5 --5 Procedure failed (possible deadlock) 
 DECLARE @RESERVED int = 6 --6 Reserved
 DECLARE @MISSING_SECRETARIAT INT = 7 --7 missing secretariat kps id
 DECLARE @MISSING_DEPARTMENT_ID INT = 8 --8 missing department id
 END -- Declare Return Codes
 ---------------------
 BEGIN -- Declare Variables
 DECLARE @rows INT
 DECLARE @currentPhaseId INT
 DECLARE @previousPhaseId INT
 DECLARE @oldTimestamp DATETIME
 DECLARE @receiptId INT
 DECLARE @departmentId INT
 
 DECLARE @canceled int = 0
 DECLARE @delivered int = 1
 DECLARE @state INT 

 DECLARE @sentByKpsAt DATETIME
 SELECT @sentByKpsAt = @sentByKps
 DECLARE @receivedAt DATETIME
 SELECT @receivedAt = @received
 
 DECLARE @user VARCHAR(50) = 'sysadmin'
 DECLARE @now DATE = CONVERT(DATE, GETDATE())
 DECLARE @lastPhase DATETIME

 END -- Declare Variables

  /* initial value for ret variable. If this doesn't change it means the procedure failed*/
  SET @ret = @PROCEDURE_FAILED

  /* validate registrationKpsId*/
  IF @registrationKpsId = 0
  BEGIN -- registrationKpsId validation
	SET @ret = @REGISTRATION_KPS_ZERO
    RETURN
  END  /*end registrationKpsId validation*/
  
  /* validate reason case insensitive*/
  SET @reason=UPPER(@reason);  
  BEGIN --ReasonValidation#------------------------------------------   
  IF (@reason <> @delivered AND @reason <> @canceled)
  BEGIN
	SET @ret = @INVALID_REASON
	RETURN
  END
   END--ReasonValidation#------------------------------------------   
   IF @reason = @delivered
   BEGIN
	 SET @state = 1
   END
   ELSE
   BEGIN
	 SET @state = 2
   END
   --#------------------------------------------   
/* initial value for current phase id
if this doesn't change there is no current phase*/

BEGIN -- Validate DepartmentID
--#------------------------------------------
IF @reason = @delivered
BEGIN
	SELECT @departmentId = ID 
	FROM Department 
	WHERE SecretaryKpsID = @secretariatKpsId		

	SET @rows = @@ROWCOUNT
                            
	IF @rows <> 1
	BEGIN				
		SET @ret = @MISSING_DEPARTMENT_ID
		RETURN
	END
END
--#------------------------------------------
END -- Validate DepartmentID

SET @currentPhaseId = 0;    
/* select current phase id*/
SELECT @currentPhaseId = ID		
FROM Phase
WHERE ( @now >= CONVERT(DATE,StartDate) AND @now <= CONVERT(DATE,EndDate) ) OR ( @now >=  CONVERT(DATE,StartDate) AND EndDate IS NULL)
--#------------------------------------------
/* If a current phase doesn't exist phase is zero */
IF @currentPhaseId = 0
BEGIN      
	/* We are going to create a new phase. Start a transaction to avoid
	multiple inserts by concurrent sessions */
	BEGIN TRANSACTION      
	/* Establish a write lock on phases table !!!!!!!!!!!!!!!!*/
	/*--------------------------------------------------*/	
	/* A concurrent session might have inserted a current phase before 
	we established the table lock. Check for a current session once more*/
	SELECT @currentPhaseId = ID		
	FROM Phase
	WHERE ( @now >= CONVERT(DATE,StartDate) AND @now <= CONVERT(DATE,EndDate) ) OR ( @now =  CONVERT(DATE,StartDate) AND EndDate IS NULL)
	--#------------------------------------------
	/* if phase is still 0, we must create a new phase*/
	IF @currentPhaseId = 0
	BEGIN      
		/*get the latest phase stop timestamp*/
		SELECT TOP 1 @lastPhase = EndDate
		FROM Phase
		ORDER BY EndDate DESC			
      
		/*insert a new session starting one second after the last one stopped*/
		INSERT INTO Phase(Year, StartDate, EndDate, PhaseAmount, TotalDebt, IsActive, CreatedAt, CreatedBy)
		VALUES(YEAR(@lastPhase), @lastPhase+1,NULL,0,0,1,GETDATE(),@user);
        
		/*The newly inserted phase is the current phase*/
		SET @currentPhaseId = @@IDENTITY;
        
	END /*end current phase check for cuncurrent transactions*/
	--#------------------------------------------
	/*commit the transaction and release table lock*/
	COMMIT TRANSACTION
END /*end check or create current phase*/
--#------------------------------------------
		
/*start a transaction to avoid conflicts of different sessions*/
BEGIN TRANSACTION

/*select previous entries of the receipt in the current phase*/
SELECT @receiptId = ID, @oldTimestamp = SentByKpsAt  
FROM Receipt 
WHERE RegistrationKpsID = @registrationKpsId AND PhaseID = @currentPhaseId
					  
SET @rows = @@ROWCOUNT

IF @rows = 1 /*if an entry exists in current phase*/
BEGIN -- if entry already exists in current Phase
	/*check the timestamp of the previous entry*/
	IF @oldTimestamp <= @sentByKpsAt
	BEGIN      
		UPDATE Receipt SET State = @state, 
							DepartmentID = CASE WHEN @state = 1 THEN @departmentId ELSE 0 END ,
							ReceivedAt = @receivedAt, SentByKpsAt = @sentByKpsAt, UpdatedAt = GETDATE(), UpdatedBy = @user
		WHERE ID = @receiptId
		
		SET @rows = @@ROWCOUNT

		IF @rows = 1 /*The inserted rows where 1. Insert was successful*/
		BEGIN							  
			SET @ret = @SUCCESS      
		END
		ELSE /*Insert failed*/
		BEGIN				  
			SET @ret = @ZERO_ROWS
		END			
			
	END --@oldTimestamp <= sentByKps -- end incomming timestamp comparison		  
	/*procedure executed successfully*/
	SET @ret = @SUCCESS
END -- if entry exists in current phase
/*if an entry exists the business logic is straight forward*/
--#------------------------------------------
ELSE /*the receipt doesn't exist in the current phase maybe it exists in a previous phase. Select the last existing entry*/
BEGIN

	SELECT TOP 1 @receiptId = ID , @previousPhaseId = PhaseID 
	FROM Receipt 
	WHERE RegistrationKpsID = @registrationKpsId 
	ORDER BY SentByKpsAt DESC	

	SET @rows = @@ROWCOUNT
	
	INSERT INTO Receipt(
	[PhaseID],[BookID],[DepartmentID],[BookCount],
	[State],[Status],[RegistrationKpsID],[SentByKpsAt],[ReceivedAt],[PreviousPhaseID],[CreatedAt],[CreatedBy])
	SELECT @currentPhaseId, Book.ID, CASE WHEN @state = 1 THEN @departmentId ELSE NULL END,1, 
	@state,0, @registrationKpsId, @sentByKps, @receivedAt, CASE WHEN @rows = 1 THEN @previousPhaseId ELSE NULL END, GETDATE(),@user
	FROM Book
	WHERE BookKpsID = @bookKpsId

	SET @rows = @@ROWCOUNT

	IF @rows = 1 /*The inserted rows where 1. Insert was successful*/
	BEGIN							  
		SET @ret = @SUCCESS      
	END
	ELSE /*Insert failed*/
	BEGIN				  
		SET @ret = @ZERO_ROWS
	END			
END/*if an entry exists in current phase or not*/

BEGIN -- do final update for the auditReceipt record
IF @ret = @SUCCESS
BEGIN 
	UPDATE AuditReceipt SET Status = 1 WHERE ID = @auditReceiptID 	
END
ELSE
BEGIN 
	UPDATE AuditReceipt SET Status = 0 where ID = @auditReceiptID
END
END -- do final update for the auditReceipt record
--#------------------------------------------
COMMIT TRANSACTION
--#------------------------------------------
RETURN 


GO


