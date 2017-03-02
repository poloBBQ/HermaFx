﻿using System;
using System.Text;
using System.Linq;

namespace HermaFx.Text
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class EncodingResolverAttribute : Attribute, IEncodingResolver
	{
		private readonly Encoding[] _encodings;

		#region .ctor
		public	EncodingResolverAttribute()
		{

		}

		public EncodingResolverAttribute(params Type[] encodings)
			: this()
		{
			Guard.IsNotNull(encodings, nameof(encodings));
			Guard.Against<ArgumentOutOfRangeException>(
				encodings.Any(x => !typeof(Encoding).IsAssignableFrom(x)),
				"Some passed objects do not implement {0}", typeof(Encoding).FullName
			);

			_encodings = encodings.Select(x => (Encoding)Activator.CreateInstance(x)).ToArray();
		}
		#endregion

		public Encoding GetEncoding(string name)
		{
			Guard.IsNotNullNorWhitespace(name, nameof(name));

			// FIXME: Throw a more meaningfull exception if encoding is not available.
			var result = _encodings.SingleOrDefault(x => x.BodyName.ToLowerInvariant() == name.ToLowerInvariant());
			if (result == null)
			{
				throw new ArgumentOutOfRangeException("Unknown encoding: {name}");
			}

			return result;
		}
	}
}
