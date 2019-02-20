using System;
using System.Data.SqlClient;
using MessengerService.Exceptions;
using System.Collections.Generic;
using MessengerService.Interfaces;
using MessengerService.Models;

namespace MessengerService
{
    public class EncryptionMessenger : IEncryptionMessenger
    {
        /// <summary>
        /// The current logged in user of the vending machine
        /// </summary>
        public UserItem CurrentUser
        {
            get
            {
                return _roleMgr.User;
            }
        }
        /// <summary>
        /// The data access layer for the encryption messenger
        /// </summary>
        private IMessengerService _db = null;

        /// <summary>
        /// Manages the user authentication and authorization
        /// </summary>
        private RoleManager _roleMgr = null;

        /// <summary>
        /// Constructor that requires the interface for the database
        /// </summary>
        /// <param name="db"></param>
        /// <param name="log"></param>
        public EncryptionMessenger(IMessengerService db)
        {
            _db = db;
            _roleMgr = new RoleManager(null);
        }

        /// <summary>
        /// Returns true if the vending machine has an authenticated user
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _roleMgr.User != null;
            }
        }

        /// <summary>
        /// Adds a new user to the encryption messenger system
        /// </summary>
        /// <param name="userModel">Model that contains all the user information</param>
        public void RegisterUser(User userModel)
        {
            UserItem userItem = null;
            try
            {
                userItem = _db.GetUserItem(userModel.Username);
            }
            catch (Exception)
            {
            }

            if (userItem != null)
            {
                throw new UserExistsException("The username is already taken.");
            }

            if (userModel.Password != userModel.ConfirmPassword)
            {
                throw new PasswordMatchException("The password and confirm password do not match.");
            }

            PasswordManager passHelper = new PasswordManager(userModel.Password);
            UserItem newUser = new UserItem()
            {              
                Username = userModel.Username,
                Salt = passHelper.Salt,
                Hash = passHelper.Hash,
            };

            _db.AddUserItem(newUser);
            LoginUser(newUser.Username, userModel.Password);
        }
        /// <summary>
        /// Logs a user into the vending machine system and throws exceptions on any failures
        /// </summary>
        /// <param name="username">The username of the user to authenicate</param>
        /// <param name="password">The password of the user to authenicate</param>
        public void LoginUser(string username, string password)
        {
            UserItem user = null;

            try
            {
                 user = _db.GetUserItem(username);
                
            }
            catch (Exception)
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            PasswordManager passHelper = new PasswordManager(password, user.Salt);
            if (!passHelper.Verify(user.Hash))
            {
                throw new Exception("Either the username or the password is invalid.");
            }
            _roleMgr = new RoleManager(user);
        }
        /// <summary>
        /// Logs the current user out of the vending machine system
        /// </summary>
        public void LogoutUser()
        {
            _roleMgr = new RoleManager(null);
        }

        /// <summary>
        /// List of all the registered vending machine users
        /// </summary>
        public IList<UserItem> Users
        {
            get
            {
                return _db.GetUserItems();
            }
        }
       
    }
}
