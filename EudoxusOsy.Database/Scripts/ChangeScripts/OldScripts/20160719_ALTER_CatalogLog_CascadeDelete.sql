ALTER TABLE [dbo].[CatalogLog] DROP CONSTRAINT [FK_CatalogLog_Catalog] 

ALTER TABLE [dbo].[CatalogLog]  WITH CHECK ADD  CONSTRAINT [FK_CatalogLog_Catalog] FOREIGN KEY([CatalogID])
REFERENCES [dbo].[Catalog] ([ID])
ON DELETE CASCADE
GO

