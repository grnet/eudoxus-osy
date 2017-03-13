CREATE TABLE [dbo].[Discount] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [PhaseID]            INT            NULL,
    [BookID]             INT            NULL,
    [BookCountFrom]      INT            NOT NULL,
    [BookCountTo]        INT            NOT NULL,
    [DiscountPercentage] DECIMAL (3, 2) NOT NULL,
    CONSTRAINT [PK_Discount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Discount_Book] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Book] ([ID]),
    CONSTRAINT [FK_Discount_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID])
);

