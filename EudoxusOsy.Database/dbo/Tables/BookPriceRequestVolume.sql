CREATE TABLE [dbo].[BookPriceRequestVolume] (
    [ID]                           INT            IDENTITY (1, 1) NOT NULL,
    [BookPriceRequestID]           INT            NOT NULL,
    [BookPriceRequestMainVolumeID] INT            NOT NULL,
    [VolumeNumber]                 INT            NOT NULL,
    [IsActive]                     BIT            NOT NULL,
    [CreatedAt]                    DATETIME2 (7)  NOT NULL,
    [CreatedBy]                    NVARCHAR (256) NOT NULL,
    [UpdatedAt]                    DATETIME2 (7)  NULL,
    [UpdatedBy]                    NVARCHAR (256) NULL,
    CONSTRAINT [PK_BookPriceRequestVolume] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BookPriceRequestVolume_BookPriceRequest] FOREIGN KEY ([BookPriceRequestID]) REFERENCES [dbo].[BookPriceRequest] ([ID]),
    CONSTRAINT [FK_BookPriceRequestVolume_BookPriceRequestMainVolume] FOREIGN KEY ([BookPriceRequestMainVolumeID]) REFERENCES [dbo].[BookPriceRequest] ([ID])
);

