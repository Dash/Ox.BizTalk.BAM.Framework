using System;

namespace Ox.BizTalk.BAM
{
	/// <summary>
	/// Attributes for your Activity class
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class BamActivityAttribute : Attribute
	{
		/// <summary>
		/// Name of the BAM activity if you cannot use the class name
		/// </summary>
		public string Name { get; set; }
	}
}
