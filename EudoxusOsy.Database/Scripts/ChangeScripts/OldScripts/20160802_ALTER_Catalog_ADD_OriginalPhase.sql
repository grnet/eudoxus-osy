ALTER TABLE Catalog
ADD OriginalPhaseID int NULL,
CONSTRAINT [FK_Catalog_OriginalPhase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase]([ID])