#from nltk.stem.porter import * 
import DataBase
import urllib.request
from bs4 import *
import os
import re
import threading
import time

import warnings
warnings.filterwarnings("ignore", category=UserWarning, module='bs4')


import nltk
#lemma = nltk.wordnet.WordNetLemmatizer()
#lemma.lemmatize('article')
#lemma.lemmatize('leaves')


class Indexer:
    def __init__(self):
        #self.StoppingWords =  re.split('\n',open('StoppingWords.txt','r').read())
        self.IDKWhatToNameThisButTheseAreTheWordsIDKWhatToDoAboutThanksForReading = ['',' ']
        self.lemma = nltk.wordnet.WordNetLemmatizer()
        self.DataBaseMaster = DataBase.DataBaseMaster()
        self.FilesLocation = self.GetFilesLocationDirectory()
        self.ImagesFileLocation = self.GetImageFilesLocationDirectory();
        self.Soup = BeautifulSoup()
        self.Tags = ["h1","h2","h3","h4","h5","h6","br","p","pre"]

    def GetFilesLocationDirectory(self):
        CurrentDirectory = os.path.dirname(os.path.realpath(__file__))
        HtmlFilesDirectory = 'MyHtmlFiles'
        FilesLocation = os.path.join(CurrentDirectory, HtmlFilesDirectory)
        return FilesLocation

    def GetImageFilesLocationDirectory(self):
        CurrentDirectory = os.path.dirname(os.path.realpath(__file__))
        HtmlFilesDirectory = 'MyImages'
        FilesLocation = os.path.join(CurrentDirectory, HtmlFilesDirectory)
        FilesLocation = 'C:/Users/vedio_000/Documents/GithubTemp/APT-SearchEngine/APT/APT/Images/MyImages/'
        return FilesLocation

    def StartIndexingSaver(self, ThreadLock):
         ThreadLock.acquire()
         Result = self.DataBaseMaster.GetURLIDByStatus('W')
         ThreadLock.release()
         if Result:
            for i in range(len(Result)):
                File_ID = int(Result[i][0]) 
                self.IndexerMechanism(ThreadLock, File_ID)
         print("Thread-Indexer Saver Leaving")

    def StartIndexing(self, ThreadLock):
        while True:
            File_ID = -1;
            ThreadLock.acquire()
            Result = self.DataBaseMaster.GetURLIDByStatus('C')

            if Result:
                File_ID = int(Result[0][0]) 
                self.DataBaseMaster.UpdateURLStatus('W', File_ID)
            ThreadLock.release()

            if File_ID != -1:
                 self.IndexerMechanism(ThreadLock,File_ID)


    def IndexerMechanism(self, ThreadLock, File_ID):
        Position = 0
        TitlePosition=0
        HeaderPosition =0  
           
        try:
            HtmlData = str(self.DataBaseMaster.GetHTMLdata(File_ID))
        except:
            print ("EXCEPTION IN FILE " + str(File_ID) + ", CANT OPEN: DELETEING FROM DATABASE!")
            #self.DataBaseMaster.DeleteURLbyID(File_ID);
            self.DataBaseMaster.UpdateURLStatus('D',File_ID)
            return

        self.DataBaseMaster.DeleteDataBeforeIndexing(File_ID)
        Title ,Headers, Paragraphs = self.GetTextFromHtml(HtmlData, File_ID, ThreadLock)
        if Title == []:
            return
                
        for word in Title:
            word = word.lower()
            #if word not in self.StoppingWords:
            if word not in self.IDKWhatToNameThisButTheseAreTheWordsIDKWhatToDoAboutThanksForReading:
                stemedWord = self.lemma.lemmatize(word)
                ThreadLock.acquire()
                if not self.DataBaseMaster.KeyWordDoesExist(stemedWord):
                    self.DataBaseMaster.InsertKeyWord(stemedWord)
                ThreadLock.release()
                Result2 = self.DataBaseMaster.GetWordID(stemedWord)
                Word_ID = int(Result2[0][0])
                self.DataBaseMaster.InsertKeyWordPositionTitle(File_ID,Word_ID,TitlePosition)
                TitlePosition = TitlePosition + 1

        for word in Headers:
            word = word.lower()
            #if word not in self.StoppingWords:
            if word not in self.IDKWhatToNameThisButTheseAreTheWordsIDKWhatToDoAboutThanksForReading:
                stemedWord = self.lemma.lemmatize(word)
                ThreadLock.acquire()
                if not self.DataBaseMaster.KeyWordDoesExist(stemedWord):
                    self.DataBaseMaster.InsertKeyWord(stemedWord)
                ThreadLock.release()
                Result2 = self.DataBaseMaster.GetWordID(stemedWord)
                Word_ID = int(Result2[0][0])
                self.DataBaseMaster.InsertKeyWordPositionHeaders(File_ID,Word_ID,HeaderPosition)
                HeaderPosition = HeaderPosition + 1

        for word in Paragraphs:
            word = word.lower()
            #if word not in self.StoppingWords:
            if word not in self.IDKWhatToNameThisButTheseAreTheWordsIDKWhatToDoAboutThanksForReading:
                stemedWord = self.lemma.lemmatize(word)
                ThreadLock.acquire()
                if not self.DataBaseMaster.KeyWordDoesExist(stemedWord):
                    self.DataBaseMaster.InsertKeyWord(stemedWord)
                ThreadLock.release()
                Result2 = self.DataBaseMaster.GetWordID(stemedWord)
                Word_ID = int(Result2[0][0])
                self.DataBaseMaster.InsertKeyWordPositionParagraph(File_ID,Word_ID,Position)
                Position = Position + 1

        totalNumberOfWords = Position+HeaderPosition+TitlePosition
        self.DataBaseMaster.UpdateNumberOfWords(File_ID,totalNumberOfWords)
        self.DataBaseMaster.UpdateURLStatus('I', File_ID)
        print(threading.current_thread().name + " INDEXED FILE: " + str(File_ID))
    #else :
        #print("FINISHED ALL THE SITES PLEASE ADD MORE")


    def GetTextFromHtml(self,HtmlData, URL_ID,ThreadLock):
        MySoup =  BeautifulSoup(HtmlData)
        PageTitle=[]
        Texts = []
        Headers = []
        regex = re.compile('[^a-zA-Z \n]')

        try:
            PageTitleTemp = MySoup.find('title').string
            temp = PageTitleTemp.replace('\\' , '')
            temp = temp.replace('\'' , '\'\'')
                
            self.DataBaseMaster.UpdateURLTitle(URL_ID,temp);
            PageTitle = regex.sub(' ',PageTitleTemp)
            PageTitle = re.split('([\t\s\n\r])|x[a-zA-Z]*', PageTitle)

        except:
            print ("EXCEPTION IN FILE " + str(URL_ID) + ", NO TITLE: DELETEING FROM DATABASE!")
            #self.DataBaseMaster.DeleteURLbyID(File_ID);
            self.DataBaseMaster.UpdateURLStatus('D',URL_ID)
            return PageTitle ,Headers,Texts


        for i in self.Tags:
            Paragraph = MySoup.findAll(i) 
            for j in Paragraph:
                if j.string :
                   SplittedWords = regex.sub('', j.string)
                   SplittedWords = re.split('([\t\s\n\r])|x[a-zA-Z]*', SplittedWords)
                   if len(SplittedWords) >1:
                       for k in SplittedWords:
                           if i[0] == 'h':
                               Headers.append(k)
                           else:
                               Texts.append(k)
                   else:
                        if i[0] == 'h':
                            Headers.append(j.string)
                        else:
                            Texts.append(j.string)

        self.ImageSearch(MySoup,URL_ID,ThreadLock)        #UNCOMMENT TO CRAWL IMAGES
        PageTitle = list(filter(None, PageTitle))
        Headers = list(filter(None, Headers))
        Texts = list(filter(None, Texts))
        return PageTitle ,Headers,Texts



    def ImageSearch(self,MySoup,URL_ID,ThreadLock):
        ignoreList = ['.img', '.php']
        #with open('1.txt') as html:
        # = open('1.txt', encoding="utf8")
        #content = file.read()

        #soup = bs4.BeautifulSoup(content)
        url = str(self.DataBaseMaster.GetURLByID(URL_ID)[0][0])
        pos=0
        AllImages =  MySoup.findAll("img")
        for image in AllImages:
        #################Download##########################
            src = image.get('src','')
            if src != '':
                src = re.split(r'[?]', src)
                src = src[0]
                if src!='':
                    imgname=""
                    type = src[-4:]
                    type = str(type).lower()
                    if type[0] == "." and type not in ignoreList:
                        ThreadLock.acquire()
                        if not self.DataBaseMaster.ImageLinkDoesExist(src):
                            self.DataBaseMaster.InsertImageLink(src)
                            ThreadLock.release()
                            if src.lower().startswith("http"):
                                imgname = str(URL_ID) + "_" + str(pos) + type
                                try:
                                    urllib.request.urlretrieve(src, self.ImagesFileLocation + '\\' + imgname)
                                    self.DataBaseMaster.InsertImageName(imgname)
                                except:
                                    continue
                            else:
                                #get base url from data base
                                image_url = urllib.parse.urljoin(url,src)
                                imgname = str(URL_ID) + "_" + str(pos) + type
                                try:
                                    urllib.request.urlretrieve(src, self.ImagesFileLocation + '\\' + imgname)
                                    self.DataBaseMaster.InsertImageName(imgname)
                                except:
                                    continue
                            pos=pos+1

                                # store in database
                ###########################################################

                            remove_format=src
                            remove_format = re.split(r'[/]', remove_format)
                            index=len(remove_format)-1
                            remove_format=remove_format[index]
                            remove_format = remove_format[:-4]  # removing last 4 chars(.png,.jpg.....)
                            remove_format = remove_format.lower()

                            remove_format = re.split(r'[/\n\r\s+,_-]', remove_format)
         
                            for i in remove_format:
                                temp1 =  self.lemma.lemmatize(i)
                                if temp1 != '':
                                    #if temp1 not in self.StoppingWords:
                                    ThreadLock.acquire()
                                    if not self.DataBaseMaster.ImageKeyWordDoesExist(temp1):
                                        self.DataBaseMaster.InsertImageKeyWord(temp1)
                                    ThreadLock.release()
                                    keyid1 = self.DataBaseMaster.GetImageWordID(temp1)
                                    Word_ID1 = int(keyid1[0][0])
                                    Image_ID1=int(self.DataBaseMaster.GetImageID_ByName(imgname)[0][0])
                                    self.DataBaseMaster.Link_URL_KeyWords(URL_ID,Word_ID1,Image_ID1)

                    ####################################################################################
                            alt=image.get('alt','')
                            alt = alt.lower()

                            alt = re.split(r"[\\/\n\r\s+,_-]", alt)


                            for b in alt:
                                temp= self.lemma.lemmatize(b)
                                if temp !='':
                                    #if temp not in self.StoppingWords:
                                    ThreadLock.acquire()
                                    if not self.DataBaseMaster.ImageKeyWordDoesExist(temp):
                                        self.DataBaseMaster.InsertImageKeyWord(temp)
                                    ThreadLock.release()
                                    keyid=self.DataBaseMaster.GetImageWordID(temp)
                                    Word_ID = int(keyid[0][0])
                                    Image_ID = int(self.DataBaseMaster.GetImageID_ByName(imgname)[0][0])
                                    self.DataBaseMaster.Link_URL_KeyWords(URL_ID, Word_ID1,Image_ID)  # 1: from musgi(file opened id),src:get id from imagename
                        else:
                            ThreadLock.release()                          