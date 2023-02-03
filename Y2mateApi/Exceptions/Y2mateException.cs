using System;

namespace Y2mateApi.Exceptions;

/// <summary>
/// Exception thrown within <see cref="Y2mateException"/>.
/// </summary>
public class Y2mateException : Exception
{
    /// <summary>
    /// Initializes an instance of <see cref="Y2mateException"/>.
    /// </summary>
    /// <param name="message"></param>
    public Y2mateException(string message) : base(message)
    {
    }
}