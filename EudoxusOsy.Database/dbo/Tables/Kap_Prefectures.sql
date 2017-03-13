CREATE TABLE [dbo].[Kap_Prefectures] (
    [ID]       INT            NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL,
    [RegionID] INT            NOT NULL,
    CONSTRAINT [PK_Kap_Prefectures] PRIMARY KEY CLUSTERED ([ID] ASC)
);

