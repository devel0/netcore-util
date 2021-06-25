using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SearchAThing
{

    public static partial class UtilToolkit
    {

        /// <summary>
        /// Tail-like method to gather file incoming lines
        /// </summary>
        /// <param name="pathfilename">source pathfilename to tail</param>
        /// <param name="seekEnd">if false read from beginning (default:true)</param>
        /// <param name="ct">optional cancellation token to control stop of tail loop</param>
        /// <param name="BUFSIZE">read buffer size (default:1024)</param>
        /// <returns>return incoming lines from given file</returns>
        public static IEnumerable<string> TailLike(string pathfilename,
            bool seekEnd = true,
            CancellationToken? ct = null,
            int BUFSIZE = 1024)
        {
            using (var fs = new FileStream(pathfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (seekEnd)
                {
                    fs.Seek(0, SeekOrigin.End);
                }                

                char[] buf = new char[BUFSIZE];
                int bufCnt = 0;
                int bufOff = 0;

                using (var sr = new StreamReader(fs))
                {
                    while (true)
                    {
                        if (ct.HasValue && ct.Value.IsCancellationRequested)
                            break;

                        var cnt = sr.Read(buf, bufCnt, BUFSIZE - bufCnt);

                        bufCnt += cnt;

                        for (int i = bufOff; i < bufCnt; ++i)
                        {
                            if (buf[i] == '\r' || buf[i] == '\n')
                            {
                                yield return new String(buf, bufOff, i - bufOff);
                                if (buf[i] == '\r' && i + 2 < bufCnt && buf[i + 1] == '\n')
                                    ++i;
                                bufOff = i + 1;
                            }
                        }

                        if (bufCnt - bufOff >= BUFSIZE) throw new Exception($"buffer not enough for line length > {BUFSIZE}");

                        if (bufOff == bufCnt)
                        {
                            bufOff = 0;
                            bufCnt = 0;
                        }
                        else if (bufCnt > 0 && bufOff > 0)
                        {
                            var w = bufCnt - bufOff;
                            for (int i = 0; i < w; ++i)
                            {
                                buf[i] = buf[bufOff + i];
                            }
                            bufCnt -= bufOff;
                            bufOff = 0;
                        }
                    }
                }
            }
        }
    }

}