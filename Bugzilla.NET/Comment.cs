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

namespace Bugzilla
{
  /// <summary>
  /// Details of a single comment against a bug.
  /// </summary>
  public class Comment
  {
    /// <summary>
    /// ID of the comment.
    /// </summary>
    private int mID;

    /// <summary>
    /// ID of the bug this comment belongs to.
    /// </summary>
    private int mBugID;

    /// <summary>
    /// If the comment was made on an attachment, this will be the ID of that attachment, otherwise null.
    /// </summary>
    private int? mAttachmentID;

    /// <summary>
    /// The actual text of the comment.
    /// </summary>
    private string mCommentText;

    /// <summary>
    /// The login name of the comment's author.
    /// </summary>
    private string mAuthor;

    /// <summary>
    /// The time (in Bugzilla's timezone) that the comment was added.
    /// </summary>
    private DateTime mCreatedDate;

    /// <summary>
    /// True if this comment is private (only visible to a certain group called the "insidergroup"), False otherwise.
    /// </summary>
    private bool mIsPrivate;

    /// <summary>
    /// Creates a new instance based on XML-RPC response data.
    /// </summary>
    /// <param name="responseCommentDets">XML-RPC response data.</param>
    internal Comment(XmlRpcStruct responseCommentDets)
    {
      mID = int.Parse(responseCommentDets["id"].ToString());
      mBugID = int.Parse(responseCommentDets["bug_id"].ToString());
      mCommentText = responseCommentDets["text"].ToString();
      mAuthor = responseCommentDets["author"].ToString();
      mCreatedDate = DateTime.Parse(responseCommentDets["time"].ToString());
      mIsPrivate = Boolean.Parse(responseCommentDets["is_private"].ToString());

      //Attachment ID may or may not be present
      if (responseCommentDets.ContainsKey("attachment_id"))
        mAttachmentID = int.Parse(responseCommentDets["attachment_id"].ToString());
    }

    /// <summary>
    /// Accessor for the comment ID.
    /// </summary>
    public int ID { get { return mID; } }
    
    /// <summary>
    /// Accessor for the ID of the bug this comment belongs to.
    /// </summary>
    public int BugID { get { return mBugID; } }

    /// <summary>
    /// ID of the attachment added when this comment was made.
    /// </summary>
    public int? AttachmentID { get { return mAttachmentID; } }

    /// <summary>
    /// Text of the comment.
    /// </summary>
    public string CommentText { get { return mCommentText; } }

    /// <summary>
    /// Login name of the person who wrote the comment.
    /// </summary>
    public string CommentAuthor { get { return mAuthor; } }

    /// <summary>
    /// Date/time (in the server's timezone) when the bug was created.
    /// </summary>
    public DateTime CreatedDate { get { return mCreatedDate; } }

    /// <summary>
    /// Accessor for whether the comment is private or not.
    /// </summary>
    public bool IsPrivate { get { return mIsPrivate; } }
  }
}
