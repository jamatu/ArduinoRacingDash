using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace iRacingSdkWrapper
{
    public class TelemetryCsvWriter
    {
        private readonly SdkWrapper wrapper;
        private int timeout = 10;
        private bool isConnected;

        public TelemetryCsvWriter()
        {
            wrapper = new SdkWrapper();
            wrapper.TelemetryUpdated += wrapper_TelemetryUpdated;
        }

        void wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            isConnected = true;
        }

        public void ReadHeaders(string filepath)
        {
            Thread t = new Thread(new ParameterizedThreadStart(ReadHeadersThread));
            t.Start(filepath);
        }

        private void ReadHeadersThread(object file)
        {
            string filepath = (string)file;
            isConnected = false;

            StringBuilder sb = new StringBuilder();

            wrapper.Start();

            // Wait for telemetry info update so we know the SDK is connected
            int attempts = 0;
            while (!isConnected)
            {
                if (attempts > timeout)
                {
                    wrapper.Stop();
                    throw new ArgumentException(
                        "Timeout connecting to sim, please run the sim, join a session and get on track first.");
                }

                attempts++;
                Thread.Sleep(1000);
            }

            // Write the column headers
            sb.AppendLine("Name,Desc,Unit,Type,Length");

            // Read the headers
            foreach (var header in wrapper.sdk.VarHeaders.Values)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3},{4}",
                                            header.Name,
                                            header.Desc,
                                            header.Unit,
                                            header.Type,
                                            header.Length / header.Bytes));
            }

            wrapper.Stop();

            string contents = sb.ToString();
            System.IO.File.WriteAllText(filepath, contents);
        }
    }
}
