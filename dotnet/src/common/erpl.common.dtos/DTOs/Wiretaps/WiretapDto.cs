using System;
using System.ComponentModel.DataAnnotations;
using erpl.common.dtos.DTOs.Base;

namespace erpl.common.dtos.DTOs.Wiretaps;

public class WiretapDto : IDto
{
    [Key]
        
    [Required]
    public string Id { get; set; }
    
    [Required]
    public DateTime DateRecorded { get; set; }
    [Required]
    public string OfficerName { get; set; }
    public string SuspectNames { get; set; }
    [Required]
    public string Duration { get; set; }
    public string Transcription { get; set; }
    [Required]
    public string Filename { get; set; }
    [Required]
    public string Filesize { get; set; }
    [Required]
    public string File { get; set; }
    [Required]
    public string Status { get; set; }
}