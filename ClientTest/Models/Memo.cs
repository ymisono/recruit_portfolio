using Newtonsoft.Json;
using System;

namespace ClientTest.Models
{
    public class Memo
    {
        public int Id { get; set; }

        public String OwnerId { get; set; }

        public String Content { get; set; }

    }
}
