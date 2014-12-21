import os
import shutil
from acSLIApp.logger import Log
from acSLIApp.utils import Config

instance = 0
cfgPath = "apps/python/acSLI/acSLI.ini"


class ConfigLoader:

    config = 0
    cfgRemoteVersion = "*"
    cfgPort = "AUTO"
    cfgSpeedUnit = "MPH"
    cfgFuelDisp = "LAP ESTIMATE"
    cfgStartPage = 0
    cfgIntensity = 0

    cfgTickFreq = 6
    cfgEnableUpdater = 1
    cfgLapOffset = 0

    cfgBrakeEnable = 0
    cfgBrakeTol = 30
    cfgBrakeSens = 3

    def __init__(self):
        global instance, cfgPath
        instance = self

        try:
            if os.path.exists("apps/python/acSLI.cfg"):
                shutil.move("apps/python/acSLI.cfg", "apps/python/acSLI/")

            self.config = Config(cfgPath)
            self.cfgPort = str(self.config.getOption("SETTINGS", "port", True, self.cfgPort)).upper()
            self.cfgRemoteVersion = str(self.config.getOption("SETTINGS", "remoteVersion", True, self.cfgRemoteVersion))
            self.cfgSpeedUnit = str(self.config.getOption("SETTINGS", "unitSpeed", True, self.cfgSpeedUnit)).upper()
            self.cfgFuelDisp = str(self.config.getOption("SETTINGS", "fuelDisplay", True, self.cfgFuelDisp)).upper()
            self.cfgStartPage = int(self.config.getOption("SETTINGS", "startupPage", True, self.cfgStartPage))
            self.cfgIntensity = int(self.config.getOption("SETTINGS", "intensity", True, self.cfgIntensity))

            self.cfgTickFreq = int(self.config.getOption("ADVANCED", "tickFrequency", True, self.cfgTickFreq))
            self.cfgEnableUpdater = int(self.config.getOption("ADVANCED", "enableUpdater", True, self.cfgEnableUpdater))
            self.cfgLapOffset = int(self.config.getOption("ADVANCED", "lapOffset", True, self.cfgLapOffset))

            self.cfgBrakeEnable = int(self.config.getOption("BRAKE-VIBE", "enable", True, self.cfgBrakeEnable))
            self.cfgBrakeTol = int(self.config.getOption("BRAKE-VIBE", "tolerance", True, self.cfgBrakeTol))
            self.cfgBrakeSens = int(self.config.getOption("BRAKE-VIBE", "sensitivity", True, self.cfgBrakeSens))

        except Exception as e:
            Log.error("Loading Config File: %s" % e)

    def rewriteConfig(self):
        self.config.updateOption("SETTINGS", "port", self.cfgPort, True)
        self.config.updateOption("SETTINGS", "unitSpeed", self.cfgSpeedUnit, True)
        self.config.updateOption("SETTINGS", "fuelDisplay", self.cfgFuelDisp, True)
        self.config.updateOption("SETTINGS", "startupPage", self.cfgStartPage, True)
        self.config.updateOption("SETTINGS", "intensity", self.cfgIntensity, True)