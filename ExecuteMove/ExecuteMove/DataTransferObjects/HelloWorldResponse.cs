using System;

namespace ExecuteMove.DataTransferObjects
{
    /// <summary>
    /// Defines the message payload
    /// </summary>
    public class HelloWorldResponse
    {
        /// <summary>
        /// Contains message that was computed on the server
        /// </summary>
        /// <value>
        /// The resultant message computed by the server
        /// </value>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Contains the date and time the message was computed by the server
        /// </summary>
        /// <value>
        /// The date and time the message was computed by the server
        /// </value>
        public DateTime ProcessedDateTime { get; set; }
    }
}
