using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Core.Model
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public MessageType MessageType { get; set; }
        public DeleteState DeleteState { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
