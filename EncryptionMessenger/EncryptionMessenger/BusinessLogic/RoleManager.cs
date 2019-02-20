using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerService.Models;

namespace MessengerService
{
    /// <summary>
    /// Holds a user and manages their permissions
    /// </summary>
    public class RoleManager
    {
        /// <summary>
        /// The available user roles
        /// </summary>
        public enum eRole
        {
            Unknown = 0,
            Administrator = 1,
            Customer = 2,
            Executive = 3,
            Serviceman = 4
        }

        /// <summary>
        /// The user to manage permissions for
        /// </summary>
        public UserItem User { get; }

        /// <summary>
        /// The name of the user's role
        /// </summary>
        public eRole RoleName { get; }

        /// <summary>
        /// Constructor for the role manager. Create this everytime a user changes.
        /// </summary>
        /// <param name="user">The user to get the permissions for</param>
        public RoleManager(UserItem user)
        {
            User = user;

            if (user != null)
            {
                RoleName = eRole.Administrator;
            }
            else
            {
                RoleName = eRole.Unknown;
            }
        }

        /// <summary>
        /// Specifies if the user has administrator permissions
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                return RoleName == eRole.Administrator;
            }
        }

        /// <summary>
        /// Specifies if the user has customer permissions
        /// </summary>
        public bool IsCustomer
        {
            get
            {
                return RoleName == eRole.Customer;
            }
        }

        /// <summary>
        /// Specifies if the user has executive permissions
        /// </summary>
        public bool IsExecutive
        {
            get
            {
                return RoleName == eRole.Executive;
            }
        }

        /// <summary>
        /// Specifies if the user has serviceman permissions
        /// </summary>
        public bool IsServiceman
        {
            get
            {
                return RoleName == eRole.Serviceman;
            }
        }

        /// <summary>
        /// Specifies if the user has unknown permissions
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return RoleName == eRole.Unknown;
            }
        }
    }
}
