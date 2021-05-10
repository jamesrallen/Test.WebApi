// Test.WebApi/Test.WebApi/Message.cs
// James Allen
// 2021/05/09/3:29 PM

namespace Test.WebApi.DTOs
{
    public class MessageDto
    {



        public int Id { get; set; }
        public string SubmittedBy { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public bool IsFlagged { get; set; }


    }
}
