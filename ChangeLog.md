Omniscient Change Log

##  0.5.0
2020-05-11

### New Features
* BID files that are signed can be read.
* The side panels in the Main Window can be hidden.
* Channels that are in different instruments can have the same name and instruments that are in different systems can have the same name.
* New icon!


##  0.4.9
2020-02-07

### New Features
* There is a button to export the list of Events in the Event List.
* When Events are generated, the number of Events is shown in the status bar.
* Users can right click on an Event in the Event List and select "View Event" to move the view to the Event.
* The Y-axis will auto scale when using the mouse to zoom in. This can be overriden by holding down the CTRL button.

### Bug Fixes
* The context menu for the Event Grid now shows up where the user clicks.


##  0.4.8
2020-02-05

### New Features
* The NGAM instrument can be configured to read either .adc files or .vbf files.
* Activating an instrument from the Site Tree in the Main Window will now process faster if all the files in the instrument's data folder follow a naming scheme with dates in the file names.
* Chart legends can be hidden/shown from the right click menu on each chart.
* Backward and Forward buttons were added to go to previous date range views.
* The hotkey for Backward is Alt+Left and the hotkey for Forward is Alt+Right.
* Users can zoom on the Y-axis of charts using the mouse using similar controls to zooming on the X-axis.
* Zooming in on the X-axis can now be done over shorter time intervals.

### Bug Fixes
* The "Open File" button now works with .bid files.
* When the mouse is dragged out of the chart's x-axis range while zooming, strange behaviours no longer occur.
* The "Open File" button will no longer crash the program when opening a file that is already open.


##  0.4.7
2020-01-27

* Users can set location of app files (e.g. SiteManager.xml) in Omniscient.exe.config using the AppDataDirectory value.


##  0.4.6
2020-01-23

* The "Open File" button can be used to open files that Omniscient has parsers for. They will be placed as an instrument under the "AutoConfig" site.
* THD SOH files can be read.


##  0.4.5
2020-01-15

* ROI virtual channels are fixed.


##  0.4.4
2020-01-13

* ATPM data can be read.