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
using System.Collections.Generic;

using Bugzilla;

namespace Bugzilla.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      BugzillaServer server = new BugzillaServer("localhost", string.Empty);

      //Log in a user
      server.Login("user@example.com", "password", false);
      Console.WriteLine("Logged in.");

      //Create a new user
      server.CreateNewUser("example@example.com", "password", "Example");
      Console.WriteLine("Created new user.");

      //Search for user by login name
      List<User> matches = server.SearchUsers(null, null, null, new string[] { "user@example.com" }, null, true);
      Console.WriteLine("Found {0} users with login name 'user@example.com'", matches.Count);

      //search by user name
      matches = server.SearchUsers(null, null, null, null, new string[] { "user" }, true);
      Console.WriteLine("Found {0} users via user match string 'user'", matches.Count);

      //Offer account
      server.OfferNewUserAccount("someuser@example.com");

      //Log out
      server.Logout();
      Console.WriteLine("Logged out.");
      Console.Read();
    }
  }
}
