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
      BugzillaServer server = new BugzillaServer("localhost", string.Empty, "user@example.com", "password", false);
      Console.WriteLine("Server version: {0}", server.ServerVersion);

      ServerTime serverTime = server.ServerTime;
      Console.WriteLine("Web server time: {0}", serverTime.WebServerTime);
      Console.WriteLine("Database server time: {0}", serverTime.DatabaseServerTime);

      //Show details of all extensions
      Console.WriteLine("Installed extensions:");

      foreach (Extension ext in server.InstalledExtensions)
      {
        Console.WriteLine("\tName: {0} Version {1}", ext.Name, ext.Version);
      }

      //Add an attachment to multiple bugs at the same time
      server.AddAttachmentToBugs(new string[] { "1", "2" }, "C:\\SomeFile.png", "Attachment 1", "Comment text", false, false);

      //Get all comments for bugs 1, 2 and 3 and the specific comments with IDs 1 and 2
      CommentCollection commentsColl = server.GetComments(new string[] { "1", "2", "3" }, new int[] { 1, 2 }, null);

      //Dig out the comments for the individual bugs, the BugComments collection is keyed off the bug ID
      List<Comment> bug1Comments = commentsColl.BugComments[1];
      List<Comment> bug2Comments = commentsColl.BugComments[2];
      List<Comment> bug3Comments = commentsColl.BugComments[3];

      //Comments requested by ID
      List<Comment> commentsByID = commentsColl.Comments;

      //Get all attachments for bugs 1, 2 and 3 and the specific attachment with IDs 1 and 2
      AttachmentCollection attachColl = server.GetAttachments(new string[] { "1", "2", "3" }, new int[] { 1, 2 });

      //Dig out the attachments for the individual bugs, the BugAttachments collection is keyed off the bug ID
      List<Attachment> bug1Attachments = attachColl.BugAttachments[1];
      List<Attachment> bug2Attachments = attachColl.BugAttachments[2];
      List<Attachment> bug3Attachments = attachColl.BugAttachments[3];

      //Attachments requested by ID
      List<Attachment> attachmentsByID = attachColl.Attachments;

      Console.Read();
    }
  }
}