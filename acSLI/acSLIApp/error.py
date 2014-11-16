from acSLIApp.logger import Log
from acSLIApp.components import Window, Label, Button

instance = 0

class ErrorBox:

    appWindow = 0
    lblTxt = 0
    btnOkay = 0

    def __init__(self, msg):
        global instance
        instance = self

        self.appWindow = Window("acSLI Error", 800, 100).setVisible(1).setPos(560, 360)\
            .setBackgroundTexture("apps/python/acSLI/image/backError.png")
        self.btnOkay = Button(self.appWindow.app, bFunc_Okay, 240, 20, 280, 70, "Okay").setAlign("center")
        self.lblVersionTxt = Label(self.appWindow.app, msg, 30, 30)\
            .setSize(760, 10).setAlign("center").setFontSize(18)


def bFunc_Okay(dummy, variables):
    global instance
    instance.appWindow.setVisible(0)