#######################################################    
#
#    Author: Turnermator13
#
#    Please find this on github at:
#    -https://github.com/Turnermator13/ArduinoRacingDash
#
#######################################################

import os.path
import configparser
from acSLIApp.logger import Log


colours = {
    "white": [255, 255, 255],
    "yellow": [247, 183,  9],
    "green": [151, 229, 68],
    "red": [255, 15, 15]
}


def rgb(colour, a=1):
    r = colour[0] / 255
    g = colour[1] / 255
    b = colour[2] / 255
    return r, g, b, a


class Config:
    
    def __init__(self, filePath):
        self.file = filePath
        self.parser = 0
        
        if not os.path.isfile(self.file):
            open(self.file, 'a').close()
        
        try:
            self.parser = configparser.RawConfigParser()
        except:
            Log.info("Utils.Config: Failed to initialize ConfigParser.")
            
        self._read()

    def _read(self):
        self.parser.read(self.file)

    def _write(self):
        with open(self.file, "w") as cfgFile:
            self.parser.write(cfgFile)

    def hasSection(self, section = None):
        if section is not None:
            return self.parser.has_section(section)
        else:
            return False

    def hasOption(self, section = None, option = None):
        if self.hasSection(section):
            if option is not None:
                return self.parser.has_option(section, option)
            else:
                return False
        else:
            return False
    
    def addSection(self, section = None):
        if section is not None:
            if not self.hasSection(section):
                self.parser.add_section(section)
                self._write()
                return True
            else:
                Log.info("Utils.Config.addSection -- Section '" + section + "' already exists.")
                return False

    def addOption(self, section = None, option = None, value = None):
        if not self.hasSection(section):
            self.addSection(section)
        
        if (option is not None) and (not self.hasOption(section, option)):
            if (value is not None):
                self.parser.set(section, option, value)
                self._write()
                return True
            else:
                Log.info("Utils.Config.addOption -- Value cannot be null")
                return False
        else:
            Log.info("Utils.Config.addOption -- Option '" + option + "' is blank or already exists in section '" + section + "'.")
            return False

    def updateOption(self, section = None, option = None, value = None, create = False):
        if self.hasOption(section, option):
            if value is not None:
                self.parser.set(section, option, value)
                self._write()
                return True
            else:
                Log.info("Utils.Config.updateOption -- Value cannot be null")
                return False
        else:
            if create:
                return self.addOption(section, option, value)
            else:
                Log.info("Utils.Config.updateOption -- Option '" + option + "' in section '" + section + "' doesn't exist.")
                return False

    def getOption(self, section, option, create = False, default = ""):
        if self.hasOption(section, option):
            return self.parser.get(section, option)
        else:
            if create:
                Log.info("Utils.Config.getOption -- Option '" + option + "' in section '" + section + "' doesn't exist. Creating with default value...")
                self.addOption(section, option, default)
                return default
            else:
                Log.info("Utils.Config.getOption -- Option '" + option + "' in section '" + section + "' doesn't exist.")
                return -1

    def removeSection(self, section):
        if self.hasSection(section):
            self.parser.remove_section(section)
            self._write()
            return True
        else:
            Log.info("Utils.Config.remSection -- section not found.")
            return False

    def removeOption(self, section, option):
        if self.hasOption(section,option):
            self.parser.remove_option(section, option)
            self._write()
            return True
        else:
            Log.info("Utils.Config.Config.remOption -- option not found.")
            return False 