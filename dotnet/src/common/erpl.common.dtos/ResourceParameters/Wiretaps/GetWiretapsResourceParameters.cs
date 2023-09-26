using erpl.common.infrastructure.ResourceParameters;

namespace erpl.common.dtos.ResourceParameters.Wiretaps;

public class GetWiretapsResourceParameters : BaseResourceParameters
{
    /// <summary>
    /// <param name="Filter">Filter in Field
    /// (id, firstname, lastname e.t.c.)</param>
    /// </summary>
    public override string Filter { get; set; }
    /// <summary>
    /// <param name="SearchQuery">Search into Fields
    /// (id, firstname, lastname e.t.c.)</param>
    /// </summary>
    public override string SearchQuery { get; set; }
    /// <summary>
    /// <param name="Fields">Fields to be Shown
    /// (id, firstname, lastname e.t.c.)</param>
    /// </summary>
    public override string Fields { get; set; }
    /// <summary>
    /// <param name="OrderBy">Order by specific field
    /// (id, firstname, lastname e.t.c.)</param>
    /// </summary>
    public override string OrderBy { get; set; } = "Id";
}