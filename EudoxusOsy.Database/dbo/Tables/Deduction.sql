CREATE TABLE [dbo].[Deduction] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [StartDate]      DATETIME2 (7)  NOT NULL,
    [EndDate]        DATETIME2 (7)  NULL,
    [VatType]        INT            NOT NULL,
    [Vat]            DECIMAL (5, 2) NOT NULL,
    [OgaPercentage]  DECIMAL (5, 2) NOT NULL,
    [MtpyPercentage] DECIMAL (5, 2) NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [CreatedAt]      DATETIME2 (7)  NOT NULL,
    [CreatedBy]      NVARCHAR (256) NOT NULL,
    [UpdatedAt]      DATETIME2 (7)  NULL,
    [UpdatedBy]      NVARCHAR (256) NULL,
    CONSTRAINT [PK_Deduction] PRIMARY KEY CLUSTERED ([ID] ASC)
);

