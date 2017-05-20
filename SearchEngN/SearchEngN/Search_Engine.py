#import IndexerMod
import ThreadingMod
import threading
import time
##MyCraweler.Crawel()
##MyIndexer = IndexerMod.Indexer()

##MyIndexer.StartIndexing()

#MyThreads=[]

#MyCraweler.Crawel("https://moz.com/top500")
#MyCraweler.Crawel("https://www.facebook.com/")
#MyCraweler.Crawel("https://www.crummy.com/software/BeautifulSoup/bs4/doc/")
#MyCraweler.Crawel("http://wordpress.stackexchange.com/questions/158015/unwanted-crawl-delay-10-line-added-to-my-robots-txt")
#MyCraweler.Crawel("https://www.youtube.com/")
#MyCraweler.Crawel("http://wordpress.stackexchange.com/questions/158015/unwanted-crawl-delay-10-line-added-to-my-robots-txt")
#MyCraweler.Crawel("https://docs.python.org/3/library/re.html")

threadLock = threading.Lock()
threadLockIndexer = threading.Lock()
MyThreads = []
CrawlerThreadNumber = input('Enter number of Crawler threads: ')
IndexerThreadNumber = input('Enter number of Indexer threads: ')
# Create new threads


for i in range(int(CrawlerThreadNumber)):
    try:
       MyThreads.append(ThreadingMod.myThread(1, "Thread-"+str(i+1), 1,threadLock,'C'))
    except:
       print("Error: unable to start new thread")

for i in range(int(IndexerThreadNumber)):
    try:
       MyThreads.append(ThreadingMod.myThread(1, "Thread-Indexer-"+str(i+1), 1,threadLockIndexer,'I'))
    except:
       print("Error: unable to start new thread")

MyThreads.append(ThreadingMod.myThread(1, "Thread-Q Saver", 1,threadLock,'Q'))
MyThreads.append(ThreadingMod.myThread(1, "Thread-Indexer Saver", 1,threadLockIndexer,'W'))

MyThreads[int(CrawlerThreadNumber) + int(IndexerThreadNumber)    ].start() #Start CralwerSaver before the others
MyThreads[int(CrawlerThreadNumber) + int(IndexerThreadNumber) + 1].start() #Start IndexerSaver before the others

time.sleep(2)

#for i in range(int(CrawlerThreadNumber) + int(IndexerThreadNumber) -2):
#    MyThreads[i].start()

for i in MyThreads:
    if not ((i.name == "Thread-Q Saver") | (i.name == "Thread-Indexer Saver")):
        i.start()


# Wait for all threads to complete
for i in MyThreads:
    i.join()


print( "Exiting Main Thread")