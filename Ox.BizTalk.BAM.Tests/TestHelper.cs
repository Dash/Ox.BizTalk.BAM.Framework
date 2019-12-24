using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ox.BizTalk.BAM.Test.EventStreams;

namespace Ox.BizTalk.BAM.Test
{
	internal static class TestHelper
	{
		public static TestActivity GetTestActivity()
		{
			return ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(new TestEventStream()));
		}

		public static TestActivity GetTestActivity(string activityId)
		{
			return ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(new TestEventStream()), activityId);
		}

		private static Random _rnd = new Random();

		public static Random Rnd
		{
			get
			{
				return _rnd;
			}
		}

		public static Dictionary<string, object> BamDataToDictionary(this object[] data)
		{
			Dictionary<string, object> result = new Dictionary<string, object>();

			for(int i = 0; i < data.Length; i++)
			{
				// Evens are keys
				if(i % 2 == 0)
				{
					// Flush through as the value comes second
					result.Add((string)data[i], data[++i]);
				}
			}

			return result;
		}
	}
}
