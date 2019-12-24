using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ox.BizTalk.BAM.Test.EventStreams;

namespace Ox.BizTalk.BAM.Test
{
	[TestClass]
	public class ActivityTests
	{
		[TestMethod]
		public void Constructors()
		{
			EventStreamMediator es = new EventStreamMediator(new TestEventStream());
			var activityId = Guid.NewGuid().ToString();

			TestActivity act = new TestActivity();
			act = new TestActivity(es);
			act = new TestActivity(activityId);
			Assert.AreEqual(activityId, act.ActivityId);
			act = new TestActivity(es, activityId);
			Assert.AreEqual(activityId, act.ActivityId);
			act = new TestActivity((string)null);
			Assert.IsNotNull(act.ActivityId);
		}


		[TestMethod]
		public void BeginActivity()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityInstance"], "activityInstance");
					rst.Set();
				};

				es.BeginActivityCalled += handler;

				activity.BeginActivity();

				Assert.IsTrue(rst.WaitOne(1000), "Begin Activity failed to complete");
			}
			finally
			{
				if(handler != null) es.BeginActivityCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void CommitActivity()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				activity.ActivityCustomDate1 = DateTime.Now;
				activity.ActivityCustomInt1 = TestHelper.Rnd.Next();
				activity.ActivityCustomString1 = Guid.NewGuid().ToString();
				activity.ActivityFieldNameChange = TestHelper.Rnd.Next();
				activity.ActivityValueType = "foo";

				handler = (sender, e) => {

					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityInstance"], "activityInstance");

					var data = ((object[])e.Arguments["data"]).BamDataToDictionary();

					Assert.AreEqual(activity.ActivityCustomDate1, data["ActivityCustomDate1"]);
					Assert.AreEqual(activity.ActivityCustomInt1, data["ActivityCustomInt1"]);
					Assert.AreEqual(activity.ActivityCustomString1, data["ActivityCustomString1"]);
					Assert.AreEqual(activity.ActivityFieldNameChange, data["Renamed Field"]);
					Assert.AreEqual(null, data["ActivityNullableDate"]);

					rst.Set();
				};

				es.UpdateActivityCalled += handler;

				activity.CommitActivity();
				Assert.IsTrue(rst.WaitOne(1000), "Commit Activity failed to complete");
			}
			finally
			{
				if(handler != null) es.UpdateActivityCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void EndActivity()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityInstance"], "activityInstance");

					rst.Set();
				};

				es.EndActivityCalled += handler;

				activity.EndActivity();
				Assert.IsTrue(rst.WaitOne(1000), "End Activity failed to complete");

			}
			finally
			{
				if(handler != null)	es.EndActivityCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void AddCustomReference()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				string referenceType = "foo", referenceName = "bar", referenceData = "blee";

				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityId"], "activityInstance");

					Assert.AreEqual(referenceType, e.Arguments["referenceType"], "referenceType");
					Assert.AreEqual(referenceName, e.Arguments["referenceName"], "referenceName");
					Assert.AreEqual(referenceData, e.Arguments["referenceData"], "referenceData");

					rst.Set();
				};

				es.AddReferenceCalled += handler;

				activity.AddCustomReference(referenceType, referenceName, referenceData);
				Assert.IsTrue(rst.WaitOne(1000), "Add Reference failed to complete");

			}
			finally
			{
				if(handler != null) es.AddReferenceCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void AddCustomReference2()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				string referenceType = "foo", referenceName = "bar", referenceData = "blee", longReferenceData = "fnar";

				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityId"], "activityInstance");

					Assert.AreEqual(referenceType, e.Arguments["referenceType"], "referenceType");
					Assert.AreEqual(referenceName, e.Arguments["referenceName"], "referenceName");
					Assert.AreEqual(referenceData, e.Arguments["referenceData"], "referenceData");
					Assert.AreEqual(longReferenceData, e.Arguments["longreferenceData"], "longreferenceData");

					rst.Set();
				};

				es.AddReferenceCalled += handler;

				activity.AddCustomReference(referenceType, referenceName, referenceData, longReferenceData);
				Assert.IsTrue(rst.WaitOne(1000), "Add Reference failed to complete");

			}
			finally
			{
				if(handler != null) es.AddReferenceCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void AddReferenceToAnotherActivity()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityId"], "activityInstance");

					Assert.AreEqual("Activity", e.Arguments["referenceType"], "referenceType");
					Assert.AreEqual("activityName", e.Arguments["referenceName"], "referenceName");
					Assert.AreEqual("otherActivityId", e.Arguments["referenceData"], "referenceData");

					rst.Set();
				};

				es.AddReferenceCalled += handler;

				activity.AddReferenceToAnotherActivity("activityName", "otherActivityId");
				Assert.IsTrue(rst.WaitOne(1000), "Add Reference to Another Activity failed to complete");

			}
			finally
			{
				if(handler != null) es.AddReferenceCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void EnableContinuation()
		{
			var es = new TestEventStream();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es));
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				var token = Guid.NewGuid().ToString();

				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityInstance"], "activityInstance");

					Assert.AreEqual(token, e.Arguments["continuationToken"], "continuationToken");

					rst.Set();
				};

				es.EnableContinuationCalled += handler;

				activity.EnableContinuation(token);
				Assert.IsTrue(rst.WaitOne(1000), "End Activity failed to complete");

			}
			finally
			{
				if(handler != null) es.EnableContinuationCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void EnableContinuationGeneratedId()
		{
			var es = new TestEventStream();
			var activityId = Guid.NewGuid().ToString();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(es),activityId);
			EventHandler<ActivityEventArgs> handler = null;
			AutoResetEvent rst = new AutoResetEvent(false);

			try
			{
				

				handler = (sender, e) => {
					Assert.AreEqual(activity.ActivityName, e.Arguments["activityName"], "activityName");
					Assert.AreEqual(activity.ActivityId, e.Arguments["activityInstance"], "activityInstance");

					Assert.AreEqual("CONT_" + activityId, e.Arguments["continuationToken"], "continuationToken");

					rst.Set();
				};

				es.EnableContinuationCalled += handler;

				activity.EnableContinuation();
				Assert.IsTrue(rst.WaitOne(1000), "End Activity failed to complete");

			}
			finally
			{
				if(handler != null) es.EnableContinuationCalled -= handler;
				activity.Dispose();
			}
		}

		[TestMethod]
		public void RenameActivity()
		{
			using(var activity = new RenamedTestActivity())
			{
				Assert.AreEqual("FlibbleActivity", activity.ActivityName);
			}
		}

		[BamActivity(Name = "FlibbleActivity")]
		private class RenamedTestActivity : ActivityBase
		{ }
	}
}
