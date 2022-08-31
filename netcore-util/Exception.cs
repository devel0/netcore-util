using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace SearchAThing
{

    public class ErrorInfo
    {

        public string Message { get; set; } = "";
        public string ExceptionType { get; set; } = "";
        public string Stacktrace { get; set; } = "";
        public string InnerException { get; set; } = "";

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool includeStackTrace)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"exception message : [{Message}]");
            sb.AppendLine($"exception type : [{ExceptionType}]");
            if (includeStackTrace) sb.AppendLine($"stacktrace : [{Stacktrace}]");

            return sb.ToString();
        }

    }

    public static partial class UtilExt
    {

        public static string Details(this Exception ex, bool includeStackTrace = true) =>
            DetailsObject(ex).ToString(includeStackTrace);

        public static ErrorInfo DetailsObject(this Exception _ex)
        {
            var res = new ErrorInfo();

            try
            {
                var ex = _ex;

#if NET6_0_OR_GREATER
                if (_ex.InnerException is Npgsql.PostgresException pex)
                {
                    res.Message = $"{pex.Message} [table:{pex.TableName}] [constraint:{pex.ConstraintName}] [routine:{pex.Routine}]";
                    res.ExceptionType = pex.GetType().ToString();
                    res.Stacktrace = pex.StackTrace.Fn(w => w == null ? "" : w.ToString());
                }
                else
#endif


                {

                    res.Message = ex.Message;
                    res.ExceptionType = ex.GetType().ToString();
                    res.Stacktrace = ex.StackTrace.Fn(w => w == null ? "" : w.ToString());
                    res.InnerException = ex.InnerException != null ? ex.InnerException.Message : "";
                }

                return res;
            }
            catch (Exception ex0)
            {
                res.Message = $"exception generating ex detail : {ex0?.Message}";
            }

            return res;
        }

    }

    /// <summary>
    /// InternalError exception
    /// </summary>
    public class InternalError : Exception
    {

        /// <summary>
        /// Internal exception constructor, some assert failed
        /// </summary>
        /// <param name="msg">assert fail msg</param>
        public InternalError(string msg) : base(msg)
        {

        }

    }

}

