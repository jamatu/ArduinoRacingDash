from app.logger import Logger
from app.utils import Config

Log = Logger()
instance = 0
cfgPath = "apps/python/acSLI.cfg"


class ConfigLoader:

    config = 0
    cfgRemoteVersion = "*"
    cfgPort = "AUTO"
    cfgSpeedUnit = "MPH"
    cfgStartPage = 0
    cfgIntensity = 0

    def __init__(self):
        global instance, cfgPath
        instance = self

        try:
            self.config = Config(cfgPath)
            self.cfgPort = str(self.config.getOption("SETTINGS", "port", True, self.cfgPort)).upper()
            self.cfgRemoteVersion = str(self.config.getOption("SETTINGS", "remoteVersion", True, self.cfgRemoteVersion))
            self.cfgSpeedUnit = str(self.config.getOption("SETTINGS", "unitSpeed", True, self.cfgSpeedUnit)).upper()
            self.cfgStartPage = str(self.config.getOption("SETTINGS", "startupPage", True, self.cfgStartPage))
            self.cfgIntensity = str(self.config.getOption("SETTINGS", "intensity", True, self.cfgIntensity))

            if not(self.cfgStartPage.isdigit() or int(self.cfgStartPage.isdigit()) > -1 or int(self.cfgStartPage.isdigit()) < 6):
                self.cfgStartPage = 0

        except Exception as e:
            Log.error("Loading Config File: %s" % e)

    def rewriteConfig(self):
        self.config.updateOption("SETTINGS", "port", self.cfgPort, True)
        self.config.updateOption("SETTINGS", "unitSpeed", self.cfgSpeedUnit, True)
        self.config.updateOption("SETTINGS", "startupPage", self.cfgStartPage, True)
        self.config.updateOption("SETTINGS", "intensity", self.cfgIntensity, True)