from app.logger import Logger
from app.utils import Config

Log = Logger()
instance = 0
cfgPath = "apps/python/acSLI.cfg"

config = 0
cfgRemoteVersion = "*"
cfgPort = "AUTO"
cfgSpeedUnit = "MPH"
cfgStartPage = 0
cfgIntensity = 0

class ConfigLoader:

    def __init__(self):
        global instance, cfgPath

        try:
            self.config = Config(cfgPath)
            self.cfgPort = str(self.config.getOption("SETTINGS", "port", True, cfgPort)).upper()
            self.cfgRemoteVersion = str(self.config.getOption("SETTINGS", "remoteVersion", True, cfgRemoteVersion))
            self.cfgSpeedUnit = str(self.config.getOption("SETTINGS", "unitSpeed", True, cfgSpeedUnit)).upper()
            self.cfgStartPage = str(self.config.getOption("SETTINGS", "startupPage", True, cfgStartPage))
            self.cfgIntensity = str(self.config.getOption("SETTINGS", "intensity", True, cfgIntensity))

            if not(self.cfgStartPage.isdigit() or int(self.cfgStartPage.isdigit()) > -1 or int(self.cfgStartPage.isdigit()) < 6):
                self.cfgStartPage = 0

            instance = self

        except Exception as e:
            Log.error("Loading Config File: %s" % e)
