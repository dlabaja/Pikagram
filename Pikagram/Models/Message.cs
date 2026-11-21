using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models;

public class Message
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid SenderId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public DateTime DateSent { get; private set; } = DateTime.UtcNow;
    [MaxLength(250)] 
    public string Text { get; private set; } = string.Empty;
    public virtual User Sender { get; private set; }
    public virtual User Receiver { get; private set; }

    public Message()
    {
    }

    public Message(Guid senderId, Guid receiverId, string text)
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Text = text;
    }
}