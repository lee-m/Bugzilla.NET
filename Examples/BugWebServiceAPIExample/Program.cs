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
using System.Linq;
using System.Text;

using Bugzilla;

namespace Bugzilla.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      BugzillaServer server = new BugzillaServer("localhost", string.Empty, "user@example.com", "password", false);
      
      //Get a "empty" bug instance without fetching any of it's data
      Bug bug = server.GetBug(1, BugzillaServer.BugFetchOptions.NoFetch);

      //Get details of all the comments against this bug
      List<Comment> allComments = bug.GetComments(null);

      //Get details of comments made in the last week
      List<Comment> weeksComments = bug.GetComments(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));

      //Add an attach from a file on disk
      bug.AddAttachment("C:\\SomeFile.png", "Attachment 1", "Comment text", false, false);

      //Add a comment
      bug.AddComment("Comment text", false, null);

      //Get all comments for this bug
      List<Comment> comments = bug.GetComments(null);

      //Get all attachments for this bug
      List<Attachment> attachments = bug.GetAttachments();

      //Get details of of a bug from the remote server
      Bug fetchedBugDets = server.GetBug(1, BugzillaServer.BugFetchOptions.FetchData);

      //Add a URL to the see also field for a bug
      bug.UpdateSeeAlsoURLs(new string[] { "http://localhost/show_bug.cgi?id=1" }, null);

      //Add a new see also URL and remove the URL added above
      bug.UpdateSeeAlsoURLs(new string[] { "http://localhost/show_bug.cgi?id=2" },
                            new string[] { "http://localhost/show_bug.cgi?id=1" });

    }
  }
}
