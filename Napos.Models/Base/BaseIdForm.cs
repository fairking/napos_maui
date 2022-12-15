using System;
using System.Collections.Generic;
using System.Text;

namespace Napos.Models.Base
{
    public abstract class BaseIdForm
    {
        public BaseIdForm() { }

        public BaseIdForm(string id) : this()
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
