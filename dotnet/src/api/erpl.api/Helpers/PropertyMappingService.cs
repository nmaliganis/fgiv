using System;
using System.Collections.Generic;
using erpl.common.dtos.DTOs.Suspects;
using erpl.common.infrastructure.PropertyMappings;
using erpl.model.Suspects;

namespace erpl.api.Helpers;

public class PropertyMappingService : BasePropertyMapping
{
  private readonly Dictionary<string, PropertyMappingValue> _suspectPropertyMapping =
    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
    {
      { "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
      { "Gender", new PropertyMappingValue(new List<string>() { "Gender" }) },
      { "Firstname", new PropertyMappingValue(new List<string>() { "Firstname" }) },
      { "Lastname", new PropertyMappingValue(new List<string>() { "Lastname" }) },
      { "Lastname", new PropertyMappingValue(new List<string>() { "Lastname" }) },
      { "Title", new PropertyMappingValue(new List<string>() { "Title" }) },
      { "Calls", new PropertyMappingValue(new List<string>() { "Calls" }) },
      { "Dob", new PropertyMappingValue(new List<string>() { "Dob" }) },
      { "Nationality", new PropertyMappingValue(new List<string>() { "Nationality" }) },
    };

  private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

  public PropertyMappingService() : base(PropertyMappings)
  {
    PropertyMappings.Add(new PropertyMapping<SuspectDto, Suspect>(_suspectPropertyMapping));
  }
}