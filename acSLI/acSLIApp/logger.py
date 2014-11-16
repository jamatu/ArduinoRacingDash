import ac
from datetime import datetime
import traceback


class _Logger:

    logFile = 0

    def __init__(self):
        try:
            self.logFile = open('apps/python/acSLI/acSLI.txt', 'w')
        except Exception as e:
            self.warning("Couldn't open log: %s" % e)

    def _ppMsg(self, lvl, msg):
        return "[acSLI] [{0!s}] {1}: {2}".format(datetime.now().strftime('%Y-%m-%d %H:%M:%S'), lvl, msg)
        
    def info(self, msg):
        m = self._ppMsg("INFO", msg)
        self.logFile.write(m + "\n")
        ac.console(m)
        ac.log(m)
        
    def warning(self, msg):
        m = self._ppMsg("WARNING", msg)
        self.logFile.write(m + "\n")
        ac.console(m)
        ac.log(m)
        
    def error(self, msg):
        m = self._ppMsg("ERROR", msg)
        self.logFile.write(m + "\n")
        ac.console(m)
        ac.log(m)
        traceback.print_exc(file=self.logFile)
        
    def log(self, msg):
        m = self._ppMsg("LOG", msg)
        self.logFile.write(m + "\n")
        ac.log(m)
        
    def msg(self, msg):
        m = self._ppMsg("MSG", msg)
        self.logFile.write(m + "\n")
        ac.console(m)

    def blank(self, msg):
        self._ppMsg("MSG", msg)

Log = _Logger()