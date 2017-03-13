if not exists (select * from sysobjects where name='AuditReceipt' and xtype='U')
    create table AuditReceipt (
        ID int not null IDENTITY(1,1) PRIMARY KEY,
		RegistrationKpsID bigint not null,
		KpsBookID bigint NOT NULL,
		ReceivedAt datetime2(7) not null,
		SecreteriatKpsID bigint not null,
		SentByKpsAt datetime2(7) not null,
		Reason int not null default 1, -- 0 cancelled, 1 deliveredCanceled
		Amount int not null default 0,
		Request xml not null,
		CreatedBy nvarchar(50) not null,
		CreatedAt datetime2(7) not null,
		Status int null -- 0 Failed, 1 Processed
    )
GO

truncate table AuditReceipt
GO