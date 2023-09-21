using System.ComponentModel.DataAnnotations;
using erpl.common.dtos.DTOs.Base;

namespace erpl.common.dtos.DTOs.Suspects;

public class SuspectDeletionDto : IDto 
{
    [Required]
    [Editable(true)]
    public string Id { get; set; }
}