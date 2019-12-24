using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ox.BizTalk.BAM.Test.EventStreams;

namespace Ox.BizTalk.BAM.Test
{
	[TestClass]
	public class ExceptionTests
	{
		[TestMethod]
		public void MissingEventStreamExceptions()
		{
			using(var activity = new TestActivity())
			{
				activity.ActivityId = "Blee";

				Assert.ThrowsException<ArgumentNullException>(() => activity.BeginActivity());
				Assert.ThrowsException<ArgumentNullException>(() => activity.AddCustomReference(String.Empty, String.Empty, String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.AddCustomReference(String.Empty, String.Empty, String.Empty, String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.AddReferenceToAnotherActivity(String.Empty, String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.CommitActivity());
				Assert.ThrowsException<ArgumentNullException>(() => activity.EnableContinuation());
				Assert.ThrowsException<ArgumentNullException>(() => activity.EnableContinuation(String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.EndActivity());
			}
		}

		[TestMethod]
		public void MissingActivityIdExceptions()
		{
			using(var activity = new TestActivity())
			{
				activity.SetEventStreamMediator(new EventStreamMediator(new TestEventStream()));

				Assert.ThrowsException<ArgumentNullException>(() => activity.BeginActivity());
				Assert.ThrowsException<ArgumentNullException>(() => activity.AddCustomReference(String.Empty, String.Empty, String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.AddCustomReference(String.Empty, String.Empty, String.Empty, String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.AddReferenceToAnotherActivity(String.Empty, String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.CommitActivity());
				Assert.ThrowsException<ArgumentNullException>(() => activity.EnableContinuation());
				Assert.ThrowsException<ArgumentNullException>(() => activity.EnableContinuation(String.Empty));
				Assert.ThrowsException<ArgumentNullException>(() => activity.EndActivity());
			}
		}

		[TestMethod]
		public void NullActivityName()
		{
			using(var activity = new NullTestActivity())
			{
				activity.SetEventStreamMediator(new EventStreamMediator(new TestEventStream()));
				activity.ActivityId = Guid.NewGuid().ToString();
				activity.BeginActivity();
			}
		}
		[TestMethod]
		public void OverwriteActivityIdException()
		{
			using(var activity = new TestActivity())
			{
				activity.ActivityId = "1";
				Assert.ThrowsException<InvalidOperationException>(() => activity.ActivityId = "2");
			}
		}

		[BamActivity(Name = null)]
		private class NullTestActivity : ActivityBase
		{ }
	}
}
