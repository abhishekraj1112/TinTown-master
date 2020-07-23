namespace TinTown.Models
{
    public class SendResponse
    {
        public string ConnectionID { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
    }

    public class RequestAction
    {
        public int Action { get; set; }
        public string Message { get; set; }
    }

    public class ConnectionList
    {
        public string Email { get; set; }
        public string ConnectionID { get; set; }
    }
}
