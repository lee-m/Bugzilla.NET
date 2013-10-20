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

using Bugzilla.Proxies.Classification.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Details about a single product within a classification that the logged in user has access to.
  /// </summary>
  public class ClassificationProduct
  {
    /// <summary>
    /// Details about the product.
    /// </summary>
    private readonly ClassificationProductDets mDets;

    /// <summary>
    /// Initialises this instance with the specified details.
    /// </summary>
    /// <param name="details">Details about the product.</param>
    internal ClassificationProduct(ClassificationProductDets details)
    {
      mDets = details;
    }

    /// <summary>
    /// ID of the product.
    /// </summary>
    public int ID { get { return mDets.ID; } }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get { return mDets.Name; } }

    /// <summary>
    /// Description of the product.
    /// </summary>
    public string Description { get { return mDets.Description; } }

  }
}
