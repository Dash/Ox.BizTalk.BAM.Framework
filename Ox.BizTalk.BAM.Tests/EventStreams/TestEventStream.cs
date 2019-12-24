using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BizTalk.Bam.EventObservation;

namespace Ox.BizTalk.BAM.Test.EventStreams
{
	public class TestEventStream : EventStream
	{
		public event EventHandler<ActivityEventArgs> AddReferenceCalled = delegate { };

		public override void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData)
		{
			Debug.WriteLine("TestEventStream.AddReference");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityId", activityID);
			e.Arguments.Add("referenceType", referenceType);
			e.Arguments.Add("referenceName", referenceName);
			e.Arguments.Add("referenceData", referenceData);
			AddReferenceCalled(this, e);
			
		}

		public override void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData, string longreferenceData)
		{
			Debug.WriteLine("TestEventStream.AddReference");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityId", activityID);
			e.Arguments.Add("referenceType", referenceType);
			e.Arguments.Add("referenceName", referenceName);
			e.Arguments.Add("referenceData", referenceData);
			e.Arguments.Add("longreferenceData", longreferenceData);
			AddReferenceCalled(this, e);
		}

		public event EventHandler<ActivityEventArgs> BeginActivityCalled = delegate { };

		public override void BeginActivity(string activityName, string activityInstance)
		{
			Debug.WriteLine("TestEventStream.BeginActivity");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityInstance", activityInstance);
			BeginActivityCalled(this, e);
		}

		public event EventHandler<ActivityEventArgs> AddRelatedActivityCalled = delegate { };

		public override void AddRelatedActivity(string activityName, string activityID, string relatedActivityName, string relatedTraceID)
		{
			Debug.WriteLine("TestEventStream.AddRelatedActivity");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityID", activityID);
			e.Arguments.Add("relatedActivityName", relatedActivityName);
			e.Arguments.Add("relatedTraceID", relatedTraceID);
			AddRelatedActivityCalled(this, e);
		}

		public event EventHandler<ActivityEventArgs> ClearCalled = delegate { };

		public override void Clear()
		{
			ClearCalled(this, new ActivityEventArgs());
		}

		public event EventHandler<ActivityEventArgs> EnableContinuationCalled = delegate { };

		public override void EnableContinuation(string activityName, string activityInstance, string continuationToken)
		{
			Debug.WriteLine("TestEventStream.EnableContinuation");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityInstance", activityInstance);
			e.Arguments.Add("continuationToken", continuationToken);
			EnableContinuationCalled(this, e);
		}

		public event EventHandler<ActivityEventArgs> EndActivityCalled = delegate { };

		public override void EndActivity(string activityName, string activityInstance)
		{
			Debug.WriteLine("TestEventStream.EndActivity");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityInstance", activityInstance);
			EndActivityCalled(this, e);
		}

		public event EventHandler<ActivityEventArgs> StoreCustomEventCalled = delegate { };

		public override void StoreCustomEvent(IPersistQueryable singleEvent)
		{
			Debug.WriteLine("TestEventStream.StoreCustomEvent");
			var e = new ActivityEventArgs();
			e.Arguments.Add("singleEvent", singleEvent);
			StoreCustomEventCalled(this, e);
		}

		public event EventHandler<ActivityEventArgs> UpdateActivityCalled = delegate { };

		public override void UpdateActivity(string activityName, string activityInstance, params object[] data)
		{
			Debug.WriteLine("TestEventStream.UpdateActivity");
			var e = new ActivityEventArgs();
			e.Arguments.Add("activityName", activityName);
			e.Arguments.Add("activityInstance", activityInstance);
			e.Arguments.Add("data", data);
			UpdateActivityCalled(this, e);

		}

		public event EventHandler<ActivityEventArgs> FlushCalled = delegate { };
		public override void Flush()
		{
			Debug.WriteLine("TestEventStream.Flush");
			FlushCalled(this, new ActivityEventArgs());
		}

	}
}
