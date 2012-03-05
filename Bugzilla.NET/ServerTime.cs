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
using Bugzilla.Proxies.Bugzilla.Responses;

namespace Bugzilla
{
  /// <summary>
  /// Time information about the Bugzilla server.
  /// </summary>
  public class ServerTime
  {
    /// <summary>
    /// Time details returned from the server.
    /// </summary>
    private readonly GetTimeResponse mTimeDets;

    /// <summary>
    /// Creates a new instance of this class wrapped around the get time response struct
    /// </summary>
    /// <param name="timeDets">Bugzilla server time details.</param>
    internal ServerTime(GetTimeResponse timeDets)
    {
      mTimeDets = timeDets;
    }

    /// <summary>
    /// Access for the Bugzilla server's database server time.
    /// </summary>
    public DateTime DatabaseServerTime { get { return mTimeDets.DatabaseServerTime; } }

    /// <summary>
    /// Accessor for the Bugzilla server's time.
    /// </summary>
    public DateTime WebServerTime { get { return mTimeDets.WebServerTime; } }
  }
}
