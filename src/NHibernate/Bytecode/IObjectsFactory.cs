using System;

namespace NHibernate.Bytecode
{
	/// <summary>
	/// Interface for instantiating NHibernate dependencies.
	/// </summary>
	public interface IObjectsFactory
	{
		/// <summary>
		/// Creates an instance of the specified type.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns>A reference to the created object.</returns>
		object CreateInstance(System.Type type);

		/// <summary>
		/// Creates an instance of the specified type.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="nonPublic">true if a public or nonpublic default constructor can match; false if only a public default constructor can match.</param>
		/// <returns>A reference to the created object.</returns>
		// Since v5.2
		[Obsolete("This method has no more usages and will be removed in a future version")]
		object CreateInstance(System.Type type, bool nonPublic);

		/// <summary>
		/// Creates an instance of the specified type using the constructor 
		/// that best matches the specified parameters.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="ctorArgs">An array of constructor arguments.</param>
		/// <returns>A reference to the created object.</returns>
		// Since v5.2
		[Obsolete("This method has no more usages and will be removed in a future version")]
		object CreateInstance(System.Type type, params object[] ctorArgs);
	}
}
