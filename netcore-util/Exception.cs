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

        public static ErrorInfo DetailsObject(this Exception ex)
        {
            var res = new ErrorInfo();

            try
            {
                res.Message = ex.Message;
                res.ExceptionType = ex.GetType().ToString();
                res.Stacktrace = ex.StackTrace.ToString();

                var sb = new StringBuilder();

                Func<Exception, string> inner_detail = null;
                inner_detail = (e) =>
                {
                    if (e is Npgsql.PostgresException)
                    {
                        var pex = e as Npgsql.PostgresException;

                        sb.AppendLine($"npgsql statement [{pex.Statement}]");
                    }

                        // TODO https://stackoverflow.com/questions/46430619/net-core-2-ef-core-error-handling-save-changes
                        /*
                        if (e is System.Data.Entity.Validation.DbEntityValidationException)
                        {
                            var dex = e as System.Data.Entity.Validation.DbEntityValidationException;

                            foreach (var deve in dex.EntityValidationErrors)
                            {
                                sb.Append($"db validation error entry : [{deve.Entry}]");

                                foreach (var k in deve.ValidationErrors)
                                {
                                    sb.Append($" {k.ErrorMessage}");
                                }
                                sb.AppendLine();
                            }
                        }*/

                    if (e.InnerException != null)
                    {
                        sb.AppendLine($"inner exception : {e.InnerException.Message}");

                        inner_detail(e.InnerException);
                    }
                    return "";
                };

                inner_detail(ex);

                if (sb.Length > 0)
                    res.InnerException = sb.ToString();

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

