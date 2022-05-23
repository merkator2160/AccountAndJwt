﻿using AccountAndJwt.Database.Models.Storage;

namespace AccountAndJwt.Database.Models
{
    public class PagedValueDb
    {
        public Int32 PageCount { get; set; }
        public Int32 TotalItemCount { get; set; }
        public Int32 PageNumber { get; set; }
        public Int32 PageSize { get; set; }
        public Boolean HasPreviousPage { get; set; }
        public Boolean HasNextPage { get; set; }
        public Boolean IsFirstPage { get; set; }
        public Boolean IsLastPage { get; set; }
        public Int32 FirstItemOnPage { get; set; }
        public Int32 LastItemOnPage { get; set; }
        public ValueDb[] Values { get; set; }
    }
}