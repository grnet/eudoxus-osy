CREATE TABLE [dbo].[UsersInRoles] (
    [ReporterID] INT NOT NULL,
    [RoleID]     INT NOT NULL,
    CONSTRAINT [PK_UsersInRoles] PRIMARY KEY CLUSTERED ([ReporterID] ASC, [RoleID] ASC),
    CONSTRAINT [FK_UsersInRoles_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]),
    CONSTRAINT [FK_UsersInRoles_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Role] ([ID])
);

