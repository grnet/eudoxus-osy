CREATE TABLE [dbo].[BookPriceRequest] (
    [ID]                                INT            IDENTITY (1, 1) NOT NULL,
    [BookID]                            INT            NULL,
    [BookKpsID]                         INT            NOT NULL,
    [SupplierID]                        INT            NOT NULL,
    [DistributionYear]                  INT            NOT NULL,
    [ContentType]                       INT            NOT NULL,
    [ApplicationType]                   INT            NOT NULL,
    [LatinPages]                        INT            NOT NULL,
    [NormalPages]                       INT            NOT NULL,
    [CoverType]                         INT            NOT NULL,
    [SelectedCoverType]                 INT            NOT NULL,
    [Width]                             FLOAT (53)     NOT NULL,
    [Height]                            FLOAT (53)     NOT NULL,
    [Dimension]                         INT            NOT NULL,
    [CommercialPrice]                   FLOAT (53)     NOT NULL,
    [PageFolding]                       INT            NOT NULL,
    [PagesPerSide]                      INT            NOT NULL,
    [Comments]                          NVARCHAR (MAX) NULL,
    [IsTechnical]                       INT            NOT NULL,
    [TechnicalNonSpecialPages]          INT            NULL,
    [TechnicalNonSpecialPagesList]      NVARCHAR (MAX) NULL,
    [TechnicalNonSpecialLatinPagesList] NVARCHAR (MAX) NULL,
    [TechnicalSpecialPages]             INT            NULL,
    [TechnicalSpecialPagesList]         NVARCHAR (MAX) NULL,
    [TechnicalSpecialLatinPagesList]    NVARCHAR (MAX) NULL,
    [TechnicalPercentage]               FLOAT (53)     NOT NULL,
    [ColorOne]                          INT            NOT NULL,
    [ColorOnePages]                     INT            NULL,
    [ColorOnePagesList]                 NVARCHAR (MAX) NULL,
    [ColorOneLatinPagesList]            NVARCHAR (MAX) NULL,
    [ColorTwo]                          INT            NOT NULL,
    [ColorTwoPages]                     INT            NULL,
    [ColorTwoPagesList]                 NVARCHAR (MAX) NULL,
    [ColorTwoLatinPagesList]            NVARCHAR (MAX) NULL,
    [ColorFour]                         INT            NOT NULL,
    [ColorFourPages]                    INT            NULL,
    [ColorFourPagesList]                NVARCHAR (MAX) NULL,
    [ColorFourLatinPagesList]           NVARCHAR (MAX) NULL,
    [IsTranslation]                     INT            NOT NULL,
    [TranslationPrototypePrice]         FLOAT (53)     NOT NULL,
    [IsMultiVolume]                     BIT            NOT NULL,
    [IsVolume]                          BIT            NOT NULL,
    [TempTechnicalPrice]                FLOAT (53)     NOT NULL,
    [ComputedPrice]                     FLOAT (53)     NOT NULL,
    [EndPrice]                          FLOAT (53)     NOT NULL,
    [State]                             INT            NOT NULL,
    [IsActive]                          BIT            NOT NULL,
    [CreatedAt]                         DATETIME2 (7)  NOT NULL,
    [CreatedBy]                         NVARCHAR (256) NOT NULL,
    [UpdatedAt]                         DATETIME2 (7)  NULL,
    [UpdatedBy]                         NVARCHAR (256) NULL,
    CONSTRAINT [PK_BookPriceRequest] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BookPriceRequest_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BookPriceRequest_BookID]
    ON [dbo].[BookPriceRequest]([BookID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BookPriceRequest_BookKpsID]
    ON [dbo].[BookPriceRequest]([BookKpsID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BookPriceRequest_SupplierID]
    ON [dbo].[BookPriceRequest]([SupplierID] ASC);

