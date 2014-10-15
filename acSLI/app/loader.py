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

    cfgTickFreq = 6
    cfgEnableUpdater = 1

    def __init__(self):
        global instance, cfgPath
        instance = self

        try:
            self.config = Config(cfgPath)
            self.cfgPort = str(self.config.getOption("SETTINGS", "port", True, self.cfgPort)).upper()
            self.cfgRemoteVersion = str(self.config.getOption("SETTINGS", "remoteVersion", True, self.cfgRemoteVersion))
            self.cfgSpeedUnit = str(self.config.getOption("SETTINGS", "unitSpeed", True, self.cfgSpeedUnit)).upper()
            self.cfgStartPage = int(self.config.getOption("SETTINGS", "startupPage", True, self.cfgStartPage))
            self.cfgIntensity = int(self.config.getOption("SETTINGS", "intensity", True, self.cfgIntensity))

            self.cfgTickFreq = int(self.config.getOption("ADVANCED", "tickFrequency", True, self.cfgTickFreq))
            self.cfgEnableUpdater = int(self.config.getOption("ADVANCED", "enableUpdater", True, self.cfgEnableUpdater))

        except Exception as e:
            Log.error("Loading Config File: %s" % e)

    def rewriteConfig(self):
        self.config.updateOption("SETTINGS", "port", self.cfgPort, True)
        self.config.updateOption("SETTINGS", "unitSpeed", self.cfgSpeedUnit, True)
        self.config.updateOption("SETTINGS", "startupPage", self.cfgStartPage, True)
        self.config.updateOption("SETTINGS", "intensity", self.cfgIntensity, True)
        self.config.updateOption("ADVANCED", "tickFrequency", self.cfgTickFreq, True)
        self.config.updateOption("ADVANCED", "enableUpdater", self.cfgEnableUpdater, True)