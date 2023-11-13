using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.Entities
{
    public class Visitor : IEntityBase
    {
        public Visitor()
        {

        }
        public Visitor(string ipAddress, string userAgent)
        {
            this.IpAddress = ipAddress;
            this.UserAgent = userAgent;
        }

        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }

        public ICollection<ArticleVisitor> ArticleVisitor { get; set; }
    }
}