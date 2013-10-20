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

using System;
using System.Linq;
using System.Collections.Generic;

using Bugzilla.Proxies.Classification.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Represents a single classification configured on the remote server.
  /// </summary>
  public class Classification
  {
    /// <summary>
    /// Details about this classification.
    /// </summary>
    private readonly ClassificationDets mDets;

    /// <summary>
    /// Initialises this instance with the specified details
    /// </summary>
    /// <param name="details">Details about this classification.</param>
    internal Classification(ClassificationDets details)
    {
      mDets = details;
    }

    /// <summary>
    /// ID of the classification.
    /// </summary>
    public int ID { get { return mDets.ID; } }

    /// <summary>
    /// Name of the classification.
    /// </summary>
    public string Name { get { return mDets.Name; } }

    /// <summary>
    /// Description of the classification.
    /// </summary>
    public string Description { get { return mDets.Description; } }

    /// <summary>
    /// Sort order of the classification.
    /// </summary>
    public int SortKey { get { return mDets.SortKey; } }

    /// <summary>
    /// Accessor for the available products within this classification.
    /// </summary>
    public IEnumerable<ClassificationProduct> Products 
    { 
      get { return mDets.AssociatedProducts.Select(c => new ClassificationProduct(c)).ToList(); }
    }
  }
}
