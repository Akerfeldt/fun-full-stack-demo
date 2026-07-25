IF OBJECT_ID('dbo.Player', 'U') IS NULL
CREATE TABLE [dbo].[Player] (
	[Id] [int] NOT NULL IDENTITY (1, 1),
	[Class] [tinyint] NOT NULL,
	[Race] [tinyint] NOT NULL,
	[LocationX] [int] NOT NULL,
	[LocationY] [int] NOT NULL,
	[Name] [varchar](10) NOT NULL,
	[UserId] [varchar](30) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
));
GO

IF OBJECT_ID('dbo.DF_Player_Deleted', 'D') IS NULL
    ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_Deleted]  DEFAULT ((0)) FOR [Deleted];
GO

IF NOT EXISTS (SELECT 1/0 FROM sys.indexes WHERE name = 'UX_Player_UserId' AND object_id = OBJECT_ID('dbo.Player'))
	CREATE UNIQUE NONCLUSTERED INDEX [UX_Player_UserId] ON [dbo].[Player] (UserId);
GO
