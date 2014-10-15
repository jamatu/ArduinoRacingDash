import os
import http.client
import re
from app.logger import Logger
import app.loader as Config
from app.components import Window, Label, Button


Log = Logger()
instance = 0


class Updater:

    isOpen = False
    appWindow = 0
    remoteVersion = 0

    lblVersionTxt = 0
    btnYes = 0
    btnNo = 0
    btnIgnore = 0

    def __init__(self, currVersion):
        global instance
        instance = self

        try:
            conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
            conn.request("GET", "/Turnermator13/ArduinoRacingDash/master/version.txt")
            versionFile = conn.getresponse()
            self.remoteVersion = re.findall(r"\'(.+?)\'", str(versionFile.read()))[0]
            conn.close()
        except Exception as e:
            Log.warning("Couldn't get Version Information: %s" % e)

        if (self.remoteVersion != 0) and (self.remoteVersion != Config.instance.cfgRemoteVersion) and \
                ("".join(self.remoteVersion.split(".")) > "".join(currVersion.split("."))):
            self.isOpen = True
            Log.info("New acSLI Version Available: v" + self.remoteVersion)

            self.appWindow = Window("acSLI Updater", 400, 120).setVisible(1).setPos(760, 350)\
                .setBackgroundTexture("apps/python/acSLI/image/backUpdater.png")
            self.btnYes = Button(self.appWindow.app, bFunc_Yes, 110, 20, 20, 90, "Okay")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")
            self.btnNo = Button(self.appWindow.app, bFunc_No, 110, 20, 145, 90, "Not Now")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")
            self.btnIgnore = Button(self.appWindow.app, bFunc_Ignore, 110, 20, 270, 90, "Ignore Version")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")
            self.lblVersionTxt = Label(self.appWindow.app, "New acSLI Version Available: v" + self.remoteVersion, 30, 30)\
                .setSize(360, 10).setAlign("center").setFontSize(20)

    def updateFiles(self):
        Log.info("Updating Files. Please Wait.")

        conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
        conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + self.remoteVersion + "/fileList.txt")
        Files = re.findall(r"\'(.+?)\'", str(conn.getresponse().read()))[0].split('\\n')

        for filename in Files:
            conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + self.remoteVersion + "/acSLI/" + filename)

            if filename.split('/')[0] == "dll" and os.path.isfile("apps/python/acSLI/" + filename):
                Log.info("DLL Exists, Skipping")
            else:
                try:
                    localfile = open("apps/python/acSLI/" + filename,'wb')
                    localfile.write(conn.getresponse().read())
                    localfile.close()
                except FileNotFoundError:
                    os.makedirs(filename.split('/')[0])
                    localfile = open("apps/python/acSLI/" + filename,'wb')
                    localfile.write(conn.getresponse().read())
                    localfile.close()
                except Exception as e:
                    Log.error("On Start: %s" % e)

        conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + self.remoteVersion + "/ArduinoDash/ArduinoDash.ino")
        arduinoSketch = open("apps/python/acSLI/ArduinoDash.ino",'wb')
        arduinoSketch.write(conn.getresponse().read())
        arduinoSketch.close()

        conn.close()



def bFunc_Yes(dummy, variables):
    global instance
    instance.updateFiles()
    instance.appWindow.setVisible(0)
    instance.isOpen = False


def bFunc_No(dummy, variables):
    global instance
    instance.appWindow.setVisible(0)
    instance.isOpen = False


def bFunc_Ignore(dummy, variables):
    global instance
    Config.instance.config.updateOption("SETTINGS", "remoteVersion", instance.remoteVersion, True)
    instance.appWindow.setVisible(0)
    instance.isOpen = False