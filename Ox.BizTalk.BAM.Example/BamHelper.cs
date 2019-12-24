using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Ox.BizTalk.BAM.Example
{
	/// <summary>
	/// Helper for connecting to BizTalk BAM infrastructure
	/// </summary>
	public static class BamHelper
	{
		private const string MGMT_ROOT = @"root\MicrosoftBizTalkServer";

		/// <summary>
		/// Gets a SQL connection string for the currently configured BAMPrimaryImportDb (does not have to be installed locally to this machine)
		/// </summary>
		/// <remarks>
		/// When a BizTalk Group is configured on a machine, irrespective of where the SQL server is, configuration data is stored in 
		/// WMI.  We can retrieve this (if running as Administrator) to automatically configure connection strings etc.
		/// </remarks>
		/// <returns>SQL Connection string using Integrated Security</returns>
		public static string GetBamPrimaryImportConnectionString()
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(MGMT_ROOT, "SELECT BamDBName,BamDBServerName FROM MSBTS_GroupSetting"); // WHERE Name = 'TrackingDBName' OR Name = 'TrackingDBServerName'");

			string dbName = null, dbHost = null;

			foreach(ManagementObject queryObj in searcher.Get())
			{
				dbName = queryObj["BamDBName"] as string;
				dbHost = queryObj["BamDBServerName"] as string;
			}

			return String.Format("Server={0};Database={1};Integrated Security=SSPI", dbHost, dbName);
		}
	}
}
