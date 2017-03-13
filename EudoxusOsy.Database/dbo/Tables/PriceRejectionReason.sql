CREATE TABLE [dbo].[PriceRejectionReason] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [OrderIndex]      INT            NOT NULL,
    [Description]     NVARCHAR (MAX) NOT NULL,
    [FullDescription] NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    CONSTRAINT [PK_PriceRejectionReason] PRIMARY KEY CLUSTERED ([ID] ASC)
);

