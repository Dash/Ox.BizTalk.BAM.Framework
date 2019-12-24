using System;
using System.Collections.Generic;
using System.Text;

namespace Ox.BizTalk.BAM.Example
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Get our "business logic" class
			// Using a little WMI BamHelper to work out the locally configured BAM Primary Import connection string
			ExampleWorkflow api = new ExampleWorkflow(BamHelper.GetBamPrimaryImportConnectionString());

			// Process our imaginary request
			var token = api.ReceiveRequest(10);

			// Process our imaginary request fulfillment
			api.FulfilRequests(token, 5);
		}
	}
}
