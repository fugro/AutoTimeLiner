import copy
import json
import datetime as dt
import os
from TimeLineEvent import TimeLineEvent
from Positions import Positions
from PIL import Image, ImageDraw, ImageFont
import sys

Q1_COLOR = (71, 156, 170)
Q2_COLOR = (140,182,128)
Q3_COLOR = (217,190, 137)
Q4_COLOR = (47, 68, 93)

QWORDS_ZZ = [(209, 472), (523, 472), (835, 472), (1147, 472)]

DATA_FILE_PATH = "Data/input_data.json"
TEMPLATE_PATH = "Template/blank.png"
LOGO_PATH = "Template/Logo.png"
OUTPUT_PATH = "Output/"
FONT_PATH = "Font/segoeui.ttf"
FONT_PATH_BOLD = "Font/segoeuib.ttf"

TOP_TOP_PIXEL = 230
TOP_BOTTOM_PIXEL = 459
BOTTOM_TOP_PIXEL = 512
BOTTOM_BOTTOM_PIXEL = 800

HORZ_START_PIXEL = 102
HORZ_STOP_PIXEL = 1358

LINE_WIDTH = 3
DOGLEG_WIDTH = 30

def is_valid():
    if (len(sys.argv) == 1 and not os.path.exists(DATA_FILE_PATH)):
        print ("No json file path provided, and " + DATA_FILE_PATH +" missing")
        return False
    
    if (len(sys.argv) > 2):
        print ("Invalid number of arguments provided! See Usage information!")
        return False
    elif (len(sys.argv) == 2 and not os.path.exists(sys.argv[1])):
        print ("Provided data file " + sys.argv[1] + "doesn't exit!")
        return False

    if (not os.path.exists(OUTPUT_PATH)):
        print("Output directory doesn't exist - Creating directory.")
        os.mkdir(OUTPUT_PATH)
    
    if (not os.path.exists(TEMPLATE_PATH)):
        print(TEMPLATE_PATH + " missing!")
        return False
    
    if (not os.path.exists(LOGO_PATH)):
        print (LOGO_PATH + " missing!")
        return False

    if (not os.path.exists(FONT_PATH)):
        print(FONT_PATH + " missing!")
        return False

    if (not os.path.exists(FONT_PATH_BOLD)):
        print(FONT_PATH_BOLD + " missing!")
        return False
    
    return True

def get_julian_day(year, month, day):
    return dt.date(year, month, day).timetuple().tm_yday

def get_start_day(year, quarter):
    if (quarter == 1):
        return 1
    elif (quarter == 2):
        return get_julian_day(year, 4, 1)
    elif (quarter == 3):
        return get_julian_day(year, 7, 1)
    elif (quarter == 4):
        return get_julian_day(year, 10, 1)

def create_quarters(start_date):
    date = dt.datetime.strptime(start_date, "%m/%d/%Y")
    year = date.year
    quarter = ((date.month - 1) // 3) + 1
    start_day = 1
    quarters = ["", "", "", ""]
    
    for i in range(4):
        start_day = get_start_day(year, quarter)
        quarters[i] = (year, quarter, start_day)
        if (quarter == 4):
            year += 1
            quarter = 1
        else:
            quarter += 1
    return quarters

def get_image_data(path):

    im = Image.open(path)

    return im

def write_image(im, ts_output):
    if (ts_output == True):
        format = "%Y-%m-%d_%H-%M-%S"
        filename = dt.datetime.now().strftime(format) + ".png"
        im.save(f"{OUTPUT_PATH}{filename}")
    else:
        im.save(f"{OUTPUT_PATH}latest.png")

def bake_in_dogleg(template, x, y, area):
    draw = ImageDraw.Draw(template)
    line = (x, y, x + DOGLEG_WIDTH, y)
    draw.line(line, fill=(0,0,0), width=3)
    if (area == "top"):
        line = (x, y, x, TOP_BOTTOM_PIXEL)
    else:
        line = (x, y, x, BOTTOM_TOP_PIXEL)
    draw.line(line, fill=(0,0,0), width=3)

    return template

def make_placard(time_line_event, placard_type):

    len_of_name = len(time_line_event.name)
    len_of_label = len(time_line_event.label)
    if (len_of_label > len_of_name):
        width_of_words = 10 * (len_of_label)
    else:
        width_of_words = 10 * (len_of_name)
    if (width_of_words < 90):
        width_of_words = 90
    height_of_words = 60

    text_to_render = ""
    
    date_string = f"{time_line_event.get_MDY_date()}"
    date_width_of_words = (len(date_string))

    if (date_width_of_words > width_of_words):
        width_of_words = date_width_of_words

    if (time_line_event.name.strip() != ""):
        text_to_render = f"{time_line_event.name}\n"
    if (time_line_event.label.strip() != ""):
        text_to_render = f"{text_to_render}{time_line_event.label}\n"
    text_to_render = f"{text_to_render}{date_string}"

    img = Image.new("RGBA", (width_of_words, height_of_words),
                    color=(255, 255, 255))
    draw = ImageDraw.Draw(img)

    fnt = ImageFont.truetype(FONT_PATH, 15)
    draw.text((10, 0), text_to_render, fill=(0, 0, 0), font=fnt)

    return img

def make_quarters():

    text_to_render = "Header"
    len_of_name = len(text_to_render)
    width_of_words = 6 * (len_of_name + 20)
    height_of_words = 15 * 4

    img = Image.new("RGBA", (width_of_words, height_of_words),
                    color=(255, 255, 255))
    draw = ImageDraw.Draw(img)

    fnt = ImageFont.truetype(FONT_PATH, 15)
    draw.text((10, 10), text_to_render, fill=(0, 0, 0), font=fnt)
    return img

def open_projects_data():

    if (len(sys.argv) > 1):
        with open(sys.argv[1], 'r') as read_json:
            input_data = json.load(read_json)
    else:
        with open(DATA_FILE_PATH, 'r') as read_json:
            input_data = json.load(read_json)

    return input_data

def convert_text_dates_to_datetimes(data_dict):

    formatted_projects = []
    for project in data_dict['projects']:
        formatted_projects.append(dict(name=project['name'],
                                       label=project['label'],
                                       date=dt.datetime.strptime(
                                           project['date'], "%m/%d/%Y")))

    data_dict['projects'] = formatted_projects
    return data_dict

def bake_placard_into_template(x, img, placard, y):
    img.paste(placard, (x, y))
    return img

def calc_julian_day(year, quarter):
    start = dt.date(year, (((quarter - 1) * 3) + 1), 1).timetuple().tm_yday - 1
    if (quarter == 1):
        end = get_julian_day(year, 3, 31)
    elif (quarter == 2):
        end = get_julian_day(year, 6, 30)
    elif (quarter == 3):
        end = get_julian_day(year, 9, 30)
    else:
        end = get_julian_day(year, 12, 31)
    return (start, end)

def get_julian_list(HORZ_START_PIXEL, HORZ_STOP_PIXEL,JDAY):
    COUNT = JDAY[1] - JDAY[0] + 1
    horizontal_pixels = range(HORZ_START_PIXEL, HORZ_STOP_PIXEL)
    julian_day = [h % COUNT for h in horizontal_pixels]
    julian_day.sort()

    julian_day_to_h_pixel_map = {}

    for h_pixel, j_day in zip(range(HORZ_START_PIXEL, HORZ_STOP_PIXEL+1), julian_day):
        julian_day_to_h_pixel_map[j_day+JDAY[0]] = h_pixel

    return julian_day_to_h_pixel_map

def auto_time_liner():

    data = open_projects_data()

    team = data['team']
    start_date = data['start_date']

    # output file will contain timestamp prefix if ts_output exists in json file.
    if ('ts_output' in data.keys()):
        ts_output = data['ts_output']
    else:
        # default to false
        ts_output = False
    
    data = convert_text_dates_to_datetimes(data)
    
    zero_zero_placard_pixels = {}
    julian_day_to_h_pixel_map = {}
    pixels = (102, 413, 724, 1033, 1357)
    quarters = create_quarters(start_date)
    jday = [(0,0), (0,0), (0,0), (0,0)]
    for i in range(4):
        jday[i] = calc_julian_day(quarters[i][0], quarters[i][1])
    
    for i in range(1, 5):
        julian_day_to_h_pixel_map.update(get_julian_list(pixels[i-1], pixels[i], jday[i-1]))

    #makes everything timeevents

    time_line_events = []
    
    for project in data["projects"]:

        time_line_events.append(TimeLineEvent(project['name'],
                                              project['label'],
                                              project['date']))

    # build placards
    placards = []
    max_placard_height = -99

    # sort placards
    order = {}
    index = 0
    for event in time_line_events:
        order[index] = event.get_julian_date()
        index += 1
    
    order = sorted(order.items(), key = lambda x:x[1])

    ordered = {}
    inx = 0
    #Get items from start quarter
    for i in range(len(order)):
         if (order[i][1] >= quarters[0][2]):
             ordered[inx] = (order[i][0], order[i][1])
             inx += 1
    for i in range(len(order)):
         if (order[i][1] < quarters[0][2]):
             ordered[inx] = (order[i][0], order[i][1])
             inx += 1

    #sort by order
    for i in range(len(order)):
        for placard_name in time_line_events[ordered[i][0]].get_field_names():
            placard_inx = len(placards)
            zero_zero_placard_pixels[placard_inx] = {"x": julian_day_to_h_pixel_map[time_line_events[ordered[i][0]].get_julian_date()], "y": -1}
            img = make_placard(time_line_events[ordered[i][0]], placard_name)
            if (img.size[1] > max_placard_height):
                max_placard_height = img.size[1]

            placards.append(img)

    # puts stuff on the image
    template = get_image_data(TEMPLATE_PATH)
    
    area = "top"
    index = 2
    previous_dogleg_position = 0
    last_placard = False
    for i in range(0, len(order)):
        placard = placards[i]
        x = zero_zero_placard_pixels[i]['x']
        if (x + DOGLEG_WIDTH + placard.width > HORZ_STOP_PIXEL):
            # try to center on Line
            placard_x = x - int(placard.width / 2)
        else:
            placard_x = x

        if (placard_x <= previous_dogleg_position):
            last_placard = True
            index = 2
            if (area == "bottom"):
                area = "top"
            else:
                area = "bottom"

        if (index == -1 and last_placard == False):
            if (index == -1):
                index = 2
            if (area == "bottom"):
                area = "top"
            else:
                area = "bottom"

        if (area == "top"): # (i <= 2):
            area = "top"
            template = bake_in_dogleg(template, x, Positions("dogleg", area, index).GetY(), area)
            template = bake_placard_into_template(placard_x + DOGLEG_WIDTH, template, placard, Positions("placard", area, index).GetY())
            index -= 1
        else:
            area = "bottom"
            template = bake_in_dogleg(template, x, Positions("dogleg", area, index).GetY(), area)
            template = bake_placard_into_template(placard_x + DOGLEG_WIDTH, template, placard, Positions("placard", area, index).GetY())
            index -= 1
        previous_dogleg_position = x
        if (last_placard):
            break

    # Add Team to Image._
    draw = ImageDraw.Draw(template)
    fnt = ImageFont.truetype(FONT_PATH, 31)
    draw.text((100, 178),team,(40,40,40),font=fnt)

    # Add Quarters to Image.
    fnt = ImageFont.truetype(FONT_PATH_BOLD, 37)
    for i in range (0, len(quarters)):
        draw.text((210 + (i * 310), 457),f"{quarters[i][0]} Q{quarters[i][1]}",(255,255,255),font=fnt)

    # Add Logo to Image.
    logo_img = get_image_data(LOGO_PATH)
    im = logo_img.copy()
    template.paste(im, (1262, 753))    
    write_image(template, ts_output)

    template.show()

if (__name__ == "__main__"):
    if (is_valid()):
        auto_time_liner()