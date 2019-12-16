using System;
using Microsoft.BizTalk.Bam.EventObservation;

namespace Ox.BizTalk.BAM
{
	/// <summary>
	/// Default wrapper for the BizTalk BAM EventObservation classes
	/// </summary>
	/// <remarks>
	/// The purpose of this mediator is to facilitate unit testing without having
	/// to bundle the BizTalk DLLs. See <see cref="Ox.BizTalk.BAM.IEventStreamMediator"/> for details.
	/// </remarks>
	[Serializable]
	public class EventStreamMediator : IEventStreamMediator, IDisposable
	{
		private readonly EventStream _underlyingStream;
		protected EventStream UnderlyingStream
		{
			get
			{
				return this._underlyingStream;
			}
		}

		public EventStreamMediator(EventStream stream)
		{
			this._underlyingStream = stream;
		}

		public int TimeoutValue
		{
			get
			{
				return this.UnderlyingStream.TimeoutValue;
			}
			set
			{
				this.UnderlyingStream.TimeoutValue = value;
			}
		}

		public void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData)
		{
			this.UnderlyingStream.AddReference(activityName, activityID, referenceType, referenceName, referenceData);
		}

		public void AddReference(string activityName, string activityID, string referenceType, string referenceName, string referenceData, string longreferenceData)
		{
			this.UnderlyingStream.AddReference(activityName, activityID, referenceType, referenceName, referenceData, longreferenceData);
		}

		public void AddRelatedActivity(string activityName, string activityID, string relatedActivityName, string relatedTraceID)
		{
			this.UnderlyingStream.AddRelatedActivity(activityName, activityID, relatedActivityName, relatedTraceID); 
		}

		public void BeginActivity(string activityName, string activityInstance)
		{
			this.UnderlyingStream.BeginActivity(activityName, activityInstance);
		}

		public void Clear()
		{
			this.UnderlyingStream.Clear();
		}

		public void EnableContinuation(string activityName, string activityInstance, string continuationToken)
		{
			this.UnderlyingStream.EnableContinuation(activityName, activityInstance, continuationToken);
		}

		public void EndActivity(string activityName, string activityInstance)
		{
			this.UnderlyingStream.EndActivity(activityName, activityInstance);
		}

		public void Flush()
		{
			this.UnderlyingStream.Flush();
		}

		public void UpdateActivity(string activityName, string activityInstance, params object[] data)
		{
			this.UnderlyingStream.UpdateActivity(activityName, activityInstance, data);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this.UnderlyingStream.Flush();
				}

				// : free unmanaged resources (unmanaged objects) and override a finalizer below.
				// : set large fields to null.

				disposedValue = true;
			}
		}

		// : override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ActivityBase() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// : uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
