//Copyright (C) 2012 by Lee Millward

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;

using CookComputing.XmlRpc;

using Bugzilla.Proxies.User.Params;
using Bugzilla.Proxies.User.Responses;

namespace Bugzilla.Proxies.User
{
  /// <summary>
  /// User level proxy for logging in and out of Bugzilla.
  /// </summary>
  [XmlRpcProxyAssemblyName("IUserProxy")]
  internal interface IUserProxy : IXmlRpcProxy
  {
    /// <summary>
    /// Logs in the specified user.
    /// </summary>
    /// <param name="loginInfo">Details of the user to log in.</param>
    [XmlRpcMethod("User.login")]
    UserIDResponse Login(LoginParam loginInfo);

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    [XmlRpcMethod("User.logout")]
    void Logout();

    /// <summary>
    /// Searches for users based on a set of search criteria.
    /// </summary>
    /// <param name="userParams">User search criteria.</param>
    /// <returns>Details of users which matched the specified criteria.</returns>
    [XmlRpcMethod("User.get")]
    GetUserResponse GetUserDetails(GetUserParams userParams);

    /// <summary>
    /// Sends an email to the specified address offering to create an account.
    /// </summary>
    /// <param name="accountParam"></param>
    [XmlRpcMethod("User.offer_account_by_email")]
    void OfferAccountByEmail(OfferAccountParam accountParam);

    /// <summary>
    /// Creates a new user with the specified details.
    /// </summary>
    /// <param name="newUserParams">New user parameters.</param>
    /// <returns>The ID of the newly created user.</returns>
    [XmlRpcMethod("User.create")]
    UserIDResponse CreateNewUser(CreateUserParams newUserParams);
  }

  /// <summary>
  /// Error codes the remote server can return if an error occurred.
  /// </summary>
  internal enum UserProxyErrorCodes
  {
    BadLoginOrGroupName     = 51,
    InvalidUserNamePassword = 300,
    AccountDisabled         = 301,
    AuthorisationRequired   = 304,
    PasswordExpired         = 305,
    AccountAlreadyExists    = 500,
    IllegalEmailAddress     = 501,
    PasswordTooShort        = 502,
    UserMatchingDenied      = 505,

  }
}