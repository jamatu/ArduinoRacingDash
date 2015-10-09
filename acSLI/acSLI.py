import sys
import os
import platform
import shutil
if platform.architecture()[0] == "64bit":
  sysdir='apps/python/acSLI/stdlib64'
else:
  sysdir='apps/python/acSLI/stdlib'
sys.path.insert(0, sysdir)
os.environ['PATH'] = os.environ['PATH'] + ";."

from acSLIApp.logger import Log
import acSLIApp.app as App
import acSLIApp.updater as Updater
import acSLIApp.loader as Loader

acSLI = 0
hasInit = False


def acMain(acVerison):
    global acSLI, hasInit, Log

    try:
        Log.info("Start Loading acSLI v" + App.Version)
        Loader.ConfigLoader()
        Updater.Updater(App.Version)
        cleanInstall()

        if not Updater.instance.isOpen:
            hasInit = True
            acSLI = App.App()
            acSLI.onStart()

        return "acSLI"
        
    except Exception as e:
        Log.error("On Start: %s" % e)
 

def acUpdate(deltaT):  
    global acSLI, hasInit

    try:
        if not Updater.instance.isOpen and not hasInit:
            hasInit = True
            acSLI = App.App()
            acSLI.onStart()

        if hasInit:
            acSLI.onUpdate()

    except Exception as e:
        Log.error("On Update: %s" % e)


def acShutdown(deltaT):  
    global acSLI

    try:
        if not Updater.instance.isOpen:
            acSLI.onClose()

    except Exception as e:
        Log.error("On Close: %s" % e)


def cleanInstall():
    if os.path.exists("apps/python/acSLI/acSLIUpdater.py"):
        os.remove("apps/python/acSLI/acSLIUpdater.py")
    if os.path.exists("apps/python/acSLI/log.txt"):
        os.remove("apps/python/acSLI/log.txt")
    if os.path.exists("apps/python/acSLI/ArduinoDash/"):
        shutil.rmtree("apps/python/acSLI/ArduinoDash")
    if os.path.exists("apps/python/acSLI/acSLIApp/.cache"):
        os.remove("apps/python/acSLI/acSLIApp/.cache")