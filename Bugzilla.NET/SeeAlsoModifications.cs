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

using CookComputing.XmlRpc;

namespace Bugzilla
{
  /// <summary>
  /// Contains the set of changes that were actually performed when the see also field on a bug was updated.
  /// </summary>
  public class SeeAlsoModifications
  {
    /// <summary>
    /// ID of the bug these changes are for.
    /// </summary>
    private readonly int mBugID;

    /// <summary>
    /// List of URLs which were added.
    /// </summary>
    private readonly string[] mAddedURLs;

    /// <summary>
    /// List of URLs which were removed.
    /// </summary>
    private readonly string[] mRemovedURLs;

    /// <summary>
    /// Creates a new instance initialised to the specified field values.
    /// </summary>
    /// <param name="bugID">ID of the bug these changes are for.</param>
    /// <param name="addedURLs"> List of URLs which were added.</param>
    /// <param name="removedURLs">List of URLs which were removed.</param>
    internal SeeAlsoModifications(int bugID, string[] addedURLs, string[] removedURLs)
    {
      mBugID = bugID;
      mAddedURLs = addedURLs;
      mRemovedURLs = removedURLs;
    }

    /// <summary>
    /// Accessor for the bug ID.
    /// </summary>
    public int BugID { get { return mBugID; } }

    /// <summary>
    /// Accessor for the set of added URLs.
    /// </summary>
    public string[] AddedURLs { get { return mAddedURLs; } }

    /// <summary>
    /// Accessor for the set of removed URLs.
    /// </summary>
    public string[] RemovedURLs { get { return mRemovedURLs; } }
  }
}
