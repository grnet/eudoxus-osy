CREATE TABLE [dbo].[Kap_Cities] (
    [ID]           INT            NOT NULL,
    [Name]         NVARCHAR (100) NOT NULL,
    [PrefectureID] INT            NOT NULL,
    CONSTRAINT [PK_Kap_Cities] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Kap_Cities_Kap_Prefectures] FOREIGN KEY ([PrefectureID]) REFERENCES [dbo].[Kap_Prefectures] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_KapCities_PrefectureID]
    ON [dbo].[Kap_Cities]([PrefectureID] ASC);

