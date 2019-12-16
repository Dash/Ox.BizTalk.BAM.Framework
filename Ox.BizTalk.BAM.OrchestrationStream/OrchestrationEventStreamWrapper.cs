using System;
using Microsoft.BizTalk.Bam.EventObservation;

namespace Ox.BizTalk.BAM
{
	/// <summary>
	/// Wraps up the XLANG <see cref="OrchestrationEventStream" /> to allow use with <see cref="Ox.BizTalk.BAM.EventStreamMediator"/>
	/// </summary>
	/// <remarks>
	/// OrchestrationEventStream is in a different assembly to the rest of the Eventing code and does not derive from
	/// <see cref="Microsoft.BizTalk.Bam.EventObservation.EventStream"/>.  <see cref="Ox.BizTalk.BAM.ActivityBase"/> requires a <see cref="Ox.BizTalk.BAM.IEventStreamMediator"/>.
	/// 
	/// To allow for a generic implementation this wrapper simply allows both Orchestrations and other DLLs to use the same
	/// definitions and inject the most appropriate event stream at runtime.
	/// </remarks>
	[Serializable]
    public class OrchestrationEventStreamWrapper : IEventStreamMediator
	{
		public OrchestrationEventStreamWrapper()
		{
			this.TimeoutValue = 0;
		}

		public int TimeoutValue { get; set; }

		public void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData)
		{
			OrchestrationEventStream.AddReference(activityName, activityID, referenceType, referenceName, referenceData);
		}

		public void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData, string longreferenceData)
		{
			OrchestrationEventStream.AddReference(activityID, activityID, referenceType, referenceName, referenceData, longreferenceData);
		}

		public void AddRelatedActivity(string activityName, string activityID, string relatedActivityName, string relatedTraceID)
		{
			OrchestrationEventStream.AddRelatedActivity(activityName, activityID, relatedActivityName, relatedTraceID);
		}

		public void BeginActivity(string activityName, string activityInstance)
		{
			OrchestrationEventStream.BeginActivity(activityName, activityInstance);
		}

		public void Clear()
		{
			OrchestrationEventStream.Clear();
		}

		public void EnableContinuation(string activityName, string activityInstance, string continuationToken)
		{
			OrchestrationEventStream.EnableContinuation(activityName, activityInstance, continuationToken);
		}

		public void EndActivity(string activityName, string activityInstance)
		{
			OrchestrationEventStream.EndActivity(activityName, activityInstance);
		}

		private void Flush()
		{
			// Do nothing, OrchestrationEventStream doesn't support this, but we don't want to throw an error
		}

		public void UpdateActivity(string activityName, string activityInstance, params object[] data)
		{
			OrchestrationEventStream.UpdateActivity(activityName, activityInstance, data);
		}

		void IEventStreamMediator.Flush()
		{
			
		}
	}
}
