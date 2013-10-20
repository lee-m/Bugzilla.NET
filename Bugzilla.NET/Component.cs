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
  /// Information about a single component.
  /// </summary>
  public class Component
  {
    /// <summary>
    /// Details about the component returned from the remote server.
    /// </summary>
    private ComponentDets mDetails;

    /// <summary>
    /// Initialises the instance with the specified component details.
    /// </summary>
    /// <param name="details"></param>
    internal Component(ComponentDets details)
    {
      mDetails = details;
    }

    /// <summary>
    /// ID of the component.
    /// </summary>
    public int Id { get { return mDetails.ID; } }

    /// <summary>
    /// Name of the component.
    /// </summary>
    public string Name { get { return mDetails.Name; } }

    /// <summary>
    /// Description of the component.
    /// </summary>
    public string Description { get { return mDetails.Description; } }

    /// <summary>
    /// The login name of the user to whom new bugs will be assigned by default.
    /// </summary>
    public string DefaultAsignee { get { return mDetails.DefaultAsignee; } }

    /// <summary>
    /// The login name of the user who will be set as the QA Contact for new bugs by default.
    /// </summary>
    public string DefaultQAContact { get { return mDetails.DefaultQAContact; } }

    /// <summary>
    /// Sort order for this component.
    /// </summary>
    public int SortKey { get { return mDetails.SortKey; } }

    /// <summary>
    /// Whether the component is active or not.
    /// </summary>
    public bool IsActive { get { return mDetails.IsActive; } }
  }
}