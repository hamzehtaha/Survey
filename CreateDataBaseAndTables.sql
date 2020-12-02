/**
Go to master and creater database 
**/
USE [master]
GO

IF OBJECT_ID('Survey') is null
begin 
CREATE DATABASE [Survey]
end
GO

USE[Survey]
GO
/**
Go to survey data base and create tables in survey data base  
**/
IF OBJECT_ID('Survey.dbo.Qustions') is null
begin 
CREATE TABLE [dbo].[Qustions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Qustions_text] [nvarchar](100) NULL,
	[Qustion_order] [int] NOT NULL,
	[Type_Of_Qustion] [nvarchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

IF OBJECT_ID('Survey.dbo.Smily') is null
begin 
CREATE TABLE [dbo].[Smily](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number_of_smily] [int] NOT NULL,
	[Qus_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

ALTER TABLE [dbo].[Smily]  WITH CHECK ADD FOREIGN KEY([Qus_ID])
REFERENCES [dbo].[Qustions] ([ID])
GO
ALTER TABLE [dbo].[Smily]  WITH CHECK ADD CHECK  (([Number_of_smily]>=(2)))
GO
ALTER TABLE [dbo].[Smily]  WITH CHECK ADD CHECK  (([Number_of_smily]<=(5)))
GO


IF OBJECT_ID('Survey.dbo.Stars') is null
begin 
CREATE TABLE [dbo].[Stars](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number_Of_Stars] [int] NOT NULL,
	[Qus_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

ALTER TABLE [dbo].[Stars]  WITH CHECK ADD FOREIGN KEY([Qus_ID])
REFERENCES [dbo].[Qustions] ([ID])
GO

ALTER TABLE [dbo].[Stars]  WITH CHECK ADD CHECK  (([Number_Of_Stars]<=(10)))
GO

ALTER TABLE [dbo].[Stars]  WITH CHECK ADD CHECK  (([Number_Of_Stars]>=(1)))
GO

IF OBJECT_ID('Survey.dbo.Slider') is null
begin 
CREATE TABLE [dbo].[Slider](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Start_Value] [int] NOT NULL,
	[End_Value] [int] NOT NULL,
	[Start_Value_Cap] [varchar](3) NULL,
	[End_Value_Cap] [varchar](3) NULL,
	[Qus_ID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

ALTER TABLE [dbo].[Slider]  WITH CHECK ADD FOREIGN KEY([Qus_ID])
REFERENCES [dbo].[Qustions] ([ID])
GO

ALTER TABLE [dbo].[Slider]  WITH CHECK ADD CHECK  (([End_Value]<=(100)))
GO
ALTER TABLE [dbo].[Slider]  WITH CHECK ADD CHECK  (([Start_Value]<=(100)))
GO
ALTER TABLE [dbo].[Slider]  WITH CHECK ADD CHECK  (([Start_Value]<([End_Value])))
GO


