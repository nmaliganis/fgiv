using System;
using System.ComponentModel.DataAnnotations;

namespace erpl.common.dtos.ResourceParameters.Wiretaps;

public class UpdateWiretapResourceParameters
{
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
