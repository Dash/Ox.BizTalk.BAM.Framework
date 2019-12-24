using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ox.BizTalk.BAM.Test.EventStreams;

namespace Ox.BizTalk.BAM.Test
{
	[TestClass]
	public class MediatorTests
	{
		[TestMethod]
		public void Clear()
		{
			var es = new TestEventStream();

			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				handler = (sender, e) => {
					rst.Set();
				};

				es.ClearCalled += handler;

				var mediator = new EventStreamMediator(es);
				mediator.Clear();

				Assert.IsTrue(rst.WaitOne(1000), "Clear() did not complete");
			}
			finally
			{
				if(handler != null) es.ClearCalled -= handler;
			}
		}
	}
}
