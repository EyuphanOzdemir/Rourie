using System;


namespace RourieWebAPI.Models.Shared
{
    public class _NavigateModel
    {
        private double rowCount;
        private int groupCount;

        public string SearchTerm { get; set; }
        public int PageId { get; set; }

        public int GroupCount { get { return groupCount; } }

        public double RowCount
        {
            get => rowCount; set { rowCount = value; groupCount = (int)Math.Ceiling(rowCount / 10); }
        }
    }
}
