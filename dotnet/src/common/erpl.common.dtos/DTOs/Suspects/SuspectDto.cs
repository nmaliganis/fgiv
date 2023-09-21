using System;
using System.ComponentModel.DataAnnotations;
using erpl.common.dtos.DTOs.Base;
using erpl.common.dtos.DTOs.Locations;

namespace erpl.common.dtos.DTOs.Suspects;

public class SuspectDto : IDto
{
    [Key]
    public string Id { get; set; }
    
    public string Gender { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Title { get; set; }
    public int Calls { get; set; }
    public DateTime Dob { get; set; }
    public string Nationality { get; set; }
    
    public LocationDto Location { get; set; }
}