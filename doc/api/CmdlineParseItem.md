# CmdlineParseItem Class
**Namespace:** SearchAThing

**Inheritance:** Object → CmdlineParseItem

Command line parser item.
            The implementation of enumerable reports Values ( useful when this item is a parameter array )

## Signature
```csharp
public class CmdlineParseItem : System.Collections.Generic.IEnumerable<string>, System.Collections.IEnumerable
```
## Methods
|**Name**|**Summary**|
|---|---|
|[Equals](CmdlineParseItem/Equals.md)||
|[GetEnumerator](CmdlineParseItem/GetEnumerator.md)|retrieve the list of values for a parameter array item|
|[GetHashCode](CmdlineParseItem/GetHashCode.md)||
|[GetType](CmdlineParseItem/GetType.md)||
|[ToString](CmdlineParseItem/ToString.md)||
## Properties
|**Name**|**Summary**|
|---|---|
|[Command](CmdlineParseItem/Command.md)|command name ( taken from shorname )
|[CommandParser](CmdlineParseItem/CommandParser.md)|if this item is a command then this sub parser allow to go deep of another level<br/>            (see examples/cmdline-parser-04)
|[Description](CmdlineParseItem/Description.md)|description used in usage to describe this item
|[FlagName](CmdlineParseItem/FlagName.md)|helper to retrieve flag name in compact form either short, long or short and long
|[HasLongName](CmdlineParseItem/HasLongName.md)|states if this item have a long name definition
|[HasShortName](CmdlineParseItem/HasShortName.md)|states if this item have a short name definition
|[HasValueName](CmdlineParseItem/HasValueName.md)|states if this element requires a value to set.<br/>            the name here will be used in usage as label.
|[LongName](CmdlineParseItem/LongName.md)|long name for this item or null if not set
|[Mandatory](CmdlineParseItem/Mandatory.md)|if true this item must specified on command line.<br/>            if not present an error occur and usage will printed out.
|[Matches](CmdlineParseItem/Matches.md)|true if this command, flag or argument parsed
|[ParameterName](CmdlineParseItem/ParameterName.md)|parameter name ( taken from shortname )
|[ShortName](CmdlineParseItem/ShortName.md)|short name for this item or null if not set
|[Type](CmdlineParseItem/Type.md)|type of this parse cmdline argument item
|[Value](CmdlineParseItem/Value.md)|value of this item ( for non val flags its the flat itself )
|[ValueName](CmdlineParseItem/ValueName.md)|label for the value of this parse item.<br/>            if null the option doesn't parse or search for a value assignment.
|[Values](CmdlineParseItem/Values.md)|values if this item is a parameters array
## Conversions
