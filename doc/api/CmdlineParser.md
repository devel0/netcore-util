# CmdlineParser Class
**Namespace:** SearchAThing

**Inheritance:** Object → CmdlineParser

Command line parser helper with multilevel nested command support.

## Signature
```csharp
public class CmdlineParser
```
## Constructors
|**Name**|**Summary**|
|---|---|
|[CmdlineParser(string, Action<SearchAThing.CmdlineParser>)](CmdlineParser/ctors.md)|construct a command line parser.<br/>            through builder command, flag, argument and nested parser can be added|
|[CmdlineParser(Action<SearchAThing.CmdlineParser>)](CmdlineParser/ctors.md#cmdlineparseractionsearchathingcmdlineparser)|construct a command line parser ( retrieve description from parent parser command that contains this parser ).<br/>            through builder command, flag, argument and nested parser can be added|
## Methods
|**Name**|**Summary**|
|---|---|
|[AddCommand](CmdlineParser/AddCommand.md)|add a command.<br/>            While foreach cmdline there can be specified only one command here you can set alternative commands available.<br/>            Commands will be strings at begin of commandline.|
|[AddLong](CmdlineParser/AddLong.md)|add optional long switch.<br/>            if valueName given a value must specified together this switch if used.|
|[AddMandatoryLong](CmdlineParser/AddMandatoryLong.md)|add mandatory short switch.<br/>            if valueName given a value must specified together this switch if used.|
|[AddMandatoryParameter](CmdlineParser/AddMandatoryParameter.md)|add mandatory parameter ( strings at end of command line, after flags )|
|[AddMandatoryShort](CmdlineParser/AddMandatoryShort.md)|add mandatory short switch.<br/>            if valueName given a value must specified together this switch if used.|
|[AddMandatoryShortLong](CmdlineParser/AddMandatoryShortLong.md)|add mandatory short,long switch.<br/>            if valueName given a value must specified together this switch if used.|
|[AddParameter](CmdlineParser/AddParameter.md)|add optional parameter ( strings at end of command line, after flags )|
|[AddParameterArray](CmdlineParser/AddParameterArray.md)|add a parameter array item ( strings at end of command line, after flags and single parameters )|
|[AddShort](CmdlineParser/AddShort.md)|add optional short switch.<br/>            if valueName given a value must specified together this switch if used.|
|[AddShortLong](CmdlineParser/AddShortLong.md)|add optional short,long switch.<br/>            if valueName given a value must specified together this switch if used.|
|[Equals](CmdlineParser/Equals.md)||
|[GetHashCode](CmdlineParser/GetHashCode.md)||
|[GetType](CmdlineParser/GetType.md)||
|[PrintUsage](CmdlineParser/PrintUsage.md)|print cmdline usage including if this is a subcommand any of inherited flags.<br/>            this will invoked automatically when a parse error occurs.|
|[Run](CmdlineParser/Run.md)|this must called only once with main arguments, then through OnCmdlineMatch user can customize application|
|[ToString](CmdlineParser/ToString.md)|retrieve a table representation of all items parsed.|
## Properties
|**Name**|**Summary**|
|---|---|
|[AllItems](CmdlineParser/AllItems.md)|list of items including inherited items.
|[Command](CmdlineParser/Command.md)|command parsed ( if any )
|[CommandItem](CmdlineParser/CommandItem.md)|command item parsed ( if any )
|[InheritedItems](CmdlineParser/InheritedItems.md)|list of inherited items. if this is a subcommand parser all of parent's parser items will inherited.
|[Items](CmdlineParser/Items.md)|list of item parser
|[LongHelp](CmdlineParser/LongHelp.md)|if set to false --help builtin long flag not used
|[OnCmdlineMatch](CmdlineParser/OnCmdlineMatch.md)|user action that will called from the parser if arguments matches without errors.<br/>            this is the primary entry point for application execution ( post-cmdline )
|[Parent](CmdlineParser/Parent.md)|non null if this parser is a nested parser
|[ProgramDescription](CmdlineParser/ProgramDescription.md)|describe the program in summary ( printed out with usage )
|[ShortHelp](CmdlineParser/ShortHelp.md)|if set to false -h builtin short flag not used
## Conversions
