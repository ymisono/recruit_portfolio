using Newtonsoft.Json;
using System;

namespace ClientTest.Models
{
    public class Memo
    {
        public int Id { get; set; }

        public String OwnerId { get; set; }

        public String Content { get; set; }

        /// <summary>
        /// DBにまだメモがないとき
        /// </summary>
        public bool IsFirstTime 
        {
            get
            {
                return Id == -1;
            }
        }

        public Memo()
        {
            Id = -1; //一回も保存されてない場合、-1
        }

        public static Memo Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<Memo>(json);
        }
    }
}
