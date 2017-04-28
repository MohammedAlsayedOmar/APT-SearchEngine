import pyodbc
import threading
import threading

class DataBaseMaster:

    def __init__(self):
        self.Connector = pyodbc.connect("Driver={ODBC Driver 13 for SQL Server};"
                                        "Server=MOHAMMED\SQLEXPRESS;"
                                        "Database=CrawlerDB;"
                                        "Trusted_Connection=yes;")
        self.Curser = self.Connector.cursor()

    def InsertNewUrl(self,URL,status,freq):
        URL = self.SlashHTML(URL)      
        try:
           self.Connector.execute("insert into dbo.Url_Container (URLName,Status,Frequency) values ('%s','%s',%d)" % (URL,status,freq))
           self.Connector.commit()
        except:
          self.Connector.rollback()
        

    def URLDoesExist(self,Data):
        Data = self.SlashHTML(Data)    
        self.Curser.execute("Select URLName from Url_Container where URLName = ('%s')" % (Data))
        data = self.Curser.fetchone()
        if data:
            return True
        else :
            return False
        

    def KeyWordDoesExist(self,Data):
        Data = self.SlashHTML(Data)  
        self.Curser.execute("Select KeyWords from KeyWords where KeyWords = ('%s')" % (Data))
        data = self.Curser.fetchone()
        if data:
            return True
        else :
            return False

    def GetURLIDByStatus(self,status):
        try:
            self.Curser.execute("Select URL_ID,URLName from Url_Container where Status = ('%s')" % (status))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()     
        

    def GetURLID(self,Url):      
        Url = self.SlashHTML(Url)  
        try:
            self.Curser.execute("Select URL_ID from Url_Container where URLName = ('%s')" % (Url))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()  
        

    def UpdateURLStatus(self,status,id):
        try:
            self.Curser.execute("update Url_Container set status='%s' where URL_ID = %d" % (status,id))
            self.Connector.commit()
        except:
            self.Connector.rollback() 
       
    def UpdateURLStatusCrawler(self,status,Url):
        Url = self.SlashHTML(Url)
        try:
            self.Curser.execute("update Url_Container set status='%s' where URLName = '%s'" % (status,Url))
            self.Connector.commit()
        except:
            self.Connector.rollback() 

    def InsertKeyWord(self,Data):
        Data = self.SlashHTML(Data)
        try:
            self.Curser.execute("Insert into KeyWords (KeyWords) values ('%s')" % (Data))
            self.Connector.commit()
        except:
            self.Connector.rollback()

    def GetWordID(self,Data):
        Data = self.SlashHTML(Data)
        try:
            self.Curser.execute("Select KeyWord_ID from KeyWords where KeyWords = ('%s')" % (Data))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()

    def InsertKeyWordPositionTitle(self,URL_ID,KeyWord_ID,Position):        
        try:
           self.Connector.execute("insert into dbo.KeyWordsPosition_Titles (URL_ID,KeyWord_ID,Position) values (%d,%d,%d)" % (URL_ID,KeyWord_ID,Position))
           self.Connector.commit()
        except:
          self.Connector.rollback()

    def InsertKeyWordPositionHeaders(self,URL_ID,KeyWord_ID,Position):        
        try:
            self.Connector.execute("insert into dbo.KeyWordsPosition_Headers(URL_ID,KeyWord_ID,Position) values (%d,%d,%d)" % (URL_ID,KeyWord_ID,Position))
            self.Connector.commit()
        except:
            self.Connector.rollback()

    def InsertKeyWordPositionParagraph(self,URL_ID,KeyWord_ID,Position):        
        try:
            self.Connector.execute("insert into dbo.KeyWordsPosition_Paragraphs(URL_ID,KeyWord_ID,Position) values (%d,%d,%d)" % (URL_ID,KeyWord_ID,Position))
            self.Connector.commit()
        except:
            self.Connector.rollback()

      #self.Curser.execute("Select u.File_ID,FileName,Position from Url_Container u,KeyWords w, KeyWord_File_Position kwp where w.Word_ID = kwp.Word_ID and u.File_ID = kwp.File_ID and Word = '%s'" % (Word))
    def GetNumberOfUrlsInDB(self):
        try:
            self.Curser.execute("SELECT COUNT(*) FROM Url_Container")
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()
   
    def GetNumberOfNoneCrawled(self):
        try:
            self.Curser.execute("SELECT COUNT(*) FROM URL_Container where Status = 'N'")
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()

## Indexer
    def DeleteURLbyID(self,URL_ID):
         try:
            self.Connector.execute("delete from Url_Container where URL_ID = %d" % (URL_ID))
            self.Connector.commit()
         except:
            self.Connector.rollback()

#Extra test
    def InsertHTMLData(self,URL_ID,htmlData):
        try:
            self.Connector.execute("insert into HtmlData (URL_ID, htmlData) values (%d,'%s')" % (URL_ID,htmlData))
            self.Connector.commit()
        except:
            self.Connector.rollback()

    def GetHTMLdata(self,URL_ID):
        try:
            self.Curser.execute("select htmlData from HTMLData where URL_ID = %d" % (URL_ID))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()

    #3mo
    def InsertImageName(self, Data):
        Data = self.SlashHTML(Data)
        try:
            self.Curser.execute("insert into ImageNames (ImageFileName) values ('%s')" % (Data))
            self.Connector.commit()
        except:
            self.Connector.rollback()

    def GetImageID_ByName(self, status):
        status = self.SlashHTML(status)
        try:
            self.Curser.execute("Select ImageFile_ID from ImageNames where ImageFileName = ('%s')" % (status))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()

    def InsertImageKeyWord(self, Data):
        Data = self.SlashHTML(Data)
        try:
            self.Curser.execute("insert into ImageKeyWords (ImageKeyWords) values ('%s')" % (Data))
            self.Connector.commit()
        except:
            self.Connector.rollback()

    def GetURLByID(self, ID):
        try:
            self.Curser.execute("Select URLName from Url_Container where URL_ID = %d" % (ID))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()

    def Link_URL_KeyWords(self,urlid,imageKeywordId,Imageurl):
        Imageurl = self.SlashHTML(Imageurl)
        try:
           self.Curser.execute("Insert into KeyWordLocationURL (URL_ID,ImageKeyWords_ID,ImageFile_ID) values (%d,%d,'%s')" % (urlid,imageKeywordId,Imageurl))
           self.Connector.commit()
        except:
          self.Connector.rollback()

    def GetImageWordID(self, Data):
        Data = self.SlashHTML(Data)
        try:
            self.Curser.execute("Select ImageKeyWord_ID from ImageKeyWords where ImageKeyWords = ('%s')" % (Data))
            result = self.Curser.fetchall()
            self.Connector.commit()
            return result
        except:
            self.Connector.rollback()

    def ImageKeyWordDoesExist(self,Data):
        Data = self.SlashHTML(Data)
        self.Curser.execute("Select ImageKeyWords from ImageKeyWords where ImageKeyWords = ('%s')" % (Data))
        data = self.Curser.fetchone()
        if data:
            return True
        else :
            return False
    
    def SlashHTML(self,Data):
        htmlSlashed = str(Data)
        l = ["'", ]
        for i in l:
            if i in htmlSlashed:
                htmlSlashed = htmlSlashed.replace(i, '\''+i)
        return htmlSlashed

    def CloseConnection(self):
        self.Connector.close()
