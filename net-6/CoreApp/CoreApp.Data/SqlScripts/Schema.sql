IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'dbDemo')
BEGIN
	CREATE DATABASE dbDemo
END

USE [dbDemo]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Demo')
BEGIN
    CREATE TABLE Demo
	(
		Id int not null identity(1, 1),
		[Text] varchar(2000) null,
		[Description] varchar(500) not null,
		Presenter varchar(60) not null,
		[Date] datetime not null
	)
END