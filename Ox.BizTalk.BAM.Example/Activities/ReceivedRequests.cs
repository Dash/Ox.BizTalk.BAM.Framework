using System;
using Ox.BizTalk.BAM;

namespace Ox.BizTalk.BAM.Example.Activities
{
	/// <summary>
	/// Class to represent the "Received Requests" activity.
	/// We make use of the BamActivityAttribute to provide the activity name as it has a space in it, otherwise we wouldn't need to.
	/// </summary>
	[BamActivity(Name = "Received Requests")]
	public class ReceivedRequests : ActivityBase
	{
		/// <summary>
		/// Make use of the BamFieldActivity as the field name has a space in it
		/// Plus we want unspecified dates to be null in the database
		/// </summary>
		[BamField(FieldName = "Data Received", NullifyDefaultValues = true)]
		public DateTime DataReceived { get; set; }

		/// <summary>
		/// Field name is ok here, we're going to use a Nullable int here, but this would
		/// be difficult to use from a BizTalk Orchestration.
		/// </summary>
		public int? Units { get; set; }
	}
}
