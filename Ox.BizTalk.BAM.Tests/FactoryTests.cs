using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ox.BizTalk.BAM;
using Ox.BizTalk.BAM.Test.EventStreams;

namespace Ox.BizTalk.BAM.Test
{
	[TestClass]
	public class FactoryTests
	{
		private readonly string _dummyConnectionString = "Server=null;Database=null;";

		private void ActivityCheckCommon(ActivityBase activity)
		{
			Assert.IsInstanceOfType(activity, typeof(TestActivity), "Activity not created");
			Assert.IsFalse(String.IsNullOrWhiteSpace(activity.ActivityId), "ActivityId is blank.");
			Assert.IsFalse(String.IsNullOrWhiteSpace(activity.ActivityName), "ActivityName is blank.");
		}

		[TestMethod]
		public void SetEventStreamMediator()
		{
			var activity = new TestActivity();
			activity.SetEventStreamMediator(new EventStreamMediator(new TestEventStream()));
		}

		[TestMethod]
		public void FailToSetEventStreamMediatorNull()
		{
			var activity = new TestActivity();
			Assert.ThrowsException<ArgumentNullException>(() => activity.SetEventStreamMediator(null));
		}


		[TestMethod]
		public void FactoryGetDirectActivityGeneric()
		{
			// Don't handle dispose as it'll try and connect to a SQL server that may not be there.
			var direct = ActivityFactory.NewDirectActivity<TestActivity>(_dummyConnectionString);

			// We have default stuff
			ActivityCheckCommon(direct);
		}

		[TestMethod]
		public void FactoryGetDirectActivityGenericWithId()
		{
			var activityId = Guid.NewGuid().ToString();

			// Don't handle dispose as it'll try and connect to a SQL server that may not be there.
			var direct = ActivityFactory.NewDirectActivity<TestActivity>(_dummyConnectionString, activityId);

			ActivityCheckCommon(direct);

			Assert.AreEqual(activityId, direct.ActivityId, "ActivityId is not as supplied.");
		}



		[TestMethod]
		public void FactoryGetDirectActivity()
		{
			var activity = (TestActivity)ActivityFactory.NewDirectActivity(typeof(TestActivity), _dummyConnectionString);
			ActivityCheckCommon(activity);
		}

		[TestMethod]
		public void FactoryGetDirectActivityWithId()
		{
			var activityId = Guid.NewGuid().ToString();
			var activity = (TestActivity)ActivityFactory.NewDirectActivity(typeof(TestActivity), _dummyConnectionString, activityId);

			ActivityCheckCommon(activity);

			Assert.AreEqual(activityId, activity.ActivityId);
		}

		[TestMethod]
		public void FactoryGetBufferedActivityGeneric()
		{
			// Don't handle dispose as it'll try and connect to a SQL server that may not be there.
			var activity = ActivityFactory.NewBufferedActivity<TestActivity>(_dummyConnectionString);

			// We have default stuff
			ActivityCheckCommon(activity);
		}

		[TestMethod]
		public void FactoryGetBufferedActivityGenericWithId()
		{
			var activityId = Guid.NewGuid().ToString();

			// Don't handle dispose as it'll try and connect to a SQL server that may not be there.
			var activity = ActivityFactory.NewBufferedActivity<TestActivity>(_dummyConnectionString, activityId);

			// We have the right object
			Assert.AreEqual(activityId, activity.ActivityId, "ActivityId is not as supplied.");

			// We have default stuff
			ActivityCheckCommon(activity);
		}



		[TestMethod]
		public void FactoryGetBufferedActivity()
		{
			var activity = (TestActivity)ActivityFactory.NewBufferedActivity(typeof(TestActivity), _dummyConnectionString);
			ActivityCheckCommon(activity);
		}

		[TestMethod]
		public void FactoryGetBufferedActivityWithId()
		{
			var activityId = Guid.NewGuid().ToString();
			var activity = (TestActivity)ActivityFactory.NewBufferedActivity(typeof(TestActivity), _dummyConnectionString, activityId);

			ActivityCheckCommon(activity);

			Assert.AreEqual(activityId, activity.ActivityId);
		}

		[TestMethod]
		public void FactoryNewActivityGeneric()
		{
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(new TestEventStream()));
			ActivityCheckCommon(activity);
		}

		[TestMethod]
		public void FactoryNewActivityGenericWithId()
		{
			var activityId = Guid.NewGuid().ToString();
			var activity = ActivityFactory.NewActivity<TestActivity>(new EventStreamMediator(new TestEventStream()), activityId);

			ActivityCheckCommon(activity);
			Assert.AreEqual(activityId, activity.ActivityId);
		}

		[TestMethod]
		public void FactoryNewActivity()
		{
			var activity = (TestActivity)ActivityFactory.NewActivity(typeof(TestActivity), new EventStreamMediator(new TestEventStream()));
			ActivityCheckCommon(activity);
		}

		[TestMethod]
		public void FactoryNewActivityWithId()
		{
			var activityId = Guid.NewGuid().ToString();
			var activity = (TestActivity)ActivityFactory.NewActivity(typeof(TestActivity), new EventStreamMediator(new TestEventStream()), activityId);

			ActivityCheckCommon(activity);
			Assert.AreEqual(activityId, activity.ActivityId);
		}

		[TestMethod]
		public void FactoryFailInvalidActivityType()
		{
			Assert.ThrowsException<ArgumentException>(() => ActivityFactory.NewActivity(typeof(object), new EventStreamMediator(new TestEventStream())));
		}
	}
}
