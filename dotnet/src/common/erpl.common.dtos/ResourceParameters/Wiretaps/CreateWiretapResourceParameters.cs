using System;
using System.ComponentModel.DataAnnotations;

namespace erpl.common.dtos.ResourceParameters.Wiretaps;

public class CreateWiretapResourceParameters
{
    [Required]
    public DateTime DateRecorded { get; set; }
    [Required]
    public string OfficerName { get; set; }
    public string SuspectNames { get; set; }
    [Required]
    public string Duration { get; set; }
    [Required]
    public string Filename { get; set; }
    [Required]
    public string Filesize { get; set; }
    [Required]
    public string File { get; set; }
    [Required]
    public string Status { get; set; }
}