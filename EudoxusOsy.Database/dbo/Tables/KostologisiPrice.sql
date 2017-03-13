CREATE TABLE [dbo].[KostologisiPrice] (
    [ID]            INT        IDENTITY (1, 1) NOT NULL,
    [TechnicType]   INT        NOT NULL,
    [DimensionType] INT        NOT NULL,
    [ColorType]     INT        NOT NULL,
    [CoverValue]    FLOAT (53) NOT NULL,
    [ColorValue]    FLOAT (53) NOT NULL
);

