namespace MedRoute.Models.System
{
    public class StatusMessage
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
    public struct Message
    {
        public const string ID_NOT_FOUND = "ID was not found";
        public const string INPUT_EMPTY = "Input was empty";
        public const string LIST_EMPTY = "List was empty";
        public const string ID_NOT_MATCH = "ID and EntityID was not matched";
        public const string GET_SUCCESS = "Retrieve data successfully";
        public const string UPDATE_SUCCESS = "Update data successfully";
        public const string ADD_SUCCESS = "Create data successfully";
        public const string DELETE_SUCCESS = "Delete data successfully";
        public const string METHOD_NOT_DEFINED = "Method was not defined";
        public const string UNKNOW_ERROR_PREFIX = "Error was not defined: ";
    }
}
