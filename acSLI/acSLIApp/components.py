import ac
from acSLIApp.logger import Log


class Window:

    def __init__(self, name="defaultAppWindow", width=100, height=100):
        self.app = ac.newApp(name)
        ac.drawBorder(self.app, 0)
        ac.setBackgroundOpacity(self.app, 0)
        self.setSize(width, height)

    def setPos(self, x, y):
        ac.setPosition(self.app, x, y)
        return self

    def setSize(self, w, h):
        ac.setSize(self.app, w, h)
        return self

    def setVisible(self, visible):
        ac.setVisible(self.app, visible)
        return self

    def setBackgroundTexture(self, texture):
        ac.setBackgroundTexture(self.app, texture)
        return self


class Label:

    def __init__(self, window, text, xPos, yPos):
        self.label = ac.addLabel(window, text)
        ac.setPosition(self.label, xPos, yPos)

    def setText(self, text):
        ac.setText(self.label, text)
        return self

    def setSize(self, w, h):
        ac.setSize(self.label, w, h)
        return self

    def setColor(self, colour):
        ac.setFontColor(self.label, *colour)
        return self

    def setFontSize(self, fontSize):
        ac.setFontSize(self.label, fontSize)
        return self

    def setAlign(self, align):
        ac.setFontAlignment(self.label, align)
        return self

    def setVisible(self, value):
        ac.setVisible(self.label, value)
        return self


class Button:

    def __init__(self, window, clickFunc, width=60, height=20, x=0, y=0, text=""):
        self.button = ac.addButton(window, text)
        self.setSize(width, height)
        self.setPos(x, y)
        ac.addOnClickedListener(self.button, clickFunc)

    def setText(self, text):
        ac.setText(self.button, text)
        return self

    def setSize(self, width, height):
        ac.setSize(self.button, width, height)
        return self

    def setPos(self, x, y):
        ac.setPosition(self.button, x, y)
        return self

    def setAlign(self, align):
        ac.setFontAlignment(self.button, align)
        return self

    def hasCustomBackground(self):
        ac.drawBorder(self.button, 0)
        ac.setBackgroundOpacity(self.button, 0)
        return self

    def setBackgroundTexture(self, texture):
        ac.setBackgroundTexture(self.button, texture)
        return self

    def setFontSize(self, fontSize):
        ac.setFontSize(self.button, fontSize)
        return self


class Spinner:

    def __init__(self, window, changeFunc, width=120, height=20, x=0, y=0, title="spn", minVal=0, maxVal=1, startVal=0):
        self.spinner = ac.addSpinner(window, title)
        self.setSize(width, height)
        self.setPos(x, y)
        self.setRange(minVal, maxVal)
        ac.addOnValueChangeListener(self.spinner, changeFunc)
        Log.blank(ac.setValue(self.spinner, startVal))   # logging it fixes really weird bug

    def setSize(self, width, height):
        ac.setSize(self.spinner, width, height)
        return self

    def setPos(self, x, y):
        ac.setPosition(self.spinner, x, y)
        return self

    def setRange(self, minVal, maxVal):
        ac.setRange(self.spinner, minVal, maxVal)
        return self

    def getValue(self):
        return ac.getValue(self.spinner)