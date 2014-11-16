from acSLIApp.logger import Log
from acSLIApp.components import Window, Label, Button
import serial.tools.list_ports
import acSLIApp.loader as Config
import acSLIApp.connection as Connection
import acSLIApp.utils as Utils

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

        self.appWindow = Window("acSLI Com Selector", 650, 360).setPos(635, 240)\
            .setBackgroundTexture("apps/python/acSLI/image/backSelector.png")

        self.btnUP = Button(self.appWindow.app, bFunc_UP, 80, 40, 285, 90, "").setAlign("center").hasCustomBackground()
        self.btnDN = Button(self.appWindow.app, bFunc_DN, 80, 40, 285, 310, "").setAlign("center").hasCustomBackground()

        self.lblMsg = Label(self.appWindow.app, "", 30, 47).setSize(590, 10).setAlign("center").setFontSize(18)\
            .setColor(Utils.rgb(Utils.colours["red"]))
        self.btnAUTO = Button(self.appWindow.app, bFunc_AUTO, 160, 20, 440, 98, "Enable AUTO Mode").setAlign("center")\
            .hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")

        self.btnB0 = Button(self.appWindow.app, bFunc_B0, 600, 20, 25, 150, "").setAlign("center")\
            .hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backList.png")
        self.btnB1 = Button(self.appWindow.app, bFunc_B1, 600, 20, 25, 190, "").setAlign("center")\
            .hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backList.png")
        self.btnB2 = Button(self.appWindow.app, bFunc_B2, 600, 20, 25, 230, "").setAlign("center")\
            .hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backList.png")
        self.btnB3 = Button(self.appWindow.app, bFunc_B3, 600, 20, 25, 270, "").setAlign("center")\
            .hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backList.png")

        self.appWindow.setVisible(0)

    def open(self, msg):

        self.listPorts = []
        for sPort, desc, hwid in sorted(serial.tools.list_ports.comports()):
            self.listPorts.append([sPort, desc, hwid])

        if len(self.listPorts) < 4:
            for x in range(len(self.listPorts), 4):
                self.listPorts.append(["-", "--", "--"])

        self.maxShift = len(self.listPorts) - 4
        self.lblMsg.setText(msg + ". Select a Port from the List:")
        self.scrollLogic()
        self.appWindow.setVisible(1)

    def scrollLogic(self):

        if self.shift == 0:
            self.btnUP = self.btnUP.setBackgroundTexture("apps/python/acSLI/image/btnUP1.png")
        else:
            self.btnUP = self.btnUP.setBackgroundTexture("apps/python/acSLI/image/btnUP0.png")

        if self.shift == self.maxShift:
            self.btnDN = self.btnDN.setBackgroundTexture("apps/python/acSLI/image/btnDN1.png")
        else:
            self.btnDN = self.btnDN.setBackgroundTexture("apps/python/acSLI/image/btnDN0.png")

        self.btnB0 = self.btnB0.setText("%s [%s]" % (self.listPorts[(0 + self.shift)][1], self.listPorts[(0 + self.shift)][2]))
        self.btnB1 = self.btnB1.setText("%s [%s]" % (self.listPorts[(1 + self.shift)][1], self.listPorts[(1 + self.shift)][2]))
        self.btnB2 = self.btnB2.setText("%s [%s]" % (self.listPorts[(2 + self.shift)][1], self.listPorts[(2 + self.shift)][2]))
        self.btnB3 = self.btnB3.setText("%s [%s]" % (self.listPorts[(3 + self.shift)][1], self.listPorts[(3 + self.shift)][2]))


def bFunc_UP(dummy, variables):
    global instance
    if instance.shift > 0:
        instance.shift -= 1
        instance.scrollLogic()


def bFunc_DN(dummy, variables):
    global instance
    if instance.shift < instance.maxShift:
        instance.shift += 1
        instance.scrollLogic()


def bFunc_AUTO(dummy, variables):
    global instance

    instance.appWindow.setVisible(0)
    Config.instance.cfgPort = "AUTO"
    Config.instance.rewriteConfig()
    Connection.findConnection().start()


def bFunc_B0(dummy, variables):
    global instance

    pos = 0 + instance.shift
    instance.appWindow.setVisible(0)
    Config.instance.cfgPort = instance.listPorts[pos][0]
    Config.instance.rewriteConfig()
    Connection.findConnection().start()


def bFunc_B1(dummy, variables):
    global instance

    pos = 1 + instance.shift
    instance.appWindow.setVisible(0)
    Config.instance.cfgPort = instance.listPorts[pos][0]
    Config.instance.rewriteConfig()
    Connection.findConnection().start()


def bFunc_B2(dummy, variables):
    global instance

    pos = 2 + instance.shift
    instance.appWindow.setVisible(0)
    Config.instance.cfgPort = instance.listPorts[pos][0]
    Config.instance.rewriteConfig()
    Connection.findConnection().start()


def bFunc_B3(dummy, variables):
    global instance

    pos = 3 + instance.shift
    instance.appWindow.setVisible(0)
    Config.instance.cfgPort = instance.listPorts[pos][0]
    Config.instance.rewriteConfig()
    Connection.findConnection().start()