import sys
sys.path.insert(0, 'apps/python/acSLI/dll')

from app.app import App
from app.logger import Logger
import app.updater as Updater
import app.loader as Config

#################
Version = "1.8.3"
#################

Log = Logger()
acSLI = 0
hasInit = False


def acMain(acVerison):
    global acSLI, Version, hasInit

    try:
        Config.ConfigLoader()
        Updater.Updater(Version)

        if not Updater.instance.isOpen:
            acSLI = App(Version)
            acSLI.onStart()
            hasInit = True

        return "acSLI"
        
    except Exception as e:
        Log.error("On Start: %s" % e)
 

def acUpdate(deltaT):  
    global acSLI, Version, hasInit

    try:
        if not Updater.instance.isOpen and not hasInit:
            acSLI = App(Version)
            acSLI.onStart()
            hasInit = True

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