import threading
import time
import CrawelerMod
import IndexerMod

class myThread (threading.Thread):
    def __init__(self, threadID, name, counter,Lock, CorI):
        threading.Thread.__init__(self)
        self.threadID = threadID
        self.name = name
        self.counter = counter
        self.Lock = Lock
        self.choice = CorI

    def run(self):
        print( "Starting " + self.name)
        if  self.choice == 'C':
            MyCraweler = CrawelerMod.Craweler()
            MyCraweler.Crawel(self.Lock)
        elif  self.choice == 'I':
            MyIndexer = IndexerMod.Indexer()
            MyIndexer.StartIndexing(self.Lock)
        elif self.choice == 'Q':
            MyCraweler = CrawelerMod.Craweler()
            MyCraweler.CrawelQOnly(self.Lock)
        elif self.choice == 'W':
            MyIndexer = IndexerMod.Indexer()
            MyIndexer.StartIndexingSaver(self.Lock)

def print_time(threadName, delay, counter):
    while counter:
        time.sleep(delay)
        print ("%s: %s" % (threadName, time.ctime(time.time())))
        counter -= 1
