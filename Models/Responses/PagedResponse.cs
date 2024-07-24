﻿namespace DataTypes.Responses
{
    public class PagedResponse<T>
    {
        public int Total { get; set; }
        public T? Items { get; set; }

    }
}