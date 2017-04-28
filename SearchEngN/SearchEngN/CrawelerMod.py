import ParserMod
import DataBase
import threading
import time
class Craweler:
        
    def __init__(self):
        self.MyDataBaseMaster = DataBase.DataBaseMaster()
        self.MyParser = ParserMod.Parser() 


    def Crawel(self,ThreadLock):
        Terminate = time.time()
        while True:
            if time.time() - Terminate  > 20:
                print(threading.current_thread().name+" dead")
                return
            #get the url from the container and start crawling for each thread (container is initlally  filed with few urls)
            Url =""
            ThreadLock.acquire()
            Temp = self.MyDataBaseMaster.GetURLIDByStatus('N')
            if Temp:
                Url = Temp[0][1]
                self.MyDataBaseMaster.UpdateURLStatusCrawler('Q',Url)
                ThreadLock.release()
            else:
                ThreadLock.release()

            if Url:
                self.GetLinkate3wa(Url, ThreadLock)

            Terminate = time.time()

    def CrawelQOnly(self,ThreadLock):
        ThreadLock.acquire()
        Temp = self.MyDataBaseMaster.GetURLIDByStatus('Q')
        ThreadLock.release()
        if Temp:
            for i in range(len(Temp)):
                Url = Temp[i][1]
                self.GetLinkate3wa(Url, ThreadLock)
        print("Thread-Q Saver Leaving")
        return

    def GetLinkate3wa(self,Url,ThreadLock):
            print(threading.current_thread().name +" has:" + Url)
            UrlsRetrived,Succes,UrlDelay,saveCompleted = self.MyParser.GetLinksFromHtml(Url,ThreadLock)

            if  Succes:
                for i in UrlsRetrived:
                    try:
                        if not self.MyDataBaseMaster.URLDoesExist(i):
                            if self.MyDataBaseMaster.GetNumberOfUrlsInDB()[0][0] < 500:
                                ThreadLock.acquire()
                                self.MyDataBaseMaster.InsertNewUrl(i,'N',0)
                                ThreadLock.release()
                    except:
                        ThreadLock.release()
                        continue
            else:
                print (threading.current_thread().name+" cant Open site: " + Url)
                self.MyDataBaseMaster.UpdateURLStatusCrawler('E',Url)
            if saveCompleted:
                self.MyDataBaseMaster.UpdateURLStatusCrawler('C',Url)