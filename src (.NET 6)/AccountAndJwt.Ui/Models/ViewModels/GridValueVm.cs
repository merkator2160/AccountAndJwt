using AccountAndJwt.Contracts.Const;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Ui.Models.ViewModels
{
    public class GridValueVm
    {
        public Int32 Id { get; set; }

        [Required]
        public Int32? Value { get; set; }

        [Required]
        [MaxLength(Limits.Value.CommentaryMaxLength)]
        public String Commentary { get; set; }
    }
}