-- Switch to the system (aka master) database
USE [master]
GO


-- Delete the Encryption Messenger Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='EncryptionMessenger')
/****** Object:  Database [EncryptionMessenger]    Script Date: 2/20/2019 1:12:02 PM ******/
DROP DATABASE [EncryptionMessenger]
GO

/****** Object:  Database [EncryptionMessenger]    Script Date: 2/20/2019 1:12:02 PM ******/
CREATE DATABASE [EncryptionMessenger]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EncryptionMessenger', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\EncryptionMessenger.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EncryptionMessenger_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\EncryptionMessenger_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO



ALTER DATABASE [EncryptionMessenger] SET COMPATIBILITY_LEVEL = 140
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EncryptionMessenger].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [EncryptionMessenger] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET ARITHABORT OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [EncryptionMessenger] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [EncryptionMessenger] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET  DISABLE_BROKER 
GO

ALTER DATABASE [EncryptionMessenger] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [EncryptionMessenger] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [EncryptionMessenger] SET  MULTI_USER 
GO

ALTER DATABASE [EncryptionMessenger] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [EncryptionMessenger] SET DB_CHAINING OFF 
GO

ALTER DATABASE [EncryptionMessenger] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [EncryptionMessenger] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [EncryptionMessenger] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [EncryptionMessenger] SET QUERY_STORE = OFF
GO

ALTER DATABASE [EncryptionMessenger] SET  READ_WRITE 
GO
--
--*****************************************************
-- Create messages table
--
SET QUOTED_IDENTIFIER ON
GO

USE EncryptionMessenger;
GO

CREATE TABLE [dbo].[messages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[message_date] [datetime] NOT NULL,
	[encrypted_text] [varchar](max) NOT NULL,
	 CONSTRAINT [pk_messages_id] PRIMARY KEY CLUSTERED ([ID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--
--*****************************************************
-- Create user table
--
/****** Object:  Table [dbo].[user]    Script Date: 2/20/2019 1:04:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	--[first_name] [varchar](30) NOT NULL,
	--[last_name] [varchar](30) NOT NULL,
	[date_added] [datetime] NOT NULL,
	[username] [varchar](30) NOT NULL,
	[hash_string] [varchar](64) NOT NULL,
	[salt_string] [char](16) NOT NULL,
	CONSTRAINT [pk_USER_id] PRIMARY KEY CLUSTERED ([id] ASC)
) ON [PRIMARY]
GO

--
--*****************************************************
-- Create user_message_from link table
--/****** Object:  Table [dbo].[user_message_from]    Script Date: 2/20/2019 1:06:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user_message_from](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[message_id] [int] NOT NULL,
	CONSTRAINT [pk_user_message_from_id] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[user_message_from]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([ID])
GO

ALTER TABLE [dbo].[user_message_from]  WITH CHECK ADD FOREIGN KEY([message_id])
REFERENCES [dbo].[messages] ([ID])
GO
--
--*****************************************************
-- Create user_message_to link table
--
/****** Object:  Table [dbo].[user_message_to]    Script Date: 2/20/2019 1:09:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user_message_to](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[message_id] [int] NOT NULL,
	CONSTRAINT [pk_user_message_to_id] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[user_message_to]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([ID])
GO

ALTER TABLE [dbo].[user_message_to]  WITH CHECK ADD FOREIGN KEY([message_id])
REFERENCES [dbo].[messages] ([ID])
GO
