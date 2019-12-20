using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ox.BizTalk.BAM
{
	[Serializable]
	public abstract class ActivityBase : IDisposable
	{
		private const string ERR_MISSING_MEDIATOR = "Event stream mediator not defined. Specify with constructor or call SetEventStreamMeditor()";

		public ActivityBase()
		{

		}

		public ActivityBase(string activityId)
		{
			if (activityId == null) activityId = Guid.NewGuid().ToString();

			this.ActivityId = activityId;
		}

		public ActivityBase(IEventStreamMediator eventStreamMediator) : this(eventStreamMediator, Guid.NewGuid().ToString())
		{
		}

		public ActivityBase(IEventStreamMediator eventStreamMediator, string activityId) : this(activityId)
		{
			this.SetEventStreamMediator(eventStreamMediator);
		}

		protected IEventStreamMediator _eventStream;
		
		private string _activityId = null;

		private string _activityName = null;
		public string ActivityName
		{
			get
			{
				if (this._activityName == null)
				{
					Type instance = this.GetType();
					string activityName = instance.Name;
					object[] classAttributes = instance.GetCustomAttributes(false);
					
					//.Where(x => x is BamActivityAttribute).Cast<BamActivityAttribute>().FirstOrDefault();
					foreach(object attr in classAttributes)
					{
						if(attr is BamActivityAttribute)
						{
							BamActivityAttribute bamActivityAttribute = (BamActivityAttribute)attr;

							if (bamActivityAttribute != null && !String.IsNullOrEmpty(bamActivityAttribute.Name))
							{
								activityName = bamActivityAttribute.Name;
							}
						}
					}

					this._activityName = activityName;
				}

				return this._activityName;
			}
		}

		public string ActivityId
		{
			get
			{
				return this._activityId;
			}
			set
			{
				if(this._activityId != null)
				{
					throw new InvalidOperationException("Unable to chagne the Activitiy id after it has already been defined.");
				}
				this._activityId = value;
			}
		}

		/// <summary>
		/// Sets the event stream mediator to handle actions against this data
		/// </summary>
		/// <remarks>
		/// Normally you'd use the constructor to specify this, but in situations where the mediator isn't know at object
		/// creation, this method can be used to set it at a later point.
		/// This isn't a property to avoid it from being serialized to TDDS.
		/// </remarks>
		/// <param name="eventStreamMediator">BAM Mediator</param>
		public virtual void SetEventStreamMediator(IEventStreamMediator eventStreamMediator)
		{
			if (eventStreamMediator == null) throw new ArgumentNullException("eventStreamMediator");

			this._eventStream = eventStreamMediator;
		}

		/// <summary>
		/// Begins the BAM activity.
		/// </summary>
		public virtual void BeginActivity()
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			this._eventStream.BeginActivity(this.ActivityName, this.ActivityId);
		}

		/// <summary>
		/// Write any data changes to the BAM activity. This must be called after any property changes.
		/// </summary>
		public virtual void CommitActivity()
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			List<object> data = new List<object>();

			var properties = this.GetType().GetProperties();

			foreach(var prop in properties)
			{
				// Explicitly skip Activity Name and Activity Id as these are system fields that can't be inserted as data
				if (prop.Name == "ActivityName" || prop.Name == "ActivityId")
					continue;

				string fieldName = prop.Name;
				object fieldValue = prop.GetValue(this, null);

				// Allows for overriding of field names where they are not compatible with CLR property names
				foreach(var att in prop.GetCustomAttributes(true))
				{
					if(att is BamFieldAttribute)
					{
						BamFieldAttribute bamField = (BamFieldAttribute)att;

						// Override field name
						if(bamField.FieldName != null)	fieldName = bamField.FieldName;

						// If the value is default, then set it to null
						if(fieldValue != null && bamField.NullifyDefaultValues)
						{
							// If value types match defaults
							if (prop.PropertyType.IsValueType && fieldValue.Equals(Activator.CreateInstance(prop.PropertyType)))
							{
								fieldValue = null;
							}
						}
					}
				}

				data.Add(fieldName);
				data.Add(fieldValue);
			}

			// Call BAM update
			this._eventStream.UpdateActivity(this.ActivityName, this.ActivityId, data.ToArray());
		}

		/// <summary>
		/// End the BAM activity. No more changes will be permitted to this activity except by continuation.
		/// </summary>
		public virtual void EndActivity()
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			this._eventStream.EndActivity(this.ActivityName, this.ActivityId);
		}

		/// <summary>
		/// Add a reference from this activity to another activity.
		/// </summary>
		/// <param name="otherActivityName">The related activity name. Reference names are limited to 128 characters.</param>
		/// <param name="otherActivityID">The related activity ID. Limited to 1024 characters of data.</param>
		public virtual void AddReferenceToAnotherActivity(string otherActivityName, string otherActivityID)
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			AddCustomReference("Activity", otherActivityName, otherActivityID);
		}

		/// <summary>
		/// Add a custom reference to this activity, this enables 'data' to be attached to an activity, such as a message body.
		/// </summary>
		/// <param name="referenceType">The related item type. Reference type identifiers are limited to 128 characters.</param>
		/// <param name="referenceName">The related item name. Reference names are limited to 128 characters.</param>
		/// <param name="referenceData">The related item data. Limited to 1024 characters of data.</param>
		/// <remarks>See http://msdn.microsoft.com/en-us/library/aa956648(BTS.10).aspx</remarks>
		public virtual void AddCustomReference(string referenceType, string referenceName, string referenceData)
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			// Add a reference to another activity
			this._eventStream.AddReference(this.ActivityName, this.ActivityId, referenceType, referenceName, referenceData);
		}

		/// <summary>
		/// Add a custom reference to this activity, this enables 'data' to be attached to an activity, such as a message body.
		/// </summary>
		/// <param name="referenceType">The related item type. Reference type identifiers are limited to 128 characters.</param>
		/// <param name="referenceName">The related item name. Reference names are limited to 128 characters.</param>
		/// <param name="referenceData">The related item data. Limited to 1024 characters of data.</param>
		/// <param name="longReferenceData">The related item data containing up to 512 KB of Unicode characters of data.</param>
		/// <remarks>See http://msdn.microsoft.com/en-us/library/aa956648(BTS.10).aspx</remarks>
		public virtual void AddCustomReference(string referenceType, string referenceName, string referenceData, string longReferenceData)
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			// Add a reference to another activity
			this._eventStream.AddReference(ActivityName, this.ActivityId, referenceType, referenceName, referenceData, longReferenceData);
		}

		/// <summary>
		/// Activate continuation for this activity. While in the context that is enabling continuation, this activity can
		/// still be updated and MUST be ended with a call to EndEmailProfilingActivity().
		/// </summary>
		/// <param name="continuationId">Optional continuation id. Defaults to CONT_[activityId]</param>
		public virtual string EnableContinuation(string continuationId = null)
		{
			if (this._eventStream == null) throw new ArgumentNullException("_eventStream", ERR_MISSING_MEDIATOR);
			if (this.ActivityName == null) throw new ArgumentNullException("ActivityName");
			if (this.ActivityId == null) throw new ArgumentNullException("ActivityId");

			if(continuationId == null) continuationId = "CONT_" + this.ActivityId;
			this._eventStream.EnableContinuation(ActivityName, this.ActivityId, continuationId);
			return continuationId;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					if(this._eventStream != null && this._eventStream is IDisposable)
					{
						IDisposable disposableEventStream = this._eventStream as IDisposable;
						disposableEventStream.Dispose();
					}
				}

				// free unmanaged resources (unmanaged objects) and override a finalizer below.

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
