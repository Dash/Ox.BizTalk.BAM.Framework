using System;

namespace Ox.BizTalk.BAM
{
	/// <summary>
	/// Apply to your activity properties where required
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BamFieldAttribute : Attribute
	{
		/// <summary>
		/// BAM field name if you cannot use the property name
		/// </summary>
		public string FieldName { get; set; }
		/// <summary>
		/// Converts non-nullable types to nulls if they have the default value for saving into the db (useful for DateTime in BizTalk)
		/// </summary>
		public bool NullifyDefaultValues { get; set; }
	}
}
