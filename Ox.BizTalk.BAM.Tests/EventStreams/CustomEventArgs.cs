using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ox.BizTalk.BAM.Test.EventStreams
{
	public class ActivityEventArgs : EventArgs
	{
		public Dictionary<string, object> Arguments = new Dictionary<string, object>();
	}
}
