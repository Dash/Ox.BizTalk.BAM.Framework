using System;
using Microsoft.BizTalk.Bam.EventObservation;

namespace Ox.BizTalk.BAM
{
	/// <summary>
	/// Generates Activity classes with relevant Event Stream
	/// </summary>
	public class ActivityFactory
	{

		/// <summary>
		/// Creates a new instance of your activity with a DirectEventStream underpinning it.
		/// </summary>
		/// <typeparam name="TActivity">Your activity</typeparam>
		/// <param name="connectionString">BAMPrimaryImport connection string</param>
		/// <param name="activityId">Optional activity ID, Guid will be used if left null</param>
		/// <returns>New instance of your activity with id and event stream setup</returns>
		public static TActivity NewDirectActivity<TActivity>(string connectionString, string activityId = null)
			where TActivity : ActivityBase, new()
		{
			return NewActivity<TActivity>(new EventStreamMediator(new DirectEventStream(connectionString, 1)), activityId);
		}

		/// <summary>
		/// Creates a new instance of your activity with a DirectEventStream underpinning it.
		/// </summary>
		/// <param name="activityType">Your activity type</typeparam>
		/// <param name="connectionString">BAMPrimaryImport connection string</param>
		/// <param name="activityId">Optional activity ID, Guid will be used if left null</param>
		/// <returns>New instance of your activity with id and event stream setup</returns>
		public static object NewDirectActivity(Type activityType, string connectionString, string activityId = null)
		{
			return NewActivity(activityType, new EventStreamMediator(new DirectEventStream(connectionString, 1)), activityId);
		}

		/// <summary>
		/// Creates a new instance of your activity with a BufferedEventStream underpinning it.
		/// </summary>
		/// <typeparam name="TActivity">Your activity</typeparam>
		/// <param name="connectionString">BAMPrimaryImport connection string</param>
		/// <param name="activityId">Optional activity ID, Guid will be used if left null</param>
		/// <returns>New instance of your activity with id and event stream setup</returns>
		public static TActivity NewBufferedActivity<TActivity>(string connectionString, string activityId = null)
			where TActivity : ActivityBase, new()
		{
			return NewActivity<TActivity>(new EventStreamMediator(new BufferedEventStream(connectionString, 1)), activityId);
		}

		/// <summary>
		/// Creates a new instance of your activity with a BufferedEventStream underpinning it.
		/// </summary>
		/// <param name="activityType">Your activity type</typeparam>
		/// <param name="connectionString">BAMPrimaryImport connection string</param>
		/// <param name="activityId">Optional activity ID, Guid will be used if left null</param>
		/// <returns>New instance of your activity with id and event stream setup</returns>
		public static object NewBufferedActivity(Type activityType, string connectionString, string activityId = null)
		{
			return NewActivity(activityType, new EventStreamMediator(new BufferedEventStream(connectionString, 1)), activityId);
		}

		/// <summary>
		/// Creates a new instance of your activity with the provided event stream underpinning it. One of the other methods might be more useful.
		/// </summary>
		/// <remarks>
		/// Due to the complex dependencies for the OrchestrationEventStream, there isn't a factory method for that, but you can pass a
		/// <see cref="Ox.BizTalk.BAM.OrchestrationEventStreamWrapper"/> into this method.
		/// </remarks>
		/// <typeparam name="TActivity">Your activity</typeparam>
		/// <param name="eventStreamMediator">Desired event stream mediator</param>
		/// <param name="activityId"></param>
		/// <returns>New instance of your activity with id and event stream setup</returns>
		public static TActivity NewActivity<TActivity>(IEventStreamMediator eventStreamMediator, string activityId = null)
			where TActivity : ActivityBase, new()
		{
			CheckActivityId(ref activityId);

			TActivity activity = new TActivity();
			InitialiseActivity(activity, eventStreamMediator, activityId);

			return activity;
		}

		/// <summary>
		/// Creates a new instance of your activity with the provided event stream underpinning it. One of the other methods might be more useful.
		/// </summary>
		/// <remarks>
		/// Due to the complex dependencies for the OrchestrationEventStream, there isn't a factory method for that, but you can pass a
		/// <see cref="Ox.BizTalk.BAM.OrchestrationEventStreamWrapper"/> into this method.
		/// </remarks>
		/// <param name="activityType">Your activity</typeparam>
		/// <param name="eventStreamMediator">Desired event stream mediator</param>
		/// <param name="activityId"></param>
		/// <returns>New instance of your activity with id and event stream setup</returns>
		public static object NewActivity(Type activityType, IEventStreamMediator eventStreamMediator, string activityId = null)
		{
			if (!typeof(ActivityBase).IsAssignableFrom(activityType)) throw new ArgumentException("Supplied type must be derived from ActivityBase.");

			CheckActivityId(ref activityId);

			ActivityBase activity = (ActivityBase) Activator.CreateInstance(activityType);
			InitialiseActivity(activity, eventStreamMediator, activityId);
			return activity;
		}

		private static void InitialiseActivity(ActivityBase activity, IEventStreamMediator eventStreamMediator, string activityId)
		{
			activity.ActivityId = activityId;
			activity.SetEventStreamMediator(eventStreamMediator);
		}

		private static string CheckActivityId(ref string activityId)
		{
			if(activityId == null)
			{
				activityId = Guid.NewGuid().ToString();
			}
			return activityId;
		}
	}
}
