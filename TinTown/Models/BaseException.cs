namespace TinTown.Models
{
    public class BaseException
    {
        public string Exception { get; set; }
        public string ExceptionType { get; set; }
        public int lineNo { get; set; }
        public string fragment { get; set; }
        public string method { get; set; }
    }
}
