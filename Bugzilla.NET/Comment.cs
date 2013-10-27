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
    /// Whether this comment is private or publicly visible.
    /// </summary>
    private CommentVisibility mVisibilityStatus;

    /// <summary>
    /// The position of this comment relative to other comments on the bug.
    /// </summary>
    private int mCommentPosition;

    /// <summary>
    /// Public/private visibility of a comment.
    /// </summary>
    public enum CommentVisibility
    {
      /// <summary>
      /// The comment is public.
      /// </summary>
      Public = 0,

      /// <summary>
      /// The comment is private.
      /// </summary>
      Private
    }

    /// <summary>
    /// Creates a new instance based on XML-RPC response data.
    /// </summary>
    /// <param name="responseCommentDets">XML-RPC response data.</param>
    internal Comment(XmlRpcStruct responseCommentDets)
    {
      mID = int.Parse(responseCommentDets["id"].ToString());
      mBugID = int.Parse(responseCommentDets["bug_id"].ToString());
      mCommentText = responseCommentDets["text"].ToString();
      mAuthor = responseCommentDets["creator"].ToString();
      mCreatedDate = DateTime.Parse(responseCommentDets["creation_time"].ToString());
      mCommentPosition = int.Parse(responseCommentDets["count"].ToString());

      if (Boolean.Parse(responseCommentDets["is_private"].ToString()))
        mVisibilityStatus = CommentVisibility.Private;
      else
        mVisibilityStatus = CommentVisibility.Public;

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
    public CommentVisibility Visibility { get { return mVisibilityStatus; } }

    /// <summary>
    /// The position of this comment relative to other comments on the bug.
    /// </summary>
    /// <remarks>Descriptions are position 0 with actual comments starting at position 1.</remarks>
    public int Position { get { return mCommentPosition; } }
  }
}
