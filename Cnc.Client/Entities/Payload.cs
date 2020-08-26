using System.ComponentModel.DataAnnotations;

namespace Cnc.Client.Entities
{
    public class Payload
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}