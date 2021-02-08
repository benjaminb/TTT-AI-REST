using System;

namespace ExecuteMove.DataTransferObjects
{
    /// <summary>
    /// Defines the message payload
    /// </summary>
    public class MessagePayload
    {
        /// <summary>
        /// The message content
        /// </summary>
        /// <value>
        /// The content of the message.
        /// </value>
        public string MessageContent { get; set; }

        /// <summary>
        /// The message date and time
        /// </summary>
        /// <value>
        /// The message date and time.
        /// </value>
        public DateTime MessageDateTime { get; set; }
    }
}
