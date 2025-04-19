using Newtonsoft.Json;

namespace AuthenticatioApi.Core.Model
{

    /// <summary>
    /// Represents a base data response model.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    public class BaseDataResponseModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataResponseModel{T}"/> class.
        /// </summary>
        public BaseDataResponseModel()
        {
            Data = default;
            TransactionStatus = new StatusResponseModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataResponseModel{T}"/> class with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public BaseDataResponseModel(T data)
        {
            Data = data;
            TransactionStatus = new StatusResponseModel();
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the transaction status.
        /// </summary>
        [JsonProperty("transactionStatus")]
        public StatusResponseModel TransactionStatus { get; set; }
    }



    /// <summary>
    /// Represents a base response list.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    public class BaseResponseList<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseList{T}"/> class.
        /// </summary>
        public BaseResponseList()
        {
            Data = null;
            TransactionStatus = new StatusResponseModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponseList{T}"/> class with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public BaseResponseList(IEnumerable<T> data)
        {
            Data = data;
            TransactionStatus = new StatusResponseModel();
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<T> Data { get; }

        /// <summary>
        /// Gets or sets the transaction status.
        /// </summary>
        [JsonProperty("transactionStatus")]
        public StatusResponseModel TransactionStatus { get; set; }
    }

    /// <summary>
    /// Represents a base response status model.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public record StatusResponseModel
    {
        /// <summary>
        /// Represents the code property of the status response model.
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; } = 200;

        /// <summary>
        /// Represents the message property of the status response model.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = "Ok";

        /// <summary>
        /// Represents the details property of the status response model.
        /// </summary>
        [JsonProperty("details")]
        public string Details { get; set; }

        /// <summary>
        /// Represents the properties of the status response model.
        /// Key is a name of property and value details.
        /// </summary>
        [JsonProperty("properties")]
        public IEnumerable<KeyValuePair<string, List<string>>> Properties { get; set; }
    }
}
