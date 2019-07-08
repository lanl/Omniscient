# Omniscient User Manual #


## Overview ##
Omniscient is a program for performing data review and analysis for nuclear safeguards applications. It provides a simple and intuitive interface for users of a wide range of technical skill levels to interpret safeguards instrumentation data. Omniscient is compatible with various forms of safeguards data and is capable of analyzing large quantities of this data.


## System Requirements ##
+ Windows 7, 8, or 10
+ .NET Framework 4.6.1 or newer
+ Screen resolution of at least 1280×720


## Installation ##
Omniscient is usually distributed as a .zip file. To install, simply unzip all of the files into the same directory.


## Organization ##
Data within Omniscient is organized into _Sites_, _Facilities_, _Systems_, _Instruments_, and _Channels_. The data structure can be seen in the Site Tree in the Omniscient Main Screen and can be edited using the Site Manager. 

### Sites ###
A _Site_ is a top-level form of organization that contains _Facilities_. To create a new _Site_,

1. Open the Site Manager from the Tools menu in the Omniscient Main Screen
2. Click the “New Site” button
3. Enter a name for the site
4. Click the “Save” button

### Facilities ###
_Facilities_ are the level of organization below Sites and contain _Systems_. To create a new _Facility_,

1. Open the Site Manager from the Tools menu in the Omniscient Main Screen
2. Select a Site from the Site Tree in which to create the _Facility_
3. Click the “New Facility” button
4. Enter a name for the _Facility_
5. Click the “Save” button

### Systems ###
A System contains a set of related _Instruments_, which generally are assaying or monitoring the same items or parts of a process. To create a new _System_,

1. Open the Site Manager from the Tools menu in the Omniscient Main Screen
2. Select a _Facility_ from the Site Tree in which to create the _System_
3. Click the “New System” button
4. Enter a name for the _System_
5. Click the “Save” button

### Instruments ###
An _Instrument_ contains data from a single piece of hardware. As of version 0.4.0, Omniscient is compatible with five types of _Instruments_: CSV, GRAND, Image, ISR, MCA, and NGAM. Each _Instrument_ contains one or more _Channels_, which each correspond to a single time-stamped list of data values. To create a new _Instrument_,

1. Open the Site Manager from the Tools menu in the Omniscient Main Screen
2. Select a _System_ from the Site Tree in which to create the Instrument
3. Click the “New Instrument” button
4. Select the _Instrument_ type you would like to create
5. Enter a name for the _Instrument_
6. Click on the “Data Directory” browse icon
7. Browse for the folder containing the instrument data
8. Click Ok
9. Click the “Save” button

Optionally, if data files from multiple _Instruments_ are stored in the same directory, the “File Prefix” setting can be used to only associate an _Instrument_ with data files whose names start with a particular prefix.

Some _Instrument_ types can be used for multiple different data file formats. Use the “File Extension” setting to select which data file format the _Instrument_ should use.


## Data Analysis ##
Omniscient has several tools to aid in analyzing data, including _Virtual Channels_ and _Event Generators_.

### Virtual Channels ###
_Virtual Channels_ are created by processing the data in other _Channels_ (including other _Virtual Channels_) within an _Instrument_. They can be used to turn raw data into more useful quantities. As of version 0.4.0, the types of _Virtual Channels_ available in Omniscient are,

+ Convolve
+ Delay
+ Local Statistic
    - Max
    - Min
    - Average
    - Standard Deviation
+ Scalar Operation
    - Sum
    - Product
    - Power
+ Transcendental
    - Abs
    - Acos
    - Asin
    - Atan
    - Cos
    - Cosh
    - Exp
    - Log
    - Log10
    - Sign
    - Sin
    - Sinh
    - Sqrt
    - Tan
    - Tanh
+ Two Channel
    - Sum
    - Difference
    - Product
    - Ratio

The order of _Virtual Channels_ within an _Instrument_ can be important because _Virtual Channels_ can only operate on _Channels_ above them. By default, all standard _Instrument_ _Channels_ come first and are fixed in their order. _Virtual Channels_ can be reordered below the standard _Instrument_ Channels. To create a _Virtual Channel_,

1. Open the Site Manager
2. Select the _Instrument_ from the Site Tree in which to create the _Virtual Channel_
3. Click the “+” button next to the “Channels” combo box
4. Select the type of _Virtual Channel_ that you would like to create
5. In the “Virtual Channel” box, enter a name for the new _Virtual Channel_
6. Enter in the rest of the settings in the “Virtual Channel” box for the given _Virtual Channel_ type
7. Click the “Save “ button
8. Use the arrow buttons in the “Virtual Channel” box to reorder the _Virtual Channel_

### Event Generators ###
_Event Generators_ create _Events_ under pre-programmed conditions. As of version 0.4.0, the types of _Event Generators_ available in Omniscient are _Threshold_, _Coincidence_, and _Gap_. The order of _Event Generators_ within a _System_ can be important because _Event Generators_ can use _Events_ generated by previous _Event Generators_ to trigger new _Events_. To create an _Event Generator_,

1. Open the Event Manager from the Tools menu in the Omniscient Main Screen
2. Select the _System_ from the Site Tree in which to create the _Event Generator_
3. Click the “+” button
4. Select the type of _Event Generator_ that you would like to create
5. Enter a name for the new _Event Generator_
6. Enter the rest of the settings for the given _Event Generator_ type
7. Click the “Save” button
8. Use the arrow buttons to reorder the _Event Generator_

_Threshold Events_ are generated when a value in a particular _Channel_ exceeds a user-set threshold. _Coincidence Events_ are generated when _Events_ from two user-set _Event Generators_ are generated within a user-set window. _Coincident Events_ can be set to only be generated when the _Events_ that they trigger on occur in a particular order. _Gap Events_ are generated when there time interval between data points in a particular _Channel_ that exceeds a user-set minimum interval.


## Data Review ##
Omniscient can be used to review time series data, spectral data, images, and event lists. In order for data from an Instrument to be reviewed, it must be loaded into Omniscient. This is done by selecting the _Instrument_ from the Site Tree in the Main Window. Subsequently, Channel Panels for the _Instrument’s_ _Channels_ (including _Virtual Channels_) will be populated on the right side of the Main Window. To display the time series data from a _Channel_ on one of the four Strip Charts, click the corresponding checkbox next to the _Channel’s_ name in its Channel Panel.

### Strip Chart Navigation ###
The easiest way to navigate data in a Strip Chart is using the mouse. Left clicking and dragging the mouse from left to right over a range of data will zoom in on that range of data. Left clicking and dragging the mouse from right to left on a Strip Chart will zoom out. Right clicking and dragging the mouse will shift the range of data displayed. The mouse wheel is used to adjust the vertical scale on a Strip Chart. Alternatively, the vertical scale can also be adjusted by right clicking on the Strip Chart and selecting Set Y-Axis Range. The View Box in the bottom right corner of the Main Window can also be used to set the start time and time range to display on the Strip Charts.

A statistical summary of a range of data can be generated by holding down the Control key on keyboard, left clicking, and dragging the mouse over the range of data. The average value, standard deviation, maximum value, and minimum value for all the data points in that time range for each _Channel_ displayed on that Strip Chart is shown. The summary data can be removed by clicking within the range of data on strip chart and pressing the Delete key.

### Event Lists ###
To use _Event Generators_ to generate _Events_, they must be selected from the Site Tree in the Main Window. Also, any _Instruments_ containing _Channels_ on which the selected _Event Generators_ are dependent must be selected. To generate _Events_, click the “Generate Events” button above the Event List. Subsequently, the Event List will be populated. By checking the “Highlight Events” checkbox above the Event List, the generated _Events_ will also be displayed on the Strip Charts.