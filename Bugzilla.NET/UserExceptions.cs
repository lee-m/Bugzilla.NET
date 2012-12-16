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

namespace Bugzilla
{
  /// <summary>
  /// Thrown when an invalid username or password was specified when attempting to login.
  /// </summary>
  public sealed class InvalidLoginDetailsException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public InvalidLoginDetailsException()
      : base("Invalid username and/or password.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt was made to log into a disabled account..
  /// </summary>
  public sealed class DisabledAccountException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public DisabledAccountException()
      : base("The specified account has been disabled.")
    {
    }
  }

  /// <summary>
  /// Thrown when attempting to login when the user has requested to change their password.
  /// </summary>
  public sealed class ExpiredPasswordException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public ExpiredPasswordException()
      : base("The password for the specified account has expired.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to create a new user account with an email address that is already used.
  /// </summary>
  public sealed class DuplicateAccountException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public DuplicateAccountException()
      : base("An account already exists with the specified email address.")
    {
    }
  }

  /// <summary>
  /// Thrown when attempting to create a new user account when an illegal email address was specified or account
  /// creation has been disabled completely.
  /// </summary>
  public sealed class IllegalEmailAddressException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public IllegalEmailAddressException()
      : base("Illegal email address specified or account creation disabled.")
    {
    }
  }

  /// <summary>
  /// Thrown when attempting to create a new user account when the specified password was too short.
  /// </summary>
  public sealed class PasswordTooShortException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PasswordTooShortException()
      : base("Password too short.")
    {
    }
  }

  /// <summary>
  /// An invalid login or email address was provided when searching for users.
  /// </summary>
  public sealed class InvalidLoginOrGroupNameException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public InvalidLoginOrGroupNameException()
      : base("Invalid login or group name specified.")
    {
    }
  }

  /// <summary>
  /// Thrown when searching for users where one or more of the requested users is not accessible.
  /// </summary>
  public sealed class UserAccessDeniedException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public UserAccessDeniedException()
      : base("One or more of the requested users are not accessible.")
    {
    }
  }
}