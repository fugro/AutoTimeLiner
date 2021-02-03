class Positions:
    # component: dogleg/placard
    # area: top/bottom
    # index: 0 = Closest to Timeline, 1 = Center, 2 = Farthest from Timeline,

    TOP_PLACARD_PIXEL     = 384
    BOTTOM_PLACARD_PIXEL  = 522
    PLACARD_HEIGHT        = 75
    DOGLEG_WIDTH          = 30
    PLACARD_Y_SPACING     = 77
    DOGLEG_Y_OFFSET       = 12

    def __init__(self, component, area, index):
        self.component = component
        self.area = area
        self.index = index

    def GetY(self):
        if (self.component.lower() == "dogleg"):
            if (self.area.lower() == "top"):
                return self.TOP_PLACARD_PIXEL - (self.PLACARD_Y_SPACING * self.index) + self.DOGLEG_Y_OFFSET
            elif (self.area.lower() == "bottom"):
                return self.BOTTOM_PLACARD_PIXEL + (self.PLACARD_Y_SPACING * self.index) + self.DOGLEG_Y_OFFSET
            else:
                return -1
        elif (self.component.lower() == "placard"):
            if (self.area.lower() == "top"):
                return self.TOP_PLACARD_PIXEL - (self.PLACARD_Y_SPACING * self.index)
            elif (self.area.lower() == "bottom"):
                return self.BOTTOM_PLACARD_PIXEL + (self.PLACARD_Y_SPACING * self.index)
            else:
                return -1
        print("Error, Incompatible component!")
        return -1