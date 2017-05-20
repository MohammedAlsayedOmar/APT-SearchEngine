USE [master]
GO
/****** Object:  Database [CrawlerDB]    Script Date: 20-May-17 9:40:41 PM ******/
CREATE DATABASE [CrawlerDB]
GO
ALTER DATABASE [CrawlerDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CrawlerDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CrawlerDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CrawlerDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CrawlerDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CrawlerDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CrawlerDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CrawlerDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CrawlerDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CrawlerDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CrawlerDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CrawlerDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CrawlerDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CrawlerDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CrawlerDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CrawlerDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CrawlerDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CrawlerDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CrawlerDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CrawlerDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CrawlerDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CrawlerDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CrawlerDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CrawlerDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CrawlerDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CrawlerDB] SET  MULTI_USER 
GO
ALTER DATABASE [CrawlerDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CrawlerDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CrawlerDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CrawlerDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CrawlerDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [CrawlerDB]
GO
/****** Object:  Table [dbo].[HtmlData]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HtmlData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL_ID] [int] NOT NULL,
	[htmlData] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_HtmlData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImageKeyWords]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageKeyWords](
	[ImageKeyWord_ID] [int] IDENTITY(1,1) NOT NULL,
	[ImageKeyWords] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ImageKeyWords] PRIMARY KEY CLUSTERED 
(
	[ImageKeyWord_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImageNames]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageNames](
	[ImageFile_ID] [int] IDENTITY(1,1) NOT NULL,
	[ImageFileName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ImageNames] PRIMARY KEY CLUSTERED 
(
	[ImageFile_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImageWebSites]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageWebSites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ImageSite] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ImageWebSites] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyWordLocationURL]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyWordLocationURL](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL_ID] [int] NOT NULL,
	[ImageKeyWords_ID] [int] NOT NULL,
	[ImageFile_ID] [int] NOT NULL,
 CONSTRAINT [PK_kljhklj] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyWords]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyWords](
	[KeyWord_ID] [int] IDENTITY(1,1) NOT NULL,
	[KeyWords] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_KeyWords] PRIMARY KEY CLUSTERED 
(
	[KeyWord_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyWordsPosition_Headers]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyWordsPosition_Headers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL_ID] [int] NOT NULL,
	[KeyWord_ID] [int] NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_KeyWordsPosition_Headers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyWordsPosition_Paragraphs]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyWordsPosition_Paragraphs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL_ID] [int] NOT NULL,
	[KeyWord_ID] [int] NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_KeyWordsPosition_Paragraphs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyWordsPosition_Titles]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyWordsPosition_Titles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL_ID] [int] NOT NULL,
	[KeyWord_ID] [int] NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_KeyWordsPosition_Titles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SearchHistory]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SearchHistory] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SearchHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Url_Container]    Script Date: 20-May-17 9:40:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Url_Container](
	[URL_ID] [int] IDENTITY(1,1) NOT NULL,
	[URLName] [nvarchar](max) NOT NULL,
	[Status] [char](1) NOT NULL CONSTRAINT [DF_Url_Container_Status]  DEFAULT ('N'),
	[Frequency] [int] NOT NULL,
	[URL_Title] [nvarchar](max) NULL,
	[TotalNumberOfWords] [int] NOT NULL CONSTRAINT [DF_Url_Container_TotalNumberOfWords]  DEFAULT ((0)),
 CONSTRAINT [PK_Url_Container] PRIMARY KEY CLUSTERED 
(
	[URL_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[HtmlData]  WITH CHECK ADD  CONSTRAINT [FK_HtmlData_Url_Container] FOREIGN KEY([URL_ID])
REFERENCES [dbo].[Url_Container] ([URL_ID])
GO
ALTER TABLE [dbo].[HtmlData] CHECK CONSTRAINT [FK_HtmlData_Url_Container]
GO
ALTER TABLE [dbo].[KeyWordLocationURL]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordLocationURL_ImageKeyWords] FOREIGN KEY([ImageKeyWords_ID])
REFERENCES [dbo].[ImageKeyWords] ([ImageKeyWord_ID])
GO
ALTER TABLE [dbo].[KeyWordLocationURL] CHECK CONSTRAINT [FK_KeyWordLocationURL_ImageKeyWords]
GO
ALTER TABLE [dbo].[KeyWordLocationURL]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordLocationURL_ImageNames] FOREIGN KEY([ImageFile_ID])
REFERENCES [dbo].[ImageNames] ([ImageFile_ID])
GO
ALTER TABLE [dbo].[KeyWordLocationURL] CHECK CONSTRAINT [FK_KeyWordLocationURL_ImageNames]
GO
ALTER TABLE [dbo].[KeyWordLocationURL]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordLocationURL_Url_Container] FOREIGN KEY([URL_ID])
REFERENCES [dbo].[Url_Container] ([URL_ID])
GO
ALTER TABLE [dbo].[KeyWordLocationURL] CHECK CONSTRAINT [FK_KeyWordLocationURL_Url_Container]
GO
ALTER TABLE [dbo].[KeyWordsPosition_Headers]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordsPosition_Headers_KeyWords] FOREIGN KEY([KeyWord_ID])
REFERENCES [dbo].[KeyWords] ([KeyWord_ID])
GO
ALTER TABLE [dbo].[KeyWordsPosition_Headers] CHECK CONSTRAINT [FK_KeyWordsPosition_Headers_KeyWords]
GO
ALTER TABLE [dbo].[KeyWordsPosition_Headers]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordsPosition_Headers_Url_Container] FOREIGN KEY([URL_ID])
REFERENCES [dbo].[Url_Container] ([URL_ID])
GO
ALTER TABLE [dbo].[KeyWordsPosition_Headers] CHECK CONSTRAINT [FK_KeyWordsPosition_Headers_Url_Container]
GO
ALTER TABLE [dbo].[KeyWordsPosition_Paragraphs]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordsPosition_Paragraphs_KeyWords] FOREIGN KEY([KeyWord_ID])
REFERENCES [dbo].[KeyWords] ([KeyWord_ID])
GO
ALTER TABLE [dbo].[KeyWordsPosition_Paragraphs] CHECK CONSTRAINT [FK_KeyWordsPosition_Paragraphs_KeyWords]
GO
ALTER TABLE [dbo].[KeyWordsPosition_Paragraphs]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordsPosition_Paragraphs_Url_Container] FOREIGN KEY([URL_ID])
REFERENCES [dbo].[Url_Container] ([URL_ID])
GO
ALTER TABLE [dbo].[KeyWordsPosition_Paragraphs] CHECK CONSTRAINT [FK_KeyWordsPosition_Paragraphs_Url_Container]
GO
ALTER TABLE [dbo].[KeyWordsPosition_Titles]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordsPosition_Titles_KeyWords] FOREIGN KEY([KeyWord_ID])
REFERENCES [dbo].[KeyWords] ([KeyWord_ID])
GO
ALTER TABLE [dbo].[KeyWordsPosition_Titles] CHECK CONSTRAINT [FK_KeyWordsPosition_Titles_KeyWords]
GO
ALTER TABLE [dbo].[KeyWordsPosition_Titles]  WITH CHECK ADD  CONSTRAINT [FK_KeyWordsPosition_Titles_Url_Container] FOREIGN KEY([URL_ID])
REFERENCES [dbo].[Url_Container] ([URL_ID])
GO
ALTER TABLE [dbo].[KeyWordsPosition_Titles] CHECK CONSTRAINT [FK_KeyWordsPosition_Titles_Url_Container]
GO
USE [master]
GO
ALTER DATABASE [CrawlerDB] SET  READ_WRITE 
GO
