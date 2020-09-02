using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string HeadLine { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public virtual Book Books { get; set; }
        public virtual Reviewer Reviewers { get; set; }
    }
}
