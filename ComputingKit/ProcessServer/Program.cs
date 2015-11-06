//----------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     This module implements Process Server Tool.
// </summary>
//----------------------------------------------------------------------------

namespace ProcessServerTool
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.Tts.Offline.Utility;

    /// <summary>
    /// ProcessServer arguments.
    /// </summary>
    [Comment("ProcessServer tool coordinates the jobs execution among the processing nodes.")]
    public class Arguments
    {
        #region Fields

        [Argument("aip", Description = "IP address of aggregator in the computing kit family",
            Optional = false, UsagePlaceholder = "aggregatorIP")]
        private string _aggregatorIp = string.Empty;

        [Argument("aport", Description = "Listening port of Aggregator Server in the computing kit family",
            Optional = false, UsagePlaceholder = "aggregatorPort")]
        private int _aggregatorPort = 0;

        [Argument("lport", Description = "Local port to listen for coming data packet",
            Optional = false, UsagePlaceholder = "localPort")]
        private int _localPort = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets aggregator IP.
        /// </summary>
        public string AggregatorIp
        {
            get { return _aggregatorIp; }
        }

        /// <summary>
        /// Gets aggregator port.
        /// </summary>
        public int AggregatorPort
        {
            get { return _aggregatorPort; }
        }

        /// <summary>
        /// Gets local port.
        /// </summary>
        public int LocalPort
        {
            get { return _localPort; }
        }

        #endregion
    }

    /// <summary>
    /// Process server application.
    /// </summary>
    internal class Program
    {
        #region Operations

        /// <summary>
        /// Main function of Process server.
        /// </summary>
        /// <param name="args">Argument strings.</param>
        /// <returns>Error code.</returns>
        private static int Main(string[] args)
        {
            return ConsoleApp<Arguments>.Run(args, Process);
        }

        /// <summary>
        /// Run the server.
        /// </summary>
        /// <param name="arguments">Argument.</param>
        /// <returns>If it finished successfully, then return successful code.</returns>
        private static int Process(Arguments arguments)
        {
            int ret = ExitCode.NoError;

            if (!Regex.Match(arguments.AggregatorIp, @"\d+\.\d+\.\d+\.\d+").Success)
            {
                Console.WriteLine(@"Invalid ip address, which should be like \d+\.\d+\.\d+\.\d+");
                ret = ExitCode.InvalidArgument;
            }
            else
            {
                string aggregatorIp = arguments.AggregatorIp;

                using (DistributeComputing.IProcessNode serverNode
                    = new DistributeComputing.ProcessServer(arguments.AggregatorIp,
                    arguments.AggregatorPort, arguments.LocalPort))
                {
                    serverNode.Start();
                    serverNode.Run();
                    serverNode.Stop();
                }
            }

            return ret;
        }

        #endregion
    }
}
