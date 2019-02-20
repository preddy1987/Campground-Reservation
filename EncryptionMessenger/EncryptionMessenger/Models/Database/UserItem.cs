using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService.Models
{
    public class UserItem : BaseItem
    {
        public string Username { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public UserItem Clone()
        {
            UserItem item = new UserItem();
            item.Id = this.Id;
            item.Username = this.Username;
            item.Hash = this.Hash;
            item.Salt = this.Salt;
            return item;
        }
    }
}
