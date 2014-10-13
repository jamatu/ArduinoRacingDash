import ac
import http.client
import re
from app.logger import Logger
import app.loader as Config
from app.components import Window, Label, Button


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

        if (self.remoteVersion != 0) and (self.remoteVersion != Config.instance.cfgRemoteVersion) and \
                ("".join(self.remoteVersion.split(".")) > "".join(currVersion.split("."))):
            self.isOpen = True
            Log.info("New acSLI Version Avalible: v" + self.remoteVersion)

            self.appWindow = Window("acSLI Updater", 400, 120).setVisible(1).setPos(760, 350)
            self.btnYes = Button(self.appWindow.app, bFunc_Yes, 235, 20, 30, 90, "Okay").setAlign("center")
            #self.btnNo = Button(self.appWindow.app, bFunc_No, 110, 20, 155, 90, "Not Now").setAlign("center")
            self.btnIgnore = Button(self.appWindow.app, bFunc_Ignore, 110, 20, 280, 90, "Ignore Version").setAlign("center")
            self.lblVersionTxt = Label(self.appWindow.app, "New acSLI Version Avalible: v" + self.remoteVersion, 30, 30)\
                .setSize(360, 10).setAlign("center").setFontSize(20)


def bFunc_Yes(dummy, variables):
    global instance
    instance.isOpen = False
    instance.appWindow.setVisible(0)


def bFunc_No(dummy, variables):
    ac.console("press no")


def bFunc_Ignore(dummy, variables):
    global instance
    Config.instance.config.updateOption("SETTINGS", "remoteVersion", instance.remoteVersion, True)
    instance.isOpen = False
    instance.appWindow.setVisible(0)