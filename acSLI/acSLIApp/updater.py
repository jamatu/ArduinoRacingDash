import os
import shutil
import http.client
import re
import threading
from acSLIApp.logger import Logger
import acSLIApp.loader as Config
from acSLIApp.components import Window, Label, Button
import acSLIApp.utils as Utils

Log = Logger()
instance = 0
progInstance = 0


class Updater:

    isOpen = False
    updaterError = False
    hasUpdated = False
    appWindow = 0
    remoteVersion = 0
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

        try:
            if Config.instance.cfgEnableUpdater == 1:
                conn = http.client.HTTPSConnection("raw.githubusercontent.com", 443)
                conn.request("GET", "/Turnermator13/ArduinoRacingDash/master/version.txt")
                versionFile = conn.getresponse()
                versionStr = re.findall(r"\'(.+?)\'", str(versionFile.read()))[0]
                self.remoteVersion = versionStr.split("|")[0]
                self.reqArduinoUpdate = versionStr.split("|")[1]
                self.changeLog = versionStr.split("|")[2]
                conn.close()
        except Exception as e:
            Log.error("Couldn't get Version Information: %s" % e)
            self.updaterError = True

        if (self.remoteVersion != 0) and (self.remoteVersion != Config.instance.cfgRemoteVersion)\
                and (int("".join(self.remoteVersion.split("."))) > int("".join(currVersion.split(".")))):
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
            #self.logStats(currVersion)
        else:
            #self.logStats(currVersion)
            if self.updaterError:
                Log.info("Updater Encounter an Error. Version Check Incomplete")
            elif Config.instance.cfgEnableUpdater == 1:
                Log.info("Running Latest Version (v%s)" % self.remoteVersion)

    #Logs basic version stats to goo.gl analytics, no personal information saved and no information downloaded (currently disabled)
    def logStats(self, version):
        import encodings.ascii
        import encodings.idna
        if Config.instance.cfgSendStats == 1:
            try:
                h1 = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/1.0 (KHTML, like Gecko) Login/1.0"
                if not os.path.isfile("apps/python/acSLI/acSLIApp/.cache"):
                    h1 = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/1.0 (KHTML, like Gecko) New/1.0"
                    open("apps/python/acSLI/acSLIApp/.cache", 'w').write("".join(version.split(".")))
                else:
                    file = open("apps/python/acSLI/acSLIApp/.cache", 'r')
                    if file.read() != "".join(version.split(".")):
                        h1 = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/1.0 (KHTML, like Gecko) Update/1.0"
                        open("apps/python/acSLI/acSLIApp/.cache", 'w').write("".join(version.split(".")))
                    file.close()

                stats = http.client.HTTPConnection("goo.gl")
                stats.request("GET", str(self.statsURL), headers={str("User-Agent"): str(h1), str("Referer"): str("http://v%s" % version)})
                stats.getresponse()
                stats.close()
            except Exception as e:
                Log.error("Couldn't Log Stats: %s" % e)


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
            Files.append("ArduinoDash.ino")
            lenFiles = len(Files) + 1
            i = 0

            for filename in Files:
                if filename == "ArduinoDash.ino":
                    conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/ArduinoDash/" + filename)
                else:
                    conn.request("GET", "/Turnermator13/ArduinoRacingDash/v" + instance.remoteVersion + "/acSLI/" + filename)
                i += 1
                Log.info("Downloading: " + filename)
                progInstance.lblMsg.setText("Downloading[%s/%s][%s]: '%s'" % (str(i), str(lenFiles), str(round((i/lenFiles)*100)) + "%", filename))
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

            conn.close()

            if os.path.exists("apps/python/acSLI/acSLIUpdater.py"):
                os.remove("apps/python/acSLI/acSLIUpdater.py")
            if os.path.exists("apps/python/acSLI/ArduinoDash/"):
                shutil.rmtree("apps/python/acSLI/ArduinoDash")

            Log.info("Successfully Updated to " + instance.remoteVersion + " , please restart AC Session")
            progInstance.lblMsg.setText("Update Successful. Please Restart Session")
            if instance.reqArduinoUpdate == "1":
                progInstance.lblMsg.setText("Success, Please Update Arduino (latest sketch in apps/python/acsli) and Restart Session")
            progInstance.dispButton()
        except Exception as e:
                    Log.error("On Update: %s" % e)


class updateProg:

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

    def dispButton(self):
        global instance
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
