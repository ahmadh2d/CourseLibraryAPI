using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.API.ResourceParameters
{
    public class AuthorResourceParameters
    {
        private const int MaxPageSize = 20;
        public string mainCategory { get; set; }
		public string searchQuery { get; set; }
        public int PageNumber { get; set; }
        private int _PageSize;
        public int PageSize
		{
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}
	}
}
