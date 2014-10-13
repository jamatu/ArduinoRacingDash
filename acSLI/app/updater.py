import ac
import http.client
import re
from app.logger import Logger
import app.loader as Config


Log = Logger()
instance = 0

appWindow = 0
remoteVersion = 0

lblVersionTxt = 0
btnYes = 0
btnNo = 0
btnIgnore = 0


class Updater:

    isOpen = False

    def __init__(self, currVersion):
        global instance
        try:
            conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
            conn.request("GET", "/Turnermator13/ArduinoRacingDash/master/version.txt")
            versionFile = conn.getresponse()
            self.remoteVersion = re.findall(r"\'(.+?)\'", str(versionFile.read()))[0]
        except Exception as e:
            Log.warning("Couldn't get Version Information: %s" % e)
        instance = self

        if (self.remoteVersion != 0) and (self.remoteVersion != Config.instance.cfgRemoteVersion) and ("".join(self.remoteVersion.split(".")) > "".join(currVersion.split("."))):
            self.isOpen = True
            Log.info("New acSLI Version Avalible: v" + self.remoteVersion)
            self.appWindow = ac.newApp("acSLI Updater")

            ac.setSize(self.appWindow, 400, 120)
            ac.drawBorder(self.appWindow, 0)
            ac.setBackgroundOpacity(self.appWindow, 0)
            ac.setVisible(self.appWindow, 1)
            ac.setPosition(self.appWindow, 760, 350)


            self.lblVersionTxt = ac.addLabel(self.appWindow, "New acSLI Version Avalible: v" + self.remoteVersion + ".")
            ac.setPosition(self.lblVersionTxt, 30, 50)
            ac.setSize(self.lblVersionTxt, 360, 10)
            ac.setFontAlignment(self.lblVersionTxt, "center")

            self.btnYes = ac.addButton(self.appWindow, "Okay")
            ac.addOnClickedListener(self.btnYes, bFunc_Yes)
            ac.setPosition(self.btnYes, 30, 90)
            ac.setSize(self.btnYes, 235, 20)
            ac.setFontAlignment(self.btnYes, "center")

            '''self.btnNo = ac.addButton(self.appWindow, "Not Now")
            ac.addOnClickedListener(self.btnNo, bFunc_No)
            ac.setPosition(self.btnNo, 155, 90)
            ac.setSize(self.btnNo, 110, 20)
            ac.setFontAlignment(self.btnNo, "center")'''

            self.btnIgnore = ac.addButton(self.appWindow, "Ignore Version")
            ac.addOnClickedListener(self.btnIgnore, bFunc_Ignore)
            ac.setPosition(self.btnIgnore, 280, 90)
            ac.setSize(self.btnIgnore, 110, 20)
            ac.setFontAlignment(self.btnIgnore, "center")


def bFunc_Yes(dummy, variables):
    global instance, appWindow
    instance.isOpen = False
    ac.setVisible(instance.appWindow, 0)


def bFunc_No(dummy, variables):
    ac.console("press no")


def bFunc_Ignore(dummy, variables):
    global instance, appWindow
    Config.instance.config.updateOption("SETTINGS", "remoteVersion", instance.remoteVersion, True)
    instance.isOpen = False
    ac.setVisible(instance.appWindow, 0)