from urllib.request import urlopen
from html.parser import HTMLParser    
from urllib import parse
import re
import os
import http
import DataBase
try:
    from itertools import imap
except ImportError:
    imap=map

class Parser(HTMLParser):


    def __init__(self):
        HTMLParser.__init__(self)
        self.MyUrls = []
        self.BaseUrl = ""
        self.DisAllowedUrls =[]
        self.IgnoringTypes = ['.css' ,'js','.png','gif']
        self.UrlDelay = 0
        self.DataBaseMaster = DataBase.DataBaseMaster()
        self.HtmlData = " "
    def handle_starttag(self, tag, attrs): # built in function to get sublinks 
        if tag =='a':
            for (key, value) in attrs:
                if key == 'href':
                   NewUrl = parse.urljoin(self.BaseUrl, value)
                   if NewUrl not in self.MyUrls:
                       if not any(imap(NewUrl.__contains__, self.IgnoringTypes)):
                          Allowed = True
                          for j in  self.DisAllowedUrls:
                             if j not in NewUrl:
                                 Allowed = False
                                 break
                          if Allowed:
                             self.MyUrls.append(NewUrl)

    #open a link and get the html data then decode it and retrive only the urls
    def GetLinksFromHtml(self,Url,ThreadLock):
        saveCompleted = False;
        if self.FindDisAllowedUrls(Url) == False:
            return [],False,0,saveCompleted
        self.BaseUrl = Url
        try:
            OpenedUrl = urlopen(Url)
        except:
            return [],False,0,saveCompleted
        HtmlDecodedData = self.GetDecodedData(OpenedUrl)
        if not HtmlDecodedData:
            return [],False,0,saveCompleted
        ThreadLock.acquire()
        Result = self.DataBaseMaster.GetURLID(Url)
        File_ID = int(Result[0][0]) 
        saveCompleted =  self.SaveHtmlToFile(self.HtmlData)
        if saveCompleted == False:
            ThreadLock.release()
            return [],False,0,False,saveCompleted
        ThreadLock.release()

        if self.DataBaseMaster.GetNumberOfUrlsInDB()[0][0] >= 500:
           return [],False,0,saveCompleted
        self.feed(HtmlDecodedData)
        SplittedHtml = re.split('"',HtmlDecodedData)
        for i in SplittedHtml:
            if str(i).startswith("http://") or str(i).startswith("https://"):
                if i not in self.MyUrls:
                     if  not any(imap(i.__contains__, self.IgnoringTypes)):
                         Allowed = True
                         for j in  self.DisAllowedUrls:
                             if j in i:
                                 Allowed = False
                                 break
                         if Allowed:
                            self.MyUrls.append(i)
        return self.MyUrls,True,self.UrlDelay,saveCompleted


    def GetDecodedData(self,OpenedUrl):     
        try:
            HtmlEncodedData = OpenedUrl.read()
        except UnicodeDecodeError as Error : 
            return False
        except http.client.IncompleteRead as e: 
            HtmlEncodedData = e.partial
        self.HtmlData = HtmlEncodedData
        HeaderContents = OpenedUrl.getheader('Content-Type')
        if str(HeaderContents).count('=') < 1 :
            try:
                FinalData = str(HtmlEncodedData.decode("utf-8"))
                return FinalData
            except:
                return str(HtmlEncodedData)
        else:
             TempStr = re.split('=',HeaderContents)
             EncodingType = TempStr[1]
             try:
                 HtmlDecodedData = HtmlEncodedData.decode(EncodingType)
                 return str(HtmlDecodedData)
             except:
                 try:
                    FinalData = str(HtmlEncodedData.decode("utf-8"))
                    return FinalData
                 except:
                    return str(HtmlEncodedData)

    def SaveHtmlToFile(self,HtmlDecodedData):
        try:
            File_ID = int(self.DataBaseMaster.GetURLID(self.BaseUrl)[0][0])
            htmlSlashed = str(self.HtmlData)
            l = ["'", ]
            for i in l:
                if i in htmlSlashed:
                    htmlSlashed = htmlSlashed.replace(i, '\''+i)
            self.DataBaseMaster.InsertHTMLData(File_ID, htmlSlashed)
            return True
        except:
            return False;
        
        #CurrentDirectory = os.path.dirname(os.path.realpath(__file__))
        #FileName = str(File_ID) + '.html'
        #HtmlFilesDirectory = 'MyHtmlFiles'
        #FilePath = os.path.join(CurrentDirectory, HtmlFilesDirectory, FileName)
        #try:
        #    NewFile = open(FilePath, 'w+')
        #    #print(FilePath)
        #    try:
        #        NewFile.write(str(self.HtmlData))
        #    except:
        #        #print(self.BaseUrl +" Failed to save")
        #        try:
        #            if os.path.exists(FilePath):
        #                #print("Deleting " + FilePath)
        #                NewFile.close()
        #                os.remove(FilePath)
        #                return False
        #            else:
        #                print("file not found")
        #        except OSError as e:
        #             print ("Failed to remove %s\nError is: %s" % (FilePath,e))
        #    NewFile.close()
        #    return True
        #except IOError:
        #    print("Wrong path provided")

    def FindDisAllowedUrls(self,Url):
        MainUrl = re.split('/',Url)
        try:
            RobotExclusion = urlopen('http://'+MainUrl[2]+'/robots.txt')
            RobotExclusion = self.GetDecodedData(RobotExclusion)
            RobotExclusion = re.split('[\r\n]+',str(RobotExclusion))
            if len(RobotExclusion) < 3:
                return []
            i = 0
            self.DisAllowedUrls = []
            self.UrlDelay = 0
            for i in range(len(RobotExclusion)):
                if RobotExclusion[i] == 'User-Agent: *' or RobotExclusion[i] == 'User-agent: *':
                    j = i+1
                    while j <len(RobotExclusion):
                        if RobotExclusion[j] == 'Disallow:' or RobotExclusion[j] == 'Allow:/':
                           self.DisAllowedUrls = []
                           return
                        if 'Crawl-delay: ' in RobotExclusion[j]:
                            return False
                            #print("Delay")
                            UrlDelay = int(RobotExclusion[j][13:])
                        if RobotExclusion[j] == 'Disallow:/':
                            return False
                        if 'Disallow: ' in RobotExclusion[j]:
                            self.DisAllowedUrls.append(RobotExclusion[j][10:])
                        if 'User-Agent:' in RobotExclusion[j] or 'User-agent:' in RobotExclusion[j]:
                            break
                        j = j+1
                        i = i+1
                i = i+1
            self.DisAllowedUrls = self.DisAllowedUrls[0:len(self.DisAllowedUrls)]
            return True
        except:
            self.DisAllowedUrls = []