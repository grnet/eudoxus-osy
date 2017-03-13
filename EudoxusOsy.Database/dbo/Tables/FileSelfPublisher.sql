CREATE TABLE [dbo].[FileSelfPublisher] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [FileID]     INT            NOT NULL,
    [SupplierID] INT            NOT NULL,
    [PhaseID]    INT            NOT NULL,
    [IsActive]   BIT            NOT NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [UpdatedAt]  DATETIME2 (7)  NULL,
    [UpdatedBy]  NVARCHAR (256) NULL,
    CONSTRAINT [PK_FileSelfPublisher] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_FileSelfPublisher_File] FOREIGN KEY ([FileID]) REFERENCES [dbo].[File] ([ID]),
    CONSTRAINT [FK_FileSelfPublisher_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]),
    CONSTRAINT [FK_FileSelfPublisher_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_FileSelfPublisher_FileID]
    ON [dbo].[FileSelfPublisher]([FileID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FileSelfPublisher_SupplierID]
    ON [dbo].[FileSelfPublisher]([SupplierID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FileSelfPublisher_PhaseID]
    ON [dbo].[FileSelfPublisher]([PhaseID] ASC);

