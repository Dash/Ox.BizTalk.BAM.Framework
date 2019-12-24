using System;
using Ox.BizTalk.BAM;

namespace Ox.BizTalk.BAM.Example.Activities
{
	/// <summary>
	/// Class to represent the "Fulfilled Requests" activity.
	/// We make use of the BamActivityAttribute to provide the activity name as it has a space in it, otherwise we wouldn't need to.
	/// </summary>
	[BamActivity(Name = "Fulfilled Requests")]
	public class FulfilledRequests : ActivityBase
	{
		/// <summary>
		/// Make use of the BamFieldActivity as the field name has a space in it
		/// Plus we want unspecified dates to be null in the database
		/// </summary>
		[BamField(FieldName = "Date Sent", NullifyDefaultValues = true)]
		public DateTime DateSent { get; set; }

		/// <summary>
		/// Field name is ok here.
		/// </summary>
		public int Quantity { get; set; }
	}
}
