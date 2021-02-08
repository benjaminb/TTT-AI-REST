using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExecuteMove.DataTransferObjects
{
    public class OutputPayload
    {
        /// <summary>
        /// Example renaming a property using JsonProperyName
        /// </summary>
        //[JsonPropertyName("theAnswer")]
        public int? move { get; set; }
        public string azurePlayerSymbol { get; set; }
        public string humanPlayerSymbol { get; set; }
        public string winner { get; set; }
        public List<string> winPositions { get; set; }
        public List<string> gameBoard { get; set; }
        public string message { get; set; }
    }
}

