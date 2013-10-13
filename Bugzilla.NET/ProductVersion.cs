//Copyright (C) 2013 by Lee Millward

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

using Bugzilla.Proxies.Product.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Details about a product version
  /// </summary>
  public class ProductVersion
  {
    /// <summary>
    /// Details about this version returned from the remote server.
    /// </summary>
    VersionDets mDetails;

    /// <summary>
    /// Initialises this instance with the specified version info from the remote server.
    /// </summary>
    /// <param name="details">Details about this version returned from the server.</param>
    internal ProductVersion(VersionDets details)
    {
      mDetails = details;
    }

    /// <summary>
    /// Name of the milestone.
    /// </summary>
    public string Name { get { return mDetails.Name; } }

    /// <summary>
    /// Sort order for the milestone.
    /// </summary>
    public int SortKey { get { return mDetails.SortKey; } }

    /// <summary>
    /// Whether the milestone is active or not.
    /// </summary>
    public bool IsActive { get { return mDetails.IsActive; } }
  }
}
