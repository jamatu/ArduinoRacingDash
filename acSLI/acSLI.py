import sys
sys.path.insert(0, 'apps/python/acSLI/dll')

from app.logger import Logger
import app.app as App
import app.updater as Updater
import app.loader as Loader
import app.selector as Selector


Log = Logger()
acSLI = 0
hasInit = False


def acMain(acVerison):
    global acSLI, hasInit

    try:
        Log.info("Start Loading acSLI v" + App.Version)
        Loader.ConfigLoader()
        Updater.Updater(App.Version)
        Selector.Selector()

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