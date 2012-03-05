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

using CookComputing.XmlRpc;

namespace Bugzilla.Proxies.User.Responses
{
  /// <summary>
  /// Response value from User.get
  /// </summary>
  internal struct GetUserResponse
  {
    /// <summary>
    /// Details of all users which matched the search criteria.
    /// </summary>
    [XmlRpcMember("users")]
    public UserDets[] Users;
  }

  /// <summary>
  /// Details of a single user.
  /// </summary>
  internal struct UserDets
  {
    /// <summary>
    /// ID of the user.
    /// </summary>
    [XmlRpcMember("id")]
    public int Id;

    /// <summary>
    /// The actual name of the user.
    /// </summary>
    [XmlRpcMember("real_name")]
    public string RealName;

    /// <summary>
    /// The email address of the user.
    /// </summary>
    [XmlRpcMember("email")]
    public string Email;

    /// <summary>
    /// The email address of the user.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// A boolean value to indicate if the user can login into Bugzilla.
    /// </summary>
    [XmlRpcMember("can_login")]
    public bool CanLogin;

    /// <summary>
    /// A boolean value to indicate if bug-related mail will be sent to the user or not.
    /// </summary>
    [XmlRpcMember("email_enabled")]
    public bool EmailEnabled;

    /// <summary>
    /// A text field that holds the reason for disabling a user from logging into bugzilla, 
    /// if empty then the user account is enabled. Otherwise it is disabled/closed.
    /// </summary>
    [XmlRpcMember("login_denied_text")]
    public string LoginDeniedText;
  }
}
