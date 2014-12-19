from acSLIApp.components import Window

instance = 0


class AdvSet:

    appWindow = 0

    def __init__(self):
        global instance
        instance = self

        self.appWindow = Window("acSLI Adv Settings", 250, 260).setPos(835, 450)\
            .setBackgroundTexture("apps/python/acSLI/image/backMain.png")

        self.appWindow.setVisible(0)

    def open(self, msg):
        self.appWindow.setVisible(1)