using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramWeb.Application.User.Queries.GetUsers;

namespace InstagramWeb.Application.Common.Interfaces;
public interface IUserServices
{
    Task<List<UserDto>> getUsersInfo();
}
