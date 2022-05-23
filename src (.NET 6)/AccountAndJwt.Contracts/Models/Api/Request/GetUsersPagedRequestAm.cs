using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class GetUsersPagedRequestAm
    {
        [Range(1, 100, ErrorMessage = "Page size min: 1, max: 100")]
        public Int32 PageSize { get; set; } = 100;

        [Range(1, UInt16.MaxValue, ErrorMessage = "Page number min: 1, max: 65535")]
        public Int32 PageNumber { get; set; } = 1;
    }
}