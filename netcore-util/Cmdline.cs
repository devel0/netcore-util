using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;

namespace SearchAThing
{

    /// <summary>
    /// item type of a command line parse element
    /// </summary>
    public enum CmdlineParseItemType
    {
        /// <summary>
        /// parse item represent a command ( string at begin )
        /// </summary>
        command,

        /// <summary>
        /// parse item represent a flag ( "-x", "--x" )
        /// </summary>
        flag,

        /// <summary>
        /// parse item represent a parameter ( those strings at end )
        /// </summary>
        parameter,

        /// <summary>
        /// parse item represent a parameter array ( those strings at end )
        /// </summary>
        parameterArray,
    }

    /// <summary>
    /// Command line parser item.
    /// The implementation of enumerable reports Values ( useful when this item is a parameter array )
    /// </summary>
    public class CmdlineParseItem : IEnumerable<string>
    {

        /// <summary>
        /// type of this parse cmdline argument item
        /// </summary>
        /// <value></value>
        public CmdlineParseItemType Type { get; private set; }

        /// <summary>
        /// command name ( taken from shorname )
        /// </summary>
        public string Command => ShortName;

        /// <summary>
        /// parameter name ( taken from shortname )
        /// </summary>
        public string ParameterName => ShortName;

        /// <summary>
        /// states if this item have a short name definition
        /// </summary>        
        public bool HasShortName => !string.IsNullOrWhiteSpace(ShortName);

        /// <summary>
        /// short name for this item or null if not set
        /// </summary>        
        public string ShortName { get; internal set; }

        /// <summary>
        /// states if this item have a long name definition
        /// </summary>        
        public bool HasLongName => !string.IsNullOrWhiteSpace(LongName);

        /// <summary>
        /// long name for this item or null if not set
        /// </summary>        
        public string LongName { get; internal set; }

        /// <summary>
        /// if true this item must specified on command line.
        /// if not present an error occur and usage will printed out.
        /// </summary>        
        public bool Mandatory { get; internal set; }

        /// <summary>
        /// states if this element requires a value to set.
        /// the name here will be used in usage as label.        
        /// </summary>
        public bool HasValueName => !string.IsNullOrWhiteSpace(ValueName);

        /// <summary>
        /// label for the value of this parse item.
        /// if null the option doesn't parse or search for a value assignment.
        /// </summary>
        public string ValueName { get; internal set; }

        /// <summary>
        /// description used in usage to describe this item
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// if this item is a command then this sub parser allow to go deep of another level
        /// (see examples/cmdline-parser-04)
        /// </summary>
        public CmdlineParser CommandParser { get; internal set; }

        /// <summary>
        /// true if this command, flag or argument parsed
        /// </summary>
        public bool Matches { get; internal set; }

        /// <summary>
        /// value of this item ( for non val flags its the flat itself )
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        /// values if this item is a parameters array
        /// </summary>
        internal List<string> values = new List<string>();

        /// <summary>
        /// values if this item is a parameters array
        /// </summary>
        public IReadOnlyList<string> Values => values;

        /// <summary>
        /// states if this item matched
        /// </summary>
        public static implicit operator bool(CmdlineParseItem item)
        {
            return item.Matches;
        }

        /// <summary>
        /// retrieve item value
        /// </summary>
        public static implicit operator string(CmdlineParseItem item)
        {
            return item.Value;
        }

        /// <summary>
        /// create a parse item
        /// </summary>
        internal CmdlineParseItem(CmdlineParseItemType type)
        {
            Type = type;
        }

        /// <summary>
        /// helper to retrieve flag name in compact form either short, long or short and long
        /// </summary>
        public string FlagName
        {
            get
            {
                var sb = new StringBuilder();

                if (HasShortName) sb.Append(ShortName);
                if (HasLongName)
                {
                    if (HasShortName) sb.Append(",");
                    sb.Append(LongName);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// retrieve the list of values for a parameter array item
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    /// <summary>
    /// Command line parser helper with multilevel nested command support.
    /// </summary>
    public class CmdlineParser
    {

        /// <summary>
        /// non null if this parser is a nested parser
        /// </summary>    
        public CmdlineParser Parent { get; private set; }

        /// <summary>
        /// list of item parser
        /// </summary>
        List<CmdlineParseItem> items = new List<CmdlineParseItem>();

        /// <summary>
        /// if set to false -h builtin short flag not used
        /// </summary>
        public bool ShortHelp { get; set; } = true;

        /// <summary>
        /// if set to false --help builtin long flag not used
        /// </summary>            
        public bool LongHelp { get; set; } = true;

        /// <summary>
        /// describe the program in summary ( printed out with usage )
        /// </summary>
        public string ProgramDescription { get; private set; }

        /// <summary>
        /// construct a command line parser.
        /// through builder command, flag, argument and nested parser can be added
        /// </summary>
        public CmdlineParser(string programDescription, Action<CmdlineParser> builder)
        {
            ProgramDescription = programDescription;
            builder(this);
        }

        /// <summary>
        /// command parsed ( if any )
        /// </summary>
        public string Command
        {
            get
            {
                if (CommandItem == null) return null;
                return CommandItem.Command;
            }
        }

        /// <summary>
        /// command item parsed ( if any )
        /// </summary>        
        public CmdlineParseItem CommandItem { get; private set; }

        /// <summary>
        /// user action that will called from the parser if arguments matches without errors.
        /// this is the primary entry point for application execution ( post-cmdline )
        /// </summary>
        public Action OnCmdlineMatch { get; set; }

        /// <summary>
        /// count how much parser parent there are.
        /// 0 if this is the primary parser
        /// </summary>
        int ParentCount
        {
            get
            {
                var cnt = 0;
                CmdlineParser p = this;
                while (p.Parent != null)
                {
                    ++cnt;
                    p = p.Parent;
                }
                return cnt;
            }
        }

        #region parse and run
        /// <summary>
        /// this must called only once with main arguments, then through OnCmdlineMatch user can customize application
        /// </summary>        
        public void Run(string[] _args)
        {
            var parseFailed = false;

            List<CmdlineParseItem> commands = null;
            List<CmdlineParseItem> flags = null;
            List<CmdlineParseItem> parameters = null;
            List<CmdlineParseItem> parametersArray = null;
            {
                var g = items.GroupBy(w => w.Type).ToDictionary(w => w.Key, w => w.ToList());
                g.TryGetValue(CmdlineParseItemType.command, out commands);
                g.TryGetValue(CmdlineParseItemType.flag, out flags);
                g.TryGetValue(CmdlineParseItemType.parameter, out parameters);
                g.TryGetValue(CmdlineParseItemType.parameterArray, out parametersArray);
            }
            if (commands == null) commands = new List<CmdlineParseItem>();
            if (flags == null) flags = new List<CmdlineParseItem>();
            if (parameters == null) parameters = new List<CmdlineParseItem>();
            if (parametersArray == null) parametersArray = new List<CmdlineParseItem>();

/*
            if (commands.Count > 0 && commands.Count - ParentCount == 0)
            {
                throw new Exception($"commands must 0 or more than 1");
            }*/

            var args = new List<string>();

            #region preprocess args ( convert { '-d', 'val' } to { '-d=val' } )
            {
                var flagsMatched = new HashSet<CmdlineParseItem>();
                var skipnext = false;

                Func<string, CmdlineParseItem> match = (a) => flags.FirstOrDefault(f =>
                {
                    if (flagsMatched.Contains(f)) return false;

                    if (f.HasShortName && a == $"-{f.ShortName}") return true;

                    if (f.HasLongName && a == $"--{f.LongName}") return true;

                    return false;
                });

                foreach (var (arg, idx, isLast) in _args.Skip(ParentCount).WithIndexIsLast())
                {
                    if (skipnext)
                    {
                        skipnext = false;
                        continue;
                    }

                    if (commands.Count > 0 && idx == 0)
                    {
                        var qcmd = commands.FirstOrDefault(w => w.Command == arg);
                        if (qcmd == null)
                        {
                            System.Console.WriteLine($"ERROR: must specify a command");
                            parseFailed = true;
                            break;
                        }
                        else
                        {
                            CommandItem = qcmd;
                            qcmd.Matches = true;
                        }
                    }

                    var qflag = match(arg);

                    if (qflag != null) flagsMatched.Add(qflag);

                    // if this arg matches and nextone not matches then glue them
                    if (qflag != null && qflag.HasValueName && idx < _args.Length - 1 && match(_args[idx + 1]) == null)
                    {
                        args.Add($"{arg}={_args[idx + 1]}");
                        skipnext = true;
                    }
                    else
                        args.Add(arg);
                }
            }
            #endregion

            var helpRequested = false;

            #region final parse
            if (!parseFailed)
            {
                var flagsMatched = new HashSet<CmdlineParseItem>();

                Func<string, CmdlineParseItem> match = (a) => flags.FirstOrDefault(f =>
                {
                    if (flagsMatched.Contains(f)) return false;

                    if (f.HasShortName)
                    {
                        if (f.HasValueName)
                        {
                            if (a.StartsWith($"-{f.ShortName}=")) return true;
                        }
                        else
                        {
                            if (a == $"-{f.ShortName}") return true;
                        }
                    }

                    if (f.HasLongName)
                    {
                        if (f.HasValueName)
                        {
                            if (a.StartsWith($"--{f.LongName}=")) return true;
                        }
                        else
                        {
                            if (a == $"--{f.LongName}") return true;
                        }
                    }

                    return false;
                });

                int lastMatchingFlag = -1;

                foreach (var (arg, idx, isLast) in args.WithIndexIsLast())
                {
                    if (parseFailed) break;

                    if (idx == 0 && !string.IsNullOrWhiteSpace(Command)) continue;

                    var qflag = match(arg);

                    if (qflag != null)
                    {
                        if (flagsMatched.Contains(qflag))
                        {
                            System.Console.WriteLine($"ERROR: flag [{qflag.FlagName}] specified twice");
                            parseFailed = true;
                            break;
                        }

                        var indexOfEq = arg.IndexOf('=');
                        if (qflag.HasValueName && indexOfEq == -1)
                        {
                            System.Console.WriteLine($"ERROR: missing value for flag [{qflag.FlagName}]");
                            parseFailed = true;
                            break;
                        }
                        qflag.Value = arg.Substring(indexOfEq + 1);
                        qflag.Matches = true;

                        lastMatchingFlag = idx;

                        flagsMatched.Add(qflag);
                    }
                    else
                    {
                        if ((ShortHelp && arg == "-h") || (LongHelp && arg == "--help"))
                        {
                            helpRequested = true;
                            break;
                        }
                    }
                }

                if (!helpRequested)
                {
                    var qmissing = flags.FirstOrDefault(r => !flagsMatched.Contains(r) && r.Mandatory);

                    if (qmissing != null)
                    {
                        System.Console.WriteLine($"ERROR: missing mandatory flag [{qmissing.FlagName}]");
                        parseFailed = true;
                    }

                    var paramMatchCount = 0;
                    if (parameters.Count > 0)
                    {
                        var idx = lastMatchingFlag + 1;

                        foreach (var param in parameters)
                        {
                            if (idx >= args.Count)
                            {
                                if (param.Mandatory)
                                {
                                    System.Console.WriteLine($"ERROR: missing mandatory parameter [{param.ParameterName}]");
                                    parseFailed = true;
                                }
                                break;
                            }
                            param.Value = args[idx];
                            param.Matches = true;
                            ++idx;
                            ++paramMatchCount;
                        }
                    }

                    if (parametersArray.Count > 0)
                    {
                        if (parametersArray.Count > 1)
                        {
                            System.Console.WriteLine($"ERROR: parameters array can't specified twice");
                            parseFailed = true;
                        }
                        else
                        {
                            var idx = lastMatchingFlag + 1 + paramMatchCount;
                            var parr = parametersArray.First();

                            if (idx >= args.Count)
                            {
                                if (parr.Mandatory)
                                {
                                    System.Console.WriteLine($"ERROR: missing mandatory parameter [{parr.ParameterName}]");
                                    parseFailed = true;
                                }
                            }
                            else
                            {
                                while (idx < args.Count)
                                {
                                    parr.values.Add(args[idx++]);
                                }
                                parr.Matches = true;
                            }
                        }
                    }
                }
            }
            #endregion

            CmdlineParser subParser = null;

            if (CommandItem != null && CommandItem.CommandParser != null)
            {
                subParser = CommandItem.CommandParser;
                subParser.Run(_args);
            }
            else
            {
                if (helpRequested || parseFailed || args.Count == 0)
                {
                    PrintUsage();
                }
                else
                {
                    if (OnCmdlineMatch != null)
                        OnCmdlineMatch();
                }
            }
        }
        #endregion

        #region add options
        /// <summary>
        /// add a command.
        /// While foreach cmdline there can be specified only one command here you can set alternative commands available.
        /// Commands will be strings at begin of commandline.
        /// </summary>
        public CmdlineParseItem AddCommand(string command, string description, CmdlineParser commandParser = null)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.command && w.ShortName == command))
            {
                throw new Exception($"a command named [{command}] already exists");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.command)
            {
                ShortName = command,
                Description = description,
                CommandParser = commandParser
            };
            items.Add(item);

            if (commandParser != null) commandParser.Parent = this;

            return item;
        }

        /// <summary>
        /// add a parameter ( strings at end of command line, after flags )
        /// </summary>
        public CmdlineParseItem AddParameter(string name, string description, bool mandatory = true)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.parameter && w.ShortName == name))
            {
                throw new Exception($"a parameter named [{name}] already exists");
            }

            if (!mandatory && items.Count(w => w.Type == CmdlineParseItemType.parameter && w.Mandatory) > 0)
            {
                throw new Exception($"can't add optional parameter after mandatories");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.parameter)
            {
                ShortName = name,
                Description = description,
                Mandatory = mandatory,
            };
            items.Add(item);

            return item;
        }

        /// <summary>
        /// add a parameter array item ( strings at end of command line, after flags and single parameters )
        /// </summary>
        public CmdlineParseItem AddParameterArray(string name, string description, bool mandatory = true)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.parameterArray && w.ShortName == name))
            {
                throw new Exception($"a parameter array named [{name}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add more than 1 array parameter");
            }

            if (!mandatory && items.Count(w => w.Type == CmdlineParseItemType.parameter && !w.Mandatory) > 0)
            {
                throw new Exception($"can't add array parameter when there are optional parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.parameterArray)
            {
                ShortName = name,
                Description = description,
                Mandatory = true,
            };
            items.Add(item);

            return item;
        }

        /// <summary>
        /// add optional short switch.
        /// if valueName given a value must specified together this switch if used.
        /// </summary>
        public CmdlineParseItem AddShort(string shortName, string description, string valueName = null)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.flag && w.ShortName == shortName))
            {
                throw new Exception($"a flag named [{shortName}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameter || w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add flag after parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.flag)
            {
                ShortName = shortName,
                Description = description,
                Mandatory = false,
                ValueName = valueName
            };
            items.Add(item);
            return item;
        }

        /// <summary>
        /// add mandatory short switch.
        /// if valueName given a value must specified together this switch if used.
        /// </summary>
        public CmdlineParseItem AddMandatoryShort(string shortName, string description, string valueName)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.flag && w.ShortName == shortName))
            {
                throw new Exception($"a flag named [{shortName}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameter || w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add flag after parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.flag)
            {
                ShortName = shortName,
                Description = description,
                Mandatory = true,
                ValueName = valueName
            };
            items.Add(item);
            return item;
        }

        /// <summary>
        /// add optional long switch.
        /// if valueName given a value must specified together this switch if used.
        /// </summary>
        public CmdlineParseItem AddLong(string longName, string description, string valueName = null)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.flag && w.LongName == longName))
            {
                throw new Exception($"a flag named [{longName}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameter || w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add flag after parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.flag)
            {
                LongName = longName,
                Description = description,
                Mandatory = false,
                ValueName = valueName
            };
            items.Add(item);
            return item;
        }

        /// <summary>
        /// add mandatory short switch.
        /// if valueName given a value must specified together this switch if used.
        /// </summary>
        public CmdlineParseItem AddMandatoryLong(string longName, string description, string valueName)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.flag && w.LongName == longName))
            {
                throw new Exception($"a flag named [{longName}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameter || w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add flag after parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.flag)
            {
                LongName = longName,
                Description = description,
                Mandatory = true,
                ValueName = valueName
            };
            items.Add(item);
            return item;
        }

        /// <summary>
        /// add optional short,long switch.
        /// if valueName given a value must specified together this switch if used.
        /// </summary>
        public CmdlineParseItem AddShortLong(string shortName, string longName, string description, string valueName = null)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.flag && (w.ShortName == shortName || w.LongName == longName)))
            {
                throw new Exception($"a flag named [{shortName}] or [{longName}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameter || w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add flag after parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.flag)
            {
                ShortName = shortName,
                LongName = longName,
                Description = description,
                Mandatory = false,
                ValueName = valueName,
            };
            items.Add(item);
            return item;
        }

        /// <summary>
        /// add mandatory short,long switch.
        /// if valueName given a value must specified together this switch if used.
        /// </summary>
        public CmdlineParseItem AddMandatoryShortLong(string shortName, string longName, string description, string valueName)
        {
            if (items.Any(w => w.Type == CmdlineParseItemType.flag && (w.ShortName == shortName || w.LongName == longName)))
            {
                throw new Exception($"a flag named [{shortName}] or [{longName}] already exists");
            }

            if (items.Count(w => w.Type == CmdlineParseItemType.parameter || w.Type == CmdlineParseItemType.parameterArray) > 0)
            {
                throw new Exception($"can't add flag after parameter");
            }

            var item = new CmdlineParseItem(CmdlineParseItemType.flag)
            {
                ShortName = shortName,
                LongName = longName,
                Description = description,
                Mandatory = true,
                ValueName = valueName,
            };
            items.Add(item);
            return item;
        }

        #endregion

        #region print usage
        /// <summary>
        /// print cmdline usage including if this is a subcommand any of inherited flags.
        /// this will invoked automatically when a parse error occurs.
        /// </summary>
        public void PrintUsage()
        {
            System.Console.WriteLine();
            {
                var sb = new StringBuilder();

                sb.Append($"Usage: {AppDomain.CurrentDomain.FriendlyName}");
                if (Parent != null)
                {
                    var sbp = new StringBuilder();
                    var p = Parent;
                    while (p != null)
                    {
                        sbp.Append($" {p.Command}");
                        p = p.Parent;
                    }
                    sb.Append(sbp);
                }
                if (items.Any(w => w.Type == CmdlineParseItemType.command)) sb.Append(" COMMAND");
                if (items.Any(w => w.Type == CmdlineParseItemType.flag)) sb.Append(" FLAGS");
                foreach (var strOpt in items.Where(w => w.Type == CmdlineParseItemType.parameter))
                {
                    sb.Append($" {strOpt.ShortName}");
                }
                var qStrArr = items.FirstOrDefault(w => w.Type == CmdlineParseItemType.parameterArray);
                if (qStrArr != null) sb.Append($" {qStrArr.ParameterName}...");
                System.Console.WriteLine(sb.ToString());
            }
            System.Console.WriteLine();
            System.Console.WriteLine(ProgramDescription);
            System.Console.WriteLine();

            var width = 0;

            Action<List<CmdlineParseItem>, List<CmdlineParseItem>, bool> sweepOpts = (_items, _inheritedItems, onlyEvalWidth) =>
             {
                 #region commands
                 {
                     var cmds = _items.Where(r => r.Type == CmdlineParseItemType.command);
                     if (cmds.Any())
                     {
                         if (!onlyEvalWidth)
                         {
                             System.Console.WriteLine("Commands:");
                             System.Console.WriteLine();
                         }
                         foreach (var o in cmds)
                         {
                             var sb = new StringBuilder();

                             sb.Append($"  {o.Command}");

                             if (onlyEvalWidth)
                             {
                                 width = Max(width, sb.Length);
                             }
                             else
                             {
                                 System.Console.Write(sb.ToString().Align(width));
                                 System.Console.WriteLine($"   {o.Description}");
                             }
                         }
                         if (!onlyEvalWidth) System.Console.WriteLine();
                     }
                 }
                 #endregion

                 #region short/long options
                 Action<List<CmdlineParseItem>, bool> sweepShortLongOpts = (_xitems, _are_inherited) =>
             {
                 foreach (var opt in _xitems.Where(r => r.Type == CmdlineParseItemType.flag).GroupBy(w => w.Mandatory))
                 {
                     if (!onlyEvalWidth)
                     {
                         if (_are_inherited) System.Console.Write("(inherited) ");
                         if (opt.Key)
                         {
                             System.Console.WriteLine("Mandatory flags:");
                         }
                         else
                         {
                             System.Console.WriteLine("Optional flags:");
                         }
                         System.Console.WriteLine();
                     }

                     foreach (var o in opt)
                     {
                         var sb = new StringBuilder();

                         sb.Append("  ");
                         if (!string.IsNullOrWhiteSpace(o.ShortName))
                             sb.Append($"-{o.ShortName}");
                         if (!string.IsNullOrWhiteSpace(o.LongName))
                         {
                             if (!string.IsNullOrWhiteSpace(o.ShortName)) sb.Append(",");
                             sb.Append($"--{o.LongName}");
                         }
                         if (!string.IsNullOrWhiteSpace(o.ValueName))
                         {
                             sb.Append($"={o.ValueName}");
                         }

                         if (onlyEvalWidth)
                         {
                             width = Max(width, sb.Length);
                         }
                         else
                         {
                             System.Console.Write(sb.ToString().Align(width));
                             System.Console.WriteLine($"   {o.Description}");
                         }
                     }

                     if (!onlyEvalWidth) System.Console.WriteLine();
                 }
             };
                 sweepShortLongOpts(_items, false);
                 sweepShortLongOpts(_inheritedItems, true);
                 #endregion

                 #region parameters
                 {
                     var parameters = _items.Where(r => r.Type == CmdlineParseItemType.parameter)
                         .Concat(_items.Where(r => r.Type == CmdlineParseItemType.parameterArray));
                     if (parameters.Any())
                     {
                         if (!onlyEvalWidth)
                         {
                             System.Console.WriteLine("Parameters:");
                             System.Console.WriteLine();
                         }
                         foreach (var o in parameters)
                         {
                             var sb = new StringBuilder();

                             sb.Append($"  ");
                             if (!o.Mandatory)
                                 sb.Append("[");
                             sb.Append($"{o.ParameterName}");
                             if (!o.Mandatory)
                                 sb.Append("]");
                             if (o.Type == CmdlineParseItemType.parameterArray) sb.Append("...");

                             if (onlyEvalWidth)
                             {
                                 width = Max(width, sb.Length);
                             }
                             else
                             {
                                 System.Console.Write(sb.ToString().Align(width));
                                 System.Console.WriteLine($"   {o.Description}");
                             }
                         }
                         if (!onlyEvalWidth) System.Console.WriteLine();
                     }
                 }
                 #endregion
             };

            var inheritedItems = InheritedItems;

            sweepOpts(items, inheritedItems, true);
            sweepOpts(items, inheritedItems, false);

        }
        #endregion

        /// <summary>
        /// list of inherited items. if this is a subcommand parser all of parent's parser items will inherited.
        /// </summary>
        public List<CmdlineParseItem> InheritedItems
        {
            get
            {
                var res = new List<CmdlineParseItem>();

                var p = Parent;
                while (p != null)
                {
                    res.AddRange(p.items);

                    p = p.Parent;
                }

                return res;
            }
        }

        #region tostring
        /// <summary>
        /// retrieve a table representation of all items parsed.
        /// </summary>        
        public override string ToString()
        {
            var rows = new List<List<string>>();
            foreach (var x in items.Union(InheritedItems))
            {
                rows.Add(new List<string>()
                {
                    x.Type.ToString(),
                    x.ShortName,
                    x.LongName,
                    x.Description,
                    x.Mandatory.ToString(),
                    x.Matches.ToString(),
                    x.Type == CmdlineParseItemType.parameterArray ? $"[ {string.Join(",", x.Values.Select(w=>$"\"{w}\""))} ]" : x.Value
                });
            }
            return rows.TableFormat(new[] { "TYPE", "SHORT-NAME", "LONG-NAME", "DESCRIPTION", "MANDATORY", "MATCHES", "VALUE" });
        }
        #endregion

    }

}
