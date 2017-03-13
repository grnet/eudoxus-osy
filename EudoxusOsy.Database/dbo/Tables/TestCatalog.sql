CREATE TABLE [dbo].[TestCatalog] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [CatalogType]  INT            NOT NULL,
    [GroupID]      INT            NULL,
    [PhaseID]      INT            NULL,
    [BookID]       INT            NOT NULL,
    [SupplierID]   INT            NOT NULL,
    [DepartmentID] INT            NOT NULL,
    [DiscountID]   INT            NOT NULL,
    [BookCount]    INT            NOT NULL,
    [Percentage]   DECIMAL (5, 2) NULL,
    [BookPriceID]  INT            NULL,
    [Amount]       DECIMAL (8, 2) NULL,
    [OldCatalogID] INT            NULL,
    [NewCatalogID] INT            NULL,
    [State]        INT            NOT NULL,
    [Status]       INT            NOT NULL,
    [CreatedAt]    DATETIME2 (7)  NOT NULL,
    [CreatedBy]    NVARCHAR (256) NOT NULL,
    [UpdatedAt]    DATETIME2 (7)  NULL,
    [UpdatedBy]    NVARCHAR (256) NULL
);

