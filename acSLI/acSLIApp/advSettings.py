from acSLIApp.components import Window

instance = 0


class Selector:

    appWindow = 0
    listPorts = []
    shift = 0
    maxShift = 0

    btnUP = 0
    btnDN = 0
    btnB0 = 0
    btnB1 = 0
    btnB2 = 0
    btnB3 = 0
    btnAUTO = 0
    lblMsg = 0
    lblInst = 0

    def __init__(self):
        global instance
        instance = self

        self.appWindow = Window("acSLI Adv Settings", 250, 260).setPos(835, 300)\
            .setBackgroundTexture("apps/python/acSLI/image/backMain.png")

        self.appWindow.setVisible(0)

    def open(self, msg):
        self.appWindow.setVisible(1)