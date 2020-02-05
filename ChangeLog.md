Omniscient Change Log

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