using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace SearchAThing
{

    public class ErrorInfo
    {

        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"exception message : [{Message}]");
            sb.AppendLine($"exception type : [{ExceptionType}]");
            sb.AppendLine($"stacktrace : [{Stacktrace}]");

            return sb.ToString();
        }

    }

    public static partial class UtilExt
    {

        public static string Details(this Exception ex)
        {
            return DetailsObject(ex).ToString();
        }

        public static ErrorInfo DetailsObject(this Exception _ex)
        {
            var res = new ErrorInfo();

            try
            {
                var ex = ((_ex.InnerException as Npgsql.PostgresException) != null) ? _ex.InnerException : _ex;

                if (ex is Npgsql.PostgresException)
                {
                    var pex = ex as Npgsql.PostgresException;

                    res.Message = $"{pex.Message} [table:{pex.TableName}] [constraint:{pex.ConstraintName}] [stmt:{pex.Statement}]";
                    res.ExceptionType = pex.GetType().ToString();
                    res.Stacktrace = pex.StackTrace.ToString();
                }
                else
                {

                    res.Message = ex.Message;
                    res.ExceptionType = ex.GetType().ToString();
                    res.Stacktrace = ex.StackTrace.ToString();
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

}

