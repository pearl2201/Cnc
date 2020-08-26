using System.ComponentModel.DataAnnotations;

namespace Cnc.Api.Entities
{
    public class Payload
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string ClassName { get; set; }

        public string MethodName { get; set; }
    }
}