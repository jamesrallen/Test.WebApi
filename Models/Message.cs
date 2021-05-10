// Test.WebApi/Test.WebApi/Message.cs
// James Allen
// 2021/05/09/4:08 PM

using System;
using Test.WebApi.DTOs;

namespace Test.WebApi.Models
{
    public class Message
    {



        public Message()
        {
            
        }

        public Message(MessageDto messageDto)
        {
            Topic = messageDto.Topic;
            Body = messageDto.Body;
            SubmittedBy = messageDto.SubmittedBy;
            IsFlagged = messageDto.IsFlagged;
        }


        public int Id { get; set; }
        public string SubmittedBy { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public bool IsFlagged { get; set; }
        public DateTime DateTime { get; set; }


        public void UpdateFrom(MessageDto messageDto)
        {
            Topic = messageDto.Topic;
            Body = messageDto.Body;
            SubmittedBy = messageDto.SubmittedBy;
            IsFlagged = messageDto.IsFlagged;
        }


    }
}
