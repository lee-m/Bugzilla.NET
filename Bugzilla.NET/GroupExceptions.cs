﻿//Copyright (C) 2013 by Lee Millward

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
  /// Thrown when an attempt is made to create a group with a name that already exists
  /// </summary>
  public sealed class DuplicateGroupNameException : BugzillaException
  {
    /// <summary>
    /// Initialises a new instance with the specified message.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public DuplicateGroupNameException(string message)
      : base(message) { }
  }

  /// <summary>
  /// Thrown when an attempt is made to create a group with an invalid user regular expression.
  /// </summary>
  public sealed class InvalidGroupRegExpException : BugzillaException
  {
    /// <summary>
    /// Initialises a new instance with the specified message.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidGroupRegExpException(string message)
      : base(message) { }
  }
}
