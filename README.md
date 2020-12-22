# AutoTimeLiner

**Summary:**
The AutoTimeLiner Application is an application developed to generate an image of a Software development (Product Delivery roadmap) and allows a development team to plan, set goals, and visualize Quarterly various tasks. 

The application has been developed in Python 3, and utilizes a number of 3rd party packages to accomplish this. It is still in the early stages of development but is in a usable state.

**Current Limitations as of December 2020:**
- Maximum of 6 tasks can be displayed on the image.
- Year is not honored, only month is considered 
    - for example, if dec 6 2019 and dec 6 2020 are used, they will show up at the same point in the plot.)

**Json File Format:**

```
{
    "team":"Enter TeamName Here:",
    "start_date":"12/15/2020",
    "ts_output":true,
    "projects":[ 
      { 
          "name":"Task 1 - Description",
          "label":"Status",
          "date":"2/22/2020"
       },
       { 
          "name":"Task 2 - Description",
          "label":"Status",
          "date":"1/25/2021"
       }
    ]
 }
 ```
 **Keys:**
 - team - Team Name to be displayed below Title.
 - start_data - Date to begin timeline (Start of quarter)
 - ts_output - [Optional] true adds the timestamp prefix to the output filename.
 - projects - [Maximum of 6] projects to be displayed in generated image.

**Prerequisites:**

    - Python 3.x - See notes below.
    - Required Directory Structure:
    -   Application Directory\
            Data\ - Contains sample input_data.json containing the data to be used.
            Font\ - Contains TrueType fonts to be used for text.
            Output\ - Images will be output to this directory.
            Template\
                Blank.png - Blank Template that will be used.
                Logo.png - Logo that will be displayed (143px x 79px)

**Python Setup:**

When python is executed on Windows 10, if it doesn't exist, the Windows Store app will be shown and allow the user to install. 

***Application developed with: Python version 3.9***

- A number of packages are required to make AutoTimeLiner work:

- PIP will allow the user to install the required packages.
    - Each command will be run from the Command Line

    - First upgrade PIP by running:
    -       pip install --upgrade pip.

    - Install the following packages by typing:
    -       pip install pillow
    -       pip install NUMPY

**Usage:**

     Python AutoTimeLiner.exe [Path]
- Arguments:

    - Path = Path to input json file, otherwise the Data\Input_data.json file will be used.

**Create Executable (.exe):**

- Install PyInstaller
-       pip install pyinstaller
    **NOTE:** *PyInstaller.exe may need to be added to the path as the executable may be unable to be found!*

**Create Executable:**

**IMPORTANT:** run from Command Prompt, Not Windows Powershell
-     pyinstaller.exe .\AutoTimeLiner.py -F

**Generated Executable will be in a new \dist directory.**
- The created executable will be in the \dist directory, and will still need all of the directories outlined above.

**Pro's to Executable:**
1. The user will be able to drag-n-drop a compatible .json file onto the executable, and the images will be created.
2. The Application can be transferred to a new computer and executed, without that PC needing to install Python and all of the related packages above.
3. The Application can be called from another program, or scripted if desired.

- The only Con to the Executable is the Size of the created .exe. When the executable is created, Python 3 and all required packages are contained within the exe, so resulting file tends to be quite large.
