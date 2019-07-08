# Creating a New Instrument Type #
In Omniscient, an _Instrument_ contains data from a single piece of hardware (e.g. a shift register or an MCA). An _Instrument_ type generally corresponds to a particular file format or a series of file formats which are used to store data that can be reviewed in Omniscient. An effort has been to make it quick and easy to add new _Instrument_ types into Omniscient. This document is a guide for that process.

- - - - - - - - - - - - - - - - -

## Table of Contents #
- [Before You Start](#before-you-start)
- [Code Patterns](#code-patterns)
- [The Procedure, in Short](#the-procedure-in-short)
- [Classes](#classes)

- - - - - - - - - - - - - - - - -

## Before You Start ##
Do you even need to make a new _Instrument_ type? As of version 0.4.0, Omniscient can read:

+ .bid files (`GRANDInstrument`)
+ .isr, .jsr, and .hmr files (`ISRInstrument`)
+ .vbf files (`NGAMInstrument`)
+ .chn, .spe, and .n42 files (`MCAInstrument`)
+ .csv files (`CSVInstrument`)
+ .jpg files (`ImageInstrument`)

If you are still convinced that Omniscient needs surgery for your purposes, follow examples in the code as you read this guide. A couple of particularly simple _Instruments_ that make good examples include `GRANDInstrument` and `ISRInstrument`. A more sophisticated example, because it deals with a much more general file format, is `CSVInstrument`. If a new spectrum file type is to be included, consider modifying `MCAInstrument` rather than creating a whole new _Instrument_.


## Code Patterns ##
### The Separation Between UI and Logic ###
One principle to adhere to as much as possible is the separation between UI and logic. Generally, when adding a new _Instrument_ type to Omniscient, this is particularly easy. As long as the way in which the data for the new _Instrument_ type is interacted with in Omniscient is not very different from how data is used for other _Instruments_, absolutely no changes to the UI are needed. You're welcome. This is achieved using _Channels_ as a standard data format and _Hookups_ and _Parameters_ which let the developer simply say what data needs to be inputted and not how it needs to be inputted. New types of _Parameters_ may need to be added from time to time but doing so is less messy than mucking around in the `SiteManagerForm`.

### _Parsers_ and _Instruments_ ###
_Parsers_ are kept separate from _Instruments_ in Omniscient. If you are creating a new _Instrument_ type for a file type that Omniscient does not currently read, you will likely need to create a new _Parser_ as well. A _Parser_ simply takes data from a file and makes it accessible in memory. An _Instrument_ provides the interface to deal with data in Omniscient (using configuration _Parameters_ and _Channels_) but generally uses a _Parser_ to read a data file. This separation keeps the code cleaner and makes it easier to use _Parsers_ for other parts of the code (or for reuse in other programs altogether).

### External Libraries ###
Think before using any external libraries in Omniscient. External libraries can add system requirements (and Omniscient has frequently been used in situations where the system environment is not flexible). They can also affect the status (or complexity) of Omniscient as open source software. This not to say "do not use external libraries" - just take these considerations into account first.


## The Procedure, in Short ##
The three steps for creating a new _Instrument_ type are:

1. Create a derived class from `Instrument` to create a type-specific `Instrument` class
2. Create a derived class from `InstrumentHookup` to create a _Hookup_ for the new _Instrument_ type
3. Add an instance of the derived class from `InstrumentHookup` to `Hookups` in the base `Instrument` class


## Classes ##
There are three important classes in Omniscient to understand when creating a new _Instrument_ type: `Parameter`, `InstrumentHookup`, and `Instrument`.

### `Parameter` ###
A _Parameter_ represents some kind of user input that the _Instrument_ acts on. For example, several _Instrument_ types have a *File Extension* parameter which lets the user choose which type of files are read by an _Instrument_ that they have configured. _Parameters_ allow new _Instrument_ types to be developed without having to worry about making changes to the UI to configure them. They also reduce the potential for introducing bugs into Omniscient because _Parameter_ input automatically includes some validation.

### `InstrumentHookup` ###
The idea behind _Hookups_ in Omniscient is that they give the code an easy way to "plug in" a new _Instrument_ type or _EventGenerator_ type. A derived class from `InstrumentHookup` must be created for each new _Instrument_ type. In its constructor, `ParameterTemplate`s should be added to the `TemplateParameters` list for any _Parameters_ that the new _Instrument_ type uses. The accessor for the `Type` string should be overridden to return the new _Instrument_ type's "type." Finally, the derived class must override the `FromParameters` method. This method creates a new `Instrument` of the new type, given a list of _Parameters_. This method should use `Instrument.ApplyStandardInstrumentParameters(instrument, parameters)` to apply standard _Parameters_ and then configure the _Instrument_ with any custom _Parameters_ before returning the newly created _Instrument_ object.

In the `Instrument` base class, there is a static array of `InstrumentHookup` objects called `Hookups`. Add an instance of the derived `InstrumentHookup` class for the new _Instrument_ type to this array to connect it into the rest of Omniscient.

### `Instrument` ###
The `Instrument` class is an abstract class from which all other _Instrument_ classes derive. A new _Instrument_ type will need a new derived class from the `Instrument` base class. The derived class requires a constructor which calls the constructor of the base class. The constructor should also set the `InstrumentType` string the same value as the `Type` string in the new _Instrument_ type's _Hookup_. Additionally, the constructor should initialize the `channels` array. There are several abstract methods which derived classes must implement:

- `public abstract DateTime GetFileDate(string file)`
- `public abstract ReturnCode IngestFile(ChannelCompartment compartment, string fileName)`
- `public abstract List<Parameter> GetParameters()`
- `public abstract void ApplyParameters(List<Parameter> parameters)`

#### `GetFileDate` ####
The 'GetFileDate' method must return a `DateTime` associated with the given file path. If the file contains a timespan of data, the returned `DateTime` should refer to the start of the data in the file.

#### `IngestFile` ####
The `IngestFile` method should read the data in a given file and store it in the _Instrument's_ _Channels_. This generally involves using a _Parser_ to parse the file and then using the `AddDataPoint` method for each of the _Instrument's_ _Channels_. Because a lot of memory can get tied up in _Parsers_, remember to  release any resources associated with the Parser when done. If the `IngestFile` executes without error, it should return `ReturnCode.SUCCESS`.

#### `GetParameters` ####
The `GetParameters` method returns a list of _Parameters_ for the `Instrument`. The list should be started by using `List<Parameter> parameters = GetStandardInstrumentParameters();` to initialize the list. This ensures that standard _Instrument_ _Parameters_ are included. Afterwards, any _Instrument_-type-specific _Parameters_ should be added to the list before it is returned.

#### `ApplyParameters` ####
The `ApplyParameters` method is given a list of _Parameters_ (e.g. from user input or a configuration file) and needs to configure the `Instrument` using that input. Standard _Instrument_ parameters should be applied using `ApplyStandardInstrumentParameters(this, parameters);`. 