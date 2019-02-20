using System;
using System.Collections.Generic;
using System.Text;
using MessengerService.Models;

namespace MessengerService
{
   public interface IEncryptionMessenger
    {
        bool IsAuthenticated { get; }
        void RegisterUser(User userModel);
        void LoginUser(string username, string password);
        void LogoutUser();
        IList<UserItem> Users { get; }
        UserItem CurrentUser { get; }
       // Report GetReport(int? year, int? userId);
       // IList<VendingOperation> GetLog(DateTime? startDate, DateTime? endDate);       
    }
}
