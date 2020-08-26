using System;
using System.ComponentModel.DataAnnotations;

namespace Cnc.Api.Entities
{
    public class ClientInfo
    {
        [Key]
        public int Id { get; set; }

        public string Ip { get; set; }

        public string Username { get; set; }

        public string OperationSystem { get; set; }

        public string ComputerName { get; set; }

        public string MacAddress { get; set; }

        public DateTime LastOnlineAt { get; set; }
    }
}