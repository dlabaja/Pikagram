using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models;

public class Message
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public Guid SenderId { get; }
    public Guid ReceiverId { get; }
    public DateTime DateSent { get; } = DateTime.UtcNow;
    [MaxLength(250)] 
    public string Text { get; } = string.Empty;
    public virtual User Sender { get; }
    public virtual User Receiver { get; }

    public Message()
    {
    }
}