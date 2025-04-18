﻿namespace CS800_Model_iCorp.CS_QueryFilters;

public class QueryStringParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }

        set
        {
            _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}
