﻿using MovieDb.Api.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieDb.Api.Models
{
	/// <summary>
	/// A model for specifying search criteria.
	/// </summary>
	public class SearchModel
	{
		/// <summary>
		/// A search string for seaching on title using a 'contains' type search. A string with minimum length 3 must be supplied.
		/// </summary>
		[MinLength(3)]
		[MaxLength(100)]		
		[DisplayName("Title contains")]
		public required string TitleContains { get; set; }

		/// <summary>
		/// A search string for searching by actor using a 'contains' type search. If specified, a minimum length 3 must be supplied.
		/// </summary>
		[MinLength(3)]
		[MaxLength(100)]
		[DisplayName("Starring")]
		public string? ActorContains { get; set; }

		/// <summary>
		/// The maximum number of search results to return. Can be between 1 and 100. If unspecified, a value of 100 is applied.
		/// </summary>
		[Range(1, 100)]
		[DisplayName("Limit results to")]
		public int? MaxNumberOfResults { get; set; }

		/// <summary>
		/// The size of each page in the results returned. Can be between 1 and 100.
		/// </summary>
		[Required]
		[Range(1, 100)]
		public int PageSize { get; set; }

		/// <summary>
		/// If the number of search results exceeds the page size, the current page number to view.
		/// </summary>
		[Required]
		[Range(1, 100)]
		public int PageNumber { get; set; }
		
		/// <summary>
		/// The sort order the search results should be returned in. Supported values are "Title" and "ReleaseDate".
		/// </summary>
		[AllowedValues(null, "Title", "ReleaseDate")]
		public string? SortBy { get; set; }

		/// <summary>
		/// If a sort order is specified, whether that sort is descending (true) or ascending (false).
		/// </summary>
		public bool SortDescending { get; set; }

		/// <summary>
		/// An array of genres to filter on in addition to the other criteria. Multiple values are treated as 'any of'.
		/// </summary>
		[StringArrayLength(100)]
		[MaxLength(20)]
		public string[]? Genres { get; set; }
	}
}
