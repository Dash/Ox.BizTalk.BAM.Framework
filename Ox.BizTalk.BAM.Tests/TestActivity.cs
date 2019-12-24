using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ox.BizTalk.BAM;

namespace Ox.BizTalk.BAM.Test
{
	[BamActivity(Name = "Test Activity")]
	public class TestActivity : ActivityBase
	{
		public TestActivity() : base()
		{ }

		public TestActivity(string activityId)
			: base(activityId)
		{ }

		public TestActivity(IEventStreamMediator mediator)
			: base(mediator)
		{ }

		public TestActivity(IEventStreamMediator mediator, string activityId)
			: base(mediator, activityId)
		{ }

		public string ActivityCustomString1 { get; set; }
		public DateTime ActivityCustomDate1 { get; set; }
		public int ActivityCustomInt1 { get; set; }
		[BamField(FieldName = "Renamed Field")]
		public int ActivityFieldNameChange { get; set; }
		[BamField(NullifyDefaultValues = true)]
		public DateTime ActivityNullableDate { get; set; }
		[BamField(NullifyDefaultValues = true)]
		public string ActivityValueType { get; set; }
		[BamField(NullifyDefaultValues = false)]
		public DateTime ActivityNonNullableDate { get; set; }

	}
}
