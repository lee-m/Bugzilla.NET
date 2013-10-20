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

using System.Linq;
using System.Collections.Generic;

using Bugzilla.Proxies.Product.Responses;

namespace Bugzilla
{
  /// <summary>
  /// A single product defined in Bugzilla.
  /// </summary>
  public class Product
  {
    /// <summary>
    /// Details about the product returned from the WS API.
    /// </summary>
    private readonly ProductDets mDets;

    /// <summary>
    /// Creates a new instance using the specified product details.
    /// </summary>
    /// <param name="dets"></param>
    internal Product(ProductDets dets)
    {
      mDets = dets;
    }

    /// <summary>
    /// Accessor for the product ID.
    /// </summary>
    public int Id { get { return mDets.Id; } }

    /// <summary>
    /// Accessor for the product name.
    /// </summary>
    public string Name { get { return mDets.Name; } }

    /// <summary>
    /// Accessor for the product description.
    /// </summary>
    public string Description { get { return mDets.Description; } }

    /// <summary>
    /// Indicates whether this product is active or not.
    /// </summary>
    public bool IsActive { get { return mDets.IsActive; } }

    /// <summary>
    /// Name of the default milestone.
    /// </summary>
    public string DefaultMilestone { get { return mDets.DefaultMilestone; } }

    /// <summary>
    /// Whether this produce has the UNCONFIRMED status.
    /// </summary>
    public bool HasUnconfirmedStatus { get { return mDets.HasUnconfirmedStatus; } }

    /// <summary>
    /// Name of the classification this product belongs to.
    /// </summary>
    public string Classification { get { return mDets.Classification; } }

    /// <summary>
    /// The set of componets available for this product.
    /// </summary>
    public IEnumerable<Component> Components { get { return mDets.Components.Select(dets => new Component(dets)).ToList(); } }

    /// <summary>
    /// The set of versions available for this product.
    /// </summary>
    public IEnumerable<ProductVersion> Versions { get { return mDets.Versions.Select(dets => new ProductVersion(dets)).ToList(); } }

    /// <summary>
    /// The set of milestones available for this product.
    /// </summary>
    public IEnumerable<Milestone> Milestones { get { return mDets.Milestones.Select(dets => new Milestone(dets)).ToList(); } }
  }
}

