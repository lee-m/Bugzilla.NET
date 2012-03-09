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
  /// Details of an attachment.
  /// </summary>
  public class Attachment
  {
    /// <summary>
    /// Raw data for the attachment.
    /// </summary>
    private byte[] mData;

    /// <summary>
    /// When the attachment was created.
    /// </summary>
    private DateTime mCreationDate;

    /// <summary>
    /// The last time the attachment was modified.
    /// </summary>
    private DateTime mLastChangedDate;

    /// <summary>
    /// The ID of the attachment.
    /// </summary>
    private int mID;
    
    /// <summary>
    /// ID of the bug the attachment belongs to.
    /// </summary>
    private int mBugID;

    /// <summary>
    /// The file name of the attachment.
    /// </summary>
    private string mFileName;

    /// <summary>
    /// A short string describing the attachment.
    /// </summary>
    private string mSummary;

    /// <summary>
    /// The MIME type of the attachment.
    /// </summary>
    private string mMIMEType;

    /// <summary>
    /// True if the attachment is private (only visible to a certain group called the "insidergroup"), False otherwise.
    /// </summary>
    private bool mIsPrivate;

    /// <summary>
    /// True if the attachment is obsolete, False otherwise.
    /// </summary>
    private bool mIsObsolete;

    /// <summary>
    /// True if the attachment is a patch, False otherwise.
    /// </summary>
    private bool mIsPatch;

    /// <summary>
    /// True if the attachment is a URL instead of actual data, False otherwise. 
    /// </summary>
    private bool mIsURL;

    /// <summary>
    /// The login name of the user that created the attachment.
    /// </summary>
    private string mCreator;

    /// <summary>
    /// Creates a new instance based on XML-RPC response data.
    /// </summary>
    /// <param name="responseAttachmentDets">XML-RPC response data.</param>
    internal Attachment(XmlRpcStruct responseAttachmentDets)
    {
      mData = (byte[])responseAttachmentDets["data"];
      mCreationDate = DateTime.Parse(responseAttachmentDets["creation_time"].ToString());
      mLastChangedDate = DateTime.Parse(responseAttachmentDets["last_change_time"].ToString());
      mID = int.Parse(responseAttachmentDets["id"].ToString());
      mBugID = int.Parse(responseAttachmentDets["bug_id"].ToString());
      mFileName = responseAttachmentDets["file_name"].ToString();
      mSummary = responseAttachmentDets["summary"].ToString();
      mMIMEType = responseAttachmentDets["content_type"].ToString();
      mIsPrivate = int.Parse(responseAttachmentDets["is_private"].ToString()) ==  1 ? true : false;
      mIsObsolete = int.Parse(responseAttachmentDets["is_obsolete"].ToString()) ==  1 ? true : false;
      mIsURL = int.Parse(responseAttachmentDets["is_url"].ToString())==  1 ? true : false;
      mIsPatch = int.Parse(responseAttachmentDets["is_patch"].ToString())==  1 ? true : false;
      mCreator = responseAttachmentDets["creator"].ToString();
    }


    /// <summary>
    /// Accessor for the attachment data.
    /// </summary>
    public byte[] AttachmentData { get { return mData; } }

    /// <summary>
    /// Accessor for the creation date.
    /// </summary>
    public DateTime CreationDate { get { return mCreationDate; } }

    /// <summary>
    /// Accessor for the date when the attachment was last changed.
    /// </summary>
    public DateTime LastChangeDate { get { return mLastChangedDate; } }

    /// <summary>
    /// Accessor for the ID of the attachment.
    /// </summary>
    public int ID { get { return mID; } }

    /// <summary>
    /// Accessor for the ID of the bug the attachment belongs to.
    /// </summary>
    public int BugID { get { return mBugID; } }

    /// <summary>
    /// Accessof for the file name of the attachment.
    /// </summary>
    public string FileName { get { return mFileName; } }

    /// <summary>
    /// Accessor for the summary of the attachment.
    /// </summary>
    public string Summary { get { return mSummary; } }

    /// <summary>
    /// Accessor for the MIME type.
    /// </summary>
    public string MIMEType { get { return mMIMEType; } }

    /// <summary>
    /// Accessor for whether the attachment is private or not.
    /// </summary>
    public bool IsPrivate { get { return mIsPrivate; } }

    /// <summary>
    /// Accessor for whether this attachment is obsolete or not.
    /// </summary>
    public bool IsObsolete { get { return mIsObsolete; } }

    /// <summary>
    /// Accessor for whether this attachment is a patch or not.
    /// </summary>
    public bool IsPatch { get { return mIsPatch; } }

    /// <summary>
    /// Accessor for whether this attachment is a URL or not.
    /// </summary>
    public bool IsURL { get { return mIsURL; } }

    /// <summary>
    /// Accessor for the login name of the person who created the attachment.
    /// </summary>
    public string Creator { get { return mCreator; } }
  }
}
