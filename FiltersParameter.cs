using CS800_Model_iCorp.CS_QueryFilters;

namespace WAPI_GS;

public class FiltersParameter : QueryStringParameters
{
    public string? Search { get; set; } = null;
    public bool? IsActive { get; set; } = null;

    public FiltersParameter()
    {
    }

    public FiltersParameter(string? search, bool? isActive)
    {
        Search = search;
        IsActive = isActive;
    }
}
