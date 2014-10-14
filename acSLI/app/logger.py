import ac
from datetime import datetime
import traceback


class Logger:

    logFile = 0

    def __init__(self):
        try:
            self.logFile = open('apps/python/acSLI/log.txt', 'w')
        except Exception as e:
            self.warning("Couldn't open log: %s" % e)

    def _ppMsg(self, lvl, msg):
        return "[acSLI] [{0!s}] {1}: {2}".format(datetime.now().strftime('%Y-%m-%d %H:%M:%S'), lvl, msg)
        
    def info(self, msg):
        ac.console(self._ppMsg("INFO", msg))
        ac.log(self._ppMsg("INFO", msg))
        
    def warning(self, msg):
        ac.console(self._ppMsg("WARNING", msg))
        ac.log(self._ppMsg("WARNING", msg))
        
    def error(self, msg):
        ac.console(self._ppMsg("ERROR", msg))
        ac.log(self._ppMsg("ERROR", msg))
        traceback.print_exc(file=self.logFile)
        
    def log(self, msg):
        ac.log(self._ppMsg("LOG", msg))
        
    def msg(self, msg):
        ac.console(self._ppMsg("MSG", msg))