using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerService.Models;

namespace MessengerService.Interfaces
{
    public interface IMessengerService
    {
        #region UserItem
        int AddUserItem(UserItem item);
        bool UpdateUserItem(UserItem item);
        void DeleteUserItem(int userId);
        UserItem GetUserItem(int userId);
        UserItem GetUserItem(string username);
        List<UserItem> GetUserItems();
        #endregion
    }
}
