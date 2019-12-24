using System;
using System.Collections.Generic;
using System.Text;

using Ox.BizTalk.BAM;
using Ox.BizTalk.BAM.Example.Activities;

namespace Ox.BizTalk.BAM.Example
{
	/// <summary>
	/// Imaginary business logic class
	/// </summary>
	public class ExampleWorkflow
	{
		/// <summary>
		/// Constructor takes the connection string for the BAM Primary Import database
		/// If you were using IOC/DI, you'd probably use IEventStreamMediator here instead as you
		/// could select the EventStream type at configuration time.
		/// </summary>
		/// <param name="connectionString">BAMPrimaryImport database</param>
		public ExampleWorkflow(string connectionString)
		{
			if(connectionString == null) throw new ArgumentNullException("connectionString");

			_connectionString = connectionString;
		}

		private readonly string _connectionString;

		/// <summary>
		/// Receive a request to process some units
		/// </summary>
		/// <param name="units">Number of units requested</param>
		/// <returns>Correlation token</returns>
		public string ReceiveRequest(int units)
		{

			// Grab an instance of our received activity using a factory to request a Direct event stream
			using(ReceivedRequests request = ActivityFactory.NewDirectActivity<ReceivedRequests>(_connectionString))
			{
				// Populate our business data
				request.Units = units;
				request.DataReceived = DateTime.Now;

				// Trigger the BAM infrastructure to commit this record to the db
				request.BeginActivity();
				request.CommitActivity();
				request.EndActivity();

				// Return the generated activity ID as a token for later relationships
				return request.ActivityId;
			}
		}

		/// <summary>
		/// Fulfil a previous request
		/// </summary>
		/// <param name="correlationToken">Correlation token from <see cref="ReceiveRequest(int)"/></param>
		/// <param name="quantity">Number of units to fulfill</param>
		public void FulfilRequests(string correlationToken, int quantity)
		{
			// Grab an instance of our fulfilment activity using a factory
			using(FulfilledRequests request = ActivityFactory.NewDirectActivity<FulfilledRequests>(_connectionString))
			{
				// Populate our business data
				request.DateSent = DateTime.Now;
				request.Quantity = quantity;

				// Trigger the BAM infrastructure to commit this record to the db
				request.BeginActivity();
				// Additionally, link this activity to the previous actvitiy we had
				request.AddReferenceToAnotherActivity("Received Requests", correlationToken);
				request.CommitActivity();
				request.EndActivity();
			}
		}
	}
}
