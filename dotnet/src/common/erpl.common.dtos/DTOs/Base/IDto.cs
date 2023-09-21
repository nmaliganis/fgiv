using System;
using System.ComponentModel.DataAnnotations;

namespace erpl.common.dtos.DTOs.Base;

public interface IDto
{
    [Key]
    string Id { get; set; }
}