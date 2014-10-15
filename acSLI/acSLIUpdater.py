import os
import http.client
import re
import threading
from app.logger import Logger
import app.loader as Config
from app.components import Window, Label, Button
import app.utils as Utils


Log = Logger()
instance = 0
progInstance = 0


class Updater:

    isOpen = False
    hasUpdated = False
    appWindow = 0
    remoteVersion = 0
    reqArduinoUpdate = 0
    changeLog = 0

    lblVersionTxt = 0
    lblLog = 0
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
            versionStr = re.findall(r"\'(.+?)\'", str(versionFile.read()))[0]
            self.remoteVersion = versionStr.split("|")[0]
            self.reqArduinoUpdate = bool(versionStr.split("|")[1])
            self.changeLog = versionStr.split("|")[2]
            conn.close()
        except Exception as e:
            Log.warning("Couldn't get Version Information: %s" % e)

        if (self.remoteVersion != 0) and (Config.instance.cfgEnableUpdater == 1) and (self.remoteVersion != Config.instance.cfgRemoteVersion)\
                and ("".join(self.remoteVersion.split(".")) > "".join(currVersion.split("."))):
            self.isOpen = True
            if self.reqArduinoUpdate:
                Log.info("New acSLI Version Available: v" + self.remoteVersion + ". Requires Arduino Sketch Update")
            else:
                Log.info("New acSLI Version Available: v" + self.remoteVersion)

            self.appWindow = Window("acSLI Updater", 400, 120).setVisible(1).setPos(760, 350)\
                .setBackgroundTexture("apps/python/acSLI/image/backUpdater.png")
            self.btnYes = Button(self.appWindow.app, bFunc_Yes, 110, 20, 20, 90, "Okay")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")
            self.btnNo = Button(self.appWindow.app, bFunc_No, 110, 20, 145, 90, "Not Now")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")
            self.btnIgnore = Button(self.appWindow.app, bFunc_Ignore, 110, 20, 270, 90, "Ignore Version")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")

            self.lblVersionTxt = Label(self.appWindow.app, "New acSLI Version Available: v" + self.remoteVersion, 20, 30)\
                .setSize(360, 10).setAlign("center").setFontSize(20).setColor(Utils.rgb(Utils.colours["red"]))
            self.lblLog = Label(self.appWindow.app, self.changeLog, 20, 60)\
                .setSize(360, 10).setAlign("center").setColor(Utils.rgb(Utils.colours["green"]))
        else:
            Log.info("Running Latest Version")



class updateFiles(threading.Thread):

    def __init__(self):
        threading.Thread.__init__(self)

    def run(self):
        global instance, progInstance

        try:
            Log.info("Updating Files. Please Wait.")

            updateProg()

            conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
            conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/fileList.txt")
            Files = re.findall(r"\'(.+?)\'", str(conn.getresponse().read()))[0].split('\\n')
            lenFiles = len(Files) + 1
            i = 0

            for filename in Files:
                conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/acSLI/" + filename)
                i += 1
                Log.info("Downloading: " + filename)
                progInstance.lblMsg.setText("Downloading[%s/%s][%s]: '%s'" % (str(i), str(lenFiles), str(round((i/lenFiles)*100, 0)) + "%", filename))
                if filename.split('/')[0] == "dll" and os.path.isfile("apps/python/acSLI/" + filename):
                    Log.info("DLL Exists, Skipping")
                    conn.getresponse().read()
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
                        Log.error("On Update: %s" % e)

            conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/ArduinoDash/ArduinoDash.ino")
            Log.info("Downloading: ArduinoDash.ino")
            progInstance.lblMsg.setText("Downloading[%s/%s][%s]: 'ArduinoDash.ino'" % (str(lenFiles), str(lenFiles), "100%"))
            arduinoSketch = open("apps/python/acSLI/ArduinoDash.ino",'wb')
            arduinoSketch.write(conn.getresponse().read())
            arduinoSketch.close()

            conn.close()
            Log.info("Successfully Updated to " + instance.remoteVersion + " , please restart AC Session")
            progInstance.lblMsg.setText("Update Successful. Please Restart Session")
            if instance.reqArduinoUpdate:
               progInstance.lblMsg.setText("Success! Please Update Arduino (latest sketch in apps/python/acsli) and Restart Session")
            progInstance.dispButton()
        except Exception as e:
                    Log.error("On Update: %s" % e)



class updateProg:

    appWindow = 0
    lblMsg = 0
    btnClose = 0

    def __init__(self):
        global progInstance
        progInstance = self

        self.appWindow = Window("acSLI Update Progress", 800, 100).setVisible(1).setPos(560, 350)\
                .setBackgroundTexture("apps/python/acSLI/image/backError.png")
        self.lblMsg = Label(self.appWindow.app, "Downloading[0/0][0%]: ", 20, 32)\
                .setSize(760, 10).setAlign("center").setFontSize(20).setColor(Utils.rgb(Utils.colours["green"]))

    def setMsg(self, msg):
        self.lblMsg.setText(msg)

    def dispButton(self):
        self.btnClose = Button(self.appWindow.app, bFunc_Close, 110, 20, 345, 70, "Okay")\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")


def bFunc_Yes(dummy, variables):
    global instance
    instance.appWindow.setVisible(0)
    updateFiles().start()


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
