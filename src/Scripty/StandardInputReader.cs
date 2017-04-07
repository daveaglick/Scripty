using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scripty
{
    /// <summary>
    /// Reads standard input (stdin). We have to be very careful because the calling process might have
    /// opened stdin and just left it open, in which case it would register as redirected but the
    /// stream won't ever return because it's just waiting for input.
    /// </summary>
    internal static class StandardInputReader
    {
        public static string Read()
        {
            if (Console.IsInputRedirected)
            {
                using (Stream stream = Console.OpenStandardInput())
                {
                    byte[] buffer = new byte[1000];
                    StringBuilder stdin = new StringBuilder();
                    int read = -1;
                    while (true)
                    {
                        AutoResetEvent gotInput = new AutoResetEvent(false);
                        Thread inputThread = new Thread(() =>
                        {
                            try
                            {
                                read = stream.Read(buffer, 0, buffer.Length);
                                gotInput.Set();
                            }
                            catch (ThreadAbortException)
                            {
                                Thread.ResetAbort();
                            }
                        })
                        {
                            IsBackground = true
                        };

                        inputThread.Start();

                        // Timeout expired?
                        if (!gotInput.WaitOne(250))
                        {
                            inputThread.Abort();
                            break;
                        }

                        // End of stream?
                        if (read == 0)
                        {
                            return stdin.ToString();
                        }

                        // Got data
                        stdin.Append(Console.InputEncoding.GetString(buffer, 0, read));
                    }

                    // Didn't get to the end of stream, but let's return what we've got
                    return stdin.ToString();
                }
            }

            return null;
        }
    }
}
