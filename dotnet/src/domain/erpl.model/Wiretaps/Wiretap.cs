using System;
using erpl.common.infrastructure;

namespace erpl.model.Wiretaps;

public class Wiretap : EntityBase<string>
{
    public Wiretap()
    {
        OnCreate();
    }

    private void OnCreate()
    {
        this.Id = Guid.NewGuid().ToString();
    }
    
    public DateTime DateRecorded { get; set; }
    public string OfficerName { get; set; }
    public string SuspectNames { get; set; }
    public string Duration { get; set; }
    public string Transcription { get; set; }
    public string Filename { get; set; }
    public string Filesize { get; set; }
    public string File { get; set; }
    
    public StatusType Status { get; set; }
    
    protected override void Validate()
    {
        
    }
    public void InjectWithModificationValues(DateTime dateRecorded, string officerName, string suspectNames, string duration, string transcription, string filename, string filesize, string file, string status)
    {
        this.DateRecorded = dateRecorded;
        this.OfficerName = officerName;
        this.SuspectNames = suspectNames;
        this.Duration = duration;
        this.Transcription = transcription;
        this.Filename = filename;
        this.Filesize = filesize;
        this.File = file;
        this.Status = (StatusType)Enum.Parse(typeof(StatusType), status, true);
    }
    
    public void InjectWithCreationValues(DateTime dateRecorded, string officerName, string suspectNames, string duration, string filename, string filesize, string file, string status)
    {
        this.DateRecorded = dateRecorded;
        this.OfficerName = officerName;
        this.SuspectNames = suspectNames;
        this.Duration = duration;
        this.Filename = filename;
        this.Filesize = filesize;
        this.File = file;
        this.Status = (StatusType)Enum.Parse(typeof(StatusType), status, true);
    }
}