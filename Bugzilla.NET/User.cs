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

using Bugzilla.Proxies.User.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Details of a user.
  /// </summary>
  public class User
  {
    /// <summary>
    /// User details returned from the Web Service API.
    /// </summary>
    private readonly UserDets mDets;

    /// <summary>
    /// Create a new instance using the specified user details.
    /// </summary>
    /// <param name="dets">User details for this instance.</param>
    internal User(UserDets dets)
    {
      mDets = dets;
    }

    /// <summary>
    /// Accessor for the user ID.
    /// </summary>
    public int Id { get { return mDets.Id; } }

    /// <summary>
    /// Accessor for the user's real name.
    /// </summary>
    public string RealName { get { return mDets.RealName; } }

    /// <summary>
    /// Accessor for the user's email address.
    /// </summary>
    public string Email { get { return mDets.Email; } }

    /// <summary>
    /// Accessor for the user's login name.
    /// </summary>
    public string Name { get { return mDets.Name; } }

    /// <summary>
    /// Accessor for the flag indicating whether the user can log in or not.
    /// </summary>
    public bool CanLogin { get { return mDets.CanLogin; } }

    /// <summary>
    /// Accessor for the email enabled flag.
    /// </summary>
    public bool EmailEnabled { get { return mDets.EmailEnabled; } }

    /// <summary>
    /// Accessor for the login denied text.
    /// </summary>
    public string LoginDeniedText { get { return mDets.LoginDeniedText; } }
  }
}
