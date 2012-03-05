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
  /// Stores comment details for comments requested by bug ID and by specific comment IDs.
  /// </summary>
  public class CommentCollection
  {
    /// <summary>
    /// Comments requested on a bug by bug basis, keyed off the bug ID.
    /// </summary>
    private Dictionary<int, List<Comment>> mBugComments;

    /// <summary>
    /// Comments requested specifically by their IDs.
    /// </summary>
    private List<Comment> mComments;

    /// <summary>
    /// Initialises the internal collections.
    /// </summary>
    public CommentCollection()
    {
      mBugComments = new Dictionary<int, List<Comment>>();
      mComments = new List<Comment>();
    }

    /// <summary>
    /// Accessor for the comments keyed off the bug ID
    /// </summary>
    public Dictionary<int, List<Comment>> BugComments { get { return mBugComments; } }

    /// <summary>
    /// Accessor for those comments specifically requested.
    /// </summary>
    public List<Comment> Comments { get { return mComments; } }
  }
}
