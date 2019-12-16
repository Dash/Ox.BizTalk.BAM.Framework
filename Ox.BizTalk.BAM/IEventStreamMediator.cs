namespace Ox.BizTalk.BAM
{
	/// <summary>
	/// Wraps up a BizTalk BAM API EventStream for usage with this framework
	/// </summary>
	public interface IEventStreamMediator
	{
		int TimeoutValue { get; set; }

		void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData);
		void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData, string longreferenceData);
		void AddRelatedActivity(string activityName, string activityID, string relatedActivityName, string relatedTraceID);
		void BeginActivity(string activityName, string activityInstance);
		void Clear();
		void EnableContinuation(string activityName, string activityInstance, string continuationToken);
		void EndActivity(string activityName, string activityInstance);
		void Flush();
		void UpdateActivity(string activityName, string activityInstance, params object[] data);
	}
}
