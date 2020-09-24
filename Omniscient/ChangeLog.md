Omniscient Change Log

##  0.9.6
2020-09-24

* Switched to .NET Framework 4.7.2


##  0.9.5
2020-09-22

### New Features
* GRAND Instruments have a "status" channel
* GRAND Instruments have an option to hide flag values from the gamma channels 

### Bug Fixes
* Fixed an issue resulting in incorrect threshold event mean values
* Fixed an issue resulting in a crash if the columns in the event table were double clicked without an event select


##  0.9.4
2020-08-07

### New Features
* Added a Refresh Data button to the Main Window
* Added XY charts for plotting channels against eachother and fitting curves to the data
* Added plateau event-detection
* Included the status values in WUCS instruments
* Added a modified-time channel to file instruments
* The mean value of data during a threshold or plateau event is now recorded

### Bug Fixes
* Fixed an issue where exported NCC data would be stored out of order
* Fixed an issue where out-of-date preset files could result in a crash


##  0.9.3
2020-07-17

### New Features
* More intuitive Events sizing after resizing the Main Window

### Bug Fixes
* Read WUCS files with non-integer values without crashing


##  0.9.2
2020-07-13

### New Features
* An option to view lines with data points on the strip charts was added

### Bug Fixes
* Fixed an issue where renaming an instrument would not update the name of its first channel
* Fixed a issue in which right clicking on virtual channel in a chart could cause a crash


##  0.9.1
2020-07-10

### New Features
* Instruments for SMMS, WUCS, and DATAZ data were added
* An Instrument was added for generic viewing of files with timestamps in the file name
* Data files can be shown in Windows Explorer by right clicking in a strip chart
* There are buttons in the Main Window menu bar to shift the view either to the start or to the end of available data
* Right clicking on an item in the Main Window Site Tree will bring up a context menu for adding, viewing, and removing items
* There is a button to collapse/expand the Events Table
* There is a button in the Main Window menu bar to collapse/expand all panels
* When starting Omniscient, the panels are automatically collapsed/expanded as they were when it was last viewed
* A Preset named "default" will automatically load when starting Omniscient
* ROI Virtual Channels can be based on either spectrum channel number or calibrated energy
* HGM spectrum files can viewed either from an MCAInstrument or in the spectrum viewer
* A group of spectra in a Main Window strip chart can be summed and displayed in the spectrum viewer
* Zooming out with the mouse in a strip chart gives a more intuitive result near the start and end of the data range
* The layout in the Site Manager was made slightly easier to use
* Performance improvements when deselecting instruments
* Many improvements to the spectrum viewer (Inspectrum) including:
    * New chart style
    * The mouse can be used to zoom in the same manner to the Main Window strip charts
    * When zooming, the chart will auto-scale (unless this is disabled by the user)
    * The horizontal scrollbar can be used to shift the displayed range
    * When spectra are loaded from a Main Window strip chart, there are buttons to view the previous or next spectrum from the Instrument
    * Spectra can be exported for external analysis

### Bug Fixes
* Fixed a bug that would cause a crash when the strip chart scrollbar got out of range
* Fixed several issues with the strip charts that could result in crashes
* Fixed an issue where a strip chart could remain visible and empty after an Instrument was deselected


##  0.9.0
2020-06-15

### New Features
* The chart area is sized to make better use of space


##  0.5.1
2020-06-12

### New Features
* When using the "Open File" button, the instrument and its channels are automatically be selected and the display range is set to view the full range of data
* There is a "Zoom to Full Range" button that zooms out to view the full range of data
* The color of a series can be set from the channel panels (and will no longer automatically change as channels are selected and deselected)
* Presets save which channels are displayed on which charts, their series colors, and line styles (connected lines or points])

### Bug Fixes
* Zooming into a range that was less than 1 second wide would make it very difficult to zoom out again
* Zooming in by dragging the mouse off the chart would sometimes cause a crash
* Presets could fail if two channels had the same name


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
* The Y-axis will auto scale when using the mouse to zoom in. This can be overridden by holding down the CTRL button.

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