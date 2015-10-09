import os
import shutil
import http.client
import re
import threading
from acSLIApp.logger import Log
import acSLIApp.loader as Config
from acSLIApp.components import Window, Label, Button
import acSLIApp.utils as Utils

instance = 0
progInstance = 0


class Updater:

    isOpen = False
    updaterError = False
    hasUpdated = False
    appWindow = 0
    currVersion = 0
    cV = 0
    remoteVersion = 0
    rV = 0
    reqArduinoUpdate = 0
    changeLog = 0
    statsURL = 0

    lblVersionTxt = 0
    lblLog = 0
    btnYes = 0
    btnNo = 0
    btnIgnore = 0

    def __init__(self, currVersion):
        global instance
        instance = self
        self.currVersion = currVersion
        self.cV = "".join(self.currVersion.split("."))
        if len(self.cV) < 4:
            self.cV = self.cV[0] + self.cV[1] + "0" + self.cV[2]

        try:
            if Config.instance.cfgEnableUpdater == 1:
                conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
                conn.request("GET", "/Turnermator13/ArduinoRacingDash/master/version.txt")
                versionStr = re.findall(r"\'(.+?)\'", str(conn.getresponse().read()))[0].split("|")
                self.remoteVersion = versionStr[0]
                self.reqArduinoUpdate = versionStr[1]
                self.changeLog = versionStr[2]
                self.statsURL = versionStr[3]
                conn.close()
        except Exception as e:
            Log.error("Couldn't get Version Information: %s" % e)
            self.updaterError = True
        logStats("Login")

        self.rV = "".join(self.remoteVersion.split("."))
        if len(self.rV) < 4:
            self.rV = self.rV[0] + self.rV[1] + "0" + self.rV[2]

        if (self.remoteVersion != 0) and (self.remoteVersion != Config.instance.cfgRemoteVersion)\
                and (int(self.rV) > int(self.cV)):
            self.isOpen = True
            if self.reqArduinoUpdate == "1":
                Log.info("New acSLI Version Available: v" + self.remoteVersion + ". Requires Arduino Sketch Update")
            else:
                Log.info("New acSLI Version Available: v" + self.remoteVersion)

            self.appWindow = Window("acSLI Updater", 400, 120).setVisible(1).setPos(760, 350)\
                .setBackgroundTexture("apps/python/acSLI/image/backUpdater.png")
            self.btnYes = Button(self.appWindow.app, bFunc_Yes, 110, 20, 20, 90, "Update")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnUpdater.png")
            self.btnNo = Button(self.appWindow.app, bFunc_No, 110, 20, 145, 90, "Not Now")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnUpdater.png")
            self.btnIgnore = Button(self.appWindow.app, bFunc_Ignore, 110, 20, 270, 90, "Ignore Version")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnUpdater.png")

            self.lblVersionTxt = Label(self.appWindow.app, "New acSLI Version Available: v" + self.remoteVersion, 20, 30)\
                .setSize(360, 10).setAlign("center").setFontSize(20).setColor(Utils.rgb(Utils.colours["red"]))
            self.lblLog = Label(self.appWindow.app, self.changeLog, 20, 60)\
                .setSize(360, 10).setAlign("center").setColor(Utils.rgb(Utils.colours["green"]))
        else:
            if self.updaterError:
                Log.info("Updater Encounter an Error. Version Check Incomplete")
            elif Config.instance.cfgEnableUpdater == 1:
                Log.info("Running Latest Version (v%s)" % self.remoteVersion)


class UpdateFiles(threading.Thread):

    def __init__(self):
        threading.Thread.__init__(self)

    def run(self):
        global instance, progInstance

        try:
            Log.info("Updating Files. Please Wait.")
            UpdateProg()
            logStats("Update")
            Log.info("Creating Backup")
            progInstance.setMsg("Creating Backup")
            shutil.copytree('apps/python/acSLI', 'apps/python/acSLI-BACKUP', ignore=shutil.ignore_patterns("stdlib", "stdlib64", ".idea", "acSLI.txt", "acSLI.ini"))

            conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
            conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/fileList.txt")
            Files = re.findall(r"\'(.+?)\'", str(conn.getresponse().read()))[0].split('\\n')
            Files.append("ArduinoDash.ino")
            lenFiles = len(Files) + 1
            i = 0
            error = "N"

            for filename in Files:
                if filename == "ArduinoDash.ino":
                    conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/ArduinoDash/" + filename)
                else:
                    conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/acSLI/" + filename)
                i += 1
                Log.info("Downloading: " + filename)
                progInstance.setMsg("Downloading[%s/%s][%s]: '%s'" % (str(i), str(lenFiles), str(round((i/lenFiles)*100)) + "%", filename))
                if (filename.split('/')[0] == "stdlib" or filename.split('/')[0] == "stdlib64") and os.path.isfile("apps/python/acSLI/" + filename):
                    Log.info("DLL Exists, Skipping")
                    conn.getresponse().read()
                else:
                    res = conn.getresponse().read()
                    if re.findall(r"\'(.+?)\'", str(res))[0] == "Not Found":
                        error = "Couldn't Find File '%s'?? Please report this to the App Author" % filename
                        Log.info("Couldn't Find File '%s'?? Please report this to the App Author" % filename)
                        break
                    else:
                        try:
                            localfile = open("apps/python/acSLI/" + filename,'wb')
                            localfile.write(res)
                            localfile.close()
                        except FileNotFoundError:
                            os.makedirs(filename.split('/')[0])
                            localfile = open("apps/python/acSLI/" + filename,'wb')
                            localfile.write(res)
                            localfile.close()
                        except Exception as e:
                            Log.error("On Update: %s" % e)

            conn.close()

            if error == "N":
                shutil.rmtree("apps/python/acSLI-BACKUP")
                Log.info("Successfully Updated to " + instance.remoteVersion + " , please restart AC Session")
                progInstance.setMsg("Update Successful. Please Restart Session")
                if instance.reqArduinoUpdate == "1":
                    progInstance.setMsg("Success, Please Update Arduino (latest sketch in apps/python/acsli) and Restart Session")
            else:
                progInstance.setColour(Utils.rgb(Utils.colours["red"]))
                progInstance.setMsg("Error Updating. Restoring from Backup")
                Log.info("Error Updating, Restoring from Backup")

                for obj in os.listdir("apps/python/acSLI/"):
                    if not (str(obj) == "stdlib" or str(obj) == "stdlib64" or str(obj) == ".idea" or str(obj) == "acSLI.txt" or str(obj) == "acSLI.ini"):
                        if not "." in str(obj):
                            shutil.rmtree("apps/python/acSLI/" + obj)
                        else:
                            os.remove("apps/python/acSLI/" + obj)

                for obj in os.listdir("apps/python/acSLI-BACKUP/"):
                    if not "." in str(obj):
                        shutil.copytree("apps/python/acSLI-BACKUP/" + obj, "apps/python/acSLI/" + obj)
                    else:
                        shutil.copy("apps/python/acSLI-BACKUP/" + obj, "apps/python/acSLI/" + obj)

                shutil.rmtree("apps/python/acSLI-BACKUP")
                progInstance.setMsg(error)
            progInstance.dispButton()
        except Exception as e:
                    Log.error("On Update: %s" % e)


class UpdateProg:

    appWindow = 0
    lblMsg = 0
    btnClose = 0

    def __init__(self):
        global instance, progInstance
        progInstance = self

        try:
            self.appWindow = Window("acSLI Update Progress", 800, 100).setVisible(1).setPos(560, 350)\
                    .setBackgroundTexture("apps/python/acSLI/image/backError.png")
            self.lblMsg = Label(self.appWindow.app, "Downloading[0/0][0%]: ", 20, 32)\
                    .setSize(760, 10).setAlign("center").setFontSize(20).setColor(Utils.rgb(Utils.colours["green"]))
        except Exception as e:
                    Log.error("On Update: %s" % e)

    def setMsg(self, msg):
        self.lblMsg.setText(msg)

    def setColour(self, colour):
        self.lblMsg.setColor(colour)

    def dispButton(self):
        global instance
        self.btnClose = Button(self.appWindow.app, bFunc_Close, 110, 20, 345, 70, "Okay")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")


def bFunc_Yes(dummy, variables):
    global instance
    instance.appWindow.setVisible(0)
    UpdateFiles().start()


def bFunc_No(dummy, variables):
    global instance
    instance.appWindow.setVisible(0)
    instance.isOpen = False


def bFunc_Ignore(dummy, variables):
    global instance
    Config.instance.config.updateOption("SETTINGS", "remoteVersion", instance.remoteVersion, True)
    instance.appWindow.setVisible(0)
    instance.isOpen = False


def bFunc_Close(dummy, variables):
    global progInstance
    progInstance.appWindow.setVisible(0)
