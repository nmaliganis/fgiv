using System;
using System.Collections.Generic;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.common.infrastructure.PropertyMappings;
using erpl.model.Wiretaps;

namespace erpl.api.Helpers;

public class PropertyMappingService : BasePropertyMapping
{
  private readonly Dictionary<string, PropertyMappingValue> _wiretapPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      { "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
      { "DateRecorded", new PropertyMappingValue(new List<string>() { "DateRecorded" }) },
      { "OfficerName", new PropertyMappingValue(new List<string>() { "OfficerName" }) },
      { "SuspectNames", new PropertyMappingValue(new List<string>() { "SuspectNames" }) },
      { "Duration", new PropertyMappingValue(new List<string>() { "Duration" }) },
      { "Transcription", new PropertyMappingValue(new List<string>() { "Transcription" }) },
      { "Filename", new PropertyMappingValue(new List<string>() { "Filename" }) },
      { "Filesize", new PropertyMappingValue(new List<string>() { "Filesize" }) },
      { "File", new PropertyMappingValue(new List<string>() { "File" }) },
      { "Status", new PropertyMappingValue(new List<string>() { "Status" }) },
    };

  private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

  public PropertyMappingService() : base(PropertyMappings)
  {
    PropertyMappings.Add(new PropertyMapping<WiretapDto, Wiretap>(_wiretapPropertyMapping));
  }
}