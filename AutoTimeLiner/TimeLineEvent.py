import datetime 

class TimeLineEvent:

    def __init__(self, name, label, date):
        self.name = name        
        self.label = label
        self.date = date

    def __str__(self):

        return f"""{self.name}
{self.label}
{self.date}
        """

    @classmethod
    def get_field_names(cls):
        return ["date"]

    def get_julian_date(self):
        return self.date.timetuple().tm_yday

    def get_MDY_date(self):
        return self.date.strftime("%m/%d/%Y")
    
    def get_label(self):
        return self.label