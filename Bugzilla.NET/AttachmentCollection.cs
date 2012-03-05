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

namespace Bugzilla
{
  /// <summary>
  /// Stores comment details for attachments requested by bug ID and by specific comment IDs.
  /// </summary>
  public class AttachmentCollection
  {
    /// <summary>
    /// Attachments requested on a bug by bug basis, keyed off the bug ID.
    /// </summary>
    private Dictionary<int, List<Attachment>> mBugAttachments;

    /// <summary>
    /// Attachments requested specifically by their IDs.
    /// </summary>
    private List<Attachment> mAttachment;

    /// <summary>
    /// Initialises the internal collections.
    /// </summary>
    public AttachmentCollection()
    {
      mBugAttachments = new Dictionary<int, List<Attachment>>();
      mAttachment = new List<Attachment>();
    }

    /// <summary>
    /// Accessor for the attachments keyed off the bug ID
    /// </summary>
    public Dictionary<int, List<Attachment>> BugAttachments { get { return mBugAttachments; } }

    /// <summary>
    /// Accessor for those attachments specifically requested.
    /// </summary>
    public List<Attachment> Attachments { get { return mAttachment; } }
  }
}
