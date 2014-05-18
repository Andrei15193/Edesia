using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Xml.Validation;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	/// <summary>
	/// Represents an abstract XmlDocumentProvider. Such providers are responsible with retrieving and saving
	/// valid XML documents. A source XML document can be considered to be valid however every time a save
	/// is attempted the provided XML document is validated.
	/// </summary>
	/// <remarks>
	/// This is a thread safe class.
	/// </remarks>
	public abstract class XmlDocumentProvider
	{
		public char DirectorySeparator
		{
			get
			{
				return _directorySeparator;
			}
			set
			{
				switch (value)
				{
					case '.':
						throw new ArgumentException("Dot cannot be used as path separator!", "PathSeparator");
					default:
						_directorySeparator = value;
						break;
				}
			}
		}
		public string DirectoryPath
		{
			get
			{
				return _directoryPath;
			}
			set
			{
				if (value == null)
					_directoryPath = string.Empty;
				else
					_directoryPath = value.Trim();
			}
		}

		/// <summary>
		/// Returns a collection XML Schema Exception interpreters.
		/// </summary>
		public ICollection<IXmlSchemaExceptionInterpreter<XmlSchemaException>> XmlSchemaExceptionInterpreters
		{
			get
			{
				return _xmlSchemaExceptionInterpreters;
			}
		}

		/// <summary>
		/// Loads an XML document into an exclusive transaction scope, optionally validating it, having the provided name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocumentName">The name of the XML document to load.</param>
		/// <param name="version">The version of the document to load.</param>
		/// <param name="xmlSchemaSet">The XML schema to use to validate the loaded document.</param>
		/// <returns>Returns a XML document.</returns>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		public abstract IExclusiveXmlTransaction BeginExclusiveTransaction(string xmlDocumentName, DateTime version, XmlSchemaSet xmlSchemaSet = null);
		/// <summary>
		/// Loads an XML document into an exclusive transaction scope, optionally validating it, having the provided name.
		/// This method is thread safe. The version is equal to DateTime.Now (latest version).
		/// </summary>
		/// <param name="xmlDocumentName">The name of the XML document to load.</param>
		/// <param name="xmlSchemaSet">The XML schema to use to validate the loaded document.</param>
		/// <returns>Returns a XML document.</returns>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		public IExclusiveXmlTransaction BeginExclusiveTransaction(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			return BeginExclusiveTransaction(xmlDocumentName, DateTime.Now, xmlSchemaSet);
		}

		/// <summary>
		/// Loads an XML document into a shared transaction scope, optionally validating it, having the provided name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocumentName">The name of the XML document to load.</param>
		/// <param name="version">The version of the document to load.</param>
		/// <param name="xmlSchemaSet">The XML schema to use to validate the loaded document.</param>
		/// <returns>Returns a XML document.</returns>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		public abstract ISharedXmlTransaction BeginSharedTransaction(string xmlDocumentName, DateTime version, XmlSchemaSet xmlSchemaSet = null);
		/// <summary>
		/// Loads an XML document into a shared transaction scope, optionally validating it, having the provided name.
		/// This method is thread safe. The version is equal to DateTime.Now (latest version).
		/// </summary>
		/// <param name="xmlDocumentName">The name of the XML document to load.</param>
		/// <param name="xmlSchemaSet">The XML schema to use to validate the loaded document.</param>
		/// <returns>Returns a XML document.</returns>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		public ISharedXmlTransaction BeginSharedTransaction(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			return BeginSharedTransaction(xmlDocumentName, DateTime.Now, xmlSchemaSet);
		}

		protected string Combine(params string[] paths)
		{
			if (paths == null)
				throw new ArgumentNullException("paths");

			return string.Join(_directorySeparator.ToString(), paths.Select(path => path.Trim(' ', '\t', '\n', '\r', DirectorySeparator)).Where(path => !string.IsNullOrWhiteSpace(path)));
		}
		protected virtual void Validate(XDocument xmlDocument, XmlSchemaSet xmlSchemaSet)
		{
			ICollection<Exception> xmlSchemaExceptions = new LinkedList<Exception>();

			xmlDocument.Validate(xmlSchemaSet, (sender, e) =>
				{
					XmlSchemaException xmlSchemaException = _xmlSchemaExceptionInterpreters.Select(interpreter => interpreter.Interpret(e.Exception))
																						   .FirstOrDefault(exception => exception != null);

					if (xmlSchemaException != null)
						xmlSchemaExceptions.Add(xmlSchemaException);
				});

			if (xmlSchemaExceptions.Any())
				throw new AggregateException(xmlSchemaExceptions);
		}

		private string _directoryPath = string.Empty;
		private char _directorySeparator = Path.DirectorySeparatorChar;
		private readonly ICollection<IXmlSchemaExceptionInterpreter<XmlSchemaException>> _xmlSchemaExceptionInterpreters = new List<IXmlSchemaExceptionInterpreter<XmlSchemaException>>();
	}
}